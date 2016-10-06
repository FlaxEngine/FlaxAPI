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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace fastJSON
{
    public delegate string Serialize(object data);

    public delegate object Deserialize(string data);

    public sealed class JSONParameters
    {
        /// <summary>
        /// Serialize DateTime milliseconds i.e. yyyy-MM-dd HH:mm:ss.nnn (default = false)
        /// </summary>
        public bool DateTimeMilliseconds = false;

        /// <summary>
        /// Output string key dictionaries as "k"/"v" format (default = False)
        /// </summary>
        public bool KVStyleStringDictionary = false;

        /// <summary>
        /// If you have parametric and no default constructor for you classes (default = False)
        /// IMPORTANT NOTE : If True then all initial values within the class will be ignored and will be not set
        /// </summary>
        public bool ParametricConstructorOverride = false;

        /// <summary>
        /// Serialize null values to the output (default = True)
        /// </summary>
        public bool SerializeNullValues = true;

        /// <summary>
        /// Maximum depth for circular references in inline mode (default = 12)
        /// </summary>
        public byte SerializerMaxDepth = 12;

        /// <summary>
        /// Use escaped unicode i.e. \uXXXX format for non ASCII characters (default = True)
        /// </summary>
        public bool UseEscapedUnicode = true;

        /// <summary>
        /// Use the fast GUID format (default = True)
        /// </summary>
        public bool UseFastGuid = true;

        /// <summary>
        /// Use the optimized fast Dataset Schema format (default = True)
        /// </summary>
        public bool UseOptimizedDatasetSchema = true;

        /// <summary>
        /// Use the UTC date format (default = True)
        /// </summary>
        public bool UseUTCDateTime = true;

        /// <summary>
        /// Output Enum values instead of names (default = False)
        /// </summary>
        public bool UseValuesOfEnums = false;
    }

    /// <summary>
    /// fastJSON serialization library
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// Globally set-able parameters for controlling the serializer
        /// </summary>
        public static JSONParameters Parameters = new JSONParameters();

        /// <summary>
        /// Create a formatted json string (beautified) from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToNiceJSON(object obj)
        {
            string s = ToJSON(obj, Parameters);// use default params

            return Beautify(s);
        }

        /// <summary>
        /// Create a formatted json string (beautified) from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToNiceJSON(object obj, JSONParameters param)
        {
            string s = ToJSON(obj, param);

            return Beautify(s);
        }

        /// <summary>
        /// Create a json representation for an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj)
        {
            return ToJSON(obj, Parameters);
        }

        /// <summary>
        /// Create a json representation for an object with parameter override on this call
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToJSON(object obj, JSONParameters param)
        {
            if (obj == null)
                return "null";

            if (obj.GetType().IsGenericType)
                Reflection.Instance.GetGenericTypeDefinition(obj.GetType());

            return new JSONSerializer(param).ConvertToJSON(obj);
        }

        /// <summary>
        /// Parse a json string and generate a Dictionary&lt;string,object&gt; or List&lt;object&gt; structure
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Parse(string json)
        {
            return new JsonParser(json).Decode();
        }

        /// <summary>
        /// Create a .net4 dynamic object from the json string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(string json)
        {
            return new DynamicJson(json);
        }

        /// <summary>
        /// Create a typed generic object from the json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject <T>(string json)
        {
            return new Deserializer(Parameters).ToObject<T>(json);
        }

        /// <summary>
        /// Create a typed generic object from the json with parameter override on this call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ToObject <T>(string json, JSONParameters param)
        {
            return new Deserializer(param).ToObject<T>(json);
        }

        /// <summary>
        /// Create an object from the json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(string json)
        {
            return new Deserializer(Parameters).ToObject(json, null);
        }

        /// <summary>
        /// Create an object from the json with parameter override on this call
        /// </summary>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ToObject(string json, JSONParameters param)
        {
            return new Deserializer(param).ToObject(json, null);
        }

        /// <summary>
        /// Create an object of type from the json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(string json, Type type)
        {
            return new Deserializer(Parameters).ToObject(json, type);
        }

        /// <summary>
        /// Fill a given object with the json represenation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object FillObject(object input, string json)
        {
            var ht = new JsonParser(json).Decode() as Dictionary<string, object>;
            if (ht == null)
                return null;
            return new Deserializer(Parameters).ParseDictionary(ht, input.GetType(), input);
        }

        /// <summary>
        /// Deep copy an object i.e. clone to a new object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeepCopy(object obj)
        {
            return new Deserializer(Parameters).ToObject(ToJSON(obj));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy <T>(T obj)
        {
            return new Deserializer(Parameters).ToObject<T>(ToJSON(obj));
        }

        /// <summary>
        /// Create a human readable string from the json
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Beautify(string input)
        {
            return Formatter.PrettyPrint(input);
        }

        /// <summary>
        /// Register custom type handlers for your own types not natively handled by fastJSON
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public static void RegisterCustomType(Type type, Serialize serializer, Deserialize deserializer)
        {
            Reflection.Instance.RegisterCustomType(type, serializer, deserializer);
        }

        /// <summary>
        /// Clear the internal reflection cache so you can start from new (you will loose performance)
        /// </summary>
        public static void ClearReflectionCache()
        {
            Reflection.Instance.ClearReflectionCache();
        }

        internal static long CreateLong(string s, int index, int count)
        {
            long num = 0;
            bool neg = false;

            for (int x = 0; x < count; x++, index++)
            {
                char cc = s[index];

                switch (cc)
                {
                    case '-':
                        neg = true;
                        break;
                    case '+':
                        neg = false;
                        break;
                    default:
                        num *= 10;
                        num += cc - '0';
                        break;
                }
            }

            return neg ? -num : num;
        }
    }

    internal class Deserializer
    {
        private JSONParameters _params;

        public Deserializer(JSONParameters param)
        {
            _params = param;
        }

        public T ToObject <T>(string json)
        {
            Type t = typeof(T);
            object o = ToObject(json, t);

            if (t.IsArray)
            {
                if ((o as ICollection).Count == 0)// edge case for "[]" -> T[]
                {
                    Type tt = t.GetElementType();
                    object oo = Array.CreateInstance(tt, 0);
                    return (T)oo;
                }

                return (T)o;
            }

            return (T)o;
        }

        public object ToObject(string json)
        {
            return ToObject(json, null);
        }

        public object ToObject(string json, Type type)
        {
            Type t = null;
            if ((type != null) && type.IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(type);

            object o = new JsonParser(json).Decode();
            if (o == null)
                return null;
            if ((type != null) && (type == typeof(DataSet)))
                return CreateDataset(o as Dictionary<string, object>);
            if ((type != null) && (type == typeof(DataTable)))
                return CreateDataTable(o as Dictionary<string, object>);

            if (o is IDictionary)
            {
                if ((type != null) && (t == typeof(Dictionary<,>)))// deserialize a dictionary
                    return RootDictionary(o, type);

                // deserialize an object
                return ParseDictionary(o as Dictionary<string, object>, type, null);
            }

            if (o is List<object>)
            {
                if ((type != null) && (t == typeof(Dictionary<,>)))// kv format
                    return RootDictionary(o, type);
                if ((type != null) && (t == typeof(List<>)))// deserialize to generic list
                    return RootList(o, type);
                if ((type != null) && type.IsArray)
                    return RootArray(o, type);
                if (type == typeof(Hashtable))
                    return RootHashTable((List<object>)o);
                return (o as List<object>).ToArray();
            }

            if ((type != null) && (o.GetType() != type))
                return ChangeType(o, type);

            return o;
        }

        #region [   p r i v a t e   m e t h o d s   ]

        private object RootHashTable(List<object> o)
        {
            var h = new Hashtable();

            foreach (Dictionary<string, object> values in o)
            {
                object key = values["k"];
                object val = values["v"];
                if (key is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)key, typeof(object), null);

                if (val is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)val, typeof(object), null);

                h.Add(key, val);
            }

            return h;
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType == typeof(int))
            {
                var s = value as string;
                if (s == null)
                    return (int)(long)value;
                return CreateInteger(s, 0, s.Length);
            }
            if (conversionType == typeof(long))
            {
                var s = value as string;
                if (s == null)
                    return (long)value;
                return JSON.CreateLong(s, 0, s.Length);
            }
            if (conversionType == typeof(string))
                return (string)value;

            if (conversionType.IsEnum)
                return CreateEnum(conversionType, value);

            if (conversionType == typeof(DateTime))
                return CreateDateTime((string)value);

            if (conversionType == typeof(DateTimeOffset))
                return CreateDateTimeOffset((string)value);

            if (Reflection.Instance.IsTypeRegistered(conversionType))
                return Reflection.Instance.CreateCustom((string)value, conversionType);

            // 8-30-2014 - James Brooks - Added code for nullable types.
            if (IsNullable(conversionType))
            {
                if (value == null)
                    return value;
                conversionType = UnderlyingTypeOf(conversionType);
            }

            // 8-30-2014 - James Brooks - Nullable Guid is a special case so it was moved after the "IsNullable" check.
            if (conversionType == typeof(Guid))
                return CreateGuid((string)value);

            // 2016-04-02 - Enrico Padovani - proper conversion of byte[] back from string
            if (conversionType == typeof(byte[]))
                return Convert.FromBase64String((string)value);

            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }

        private object CreateDateTimeOffset(string value)
        {
            //                   0123456789012345678 9012 9/3 0/4  1/5
            // datetime format = yyyy-MM-ddTHH:mm:ss .nnn  _   +   00:00
            int year;
            int month;
            int day;
            int hour;
            int min;
            int sec;
            var ms = 0;
            var th = 0;
            var tm = 0;

            year = CreateInteger(value, 0, 4);
            month = CreateInteger(value, 5, 2);
            day = CreateInteger(value, 8, 2);
            hour = CreateInteger(value, 11, 2);
            min = CreateInteger(value, 14, 2);
            sec = CreateInteger(value, 17, 2);

            if ((value.Length > 21) && (value[19] == '.'))
                ms = CreateInteger(value, 20, 3);
            var p = 20;
            if (ms > 0)
                p = 24;
            th = CreateInteger(value, p + 1, 2);
            tm = CreateInteger(value, p + 1 + 2 + 1, 2);

            if (value[p] == '-')
                th = -th;

            return new DateTimeOffset(year, month, day, hour, min, sec, ms, new TimeSpan(th, tm, 0));
        }

        private bool IsNullable(Type t)
        {
            if (!t.IsGenericType)
                return false;
            Type g = t.GetGenericTypeDefinition();
            return g.Equals(typeof(Nullable<>));
        }

        private Type UnderlyingTypeOf(Type t)
        {
            return t.GetGenericArguments()[0];
        }

        private object FastCreateInstance(Type objtype)
        {
            try
            {
                return Activator.CreateInstance(objtype);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assembly '{1}'", objtype.FullName, objtype.AssemblyQualifiedName), exc);
            }
        }

        private object RootList(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
            var o = (IList)FastCreateInstance(type);
            DoParseList(parse, gtypes[0], o);
            return o;
        }

        private void DoParseList(object parse, Type it, IList o)
        {
            foreach (object k in (IList)parse)
            {
                object v = k;
                if (k is Dictionary<string, object>)
                    v = ParseDictionary(k as Dictionary<string, object>, it, null);
                else
                    v = ChangeType(k, it);

                o.Add(v);
            }
        }

        private object RootArray(object parse, Type type)
        {
            Type it = type.GetElementType();
            var o = (IList)FastCreateInstance(typeof(List<>).MakeGenericType(it));
            DoParseList(parse, it, o);
            Array array = Array.CreateInstance(it, o.Count);
            o.CopyTo(array, 0);

            return array;
        }

        private object RootDictionary(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
            Type t1 = null;
            Type t2 = null;
            if (gtypes != null)
            {
                t1 = gtypes[0];
                t2 = gtypes[1];
            }
            Type arraytype = t2.GetElementType();
            if (parse is Dictionary<string, object>)
            {
                var o = (IDictionary)FastCreateInstance(type);

                foreach (KeyValuePair<string, object> kv in (Dictionary<string, object>)parse)
                {
                    object v;
                    object k = ChangeType(kv.Key, t1);

                    if (kv.Value is Dictionary<string, object>)
                        v = ParseDictionary(kv.Value as Dictionary<string, object>, t2, null);

                    else if (t2.IsArray && (t2 != typeof(byte[])))
                        v = CreateArray((List<object>)kv.Value, t2, arraytype);

                    else if (kv.Value is IList)
                        v = CreateGenericList((List<object>)kv.Value, t2, t1);

                    else
                        v = ChangeType(kv.Value, t2);

                    o.Add(k, v);
                }

                return o;
            }
            if (parse is List<object>)
                return CreateDictionary(parse as List<object>, type, gtypes);

            return null;
        }

        private Type GetChangeType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Reflection.Instance.GetGenericArguments(conversionType)[0];

            return conversionType;
        }

        internal object ParseDictionary(Dictionary<string, object> d, Type type, object input)
        {
            if (type == typeof(NameValueCollection))
                return CreateNV(d);
            if (type == typeof(StringDictionary))
                return CreateSD(d);

            if ((type == typeof(object)))
                return d;// CreateDataset(d, globaltypes);

            if (type == null)
                throw new Exception("Cannot determine type");

            object o = input;
            if (o == null)
                if (_params.ParametricConstructorOverride)
                    o = FormatterServices.GetUninitializedObject(type);
                else
                    o = FastCreateInstance(type);

            foreach (KeyValuePair<string, object> e in d)
            {
                // Cache data
                FieldInfo field = Reflection.Instance.FindField(type, e.Key);
                if(field == null)
                    continue;
                object v = e.Value;
                object oset = null;
                
                // Create value for field from parsed JSON value (it depends on field type)
                var fieldType = field.FieldType;
                if ((fieldType == typeof(float)) || (fieldType == typeof(float?)))
                {
                    // We want float type to be first since most of data in scripts is floating point

                    if (v is long)
                        oset = (float)(long)v;
                    else
                        oset = (float)(double)v;
                }
                else if ((fieldType == typeof(int)) || (fieldType == typeof(int?)))
                {
                    oset = (int)(long)v;
                }
                else if ((fieldType == typeof(long)) || (fieldType == typeof(long?)))
                {
                    oset = (long)v;
                }
                else if (fieldType == typeof(string))
                {
                    oset = (string)v;
                }
                else if ((fieldType == typeof(bool)) || (fieldType == typeof(bool?)))
                {
                    oset = (bool)v;
                }
                else if ((fieldType == typeof(DateTime)) || (fieldType == typeof(DateTime?)))
                {
                    oset = CreateDateTime((string)v);
                }
                else if (fieldType.IsEnum)
                {
                    oset = CreateEnum(fieldType, v);
                }
                else if ((fieldType == typeof(Guid)) || (fieldType == typeof(Guid?)))
                {
                    oset = CreateGuid((string)v);
                }
                else if (fieldType == typeof(StringDictionary))
                {
                    oset = CreateSD((Dictionary<string, object>)v);
                }
                else if (fieldType == typeof(NameValueCollection))
                {
                    oset = CreateNV((Dictionary<string, object>)v);
                }
                else if (fieldType.IsArray)
                {
                    if (fieldType == typeof(byte[]))
                    {
                        oset = Convert.FromBase64String((string)v);
                    }
                    else if (!fieldType.IsValueType)
                    {
                        var bt = fieldType.IsGenericType ? fieldType.GetGenericArguments()[0] : fieldType.GetElementType();
                        oset = CreateArray((List<object>)v, fieldType, bt);
                    }
                }
                else if (fieldType.Name.Contains("Dictionary"))
                {
                    var genericTypes = Reflection.Instance.GetGenericArguments(fieldType);

                    if ((genericTypes.Length > 0) && (genericTypes[0] == typeof(string)))
                    {
                        oset = CreateStringKeyDictionary((Dictionary<string, object>)v, fieldType, genericTypes);
                    }
                    else
                    {
                        oset = CreateDictionary((List<object>)v, fieldType, genericTypes);
                    }
                }
                else if (fieldType == typeof(Hashtable))
                {
                    var genericTypes = Reflection.Instance.GetGenericArguments(fieldType);

                    oset = CreateDictionary((List<object>)v, fieldType, genericTypes);
                }
                else if (fieldType == typeof(DataSet))
                {
                    oset = CreateDataset((Dictionary<string, object>)v);
                }
                else if (fieldType == typeof(DataTable))
                {
                    oset = CreateDataTable((Dictionary<string, object>)v);
                }
                else if (Reflection.Instance.IsTypeRegistered(fieldType))
                {
                    oset = Reflection.Instance.CreateCustom((string)v, fieldType);
                }
                else
                {
                    bool isStruct = fieldType.IsValueType && !fieldType.IsPrimitive && !fieldType.IsEnum && (fieldType != typeof(decimal));

                    if (fieldType.IsGenericType && (fieldType.IsValueType == false) && v is List<object>)
                        oset = CreateGenericList((List<object>)v, fieldType, fieldType.GetGenericArguments()[0]);
                    else if ((fieldType.IsClass || isStruct || fieldType.IsInterface) && v is Dictionary<string, object>)
                        oset = ParseDictionary((Dictionary<string, object>)v, fieldType, field.GetValue(o));
                    else if (v is List<object>)
                        oset = CreateArray((List<object>)v, fieldType, typeof(object));
                    else if (fieldType.IsValueType)
                        oset = ChangeType(v, GetChangeType(fieldType));
                    else
                        oset = v;
                }

                // Set property value
                field.SetValue(o, oset);
            }

            return o;
        }

        private StringDictionary CreateSD(Dictionary<string, object> d)
        {
            var nv = new StringDictionary();

            foreach (KeyValuePair<string, object> o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }

        private NameValueCollection CreateNV(Dictionary<string, object> d)
        {
            var nv = new NameValueCollection();

            foreach (KeyValuePair<string, object> o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }
        
        private int CreateInteger(string s, int index, int count)
        {
            var num = 0;
            var neg = false;
            for (var x = 0; x < count; x++, index++)
            {
                char cc = s[index];

                if (cc == '-')
                    neg = true;
                else if (cc == '+')
                    neg = false;
                else
                {
                    num *= 10;
                    num += cc - '0';
                }
            }
            if (neg)
                num = -num;

            return num;
        }

        private object CreateEnum(Type pt, object v)
        {
            // FEATURE : optimize create enum
            return Enum.Parse(pt, v.ToString());
        }

        private Guid CreateGuid(string s)
        {
            if (s.Length > 30)
                return new Guid(s);
            return new Guid(Convert.FromBase64String(s));
        }

        private DateTime CreateDateTime(string value)
        {
            var utc = false;
            //                   0123456789012345678 9012 9/3
            // datetime format = yyyy-MM-ddTHH:mm:ss .nnn  Z
            var ms = 0;

            int year = CreateInteger(value, 0, 4);
            int month = CreateInteger(value, 5, 2);
            int day = CreateInteger(value, 8, 2);
            int hour = CreateInteger(value, 11, 2);
            int min = CreateInteger(value, 14, 2);
            int sec = CreateInteger(value, 17, 2);
            if ((value.Length > 21) && (value[19] == '.'))
                ms = CreateInteger(value, 20, 3);

            if (value[value.Length - 1] == 'Z')
                utc = true;

            if ((_params.UseUTCDateTime == false) && (utc == false))
                return new DateTime(year, month, day, hour, min, sec, ms);
            return new DateTime(year, month, day, hour, min, sec, ms, DateTimeKind.Utc).ToLocalTime();
        }

        private object CreateArray(List<object> data, Type pt, Type bt)
        {
            if (bt == null)
                bt = typeof(object);

            Array col = Array.CreateInstance(bt, data.Count);
            Type arraytype = bt.GetElementType();
            // create an array of objects
            for (var i = 0; i < data.Count; i++)
            {
                object ob = data[i];
                if (ob == null)
                    continue;
                if (ob is IDictionary)
                    col.SetValue(ParseDictionary((Dictionary<string, object>)ob, bt, null), i);
                else if (ob is ICollection)
                    col.SetValue(CreateArray((List<object>)ob, bt, arraytype), i);
                else
                    col.SetValue(ChangeType(ob, bt), i);
            }

            return col;
        }

        private object CreateGenericList(List<object> data, Type pt, Type bt)
        {
            if (pt != typeof(object))
            {
                var col = (IList)FastCreateInstance(pt);
                Type it = pt.GetGenericArguments()[0];
                // create an array of objects
                foreach (object ob in data)
                    if (ob is IDictionary)
                        col.Add(ParseDictionary((Dictionary<string, object>)ob, bt, null));

                    else if (ob is List<object>)
                        if (bt.IsGenericType)
                            col.Add((List<object>)ob);//).ToArray());
                        else
                            col.Add(((List<object>)ob).ToArray());
                    else
                        col.Add(ChangeType(ob, it));
                return col;
            }
            return data;
        }

        private object CreateStringKeyDictionary(Dictionary<string, object> reader, Type pt, Type[] types)
        {
            var col = (IDictionary)FastCreateInstance(pt);
            Type arraytype;
            Type t2 = null;
            if (types != null)
                t2 = types[1];

            Type generictype = null;
            Type[] ga = t2.GetGenericArguments();
            if (ga.Length > 0)
                generictype = ga[0];
            arraytype = t2.GetElementType();

            foreach (KeyValuePair<string, object> values in reader)
            {
                string key = values.Key;
                object val;

                if (values.Value is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)values.Value, t2, null);

                else if ((types != null) && t2.IsArray)
                    if (values.Value is Array)
                        val = values.Value;
                    else
                        val = CreateArray((List<object>)values.Value, t2, arraytype);
                else if (values.Value is IList)
                    val = CreateGenericList((List<object>)values.Value, t2, generictype);

                else
                    val = ChangeType(values.Value, t2);

                col.Add(key, val);
            }

            return col;
        }

        private object CreateDictionary(List<object> reader, Type pt, Type[] types)
        {
            var col = (IDictionary)FastCreateInstance(pt);
            Type t1 = null;
            Type t2 = null;
            if (types != null)
            {
                t1 = types[0];
                t2 = types[1];
            }

            foreach (Dictionary<string, object> values in reader)
            {
                object key = values["k"];
                object val = values["v"];

                if (key is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)key, t1, null);
                else
                    key = ChangeType(key, t1);

                if (typeof(IDictionary).IsAssignableFrom(t2))
                    val = RootDictionary(val, t2);
                else if (val is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)val, t2, null);
                else
                    val = ChangeType(val, t2);

                col.Add(key, val);
            }

            return col;
        }

        private DataSet CreateDataset(Dictionary<string, object> reader)
        {
            var ds = new DataSet();
            ds.EnforceConstraints = false;
            ds.BeginInit();

            // read dataset schema here
            object schema = reader["$schema"];

            if (schema is string)
            {
                TextReader tr = new StringReader((string)schema);
                ds.ReadXmlSchema(tr);
            }
            else
            {
                var ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, typeof(DatasetSchema), null);
                ds.DataSetName = ms.Name;
                for (var i = 0; i < ms.Info.Count; i += 3)
                {
                    if (ds.Tables.Contains(ms.Info[i]) == false)
                        ds.Tables.Add(ms.Info[i]);
                    ds.Tables[ms.Info[i]].Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
                }
            }

            foreach (KeyValuePair<string, object> pair in reader)
            {
                if (pair.Key == "$schema")
                    continue;

                var rows = (List<object>)pair.Value;
                if (rows == null)
                    continue;

                DataTable dt = ds.Tables[pair.Key];
                ReadDataTable(rows, dt);
            }

            ds.EndInit();

            return ds;
        }

        private void ReadDataTable(List<object> rows, DataTable dt)
        {
            dt.BeginInit();
            dt.BeginLoadData();
            var guidcols = new List<int>();
            var datecol = new List<int>();

            foreach (DataColumn c in dt.Columns)
            {
                if ((c.DataType == typeof(Guid)) || (c.DataType == typeof(Guid?)))
                    guidcols.Add(c.Ordinal);
                if (_params.UseUTCDateTime && ((c.DataType == typeof(DateTime)) || (c.DataType == typeof(DateTime?))))
                    datecol.Add(c.Ordinal);
            }

            foreach (List<object> row in rows)
            {
                var v = new object[row.Count];
                row.CopyTo(v, 0);
                foreach (int i in guidcols)
                {
                    var s = (string)v[i];
                    if ((s != null) && (s.Length < 36))
                        v[i] = new Guid(Convert.FromBase64String(s));
                }
                if (_params.UseUTCDateTime)
                    foreach (int i in datecol)
                    {
                        var s = (string)v[i];
                        if (s != null)
                            v[i] = CreateDateTime(s);
                    }
                dt.Rows.Add(v);
            }

            dt.EndLoadData();
            dt.EndInit();
        }

        private DataTable CreateDataTable(Dictionary<string, object> reader)
        {
            var dt = new DataTable();

            // read dataset schema here
            object schema = reader["$schema"];

            if (schema is string)
            {
                TextReader tr = new StringReader((string)schema);
                dt.ReadXmlSchema(tr);
            }
            else
            {
                var ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, typeof(DatasetSchema), null);
                dt.TableName = ms.Info[0];
                for (var i = 0; i < ms.Info.Count; i += 3)
                    dt.Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
            }

            foreach (KeyValuePair<string, object> pair in reader)
            {
                if (pair.Key == "$schema")
                    continue;

                var rows = (List<object>)pair.Value;
                if (rows == null)
                    continue;

                if (!dt.TableName.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                ReadDataTable(rows, dt);
            }

            return dt;
        }

        #endregion
    }
}
