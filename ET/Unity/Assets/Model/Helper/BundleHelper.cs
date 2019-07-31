using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class BundleHelper
    {
        public static async Task DownloadBundle()
        {
            await StartDownLoadResources();
        }
        public static async Task<bool> IsGameVersionCodeEqual()
        {
            string versionCodeInner = GlobalConfigComponent.Instance.GlobalProto.VersionCodeInner;
            Log.Debug($"versionCodeInner:{versionCodeInner}");
            string versionCodeOuter = GlobalConfigComponent.Instance.GlobalProto.VersionCodeOuter;
            Log.Debug($"versionCodeOuter:{versionCodeOuter}");
            string versionCode = versionCodeOuter;
            if (ETModel.Define.isInnetNet)
            {
                versionCode = versionCodeInner;
            }
            UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>();
            try
            {
                //下载VersionCode
                var webRequestGameVersion = webRequestAsync.DownloadAsync(versionCode);
                await webRequestGameVersion;
                var versionCodeText = webRequestAsync.Request.downloadHandler.text;
                ZLog.Info($"versionCode:{versionCodeText}");
                webRequestAsync.Dispose();
                //比较VersionCode
                ZLog.Info($"ETModel.Define.versionGameCode:{ETModel.Define.versionGameCode}");
                if (versionCodeText.Equals(ETModel.Define.versionGameCode))
                {
                    ZLog.Info($"versionGameCode Equal");
                    return true;
                }
                else
                {
                    ZLog.Info($"versionGameCode Not Equal");
                    //while (!VideoUtil.videoFinished)
                    //    await UniRx.Async.UniTask.DelayFrame(1);
                    UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
                    UI uiLoading = uiComponent.Get(UIType.UILoading);
                    var uiLoadingComponent = uiLoading.GetComponent<UILoadingComponent>();
                    //弹提示.
                    var trans = uiLoadingComponent.view.transform.Find("ConfirmWindowVersion");
                    var tip = new UIUpdateVersionTip(trans);
                    tip.OnConfirm = () =>
                    {
                        ETModel.Define.QuitApplication();
                    };
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("request error"))
                {
                    webRequestAsync.Dispose();
                    ZLog.Info("load VersionGameCode err", e.Message);
                    return false;
                }
            }
            return false;
        }

        public static async Task StartDownLoadResources()
        {
            if (Define.isUseAssetBundle)
            {
                try
                {
                    using (BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>())
                    {
                        var t = bundleDownloaderComponent.LoadInfo();

                        UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
                        UI uiLoading = uiComponent.Get(UIType.UILoading);
                        var uiLoadingComponent = uiLoading.GetComponent<UILoadingComponent>();
                        uiLoadingComponent.DownLoadInfo = bundleDownloaderComponent.DownloadInfo;
                        bundleDownloaderComponent.BundleRealProgress = uiLoadingComponent.BundleRealDownload;
                        bundleDownloaderComponent.BundleEachFrameProgress = uiLoadingComponent.BundleDownloadFrames;
                        uiLoadingComponent.UpdateProgress = bundleDownloaderComponent.UpdateProgress;
                        var needDown = await t;
                        if (needDown)
                        {
                            var x1 = uiLoadingComponent.DownLoadInfo.TotalSize / 1024;
                            var x = x1 / 1024f;

                            //如果大于1m 不是wifi才弹提示
                            if (x > 1 /*&& Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork*/)
                            {
                                //while (!VideoUtil.videoFinished)
                                //    await UniRx.Async.UniTask.DelayFrame(1);
                                var actionEvent = new ActionEvent();
                                //弹提示.
                                var trans = uiLoadingComponent.view.transform.Find("ConfirmWindow");
                                var tip = new UIUpdateTip(trans);
                                //取两位小数
                                int j = (int)(x * 100);
                                x = j / 100f;
                                tip.SetInfo($"当前不是wifi环境, 更新需要消耗{x}M流量,\n是否更新 ? (点击取消将退出游戏)");
                                tip.OnConfirm = () =>
                                {
                                    trans.gameObject.SetActive(false);
                                    actionEvent.Dispatch();
                                };
                                tip.OnCancel = () =>
                                {
                                    Define.QuitApplication();
                                    return;
                                };

                                await actionEvent;
                            }
                        }
                        await bundleDownloaderComponent.Down();
                        uiLoadingComponent.DownLoadInfo.IsEnd = true;
                    }

                    //Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
                    await Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundleAsync("StreamingAssets");

                    ResourcesComponent.AssetBundleManifestObject = (AssetBundleManifest)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("StreamingAssets", "AssetBundleManifest");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

            }
        }
    }
}
