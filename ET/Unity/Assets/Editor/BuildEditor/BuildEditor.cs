﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ETEditor
{
	public class BundleInfo
	{
		public List<string> ParentPaths = new List<string>();
	}

	public enum PlatformType
	{
		None,
		Android,
		IOS,
		PC,
	}

	public class BuildEditor : EditorWindow
	{
		private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

		private PlatformType platformType;
		private bool isBuildExe;
		private bool isContainAB=true;
		private BuildOptions buildOptions = BuildOptions.CompressWithLz4HC;
		private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;

		[MenuItem("Tools/打包工具")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuildEditor));
		}

		private void OnGUI()
		{
			if (GUILayout.Button("标记"))
			{
				SetPackingTagAndAssetBundle();
			}

			//if (GUILayout.Button("清除标记"))
			//{
			//	ClearPackingTagAndAssetBundle();
			//}

			this.platformType = (PlatformType)EditorGUILayout.EnumPopup(platformType);
            if (platformType == PlatformType.None) platformType = PlatformType.Android;
			this.isBuildExe = EditorGUILayout.Toggle("是否打包EXE: ", this.isBuildExe);
			this.isContainAB = EditorGUILayout.Toggle("是否同将资源打进EXE: ", this.isContainAB);
			this.buildOptions = (BuildOptions)EditorGUILayout.EnumMaskField("BuildOptions(可多选): ", this.buildOptions);
			this.buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumMaskField("BuildAssetBundleOptions(可多选): ", this.buildAssetBundleOptions);

			if (GUILayout.Button("开始打包"))
			{
				if (this.platformType == PlatformType.None)
				{
					Log.Error("请选择打包平台!");
					return;
				}
				BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB);
			}
		}

        //自动打包的标记
        public static void SetOneKeyPackingTag()
        {
            SetPackingTagAndAssetBundle();
        }
        //自动打AB包
        public static void BuildAB(PackageConfig packageConfig)
        {
            PlatformType platformType = PlatformType.PC;
            bool isBuildExe = false;
            bool isContainAB = true;
            switch (packageConfig.platformType)
            {
                case PackageConfig.PlatformType.Andriod:
                    platformType = PlatformType.Android;
                    break;
                case PackageConfig.PlatformType.IOS:
                    platformType = PlatformType.IOS;
                    break;
            }
            BuildHelper.Build(platformType, packageConfig.buildAssetBundleOptions, packageConfig.buildOptions, isBuildExe, isContainAB);
        }

		private static void SetPackingTagAndAssetBundle()
		{
			//ClearPackingTagAndAssetBundle();

            SetIndependentBundleAndAtlas("Assets/Bundles/Independent");
            SetRootBundleOnly("Assets/Bundles/TabtoyData");
            SetRootBundleOnly("Assets/Bundles/SoundList");

            SetBundleByParentPath("Assets/Bundles/UI","ui");

            SetRootBundleOnly("Assets/Bundles/Scenes");

            SetBundleIcon("Assets/Bundles/Icon");
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
		}

		private static void SetNoAtlas(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

			foreach (string path in paths)
			{
				List<string> pathes = CollectDependencies(path);

				foreach (string pt in pathes)
				{
					if (pt == path)
					{
						continue;
					}

					SetAtlas(pt, "", true);
				}
			}
		}

		// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
		private static void SetBundles(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);
			}
		}
        /// <summary>
        /// 会将目录下的每个prefab引用的资源打成一个包,只给顶层prefab打包
        /// 包添加一个前缀路径
        /// </summary>
        /// <param name="dir"></param>
        private static void SetBundleByParentPath(string dir,string parentPathName="ui")
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, parentPathName + "/" + go.name,true);
            }
        }
		// 会将目录下的每个prefab引用的资源打成一个包,只给顶层prefab打包
		private static void SetRootBundleOnly(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);
			}
		}

		// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
		private static void SetIndependentBundleAndAtlas(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				AssetImporter importer = AssetImporter.GetAtPath(path1);
				if (importer == null || go == null)
				{
					Log.Error("error: " + path1);
					continue;
				}
				importer.assetBundleName = $"{go.name}.unity3d";

				List<string> pathes = CollectDependencies(path1);

				foreach (string pt in pathes)
				{
					if (pt == path1)
					{
						continue;
					}

					SetBundleAndAtlas(pt, go.name, true);
				}
			}
		}

		private static void SetBundleAndAtlasWithoutShare(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name,true);

				//List<string> pathes = CollectDependencies(path1);
				//foreach (string pt in pathes)
				//{
				//	if (pt == path1)
				//	{
				//		continue;
				//	}
				//
				//	SetBundleAndAtlas(pt, go.name);
				//}
			}
		}

		private static List<string> CollectDependencies(string o)
		{
			string[] paths = AssetDatabase.GetDependencies(o);

			//Log.Debug($"{o} dependecies: " + paths.ToList().ListToString());
			return paths.ToList();
		}

		// 分析共享资源
		private void SetShareBundleAndAtlas(string dir)
		{
			this.dictionary.Clear();
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);

				List<string> pathes = CollectDependencies(path1);
				foreach (string pt in pathes)
				{
					if (pt == path1)
					{
						continue;
					}

					// 不存在则记录下来
					if (!this.dictionary.ContainsKey(pt))
					{
						// 如果已经设置了包
						if (GetBundleName(pt) != "")
						{
							continue;
						}
						Log.Info($"{path1}----{pt}");
						BundleInfo bundleInfo = new BundleInfo();
						bundleInfo.ParentPaths.Add(path1);
						this.dictionary.Add(pt, bundleInfo);

						SetAtlas(pt, go.name);

						continue;
					}

					// 依赖的父亲不一样
					BundleInfo info = this.dictionary[pt];
					if (info.ParentPaths.Contains(path1))
					{
						continue;
					}
					info.ParentPaths.Add(path1);

					DirectoryInfo dirInfo = new DirectoryInfo(dir);
					string dirName = dirInfo.Name;

					SetBundleAndAtlas(pt, $"{dirName}-share", true);
				}
			}
		}

		private static void ClearPackingTagAndAssetBundle()
		{
            List<string> bundlePaths = EditorResHelper.GetAllResourcePath("Assets/Bundles/", true);
            foreach (string bundlePath in bundlePaths)
            {
                SetBundle(bundlePath, "", true);
                List<string> pathes = CollectDependencies(bundlePath);

                foreach (string pt in pathes)
                {
                    if (pt == bundlePath)
                    {
                        continue;
                    }

                    SetBundleAndAtlas(pt, "", true);
                }
            }

            List<string> paths = EditorResHelper.GetAllResourcePath("Assets/Res", true);
			foreach (string pt in paths)
			{
				SetBundleAndAtlas(pt, "", true);
			}
		}

		private static string GetBundleName(string path)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return "";
			}
			if (path.Contains("Resources"))
			{
				return "";
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return "";
			}

			return importer.assetBundleName;
		}

		public static void SetBundle(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return;
			}

			if (importer.assetBundleName != "" && overwrite == false)
			{
				return;
			}

			//Log.Info(path);
			string bundleName = "";
			if (name != "")
			{
				bundleName = $"{name}.unity3d";
			}
            if (importer.assetBundleName == bundleName) return;
			importer.assetBundleName = bundleName;
		}

		private static void SetAtlas(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if (textureImporter == null)
			{
				return;
			}

			if (textureImporter.spritePackingTag != "" && overwrite == false)
			{
				return;
			}

			textureImporter.spritePackingTag = name;
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
		}

		private static void SetBundleAndAtlas(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js" || extension == ".mat")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return;
			}

			if (importer.assetBundleName == "" || overwrite)
			{
				string bundleName = "";
				if (name != "")
				{
					bundleName = $"{name}.unity3d";
				}

				importer.assetBundleName = bundleName;
			}

			TextureImporter textureImporter = importer as TextureImporter;
			if (textureImporter == null)
			{
				return;
			}

			if (textureImporter.spritePackingTag == "" || overwrite)
			{
				textureImporter.spritePackingTag = name;
				AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
			}
		}

        private static void SetBundleIcon(string dir,bool subDir =true)
        {
            List<string> paths = EditorResHelper.GetAllResourcePath(dir, subDir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);
                SetBundle(path1, "Icon", true);
            }
        }

        private static void SetBundleMat(string dir, bool subDir = true)
        {
            List<string> paths = EditorResHelper.GetAllResourcePath(dir, subDir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);
                SetBundle(path1, go.name, true);
            }
        }

        private static void SetBundleHumanTexture(string dir, bool subDir = true)
        {
            List<string> paths = EditorResHelper.GetAllResourcePath(dir, subDir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);
                SetBundle(path1, go.name, true);
            }
        }
    }
}