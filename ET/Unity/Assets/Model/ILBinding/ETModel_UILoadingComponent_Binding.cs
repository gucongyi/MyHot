using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_UILoadingComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.UILoadingComponent);

            field = type.GetField("view", flag);
            app.RegisterCLRFieldGetter(field, get_view_0);
            app.RegisterCLRFieldSetter(field, set_view_0);


        }



        static object get_view_0(ref object o)
        {
            return ((ETModel.UILoadingComponent)o).view;
        }
        static void set_view_0(ref object o, object v)
        {
            ((ETModel.UILoadingComponent)o).view = (ETModel.UILoadingComponent.View)v;
        }


    }
}
