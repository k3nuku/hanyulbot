/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: CSocket.cs (Core/CSocket.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : A module pipelining telegram-cli with hanyulbot wrapper
 * 
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Mono.Unix;

namespace hanyulbot
{
	public class CSocket : IDisposable
	{
		private ManualResetEvent sendSwitch = new ManualResetEvent (false);
		private ManualResetEvent connectSwitch = new ManualResetEvent (false);
		private ManualResetEvent disconnectSwitch = new ManualResetEvent (false);
		private static CSocket _instance;
		private const string TAG = "Socket";
		private const int bufferSize = 2048;
		private const int disconnectTimeOut = 15000;
		private Socket tg_csock;
		private byte[] recv_buf;
		private byte[] send_buf;
		private EndPoint unixEp;
	
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="location_unixsocket">Location unixsocket.</param>
		public static CSocket GetInstance(string location_unixsocket)
		{
			if (_instance == null)
				_instance = new CSocket (location_unixsocket);

			return _instance;
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static CSocket GetInstance()
		{
			return _instance;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="hanyulbot.CSocket"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="hanyulbot.CSocket"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="hanyulbot.CSocket"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="hanyulbot.CSocket"/> so the garbage
		/// collector can reclaim the memory that the <see cref="hanyulbot.CSocket"/> was occupying.</remarks>
		public void Dispose()
		{
			if (!Disconnect (false))
				Disconnect (true);

			tg_csock = null;
		}

		/// <summary>
		/// Sets the communication with telegram-cli with Unix Domain Socket.
		/// </summary>
		/// <param name="location_unixsocket">Location of unixsocket</param>
		private CSocket(string location_unixsocket)
		{
			string unixsocket = location_unixsocket; // should be moved to configuration as a file
			unixEp = new UnixEndPoint (unixsocket);

			if (System.IO.File.Exists (unixsocket)) {
				tg_csock = new Socket (AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
			} else
				InternalLogger.e (TAG, String.Format ("No UnixSocket has been found. (location : {0})", unixsocket));
		}

		/// <summary>
		/// Starts the communication with telegram-cli with Unix Domain Socket.
		/// </summary>
		public void Connect(bool async=false)
		{
			if (unixEp == null) {
				InternalLogger.e (TAG, "Tried to connect nevertheless EndPoint has not been set");
				return;
			}

			if (async)
				tg_csock.BeginConnect (unixEp, new AsyncCallback (ConnectCallback), tg_csock);
			else {
				tg_csock.Connect (unixEp);

				connectSwitch.Set();
			}
		}

		/// <summary>
		/// (Used for Synchronous Connect) Receive a data from telegram-cli Socket.
		/// </summary>
		public byte[] Receive(bool async=true) {
			if (async) {
				recv_buf = new byte[bufferSize];
				tg_csock.BeginReceive (recv_buf, 0, bufferSize, SocketFlags.None, new AsyncCallback (ReceiveCallback), tg_csock);

				return null;
			} else {
				if (tg_csock.Receive (recv_buf) > 0)
					return recv_buf;
				else
					return null;
			}
		}

		/// <summary>
		/// (experimental) Send command to socket.
		/// </summary>
		/// <param name="command">telegram-cli command</param>
		public bool Send(string command, bool async=false)
		{
			if (connectSwitch.WaitOne (0) != true) {
				InternalLogger.e (TAG, "Tried to send command before establishing connection. " + command);
				return false;
			}

			InternalLogger.d (TAG, "Sending commands : " + command);
			send_buf = Misc.ConvertToUTF8Bytes (command + "\r\n");

			if (async) {
				sendSwitch.Reset ();
				tg_csock.BeginSend (send_buf, 0, send_buf.Length, SocketFlags.None, new AsyncCallback (SendCallback), tg_csock);

				if (sendSwitch.WaitOne (5000)) {
					sendSwitch.Set ();
					return true;
				} else {
					InternalLogger.e (TAG, "Timed out : Cannot send command to socket, it would be already disconnected or has an error.");
					sendSwitch.Set ();
					return false;
				}
			} else {
				int sentBytes = tg_csock.Send (send_buf);
				InternalLogger.d (TAG, string.Format("Successfully sent {0} bytes to socket.", sentBytes));
				return true;
			}
		}

		/// <summary>
		/// Disconnect the socket connected to telegram-cli.
		/// </summary>
		/// <param name="force">If set to <c>true</c> then force close socket.</param>
		public bool Disconnect(bool force)
		{
			if (connectSwitch.WaitOne () != true) {
				InternalLogger.e (TAG, "Tried to disconnect from telegram-cli before establishing connection.");
				return false;
			}

			if (force)
				tg_csock.Disconnect (false);
			else {
				tg_csock.BeginDisconnect (false, new AsyncCallback (DisconnectCallback), tg_csock);
				InternalLogger.d (TAG, "Wait for disconnect from telegram-cli...");

				if (disconnectSwitch.WaitOne (disconnectTimeOut)) {
					disconnectSwitch.Reset ();
					return true;
				} else {
					InternalLogger.e (TAG, "Timed out : Cannot disconnect socket, it already would be disconnected or has an error.");
					disconnectSwitch.Reset ();
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Callback of disconnect
		/// </summary>
		private void DisconnectCallback(IAsyncResult iar)
		{
			try {
				Socket tmp_csock = (Socket)iar.AsyncState;
				tmp_csock.EndDisconnect (iar);
				InternalLogger.i (TAG, "Succesfully disconnected from telegram-cli.");

				connectSwitch.Reset ();
				disconnectSwitch.Set ();
			} catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while disconnecting from telegram-cli. : " + e.Message);
			}
		}

		/// <summary>
		/// Callback of send
		/// </summary>
		private void SendCallback(IAsyncResult iar)
		{
			try {
				Socket tmp_tgsock = (Socket)iar.AsyncState;
				int writeSize = tmp_tgsock.EndSend (iar);
				InternalLogger.d (TAG, string.Format ("Successfully sent {0} bytes to telegram-cli.", writeSize));

				sendSwitch.Set ();
			} catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while sending command to telegram-cli. : " + e.Message);
			}
		}

		/// <summary>
		/// Callback of connect
		/// </summary>
		private void ConnectCallback(IAsyncResult iar)
		{
			try {
				Socket tmp_tgsock = (Socket)iar.AsyncState;

				tmp_tgsock.EndConnect (iar);
				InternalLogger.i (TAG, "Successfully Connected to telegram-cli with Unix Socket.");

				connectSwitch.Set();

				recv_buf = new byte[bufferSize];
				tmp_tgsock.BeginReceive (recv_buf, 0, bufferSize, SocketFlags.None, new AsyncCallback (ReceiveCallback), tmp_tgsock);
			} catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while connecting with telegram-cli. : " + e.Message);
			}
		}

		/// <summary>
		/// Callback of receive
		/// </summary>
		private void ReceiveCallback(IAsyncResult iar)
		{
			try {
				Socket tmp_tgsock = (Socket)iar.AsyncState;
				int readSize = tmp_tgsock.EndReceive (iar);

				if (readSize > 0) {
					InternalLogger.d(TAG, "received bytes from socket : " + readSize + "bytes");
					string msg = Misc.ConvertToUTF8String (recv_buf, readSize);
					Poll.PollingReceive(msg);
				} else if (readSize < 0)
					InternalLogger.d (TAG, "Readsize has minus value; expecting connection gonna be closed.");

				Array.Clear (recv_buf, 0, send_buf.Length);
				tmp_tgsock.BeginReceive (recv_buf, 0, bufferSize, SocketFlags.None, new AsyncCallback (ReceiveCallback), tmp_tgsock);
			} catch (SocketException e) {
				InternalLogger.e (TAG, "Connection has been closed; " + e.Message);
				connectSwitch.Reset ();
			}
		}
	}
}

