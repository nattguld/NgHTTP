using NgHTTP.Headers;
using NgHTTP.Sockets.Http;
using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Content {
    public abstract class ContentBody {

        public EncType EncType { get; }

        public long ContentLength { get; protected set; }

        public bool Chunked { get; set; }

        public ChunkHandler ChunkHandler { get; set; }

        public bool UrlEncoded { get; set; }


        public ContentBody(EncType encType) {
            EncType = encType;
        }

        protected virtual void SetContentLength(long contentLength) {
            ContentLength = contentLength;
        }

        protected virtual void SetContentHeaders(StringStringKeyValuePairContainer headers) {
            headers.Put(HeaderKeys.ContentType, EncType.ContentValue);
            headers.Put(HeaderKeys.ContentLength, ContentLength.ToString());
        }

        protected abstract void Build(IHttpStreamable httpStream, bool prepare);

        public ContentBody Prepare(StringStringKeyValuePairContainer headers) {
            using (HttpMemoryStream httpMemStream = new HttpMemoryStream()) {
                Build(httpMemStream, false);

                SetContentLength(httpMemStream.Length);
                SetContentHeaders(headers);
            }
            return this;
        }

        public ContentBody Write(HttpSocket httpSocket) {
            Build(httpSocket, true);
            return this;
        }

        public abstract T GetContent<T>();

    }
}
