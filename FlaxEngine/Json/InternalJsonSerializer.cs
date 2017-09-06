////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using FlaxEngine.Json.JsonCustomSerializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            writer.WriteValue(id.ToString("N"));
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
            // Skip serialziation as reference id for the root object serialization (eg. Script)
            var writer = InternalJsonSerializer.CurrentWriter.Value;
            if (writer != null && writer.SerializeStackSize == 0)
            {
                return false;
            }

            return typeof(Object).IsAssignableFrom(objectType);
        }
    }

    internal static class InternalJsonSerializer
    {
        public static JsonSerializerSettings Settings = CreateDefaultSettings();
        public static ThreadLocal<JsonSerializerInternalWriter> CurrentWriter = new ThreadLocal<JsonSerializerInternalWriter>();

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
            Type type = obj.GetType();

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(Settings);
            jsonSerializer.Formatting = Formatting.Indented;

            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                // Prepare writer settings
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonWriter.DateFormatHandling = jsonSerializer.DateFormatHandling;
                jsonWriter.DateTimeZoneHandling = jsonSerializer.DateTimeZoneHandling;
                jsonWriter.FloatFormatHandling = jsonSerializer.FloatFormatHandling;
                jsonWriter.StringEscapeHandling = jsonSerializer.StringEscapeHandling;
                jsonWriter.Culture = jsonSerializer.Culture;
                jsonWriter.DateFormatString = jsonSerializer.DateFormatString;

                JsonSerializerInternalWriter serializerWriter = new JsonSerializerInternalWriter(jsonSerializer);
                CurrentWriter.Value = serializerWriter;

                serializerWriter.Serialize(jsonWriter, obj, type);

                CurrentWriter.Value = null;
            }

            return sw.ToString();
        }

        internal static void Deserialize(object input, string json)
        {
            JsonConvert.PopulateObject(json, input, Settings);
        }
    }
}
