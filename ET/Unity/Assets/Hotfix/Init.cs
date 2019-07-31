using System;
using System.IO;
using System.Threading.Tasks;
using Company.Cfg;
using ETModel;
using UniRx.Async;
using UnityEngine;

namespace ETHotfix
{
	public static class Init
	{

        public static async void Start()
		{
			try
			{
               
                Debug.Log("Begin ETHotfix.Init.Start");
                Game.Scene.ModelScene = ETModel.Game.Scene;
                ////Test
                //UnityEngine.Sprite sprite = await ABManager.GetAssetAsync<UnityEngine.Sprite>(Hotfix.ABMapping.ArrowUp1);
                //Debug.Log($"{Hotfix.ABMapping.ArrowUp1}:{sprite.name}");
                GameSoundPlayer.Instance.PlayBgMusic("BgMusicHotfix");
                GameSoundPlayer.Instance.PlaySoundEffect("laugh2D");
                GameSoundPlayer.Instance.PlaySoundEffect("water", Vector3.zero);
                // 注册热更层回调
                ETModel.Game.Hotfix.Update = () => { Update(); };
				ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
				ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
                ETModel.Game.Hotfix.OnApplicationFocus = () => { OnApplicationFocus(); };
				
				Game.Scene.AddComponent<UIComponent>();
                ////预加载一些资源 
                //await PreLoad();
                //var taskLoadSceneRes1 = PreLoad1();
				// 加载热更配置
				await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync<UnityEngine.Sprite>("icon.unity3d");
                //解析数据表
                var tabtoyText=await ABManager.GetAssetAsync<TextAsset>(Hotfix.ABMapping.tabtoyData);
                //读取
                MemoryStream ms = null;
                var il = new ILSerial();
                Config configTabtoy = il.ReadFileBytes<Config>(tabtoyText.bytes, out ms);
                StaticTool.TabToyDataConfig = configTabtoy;
                ms.Close();
                var define = configTabtoy.Test[0];
                Debug.LogError($"热更层 解析数据表不建议使用float,float数组会导致解析错误，最好用int32替代");
                Debug.LogError($"热更层 数据表中string要加\"\",否则会解析出错");
                Debug.LogError($"热更层 Test define.TestInt:{define.TestInt} define.TestLong:{define.TestLong}");
                Debug.LogError($"热更层 Test define.TestFloat:{define.TestFloat}");
                Debug.LogError($"热更层 Test define.TestString:{define.TestString}");
                Debug.LogError($"热更层 Test define.TestIntArr:{define.TestIntArr[0]} {define.TestIntArr[1]} {define.TestIntArr[2]} {define.TestIntArr[3]} {define.TestIntArr[4]} {define.TestIntArr[5]}");
                Debug.LogError($"热更层 Test define.TestFloat2:{define.TestFloat2}");
                Debug.LogError($"热更层 Test define.TestStringArr:{define.TestStringArr[0]} {define.TestStringArr[1]} {define.TestStringArr[2]}");
                Debug.LogError($"热更层 Test define.TestEnum0:{define.TestEnum0} define.TestEnum1:{define.TestEnum1}");
                Debug.LogError($"热更层 Test define.TestClass:{define.TestClass.DropID} {define.TestClass.DropNumber}");

                //初始化ab字典
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().InitDicABRequest();
                ////测试DoTween插件
                //UI uiDoTweenTest = Game.Scene.GetComponent<UIComponent>().Create(UIType.UIDoTweenTest);
                Debug.Log("Begin ETHotfix.Init.Start UIType.UIFrameShow");
                await Game.Scene.GetComponent<UIComponent>().CreateAsync(UIType.UIFrameShow);
                Debug.Log("End ETHotfix.Init.Start UIType.UIFrameShow");
                Debug.Log("Begin ETHotfix.Init.Start UIType.UILogin");
                UI ui = await Game.Scene.GetComponent<UIComponent>().CreateAsync(UIType.UILogin);
                Debug.Log("End ETHotfix.Init.Start UIType.UILogin");
                ETModel.Game.Scene.GetComponent<ETModel.UIComponent>().Remove(ETModel.UIType.UILoading);//关闭进度条
                Debug.Log("End ETHotfix.Init.Start Remove ETModel.UIType.UILoading");
               
            }
            catch (Exception e)
			{
				Log.Error(e);
			}
		}
      
        private async static  Task PreLoad()
        {
            var list = new System.Collections.Generic.List<string>() {
                //commonforder/
                "commonforder/horse.unity3d",
                "commonforder/malejockey.unity3d",
                "commonforder/male.unity3d",
                "commonforder/female.unity3d",
                "textures.unity3d",
                "sceneassets.unity3d",
            };
            var uiComponent = ETModel.Game.Scene.GetComponent<ETModel.UIComponent>();
            var uiLoading = uiComponent.Get(ETModel.UIType.UILoading);
            var uiLoadingComponent = uiLoading.GetComponent<ETModel.UILoadingComponent>();
            if (uiLoadingComponent == null)
            {
                return;
            }
            var view = uiLoadingComponent.view;
            if (view == null)
            {
                return;
            }
            //var DownLoadInfo = uiLoadingComponent.DownLoadInfo;
            if (view.text_txtLeft == null)
            {
                return;
            }
            view.text_txtLeft.text = "";
            var progress = 0f;
            Action UpdateTxt = () =>
            {
                view.text_txtCenter1.text = "";
                view.text_txtCenter.text = "";
                view.text_txtRight.text = "资源加载 不消耗流量";
                view.text_txtLeft.text = $"资源加载中 {(int)progress}%";
            };
            
            //view.text_txtCenter.text = DownLoadInfo.SpeedString;
            //view.text_txtRight.text = DownLoadInfo.AlreadyDownloadString;
            view.image_ProgressForward_3.fillAmount = 0;

            //Action updateTxt = () => view.text_txtLeft.text = $"资源加载中 {(int)progress}%";
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            for (int i = 0; i < list.Count; i++)
            {
                await res.LoadBundleAsync(list[i], true);
                progress = (i+1)*100f / list.Count;
                view.image_ProgressForward_3.fillAmount = progress / 100f;
                UpdateTxt();
            }
        }

       
		public static void Update()
		{
			try
			{
				Game.EventSystem.Update();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void LateUpdate()
		{
			try
			{
				Game.EventSystem.LateUpdate();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void OnApplicationQuit()
		{
			Game.Close();
            StaticTool.Dispose();
        }

        public static void OnApplicationFocus()
        {
            
        }
    }
}