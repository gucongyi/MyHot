using ETModel;
using System;
using System.Threading.Tasks;

namespace ETHotfix
{
    /// <summary>
    /// 添加dispose自动打断await功能,用法如下
    /// 第一种
    ///  await RelyAwait(Task.Delay(100));
    ///  var num = await RelyAwait(GetInt());
    ///  第二种
    ///  var p = awaitBreakPoint;
    ///  await task;
    ///  await p;
    /// </summary>
    public abstract class UIBaseComponent : Component
    {
        public event Action OnCloseOneTime;
        public event Action OnShow;
        public event Action OnClose;


        public bool InShow { get { return Layer != UILayerType.Hide; } }

        public string Layer { get; set; } = UILayerType.Hide;

        protected AwaitBreakPoint awaitBreakPoint { private set; get; } = new AwaitBreakPoint();
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
        }

        public virtual void AnimationOnShow()
        {

        }

        public virtual void AnimationOnClose()
        {

        }
        protected async Task RelyAwait(Task task)
        {
            var p = awaitBreakPoint;
            await task;
            await p;
        }
        protected async Task<T> RelyAwait<T>(Task<T> task)
        {
            var p = awaitBreakPoint;
            var r = await task;
            await p;
            return r;
        }

        public override void Dispose()
        {
            base.Dispose();
            awaitBreakPoint.End();
            awaitBreakPoint = new AwaitBreakPoint();
            OnCloseOneTime = null;
            OnShow = null;
            OnClose = null;
            Layer = UILayerType.Hide;
        }
    }
}
