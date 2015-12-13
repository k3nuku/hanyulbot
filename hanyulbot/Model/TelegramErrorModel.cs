/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: TelegramErrorModel.cs (Model/TelegramErrorModel.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 12, 2015.
 * 		Description : A Serializable model of Telegram Error
 * 
 */
namespace hanyulbot
{
	public class TelegramErrorModel
	{
		public string result { get; set; }
		public string error_code { get; set; }
		public string error { get; set; }
	}
}

