using NgUtil.Generics.Enums;

namespace NgHTTP.Util {
    public sealed class HttpCode : ExtendedEnum<HttpCode> {

        public int Code { get; }

        public string Message { get; }


        private HttpCode(string name, int code, string message) : base(name) {
            Code = code;
            Message = message;
        }

		public bool IsInformational() {
			return Code >= 100 && Code < 200;
		}

		public bool IsSuccess() {
			return Code >= 200 && Code < 300;
		}

		public bool IsRedirection() {
			return Code >= 300 && Code < 400;
		}

		public bool IsClientError() {
			return Code >= 400 && Code < 500;
		}

		public bool IsServerError() {
			return Code >= 500;
		}

		public override string ToString() {
			return "[" + Code + "][" + NameIdentifier + "]: " + Message;
		}

		public static HttpCode GetForCode(int code) {
			foreach (HttpCode hc in Values()) {
				if (hc.Code == code) {
					return hc;
				}
			}
			return Invalid;
		}

		public static readonly HttpCode Invalid = new HttpCode("Invalid", 0, "Invalid");
		public static readonly HttpCode Unknown = new HttpCode("Unknown", 0, "Unknown");

		//Informational
		public static readonly HttpCode Continue = new HttpCode("Continue", 100, "The server, has received the request headers and the client should proceed to send the request body");
		public static readonly HttpCode SwitchingProtocols = new HttpCode("Switching Protocols", 101, "The requester has asked the server to switch protocols and the server has agreed to do so");
		public static readonly HttpCode Processing = new HttpCode("Processing", 102, "The server has received and is processing the request, but no response is available yet");
		public static readonly HttpCode EarlyHints = new HttpCode("Early Hints", 103, "Partial response received, still waiting for final message");

		//Success
		public static readonly HttpCode Ok = new HttpCode("OK", 200, "Standard response for successful HTTP requests");
		public static readonly HttpCode Created = new HttpCode("Created", 201, "The request has been fulfilled, resulting in the creation of a new resource");
		public static readonly HttpCode Accepted = new HttpCode("Accepted", 202, "The request has been accepted for processing, but the processing has not been completed");
		public static readonly HttpCode NonAuthoritativeInformation = new HttpCode("Non-Authoritative Information", 203, "The server is a transforming proxy that received a 200 OK from its origin, but is returning a modified version of the origin's response");
		public static readonly HttpCode NoContent = new HttpCode("No Content", 204, "The server successfully processed the request and is not returning any content");
		public static readonly HttpCode ResetContent = new HttpCode("Reset Content", 205, "The server successfully processed the request, but is not returning any content, reset the document view");
		public static readonly HttpCode PartialContent = new HttpCode("Partial Content", 206, "The server is delivering only part of the resource due to a range header sent by the client");
		public static readonly HttpCode MultiStatus = new HttpCode("Multi-Status", 207, "The message body that follows is by default an XML message and can contain a number of separate response codes, depending on how many sub-requests were made");
		public static readonly HttpCode AlreadyReported = new HttpCode("Already Reported", 208, "The members of a DAV binding have already been enumerated in a preceding part of the (multistatus) response, and are not being included again");
		public static readonly HttpCode ImUsed = new HttpCode("IM Used", 226, "The server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance");

		//Redirection
		public static readonly HttpCode MultipleChoices = new HttpCode("Multiple Choices", 300, "Indicates multiple options for the resource from which the client may choose");
		public static readonly HttpCode MovedPermanently = new HttpCode("Moved Permanently", 301, "This and all future requests should be directed to the given URI");
		public static readonly HttpCode Found = new HttpCode("Found", 302, "Tells the client to look at (browse to) another URL");
		public static readonly HttpCode SeeOther = new HttpCode("See Other", 303, "The response to the request can be found under another URI using the GET method");
		public static readonly HttpCode NotModified = new HttpCode("Not Modified", 304, "Indicates that the resource has not been modified since the version specified by the request headers If-Modified-Since or If-None-Match");
		public static readonly HttpCode TemporaryRedirect = new HttpCode("Temporary Redirect", 307, "The request should be repeated with another URI; however, future requests should still use the original URI");
		public static readonly HttpCode PermanentRedirect = new HttpCode("Permanent Redirect", 308, "The request and all future requests should be repeated using another URI");

		//Client error
		public static readonly HttpCode BadRequest = new HttpCode("Bad Request", 400, "The server cannot or will not process the request due to an apparent client error");
		public static readonly HttpCode Unauthorized = new HttpCode("Unauthorized", 401, "Authentication is required and has failed or has not yet been provided");
		public static readonly HttpCode Forbidden = new HttpCode("Forbidden", 403, "The request contained valid data and was understood by the server, but the server is refusing action");
		public static readonly HttpCode NotFound = new HttpCode("Not Found", 404, "The requested resource could not be found but may be available in the future");
		public static readonly HttpCode MethodNotAllowed = new HttpCode("Method Not Allowed", 405, "A request method is not supported for the requested resource");
		public static readonly HttpCode NotAcceptable = new HttpCode("Not Acceptable", 406, "The requested resource is capable of generating only content not acceptable according to the Accept headers sent in the request");
		public static readonly HttpCode ProxyAuthenticationRequired = new HttpCode("Proxy Authentication Required", 407, "The client must first authenticate itself with the proxy");
		public static readonly HttpCode RequestTimeout = new HttpCode("Request Timeout", 408, "The server timed out waiting for the request");
		public static readonly HttpCode Conflict = new HttpCode("Conflict", 409, "Indicates that the request could not be processed because of conflict in the current state of the resource, such as an edit conflict between multiple simultaneous updates");
		public static readonly HttpCode Gone = new HttpCode("Gone", 410, "Indicates that the resource requested is no longer available and will not be available again");
		public static readonly HttpCode LenghtRequired = new HttpCode("Length Required", 411, "The request did not specify the length of its content, which is required by the requested resource");
		public static readonly HttpCode PreconditionFailed = new HttpCode("Precondition Failed", 412, "The server does not meet one of the preconditions that the requester put on the request header fields");
		public static readonly HttpCode PayloadTooLarge = new HttpCode("Payload Too Large", 413, "The request is larger than the server is willing or able to process");
		public static readonly HttpCode UriTooLong = new HttpCode("URI Too Long", 414, "The URI provided was too long for the server to process");
		public static readonly HttpCode UnsupportedMediaType = new HttpCode("Unsupported Media Type", 415, "The request entity has a media type which the server or resource does not support");
		public static readonly HttpCode RangeNotSatisfiable = new HttpCode("Range Not Satisfiable", 416, "The client has asked for a portion of the file, but the server cannot supply that portion");
		public static readonly HttpCode ExceptionFailed = new HttpCode("Expectation Failed", 417, "The server cannot meet the requirements of the Expect request-header field");
		public static readonly HttpCode MisredirectedRequest = new HttpCode("Misdirected Request", 421, "The request was directed at a server that is not able to produce a response");
		public static readonly HttpCode UnprocessableEntity = new HttpCode("Unprocessable Entity", 422, "The request was well-formed but was unable to be followed due to semantic errors");
		public static readonly HttpCode Locked = new HttpCode("Locked", 423, "The resource that is being accessed is locked");
		public static readonly HttpCode FailedDependency = new HttpCode("Failed Dependency", 424, "The request failed because it depended on another request and that request failed");
		public static readonly HttpCode TooEarly = new HttpCode("Too Early", 425, "Indicates that the server is unwilling to risk processing a request that might be replayed");
		public static readonly HttpCode UpgradeRequired = new HttpCode("Upgrade Required", 426, "The client should switch to a different protocol such as TLS/1.0, given in the Upgrade header field");
		public static readonly HttpCode PreconditionRequired = new HttpCode("Precondition Required", 428, "The origin server requires the request to be conditional");
		public static readonly HttpCode TooManyRequests = new HttpCode("Too Many Requests", 429, "The user has sent too many requests in a given amount of time");
		public static readonly HttpCode RequestHeaderFieldsTooLarge = new HttpCode("Request Header Fields Too Large", 431, "The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large");
		public static readonly HttpCode UnavailableForLegalReasons = new HttpCode("Unavailable For Legal Reasons", 451, "A server operator has received a legal demand to deny access to a resource or to a set of resources that includes the requested resource");

		//Server error
		public static readonly HttpCode InternalServerError = new HttpCode("Internal Server Error", 500, "A generic error message, given when an unexpected condition was encountered and no more specific message is suitable");
		public static readonly HttpCode NotImplemented = new HttpCode("Not Implemented", 501, "The server either does not recognize the request method, or it lacks the ability to fulfil the request");
		public static readonly HttpCode BadGateway = new HttpCode("Bad Gateway", 502, "The server was acting as a gateway or proxy and received an invalid response from the upstream server");
		public static readonly HttpCode ServiceUnavailable = new HttpCode("Service Unavailable", 503, "The server cannot handle the request (because it is overloaded or down for maintenance)");
		public static readonly HttpCode GatewayTimeout = new HttpCode("Gateway Timeout", 504, "The server was acting as a gateway or proxy and did not receive a timely response from the upstream server");
		public static readonly HttpCode HttpVersionNotSupported = new HttpCode("HTTP Version Not Supported", 505, "The server does not support the HTTP protocol version used in the request");
		public static readonly HttpCode VariantAlsoNegotiates = new HttpCode("Variant Also Negotiates", 506, "Transparent content negotiation for the request results in a circular reference");
		public static readonly HttpCode InsufficientStorage = new HttpCode("Insufficient Storage", 507, "The server is unable to store the representation needed to complete the request");
		public static readonly HttpCode LoopDetected = new HttpCode("Loop Detected", 508, "The server detected an infinite loop while processing the request");
		public static readonly HttpCode NotExtended = new HttpCode("Not Extended", 510, "Further extensions to the request are required for the server to fulfil it");
		public static readonly HttpCode NetworkAuthenticationRequired = new HttpCode("Network Authentication Required", 511, "The client needs to authenticate to gain network access");

    }
}
