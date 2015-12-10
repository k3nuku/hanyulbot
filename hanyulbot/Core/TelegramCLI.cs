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
using System.Diagnostics;

namespace hanyulbot
{
	public class TelegramCLI
	{
		private const string TAG = "TelegramCLI";
		private static TelegramCLI _instance = null;
		private Process tg_cli = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="hanyulbot.TelegramCLI"/> class.
		/// </summary>
		/// <param name="tg_cli_location">binary location of telegram-cli</param>
		/// <param name="parameters">Additional running parameters</param>
		private TelegramCLI (string tg_cli_location, string parameters)
		{
			ProcessStartInfo tgcli_psi = new ProcessStartInfo (tg_cli_location, "--json -S /tmp/telegram.tmp " + parameters);
			tgcli_psi.UseShellExecute = false;

			tg_cli = new Process ();
			tg_cli.StartInfo = tgcli_psi;
			tg_cli.StartInfo.RedirectStandardOutput = true;
			tg_cli.EnableRaisingEvents = true;
			tg_cli.OutputDataReceived += Poll.LongPollingStdout;

			InternalLogger.d (TAG, 
				String.Format ("TelegramCLI Instance has been initialized.\ntelegram-cli : {0}, params : {1}", tg_cli_location, parameters));
		}

		/// <summary>
		/// Starts telegram-cli process with hanyul's handle
		/// </summary>
		/// <returns><c>true</c>, if telegram-cli was started, <c>false</c> otherwise.</returns>
		public bool StartTgCLI()
		{
			InternalLogger.d (TAG, String.Format ("Starting telegram-cli with provided settings"));

			try {
				tg_cli.Start ();
				tg_cli.BeginOutputReadLine ();
				Console.WriteLine ("Successfully started, telegram-cli is running : " + tg_cli.Id);
			}
			catch (Exception e) {
				InternalLogger.e (TAG, "An error has occured while starting telegram-cli.");

				if (tg_cli.Id > 0)
					InternalLogger.e (TAG, "But it seems to be started even it has an error.");
				else
					return false;
			}

			return true;
		}

		/// <summary>
		/// Get instance of TelegramCLI class
		/// </summary>
		/// <value>TelegramCLI Class Singleton Instance</value>
		public static TelegramCLI getInstance
		{
			get {
				if (_instance == null)
					_instance = new TelegramCLI (tg_cli_location, parameters);

				return _instance;
			}
		}
	}
}

