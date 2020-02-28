using NgHTTP.Proxies.Authentication;
using NgHTTP.Util;
using NgUtil.Debugging.Contracts;
using NgUtil.Files;
using NgUtil.Files.IO;
using NgUtil.Maths;
using System;
using System.Collections.Generic;

namespace NgHTTP.Proxies.Data {
    public static class ProxyIO {


        public static void LoadProxiesFromJson(string filePath) {
            List<Proxy> proxies = JsonIO.DeserializeObject<List<Proxy>>(filePath);
            ProxyManager.Proxies.Clear();
            ProxyManager.Proxies.AddRange(proxies);
        }

        public static void SaveProxiesToJson(string filePath) {
            JsonIO.SaveObject(filePath, ProxyManager.Proxies);
        }

        public static void LoadProxiesFromTextFile(string filePath, ProxyType proxyType = ProxyType.Http, NetworkType netType = NetworkType.Datacenter) {
            EmptyParamContract.Validate(filePath);
            LoadProxiesFromTextFile(new FileLink(filePath), proxyType, netType);
        }

        public static void LoadProxiesFromTextFile(FileLink fileLink, ProxyType proxyType = ProxyType.Http, NetworkType netType = NetworkType.Datacenter) {
            EmptyParamContract.Validate(fileLink);

            foreach (string line in TextIO.GetLines(fileLink.Path)) {
                Proxy proxy = Parse(line, proxyType, netType);

                if (proxy != null) {
                    ProxyManager.AddProxy(proxy, false);
                }
            }
        }

        public static void AppendProxyToTextFile(string filePath, Proxy proxy) {
            EmptyParamContract.Validate(filePath);

            AppendProxyToTextFile(new FileLink(filePath), proxy);
        }

        public static void AppendProxyToTextFile(FileLink fileLink, Proxy proxy) {
            EmptyParamContract.Validate(fileLink);
            EmptyParamContract.Validate(proxy);

            TextIO.WriteAsync(fileLink.Path, proxy.ToString(), true);
        }

        public static void WriteProxiesToTextFile(string filePath) {
            EmptyParamContract.Validate(filePath);
            WriteProxiesToTextFile(new FileLink(filePath));
        }

        public static void WriteProxiesToTextFile(FileLink fileLink) {
            EmptyParamContract.Validate(fileLink);

            if (fileLink.Exists()) {
                FileHandler.Delete(fileLink);
            }
            foreach (Proxy proxy in ProxyManager.Proxies) {
                AppendProxyToTextFile(fileLink, proxy);
            }
            Console.WriteLine(ProxyManager.Proxies.Count + " proxies written to " + fileLink.Path);
        }

        public static Proxy Parse(string input, ProxyType proxyType = ProxyType.Http, NetworkType netType = NetworkType.Datacenter) {
            EmptyParamContract.Validate(input != null);

            input = input.Trim();

            if (string.IsNullOrEmpty(input)) {
                Console.WriteLine("Unable to parse proxy from empty input");
                return null;
            }
            if (input.Equals("localhost", StringComparison.OrdinalIgnoreCase)) {
                return ProxyManager.LocalHost;
            }
            if (!input.Contains(":", StringComparison.Ordinal)) {
                Console.WriteLine("Unable to parse proxy, invalid format => " + input);
                return null;
            }
            string[] parts = input.Split(":");

            if (parts.Length < 2 || parts.Length > 5) {
                Console.WriteLine("Unable to parse proxy, invalid amount of parts => " + parts);
                return null;
            }
            string host = parts[0].Trim();

            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase)) {
                return ProxyManager.LocalHost;
            }
            int hostPartsSize = host.Split("\\.").Length;

            if (hostPartsSize < 2 || hostPartsSize > 4) {
                Console.WriteLine("Unable to parse proxy, invalid host => " + host);
                return null;
            }
            string portStr = parts[1].Trim();
            int port = MathUtil.ParseInt(portStr);

            if (port == 0) {
                Console.WriteLine("Unable to parse proxy, invalid port => " + portStr);
                return null;
            }
            if (host.Equals("127.0.0.1") && port == 8888) {
                return ProxyManager.FiddlerProxy;
            }
            Proxy proxy = new Proxy(proxyType, host, port);
            proxy.NetworkType = netType;

            if (parts.Length > 4) {
                proxy.ProxyCreds = new ProxyCredentials(parts[2].Trim(), parts[3].Trim());
            }
            return proxy;
        }

    }
}
