using System.IO;
using System.IO.Compression;

namespace NgHTTP.Requests.Responses.Decoders.Impl {
    public sealed class BrotliDecoder : IResponseDecoder {


        public Stream Decode(Stream inputStream) {
            return new BrotliStream(inputStream, CompressionMode.Decompress);
        }

    }
}
