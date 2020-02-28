using NgUtil.Files;
using System;

namespace NgHTTP.Requests.Responses.Bodies.Impl {
    public class FileResponseBody : IResponseBody<FileLink> {

        private readonly FileLink fileLink;


        public FileResponseBody(FileLink fileLink) {
            this.fileLink = fileLink;
        }

        /*public FileLink GetBody<FileLink>() {
            return (FileLink)Convert.ChangeType(fileLink, typeof(FileLink));
        }*/

        public FileLink GetBody() {
            return fileLink;
        }

    }
}
