/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: Misc.cs (Misc/Misc.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : Miscellaneous functions
 * 
 */
using System;
using Mono.Unix;

namespace hanyulbot
{
	public class Misc
	{
		/// <summary>
		/// Converts to UTF-8 bytes.
		/// </summary>
		/// <returns>The UTF-8 bytes of origin string</returns>
		/// <param name="input">string</param>
		public static byte[] ConvertToUTF8Bytes(string input)
		{
			return UnixEncoding.UTF8.GetBytes (input);
		}

		/// <summary>
		/// Converts to UTF-8 string.
		/// </summary>
		/// <returns>The UTF-8 string of origin bytes</returns>
		/// <param name="input">bytes</param>
		public static string ConvertToUTF8String(byte[] input, int size=0)
		{
			if (size == 0)
				return UnixEncoding.UTF8.GetString (input);
			else
				return UnixEncoding.UTF8.GetString (input, 0, size);
		}
	}
}

