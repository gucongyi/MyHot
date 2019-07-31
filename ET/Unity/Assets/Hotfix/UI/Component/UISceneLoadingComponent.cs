using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UISceneLoadingComponentAwakeSystem : AwakeSystem<UISceneLoadingComponent>
    {
        public override void Awake(UISceneLoadingComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
	public class UISceneLoadingComponentUpdateSystem : UpdateSystem<UISceneLoadingComponent>
	{
		public override void Update(UISceneLoadingComponent self)
		{
			self.Update();
		}
	}
	
	public class UISceneLoadingComponent : UIBaseComponent
	{
        private Transform trans;
        private SceneLoadingData sceneLoadingData;
        int TimesEachSeconds;
        float currTime;
        private float realProgress;
        private float frameProgress;
        public void Awake()
		{
            realProgress = 0f;
            frameProgress = 0f;
            TimesEachSeconds = 0;
            currTime = 0f;
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            trans= rc.Get<GameObject>("UISceneLoading").transform;
            sceneLoadingData = new SceneLoadingData();
            sceneLoadingData.transform = trans;
            sceneLoadingData.InitUI();

            sceneLoadingData.image_ProgressForward_3.fillAmount = 0;
        }

        public void OnRealProgressCallback(float realProgress)
        {
            this.realProgress = realProgress;
            ShowProgress();
        }

        private void ShowProgress()
        {
            //float minProgress = Mathf.Min(this.realProgress, this.frameProgress);
            //SetSliderValue( minProgress/100f);
            SetSliderValue(realProgress / 100f);
        }

        public void OnFrameProgressCallback(float frameProgress)
        {
            this.frameProgress = frameProgress;
            SetSliderValue(frameProgress/100f);
        }

        public void SetSliderValue(float progress)
        {
            sceneLoadingData.image_ProgressForward_3.fillAmount = progress;
        }

        public void Update()
        {
            
        }

    }
}
