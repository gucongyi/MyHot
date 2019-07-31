using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIEventMaskComponentAwakeSystem : AwakeSystem<UIEventMaskComponent>
    {
        public override void Awake(UIEventMaskComponent self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
	public class UIEventMaskComponentUpdateSystem : UpdateSystem<UIEventMaskComponent>
	{
		public override void Update(UIEventMaskComponent self)
		{
			if (!self.InShow) return;
			self.Update();
		}
	}
	
	public class UIEventMaskComponent : UIBaseComponent
	{

        public void Awake()
		{
        }

        public void Update()
        {
        }

    }
}
