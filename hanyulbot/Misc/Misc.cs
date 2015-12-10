using Mono.Unix;
using System;

namespace hanyulbot
{
	public class Misc
	{
		/// <summary>
		/// Converts to UTF-8 bytes.
		/// </summary>
		/// <returns>The UTF-8 bytes of origin string</returns>
		/// <param name="input">string</param>
		public static byte[] ConvertToUTF8Bytes(string input, long size=0)
		{
			if (size == 0)
				return UnixEncoding.UTF8.GetBytes (input);
			else
				return UnixEncoding.UTF8.GetBytes (input, 0, size);
		}

		/// <summary>
		/// Converts to UTF-8 string.
		/// </summary>
		/// <returns>The UTF-8 string of origin bytes</returns>
		/// <param name="input">bytes</param>
		public static string ConvertToUTF8String(byte[] input)
		{
			return UnixEncoding.UTF8.GetString (input);
		}
	}
}

