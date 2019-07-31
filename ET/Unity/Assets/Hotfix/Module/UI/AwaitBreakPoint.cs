using System;
using System.Runtime.CompilerServices;

namespace ETHotfix
{
    public class AwaitBreakPoint : INotifyCompletion
    {
        private bool isRun = true;
        private Action continuation;
        public AwaitBreakPoint()
        {
        }
        public bool IsCompleted
        {
            get
            {
                return isRun;
            }
        }
        public void End()
        {
            isRun = false;
        }
        public void Reset()
        {
            isRun = true;
        }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            //这个方法只有ui组件被dispose之后才会执行.不需要continuation
            //if (isRun)
            //{
            //    continuation();
            //}
        }
        public void GetResult()
        {

        }
        public AwaitBreakPoint GetAwaiter()
        {
            return this;
        }

    }
}
