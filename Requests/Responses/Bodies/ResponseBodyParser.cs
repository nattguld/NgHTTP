using NgHTTP.Configs;
using NgHTTP.Headers;
using NgHTTP.Requests.Responses.Bodies.Impl;
using NgHTTP.Requests.Responses.Interpretors;
using NgHTTP.Requests.Responses.Interpretors.Impl;
using NgHTTP.Requests.Responses.Listeners;
using NgHTTP.Streams;
using NgUtil.Debugging.Logging;
using NgUtil.Generics.Kvps.Impl;
using NgUtil.Maths;
using NgUtil.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NgHTTP.Requests.Responses.Bodies {
	public static class ResponseBodyParser {

		private static readonly Logger logger = Logger.GetLogger("ResponseBodyParser");


		public static IBaseResponseBody ParseResponseBody(Request request, StringStringKeyValuePairContainer responseHeaders, Stream inputStream) {
			if (request.HasProperty(RequestProperty.NoDecode)) {
				return new StringResponseBody("Body decoding is turned off for this request");
			}
			string contentEncoding = responseHeaders.GetValue(HeaderKeys.ContentEncoding); //The content encoding of the response, will be null when it's plain text
			string contentLength = responseHeaders.GetValue(HeaderKeys.ContentLength);
			string contentType = responseHeaders.GetValue(HeaderKeys.ContentType);
			string chunkedValue = responseHeaders.GetValue(HeaderKeys.TransferEncoding);
			bool chunked = chunkedValue != null && contentLength == null
					&& chunkedValue.Equals(HeaderValues.TransferEncodingChunked, StringComparison.OrdinalIgnoreCase);

			long bodySize = 0;

			if (contentLength != null) {
				if (!MathUtil.IsInteger(contentLength)) {
					throw new Exception("Content length for " + request.Url + " is not a valid integer: " + contentLength);
				}
				bodySize = MathUtil.ParseInt(contentLength, true, 0);
			}
			if (!chunked && contentLength == null) {
				//TODO Temporary removed to not block throw new NetException("Response for " + request.getUrl() + " is not chunked but has no Content-Length header.");
				logger.Warning("Response for " + request.Url + " is not chunked but has no Content-Length header.");
			}
			Console.WriteLine("Parsing server response body for [" + request.Url + "] with properties [Content-Encoding: " + contentEncoding
						+ ", Transfer-Encoding: " + chunkedValue + ", Content-Length: " + bodySize + ", Content-Type: " + contentType + "]"); //TODO

			bool download = request.SavePath != null && contentType != null;

			if (chunked) { //Read the chunks if the response is chunked
				if (responseHeaders.GetValue(HeaderKeys.Location) != null) { //When it's a redirect we don't need to read any body
					return new StringResponseBody("");
				}
				logger.Debug("Reading response body chunks");

				inputStream = ReadChunks(inputStream);

				if (inputStream is null) {
					logger.Error("Failed to read chunks on " + request.Url);

					if (HttpConfig.DebugMode) {
						throw new Exception("Failed to read chunks on " + request.Url);
					}
					return download ? new FileResponseBody(null) : (IBaseResponseBody)new StringResponseBody("Failed to read chunks");
				}
				if (bodySize > 0 && bodySize != inputStream.Length) {
					logger.Warning("Unexpected body size for " + request.Url + ", received " + inputStream.Length + " instead of " + bodySize);
				}
				bodySize = inputStream.Length;
			}
			if (contentLength != null && contentLength.Equals("0", StringComparison.Ordinal)) {
				logger.Debug("Content length is zero for " + request.Url + ", no body to parse");
				return new StringResponseBody("");
			}
			BaseResponseInterpretor interpretor = download ? new FileInterpretor(bodySize, request.SavePath)
					: (BaseResponseInterpretor)new StringInterpretor(bodySize, contentEncoding, contentType);

			if (request.HasProgressListener) {
				Task.Run(() => TrackProgress(request.ProgressListener, interpretor));
			}
			IBaseResponseBody respBody;

			if (download) {
				FileInterpretor fi = interpretor as FileInterpretor;
				respBody = fi.Interpret(inputStream);

			} else {
				StringInterpretor si = interpretor as StringInterpretor;
				respBody = si.Interpret(inputStream);
			}
			return respBody;
		}

		private static void TrackProgress(ProgressListener progressListener, BaseResponseInterpretor interpretor) {
			while (interpretor.Progress < 100) {
				progressListener.SetProgress(interpretor.Progress);
			}
		}

		private static Stream ReadChunks(Stream inputStream) {
			using (MemoryStream memStream = new MemoryStream()) {
				List<byte> lineBuffer = new List<byte>();

				while (true) {
					byte b = (byte)inputStream.ReadByte();

					if (b < 0) {
						//Removed |We reached the end of the input stream b < 0   4=end of transmission - 23=end of transmission block
						Console.WriteLine("Malformed server response received, EOF reached unexpectedly");
						break;
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
					lineBuffer.Clear();

					if (line == string.Empty) { //Nothing to handle here
						continue;
					}
					if (line.Equals("0", StringComparison.Ordinal)
						|| line.Equals(Delimiters.Linebreak, StringComparison.Ordinal)) {
						break;
					}
					int chunkSize = 0;

					try { //Check if the line is a hex giving us the chunk size and convert to decimal if so
						chunkSize = MathUtil.HexToInteger(line); //Integer.parseInt(s.trim(), 16);

					} catch (Exception ex) { //Get rid of the received bytes if it's not a chunk size indicator
						Console.WriteLine(ex.ToString());
						memStream.Write(lineBufferArray, 0, lineBufferArray.Length); //Write the line buffer to our unchunked body stream
						continue;
					}
					int received = 0; //Holds how many bytes we've received in total for this chunk

					while (received < chunkSize) { //As long as we didn't receive the expected bytes, keep waiting and reading
						byte[] chunkBuffer = new byte[chunkSize - received]; //Create a byte buffer with the chunk size
						int readCount = inputStream.Read(chunkBuffer); //Fill up the chunk buffer and retrieve the amount of bytes we've read
						memStream.Write(chunkBuffer, 0, readCount); //Write the chunk buffer to our unchunked body stream
						received += readCount; //Add the count we read to our total received count
					}
				}
				if (memStream.Length <= 0) {
					Console.WriteLine("Empty body payload received after reading chunks");
					return null;
				}
				return new MemoryStream(memStream.ToArray());
			}
		}

    }
}
