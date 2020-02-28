using System;

namespace NgHTTP.Streams {
    public interface IHttpStreamable : IDisposable {


        public IHttpStreamable AppendString(string value);

        public IHttpStreamable WriteLine();

        public IHttpStreamable WriteLine(string value);

        public IHttpStreamable Write(byte[] buffer);

        public IHttpStreamable FlushUnderlying();

        public IHttpStreamable WriteUnderlying(byte[] buffer, int offset, int count);

    }
}
