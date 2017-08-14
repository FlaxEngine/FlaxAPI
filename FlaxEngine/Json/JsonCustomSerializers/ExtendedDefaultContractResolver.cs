////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlaxEngine.Json.JsonCustomSerializers
{
    internal class ExtendedDefaultContractResolver : DefaultContractResolver
    {
        private Type[] _attributesIgnoreList =
        {
            typeof(UnmanagedCallAttribute),
            typeof(NonSerializedAttribute),
            typeof(NoSerializeAttribute),
            typeof(HideInEditorAttribute)
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
            foreach (var attribute in attr)
            {
                if (_attributesIgnoreList.Contains(attribute.GetType()))
                    return false;
            }
            return true;
        }
    }
}
