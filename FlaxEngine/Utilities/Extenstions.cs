using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.Utilities
{
    public static partial class Extenstions
    {
        /// <summary>
        /// Creates deep clone for a class if all members of this class are marked as serializable
        /// </summary>
        /// <param name="instance">Current instance of an object</param>
        /// <typeparam name="T">Instance type of an object</typeparam>
        /// <returns>Returns new object of provided class</returns>
        public static T DeepClone<T>(this T instance) 
            where T : new()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, instance);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
