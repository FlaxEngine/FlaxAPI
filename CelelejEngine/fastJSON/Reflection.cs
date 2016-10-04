// Celelej Game Engine scripting API

// -----------------------------------------------------------------------------
// Original code from fastJSON project. https://github.com/mgholam/fastJSON
// Greetings to Mehdi Gholam
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace fastJSON
{
    internal enum myPropInfoType
    {
        Int,
        Long,
        String,
        Bool,
        DateTime,
        Enum,
        Guid,

        Array,
        ByteArray,
        Dictionary,
        StringKeyDictionary,
        NameValue,
        StringDictionary,
        Hashtable,
        DataSet,
        DataTable,
        Custom,
        Unknown
    }

    internal struct myPropInfo
    {
        public Type pt;
        public Type bt;
        public Type changeType;
        public Reflection.GenericSetter setter;
        public Reflection.GenericGetter getter;
        public Type[] GenericTypes;
        public string Name;
        public myPropInfoType Type;
        public bool CanWrite;

        public bool IsClass;
        public bool IsValueType;
        public bool IsGenericType;
        public bool IsStruct;
        public bool IsInterface;
    }

    internal sealed class Reflection
    {
        // Sinlgeton pattern 4 from : http://csharpindepth.com/articles/general/singleton.aspx
        private SafeDictionary<Type, CreateObject> _constrcache = new SafeDictionary<Type, CreateObject>();
        private SafeDictionary<Type, Type> _genericTypeDef = new SafeDictionary<Type, Type>();
        private SafeDictionary<Type, Type[]> _genericTypes = new SafeDictionary<Type, Type[]>();
        private SafeDictionary<Type, FieldInfo[]> _fieldsCache = new SafeDictionary<Type, FieldInfo[]>();
        private SafeDictionary<string, Dictionary<string, myPropInfo>> _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>();

        private SafeDictionary<Type, string> _tyname = new SafeDictionary<Type, string>();
        private SafeDictionary<string, Type> _typecache = new SafeDictionary<string, Type>();
        public static Reflection Instance { get; } = new Reflection();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Reflection()
        {
        }

        private Reflection()
        {
        }

        public Type GetGenericTypeDefinition(Type t)
        {
            Type tt = null;
            if (_genericTypeDef.TryGetValue(t, out tt))
                return tt;
            tt = t.GetGenericTypeDefinition();
            _genericTypeDef.Add(t, tt);
            return tt;
        }

        public Type[] GetGenericArguments(Type t)
        {
            Type[] tt = null;
            if (_genericTypes.TryGetValue(t, out tt))
                return tt;
            tt = t.GetGenericArguments();
            _genericTypes.Add(t, tt);
            return tt;
        }

        public Dictionary<string, myPropInfo> Getproperties(Type type, string typename)
        {
            Dictionary<string, myPropInfo> sd = null;
            if (_propertycache.TryGetValue(typename, out sd))
                return sd;
            sd = new Dictionary<string, myPropInfo>();
            BindingFlags bf = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            PropertyInfo[] pr = type.GetProperties(bf);
            foreach (PropertyInfo p in pr)
            {
                if (p.GetIndexParameters().Length > 0)// Property is an indexer
                    continue;

                myPropInfo d = CreateMyProp(p.PropertyType, p.Name);
                d.setter = CreateSetMethod(type, p);
                if (d.setter != null)
                    d.CanWrite = true;
                d.getter = CreateGetMethod(type, p);
                sd.Add(p.Name.ToLower(), d);
            }
            FieldInfo[] fi = type.GetFields(bf);
            foreach (FieldInfo f in fi)
            {
                myPropInfo d = CreateMyProp(f.FieldType, f.Name);
                if (f.IsLiteral == false)
                {
                    d.setter = CreateSetField(type, f);
                    if (d.setter != null)
                        d.CanWrite = true;
                    d.getter = CreateGetField(type, f);
                    sd.Add(f.Name.ToLower(), d);
                }
            }

            _propertycache.Add(typename, sd);
            return sd;
        }

        private myPropInfo CreateMyProp(Type t, string name)
        {
            var d = new myPropInfo();
            var d_type = myPropInfoType.Unknown;

            if ((t == typeof(int)) || (t == typeof(int?)))
                d_type = myPropInfoType.Int;
            else if ((t == typeof(long)) || (t == typeof(long?)))
                d_type = myPropInfoType.Long;
            else if (t == typeof(string))
                d_type = myPropInfoType.String;
            else if ((t == typeof(bool)) || (t == typeof(bool?)))
                d_type = myPropInfoType.Bool;
            else if ((t == typeof(DateTime)) || (t == typeof(DateTime?)))
                d_type = myPropInfoType.DateTime;
            else if (t.IsEnum)
                d_type = myPropInfoType.Enum;
            else if ((t == typeof(Guid)) || (t == typeof(Guid?)))
                d_type = myPropInfoType.Guid;
            else if (t == typeof(StringDictionary))
                d_type = myPropInfoType.StringDictionary;
            else if (t == typeof(NameValueCollection))
                d_type = myPropInfoType.NameValue;
            else if (t.IsArray)
            {
                d.bt = t.GetElementType();
                if (t == typeof(byte[]))
                    d_type = myPropInfoType.ByteArray;
                else
                    d_type = myPropInfoType.Array;
            }
            else if (t.Name.Contains("Dictionary"))
            {
                d.GenericTypes = Instance.GetGenericArguments(t);
                if ((d.GenericTypes.Length > 0) && (d.GenericTypes[0] == typeof(string)))
                    d_type = myPropInfoType.StringKeyDictionary;
                else
                    d_type = myPropInfoType.Dictionary;
            }
            else if (t == typeof(Hashtable))
                d_type = myPropInfoType.Hashtable;
            else if (t == typeof(DataSet))
                d_type = myPropInfoType.DataSet;
            else if (t == typeof(DataTable))
                d_type = myPropInfoType.DataTable;
            else if (IsTypeRegistered(t))
                d_type = myPropInfoType.Custom;

            if (t.IsValueType && !t.IsPrimitive && !t.IsEnum && (t != typeof(decimal)))
                d.IsStruct = true;

            d.IsInterface = t.IsInterface;
            d.IsClass = t.IsClass;
            d.IsValueType = t.IsValueType;
            if (t.IsGenericType)
            {
                d.IsGenericType = true;
                d.bt = t.GetGenericArguments()[0];
            }

            d.pt = t;
            d.Name = name;
            d.changeType = GetChangeType(t);
            d.Type = d_type;

            return d;
        }

        private Type GetChangeType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return Instance.GetGenericArguments(conversionType)[0];

            return conversionType;
        }

        internal void ResetPropertyCache()
        {
            _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>();
        }

        internal void ClearReflectionCache()
        {
            _tyname = new SafeDictionary<Type, string>();
            _typecache = new SafeDictionary<string, Type>();
            _constrcache = new SafeDictionary<Type, CreateObject>();
            _fieldsCache = new SafeDictionary<Type, FieldInfo[]>();
            _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>();
            _genericTypes = new SafeDictionary<Type, Type[]>();
            _genericTypeDef = new SafeDictionary<Type, Type>();
        }

        internal delegate object GenericSetter(object target, object value);

        internal delegate object GenericGetter(object obj);

        private delegate object CreateObject();

        #region bjson custom types

        internal UnicodeEncoding unicode = new UnicodeEncoding();
        internal UTF8Encoding utf8 = new UTF8Encoding();

        #endregion

        #region json custom types

        // JSON custom
        internal SafeDictionary<Type, Serialize> _customSerializer = new SafeDictionary<Type, Serialize>();
        internal SafeDictionary<Type, Deserialize> _customDeserializer = new SafeDictionary<Type, Deserialize>();

        internal object CreateCustom(string v, Type type)
        {
            Deserialize d;
            _customDeserializer.TryGetValue(type, out d);
            return d(v);
        }

        internal void RegisterCustomType(Type type, Serialize serializer, Deserialize deserializer)
        {
            if ((type != null) && (serializer != null) && (deserializer != null))
            {
                _customSerializer.Add(type, serializer);
                _customDeserializer.Add(type, deserializer);
                // reset property cache
                Instance.ResetPropertyCache();
            }
        }

        internal bool IsTypeRegistered(Type t)
        {
            if (_customSerializer.Count == 0)
                return false;
            Serialize s;
            return _customSerializer.TryGetValue(t, out s);
        }

        #endregion

        #region [   PROPERTY GET SET   ]

        internal string GetTypeAssemblyName(Type t)
        {
            var val = "";
            if (_tyname.TryGetValue(t, out val))
                return val;
            string s = t.AssemblyQualifiedName;
            _tyname.Add(t, s);
            return s;
        }

        internal Type GetTypeFromCache(string typename)
        {
            Type val;
            if (_typecache.TryGetValue(typename, out val))
                return val;

            Type t = Type.GetType(typename);
            //if (t == null) // RaptorDB : loading runtime assemblies
            //{
            //    t = Type.GetType(typename, (name) => {
            //        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name.FullName).FirstOrDefault();
            //    }, null, true);
            //}
            _typecache.Add(typename, t);
            return t;
        }

        internal object FastCreateInstance(Type objtype)
        {
            try
            {
                CreateObject c = null;
                if (_constrcache.TryGetValue(objtype, out c))
                    return c();
                if (objtype.IsClass)
                {
                    var dynMethod = new DynamicMethod("_", objtype, null);
                    ILGenerator ilGen = dynMethod.GetILGenerator();
                    ilGen.Emit(OpCodes.Newobj, objtype.GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Ret);
                    c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    _constrcache.Add(objtype, c);
                }
                else// structs
                {
                    var dynMethod = new DynamicMethod("_", typeof(object), null);
                    ILGenerator ilGen = dynMethod.GetILGenerator();
                    LocalBuilder lv = ilGen.DeclareLocal(objtype);
                    ilGen.Emit(OpCodes.Ldloca_S, lv);
                    ilGen.Emit(OpCodes.Initobj, objtype);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Box, objtype);
                    ilGen.Emit(OpCodes.Ret);
                    c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    _constrcache.Add(objtype, c);
                }
                return c();
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assembly '{1}'",
                    objtype.FullName, objtype.AssemblyQualifiedName), exc);
            }
        }

        internal static GenericSetter CreateSetField(Type type, FieldInfo fieldInfo)
        {
            var arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            var dynamicSet = new DynamicMethod("_", typeof(object), arguments, type);

            ILGenerator il = dynamicSet.GetILGenerator();

            if (!type.IsClass)// structs
            {
                LocalBuilder lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsClass)
                    il.Emit(OpCodes.Castclass, fieldInfo.FieldType);
                else
                    il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                il.Emit(OpCodes.Stfld, fieldInfo);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Box, type);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                il.Emit(OpCodes.Stfld, fieldInfo);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);
            }
            return (GenericSetter)dynamicSet.CreateDelegate(typeof(GenericSetter));
        }

        internal static GenericSetter CreateSetMethod(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
                return null;

            var arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            var setter = new DynamicMethod("_", typeof(object), arguments);
            ILGenerator il = setter.GetILGenerator();

            if (!type.IsClass)// structs
            {
                LocalBuilder lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldarg_1);
                if (propertyInfo.PropertyType.IsClass)
                    il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                else
                    il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                il.EmitCall(OpCodes.Call, setMethod, null);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Box, type);
            }
            else
            {
                if (!setMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                    il.Emit(OpCodes.Ldarg_1);
                    if (propertyInfo.PropertyType.IsClass)
                        il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                    else
                        il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    il.EmitCall(OpCodes.Callvirt, setMethod, null);
                    il.Emit(OpCodes.Ldarg_0);
                }
                else
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    if (propertyInfo.PropertyType.IsClass)
                        il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                    else
                        il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    il.Emit(OpCodes.Call, setMethod);
                }
            }

            il.Emit(OpCodes.Ret);

            return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
        }

        internal static GenericGetter CreateGetField(Type type, FieldInfo fieldInfo)
        {
            var dynamicGet = new DynamicMethod("_", typeof(object), new[] {typeof(object)}, type);

            ILGenerator il = dynamicGet.GetILGenerator();

            if (!type.IsClass)// structs
            {
                LocalBuilder lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, fieldInfo.FieldType);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, fieldInfo.FieldType);
            }

            il.Emit(OpCodes.Ret);

            return (GenericGetter)dynamicGet.CreateDelegate(typeof(GenericGetter));
        }

        internal static GenericGetter CreateGetMethod(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;

            var getter = new DynamicMethod("_", typeof(object), new[] {typeof(object)}, type);

            ILGenerator il = getter.GetILGenerator();

            if (!type.IsClass)// structs
            {
                LocalBuilder lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.EmitCall(OpCodes.Call, getMethod, null);
                if (propertyInfo.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }
            else
            {
                if (!getMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                    il.EmitCall(OpCodes.Callvirt, getMethod, null);
                }
                else
                    il.Emit(OpCodes.Call, getMethod);

                if (propertyInfo.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }

            il.Emit(OpCodes.Ret);

            return (GenericGetter)getter.CreateDelegate(typeof(GenericGetter));
        }

        internal FieldInfo[] GetFields(Type type)
        {
            // Check if type has been laready cached
            FieldInfo[] result;
            if (_fieldsCache.TryGetValue(type, out result))
                return result;

            // Get fields to serialize
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int length = fields.Length;
            var validFields = new List<FieldInfo>(length);
            for (int i = 0; i < length; i++)
            {
                FieldInfo field = fields[i];

                // Check if field is private
                if (field.IsPrivate)
                {
                    // Check if has been marked for serialization
                    bool isSerializable = field.IsDefined(typeof(CelelejEngine.SerializeField), false);
                    if (!isSerializable)
                        continue;
                }

                // Check if can serialize that object type
                var fieldType = field.FieldType;
                if (fieldType.IsAbstract || !fieldType.IsSerializable)
                    continue;

                // Register field for serializaion
                validFields.Add(field);
            }

            // Cache fields
            result = validFields.ToArray();
            _fieldsCache.Add(type, result);

            return result;
        }

        #endregion
    }
}
