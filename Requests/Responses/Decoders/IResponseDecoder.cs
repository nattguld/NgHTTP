using System.IO;

namespace NgHTTP.Requests.Responses.Decoders {
    public interface IResponseDecoder {


        public Stream Decode(Stream inputStream);

    }
}
