using System.Collections.Generic;

namespace BlankProject
{
    public static class WorkerUtils
    {
        public const string UnityClient = "UnityClient";
        public const string UnityGameLogic = "UnityGameLogic";
        public const string AndroidClient = "AndroidClient";
        public const string iOSClient = "iOSClient";

        public static readonly List<string> AllWorkerAttributes =
            new List<string>
            {
            UnityGameLogic,
            UnityClient,
            AndroidClient,
            iOSClient
            };
    }
}
