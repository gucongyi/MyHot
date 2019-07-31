using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UiComponentLoadSystem : LoadSystem<UIComponent>
	{
		public override void Load(UIComponent self)
		{
			self.Load();
		}
	}

	/// <summary>
	/// 管理所有UI
	/// </summary>
	public class UIComponent: Component
	{
        public UiBgMaskSystem BgMaskmanager { get; set; } = new UiBgMaskSystem();
		private GameObject Root;
        //ReferenceCollector rcRoot;
        private UIFactory uiFactory;
        public Camera uiCamera;
        //private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();
        private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();
        private Dictionary<string, GameObject> m_allLayers = new Dictionary<string, GameObject>();
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
            BgMaskmanager = null;

            foreach (string type in uis.Keys.ToArray())
            {
                UI ui;
                if (!uis.TryGetValue(type, out ui))
                {
                    continue;
                }
                uis.Remove(type);
                ui.Dispose();
            }

			//this.UiTypes.Clear();
			this.uis.Clear();
		}

		public void Awake()
		{
			this.Root = GameObject.Find("Global/UI/");
            //this.rcRoot = this.Root.GetComponent<ReferenceCollector>();
            this.InstantiateUi(Root.transform);
            this.Load();
		}

		public void Load()
		{
			//UiTypes.Clear();
            UIType.AddUITypeComponent();
            uiFactory = new UIFactory();
            //List<Type> types = ETModel.Game.Hotfix.GetHotfixTypes();

			//foreach (Type type in types)
			//{
			//	object[] attrs = type.GetCustomAttributes(typeof (UIFactoryAttribute), false);
			//	if (attrs.Length == 0)
			//	{
			//		continue;
			//	}

			//	UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
			//	if (UiTypes.ContainsKey(attribute.Type))
			//	{
   //                 Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
			//		throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
			//	}
			//	object o = Activator.CreateInstance(type);
			//	IUIFactory factory = o as IUIFactory;
			//	if (factory == null)
			//	{
			//		Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
			//		continue;
			//	}
			//	this.UiTypes.Add(attribute.Type, factory);
			//}
		}

        /// <summary>
        /// 初始化UI设置，建立层级结构
        /// </summary>
        /// <param name="parent"></param>
        private void InstantiateUi(Transform parent)
        {
            //此处务必按照显示层级由低到高排序
            string[] _names = new String[] {
                UILayerType.Hide,
                UILayerType.Bottom,
                UILayerType.Medium,
                UILayerType.Top,
                //UILayerType.TopMost
            };
            this.m_allLayers.Clear();
            uiCamera = GameObject.Find("Global/Camera/UICamera").GetComponent<Camera>();
            for (int i = 0; i < _names.Length; i++)
            {
                GameObject _go = GameObject.Find(string.Format("Global/UI/{0}", _names[i]));
                if (!_go)
                {
                    _go = new GameObject();

                    Canvas _canvas = _go.AddComponent<Canvas>();
                    _canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    _canvas.worldCamera = uiCamera;
                    _canvas.sortingOrder = i * 1000;
                    _canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)(1 | 1 << 3 | 1 << 4);

                    CanvasScaler _scale = _go.AddComponent<CanvasScaler>();
                    _scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    _scale.referenceResolution = new Vector2(1280, 720);
                    _scale.matchWidthOrHeight = 0;

                    GraphicRaycaster _graphic = _go.AddComponent<GraphicRaycaster>();
                    _go.name = _names[i];
                    _go.transform.SetParent(parent);
                    _go.transform.localPosition = Vector3.zero;

                    if (_names[i] == UILayerType.Hide)
                    {
                        _go.layer = LayerMask.NameToLayer("UIHide");
                        _graphic.enabled = false;
                    }
                    else
                    {
                        _go.layer = LayerMask.NameToLayer("UI");
                    }
                    //this.rcRoot.Add(_names[i], _go);
                }
                this.m_allLayers.Add(_names[i], _go);
            }
            InstantiateTopMostUiAndFit18And9(parent);
        }
        /// <summary>
        /// 临时高度适配
        /// </summary>
        /// <param name="parent"></param>
        async void InstantiateTopMostUiAndFit18And9(Transform parent)
        {
            //不受UI组件控制，用来放适配黑边的
            string nameTopMost = UILayerType.TopMost;
            //实例化topMost
            GameObject goTop = GameObject.Find(string.Format("Global/UI/{0}", nameTopMost));
            if (!goTop)
            {
                goTop = new GameObject();

                Canvas _canvas = goTop.AddComponent<Canvas>();
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
                _canvas.worldCamera = uiCamera;
                _canvas.sortingOrder = 10 * 1000;
                _canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)(1 | 1 << 3 | 1 << 4);

                CanvasScaler _scale = goTop.AddComponent<CanvasScaler>();
                _scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                _scale.referenceResolution = new Vector2(1280, 720);
                _scale.matchWidthOrHeight = 0;

                GraphicRaycaster _graphic = goTop.AddComponent<GraphicRaycaster>();
                goTop.name = nameTopMost;
                goTop.transform.SetParent(parent);
                goTop.transform.localPosition = Vector3.zero;
                goTop.layer = LayerMask.NameToLayer("UI");
            }
            Transform trans = goTop.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                GameObject.Destroy(trans.GetChild(0));
            }
            //实例化黑边
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
        }
        public async Task<T> CreateAsync<T>(string type)where T :UIBaseComponent
        {
            return (await CreateAsync(type)).UiComponent as T;
        }

        /// <summary>
        /// 如果正在显示，不做处理
        /// 如果在hide ，显示出来
        /// 其他情况就创建一个新的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UI> CreateAsync(string type)
        {
            UI ui = Get(type);
            if (ui!=null&& ui.UiComponent.InShow)
            {
                return ui;
            }
            //打开遮罩，阻挡点击事件,只有一个控件，很快的
            UI uiEventMask = await CreateUIEventMask();

            await semaphoreSlim.WaitAsync();
            bool isRelease = false;
            //信号量释放后要重新刷新堵塞前的状态
            ui = Get(type);
            if (ui != null && ui.UiComponent.InShow)
            {
                ReleaseRelated(uiEventMask, ref isRelease);
                return ui;
            }
            try
            {
                if (ui == null)
                {
                    //ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
                    ui = await uiFactory.CreateAsync(type);
                    uis.Add(type, ui);
                }
                if (ui.GameObject == null)
                {
                    ZLog.Error(type + "CreateAsync  ui.GameObject==null");
                    //return null;
                }
                else
                {
                    SetViewParent(ui, ui.GameObject.GetComponent<CanvasConfig>()?.CanvasName);
                    ui.ShowUI();
                    AddCanvas(ui.GameObject);
                    
                }

                //热更层不会走finally,所有这里直接释放掉
                ReleaseRelated(uiEventMask, ref isRelease);
                return ui;
            }
            catch (Exception e)
            {
                throw new Exception($"{type} UI 错误: {e}");
            }
            finally
            {
                ReleaseRelated(uiEventMask, ref isRelease);
            }
        }

        private void ReleaseRelated(UI uiEventMask, ref bool isRelease)
        {
            if (!isRelease)
            {
                semaphoreSlim.Release();
                //关闭遮罩，可以继续点击
                HideUIEventMask(uiEventMask);
                isRelease = true;
            }
        }

        private void HideUIEventMask(UI uiEventMask)
        {
            uiEventMask.UiComponent.Hide();
            SetViewParent(uiEventMask, UILayerType.Hide);
        }

        private async Task<UI> CreateUIEventMask()
        {
            UI uiEventMask = Get(UIType.UIEventMask);
            if (uiEventMask == null)
            {
                uiEventMask = await uiFactory.CreateAsync(UIType.UIEventMask);
                uis.Add(UIType.UIEventMask, uiEventMask);
            }
            SetViewParent(uiEventMask, UILayerType.Top);
            uiEventMask.UiComponent.Show();
            AddCanvas(uiEventMask.GameObject);
            return uiEventMask;
        }

        /// <summary>
        /// 隐藏ui
        /// </summary>
        /// <param name="type"></param>
        public void Hide(string type)
        {
            UI ui = Get(type);
            if (ui == null) return;
            uis[type].UiComponent.Hide();
            SetViewParent(uis[type], UILayerType.Hide);
            string canvasName = "";
            CanvasConfig canvasConfig = ui.GameObject.GetComponent<CanvasConfig>();
            if (canvasConfig != null)
            {
                canvasName = canvasConfig.CanvasName;
            }
            ChangeOrderOnRemove(canvasName);
            BgMaskmanager.UpdateUI();
        }

        /// <summary>
        /// 设置ui显示层级
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="layer"></param>
        private void SetViewParent(UI ui, string layer)
        {
            if (string.IsNullOrEmpty(layer))
            {
                //公共货币栏
                return;
            }
            RectTransform _rt = ui.GameObject.GetComponent<RectTransform>();
            _rt.SetParent(m_allLayers[layer].transform);
            _rt.anchorMin = Vector2.zero;
            _rt.anchorMax = Vector2.one;
            _rt.offsetMax = Vector2.zero;
            _rt.offsetMin = Vector2.zero;
            _rt.pivot = new Vector2(0.5f, 0.5f);
            _rt.localScale = Vector3.one;
            _rt.localPosition = Vector3.zero;
            _rt.localRotation = Quaternion.identity;
            ui.UiComponent.Layer = layer;
            if (!layer.Equals(UILayerType.Hide)) ui.SetAsLastSibling();
        }

        public void Add(string type, UI ui)
		{
			this.uis.Add(type, ui);
		}
 
		public void Remove(string type)
		{
            UI ui;
            if (!uis.TryGetValue(type, out ui))
            {
                return;
            }
            //UiTypes[type].Remove(type);
            uiFactory.Remove(type);
            uis.Remove(type);
            string canvasName = "";
            if (ui.GameObject != null)
            {
                CanvasConfig canvasConfig = ui.GameObject.GetComponent<CanvasConfig>();
                if (canvasConfig != null)
                {
                    canvasName = canvasConfig.CanvasName;
                }

            }
            ui.Dispose();
            ChangeOrderOnRemove(canvasName);
        }

		public void RemoveAll()
		{
			foreach (string type in this.uis.Keys.ToArray())
			{
                UI ui;
                if (!this.uis.TryGetValue(type, out ui))
                {
                    continue;
                }
                //UiTypes[type].Remove(type);
                uiFactory.Remove(type);
                this.uis.Remove(type);
                ui.Dispose();
            }
		}


        public void ClearAllUI()
        {
            RemoveAll();
            ReferenceCollector.ClearAllObject(m_allLayers);
        }

        public UI Get(string type)
		{
            UI ui;
            this.uis.TryGetValue(type, out ui);
            return ui;
		}

		public List<string> GetUITypeList()
		{
			return new List<string>(this.uis.Keys);
		}

        public void HideAll()
        {
            foreach (var kv in uis)
            {
                if (kv.Key.Equals(UIType.UIFrameShow))
                    continue;
                Hide(kv.Key);
            }
        }

        #region

        private void AddCanvas(GameObject uiGameobject)
        {
            CanvasConfig canvasConfig = uiGameobject.GetComponent<CanvasConfig>();
            if (canvasConfig == null)
                return;
            GameObject parentGameobject = m_allLayers[canvasConfig.CanvasName];
            int sortingOrder = parentGameobject.GetComponent<Canvas>().sortingOrder + (parentGameobject.transform.childCount - 1) * 100; 
            Canvas _canvas = uiGameobject.GetComponent<Canvas>();
            if (_canvas == null)
            {
                _canvas = uiGameobject.AddComponent<Canvas>();
            }
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = sortingOrder;
            _canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)(1 | 1 << 3 | 1 << 4);
            GraphicRaycaster _graphic = uiGameobject.GetComponent<GraphicRaycaster>();
            if (_graphic == null)
            {
                _graphic = uiGameobject.AddComponent<GraphicRaycaster>();
            }

            ChangeOrderRender(uiGameobject.transform, sortingOrder);
        }

        public void ChangeOrderRender(Transform transform, int sortingOrder)
        {
            if (transform == null)
                return;

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                renderer.sortingLayerID = SortingLayer.NameToID("Default");
                int remain = renderer.sortingOrder % 100;
                renderer.sortingOrder = remain + sortingOrder;
            }

            Canvas[] canvass = transform.GetComponentsInChildren<Canvas>(true);
            foreach (var canvas in canvass)
            {
                if (FilterDropdown(canvas))
                {
                    continue;
                }
                canvas.sortingLayerID = SortingLayer.NameToID("Default");
                int remain = canvas.sortingOrder % 100;
                canvas.sortingOrder = remain + sortingOrder;
            }

            UnityEngine.Rendering.SortingGroup[] sortingGroups = transform.GetComponentsInChildren<UnityEngine.Rendering.SortingGroup>(true);
            foreach (var sortingGroup in sortingGroups)
            {
                sortingGroup.sortingLayerID = SortingLayer.NameToID("Default");
                int remain = sortingGroup.sortingOrder % 100;
                sortingGroup.sortingOrder = remain + sortingOrder;
            }
        }

        private void ChangeOrderOnRemove(string canvasName)
        {
            if (string.IsNullOrEmpty(canvasName))
            {
                return;
            }
            GameObject parentGameobject = m_allLayers[canvasName];
            int parentOrder = parentGameobject.GetComponent<Canvas>().sortingOrder;
            int childCount = parentGameobject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                int sortingOrder = parentOrder + (i * 100);
                Transform childTransform = parentGameobject.transform.GetChild(i);
                childTransform.GetComponent<Canvas>().sortingOrder = sortingOrder;
                ChangeOrderRender(childTransform, sortingOrder);
            }
        }

        //Dropdown内部控制sortOrder且没有接口开放出来，在改变ui的sortOrder时过滤掉Dropdown
        private bool FilterDropdown(Canvas canvas)
        {
            if (string.Equals(canvas.gameObject.name, "Template"))
            {
                if (canvas.transform.parent.GetComponent<Dropdown>() != null)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}