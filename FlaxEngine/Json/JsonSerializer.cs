////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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
	/// <summary>
	/// Serialize references to the FlaxEngine.Object as Guid (format N).
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	internal class FlaxObjectConverter : JsonConverter
	{
		internal struct GuidInterop
		{
			public uint A;
			public uint B;
			public uint C;
			public uint D;
		}

		internal static unsafe void ParseHex(char* str, int length, out uint result)
		{
			uint sum = 0;
			char* p = str;
			char* end = str + length;

			if (*p == '0' && *(p + 1) == 'x')
				p += 2;

			while (p < end && *p != 0)
			{
				int c = *p - '0';

				if (c < 0 || c > 9)
				{
					c = char.ToLower(*p) - 'a' + 10;
					if (c < 10 || c > 15)
					{
						result = 0;
						return;
					}
				}

				sum = 16 * sum + (uint)c;

				p++;
			}

			result = sum;
		}

		internal static void ParseHex(string str, int start, int length, out uint result)
		{
			uint sum = 0;
			int p = start;
			int end = start + length;

			if (str.Length < end)
			{
				result = 0;
				return;
			}

			if (str[p] == '0' && str[p + 1] == 'x')
				p += 2;

			while (p < end && str[p] != 0)
			{
				int c = str[p] - '0';

				if (c < 0 || c > 9)
				{
					c = char.ToLower(str[p]) - 'a' + 10;
					if (c < 10 || c > 15)
					{
						result = 0;
						return;
					}
				}

				sum = 16 * sum + (uint)c;

				p++;
			}

			result = sum;
		}

		internal static unsafe string GetStringID(Guid id)
		{
			GuidInterop* g = (GuidInterop*)&id;
			return string.Format("{0:x8}{1:x8}{2:x8}{3:x8}", g->A, g->B, g->C, g->D);
		}

		internal static unsafe void ParseID(string str, out Guid id)
		{
			GuidInterop g;

			// Broken after VS 15.5
			/*fixed (char* a = str)
			{
			    char* b = a + 8;
			    char* c = b + 8;
			    char* d = c + 8;

			    ParseHex(a, 8, out g.A);
			    ParseHex(b, 8, out g.B);
			    ParseHex(c, 8, out g.C);
			    ParseHex(d, 8, out g.D);
			}*/

			// Temporary fix (not using raw char* pointer)
			ParseHex(str, 0, 8, out g.A);
			ParseHex(str, 8, 8, out g.B);
			ParseHex(str, 16, 8, out g.C);
			ParseHex(str, 24, 8, out g.D);

			id = *(Guid*)&g;
		}

		/// <inheritdoc />
		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			Guid id = Guid.Empty;
			if (value is Object obj)
				id = obj.ID;

			writer.WriteValue(GetStringID(id));
		}

		/// <inheritdoc />
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String)
			{
				Guid id;
				ParseID((string)reader.Value, out id);
				return Object.Find<Object>(ref id);
			}

			return null;
		}

		/// <inheritdoc />
		public override bool CanConvert(Type objectType)
		{
			// Skip serialziation as reference id for the root object serialization (eg. Script)
			var writer = JsonSerializer.CurrentWriter.Value;
			if (writer != null && writer.SerializeStackSize == 0)
			{
				return false;
			}

			return typeof(Object).IsAssignableFrom(objectType);
		}
	}

	/*
	/// <summary>
	/// Serialize Guid values using `N` format
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	internal class GuidConverter : JsonConverter
	{
	    /// <inheritdoc />
	    public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
	    {
	        Guid id = (Guid)value;
	        writer.WriteValue(id.ToString("N"));
	    }

	    /// <inheritdoc />
	    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
	    {
	        if (reader.TokenType == JsonToken.String)
	        {
	            var id = Guid.Parse((string)reader.Value);
	            return id;
	        }

	        return Guid.Empty;
	    }

	    /// <inheritdoc />
	    public override bool CanConvert(Type objectType)
	    {
	        return objectType == typeof(Guid);
	    }
	}
	*/
	/// <summary>
	/// Objects serialization tool (json format).
	/// </summary>
	public static class JsonSerializer
	{
		internal static JsonSerializerSettings Settings = CreateDefaultSettings();
		internal static FlaxObjectConverter ObjectConverter;
		internal static ThreadLocal<JsonSerializerInternalWriter> CurrentWriter = new ThreadLocal<JsonSerializerInternalWriter>();

		internal static JsonSerializerSettings CreateDefaultSettings()
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new ExtendedDefaultContractResolver(),
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				TypeNameHandling = TypeNameHandling.Auto,
				NullValueHandling = NullValueHandling.Ignore,
			};
			settings.Converters.Add(ObjectConverter = new FlaxObjectConverter());
			//settings.Converters.Add(new GuidConverter());
			return settings;
		}

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>The output json string.</returns>
		public static string Serialize(object obj)
		{
			Type type = obj.GetType();

			Newtonsoft.Json.JsonSerializer jsonSerializer = Newtonsoft.Json.JsonSerializer.CreateDefault(Settings);
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

		/// <summary>
		/// Deserializes the specified object (from the input json data).
		/// </summary>
		/// <param name="input">The objeect.</param>
		/// <param name="json">The input json data.</param>
		public static void Deserialize(object input, string json)
		{
			JsonConvert.PopulateObject(json, input, Settings);
		}
	}
}
