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
    unsafe class ETModel_UILoadingComponent_Binding_View_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.UILoadingComponent.View);

            field = type.GetField("text_txtCenter1", flag);
            app.RegisterCLRFieldGetter(field, get_text_txtCenter1_0);
            app.RegisterCLRFieldSetter(field, set_text_txtCenter1_0);
            field = type.GetField("text_txtCenter", flag);
            app.RegisterCLRFieldGetter(field, get_text_txtCenter_1);
            app.RegisterCLRFieldSetter(field, set_text_txtCenter_1);
            field = type.GetField("text_txtRight", flag);
            app.RegisterCLRFieldGetter(field, get_text_txtRight_2);
            app.RegisterCLRFieldSetter(field, set_text_txtRight_2);
            field = type.GetField("text_txtLeft", flag);
            app.RegisterCLRFieldGetter(field, get_text_txtLeft_3);
            app.RegisterCLRFieldSetter(field, set_text_txtLeft_3);
            field = type.GetField("image_ProgressForward_3", flag);
            app.RegisterCLRFieldGetter(field, get_image_ProgressForward_3_4);
            app.RegisterCLRFieldSetter(field, set_image_ProgressForward_3_4);


        }



        static object get_text_txtCenter1_0(ref object o)
        {
            return ((ETModel.UILoadingComponent.View)o).text_txtCenter1;
        }
        static void set_text_txtCenter1_0(ref object o, object v)
        {
            ((ETModel.UILoadingComponent.View)o).text_txtCenter1 = (UnityEngine.UI.Text)v;
        }
        static object get_text_txtCenter_1(ref object o)
        {
            return ((ETModel.UILoadingComponent.View)o).text_txtCenter;
        }
        static void set_text_txtCenter_1(ref object o, object v)
        {
            ((ETModel.UILoadingComponent.View)o).text_txtCenter = (UnityEngine.UI.Text)v;
        }
        static object get_text_txtRight_2(ref object o)
        {
            return ((ETModel.UILoadingComponent.View)o).text_txtRight;
        }
        static void set_text_txtRight_2(ref object o, object v)
        {
            ((ETModel.UILoadingComponent.View)o).text_txtRight = (UnityEngine.UI.Text)v;
        }
        static object get_text_txtLeft_3(ref object o)
        {
            return ((ETModel.UILoadingComponent.View)o).text_txtLeft;
        }
        static void set_text_txtLeft_3(ref object o, object v)
        {
            ((ETModel.UILoadingComponent.View)o).text_txtLeft = (UnityEngine.UI.Text)v;
        }
        static object get_image_ProgressForward_3_4(ref object o)
        {
            return ((ETModel.UILoadingComponent.View)o).image_ProgressForward_3;
        }
        static void set_image_ProgressForward_3_4(ref object o, object v)
        {
            ((ETModel.UILoadingComponent.View)o).image_ProgressForward_3 = (UnityEngine.UI.Image)v;
        }


    }
}
