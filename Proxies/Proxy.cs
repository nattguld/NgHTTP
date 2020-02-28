using NgHTTP.Proxies.Authentication;
using NgHTTP.Util;
using System;
using System.Net.Sockets;

namespace NgHTTP.Proxies {
    public class Proxy {

        public ProxyType ProxyType {get;}

        public string Host { get; }

        public int Port { get; }

        public NetworkType NetworkType { get; set; }

        public AddressFamily AddressFamily { get; set; }

        public bool IsStaticIP { get; set; }

        public ProxyState ProxyState { get; set; }

        public ProxyCredentials ProxyCreds { get; set; }

        public bool HasAuthentication => ProxyCreds != null;

        public string Address => Host + ":" + Port;

        public Proxy(string host, int port) : this(ProxyType.Http, host, port) { }

        public Proxy(ProxyType proxyType, string host, int port) : this(proxyType, host, port, null) { }

        public Proxy(string host, int port, ProxyCredentials proxyCreds) : this(ProxyType.Http, host, port, proxyCreds) { }

        public Proxy(ProxyType proxyType, string host, int port, ProxyCredentials proxyCreds) {
            ProxyType = proxyType;
            Host = host;
            Port = port;
            ProxyCreds = proxyCreds;
            ProxyState = ProxyState.Online;
            NetworkType = NetworkType.Datacenter;
            AddressFamily = AddressFamily.InterNetwork;
            IsStaticIP = true;
        }

        public override bool Equals(object obj) {
            if (obj is null || !(obj is Proxy)) {
                return false;
            }
            Proxy otherProxy = obj as Proxy;

            return otherProxy.ProxyType == ProxyType && otherProxy.Host.Equals(Host, StringComparison.OrdinalIgnoreCase)
                && otherProxy.Port == Port;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return Address + (HasAuthentication ? (":" + ProxyCreds.ToString()) : "");
        }

    }
}
