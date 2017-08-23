////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Material"/> and <see cref="MaterialInstance"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    public abstract class MaterialBase : BinaryAsset
    {
        internal int _parametersHash; // Helper value used to keep material paramaters collection in sync with actual backend data
        private MaterialParameter[] _parameters;

        /// <summary>
        /// Gets the material info, structure which describies material surface.
        /// </summary>
        [UnmanagedCall]
        public MaterialInfo Info
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { MaterialInfo resultAsRef; Internal_GetInfo(unmanagedPtr, out resultAsRef); return resultAsRef; }
#endif
        }

        /// <summary>
        /// Gets or sets the material parameters collection.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public MaterialParameter[] Parameters
        {
            get
            {
                // Check if has cached value
                if (_parameters != null)
                    return _parameters;

                // Get next hash #hashtag
                _parametersHash++;

                // Get parameters metadata from the backend
                var parameters = Internal_CacheParameters(unmanagedPtr);
                if (parameters != null && parameters.Length > 0)
                {
                    _parameters = new MaterialParameter[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];

                        // Packed:
                        // Bits 0-7: Type
                        // Bit 8: IsPublic
                        var type = (MaterialParameterType)(p & 0b1111);
                        var isPublic = (p & 0b10000) != 0;

                        _parameters[i] = new MaterialParameter(_parametersHash, this, i, type, isPublic);
                    }
                }
                else
                {
                    // No parameters at all
                    _parameters = new MaterialParameter[0];
                }

                return _parameters;
            }
        }

        internal void Internal_ClearParams()
        {
            _parametersHash = 0;
            _parameters = null;
        }

        #region Internal Calls
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInfo(IntPtr obj, out MaterialInfo resultAsRef);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong[] Internal_CacheParameters(IntPtr obj);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamName(IntPtr obj, int index);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_SetParamValue(IntPtr obj, int index, IntPtr ptr);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamValue(IntPtr obj, int index, IntPtr ptr);
#endif
        #endregion
    }
}
