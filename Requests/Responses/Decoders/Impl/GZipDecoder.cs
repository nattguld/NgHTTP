using System.IO;
using System.IO.Compression;

namespace NgHTTP.Requests.Responses.Decoders.Impl {
    public sealed class GZipDecoder : IResponseDecoder {


        public Stream Decode(Stream inputStream) {
            return new GZipStream(inputStream, CompressionMode.Decompress);
        }

    }
}
