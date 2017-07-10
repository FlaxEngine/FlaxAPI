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
        public Rendering.MaterialInfo Info
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { Rendering.MaterialInfo resultAsRef; Internal_GetInfo(unmanagedPtr, out resultAsRef); return resultAsRef; }
#endif
        }

        private void CacheParams()
        {
            // Get next hash #hashtag
            _parametersHash++;

            // TODO: finsih _parameters caching and expose to api...
        }

        private void ClearParams()
        {
            _parametersHash = 0;
            _parameters = null;
        }

        #region Internal Calls
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInfo(IntPtr obj, out Rendering.MaterialInfo resultAsRef);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamName(IntPtr obj, int index);
#endif
        #endregion
    }
}
