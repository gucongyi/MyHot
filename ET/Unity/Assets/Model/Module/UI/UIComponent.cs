using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
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
        private GameObject Root;
        //ReferenceCollector rcRoot;
        private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();
		private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();
        private Dictionary<string, GameObject> m_allLayers = new Dictionary<string, GameObject>();

        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
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

			this.uis.Clear();
			this.UiTypes.Clear();
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
			this.UiTypes.Clear();
            
			List<Type> types = Game.EventSystem.GetTypes();

			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof (UIFactoryAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
				if (UiTypes.ContainsKey(attribute.Type))
				{
                    Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
					throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
				}
				object o = Activator.CreateInstance(type);
				IUIFactory factory = o as IUIFactory;
				if (factory == null)
				{
					Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
					continue;
				}
				this.UiTypes.Add(attribute.Type, factory);
			}
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
                UILayerType.TopMost
            };
            this.m_allLayers.Clear();
            Camera uiCamera = GameObject.Find("Global/Camera/UICamera").GetComponent<Camera>();
            int canvasScalerMatch = CanvasScalerMatch();
            for (int i = 0; i < _names.Length; i++)
            {
                GameObject _go = GameObject.Find(string.Format("Global/UI/{0}", _names[i]));
                if(!_go)
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
                    _scale.matchWidthOrHeight = canvasScalerMatch;

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
        }

        private int CanvasScalerMatch()
        {
            var nowHeightWidth = Screen.width / (float)Screen.height;
            var defaultHeightWidth = 16 / 9.0f;
            if (nowHeightWidth >= defaultHeightWidth)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 如果正在显示，不做处理
        /// 如果在hide ，显示出来
        /// 其他情况就创建一个新的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public UI Create(string type)
		{
			try
            {
                UI ui = Get(type);
                if (ui == null)
                {
                    ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
                    uis.Add(type, ui);
                }
                else
                {
                    if (ui.UiComponent.InShow) return null;
                }

                SetViewParent(ui, ui.GameObject.GetComponent<CanvasConfig>().CanvasName);
                ui.UiComponent.Show();
                return ui;
			}
			catch (Exception e)
			{
				throw new Exception($"{type} UI 错误: {e}");
			}
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
            uis[type].UiComponent.Dispose();
        }

        /// <summary>
        /// 设置ui显示层级
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="layer"></param>
        private void SetViewParent(UI ui, string layer)
        {
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
            UiTypes[type].Remove(type);
            uis.Remove(type);
            ui.Dispose();
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
				UiTypes[type].Remove(type);
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
                //if (kv.Key.Equals(UIType.UIFrameShow))
                //    continue;
                Hide(kv.Key);
            }
        }
    }
}