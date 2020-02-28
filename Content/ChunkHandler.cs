using NgHTTP.Configs;
using NgHTTP.Streams;
using NgUtil.Files;
using System;
using System.IO;

namespace NgHTTP.Content {
    public sealed class ChunkHandler : IDisposable {

        public FileLink FileLink { get; }

        private readonly long fileSize;

        private readonly FileStream fileInputStream;

        private long chunks;

        private int chunkId;

        public int ChunkSize { get; private set; }

        private long restChunkSize;

        public int StartRange { get; private set; }

        public long EndRange { get; private set; }

        public int Progress => (int)Math.Round(((double)bytesSent / (double)fileSize) * 100);

        public bool IsFinished => bytesSent >= fileSize;

        private int bytesSent;


        public ChunkHandler(FileLink fileLink) {
            FileLink = fileLink;
            fileSize = ((FileInfo)fileLink.GetInfo()).Length;
            chunks = fileSize <= HttpConfig.ChunkSize ? 1 : (fileSize / HttpConfig.ChunkSize);
            fileInputStream = new FileStream(fileLink.Path, FileMode.Open);

            if (chunks > 1) {
                restChunkSize = fileSize % (chunks * HttpConfig.ChunkSize);

                if (restChunkSize > 0) {
                    chunks++;
                }
            }
        }

        public ChunkHandler PrepareTransfer() {
            ChunkSize = HttpConfig.ChunkSize;
            StartRange = chunkId * ChunkSize;
            EndRange = ((chunkId + 1) * ChunkSize) - 1;

            if ((chunkId + 1) == chunks) {
                EndRange = fileSize - 1;
                ChunkSize = (int)restChunkSize;
            }
            return this;
        }

        public ChunkHandler WriteChunk(IHttpStreamable httpStream) {
            byte[] buffer = new byte[ChunkSize];
            fileInputStream.Read(buffer, 0, buffer.Length);

            httpStream.Write(buffer);

            bytesSent += ChunkSize;
            chunkId++;
            return this;
        }

        public void Dispose() {
            fileInputStream.Dispose();
        }
    }
}
