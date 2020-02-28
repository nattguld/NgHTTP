using NgHTTP.Util;
using NgUtil.Systems;
using System;
using System.Text;

namespace NgHTTP.Configs {
    public static class HttpConfig {

        public static bool FiddlerMode { get; set; } = false;

        public static bool DeveloperMode { get; set; } = true;

        public static bool DebugMode { get; set; } = true;

        public static bool DoNotTrack { get; set; } = false;

        public static int SendTimeout { get; set; } = 5;

        public static int ReadTimeout { get; set; } = 15;

        public static int ChunkSize { get; set; } = 11534335;

        public static int MaxConnection { get; set; } = SystemUtil.GetCPUCores() * 50;

    }
}
