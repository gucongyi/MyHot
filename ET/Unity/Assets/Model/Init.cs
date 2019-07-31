using Company;
using System;
using System.Threading;
using UniRx.Async;
using UnityEngine;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
        public string versionGameCode;
        public string GameShowVersion;
        /// <summary>
        /// 是否显示FPS
        /// </summary>
        public bool isShowFPS;
        public bool isUseAssetBundle;
        //默认不选，从服务器下载
        public bool isABNotFromServer;
        public string selfResourceServerIpAndPort;
        public int TargetFrameRate;
        public bool isInnetNet;
/*#if UNITY_ANDROID
        private int scaleWidth = 0;
        private int scaleHeight = 0;
        public void setDesignContentScale()
        {
            if (scaleWidth == 0 && scaleHeight == 0)
            {
                int width = Screen.currentResolution.width;
                int height = Screen.currentResolution.height;
                int designWidth = 1136;
                int designHeight = 640;
                float s1 = (float)designWidth / (float)designHeight;
                float s2 = (float)width / (float)height;
                if (s1 < s2)
                {
                    designWidth = (int)Mathf.FloorToInt(designHeight * s2);
                }
                else if (s1 > s2)
                {
                    designHeight = (int)Mathf.FloorToInt(designWidth / s2);
                }
                float contentScale = (float)designWidth / (float)width;
                if (contentScale < 1.0f)
                {
                    scaleWidth = designWidth;
                    scaleHeight = designHeight;
                }
            }
            if (scaleWidth > 0 && scaleHeight > 0)
            {
                if (scaleWidth % 2 == 0)
                {
                    scaleWidth += 1;
                }
                else
                {
                    scaleWidth -= 1;
                }
                Screen.SetResolution(scaleWidth, scaleHeight, true);
            }
        }

        void OnApplicationPause(bool paused)
        {
            if (paused)
            {
            }
            else
            {
                setDesignContentScale();
            }
        }
#endif*/
        private async void Start()
		{
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //禁用多点触控
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = TargetFrameRate;
            ETModel.Define.IsABNotFromServer = isABNotFromServer;
            ETModel.Define.SelfResourceServerIpAndPort= selfResourceServerIpAndPort;
            ETModel.Define.isShowFPS = isShowFPS;
            ETModel.Define.versionGameCode = versionGameCode;
            ETModel.Define.GameShowVersion = GameShowVersion;
            ETModel.Define.isInnetNet = isInnetNet;
            try
            {
                //播放背景音乐
                GameSoundPlayer.Instance.PlayBgMusic("ExampleBgMusic");
                Define.isUseAssetBundle = isUseAssetBundle;
                DontDestroyOnLoad(gameObject);
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<BundleDownloaderComponent>();
                Game.EventSystem.Run(EventIdType.LoadingBegin);//打开进度条，最外层的
                var IsGameVersionCodeEqual = await BundleHelper.IsGameVersionCodeEqual();
                if (!IsGameVersionCodeEqual)
                {
                    return;
                }
                // 下载ab包
                await BundleHelper.DownloadBundle();
                //await TestWifiUi();
                Game.Scene.RemoveComponent<BundleDownloaderComponent>();
                //解析abmapping
                ABManager.Init();
                //下载好了，声音ab绑定到固定的地方
                await ResetSoundPlayers();
                await UniTask.DelayFrame(1);
                Game.Hotfix.LoadHotfixAssembly();
                // 加载配置
                await Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

                Game.Hotfix.GotoHotfix();
            }
            catch (Exception e)
			{
				Log.Error(e);
			}
		}

        private static async System.Threading.Tasks.Task ResetSoundPlayers()
        {
            var goMusicPlayer = GameObject.Find("Global/GameSoundPlayer/MusicPlayer");
            var goEffectPlayer = GameObject.Find("Global/GameSoundPlayer/SFXPlayer");
            var soundPlayerMusic = goMusicPlayer.GetComponent<SoundPlayer>();
            var soundPlayerEffect = goEffectPlayer.GetComponent<SoundPlayer>();
            var goMusicList = await ABManager.GetAssetAsync<UnityEngine.GameObject>(Model.ABMapping.MusicList);
            goMusicList = GameObject.Instantiate(goMusicList);
            goMusicList.transform.parent = goMusicPlayer.transform;
            soundPlayerMusic.soundLists[0] = goMusicList.GetComponent<SoundList>();
            var goSound2dEffectList = await ABManager.GetAssetAsync<UnityEngine.GameObject>(Model.ABMapping.SoundList2D);
            var goSound3dEffectList = await ABManager.GetAssetAsync<UnityEngine.GameObject>(Model.ABMapping.SoundList3D);
            goSound2dEffectList = GameObject.Instantiate(goSound2dEffectList);
            goSound3dEffectList = GameObject.Instantiate(goSound3dEffectList);
            goSound2dEffectList.transform.parent = goEffectPlayer.transform;
            goSound3dEffectList.transform.parent = goEffectPlayer.transform;
            soundPlayerEffect.soundLists[0] = goSound2dEffectList.GetComponent<SoundList>();
            soundPlayerEffect.soundLists[1] = goSound3dEffectList.GetComponent<SoundList>();
            //重新初始话
            soundPlayerMusic.soundLists[0].InitializeAudioSourcePools();
            soundPlayerEffect.soundLists[0].InitializeAudioSourcePools();
            soundPlayerEffect.soundLists[1].InitializeAudioSourcePools();
        }

        private async System.Threading.Tasks.Task TestWifiUi()
        {
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            UI uiLoading = uiComponent.Get(UIType.UILoading);
            var uiLoadingComponent = uiLoading.GetComponent<UILoadingComponent>();

            var x = 2048 / 1024f;
            //如果大于1m才弹提示
            if (x > 1)
            {
                while (!VideoUtil.videoFinished)
                    await UniTask.DelayFrame(1);
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

        private void Update()
		{
			Game.Hotfix.Update?.Invoke();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.Hotfix.LateUpdate?.Invoke();
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Hotfix.OnApplicationQuit?.Invoke();
			Game.Close();
		}

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Game.Hotfix.OnApplicationFocus?.Invoke();
            }
        }
    }
}