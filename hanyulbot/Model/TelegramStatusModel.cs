/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: TelegramStatusModel.cs (Model/TelegramStatusModel.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 12, 2015.
 * 		Description : A Serializable model of Telegram Status
 * 
 */
namespace hanyulbot
{
	public class TelegramStatusModel
	{
		public TelegramEndPointModel user { get; set; }
		public bool online { get; set; }
		public string @event { get; set; }
		public string when { get; set; }
		public int state { get; set; }
	}
}

