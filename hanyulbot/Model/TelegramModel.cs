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
using System;

namespace hanyulbot
{
	public class TelegramModel
	{
		public string @out;
		public string @event;
		public string id;
		public string flags;
		public string text;
		public string service;
		public string unread;
		public string date;

		public TelegramEndPointModel @to;
		public TelegramEndPointModel @from;
	}
}

