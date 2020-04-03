using UnityEngine;

namespace TeamZero.Core.Unity
{
    public static class SystemStorageInfo
    {
        public static ISystemStorageInfo GetDefault()
        {
            #if UNITY_EDITOR || UNITY_IOS
            return new UnityApplicationStorageInfo();
            #elif UNITY_ANDROID
            //TODO: Override GetExternalPath
            return new UnityApplicationStorageInfo();
            #elif UNITY_STANDALONE || UNITY_STANDALONE_WIN
            return new UnityApplicationStorageInfo();
            #else
            //TODO: LogError
            return null;
            #endif
        }
    }

    public class UnityApplicationStorageInfo : ISystemStorageInfo
    {
        public string GetExternalPath()
        {
            return Application.persistentDataPath;
        }

        public string GetInternalPath()
        {
            return Application.persistentDataPath;
        }
    }
}
