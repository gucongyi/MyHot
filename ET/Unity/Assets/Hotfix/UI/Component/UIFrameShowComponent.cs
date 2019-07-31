using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIFrameShowComponentAwakeSystem : AwakeSystem<UIFrameShowComponent>
    {
        public override void Awake(UIFrameShowComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
	public class UIFrameShowComponentUpdateSystem : UpdateSystem<UIFrameShowComponent>
	{
		public override void Update(UIFrameShowComponent self)
		{
			if (!self.InShow) return;
			self.Update();
		}
	}
	
	public class UIFrameShowComponent : UIBaseComponent
	{
        private Transform trans;
        private FrameShowData frameShowData;
        int TimesEachSeconds;
        float currTime;
        public void Awake()
		{
            TimesEachSeconds = 0;
            currTime = 0f;
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            trans= rc.Get<GameObject>("UIFrameShow").transform;
            frameShowData = new FrameShowData();
            frameShowData.transform = trans;
            frameShowData.InitUI();

            frameShowData.text_FrameCount.gameObject.SetActive(ETModel.Define.isShowFPS);
            frameShowData.text_FrameCount.text = Application.targetFrameRate + " FPS";//初始值
        }

        public void Update()
        {
            currTime += Time.unscaledDeltaTime;
            TimesEachSeconds++;
            if (currTime>=1f)
            {
                frameShowData.text_FrameCount.text = TimesEachSeconds + " FPS " + Mathf.Round(10000f / TimesEachSeconds) / 10 + "ms";
                TimesEachSeconds = 0;
                currTime = 0f;
            }
            //float current = 0;
            //current = (int)(1f / Time.unscaledDeltaTime);
            //frameShowData.text_FrameCount.text = current + " FPS";
        }

    }
}
