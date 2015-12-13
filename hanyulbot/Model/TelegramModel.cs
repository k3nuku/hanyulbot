/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: TelegramModel.cs (Model/TelegramModel.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : A Serializable model of Telegram basics
 * 
 */
namespace hanyulbot
{
	public class TelegramModel
	{
		public string @event { get; set; }
		public string service { get; set; }
		public int id { get; set; }
		public bool unread { get; set; }
		public int flags { get; set; }
		public string text { get; set; }
		public bool @out { get; set; }
		public string date { get; set; }

		public TelegramEndPointModel @to { get; set; }
		public TelegramEndPointModel @from { get; set; }
		public TelegramMediaModel media { get; set; } // for media
	}
}

