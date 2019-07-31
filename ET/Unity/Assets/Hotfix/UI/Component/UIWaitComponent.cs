using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIWaitComponentAwakeSystem : AwakeSystem<UIWaitComponent>
    {
        public override void Awake(UIWaitComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
	public class UIWaitComponentUpdateSystem : UpdateSystem<UIWaitComponent>
	{
		public override void Update(UIWaitComponent self)
		{
			if (!self.InShow) return;
			self.Update();
		}
	}
	
	public class UIWaitComponent : UIBaseComponent
	{
        private Transform trans;
        private WaitData waitData;
        GameObject go_GoAwait;
        private float speedRotate = 360f;
        private const float defaultShowRotateTime = 2f;
        float currShowUITime = 0f;

        public void Awake()
		{
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            trans= rc.Get<GameObject>("UIWait").transform;
            go_GoAwait = rc.Get<GameObject>("GoWait");
            go_GoAwait.SetActive(false);
            waitData = new WaitData();
            waitData.transform = trans;
            waitData.InitUI();
        }
        public override void Show()
        {
            base.Show();
            currShowUITime = 0f;
            go_GoAwait.SetActive(false);
        }

        public void Update()
        {
            waitData.image_ImageWaitFg.transform.Rotate(Vector3.forward*Time.unscaledDeltaTime*speedRotate);
            currShowUITime += Time.deltaTime;
            if (currShowUITime > defaultShowRotateTime)
            {
                go_GoAwait.SetActive(true);
            }
        }

    }
}
