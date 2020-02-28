using System;

namespace NgHTTP.Requests.Responses.Interpretors {
    public abstract class BaseResponseInterpretor {
        public int Progress => progress;

        protected long BodySize { get; }

        private volatile int progress; //No need for Interlocked as 1 thread only writes & 1 only reads


        public BaseResponseInterpretor(long bodySize) {
            BodySize = bodySize;
        }

        protected int AddProgress(int increment) {
            progress += increment;
            return progress;
        }

        public int GetProgressPercentage() {
            return (int)Math.Round(((double)((double)Progress / (double)BodySize * 100)));
        }

    }
}
