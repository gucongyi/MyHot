using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ETModel
{
    public static class ILHelper
    {
        public static unsafe void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appDomain)
        {
            // 注册重定向函数

            // 注册委托
            appDomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appDomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
            appDomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appDomain.DelegateManager.RegisterMethodDelegate<Quick.UI.Tab>();  
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.HTTPRequest, BestHTTP.HTTPResponse>();
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.WebSocket.WebSocket>();
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.WebSocket.WebSocket, System.String>();
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.WebSocket.WebSocket, System.UInt16, System.String>();
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.WebSocket.WebSocket, System.Exception>();
            appDomain.DelegateManager.RegisterMethodDelegate<BestHTTP.WebSocket.WebSocket, System.Byte[]>();


            appDomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
            appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Boolean>();
            appDomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance,System.Int64>();

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketOpenDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketOpenDelegate((webSocket) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket>)act)(webSocket);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketMessageDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketMessageDelegate((webSocket, message) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket,string>)act)(webSocket,message);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketBinaryDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketBinaryDelegate((webSocket,data) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket,byte[]>)act)(webSocket,data);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketClosedDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketClosedDelegate((webSocket, code,message) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket, UInt16 , string >)act)(webSocket, code, message);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketErrorDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketErrorDelegate((webSocket, ex) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket,System.Exception>)act)(webSocket, ex);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.WebSocket.OnWebSocketErrorDescriptionDelegate>((act) =>
            {
                return new BestHTTP.WebSocket.OnWebSocketErrorDescriptionDelegate((webSocket, reason) =>
                {
                    ((Action<BestHTTP.WebSocket.WebSocket, string>)act)(webSocket, reason);
                });
            });

           

            appDomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int32>>((act) =>
            {
                return new System.Predicate<System.Int32>((obj) =>
                {
                    return ((Func<System.Int32, System.Boolean>)act)(obj);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.OnRequestFinishedDelegate>((act) =>
            {
                return new BestHTTP.OnRequestFinishedDelegate((originalRequest, response) =>
                {
                    ((Action<BestHTTP.HTTPRequest, BestHTTP.HTTPResponse>)act)(originalRequest, response);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    //((Action<>)act)();
                    ((System.Action)act)();
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() =>
                {
                    ((System.Action)act)();
                });
            });

            appDomain.DelegateManager.RegisterMethodDelegate<Quick.UI.Tab, UnityEngine.EventSystems.PointerEventData>();
            appDomain.DelegateManager.RegisterDelegateConvertor<Quick.UI.Tab.PointerTabFunc>((act) =>
            {
                return new Quick.UI.Tab.PointerTabFunc((target, eventData) =>
                {
                    ((Action<Quick.UI.Tab, UnityEngine.EventSystems.PointerEventData>)act)(target, eventData);
                });
            });
            appDomain.DelegateManager.RegisterFunctionDelegate<System.IO.FileInfo, System.IO.FileInfo, System.Int32>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>)act)(arg0);
                });
            });


            appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appDomain.DelegateManager.RegisterMethodDelegate<Quick.UI.TabGroup, UnityEngine.EventSystems.PointerEventData>();
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
            appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32>(); 
            appDomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
            appDomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
            appDomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int64>>((act) =>
            {
                return new System.Predicate<System.Int64>((obj) =>
                {
                    return ((Func<System.Int64, System.Boolean>)act)(obj);
                });
            });
            appDomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Boolean>();

            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Video.VideoPlayer>();
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Video.VideoPlayer.EventHandler>((act) =>
            {
                return new UnityEngine.Video.VideoPlayer.EventHandler((source) =>
                {
                    ((Action<UnityEngine.Video.VideoPlayer>)act)(source);
                });
            });
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                {
                    ((Action<UnityEngine.Vector2>)act)(arg0);
                });
            });
            appDomain.DelegateManager.RegisterMethodDelegate<System.Single>();



            appDomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.IO.FileInfo>>((act) =>
            {
                return new System.Comparison<System.IO.FileInfo>((x, y) =>
                {
                    return ((Func<System.IO.FileInfo, System.IO.FileInfo, System.Int32>)act)(x, y);
                });
            });


            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int64>>((act) =>
            {
                return new System.Predicate<System.Int64>((obj) =>
                {
                    return ((Func<System.Int64, System.Boolean>)act)(obj);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
            {
                return new System.Comparison<System.Int32>((x, y) =>
                {
                    return ((Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
                });
            });

            

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
                {
                    ((Action<System.Int32>)act)(arg0);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
                {
                    ((Action<System.Single>)act)(arg0);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>>((act) =>
            {
                return new System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>((input) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>)act)(input);
                });
            });
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            appDomain.DelegateManager.RegisterMethodDelegate<ETModel.AEvent>();

            appDomain.DelegateManager.RegisterMethodDelegate<System.String, System.String>();
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Text>();



            CLRBindings.Initialize(appDomain);

            // 注册适配器
            Assembly assembly = typeof(Init).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                object obj = Activator.CreateInstance(type);
                CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
                if (adaptor == null)
                {
                    continue;
                }
                appDomain.RegisterCrossBindingAdaptor(adaptor);
            }
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
            //ILSerial.RegisterILRuntimeCLRRedirection(appDomain);
        }
        private static object PType_CreateInstance(ILRuntime.Runtime.Enviorment.AppDomain appDomain, string typeName)
        {
            return appDomain.Instantiate(typeName);
        }
    }
}