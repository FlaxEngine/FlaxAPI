////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Material parameters types.
    /// </summary>
    public enum MaterialParameterType : byte
    {
        Invalid = 0,
        Bool,
        Inteager,
        Float,
        Vector2,
        Vector3,
        Vector4,
        Color,
        Texture,
        CubeTexture,
        NormalMap
    }

    /// <summary>
    /// Material variable object. Allows to modify material parameter at runtime.
    /// </summary>
    public sealed class MaterialParameter
    {
        private int _hash;
        private MaterialBase _material;
        private int _index;
        private MaterialParameterType _type;
        private bool _isPublic;

        /// <summary>
        /// Gets the parent material.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        public MaterialBase Material => _material;

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public MaterialParameterType Type => _type;

        /// <summary>
        /// Gets a value indicating whether this parameter is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this parameter is public; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic => _isPublic;

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            // If your game is using material parameter names a lot (lookups, searching by name, etc.) cache name in the constructor fo better performance
            get
            {
                // Validate the hash
                if(_hash != _material._parametersHash)
                    throw new InvalidOperationException("Cannot use invalid material parameter.");
                
                return MaterialBase.Internal_GetParamName(_material.unmanagedPtr, _index);
            }
        }

        internal MaterialParameter(int hash, MaterialBase material, int index, MaterialParameterType type, bool isPublic)
        {
            _hash = hash;
            _material = material;
            _index = index;
            _type = type;
            _isPublic = isPublic;
        }
    }
}
