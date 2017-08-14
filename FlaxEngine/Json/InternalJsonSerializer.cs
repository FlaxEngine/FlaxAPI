////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.Json.JsonCustomSerializers;
using Newtonsoft.Json;

namespace FlaxEngine.Json
{
    internal class FlaxObjectConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guid id = Guid.Empty;
            if (value is Object obj)
                id = obj.ID;

            writer.WriteValue(id);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var id = Guid.Parse((string)reader.Value);
                return Object.Find<Object>(ref id);
            }

            return null;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(Object).IsAssignableFrom(objectType);
        }
    }

    internal static class InternalJsonSerializer
    {
        public static JsonSerializerSettings Settings = CreateDefaultSettings();

        public static JsonSerializerSettings CreateDefaultSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new ExtendedDefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
            };
            settings.Converters.Add(new FlaxObjectConverter());
            return settings;
        }

        internal static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }

        internal static void Deserialize(object input, string json)
        {
            JsonConvert.PopulateObject(json, input, Settings);
        }
    }
}
