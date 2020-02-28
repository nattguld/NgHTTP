using NgHTTP.Configs;
using NgHTTP.Headers;
using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil.Files;
using NgUtil.Generics.Kvps.Impl;
using NgUtil.Text;
using System;
using System.IO;
using System.Text;

namespace NgHTTP.Content.Bodies {
    public sealed class MultipartBody : ContentBody {

        private readonly AttributeKeyValuePairContainer kvpContainer;

        public string Boundary { get; set; }

        public bool NoMediaExtension { get; set; }

        private long fileSize;


        public MultipartBody() : base(EncType.Multipart) {
            kvpContainer = new AttributeKeyValuePairContainer();
            Boundary = "----WebKitFormBoundary" + TextUtil.RandomString(16, 16
                , ETextSeed.Digits, ETextSeed.Lowercase, ETextSeed.Uppercase);
        }

        public MultipartBody Add(string key, object value, bool noReplace = false) {
            if (!noReplace) {
                AttributeKeyValuePair exists = kvpContainer.Get(key);

                if (exists != null) {
                    kvpContainer.Remove(exists);
                }
            }
            kvpContainer.Put(new AttributeKeyValuePair(key, value));
            return this;
        }

        protected override void SetContentHeaders(StringStringKeyValuePairContainer headers) {
            base.SetContentHeaders(headers);

            headers.Put(HeaderKeys.ContentType, EncType.ContentValue + "; boundary=" + Boundary);
        }

        protected override void SetContentLength(long contentLength) {
            base.SetContentLength(contentLength + fileSize);
        }

        protected override void Build(IHttpStreamable httpStream, bool prepare) {
            fileSize = 0;
            bool lastEntryWasFile = false;

            foreach (AttributeKeyValuePair kvp in kvpContainer.Kvps) {
                string key = Encoding.UTF8.GetString(Encoding.Default.GetBytes(kvp.Key));
                lastEntryWasFile = true;

                httpStream.WriteLine("--" + Boundary);

                if (kvp.Value is null) {
                    WriteOctetStreamPart(httpStream, key);
                    continue;
                }
                if (kvp.Value is byte[]) {
                    WriteOctetStreamPart(httpStream, prepare, key, (byte[])kvp.Value);
                    fileSize += ((byte[])kvp.Value).Length;
                    continue;
                }
                if (kvp.Value is FileLink) {
                    if (Chunked) {
                        if (ChunkHandler is null) {
                            ChunkHandler = new ChunkHandler((FileLink)kvp.Value);
                        }
                        ChunkHandler.PrepareTransfer();
                        fileSize += ChunkHandler.ChunkSize;
                    } else {
                        fileSize += ((FileInfo)((FileLink)kvp.Value).GetInfo()).Length;
                    }
                    WriteFilePart(httpStream, prepare, key, (FileLink)kvp.Value);
                    continue;
                }
                string value = kvp.Value is string ? (string)kvp.Value : kvp.Value.ToString();
                value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(value));

                lastEntryWasFile = false;

                WriteStringPart(httpStream, key, value);
            }
            if (lastEntryWasFile) {
                httpStream.WriteLine();
                httpStream.FlushUnderlying();
            }
            httpStream.WriteLine("--" + Boundary + "--");
        }

        public override AttributeKeyValuePairContainer GetContent<AttributeKeyValuePairContainer>() {
            return (AttributeKeyValuePairContainer)Convert.ChangeType(kvpContainer, typeof(AttributeKeyValuePairContainer));
        }

        private void WriteStringPart(IHttpStreamable httpStream, string key, string value) {
            httpStream.WriteLine("Content-Disposition: form-data; name=\"" + key + "\"");
		    //httpStream.writeLine("Content-Type: text/plain; charset=UTF-8");
		    httpStream.WriteLine();
		    httpStream.WriteLine(value);
		    httpStream.FlushUnderlying();
	    }

        private void WriteFilePart(IHttpStreamable httpStream, bool prepare, string key, FileLink value) {
            FileInfo fileInfo = (FileInfo)value.GetInfo();
            string fileName = fileInfo.Name;

            if (NoMediaExtension) {
                fileName = StringFunc.Substring(fileName, 0, fileName.IndexOf(fileInfo.Extension));
            }
            httpStream.WriteLine("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + fileName + "\"");
            httpStream.WriteLine("Content-Type: " + MimeType.GetByFile(value).Notation);

            if (!Chunked) {
                httpStream.WriteLine("Content-Transfer-Encoding: binary");
            }
            httpStream.WriteLine();
            httpStream.FlushUnderlying();

            if (!prepare) {
                WriteFile(httpStream, value);
            }
            httpStream.WriteLine();
            httpStream.FlushUnderlying();
        }

        private void WriteFile(IHttpStreamable httpStream, FileLink fileLink) {
            if (Chunked) {
                ChunkHandler.WriteChunk(httpStream);
                httpStream.FlushUnderlying();

                if (ChunkHandler.IsFinished) {
                    ChunkHandler.Dispose();
                }
                return;
            }
            using (FileStream fis = new FileStream(fileLink.Path, FileMode.Open)) {
                int bytesRead = -1;
                byte[] buffer = new byte[HttpConfig.ChunkSize];

                while ((bytesRead = fis.Read(buffer)) != -1) {
                    httpStream.WriteUnderlying(buffer, 0, bytesRead);
                }
                httpStream.FlushUnderlying();
            }
        }

        private void WriteOctetStreamPart(IHttpStreamable httpStream, bool prepare, string key, byte[] payload) {
            httpStream.WriteLine("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"blob\"");
            httpStream.WriteLine("Content-Type: application/octet-stream");
            httpStream.WriteLine();

            if (!prepare) {
                httpStream.Write(payload);
            }
            httpStream.WriteLine();
            httpStream.FlushUnderlying();
        }

        private void WriteOctetStreamPart(IHttpStreamable httpStream, string key) {
            httpStream.WriteLine("Content-Disposition: form-data; name=\"" + key + "\"; filename=\"\"");
            httpStream.WriteLine("Content-Type: application/octet-stream");
            httpStream.WriteLine();
            httpStream.WriteLine();
            httpStream.FlushUnderlying();
        }

    }
}
