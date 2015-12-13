/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: TelegramEndPointModel.cs (Model/TelegramEndPointModel.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : A Serializable model of Telegram EndPoint(Recipent, Sender)
 * 
 */
namespace hanyulbot
{
	public class TelegramEndPointModel
	{
		public int id { get; set; }
		public string type { get; set; }
		public string username { get; set; }
		public string print_name { get; set; }
		public int flags { get; set; }
		public string first_name { get; set; }
		public string when { get; set; }
		public string phone { get; set; }
		public string last_name { get; set; }

		public TelegramEndPointModel admin { get; set; } // for group chat
	}
}

