namespace ETModel
{
    [Event(EventIdType.LoadingFinish)]
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent
    {
        public override void Run()
        {
            ZLog.Error("移除UILoading ");
			Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILoading);
        }
    }
}
