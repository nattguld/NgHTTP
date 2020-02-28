using NgUtil.Text;
using System.IO;
using System.Text;

namespace NgHTTP.Streams {
    public sealed class HttpMemoryStream : MemoryStream, IHttpStreamable {


        public IHttpStreamable AppendString(string value) {
            return Write(Encoding.UTF8.GetBytes(value));
        }

        public IHttpStreamable Write(byte[] buffer) {
            return WriteUnderlying(buffer, 0, buffer.Length);
        }

        public IHttpStreamable WriteLine(string value) {
            return AppendString(value).WriteLine();
        }

        public IHttpStreamable WriteLine() {
            return AppendString(Delimiters.Linebreak);
        }

        public IHttpStreamable FlushUnderlying() {
            Flush();
            return this;
        }

        public IHttpStreamable WriteUnderlying(byte[] buffer, int offset, int count) {
            Write(buffer, offset, count);
            return this;
        }

    }
}
