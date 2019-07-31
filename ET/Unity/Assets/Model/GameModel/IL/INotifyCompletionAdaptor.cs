using System;
using System.Runtime.CompilerServices;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.TypeSystem;

namespace ETModel
{
    [ILAdapter]
    public class INotifyCompletionClassInheritanceAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(INotifyCompletion);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(INotifyCompletionAdaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new INotifyCompletionAdaptor(appdomain, instance);
        }

        public class INotifyCompletionAdaptor : INotifyCompletion, CrossBindingAdaptorType
        {
            private ILTypeInstance instance;
            private ILRuntime.Runtime.Enviorment.AppDomain appDomain;

            private IMethod iNotifyCompletion;
            private readonly object[] param0 = new object[1];

            public INotifyCompletionAdaptor()
            {
            }

            public INotifyCompletionAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appDomain, ILTypeInstance instance)
            {
                this.appDomain = appDomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance
            {
                get
                {
                    return instance;
                }
            }

            public void OnCompleted(Action continuation)
            {
                if (this.iNotifyCompletion == null)
                {
                    this.iNotifyCompletion = instance.Type.GetMethod("OnCompleted");
                    if(this.iNotifyCompletion==null)
                    {
                        IType iNotifyCompletionType = instance.Type.BaseType;
                        while(iNotifyCompletionType!=null)
                        {
                            this.iNotifyCompletion = iNotifyCompletionType.GetMethod("OnCompleted", 1);
                            if (this.iNotifyCompletion == null)
                            {
                                iNotifyCompletionType = iNotifyCompletionType.BaseType;
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                    }
                }
                this.param0[0] = continuation;
                this.appDomain.Invoke(iNotifyCompletion, instance, this.param0);
            }

            public override string ToString()
            {
                IMethod m = this.appDomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }

                return instance.Type.FullName;
            } 
        }
    }
}
