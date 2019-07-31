using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ILSerial
{ 
    static Dictionary<Type, FieldInfo[]> dic = new Dictionary<Type, FieldInfo[]>();
    static Dictionary<Type, int> dicTypeCode = new Dictionary<Type, int>();
    static Type TypeDicGeneric = typeof(Dictionary<,>);
    public BinaryWriter writer;
    public BinaryReader reader;
    public ILSerial()
    {
        
    }

    public unsafe static void RegisterILRuntimeCLRRedirection(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        foreach (var i in typeof(ILSerial).GetMethods())
        {
            if (i.Name == "Read" && i.IsGenericMethodDefinition)
            { 
                appdomain.RegisterCLRMethodRedirection(i, BinaryToObject);
            }
        }
    }
    public unsafe static StackObject* BinaryToObject(ILIntepreter intp, StackObject* esp, IList<object> mStack, CLRMethod method, bool isNewObj)
    { 
        var type = method.GenericArguments[0].ReflectionType;
        StackObject* ret = ILIntepreter.Minus(esp, 1);
        object message = StackObject.ToObject(ret, intp.AppDomain, mStack);
        var result_of_this_method =((ILSerial)message).Read(type); 
        return ILIntepreter.PushObject(esp, mStack, result_of_this_method);
    }
    public void Write(string filePath,object obj)
    {
        FileStream fs = new FileStream(filePath, FileMode.Create);
        writer = new BinaryWriter(fs);
        WriteValue(obj);
        fs.Close();
    }

    public object Read(Type type)
    {
        return ReadByType(type);
    }
    public T Read<T>(string filePath,out FileStream fs)
    {
        fs = new FileStream(filePath, FileMode.Open);
        reader = new BinaryReader(fs);
        if (reader.BaseStream.Length == 0)
            return default(T); 
        return (T)ReadTypeValue(typeof(T)); 
    }

    public T ReadFileBytes<T>(byte[] bytes, out MemoryStream ms)
    {
        ms = new MemoryStream(bytes);
        ms.Position = 0;
        reader = new BinaryReader(ms);
        if (reader.BaseStream.Length == 0)
            return default(T);
        return (T)ReadTypeValue(typeof(T));
    }
    object ReadByType(Type type)
    {
        if (reader.BaseStream.Length == 0)
            return null;  
        return ReadTypeValue(type);
    }

    object ReadTypeValue(Type type,object obj=null)
    {   
        switch (GetTypeCode(type))
        {
            case 3:// TypeCode.Boolean:
                return reader.ReadBoolean();
            case 18:// TypeCode.String:
                return reader.ReadString();
            case 9:// TypeCode.Int32:
                return reader.ReadInt32();
            case 13:// TypeCode.Single:
                return reader.ReadSingle();
            case 14:// TypeCode.Double:
                return reader.ReadDouble();
            case 7:// TypeCode.Int16:
                return reader.ReadInt16();
            case 10: //TypeCode.UInt32:
                return reader.ReadUInt32();
            case 8:// TypeCode.UInt16:
                return reader.ReadUInt16();
            case 11:// TypeCode.Int64:
                return reader.ReadInt64();
            case 12:// TypeCode.UInt64:
                return reader.ReadUInt64();
            case 19://enum
                return reader.ReadInt32();
            case 20:
                return ReadArray(type);
            case 21:
                return ReadDic(type);
            case 22:
                return ReadList(type);
            default:
                return ReadClass(type, obj);
        }  
    }
    IList ReadList(Type listType)
    {
        if (reader.ReadBoolean())
        {
            return null;
        }
        int len = reader.ReadInt32();

        var list = (IList)Activator.CreateInstance(listType);
        Type elementType;
        if (listType is ILRuntime.Reflection.ILRuntimeWrapperType)
        {
            var wt = (ILRuntime.Reflection.ILRuntimeWrapperType)listType;
            elementType = wt.CLRType.GenericArguments[0].Value.ReflectionType;
        }
        else
        {
            elementType=listType.GenericTypeArguments[0];
        }
         
        for (int i = 0; i < len; i++)
        {
            list.Add(ReadTypeValue(elementType));
        }
        return list;
    }
    object ReadClass(Type type, object obj = null)
    {
        if (reader.ReadBoolean())
            return null;
        if (obj == null)
        {
            if (type is ILRuntimeType)
                obj = ((ILRuntimeType)type).ILType.Instantiate();
            else
            {
                if (type is ILRuntime.Reflection.ILRuntimeWrapperType)
                    type = ((ILRuntime.Reflection.ILRuntimeWrapperType)type).RealType;
                obj = Activator.CreateInstance(type);
            }
                
        }

        var fields = GetFieldInfos(type);

        foreach (var item in fields)
        { 
            item.SetValue(obj, ReadTypeValue(item.FieldType));
        }

        return obj;
    }

    IDictionary ReadDic(Type dicType)
    {
        if (reader.ReadBoolean())
        {
            return null;
        }
        int len = reader.ReadInt32();

        Type[] types;
        IDictionary dict;
        if (dicType is ILRuntimeWrapperType)
        {
            var t = (ILRuntimeWrapperType)dicType;
            dict = (IDictionary)Activator.CreateInstance(t.RealType);
            var ar = t.CLRType.GenericArguments;
            types = new Type[2] { ar[0].Value.ReflectionType, ar[1].Value.ReflectionType }; 
        }
        else
        {
            types = dicType.GetGenericArguments();
            Type dictType = typeof(Dictionary<,>).MakeGenericType(types[0], types[1]);
            dict = (IDictionary)Activator.CreateInstance(dictType);
        }  

        for (int i = 0; i < len; i++)
        {
            dict[ReadTypeValue(types[0])] = ReadTypeValue(types[1]); 
        }

        return dict;
    } 

    Array ReadArray(Type arrayType)
    {
        if (reader.ReadBoolean())
        {
            return null;
        }
        int len = reader.ReadInt32();
        
        Type elementType= arrayType.GetElementType();   
         
        int typeCode = GetTypeCode(elementType);

        if(typeCode==1||typeCode==21||typeCode==20||typeCode==22)
        { 
            Array array = Array.CreateInstance(elementType, len);
            for (int i = 0; i < len; i++)
            {
                array.SetValue(ReadTypeValue(elementType), i); 
            }
            return array;
        }
        else
        {
            return SetArraySimpleValue( typeCode, len);
        }   
    }
    Array SetArraySimpleValue(int type, int len)
    {  
        switch (type)
        {
            case 3:// TypeCode.Boolean:
                bool[] bools = new bool[len];
                for (int i = 0; i < len; i++)
                {
                    bools[i] = reader.ReadBoolean(); 
                }
                return bools;
            case 18:// TypeCode.String:
                string[] strs = new string[len];
                for (int i = 0; i < len; i++)
                {
                    strs[i] = reader.ReadString(); 
                }
                return strs;
            case 19://enum
            case 9:// TypeCode.Int32:
                int[] ints =new int[len];
                for (int i = 0; i < len; i++)
                {
                    ints[i]= reader.ReadInt32(); 
                }
                return ints;
            case 13:// TypeCode.Single:
                float[] floats =new float[len];
                for (int i = 0; i < len; i++)
                {
                    floats[i] = reader.ReadSingle(); 
                }
                return floats;
            case 14:// TypeCode.Double:
                double[] doubles=new double[len];
                for (int i = 0; i < len; i++)
                {
                    doubles[i] = reader.ReadDouble(); 
                }
                return doubles;
            case 7:// TypeCode.Int16:
                Int16[] Int16s =new Int16[len];
                for (int i = 0; i < len; i++)
                {
                    Int16s[i] = reader.ReadInt16();
                } 
                return Int16s;
            case 10:// TypeCode.UInt32:
                UInt32[] UInt32s =new UInt32[len];
                for (int i = 0; i < len; i++)
                {
                    UInt32s[i] = reader.ReadUInt32(); 
                }
                return UInt32s;
            case 8:// TypeCode.UInt16:
                UInt16[] UInt16s =new UInt16[len];
                for (int i = 0; i < len; i++)
                {
                    UInt16s[i] = reader.ReadUInt16(); 
                }
                return UInt16s;
            case 11:// TypeCode.Int64:
                Int64[] Int64s =new Int64[len];
                for (int i = 0; i < len; i++)
                {
                    Int64s[i] = reader.ReadInt64(); 
                }
                return Int64s;
            case 12:// TypeCode.UInt64:
                UInt64[] UInt64s =new UInt64[len];
                for (int i = 0; i < len; i++)
                {
                    UInt64s[i] = reader.ReadUInt64(); 
                }
                return UInt64s;
            default:
                return null;
        } 
    } 

    void WriteValue(object obj)
    {
        if (obj is Array)
        {
            WriteArray((Array)obj);
        }
        else if (obj is IDictionary)
        {
            WriteDic((IDictionary)obj);
        }
        else if(obj.GetType().GetInterface("System.Collections.IList") !=null)
        {
            WriteList(obj);
        }
        else if (!WriteSimpleValue(obj))
        {
            WriteClass(obj);
        }
    }
    void WriteList(object obj)
    {
        if (obj == null)
        {
            writer.Write(true);//is null 
            return;
        }
        writer.Write(false);
        IList list = (IList)obj; 
        writer.Write(list.Count);
        foreach (object elem in list)
            WriteValue(elem);
    }
    void WriteDic(IDictionary obj)
    {
        if (obj == null)
        {
            writer.Write(true);//is null 
            return;
        }
        writer.Write(false);
        writer.Write(obj.Count);
        foreach (DictionaryEntry item in obj)
        {
            WriteValue(item.Key);
            WriteValue(item.Value);
        }
    }

    void WriteArray(Array obj)
    {
        if (obj == null)
        {
            writer.Write(true);//is null 
            return;
        }
        writer.Write(false);
        writer.Write(obj.Length);
        foreach (var item in obj)
        {
            WriteValue(item);
        }
    }

    void WriteClass(object obj)
    {
        if (obj == null)
        {
            writer.Write(true);//is null 
            return;
        }

        writer.Write(false);

        Type obj_type ; 
        if(obj is ILTypeInstance)
        {
            obj_type = ((ILTypeInstance)obj).Type.ReflectionType;
        }
        else
        {
            obj_type = obj.GetType();
        }
         
        var fields = GetFieldInfos(obj_type); 
        
        foreach (var item in fields)
        {
            WriteValue(item.GetValue(obj));
        }
    }

    bool WriteSimpleValue(object obj)
    {
        var t = GetTypeCode(obj.GetType());
        switch (t)
        {
            case 3:
                writer.Write((bool)obj);
                break;
            case 18:
                writer.Write((string)obj);
                break;
            case 9:
                writer.Write((int)obj);
                break;
            case 13:
                writer.Write((float)obj);
                break;
            case 14:
                writer.Write((double)obj);
                break;
            case 7:
                writer.Write((short)obj);
                break;
            case 10:
                writer.Write((uint)obj);
                break;
            case 8:
                writer.Write((ushort)obj);
                break;
            case 11:
                writer.Write((long)obj);
                break;
            case 12:
                writer.Write((ulong)obj);
                break;
            case 19:
                writer.Write((int)obj);
                break;
            default:
                return false;
        }
        return true;
    }

    FieldInfo[] GetFieldInfos(Type type)
    {
        FieldInfo[] fieldInfos;
        if (dic.TryGetValue(type, out fieldInfos))
        {
            return fieldInfos;
        }
        fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        dic[type] = fieldInfos;
        return fieldInfos;
    } 

    int GetTypeCode(Type type)
    {
        int code;
        if (dicTypeCode.TryGetValue(type, out code))
            return code;
        if (type.FastIsEnum())
            code = 19;
        else if (type.IsArray)
            code = 20;
        else if (type.GetInterface("System.Collections.IDictionary") != null)
            code = 21;
        else if (type.GetInterface("System.Collections.IList") != null)
            code = 22;
        else
            code = (int)Type.GetTypeCode(type); 
        dicTypeCode[type] = code;
        return code;
    }
}
