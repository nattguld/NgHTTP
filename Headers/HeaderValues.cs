
namespace NgHTTP.Headers {
    public static class HeaderValues {

		public static readonly string NoCache = "no-cache";
		public static readonly string MaxZeroCache = "max-age=0";
	
		public static readonly string ConnectionKeepAlive = "keep-alive";
		public static readonly string ConnectionClose = "close";
	
		public static readonly string XmlHttpRequest = "XMLHttpRequest";
	
		public static readonly string AcceptEncoding = "gzip"; //gzip, deflate, br

		public static readonly string SecFetchNone = "none";
		public static readonly string SecFetchNavigate = "navigate";
		public static readonly string SecFetchNestedNavigate = "nested-navigate";
		public static readonly string SecFetchCors = "cors";
		public static readonly string SecFetchSameOrigin = "same-origin";
		public static readonly string SecFetchSameSite = "same-site";
		public static readonly string SecFetchCrossSite = "cross-site";
		public static readonly string SecFetchUser = "?1";
	
		public static readonly string TransferEncodingChunked = "chunked";

    }
}
