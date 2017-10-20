////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.Json;

namespace FlaxEngine
{
	public partial class JsonAsset
	{
        /// <summary>
        /// The texture asset type unique ID.
        /// </summary>
        public const int TypeID = 15;
        
        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Document;

        /// <summary>
        /// Creates the serialized object instance from the json asset data. Asset must be loaded.
        /// </summary>
        /// <returns>The created object or null.</returns>
        public object CreateInstance()
        {
            if(!IsLoaded)
                throw new InvalidOperationException("Cannot use unloaded asset.");

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = new[]
            {
                Utils.GetAssemblyByName("Assembly", allAssemblies),
                Utils.GetAssemblyByName("Assembly.Editor", allAssemblies),
                Utils.GetAssemblyByName("FlaxEditor", allAssemblies),
                Utils.GetAssemblyByName("FlaxEngine", allAssemblies),
            };

            var dataTypeName = DataTypeName;
            
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                if (assembly != null)
                {
                    var type = assembly.GetType(dataTypeName);
                    if (type != null)
                    {
                        object obj = null;
                        try
                        {
                            // Create instance
                            obj = Activator.CreateInstance(type);

                            // Deserialzie object
                            var data = Data;
                            JsonSerializer.Deserialize(obj, data);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }

                        return obj;
                    }
                }
            }
            return null;
        }
    }
}
