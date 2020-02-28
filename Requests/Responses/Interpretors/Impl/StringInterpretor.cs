using NgHTTP.Configs;
using NgHTTP.Requests.Responses.Bodies.Impl;
using NgHTTP.Requests.Responses.Decoders;
using NgHTTP.Requests.Responses.Decoders.Impl;
using NgUtil.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NgHTTP.Requests.Responses.Interpretors.Impl {
    public sealed class StringInterpretor : ResponseInterpretor<StringResponseBody> {

        private static readonly Dictionary<string, IResponseDecoder> decoders = new Dictionary<string, IResponseDecoder>();

        static StringInterpretor() {
            decoders.Add("gzip", new GZipDecoder());
            decoders.Add("br", new BrotliDecoder());
            decoders.Add("deflate", new DeflateDecoder());
        }

        private readonly string contentEncoding;

        private readonly string contentType;


        public StringInterpretor(long bodySize, string contentEncoding, string contentType) : base(bodySize) {
            this.contentEncoding = contentEncoding;
            this.contentType = contentType;
        }

        public static void CopyTo(Stream src, Stream dest) {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

        public override StringResponseBody Interpret(Stream stream) {
            bool hasContentEncoding = !string.IsNullOrEmpty(contentEncoding);
            IResponseDecoder decoder = !hasContentEncoding ? null : decoders.GetValueOrDefault(contentEncoding.ToLower());

            if (HttpConfig.DebugMode) {
                Console.WriteLine("Interpretation decoder " + decoder + " for content type: " + contentType + ", Body size: " + BodySize);
            }
            StringBuilder sb = new StringBuilder();
            
            using (StreamReader reader = new StreamReader(decoder is null ? stream : decoder.Decode(stream), Encoding.UTF8)) {
                while (true) { //!reader.EndOfStream
                     try {
                         string line = reader.ReadLine();

                         if (line is null) {
                            break;
                         }
                         AddProgress(line.Length);
                         sb.Append(line + Delimiters.Linebreak);

                         if (!hasContentEncoding && sb.Length >= BodySize) {
                             break;
                         }
                     } catch (Exception ex) {
                         Console.WriteLine(ex.ToString());
                         continue;
                     }
                }
            }
            if (HttpConfig.DebugMode) {
                Console.WriteLine("Body interpretation successfull");
            }
            //return (StringResponseBody)Activator.CreateInstance(typeof(StringResponseBody), new object[] { sb.ToString() });
            byte[] bytes = Encoding.ASCII.GetBytes(sb.ToString());
            return new StringResponseBody(Encoding.UTF8.GetString(bytes));
        }

    }
}
