/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: Poll.cs (Core/Poll.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : A module that polling new or historical messages
 * 
 */
using System;
using System.Diagnostics;

namespace hanyulbot
{
	public class Poll
	{
		private static string TAG = "PollOU";
		private static int initialMsgCount = 9;

		/// <summary>
		/// Do a long-polling from stdout.
		/// </summary>
		public static void LongPollingStdout(object sender, DataReceivedEventArgs e)
		{
			if (initialMsgCount-- > 0)
				return;
			
			if (e.Data.StartsWith ("{")) {
				TelegramModel tm = JSONParser.Deserialize<TelegramModel> (e.Data);

				if (tm == null) { // Not valid JSON
					InternalLogger.e (TAG, "Not a valid JSON.");
					InternalLogger.d (TAG, e.Data);
				} else if (tm.@event != null) // if event is not null :: Valid TelegramModel
					CorePreprocessor.Process (tm);
				else if (tm != null) { // if tm is null :: Not valid TelegramModel (Assuming with TelegramErrorModel)
					TelegramErrorModel tem = JSONParser.Deserialize<TelegramErrorModel> (e.Data);
					CorePreprocessor.ErrorProcess (tem);
				} else { // Unknown JSON response
					InternalLogger.e (TAG, "Unknown Model type detected.");
					InternalLogger.d (TAG, e.Data);
				}
			} else {
				InternalLogger.e (TAG, "Data does not starts with '{'.");
				InternalLogger.d (TAG, e.Data);

			}
		}

		/// <summary>
		/// Do a long-polling result from socket.
		/// </summary>
		/// <param name="received">Received string</param>
		public static void PollingReceive(string received)
		{
			if (received.Contains ("{")) {
				received = received.Substring (received.IndexOf ('{'), received.Length - received.IndexOf ('{')); // get JSON Text only

				if (received.StartsWith ("{")) {
					TelegramAnswerModel tam = JSONParser.Deserialize<TelegramAnswerModel> (received);

					if (tam == null) {
						InternalLogger.e (TAG, "Not a valid JSON.");
						InternalLogger.d (TAG, received);
					} else if (tam.@result != null) // if result is not null :: Valid TelegramAnswerModel
						CorePreprocessor.AnswerProcess (tam);
					else if (tam != null) { // if tm is null :: Not valid TelegramModel (Assuming with TelegramErrorModel)
						TelegramErrorModel tem = JSONParser.Deserialize<TelegramErrorModel> (received);
						CorePreprocessor.ErrorProcess (tem);
					} else { // Unknown JSON response
						InternalLogger.e (TAG, "Unknown Model type detected.");
						InternalLogger.d (TAG, received);
					}
				} else {
					InternalLogger.e (TAG, "Data does not starts with '{'.");
					InternalLogger.d (TAG, received);
				}
			} else {
				/*
				stats
				users_allocated	4
				chats_allocated	1
				encr_chats_allocated	0
				peer_num	5
				messages_allocated	37
				*/

			}
		}
	}
}