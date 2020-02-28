using System;
using System.Collections.Generic;
using System.Text;

namespace NgHTTP.Requests.Responses.Listeners {
    public abstract class ProgressListener {

        public int Progress { get; private set; } = -1;


        protected abstract void OnChange(int progress);

        public ProgressListener SetProgress(int progress) {
            if (Progress == progress) {
                return this;
            }
            Progress = progress;
            OnChange(progress);
            return this;
        }

    }
}
