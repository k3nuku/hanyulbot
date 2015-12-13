/*
 * 				Hanyul Bot - Ha:nyul (ㅎㅏㄴㅠㄹ) Telegram Bot
 * 		
 * 				
 * 		File Name 	: JSONParser.cs (Parser/JSONParser.cs)
 * 		Author 		: Sokdak (a.k.a k3nuku)
 * 		Created at	: Fri Dec 11, 2015.
 * 		Description : A module that parsing JSON strings and deserializing them
 * 
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using Mono.Unix;

namespace hanyulbot
{
	public class JSONParser
	{
		/// <summary>
		/// Deserialize the specified jsontext and mode.
		/// </summary>
		/// <param name="jsontext">Jsontext.</param>
		/// <param name="mode">DeserializeObject</param>
		public static T Deserialize<T>(string jsonString)
		{
			try {
				using (MemoryStream ms = new MemoryStream (UnixEncoding.Unicode.GetBytes (jsonString))) {
					DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof(T));
					return (T)serializer.ReadObject (ms);
				}
			} catch {
				return default(T);
			}
		}
	}
}

