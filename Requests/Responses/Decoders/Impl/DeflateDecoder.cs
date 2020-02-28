using System.IO;
using System.IO.Compression;

namespace NgHTTP.Requests.Responses.Decoders.Impl {
    public sealed class DeflateDecoder : IResponseDecoder {


        public Stream Decode(Stream inputStream) {
            return new DeflateStream(inputStream, CompressionMode.Decompress);
        }

    }
}
