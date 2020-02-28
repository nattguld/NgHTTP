using NgHTTP.Configs;
using NgHTTP.Headers;
using NgHTTP.Sockets.Http;
using NgHTTP.Storage.Cookies;
using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil;
using NgUtil.Generics.Kvps.Impl;
using NgUtil.Maths;
using NgUtil.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NgHTTP.Requests.Responses.Decoders.Impl {
    public sealed class HttpHeaderDecoder : IResponseDecoder {

        public ResponseStatus ResponseStatus { get; private set; } = new ResponseStatus(HttpVersion.Http_1_1, HttpCode.Invalid, "Header not found in decoding process");

        public StringStringKeyValuePairContainer Headers { get; } = new StringStringKeyValuePairContainer();

        public List<Cookie> Cookies { get; } = new List<Cookie>();


        public Stream Decode(Stream inputStream) {
            List<byte> lineBuffer = new List<byte>();

			while (true) {
				byte b = (byte)inputStream.ReadByte();

				if (b < 0) {
					Console.WriteLine("Malformed server response received, EOF reached unexpectedly");
					return null;
				}
				lineBuffer.Add(b);

				if (b != 10) { //Keep collecting bytes if there's no linefeed (new line) \n indicated
					continue;
				}
				byte[] lineBufferArray = new byte[lineBuffer.Count];

				for (int i = 0; i < lineBufferArray.Length; i++) {
					lineBufferArray[i] = lineBuffer[i];
				}
				string line = Encoding.ASCII.GetString(lineBufferArray).Trim(); //Build a string out of our collected bytes and trim off the line break
				lineBuffer.Clear(); //Clears the line buffer for the next read

				if (line == string.Empty) { //When an empty line is found it means we parsed all headers
					break;
				}
				if (line.Equals("0", StringComparison.Ordinal)
					|| line.Equals(Delimiters.Linebreak, StringComparison.Ordinal)) {
					break;
				}
				if (line.StartsWith("HTTP", StringComparison.OrdinalIgnoreCase)) { //The status line of the response
					string[] msgParts = line.Split(" ");
					string httpProtocolVersion = msgParts[0];
					string responseCode = msgParts[1];
					StringBuilder responseMsgBuilder = new StringBuilder();

					for (int i = 2; i < msgParts.Length; i++) {
						responseMsgBuilder.Append(msgParts[i] + " ");
					}
					int code = MathUtil.ParseInt(responseCode);
					HttpCode httpCode = HttpCode.GetForCode(code);
					string msg = responseMsgBuilder.ToString().Trim();

					if (string.IsNullOrEmpty(msg)) {
						msg = httpCode.Message;
					}
					if (httpCode == HttpCode.Invalid) {
						msg = "[Unhandled code: " + code + "] - " + msg;
					}
					ResponseStatus = new ResponseStatus(HttpVersion.Parse(httpProtocolVersion), httpCode, msg);
					continue;
				}
				string key = StringFunc.Substring(line, 0, line.IndexOf(":")).Trim();
				string value = StringFunc.Substring(line, line.IndexOf(":") + 1, line.Length).Trim();

				if (!key.Equals(HeaderKeys.SetCookie, StringComparison.OrdinalIgnoreCase)) {
					Headers.Put(key, value);

					if (HttpConfig.DebugMode) {
						Console.WriteLine("RESPONSE-HEADER => " + key + ": " + value);
					}
					continue;
				}
				Cookie cookie = ExtractCookie(value);
				Cookies.Add(cookie);

				if (HttpConfig.DebugMode) {
					Console.WriteLine("RESPONSE-COOKIE => " + cookie.ToString());
				}
			}
			return inputStream;
        }

		private static Cookie ExtractCookie(string headerValue) {
			string[] fields = headerValue.Split(";");

			string key = StringFunc.Substring(fields[0], 0, fields[0].IndexOf("=")).Trim();
			string value = StringFunc.Substring(fields[0], fields[0].IndexOf("=") + 1, fields[0].Length);
			string expires = null;
			string path = "/";
			string domain = "/";
			bool secure = false;
			bool httpOnly = false;
			long maxAge = 0L;
			string sameSite = "Lax";

			for (int i = 1; i < fields.Length; i++) {
				string field = fields[i].Trim();

				if (field.Equals(CookieKeys.Secure, StringComparison.OrdinalIgnoreCase)) {
					secure = true;
					continue;
				}
				if (field.Equals(CookieKeys.HttpOnly, StringComparison.OrdinalIgnoreCase)) {
					httpOnly = true;
					continue;
				}
				if (!field.Contains("=", StringComparison.Ordinal)) {
					Console.WriteLine("Malformed cookie field: " + fields[i]);
					continue;
				}
				string fieldKey = StringFunc.Substring(field, 0, field.IndexOf("=")).Trim();
				string fieldValue = StringFunc.Substring(field, field.IndexOf("=") + 1, field.Length);

				if (fieldKey.Equals(CookieKeys.Expires, StringComparison.OrdinalIgnoreCase)) {
					expires = fieldValue;
					continue;
				}
				if (fieldKey.Equals(CookieKeys.Domain, StringComparison.OrdinalIgnoreCase)) {
					domain = fieldValue;
					continue;
				}
				if (fieldKey.Equals(CookieKeys.Path, StringComparison.OrdinalIgnoreCase)) {
					path = fieldValue;
					continue;
				}
				if (fieldKey.Equals(CookieKeys.MaxAge, StringComparison.OrdinalIgnoreCase)) {
					maxAge = MathUtil.ParseLong(fieldValue, true, 0L);
					continue;
				}
				if (fieldKey.Equals(CookieKeys.SameSite, StringComparison.OrdinalIgnoreCase)) {
					sameSite = fieldValue;
					continue;
				}
				Console.WriteLine("Unhandled cookie field: " + fields[i]);
			}
			Cookie cookie = new Cookie(key, value, domain);
			cookie.Expires = expires;
			cookie.Path = path;
			cookie.Secure = secure;
			cookie.MaxAge = maxAge;
			cookie.SameSite = sameSite;

			return cookie;
		}

	}
}
