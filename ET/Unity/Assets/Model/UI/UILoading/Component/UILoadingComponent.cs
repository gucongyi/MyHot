using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ETModel
{
    [ObjectSystem]
	public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
	{
		public override void Awake(UILoadingComponent self)
		{
            self.Awake();
            //self.text = self.GetParent<UI>().GameObject.Get<GameObject>("Text").GetComponent<Text>();
        }
	}

    public class UILoadingComponent : UIBaseComponent
	{
		public View view;
        public BundleDownloadInfo DownLoadInfo;
        public Action UpdateProgress = () => Log.Info("UpdateProgress");

        private int realProgress;
	    private int downloadFrames;
	    public void BundleRealDownload(int progress)
	    {
	        //this.realProgress = progress;
            //ShowProgress();
        }
        public void UpdateShow()
        {
            view.text_txtCenter1.text = "正在下载";
            view.text_txtCenter.text = DownLoadInfo.SpeedString;
            view.text_txtRight.text = DownLoadInfo.AlreadyDownloadString;
            view.image_ProgressForward_3.fillAmount = DownLoadInfo.Progress / 100f;
        }
        private void ShowProgress()
        {
            //int minProgress = Mathf.Min(this.realProgress, this.downloadFrames);
            //view.text_txtCenter.text = $"{minProgress}%";
            //view.image_ProgressForward_3.fillAmount = minProgress/100f;
        }

        //public void OnFrameProgressCallback(float frameProgress)
        //{
        //    this.frameProgress = frameProgress;
        //    SetSliderValue(frameProgress / 100f);
        //}

        public void BundleDownloadError(string error)
	    {
	        view.text_txtCenter.text = $"{error}%";
        }
	    public void BundleDownloadFrames(int frames)
	    {
	        this.downloadFrames = frames;
            ShowProgress();
        }
        

        internal void Awake()
        {
            view = new View(GetParent<UI>().GameObject.transform);
            view.image_ProgressForward_3.fillAmount = 0;
            view.text_txtCenter.text = "";
            view.text_txtCenter1.text = "";
            view.text_txtRight.text = "";
        } 
         
        public class View
        {

            public Text text_txtLeft;
            public Text text_txtCenter;
            public Text text_txtCenter1;
            public Text text_txtRight;
            public Transform transform;
            public Image image_ProgressBackward_2;
            public Image image_ProgressForward_3;
            public GameObject go_m_LoadingProgress;

            public VideoPlayer videoPlayer;
            public RawImage rawImage;

            public View(Transform transform)
            {
                this.transform = transform;
                image_ProgressBackward_2 = transform.Find("m_LoadingProgress/ProgressBackward_2").GetComponent<Image>();
                image_ProgressForward_3 = transform.Find("m_LoadingProgress/ProgressForward_3").GetComponent<Image>();
                text_txtLeft = transform.Find("container/txtLeft").GetComponent<Text>();
                text_txtCenter = transform.Find("container/txtCenter").GetComponent<Text>();
                text_txtCenter1 = transform.Find("container/txtCenter1").GetComponent<Text>();
                text_txtRight = transform.Find("container/txtRight").GetComponent<Text>();
                go_m_LoadingProgress = transform.Find("m_LoadingProgress").gameObject;
                videoPlayer = transform.Find("Video").GetComponent<VideoPlayer>();
                rawImage = videoPlayer.GetComponent<RawImage>();
            } 
        }
    }
}
