////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	public sealed partial class Model
	{
	    /// <summary>
	    /// The model asset type unique ID.
	    /// </summary>
	    public const int TypeID = 3;

	    /// <summary>
	    /// The asset type content domain.
	    /// </summary>
	    public const ContentDomain Domain = ContentDomain.Model;

	    private ModelLOD[] _lods;

	    /// <summary>
	    /// Gets the model level of details collection. Each level of detail contains array of meshes.
	    /// </summary>
	    /// <value>
	    /// The LODs array. It's null if model has not been loaded yet.
	    /// </value>
	    public ModelLOD[] LODs
	    {
	        get
	        {
	            // Check if has cached data
	            if (_lods != null)
	                return _lods;

	            // Ask unmanaged world for array with mesh count per lod
	            var lodsSizes = Internal_GetLODs(unmanagedPtr);
	            if (lodsSizes != null)
	            {
	                _lods = new ModelLOD[lodsSizes.Length];
	                for (int i = 0; i < lodsSizes.Length; i++)
	                {
	                    _lods[i] = new ModelLOD(this, i, lodsSizes[i]);
	                }
                }
                
                return _lods;
	        }
	    }

	    internal void Internal_OnUnload()
	    {
	        // Clear cached data
	        _lods = null;

            Debug.Log("clear managed LOD cache");
	    }

#if !UNIT_TEST_COMPILANT
	    [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern int[] Internal_GetLODs(IntPtr obj);
#endif
    }
}
