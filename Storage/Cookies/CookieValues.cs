
namespace NgHTTP.Storage.Cookies {
    public static class CookieValues {

        public static readonly string SameSiteStrict = "Strict"; //Only on the exact domain???
	    public static readonly string SameSiteLax = "Lax"; //Only serve the cookie on the top level domain
	    public static readonly string SameSiteNone = "None"; //Serve cookies to 3rd parties

    }
}
