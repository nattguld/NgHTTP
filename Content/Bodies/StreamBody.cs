using NgHTTP.Configs;
using NgHTTP.Headers;
using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil.Files;
using NgUtil.Generics.Kvps.Impl;
using System;
using System.IO;
using System.Text;

namespace NgHTTP.Content.Bodies {
    public sealed class StreamBody : ContentBody {

        private readonly FileLink fileLink;

        private readonly FileInfo fileInfo;

        private readonly bool raw;


        public StreamBody(FileLink fileLink, bool raw = false) : base(EncType.Stream) {
            this.fileLink = fileLink;
            this.fileInfo = (FileInfo)fileLink.GetInfo();
            this.raw = raw;
        }

        public override FileLink GetContent<FileLink>() {
            return (FileLink)Convert.ChangeType(fileLink, typeof(FileLink));
        }

        protected override void Build(IHttpStreamable httpStream, bool prepare) {
            if (prepare) {
                return;
            }
            using (FileStream fis = new FileStream(fileLink.Path, FileMode.Open)) {
                int bytesRead = -1;
                byte[] buffer = new byte[HttpConfig.ChunkSize];

                while ((bytesRead = fis.Read(buffer, bytesRead, buffer.Length)) != -1) {
                    httpStream.WriteUnderlying(buffer, 0, bytesRead);
                }
                httpStream.FlushUnderlying();
            }
        }

        protected override void SetContentLength(long contentLength) {
            base.SetContentLength(contentLength + fileInfo.Length);
        }

        protected override void SetContentHeaders(StringStringKeyValuePairContainer headers) {
            headers.Put(HeaderKeys.ContentType, raw ? EncType.ContentValue : MimeType.GetByFile(fileLink).Notation);
            headers.Put(HeaderKeys.ContentLength, ContentLength.ToString());
        }

    }
}
