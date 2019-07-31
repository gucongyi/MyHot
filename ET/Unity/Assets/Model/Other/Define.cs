using System.Collections.Generic;
using System.Threading.Tasks;

namespace ETModel
{
	public static class Define
	{
        public static string ParentResABDirectory;
        public static string versionGameCode;
        public static string GameShowVersion;
        public static bool isInnetNet;
        /// <summary>
        /// 下载buddle,Editor也用ab.
        /// </summary>
        public static bool isUseAssetBundle = false;
        public static bool IsABNotFromServer = false;
        public static string SelfResourceServerIpAndPort;
        public static bool isUseStreamingAssetRes=false;
        /// <summary>
        /// 是否显示FPS
        /// </summary>
        public static bool isShowFPS;
        public static void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}