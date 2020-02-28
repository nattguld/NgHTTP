using NgHTTP.Streams;
using NgUtil.Text;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NgHTTP.Sockets.Http {
    public sealed class HttpSocket : IDisposable, IHttpStreamable {

        public Socket Socket { get; }

        public NetworkStream NetStream { get; }

        public SslStream SslStream { get; private set; }

        public Stream Stream => SslStream is null ? NetStream : (Stream)SslStream;

        private readonly int readTimeoutMs;

        private readonly int writeTimeoutMs;



        public HttpSocket(Socket socket, int readTimeoutMs, int writeTimeoutMs) {
            Socket = socket;

            this.readTimeoutMs = readTimeoutMs;
            this.writeTimeoutMs = writeTimeoutMs;

            NetStream = new NetworkStream(socket, false) {
                ReadTimeout = readTimeoutMs,
                WriteTimeout = writeTimeoutMs
            };
        }

        public HttpSocket OpenSslStream() {
            SslStream = new SslStream(NetStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null) {
                ReadTimeout = readTimeoutMs,
                WriteTimeout = writeTimeoutMs
            };
            return this;
        }

        public IHttpStreamable AppendString(string value) {
            return Write(Encoding.UTF8.GetBytes(value));
        }

        public IHttpStreamable WriteLine(string value) {
            return AppendString(value).WriteLine();
        }

        public IHttpStreamable WriteLine() {
            return AppendString(Delimiters.Linebreak);
        }

        public IHttpStreamable Write(byte[] buffer) {
            return WriteUnderlying(buffer, 0, buffer.Length);
        }

        public IHttpStreamable WriteUnderlying(byte[] buffer, int offset, int length) {
            Stream.Write(buffer, offset, length);
            return this;
        }

        public IHttpStreamable FlushUnderlying() {
            Stream.Flush();
            return this;
        }

        public void Dispose() {
            if (SslStream != null) {
                SslStream.Dispose();
                SslStream.Close();
            }
            NetStream.Dispose();
            NetStream.Close();

            Socket.Dispose();
            Socket.Close();
        }

        private static bool ValidateServerCertificate(object sender,
           X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (sslPolicyErrors == SslPolicyErrors.None) {
                return true;
            }
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

    }
}
