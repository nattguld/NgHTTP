using NgHTTP.Headers;
using NgHTTP.Requests.Responses.Bodies;
using NgHTTP.Requests.Responses.Bodies.Impl;
using NgHTTP.Util;
using NgUtil.Files.IO.Json;
using NgUtil.Generics.Kvps.Impl;
using Supremes;
using Supremes.Nodes;
using Supremes.Parsers;
using System.IO;
using System.Text;

namespace NgHTTP.Requests.Responses {
    public sealed class RequestResponse {

        public string Endpoint { get; }

        public HttpCode ExpectedHttpCode { get; }

        public ResponseStatus ResponseStatus { get; }

        public int ResponseCode => ResponseStatus.HttpCode.Code;

        public StringStringKeyValuePairContainer ResponseHeaders { get; }

        public IBaseResponseBody ResponseBody { get; }

        private Document htmlDoc;

        private IJsonDocumentReader jsonReader;

        private string responseContent;


        public RequestResponse(string endpoint, HttpCode expectedHttpCode, ResponseStatus responseStatus
            , IBaseResponseBody responseBody, StringStringKeyValuePairContainer responseHeaders) {
            Endpoint = endpoint;
            ExpectedHttpCode = expectedHttpCode;
            ResponseStatus = responseStatus;
            ResponseBody = responseBody;
            ResponseHeaders = responseHeaders;
        }

        public bool Validate() {
            return Validate(ExpectedHttpCode);
        }

        public bool Validate(HttpCode code) {
            return Validate(code.Code);
        }

        public bool Validate(int code) {
            return ResponseCode == code;
        }

        public string GetResponseContent() {
            if (responseContent != null) {
                return responseContent;
            }
            if (!(ResponseBody is StringResponseBody)) {
                throw new System.Exception("Unable to fetch repsonse content from non-StringResponseBody");
            }
            responseContent = ((StringResponseBody)ResponseBody).GetBody();
            return responseContent;
        }

        public Document GetAsDoc() {
            if (htmlDoc  is null) {
                string baseUri = HttpUtil.GetBaseUrl(Endpoint);

                using (MemoryStream memStream = new MemoryStream()) {
                    byte[] buffer = Encoding.UTF8.GetBytes(GetResponseContent());
                    memStream.Write(buffer, 0, buffer.Length);
                    htmlDoc = Dcsoup.Parse(memStream, "UTF-8", baseUri, Parser.HtmlParser);
                }
                /*
                 * try {
    			doc = Jsoup.parse(new String(html.getBytes(), "UTF-8"), baseUri);
    			doc.outputSettings().charset("UTF-8");
    			
    		} catch (UnsupportedEncodingException ex2) {
    			ex2.printStackTrace();
				doc = Jsoup.parse(html, baseUri);
			}
                 */
            }
            return htmlDoc;
        }

        public IJsonDocumentReader GetJsonReader() {
            //return JsonIO.ParseString(GetResponseContent(), );
            return null; //TODO
        }

        public string GetLocation() {
            return ResponseHeaders.Get(HeaderKeys.Location).Value;
        }

        public override string ToString() {
            return "[endpoint: " + Endpoint + "][status: " + ResponseStatus + "]";
        }

    }
}
