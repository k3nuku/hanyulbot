/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: CorePreprocessor.cs (Preprocessor/CorePreprocessor.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : Heart of processing messages
 * 
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hanyulbot
{
	public class CorePreprocessor
	{
		private static string TAG = "CorePP";

		public static void Process(TelegramModel model)
		{
			
		}

		public static void ErrorProcess(TelegramErrorModel model)
		{
			InternalLogger.d (TAG, string.Format ("result: {0} error_code: {1} error_msg: {2}", model.result, model.error_code, model.error));
		}

		public static void AnswerProcess(TelegramAnswerModel model)
		{
			InternalLogger.d (TAG, string.Format ("result: {0} event: {1}", model.result, model.@event));
		}
	}
}

