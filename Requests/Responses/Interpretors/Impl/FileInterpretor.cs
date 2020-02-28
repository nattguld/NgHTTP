using NgHTTP.Requests.Responses.Bodies.Impl;
using NgUtil.Files;
using System;
using System.IO;

namespace NgHTTP.Requests.Responses.Interpretors.Impl {
    public sealed class FileInterpretor : ResponseInterpretor<FileResponseBody> {

		public readonly string savePath;


        public FileInterpretor(long bodySize, string savePath) : base(bodySize) {
            this.savePath = savePath;
        }

        public override FileResponseBody Interpret(Stream stream) {
			int read = 0;

			using (FileStream fileOutputStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write)) {
				byte[] dataBuffer = new byte[4096];
				int bytesRead = 0;

				while ((bytesRead = stream.Read(dataBuffer, 0, 4096)) != -1) {
					fileOutputStream.Write(dataBuffer, 0, bytesRead);
					read += bytesRead;

					AddProgress(bytesRead);

					if (read == BodySize) {
						break;
					}
				}
				//return (FileResponseBody)Activator.CreateInstance(typeof(FileResponseBody), new object[] { new FileLink(savePath)});
				return new FileResponseBody(new FileLink(savePath));
			}
		}

    }
}
