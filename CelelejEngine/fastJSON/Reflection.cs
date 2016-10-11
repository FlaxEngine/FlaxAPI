// Celelej Game Engine scripting API

// -----------------------------------------------------------------------------
// Original code from fastJSON project. https://github.com/mgholam/fastJSON
// Greetings to Mehdi Gholam
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace fastJSON
{
    internal sealed class Reflection
    {
        // Sinlgeton pattern 4 from : http://csharpindepth.com/articles/general/singleton.aspx
        private SafeDictionary<Type, Type> _genericTypeDef = new SafeDictionary<Type, Type>();
        private SafeDictionary<Type, Type[]> _genericTypes = new SafeDictionary<Type, Type[]>();
        private SafeDictionary<Type, FieldInfo[]> _fieldsCache = new SafeDictionary<Type, FieldInfo[]>();

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
            Type tt;
            if (_genericTypeDef.TryGetValue(t, out tt))
                return tt;
            tt = t.GetGenericTypeDefinition();
            _genericTypeDef.Add(t, tt);
            return tt;
        }

        public Type[] GetGenericArguments(Type t)
        {
            Type[] tt;
            if (_genericTypes.TryGetValue(t, out tt))
                return tt;
            tt = t.GetGenericArguments();
            _genericTypes.Add(t, tt);
            return tt;
        }

        internal void ClearReflectionCache()
        {
            _fieldsCache.Clear();
            _genericTypes.Clear();
            _genericTypeDef.Clear();
        }

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
            }
        }

        internal bool IsTypeRegistered(Type t)
        {
            if (_customSerializer.Count == 0)
                return false;
            Serialize s;
            return _customSerializer.TryGetValue(t, out s);
        }

        private void findFields(List<FieldInfo> validFields, Type type)
        {
            // Check base clases
            //if (type.BaseType != null)
            //    findFields(validFields, type.BaseType);

            // Get fields to serialize
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                FieldInfo field = fields[i];

                // Skip not serialized fields
                if(field.IsNotSerialized)
                    continue;

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
        }

        internal FieldInfo[] GetFields(Type type)
        {
            // Check if type has been laready cached
            FieldInfo[] result;
            if (_fieldsCache.TryGetValue(type, out result))
                return result;

            // Get fields to serialize
            var validFields = new List<FieldInfo>();
            findFields(validFields, type);

            // Cache fields
            result = validFields.ToArray();
            _fieldsCache.Add(type, result);

            return result;
        }

        internal FieldInfo FindField(Type type, string name)
        {
            // Get type fields
            var fields = GetFields(type);
            int length = fields.Length;

            // Iterate
            for (int i = 0; i < length; i++)
            {
                if (fields[i].Name == name)
                    return fields[i];
            }

            // Nothing found
            return null;
        }
    }
}
