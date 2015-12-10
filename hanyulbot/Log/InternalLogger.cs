/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: InternalLogger.cs (Log/InternalLogger.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : Basic logging module for internal shell
 * 
 */
using System;

namespace hanyulbot
{
	public class InternalLogger
	{
		/// <summary>
		/// Internal logging with debug flag
		/// </summary>
		/// <param name="TAG">Process TAG</param>
		/// <param name="message">output message</param>
		public static void d(string TAG, string message, params string formatparams)
		{// [yymmdd hh:mm:ss:fff] [tag] message
			Console.WriteLine (String.Format("[{0}] [{1}::debug]\t{2}", DateTime.Now.ToString("yyMMdd HH:mm:ss.fff"), TAG, message));
		}

		/// <summary>
		/// Internal logging with information flag
		/// </summary>
		/// <param name="TAG">Process TAG</param>
		/// <param name="message">output message</param>
		public static void i(string TAG, string message)
		{
			Console.WriteLine (String.Format("[{0}] [{1}::info]\t{2}", DateTime.Now.ToString("yyMMdd HH:mm:ss.fff"), TAG, message));
		}

		/// <summary>
		/// Internal logging with error flag
		/// </summary>
		/// <param name="TAG">Proess TAG</param>
		/// <param name="message">output message</param>
		public static void e(string TAG, string message)
		{
			Console.WriteLine (String.Format("[{0}] [{1}::error]\t{2}", DateTime.Now.ToString("yyMMdd HH:mm:ss.fff"), TAG, message));
		}
	}
}

