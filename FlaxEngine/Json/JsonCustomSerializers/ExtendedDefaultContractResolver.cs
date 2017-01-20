using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlaxEngine.Json.JsonCustomSerializers
{
    internal class ExtendedDefaultContractResolver : DefaultContractResolver
    {
        private List<Type> _attributesIgnoreList = new List<Type>
            {
                typeof(UnmanagedCallAttribute),
                typeof(NonSerializedAttribute),
                typeof(HideInInspectorAttribute)
            };

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(f => base.CreateProperty(f, memberSerialization))
                .ToList();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().GetLength(0) == 0)
                .Where(IgnoreAttributes)
                .Select(p => base.CreateProperty(p, memberSerialization))
                .ToList();

            foreach (var jsonProperty in properties)
            {
                Debug.Log(jsonProperty.PropertyName + " " + jsonProperty.PropertyType);
            }

            var props = fields.Union(properties).ToList();
            props.ForEach(p =>
            {
                p.Writable = true;
                p.Readable = true;
            });
            return props;
        }

        private bool IgnoreAttributes(PropertyInfo info)
        {
            var attr = info.GetCustomAttributes();
            foreach (var type in _attributesIgnoreList)
            {
                foreach (var attribute in attr)
                {
                    if (attribute.GetType() == type)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
