using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETModel
{

    public class ResourcesComponent : Component
	{
        public  Dictionary<string, AssetBundleCreateRequest> DicABRequest = new Dictionary<string, AssetBundleCreateRequest>();
        public  void InitDicABRequest()
        {
            DicABRequest.Clear();
        }
        public static AssetBundleManifest AssetBundleManifestObject { get; set; }

		

		private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();

		// lru缓存队列
		private readonly QueueDictionary<string, ABInfo> cacheDictionary = new QueueDictionary<string, ABInfo>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (var abInfo in this.bundles)
			{
				abInfo.Value?.AssetBundle?.Unload(true);
			}

			while (cacheDictionary.Count > 0)
			{
				ABInfo abInfo = this.cacheDictionary.FirstValue;
				this.cacheDictionary.Dequeue();
				abInfo.AssetBundle?.Unload(true);
			}

			this.bundles.Clear();
			this.cacheDictionary.Clear();
            ABManager.resourceCache.Clear();
		}


        private bool noUnloadBundle = true;
        public void UnloadBundle(string assetBundleName)
		{
            //暂时屏蔽卸载
            if(noUnloadBundle)return;
			assetBundleName = assetBundleName.ToLower();
			string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

			//Log.Debug($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				this.UnloadOneBundle(dependency);
			}
		}

		private void UnloadOneBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();

			ABInfo abInfo;
			if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
                ZLog.Error("==========所有 bundles==========");
                //return;
                foreach (var item in bundles)
                {
                    ZLog.Error(item.Key);
                }
				throw new Exception($"not found assetBundle: {assetBundleName}");
			}

			//Log.Debug($"---------- unload one bundle {assetBundleName} refcount: {abInfo.RefCount}");

			--abInfo.RefCount;
			if (abInfo.RefCount > 0)
			{
				return;
			}

            ZLog.Error($"移除assetBundleName??{assetBundleName}");
			this.bundles.Remove(assetBundleName);

			// 缓存10个包
			this.cacheDictionary.Enqueue(assetBundleName, abInfo);
			if (this.cacheDictionary.Count > 10)
			{
				abInfo = this.cacheDictionary[this.cacheDictionary.FirstKey];
				this.cacheDictionary.Dequeue();
				abInfo.Dispose();
			}
			//Log.Debug($"cache count: {this.cacheDictionary.Count}");
		}

		/// <summary>
		/// 同步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public void LoadBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

			//Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				if (string.IsNullOrEmpty(dependency))
				{
					continue;
				}
				this.LoadOneBundle(dependency);
			}
		}

		public void LoadOneBundle(string assetBundleName)
		{
			//Log.Debug($"---------------load one bundle {assetBundleName}");
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				++abInfo.RefCount;
				return;
			}


			if (this.cacheDictionary.ContainsKey(assetBundleName))
			{
				abInfo = this.cacheDictionary[assetBundleName];
				++abInfo.RefCount;
				this.bundles[assetBundleName] = abInfo;
				this.cacheDictionary.Remove(assetBundleName);
				return;
			}


			if (!Define.isUseAssetBundle)
			{
#if UNITY_EDITOR
				//string[] realPath = null;
				//realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
				//foreach (string s in realPath)
				//{
				//	string assetName = Path.GetFileNameWithoutExtension(s);
				//	string path = $"{assetBundleName}/{assetName}".ToLower();
				//	UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
				//	this.resourceCache[path] = resource;
				//}

				this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
				return;
			}

			string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
			AssetBundle assetBundle = null;
			if (File.Exists(p))
			{
				assetBundle = AssetBundle.LoadFromFile(p);
			}
			else
			{
				p = Path.Combine(PathHelper.AppResPath, assetBundleName);
				assetBundle = AssetBundle.LoadFromFile(p);
			}

			if (assetBundle == null)
			{
				throw new Exception($"assets bundle not found: {assetBundleName}");
			}

            //if (!assetBundle.isStreamedSceneAssetBundle)
            //{
            //	// 异步load资源到内存cache住
            //	UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
            //	foreach (UnityEngine.Object asset in assets)
            //	{
            //		string path = $"{assetBundleName}/{asset.name}".ToLower();
            //		this.resourceCache[path] = asset;
            //	}
            //}
            ZLog.Info("读取 assetBundle", assetBundleName, " loaded: ", assetBundle);
            this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
		}


        private bool loading = false;
        private string loadName = "";
		/// <summary>
		/// 异步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public async Task LoadBundleAsync(string assetBundleName, bool loadAllAssets = false)
        {
            assetBundleName = assetBundleName.ToLower();
            await LoadBundleAsync(assetBundleName);
            if (loadAllAssets)
            {
                var info = bundles[assetBundleName];
                if (info.IsLoadAllAssets) return;
                var loadStartTime = Time.realtimeSinceStartup;
                await LoadAllAssests(assetBundleName, info.AssetBundle);
                info.IsLoadAllAssets = true;
                Log.Debug($"缓存{assetBundleName} 所有资源 耗费时间 {Time.realtimeSinceStartup - loadStartTime}");
            }
        }

        private async Task LoadBundleAsync(string assetBundleName)
        {
            if (bundles.ContainsKey(assetBundleName))
            {
                Log.Debug($"已有Bundle {assetBundleName}");
                return;
            }

            if (loading)
            {
                Log.Debug($"读取 {assetBundleName} 等待 之前的 读取 {loadName}");
                while (loading)
                {
                    await UniRx.Async.UniTask.DelayFrame(2);
                }
            }
            if (bundles.ContainsKey(assetBundleName))
            {
                Log.Debug($"重复读取{assetBundleName}");
                return;
            }
            var loadStartTime = Time.realtimeSinceStartup;
            loading = true;
            loadName = assetBundleName;
            //等待读取完成.
            await RealLoadBundleAsync(assetBundleName);
            Log.Debug($"读取Bundle: {assetBundleName} 完成 耗费时间 {Time.realtimeSinceStartup - loadStartTime}");

            loading = false;
            loadName = "";
        }
        private async Task LoadAllAssests(string assetBundleName, AssetBundle assetBundle)
        {
            if (!Define.isUseAssetBundle)
            {
#if UNITY_EDITOR
                var realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                foreach (string s in realPath)
                {
                    string assetName = Path.GetFileNameWithoutExtension(s);
                    string path = $"{assetBundleName}/{assetName}".ToLower();
                    UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                    ABManager.resourceCache[path] = resource;
                }
#endif
                return;
            }
            if (assetBundle == null)
            {
                ZLog.Error($"{assetBundleName} is Null?");
                return;
            }
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                // 异步load资源到内存cache住
                UnityEngine.Object[] assets;
                using (AssetsLoaderAsync assetsLoaderAsync = ComponentFactory.Create<AssetsLoaderAsync, AssetBundle>(assetBundle))
                {
                    assets = await assetsLoaderAsync.LoadAllAssetsAsync();
                }
                foreach (UnityEngine.Object asset in assets)
                {
                    string path = $"{assetBundleName}/{asset.name}".ToLower();
                    ABManager.resourceCache[path] = asset;
                }
            }
        }

        private async Task RealLoadBundleAsync(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();

            //ABInfo abInfo;
            //if (this.bundles.TryGetValue(assetBundleName, out abInfo))
            //{
            //    Log.Debug($"RealLoadBundleAsync 读取到已经有的Bundle: {assetBundleName}");
            //    ++abInfo.RefCount;
            //    return;
            //}

            string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

            // Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }
                await this.LoadOneBundleAsync(dependency);
            }
            //await LoadOneBundleAsync(assetBundleName);
        }

        public async Task LoadOneBundleAsync(string assetBundleName,Action<float> updateProgress=null)
		{
			//Log.Debug($"---------------load one bundle {assetBundleName}");
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
                //Log.Debug($"读取到已经有的Bundle: {assetBundleName}");
                ++abInfo.RefCount;
				return;
			}


			if (this.cacheDictionary.ContainsKey(assetBundleName))
			{
				abInfo = this.cacheDictionary[assetBundleName];
				++abInfo.RefCount;
				this.bundles[assetBundleName] = abInfo;
				this.cacheDictionary.Remove(assetBundleName);
				return;
			}

            
			if (!Define.isUseAssetBundle)
			{
#if UNITY_EDITOR
                //ZLog.Info("UNITY_EDITOR LoadOneBundleAsync");
				//string[] realPath = null;
				//realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
				//foreach (string s in realPath)
				//{
				//	string assetName = Path.GetFileNameWithoutExtension(s);
				//	string path = $"{assetBundleName}/{assetName}".ToLower();
				//	UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
				//	this.resourceCache[path] = resource;
				//}

				this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
                return;
			}

			string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
			AssetBundle assetBundle = null;
			if (!File.Exists(p))
			{
				p = Path.Combine(PathHelper.AppResPath, assetBundleName);
			}
			
			using (AssetsBundleLoaderAsync assetsBundleLoaderAsync = ComponentFactory.Create<AssetsBundleLoaderAsync>())
			{
                assetBundle = await assetsBundleLoaderAsync.LoadAsync(p, updateProgress);
			}

            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {p}");
            }
            if (!assetBundle.isStreamedSceneAssetBundle && assetBundleName == "StreamingAssets")
            {

                // 异步load资源到内存cache住
                UnityEngine.Object[] assets;
                using (AssetsLoaderAsync assetsLoaderAsync = ComponentFactory.Create<AssetsLoaderAsync, AssetBundle>(assetBundle))
                {
                    assets = await assetsLoaderAsync.LoadAllAssetsAsync();
                }
                foreach (UnityEngine.Object asset in assets)
                {
                    string path = $"{assetBundleName}/{asset.name}".ToLower();
                    ABManager.resourceCache[path] = asset;
                }
            }

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
		}

        public Dictionary<string,ABInfo> GetBundle()
        {
            return this.bundles;
        }


		public string DebugString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (ABInfo abInfo in this.bundles.Values)
			{
				sb.Append($"{abInfo.Name}:{abInfo.RefCount}\n");
			}
			return sb.ToString();
		}

        public Sprite GetSprite(string spriteName)
        {
            var sprite = this.GetAsset<Sprite>($"icon.unity3d", spriteName);
            return sprite;
        }

        public T GetAsset<T>(string bundleName, string prefab) where T : UnityEngine.Object
        {
            //string path = $"{bundleName}/{prefab}".ToLower();

            //UnityEngine.Object resource = null;
            //if (!this.resourceCache.TryGetValue(path, out resource))
            //{
            //    var asset = bundles[bundleName].AssetBundle.LoadAsset<T>(prefab);
            //    if (asset == null)
            //    {
            //        throw new Exception($"not found asset: {path}");
            //    }
            //    this.resourceCache[path] = asset;
            //}

            //return (T)resource;
            bundleName = bundleName.ToLower();
            prefab = prefab.ToLower();
            var resource = GetAssetCache(bundleName, prefab);
            if (resource != null) return (T)resource;

#if UNITY_EDITOR
            var s = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName, prefab);
            if (s.Length == 0)
            {
                throw new Exception("bundleName " + bundleName + "  AssetName " + prefab);
            }
            resource = AssetDatabase.LoadAssetAtPath<T>(s[0]);
#else
            if (!bundles.ContainsKey(bundleName))
            {
                throw new Exception($"读取资源{prefab} 没有找到 bundle  {bundleName} ");
            }
            resource = bundles[bundleName].AssetBundle.LoadAsset<T>(prefab);
#endif
            if (resource == null)
            {
                throw new Exception($"not found asset: {bundleName}/{prefab}");
            }
            string path = $"{bundleName}/{prefab}".ToLower();
            ABManager.resourceCache[path] = resource;
            return (T)resource;
        }
        public UnityEngine.Object GetAsset(string bundleName, string prefab)
        {
            var resource = GetAssetCache(bundleName, prefab);
            if (resource != null) return resource;
            bundleName = bundleName.ToLower();
            prefab = prefab.ToLower();
            if (!bundles.ContainsKey(bundleName))
            {
                throw new Exception("bundles找不到 bundleName " + bundleName + "  AssetName " + prefab);
            }
            
            if (!Define.isUseAssetBundle)
            {
#if UNITY_EDITOR
                var s = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName, prefab);
                //var realPath = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                if (s.Length == 0)
                {
                    throw new Exception("bundleName " + bundleName + "  AssetName " + prefab);
                }
                resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s[0]);
#endif
            }
            else
            {
#if UNITY_EDITOR
                //这里代码特殊处理,方便测试,不用重新打Bundle
                if (prefab == "code")
                {
                    var s = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName, prefab);
                    resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s[0]);
                }
                else
                {
                    resource = bundles[bundleName].AssetBundle.LoadAsset(prefab);
                }
#else
                resource = bundles[bundleName].AssetBundle.LoadAsset(prefab);
#endif
            }
            if (resource == null)
                {
                    throw new Exception($"not found asset: {bundleName}/{prefab}");
                }
                string path = $"{bundleName}/{prefab}".ToLower();
            ABManager.resourceCache[path] = resource;
            return resource;
        }
        private UnityEngine.Object GetAssetCache(string bundleName, string prefab)
        {
            string path = $"{bundleName}/{prefab}".ToLower();
            if (ABManager.resourceCache.ContainsKey(path)) return ABManager.resourceCache[path];
            return null;
        }

        /// <summary>
		/// 同步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public async Task LoadBundleAsync<T>(string assetBundleName) where T : UnityEngine.Object
        {
            assetBundleName = assetBundleName.ToLower();
            string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

            //Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }
                if (assetBundleName.Equals(dependency))
                {
                    await this.LoadOneBundleAsync<T>(dependency);
                }
                else
                {
                    await this.LoadOneBundleAsync(dependency);
                }
			}
		}

		async Task LoadOneBundleAsync<T>(string assetBundleName) where T : UnityEngine.Object
        {
            //Log.Debug($"---------------load one bundle {assetBundleName}");
            ABInfo abInfo;
            if (this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                ++abInfo.RefCount;
                return;
            }


            if (this.cacheDictionary.ContainsKey(assetBundleName))
            {
                abInfo = this.cacheDictionary[assetBundleName];
                ++abInfo.RefCount;
                this.bundles[assetBundleName] = abInfo;
                this.cacheDictionary.Remove(assetBundleName);
                return;
            }


            if (!Define.isUseAssetBundle)
            {
#if UNITY_EDITOR
                //string[] realPath = null;
                //realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                //foreach (string s in realPath)
                //{
                //    string assetName = Path.GetFileNameWithoutExtension(s);
                //    string path = $"{assetBundleName}/{assetName}".ToLower();
                //    T resource = AssetDatabase.LoadAssetAtPath<T>(s);
                //    this.resourceCache[path] = resource;
                //}

                this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
                return;
            }

            string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
            AssetBundle assetBundle = null;
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, assetBundleName);
            }
            
            using (AssetsBundleLoaderAsync assetsBundleLoaderAsync = ComponentFactory.Create<AssetsBundleLoaderAsync>())
            {
                assetBundle = await assetsBundleLoaderAsync.LoadAsync(p);
            }

            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {assetBundleName}");
            }

            //if (!assetBundle.isStreamedSceneAssetBundle)
            //{
            //    // 异步load资源到内存cache住
            //    UnityEngine.Object[] assets;
            //    using (AssetsLoaderAsync assetsLoaderAsync = ComponentFactory.Create<AssetsLoaderAsync, AssetBundle>(assetBundle))
            //    {
            //        assets = await assetsLoaderAsync.LoadAllAssetsAsync<T>();
            //    }
            //    foreach (UnityEngine.Object asset in assets)
            //    {
            //        string path = $"{assetBundleName}/{asset.name}".ToLower();
            //        this.resourceCache[path] = asset;
            //    }
            //}

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
        }
    }
}