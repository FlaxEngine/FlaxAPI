// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Material"/> and <see cref="MaterialInstance"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    public abstract class MaterialBase : BinaryAsset
    {
        /// <summary>
        /// Helper value used to keep material parameters collection in sync with actual backend data.
        /// </summary>
        internal int _parametersHash;

        private MaterialParameter[] _parameters;

        /// <summary>
        /// Gets the material info, structure which describes material surface.
        /// </summary>
        [UnmanagedCall]
        public MaterialInfo Info
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get
            {
                MaterialInfo resultAsRef;
                Internal_GetInfo(unmanagedPtr, out resultAsRef);
                return resultAsRef;
            }
#endif
        }

        /// <summary>
        /// Gets a value indicating whether this material is a surface shader (can be used with a normal meshes).
        /// </summary>
        public bool IsSurface => Info.Domain == MaterialDomain.Surface;

        /// <summary>
        /// Gets a value indicating whether this material is post fx (cannot be used with a normal meshes).
        /// </summary>
        public bool IsPostFx => Info.Domain == MaterialDomain.PostProcess;

        /// <summary>
        /// Gets a value indicating whether this material is decal (cannot be used with a normal meshes).
        /// </summary>
        public bool IsDecal => Info.Domain == MaterialDomain.Decal;

        /// <summary>
        /// Gets a value indicating whether this material is a GUI shader (cannot be used with a normal meshes).
        /// </summary>
        public bool IsGUI => Info.Domain == MaterialDomain.GUI;

        /// <summary>
        /// Gets a value indicating whether this material is a terrain shader (cannot be used with a normal meshes).
        /// </summary>
        public bool IsTerrain => Info.Domain == MaterialDomain.Terrain;

        /// <summary>
        /// Gets a value indicating whether this material is a particle shader (cannot be used with a normal meshes).
        /// </summary>
        public bool IsParticle => Info.Domain == MaterialDomain.Particle;

        /// <summary>
        /// Gets the material parameters collection.
        /// </summary>
        public MaterialParameter[] Parameters
        {
            get
            {
                // Check if has cached value or is not loaded
                if ((_parameters != null && Internal_GetParametersHash(unmanagedPtr) == _parametersHash) || WaitForLoaded())
                    return _parameters;

                // Get next hash #hashtag
                _parametersHash = Internal_GetParametersHash(unmanagedPtr);

                // Get parameters metadata from the backend
                var parameters = Internal_CacheParameters(unmanagedPtr);
                if (parameters != null && parameters.Length > 0)
                {
                    _parameters = new MaterialParameter[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];

                        // Packed:
                        // Bits 0-9: Type
                        // Bit 10: IsPublic
                        var type = (MaterialParameterType)(p & 0b111111);
                        var isPublic = (p & 0b1000000) != 0;

                        _parameters[i] = new MaterialParameter(_parametersHash, this, i, type, isPublic);
                    }
                }
                else
                {
                    // No parameters at all
                    _parameters = Utils.GetEmptyArray<MaterialParameter>();
                }

                return _parameters;
            }
        }

        /// <summary>
        /// Gets the parameter by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The material parameter.</returns>
        public MaterialParameter GetParam(int index)
        {
            return Parameters[index];
        }

        /// <summary>
        /// Gets the parameter by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The material parameter.</returns>
        public MaterialParameter GetParam(string name)
        {
            var parameters = Parameters;
            var index = Internal_GetParamIndexByName(unmanagedPtr, name);
            return index >= 0 && index < parameters.Length ? parameters[index] : null;
        }

        /// <summary>
        /// Creates the virtual material instance of this material which allows to override any material parameters.
        /// </summary>
        /// <returns>The created virtual material instance asset.</returns>
        public abstract MaterialInstance CreateVirtualInstance();

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParametersHash(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInfo(IntPtr obj, out MaterialInfo resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong[] Internal_CacheParameters(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamName(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParamValue(IntPtr obj, int index, IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParamValue(IntPtr obj, int index, IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParamIndexByName(IntPtr obj, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParamOverride(IntPtr obj, int index, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetParamOverride(IntPtr obj, int index);
#endif

        #endregion
    }
}
