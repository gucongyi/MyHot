using ETModel;
using System;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiLoginComponentSystem : AwakeSystem<UILoginComponent>
    {
        public override void Awake(UILoginComponent self)
        {
            self.Awake();
        }
    }

    public class UILoginComponent : UIBaseComponent
    {
        private Transform trans;
        public Text text_HotfixText;

        public async void Awake()
        {
           
        }
        public override void Show()
        {
            base.Show();
            text_HotfixText = GetParent<UI>().GameObject.transform.Find("HotfixText").GetComponent<Text>();
            text_HotfixText.text = StaticTool.TabToyDataConfig.Test[0].Name;
        }
    }
}
