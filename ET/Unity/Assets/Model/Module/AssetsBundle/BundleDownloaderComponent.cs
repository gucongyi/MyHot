using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{


    [ObjectSystem]
    public class UiBundleDownloaderComponentAwakeSystem : AwakeSystem<BundleDownloaderComponent>
    {
        public override void Awake(BundleDownloaderComponent self)
        {
            self.bundles = new Queue<string>();
            self.downloadedBundles = new HashSet<string>();
            self.downloadingBundle = "";
        }
    }

    /// <summary>
    /// 用来对比web端的资源，比较md5，对比下载资源
    /// </summary>
    public class BundleDownloaderComponent : Component
    {
        public VersionConfig VersionConfig { get; private set; }

        public Queue<string> bundles;

        public long TotalSize;
        public BundleDownloadInfo DownloadInfo = new BundleDownloadInfo();

        public HashSet<string> downloadedBundles;

        public string downloadingBundle;

        public UnityWebRequestAsync webRequest;

        public TaskCompletionSource<bool> Tcs;

        public TaskCompletionSource<bool> FrameTcs;

        public Action<int> BundleRealProgress;

        public Action<int> BundleEachFrameProgress;

        public Action<string> FileServerNotReachCallBack;

        private bool isNetNotReach;
        public string GetUrlWithPlatform(string url)
        {
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
        /// <summary>
        /// 返回是否需要下载
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadInfo()
        {
            isNetNotReach = false;
            string versionUploadSelfResUrl = GlobalConfigComponent.Instance.GlobalProto.VersionUploadSelfResUrl;
            Log.Debug($"versionUploadSelfResUrl:{versionUploadSelfResUrl}");
            if (ETModel.Define.isInnetNet)
            {
                versionUploadSelfResUrl = GlobalConfigComponent.Instance.GlobalProto.VersionUploadSelfResUrl_InnerNet;
                Log.Debug($"VersionUploadSelfResUrl_InnerNet:{versionUploadSelfResUrl}");
            }
            UnityWebRequestAsync webRequestSelfVersionAsync = ComponentFactory.Create<UnityWebRequestAsync>();
            UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>();
            string versionText = string.Empty;
            try
            {
                //下载外层资源文件夹版本，即父目录
                var webRequestSelfResVersionTask = webRequestSelfVersionAsync.DownloadAsync(versionUploadSelfResUrl);
                await webRequestSelfResVersionTask;
                ZLog.Info($"webRequestSelfResVersionText:{webRequestSelfVersionAsync.Request.downloadHandler.text}");
                ETModel.Define.ParentResABDirectory = webRequestSelfVersionAsync.Request.downloadHandler.text;
                ZLog.Info($"ParentDirectory:{ETModel.Define.ParentResABDirectory}");
                webRequestSelfVersionAsync.Dispose();
                //下载bundle流程
                string versionUrl = GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + "Version.txt";
                if (ETModel.Define.IsABNotFromServer)
                {
                    versionUrl = GetUrlWithPlatform(ETModel.Define.SelfResourceServerIpAndPort + "/") + "StreamingAssets/" + "Version.txt";
                }
                Log.Debug($"versionUrl:{versionUrl}");
                var webRequestTask = webRequestAsync.DownloadAsync(versionUrl);
                await webRequestTask;
                versionText = webRequestAsync.Request.downloadHandler.text;
                webRequestSelfVersionAsync.Dispose();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("request error"))
                {
                    webRequestSelfVersionAsync.Dispose();
                    webRequestAsync.Dispose();
                    ZLog.Info("load Version err", e.Message);
                    Define.isUseStreamingAssetRes = true;
                    OnFileServerNotReach(e.Message);
                    return false;
                }
            }
            ZLog.Info($"versionText:{versionText}");
            if (!versionText.StartsWith("{"))
            {
                this.VersionConfig = null;
                return false;
            }
            this.VersionConfig = JsonHelper.FromJson<VersionConfig>(versionText);
            //Log.Debug(JsonHelper.ToJson(this.VersionConfig));


            if (isNetNotReach)//文件服务器没开启
            {
                //var timeTask = DelayFrame();
                //this.TagDownloadFinish();
                //await timeTask;
                return false;
            }
            else //成功的事情
            {
                VersionConfig localVersionConfig;
                // 对比本地的Version.txt
                string versionPath = Path.Combine(PathHelper.AppHotfixResPath, "Version.txt");
                if (File.Exists(versionPath))
                {
                    localVersionConfig = JsonHelper.FromJson<VersionConfig>(File.ReadAllText(versionPath));
                }
                else
                {
                    versionPath = Path.Combine(PathHelper.AppResPath4Web, "Version.txt");

                    using (UnityWebRequestAsync request = ComponentFactory.Create<UnityWebRequestAsync>())
                    {
                        try
                        {
                            await request.DownloadAsync(versionPath);
                            localVersionConfig = JsonHelper.FromJson<VersionConfig>(request.Request.downloadHandler.text);
                        }
                        catch (System.Exception e)
                        {
                            Log.Debug(e.ToString());
                            localVersionConfig = null;
                        }
                    }
                }

                if (localVersionConfig != null)
                {
                    // 先删除服务器端没有的ab
                    foreach (FileVersionInfo fileVersionInfo in localVersionConfig.FileInfoDict.Values)
                    {
                        if (this.VersionConfig.FileInfoDict.ContainsKey(fileVersionInfo.File))
                        {
                            continue;
                        }
                        string abPath = Path.Combine(PathHelper.AppHotfixResPath, fileVersionInfo.File);

                        if(File.Exists(abPath))File.Delete(abPath);
                    }
                }


                // 再下载
                foreach (FileVersionInfo fileVersionInfo in this.VersionConfig.FileInfoDict.Values)
                {
                    FileVersionInfo localVersionInfo;
                    if (localVersionConfig != null && localVersionConfig.FileInfoDict.TryGetValue(fileVersionInfo.File, out localVersionInfo))
                    {
                        if (fileVersionInfo.MD5 == localVersionInfo.MD5)
                        {
                            continue;
                        }
                    }

                    if (fileVersionInfo.File == "Version.txt")
                    {
                        continue;
                    }

                    this.bundles.Enqueue(fileVersionInfo.File);
                    this.TotalSize += fileVersionInfo.Size;
                }
                DownloadInfo.TotalSize = TotalSize;

                //if (this.bundles.Count == 0)
                //{
                //	return;
                //}

                //Log.Debug($"need download bundles: {this.bundles.ToList().ListToString()}");
                return true;
                //await Down();
            }
        }
        public async Task Down()
        {
            var timeTask = DelayFrame();
            var t = this.RealDown();
            await timeTask;
            await t;
        }

        public void OnFileServerNotReach(string error)
        {
            isNetNotReach = true;
            FileServerNotReachCallBack?.Invoke(error);
        }
        public Task<bool> DelayFrame()
        {
            FrameTcs = new TaskCompletionSource<bool>();
            UpdateFrames();
            return FrameTcs.Task;
        }

        public async void UpdateFrames()
        {
            int Frames = 0;
            while (Frames < 100)//100帧
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(20);//每帧20ms
                Frames++;
                this.BundleEachFrameProgress?.Invoke(Frames);
                //Debug.Log("Frames:"+ Frames);
            }
            this.FrameTcs.SetResult(true);
        }

        private async void UpdateAsync()
        {
            try
            {
                while (true)
                {
                    if (this.bundles.Count == 0)
                    {
                        TagDownloadFinish();
                        break;
                    }

                    this.downloadingBundle = this.bundles.Dequeue();

                    await DownServerBundle();
                    this.downloadedBundles.Add(this.downloadingBundle);
                    this.downloadingBundle = "";
                    this.webRequest = null;
                }

                using (FileStream fs = new FileStream(Path.Combine(PathHelper.AppHotfixResPath, "Version.txt"), FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(JsonHelper.ToJson(this.VersionConfig));
                }

                this.Tcs?.SetResult(true);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private async Task DownServerBundle()
        {
            while (true)
            {
                try
                {
                    using (this.webRequest = ComponentFactory.Create<UnityWebRequestAsync>())
                    {
                        DownloadInfo.IsStart = true;
                        if (ETModel.Define.IsABNotFromServer)
                        {
                            await this.webRequest.DownloadAsync(GetUrlWithPlatform(ETModel.Define.SelfResourceServerIpAndPort + "/") + "StreamingAssets/" + this.downloadingBundle);
                        }
                        else
                        {
                            await this.webRequest.DownloadAsync(GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + this.downloadingBundle);
                        }
                        //await this.webRequest.DownloadAsync(GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + this.downloadingBundle);
                        byte[] data = this.webRequest.Request.downloadHandler.data;
                        string path = Path.Combine(PathHelper.AppHotfixResPath, this.downloadingBundle);
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }


                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            fs.Write(data, 0, data.Length);
                            ZLog.Info($"更新Bundle:{path} 完成");
                        }
                        var p = this.Progress;
                        BundleRealProgress?.Invoke(p);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"download bundle error: {this.downloadingBundle}\n{e}");
                    //如果报错了,等1秒
                    await UniRx.Async.UniTask.Delay(1);
                    continue;
                }

                break;
            }
        }

        public void UpdateProgress()
        {
            var p = Progress;
        }

        public int Progress
        {
            get
            {
                if (this.VersionConfig == null)
                {
                    return 0;
                }
                if (this.TotalSize == 0)
                {
                    return 0;
                }

                long alreadyDownloadBytes = 0;
                foreach (string downloadedBundle in this.downloadedBundles)
                {
                    long size = this.VersionConfig.FileInfoDict[downloadedBundle].Size;
                    alreadyDownloadBytes += size;
                }
                if (this.webRequest != null)
                {
                    alreadyDownloadBytes += (long)this.webRequest.Request.downloadedBytes;
                }

                var p = (int)(alreadyDownloadBytes * 100f / this.TotalSize);
                DownloadInfo.alreadyDownloadBytes = alreadyDownloadBytes;
                DownloadInfo.TotalSize = TotalSize;
                return p;
            }
        }

        private void TagDownloadFinish()
        {
            BundleRealProgress?.Invoke(100);//表示下载完了
        }

        public Task<bool> RealDown()
        {
            if (this.bundles.Count == 0 && this.downloadingBundle == "")
            {
                TagDownloadFinish();
                return Task.FromResult(true);
            }

            this.Tcs = new TaskCompletionSource<bool>();

            UpdateAsync();

            return this.Tcs.Task;
        }
    }
}
