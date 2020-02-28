

namespace NgHTTP.Util {
    public static class HttpUtil {


		public static string GetBaseUrl(string url) {
			return (url.StartsWith("https") ? "https://" : "http://") + GetDomain(url);
		}


		public static string GetDomain(string url) {
			string[] args = url.Split("/");
			return url.StartsWith("http") ? args[2] : args[0];
		}

	}
}
