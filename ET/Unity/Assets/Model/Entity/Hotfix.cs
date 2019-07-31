using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
	public sealed class Hotfix : Object
	{
#if ILRuntime
		private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
        private MemoryStream dllStream;
        private MemoryStream pdbStream;
#else
		private Assembly assembly;
#endif

        private IStaticMethod start;
        private List<Type> hotfixTypes;

        public Action Update;
		public Action LateUpdate;
		public Action OnApplicationQuit;
        public Action OnApplicationFocus;

		public Hotfix()
		{

		}

		public void GotoHotfix()
		{
#if ILRuntime
			ILHelper.InitILRuntime(this.appDomain);
#endif
			this.start.Run();
		}

		public List<Type> GetHotfixTypes()
		{
             return this.hotfixTypes; 
//#if ILRuntime
//            if (this.appDomain == null)
//			{
//				return new List<Type>();
//			}
//            List<Type> list = new List<Type>();
//            foreach (var item in this.appDomain.LoadedTypes.Values)
//            {
//                if(!(item.IsGenericParameter|| item.IsByRef||item.IsArray))
//                {
//                    list.Add(item.ReflectionType);
//                }
//            }
//            return list;
//            //return this.appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).Where(x=>!(x.IsGenericParameter||x.IsByRef||x.IsArray)).ToList();
//#else
//			if (this.assembly == null)
//			{
//				return new List<Type>();
//			}
//			return this.assembly.GetTypes().ToList();
//#endif
		}


		public  void LoadHotfixAssembly()
		{
   //         if (Game.Scene == null) return;
   //         if (Game.Scene.GetComponent<ResourcesComponent>() == null) return;
			//await Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync($"code.unity3d");

            Game.Scene.GetComponent<ResourcesComponent>().LoadBundle($"code.unity3d");
            GameObject code = (GameObject)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("code.unity3d", "Code");

            byte[] assBytes = code.Get<TextAsset>("Hotfix.dll").bytes;
            byte[] pdbBytes = code.Get<TextAsset>("Hotfix.pdb").bytes;

#if ILRuntime
            //            this.appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            //#if ILRuntimeDebug
            //            this.appDomain.DebugService.StartDebugService(56000);
            //#endif
            //            GameObject code = (GameObject)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("code.unity3d", "Code");
            //			byte[] assBytes = code.Get<TextAsset>("Hotfix.dll").bytes;
            //			byte[] mdbBytes = code.Get<TextAsset>("Hotfix.pdb").bytes;

            //			using (MemoryStream fs = new MemoryStream(assBytes))
            //			using (MemoryStream p = new MemoryStream(mdbBytes))
            //			{
            //				this.appDomain.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
            //			}
            //            ILHelper.InitILRuntime(this.appDomain);
            //            this.start = new ILStaticMethod(this.appDomain, "ETHotfix.Init", "Start", 0);

            Log.Debug($"当前使用的是ILRuntime模式");
            this.appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();

            this.dllStream = new MemoryStream(assBytes);
            this.pdbStream = new MemoryStream(pdbBytes);
            this.appDomain.LoadAssembly(this.dllStream, this.pdbStream, new Mono.Cecil.Pdb.PdbReaderProvider());

            this.start = new ILStaticMethod(this.appDomain, "ETHotfix.Init", "Start", 0);

            this.hotfixTypes = this.appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
#else
			
			this.assembly = Assembly.Load(assBytes, pdbBytes);

			Type hotfixInit = this.assembly.GetType("ETHotfix.Init");
			this.start = new MonoStaticMethod(hotfixInit, "Start");
            this.hotfixTypes = this.assembly.GetTypes().ToList();
#endif
            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"code.unity3d");
		}
	}
}