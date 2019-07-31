using UnityEngine;
#if UNITY_EDITOR
#endif

namespace ETModel
{
    public class ABInfo : Component
	{
		private int refCount;
        public bool IsLoadAllAssets = false;
		public string Name { get; }

		public int RefCount
		{
			get
			{
				return this.refCount;
			}
			set
			{
				//Log.Debug($"{this.Name} refcount: {value}");
				this.refCount = value;
			}
		}

		public AssetBundle AssetBundle { get; }

		public ABInfo(string name, AssetBundle ab)
		{
			this.Name = name;
			this.AssetBundle = ab;
			this.RefCount = 1;
			//Log.Debug($"load assetbundle: {this.Name}");
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			//Log.Debug($"desdroy assetbundle: {this.Name}");

			this.AssetBundle?.Unload(true);
		}
	}
}