using System;

namespace ETModel
{
    public abstract class UIBaseComponent : Component
    {
        public event Action OnCloseOneTime;
        public event Action OnShow;
        public event Action OnClose;


        public bool InShow { get { return Layer != UILayerType.Hide; } }

        public string Layer { get; set; } = UILayerType.Hide;


        public virtual void Show()
        {
            GetParent<UI>().GameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            GetParent<UI>().GameObject.SetActive(false);
            if (OnCloseOneTime != null)
            {
                OnCloseOneTime.Invoke();
                OnCloseOneTime = null;
            }
            OnClose?.Invoke();
            base.Dispose();
        }

        public virtual void AnimationOnShow()
        {

        }

        public virtual void AnimationOnClose()
        {

        }

        public override void Dispose()
        {
            base.Dispose();

            OnCloseOneTime = null;
            OnShow = null;
            OnClose = null;
            Layer = UILayerType.Hide;
        }
    }
}
