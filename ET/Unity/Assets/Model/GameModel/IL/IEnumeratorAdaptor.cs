using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
/*System.Collections.Generic.IEnumerator<System.Single>.get_Current
    System.IDisposable.Dispose
    "System.Collections.IEnumerator.Reset"
    "System.Collections.IEnumerator.get_Current"*/
namespace ETModel
{
    [ILAdapter]
    public class IEnumeratorBindingAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(System.Collections.Generic.IEnumerator<System.Single>);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(IEnumeratorAdaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new IEnumeratorAdaptor(appdomain, instance);
        }

        public class IEnumeratorAdaptor : System.Collections.Generic.IEnumerator<System.Single>, CrossBindingAdaptorType
        {
            private ILTypeInstance instance;
            private ILRuntime.Runtime.Enviorment.AppDomain appDomain;

            private IMethod mMoveNext;
            private IMethod mReset;
            private IMethod mCurrent;
            private IMethod mDispose;

            public IEnumeratorAdaptor()
            {
            }

            public IEnumeratorAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appDomain, ILTypeInstance instance)
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

            public bool MoveNext()
            {
                if (this.mMoveNext == null)
                {
                    mMoveNext = instance.Type.GetMethod("MoveNext", 0);
                }
                return (bool)this.appDomain.Invoke(mMoveNext, instance, null);
            }

            public void Reset()
            {
                if (this.mReset == null)
                {
                    mReset = instance.Type.GetMethod("System.Collections.IEnumerator.Reset", 0);
                }
                this.appDomain.Invoke(mReset, instance, null);
            }

            public void Dispose()
            {
                if (this.mDispose == null)
                {
                    mDispose = instance.Type.GetMethod("System.IDisposable.Dispose", 0);
                }
                this.appDomain.Invoke(mDispose, instance, null);
            }

            public System.Single Current
            {
                get
                {
                    if (this.mCurrent == null)
                    {
                        mCurrent = instance.Type.GetMethod("System.Collections.Generic.IEnumerator<System.Single>.get_Current", 0);
                    }
                    return (System.Single)this.appDomain.Invoke(mCurrent, instance, null);
                } 
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }  

            bool IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            void IEnumerator.Reset()
            {
                Reset();
            }

            void IDisposable.Dispose()
            {
                Dispose();
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
