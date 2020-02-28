using NgHTTP.Browser;
using NgHTTP.Headers;
using NgHTTP.Proxies;
using NgHTTP.Requests;
using NgHTTP.Requests.Responses.Decoders.Impl;
using NgHTTP.Util;
using NgUtil.Debugging.Logging;
using NgUtil.Generics.Kvps.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace NgHTTP.Sockets.Http {
    public static class HttpSocketFactory {

        private static readonly Logger logger = Logger.GetLogger("HttpSocketFactory");

        public static List<string> SslHosts = new List<string>();


        public static HttpSocket Connect(Proxy proxy, string hostAddress, int port, BrowserConfig browserCfg, bool ssl) {
            if (proxy == ProxyManager.InvalidProxy) {
                throw new Exception("Invalid proxy");
            }
            if (proxy == ProxyManager.LocalHost) {
                proxy = null;
            }
            logger.Debug(string.Format("Socket connect requested ({0}:{1}, proxy: {2}, SSL: {3})", hostAddress, port, proxy != null, ssl));
            
            return ssl 
                ? SecureConnect(proxy, hostAddress, port, browserCfg) 
                : InsecureConnect(proxy, hostAddress, port, browserCfg);
        }

        private static HttpSocket InsecureConnect(Proxy proxy, string hostAddress, int port, BrowserConfig browserCfg) {
            logger.Debug(string.Format("Connecting socket ({0}:{1}, proxy => {2})", hostAddress, port, proxy != null));

            Socket socket = proxy is null 
                ? ConnectToServerHost(hostAddress, port, browserCfg) 
                : ConnectToProxyHost(proxy, browserCfg);

            if (socket is null || !socket.Connected) {
                return null;
            }
            HttpSocket httpSocket = new HttpSocket(socket, browserCfg.ReadTimeout * 1000, browserCfg.WriteTimeout * 1000);

            if (proxy != null && proxy.HasAuthentication) {
                PerformTunnelHandshake(httpSocket, proxy, hostAddress, port, browserCfg);
            }
            logger.Debug(string.Format("Successfully connected socket ({0}:{1}, proxy => {2})", hostAddress, port, proxy != null));
            return httpSocket;
        }

        private static HttpSocket SecureConnect(Proxy proxy, string hostAddress, int port, BrowserConfig browserCfg) {
            logger.Debug(string.Format("Connecting SSL socket ({0}:{1}, proxy => {2})", hostAddress, port, proxy != null));

            Socket socket = proxy is null 
                ? ConnectToServerHost(hostAddress, port == 80 ? 443 : port, browserCfg) 
                : ConnectToProxyHost(proxy, browserCfg);

            if (socket is null || !socket.Connected) {
                return null;
            }
            HttpSocket httpSocket = new HttpSocket(socket, browserCfg.ReadTimeout * 1000, browserCfg.WriteTimeout * 1000);

            if (proxy != null) {
                PerformTunnelHandshake(httpSocket, proxy, hostAddress, 443, browserCfg);
            }
            httpSocket.OpenSslStream();

            try {
                httpSocket.SslStream.AuthenticateAsClient(hostAddress);

            } catch (IOException ex) {
                Console.WriteLine(ex.ToString());
            }
            if (!httpSocket.SslStream.IsAuthenticated) {
                Console.WriteLine("not authenticated");
                return null;
            }
            logger.Debug(string.Format("Successfully connected SSL socket ({0}:{1}, proxy => {2})", hostAddress, port, proxy != null));
            
            if (!SslHosts.Contains(hostAddress)) {
                SslHosts.Add(hostAddress);
            }
            return httpSocket;
        }

        private static void PerformTunnelHandshake(HttpSocket httpSocket, Proxy proxy, string hostAddress, int port, BrowserConfig browserCfg) {
            StringStringKeyValuePairContainer headersContainer = new StringStringKeyValuePairContainer();
            headersContainer.Put(HeaderKeys.Host, hostAddress + ":" + port);
            headersContainer.Put(HeaderKeys.UserAgent, browserCfg.UserAgent);
            headersContainer.Put(HeaderKeys.Connection, HeaderValues.ConnectionKeepAlive);

            if (proxy != null && proxy.HasAuthentication) {
                headersContainer.Put(HeaderKeys.ProxyConnection, HeaderValues.ConnectionKeepAlive);
                headersContainer.Put(HeaderKeys.ProxyAuthorization, "Basic " + proxy.ProxyCreds.GetBase64Auth());
            }
            httpSocket.WriteLine(RequestType.Connect.Notation + " " + hostAddress + ":" + port + " " + browserCfg.HttpVersion.Notation);

            foreach (StringStringKeyValuePair kvp in headersContainer.Kvps) {
                httpSocket.WriteLine(kvp.Key + ": " + kvp.Value);
            }
            httpSocket.WriteLine();
            httpSocket.FlushUnderlying();

            HttpHeaderDecoder hd = new HttpHeaderDecoder();
            hd.Decode(httpSocket.Stream);

            if (hd.ResponseStatus.HttpCode != HttpCode.Ok) {
                throw new IOException("Unable to tunnel through proxy => " + hd.ResponseStatus);
            }
            logger.Debug("Successfully tunneled through proxy");
        }

        private static Socket ConnectToProxyHost(Proxy proxy, BrowserConfig browserCfg) {
            IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Parse(proxy.Host), proxy.Port);

            Socket socket = new Socket(proxy.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SetSocketConfigs(socket, browserCfg);

            socket.Connect(ipEndpoint);

            if (!socket.Connected) {
                logger.Debug(string.Format("Failed to connect proxy socket ({0}:{1}, type => {2})", proxy.Host, proxy.Port, proxy.AddressFamily));
                return null;
            }
            return socket;
        }

        private static Socket ConnectToServerHost(string hostAddress, int port, BrowserConfig browserCfg, AddressFamily addressFamily = AddressFamily.Unspecified) {
            IPHostEntry hostEntry = Dns.GetHostEntry(hostAddress);

            foreach (IPAddress ipAddress in hostEntry.AddressList) {
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

                if (addressFamily != AddressFamily.Unspecified && ipEndpoint.AddressFamily != addressFamily) {
                    continue;
                }
                Socket socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                SetSocketConfigs(socket, browserCfg);

                socket.Connect(ipEndpoint);

                if (socket.Connected) {
                    logger.Debug(string.Format("Connected to IP endpoint ({0}:{1}, type => {2})", ipEndpoint.Address, port, ipEndpoint.AddressFamily));
                    return socket;
                }
            }
            logger.Debug(string.Format("Failed to connect to remote server ({0}:{1})", hostAddress, port));
            return null;
        }

        private static void SetSocketConfigs(Socket socket, BrowserConfig browserCfg) {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //socket.LingerState = new LingerOption(true, 10); The socket will linger for 10 seconds after socket.close is called
            socket.NoDelay = true;
            socket.SendTimeout = browserCfg.WriteTimeout * 1000;
            socket.ReceiveTimeout = browserCfg.ReadTimeout * 1000;
            socket.SendBufferSize = 65536;
            socket.ReceiveBufferSize = 65536;
            socket.ExclusiveAddressUse = false;
        }

    }
}
