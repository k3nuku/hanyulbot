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
using System;

namespace hanyulbot
{
	public class TelegramEndPointModel
	{
		public string id;
		public string type;
		public string username;
		public string print_name;
		public string flags;
		public string first_name;
		public string when;
		public string phone;
		public string last_name;
	}
}

