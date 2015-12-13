/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: Program.cs (Program.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : Defines where program starts
 * 
 */
using System;
using System.Collections;

namespace hanyulbot
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string TAG = "PGMain";
			Console.Title = "Hanyul Bot";

			ArrayList a = new ArrayList (Environment.GetCommandLineArgs ());

			string binLocation = null;
			string socketLocation = null;

			if (a.Count > 2) {
				int b = a.IndexOf ("--bin");
				int c = a.IndexOf ("--socket");

				if (b > 0)
					binLocation = (string)a [b + 1];

				if (c > 0)
					socketLocation = (string)a [c + 1];
			}

			InternalLogger.d (TAG, "Starting Hanyulbot with Thread " + Environment.CurrentManagedThreadId);
			TelegramCLI tcli = new TelegramCLI (binLocation, socketLocation);
			tcli.Start ();
			CSocket cs = CSocket.GetInstance("/tmp/telegram.tmp");
			cs.Connect ();
			cs.Receive ();
			new System.Threading.ManualResetEvent (false).WaitOne ();
		}
	}
}
