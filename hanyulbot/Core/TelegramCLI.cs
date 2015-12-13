/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: TelegramCLI.cs (Core/TelegramCLI.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : Main controller of telegram-cli process
 * 
 */
using System;
using System.Threading;
using System.Diagnostics;

namespace hanyulbot
{
	public class TelegramCLI
	{
		private const string TAG = "TG_CLI";
		private Process tg_cli = null;
		public TelegramEndPointModel profile { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="hanyulbot.TelegramCLI"/> class.
		/// </summary>
		/// <param name="tg_cli_location">binary location of telegram-cli</param>
		/// <param name="socket_location">socket location</param>
		public TelegramCLI (string tg_cli_location, string socket_location)
		{
			if (tg_cli_location == null) {
				InternalLogger.d (TAG, "Not valid telegram-cli location. telegram-cli will started at default location.");
				tg_cli_location = "../../../tg-bin/telegram-cli";
			}

			if (System.IO.File.Exists (socket_location))
				System.IO.File.Delete (socket_location);

			tg_cli = new Process ();
			tg_cli.StartInfo = new ProcessStartInfo (tg_cli_location, string.Format("-C -R -I -WS {0} --json", socket_location));
			tg_cli.StartInfo.RedirectStandardOutput = true;
			tg_cli.StartInfo.UseShellExecute = false;
			tg_cli.EnableRaisingEvents = true;
			tg_cli.OutputDataReceived += new DataReceivedEventHandler (Poll.LongPollingStdout);

			InternalLogger.d (TAG, "TelegramCLI Instance has been initialized.");
			InternalLogger.d (TAG, string.Format ("telegram-cli : {0}, params : {1}", tg_cli_location, socket_location));
		}

		/// <summary>
		/// Stops the telegram-cli.
		/// </summary>
		public void Stop()
		{
			InternalLogger.d (TAG, "Shutting down telegram-cli");

			try {
				tg_cli.Close ();
				System.IO.File.Delete ("/tmp/telegram.tmp");
			} catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while closing telegram-cli : " + e.Message);
			}
		}

		/// <summary>
		/// Starts telegram-cli process with hanyul's handle
		/// </summary>
		/// <returns><c>true</c>, if telegram-cli was started, <c>false</c> otherwise.</returns>
		public bool Start()
		{
			InternalLogger.d (TAG, String.Format ("Starting telegram-cli with provided settings"));

			try {
				tg_cli.Start ();
				tg_cli.BeginOutputReadLine ();
				InternalLogger.d (TAG, "Successfully started, telegram-cli is running : " + tg_cli.Id);
			}
			catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while starting telegram-cli. : " + e.Message);

				if (tg_cli.Id > 0)
					InternalLogger.e (TAG, "But it seems to be started even it has an error.");
				else
					return false;
			}

			return true;
		}
	}
}

