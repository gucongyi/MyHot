using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            ILRuntime_Runtime_Enviorment_AppDomain_Binding.Register(app);
            ILRuntime_CLR_Method_CLRMethod_Binding.Register(app);
            ILRuntime_CLR_TypeSystem_IType_Binding.Register(app);
            ILRuntime_Runtime_Intepreter_ILIntepreter_Binding.Register(app);
            ILRuntime_Runtime_Stack_StackObject_Binding.Register(app);
            System_IO_FileStream_Binding.Register(app);
            System_IO_BinaryWriter_Binding.Register(app);
            System_IO_Stream_Binding.Register(app);
            System_IO_MemoryStream_Binding.Register(app);
            System_IO_BinaryReader_Binding.Register(app);
            System_Activator_Binding.Register(app);
            //ILRuntime_Reflection_ILRuntimeWrapperType_Binding.Register(app);
            ILRuntime_CLR_TypeSystem_CLRType_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_IType_Binding.Register(app);
            System_Collections_IList_Binding.Register(app);
            ILRuntime_Reflection_ILRuntimeType_Binding.Register(app);
            ILRuntime_CLR_TypeSystem_ILType_Binding.Register(app);
            System_Reflection_FieldInfo_Binding.Register(app);
            System_Collections_IDictionary_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_Single_Binding.Register(app);
            System_Double_Binding.Register(app);
            System_Int16_Binding.Register(app);
            System_UInt32_Binding.Register(app);
            System_UInt16_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_UInt64_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Collections_ICollection_Binding.Register(app);
            System_Collections_IEnumerable_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_DictionaryEntry_Binding.Register(app);
            ILRuntime_Runtime_Intepreter_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_FieldInfo_Array_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_Int32_Binding.Register(app);
            ILRuntime_CLR_Utils_Extensions_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            System_IO_File_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Exception_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_Enum_Binding.Register(app);
            System_Text_UTF8Encoding_Binding.Register(app);
            System_Diagnostics_Debug_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            ETModel_Log_Binding.Register(app);
            ETModel_IdGenerater_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding.Register(app);
            ETModel_UnOrderMultiMap_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_Int64_Binding.Register(app);
            ETModel_Game_Binding.Register(app);
            ETModel_Hotfix_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding_Enumerator_Binding.Register(app);
            ETModel_EventAttribute_Binding.Register(app);
            ETModel_EventProxy_Binding.Register(app);
            ETModel_EventSystem_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            ETModel_Component_Binding.Register(app);
            ETModel_Scene_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            ETModel_ABManager_Binding.Register(app);
            System_Threading_Tasks_Task_1_Sprite_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Sprite_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            ETModel_Entity_Binding.Register(app);
            ETModel_ResourcesComponent_Binding.Register(app);
            System_Threading_Tasks_Task_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_Binding.Register(app);
            System_Threading_Tasks_Task_1_TextAsset_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_TextAsset_Binding.Register(app);
            UnityEngine_TextAsset_Binding.Register(app);
            System_Threading_Tasks_Task_1_ILTypeInstance_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_ILTypeInstance_Binding.Register(app);
            ETModel_UIComponent_Binding.Register(app);
            ETModel_UILoadingComponent_Binding_View_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            ETModel_UILoadingComponent_Binding.Register(app);
            UnityEngine_UI_Image_Binding.Register(app);
            System_Action_Binding.Register(app);
            ETModel_GameObjectHelper_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Threading_Interlocked_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_GameObject_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            UnityEngine_Behaviour_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_ILTypeInstance_Binding.Register(app);
            System_Threading_SemaphoreSlim_Binding.Register(app);
            ETModel_CanvasConfig_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            ReferenceCollector_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            UnityEngine_SortingLayer_Binding.Register(app);
            UnityEngine_Renderer_Binding.Register(app);
            UnityEngine_Rendering_SortingGroup_Binding.Register(app);
            ZLog_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding.Register(app);
            ETModel_Define_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            System_Collections_Generic_List_1_Transform_Binding.Register(app);
            UnityEngine_UI_Graphic_Binding.Register(app);
            UnityEngine_Color_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
