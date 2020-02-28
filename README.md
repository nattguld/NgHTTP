# NgHTTP
An advanced Net.Core HTTP library covering all aspects of HTTP communication such as SSL, proxies, session etc... Written with web automation in mind.

## Dependencies
This repository uses the following dependencies:  
**NgUtil:** For various utility and helper methods. https://github.com/nattguld/Net.Core.NgUtil  
**dcsoup** For parsing HTML documents. https://github.com/matarillo/dcsoup  
**jint (optional)** For parsing javascript, this is optional in the scenario where a javascript equation must be solved and sent back to the web server. https://github.com/sebastienros/jint 

## About
NgHTTP is a library built on raw sockets for maximum customizibility. This project was made with the focus on web automation. There's user agents and other browser mimic functionality built in and every request can be built exactly how you want it. You'll rarely find yourself needing to extend on existing code as most situations are already managed by default and customization options are present. Next to the default plain text & gzip also brotli encoded responses can be handled. Cookies are managed per HTTP client instance to allow you to create complete automation flows. Below you can find some basic example usage. You should have no troubles with advanced usage as everything is implemented in the default requests & methods. Response bodies have built-in support for parsing HTML documents, plain text or JSON.

## Examples
### HttpSessions
There is a base class you can derive from to create custom sessions "HttpSession".
2 types of sessions are available by default, WebSession & AppSession.
WebSession emulates browser behavior while AppSession emulates in-app behavior.
SSL is automatically handled by listening to the webserver and upgrading to SSL when required.
```csharp
using (WebSession session = new WebSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy)) { }
using (AppSession session = new AppSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy)) { }
```

### Use of proxies
The session support HTTP proxies, proxy authentication, proxy tunneling & SSL. All you would need to automation scenarios.
If you have advanced proxy setups you can modify them in detail by changing the properties as shown in the example below:
```csharp
//Standard HTTP(S) proxy
Proxy proxy = new Proxy("127.0.0.1", 8888);

//Advanced settings
Proxy proxy = new Proxy("127.0.0.1", 8888) {
	ProxyType = ProxyType.HTTPS,
	NetworkType = NetworkType.Cellular,
	AddressFamily = AddressFamily.InterNetwork,
	ProxyCredentials = new ProxyCredentials("username", "password")
};
```

You can also parse proxies from a string like this:
```csharp
Proxy proxy = ProxyManager.Parse("127.0.01:8888");
Proxy proxy = ProxyManager.Parse("127.0.01:8888:username:password");
```

### HTTP GET & Download example
```csharp
using (WebSession c = new WebSession()) {
    RequestResponse rr = c.DispatchRequest(new GetRequest("https://github.com/randqm/");
    
    if (!rr.Validate()) {
      Console.WriteLine("Unexpected response code: " + rr.ResponseCode);
      return;
    }
    Document doc = rr.GetAsDoc();
    String content = rr.GetResponseContent();
    String endpoint = rr.Endpoint;
    Headers headers = rr.ResponseHeaders;
    
    FileLink dl = c.Download("savepath", new GetRequest("url"));
}
```

### HTTP POST example
```csharp
//Form
FormBody fb = new FormBody();
fb.Add("key", "value");
fb.Add("file", file);
fb.Add("number", 1337);

//Multipart
MultipartBody mb = new MultipartBody();
mb.Add("key", "value");
mb.Add("file", file);
mb.Add("bytes", new byte[]{});
mb.Add("number", 1337);

//Stream
new StreamBody(FileLink f);
//Json
new StringBody(EncType.Json, jsonStr);
//XML
new StringBody(EncType.Xml, xmlStr);
//Plain text
new StringBody(EncType.PlainText, xmlStr);

//Request
RequestResponse rr = c.DispatchRequest(new PostRequest("url", body));
```

### Custom request example
```csharp
Request request = new Request("url", expectedResponseCode, body, RequestProperty.XMLHttpRequest) {
    ResponseEncType = EncType.Json
};
request.CustomHeaders.Put("customkey", "customvalue");

RequestResponse rr = c.DispatchRequest(request);
```
