namespace ETModel
{
	public class GlobalProto
	{
        public string VersionUploadSelfResUrl;
        public string VersionUploadSelfResUrl_InnerNet;
        public string AssetBundleServerUrl;
        public string AssetBundleServerUrl_InnerNet;
        public string Address;
        public string VersionCodeInner;
        public string VersionCodeOuter;


        public string GetUrl()
		{
			string url = this.AssetBundleServerUrl;
            if (ETModel.Define.isInnetNet)
            {
                url = this.AssetBundleServerUrl_InnerNet;
            }
            url += ETModel.Define.ParentResABDirectory + "/";
#if UNITY_ANDROID
            url += "Android/";
#elif UNITY_IOS
			url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#else
			url += "PC/";
#endif
			//Log.Debug(url);
			return url;
		}
	}
}
