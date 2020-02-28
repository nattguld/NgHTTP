using NgHTTP.Configs;
using NgHTTP.Proxies.Authentication;
using NgHTTP.Util;
using NgUtil.Debugging.Contracts;
using NgUtil.Files;
using NgUtil.Files.IO;
using NgUtil.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgHTTP.Proxies {
    public static class ProxyManager {

        public static readonly Proxy InvalidProxy = new Proxy("Invalid", 0);

        public static readonly Proxy FiddlerProxy = new Proxy("127.0.0.1", 8888);

        public static readonly Proxy LocalHost = new Proxy("localhost", 80);

        public static readonly List<Proxy> Proxies = new List<Proxy>();


        public static void AddProxy(Proxy proxy, bool allowDuplicates = false) {
            EmptyParamContract.Validate(proxy);

            if (!allowDuplicates) {
                Proxy exists = GetByAddress(proxy.Address);

                if (exists != null) {
                    return;
                }
            }
            Proxies.Add(proxy);
        }

        public static void RemoveProxy(Proxy proxy) {
            EmptyParamContract.Validate(proxy);

            Proxies.Remove(proxy);
        }

        public static Proxy GetProxyByChoice(NetworkType netType, bool staticIP = true, bool allowNoProxy = false) {
            if (HttpConfig.FiddlerMode) {
                return FiddlerProxy;
            }
            List<Proxy> filtered = new List<Proxy>();

            foreach (Proxy proxy in Proxies) {
                if (proxy.NetworkType != netType) {
                    continue;
                }
                if (proxy.IsStaticIP != staticIP) {
                    continue;
                }
                filtered.Add(proxy);
            }
            if (filtered.Count == 0) {
                if (!allowNoProxy) {
                    throw new Exception("No proxies found for given choices");
                }
                Console.WriteLine("No proxies found for given choices");
                return null;
            }
            return filtered[MathUtil.Random(filtered.Count)];
        }

        public static Proxy GetBestAvailableProxy(bool allowNoProxy = false) {
            Proxy proxy = GetProxyByChoice(NetworkType.Residential, false);

            if (proxy != null) {
                return proxy;
            }
            proxy = GetProxyByChoice(NetworkType.Residential, true);

            if (proxy != null) {
                return proxy;
            }
            proxy = GetProxyByChoice(NetworkType.Datacenter, false);

            if (proxy != null) {
                return proxy;
            }
            proxy = GetProxyByChoice(NetworkType.Datacenter, true);

            if (proxy != null) {
                return proxy;
            }
            if (!allowNoProxy) {
                throw new Exception("No best available proxy found");
            }
            return null;
        }

        public static Proxy GetByAddress(string host, string port) {
            EmptyParamContract.Validate(host);
            EmptyParamContract.Validate(port);

            return GetByAddress(host + ":" + port);
        }

        public static Proxy GetByAddress(string address) {
            EmptyParamContract.Validate(address);

            foreach (Proxy proxy in Proxies) {
                if (proxy.Address.Equals(address, StringComparison.OrdinalIgnoreCase)) {
                    return proxy;
                }
            }
            return null;
        }

        public static List<Proxy> GetByProxyType(ProxyType proxyType) {
            EmptyParamContract.Validate(proxyType);
            List<Proxy> filtered = new List<Proxy>();

            foreach (Proxy proxy in Proxies) {
                if (proxy.ProxyType == proxyType) {
                    filtered.Add(proxy);
                }
            }
            return filtered;
        }

        public static List<Proxy> GetByNetworkType(NetworkType netType) {
            EmptyParamContract.Validate(netType);
            List<Proxy> filtered = new List<Proxy>();

            foreach (Proxy proxy in Proxies) {
                if (proxy.NetworkType == netType) {
                    filtered.Add(proxy);
                }
            }
            return filtered;
        }

        public static List<Proxy> GetByState(ProxyState proxyState) {
            EmptyParamContract.Validate(proxyState);
            List<Proxy> filtered = new List<Proxy>();

            foreach (Proxy proxy in Proxies) {
                if (proxy.ProxyState == proxyState) {
                    filtered.Add(proxy);
                }
            }
            return filtered;
        }

    }
}
