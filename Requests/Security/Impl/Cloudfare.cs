using NgHTTP.Requests.Impl;
using NgHTTP.Requests.Responses;
using NgHTTP.Scripting;
using NgHTTP.Sessions;
using NgHTTP.Storage;
using NgUtil;
using NgUtil.Text;
using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgHTTP.Requests.Security.Impl {
    public class Cloudfare : IConnectionSecurity {

        public const int CloudfarereResponseCode = 503;


        public bool Encountered(HttpSession<Application> session, Request req, RequestResponse rr) {
            return rr.Validate(CloudfarereResponseCode);
        }

        public bool Bypass(HttpSession<Application> session, Request req, RequestResponse rr) {
			string url = req.Url;
			string baseUrl = url.EndsWith("/") ? StringFunc.Substring(url, 0, url.Length - 1) : url;
			string domainName = baseUrl.Split("/")[2];

			string resolveUrl = ResolveUrl(rr.GetAsDoc(), domainName, baseUrl);

			if (string.IsNullOrEmpty(resolveUrl)) {
				return false;
			}
			Misc.Sleep(5500);

			rr = session.DispatchRequest(new GetRequest(resolveUrl));

			if (rr.Validate(200)) {
				return true;
			}
			if (rr.Validate(403)) {
				//String captcha = new CaptchaService().recaptchaV2(c, "6LfBixYUAAAAABhdHynFUIMA_sa4s-XsJvnjtgB0", resolveUrl);
				//String url = "https://" + domainName + "/cdn-cgi/l/chk_captcha";

				Console.WriteLine("CAPTCHA ON CLOUDFARE!");
				return false;
			}
			Console.WriteLine("Unexpected response (" + rr.ResponseCode + ")");
			return false;
		}

        public string GetName() {
            return "Cloudfare";
        }

		private static string ResolveUrl(Document doc, string domainName, string baseUrl) {
			try {
				Element jschlEl = doc.Select("[name=jschl_vc]").First;

				if (jschlEl == null) {
					Console.WriteLine("Failed to extract jsch element");
					return null;
				}
				string jschl_vc = jschlEl.Attr("value");
				string pass = doc.Select("[name=pass]").First.Attr("value");

				Element scriptEl = doc.GetElementsByTag("script").First;

				if (scriptEl == null) {
					Console.WriteLine("Failed to extract script element");
					return null;
				}
				string scriptHtml = scriptEl.Html;
				string content = StringFunc.Substring(scriptHtml, scriptHtml.IndexOf("setTimeout(function(){") + 22, scriptHtml.Length).Trim();

				string part1 = StringFunc.Substring(content, 31, content.IndexOf("};") + 2); //@INFO: The first equation initially added to the builder var

				string[] part1Split = part1.Split("=");
				string objName = part1Split[0];
				string objKey = StringFunc.Substring(part1Split[1], 2, part1Split[1].IndexOf(":") - 1);
				string objVar = objName + "." + objKey; //@INFO: The var of the object to use (name.key)

				string equationBlock = StringFunc.Substring(content, content.IndexOf(";" + objVar), content.IndexOf("a.value")); //The equation block

				double equationResult = (double)Javascript.ExecuteFunction("function test() { " + part1 + "" + equationBlock + " var aval = +" + objVar + ".toFixed(10); return aval;}", "test");
				//System.out.println(equationResult);
				double result = equationResult + domainName.Length;
				//System.out.println(result);
				string formatResult = string.Format("%.10f", result).Replace(",", ".");

				return baseUrl + "/cdn-cgi/l/chk_jschl?jschl_vc=" + jschl_vc + "&pass=" + pass + "&jschl_answer=" + formatResult;

			} catch (Exception ex) {
				Console.WriteLine(ex.StackTrace);
			}
			return null;
		}

	}
}
