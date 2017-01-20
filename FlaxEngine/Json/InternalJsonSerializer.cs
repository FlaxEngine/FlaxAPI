using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.Json.JsonCustomSerializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Attibute stops serialization of given property since it is calling unmanaged code
/// </summary>
internal class UnmanagedCallAttribute : Attribute
{
}

namespace FlaxEngine.Json
{
    internal static class InternalJsonSerializer
    {
        internal static string Serialize(object obj)
        {
            try
            {
                Debug.LogError(obj.ToString());
                var seetings = new JsonSerializerSettings() {ContractResolver = new ExtendedDefaultContractResolver()};
                return JsonConvert.SerializeObject(obj, Formatting.Indented, seetings);
            }
            catch (Exception e)
            {
                return e.Message + " " + e.StackTrace;
            }
        }

        internal static object Deserialize(object input, string json)
        {
            return input = JsonConvert.DeserializeObject(json);
        }
    }
}