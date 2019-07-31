using System;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class UIType
    {
        //注册时手动添加
        public const string UIWait = "UIWait";
        public const string UIEventMask = "UIEventMask";
        public const string UIFrameShow = "UIFrameShow";
        public const string UISceneLoading = "UISceneLoading";
        public const string UILogin = "UILogin";

        private static Dictionary<string, Type> UITypeComponentDic = new Dictionary<string, Type>();
        public static void AddUITypeComponent()
        {
            UITypeComponentDic.Clear();
            //注册时手动添加
            DicAddUIType(UIWait, typeof(UIWaitComponent));
            DicAddUIType(UIEventMask, typeof(UIEventMaskComponent));
            DicAddUIType(UIFrameShow, typeof(UIFrameShowComponent));
            DicAddUIType(UISceneLoading, typeof(UISceneLoadingComponent));
            DicAddUIType(UILogin, typeof(UILoginComponent));
        }
        
        public static Type GetComponentTypeByString(string type)
        {
            return UITypeComponentDic[type];
        }

        private static void DicAddUIType(string uiType, Type typeComponent)
        {
            if (!UITypeComponentDic.ContainsKey(uiType))
            {
                UITypeComponentDic.Add(uiType, typeComponent);
            }
            else
            {
                Log.Debug($"已经存在同类UIBaseComponent: {typeComponent.ToString()}");
                throw new Exception($"已经存在同类UIBaseComponent: {typeComponent.ToString()}");
            }
        }

    }
}