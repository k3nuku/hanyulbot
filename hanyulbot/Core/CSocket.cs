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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace hanyulbot
{
	public class CSocket
	{
		private static ManualResetEvent sendSwitch = new ManualResetEvent (false);
		private static ManualResetEvent connectSwitch = new ManualResetEvent (false);
		private static ManualResetEvent disconnectSwitch = new ManualResetEvent (false);
		private static string TAG = "CSocket";
		private static Socket tg_csock;
		private static byte[] recv_tmp;

		/// <summary>
		/// Starts the communication with telegram-cli with Unix Domain Socket.
		/// </summary>
		/// <param name="location_unixsocket">Location of unixsocket</param>
		public static void StartSocket(string location_unixsocket)
		{
			string unixsocket = "/tmp/telegram.tmp"; // should be moved to configuration as a file
			EndPoint unixEp = new UnixEndPoint (unixsocket);

			if (File.Exists (unixsocket))
			{
				tg_csock = new Socket (AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
				tg_csock.BeginConnect (unixEp, ConnectCallback, tg_csock);
			}
			else
				InternalLogger.e (TAG, String.Format ("No UnixSocket has been found. (location : {0})", unixsocket));
		}

		/// <summary>
		/// (experimental) Send command to socket.
		/// </summary>
		/// <param name="command">telegram-cli command</param>
		public static bool SendToSocket(string command)
		{
			if (connectSwitch.WaitOne () != true)
			{
				InternalLogger.e (TAG, "Tried to send command before establishing connection.");
				return false;
			}

			byte[] tmp_buf = Misc.ConvertToUTF8Bytes (command);
			sendSwitch.Reset ();
			tg_csock.BeginSend(tmp_buf, 0, tmp_buf.Length, SocketFlags.None, new AsyncCallback(SendCallback), tg_csock);

			return sendSwitch.WaitOne (5000) ? true : false;
		}

		/// <summary>
		/// Disconnect the socket connected to telegram-cli.
		/// </summary>
		/// <param name="force">If set to <c>true</c> then force close socket.</param>
		public static void Disconnect(bool force)
		{
			if (connectSwitch.WaitOne () != true)
			{
				InternalLogger.e (TAG, "Tried to disconnect from telegram-cli before establishing connection.");
				return false;
			}

			if (force)
				tg_csock.Disconnect (false);
			else
			{
				tg_csock.BeginDisconnect (false, new AsyncCallback (DisconnectCallback), tg_csock);
				InternalLogger.d (TAG, "Wait for disconnect from telegram-cli...");

				if (disconnectSwitch.WaitOne (15000)) {
					disconnectSwitch.Reset ();
					return true;
				} else {
					InternalLogger.e (TAG, "Cannot disconnect socket, it already would be disconnected or has an error.");
					disconnectSwitch.Reset ();
					return false;
				}
			}

			return true;
		}

		private static void DisconnectCallback(IAsyncResult iar)
		{
			try
			{
				Socket tmp_csock = (Socket)iar.AsyncState;

				tmp_csock.EndDisconnect(iar);
				InternalLogger.i(TAG, "Succesfully disconnected from telegram-cli.");

				connectSwitch.Reset();
				disconnectSwitch.Set();
			}
			catch (Exception e)
			{
				InternalLogger.e (TAG, "An error has been occured while disconnecting from telegram-cli.");
			}
		}

		/// <summary>
		/// Callback of send
		/// </summary>
		private static void SendCallback(IAsyncResult iar)
		{
			try
			{
				Socket tmp_tgsock = (Socket)iar.AsyncState;

				int writeSize = tmp_tgsock.EndSend(iar);
				InternalLogger.d(TAG, string.Format("Succesfully sent {0} bytes to telegram-cli.", writeSize));

				sendSwitch.Set();
			}
			catch (Exception e)
			{
				InternalLogger.e (TAG, "An error has been occured while sending command to telegram-cli.");
			}
		}

		/// <summary>
		/// Callback of connect
		/// </summary>
		private static void ConnectCallback(IAsyncResult iar)
		{
			try
			{
				Socket tmp_tgsock = (Socket)iar.AsyncState;

				tmp_tgsock.EndConnect (iar);
				InternalLogger.i (TAG, "Successfully Connected to telegram-cli with Unix Socket.");

				recv_tmp = new byte[1024];
				tmp_tgsock.BeginReceive (recv_tmp, 0, 1024, SocketFlags.None, new AsyncCallback (ReceiveCallback), tmp_tgsock);
			}
			catch (Exception e)
			{
				InternalLogger.e (TAG, "An error has been occured while connecting with telegram-cli.");
			}
		}

		/// <summary>
		/// Callback of receive
		/// </summary>
		private static void ReceiveCallback(IAsyncResult iar)
		{
			try
			{
				int readSize = 0;
				Socket tmp_tgsock = (Socket)iar.AsyncState;

				if ((readSize = tmp_tgsock.EndReceive(iar)) > 0)
				{
					string msg = Misc.ConvertToUTF8String(recv_tmp, readSize);
					InternalLogger.d(TAG, string.Format("ReceiveCallback::Received - {0}", msg.Substring(0, 30)));
				}
				else if (readsize < 0)
				{
					InternalLogger.d(TAG, "Readsize has minus value; expecting connection gonna be closed.");
				}

				recv_tmp = new byte[1024];
				tmp_tgsock.BeginReceive(recv, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), tmp_tgsock);
			}
			catch (Exception e)
			{
				InternalLogger.e (TAG, "Connection has been closed; " + e.Message);
				connectSwitch.Reset ();
			}
		}
	}
}

