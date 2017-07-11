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

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public unsafe object Value
        {
            // TODO: implement getter
            set
            {
                // Validate the hash
                if (_material._parametersHash != _hash)
                    throw new InvalidOperationException("Cannot use invalid material parameter.");
                if (!_isPublic)
                    throw new InvalidOperationException("Cannot set private material parameters.");

                IntPtr ptr;
                bool vBool;
                int vInt;
                float vFloat;
                Vector2 vVector2;
                Vector3 vVector3;
                Vector4 vVector4;
                Color vColor;

                switch (_type)
                {
                    case MaterialParameterType.Bool:
                        vBool = (bool)value;
                        ptr = new IntPtr(&vBool);
                        break;
                    case MaterialParameterType.Inteager:
                        vInt = (int)value;
                        ptr = new IntPtr(&vInt);
                        break;
                    case MaterialParameterType.Float:
                        vFloat = (float)value;
                        ptr = new IntPtr(&vFloat);
                        break;
                    case MaterialParameterType.Vector2:
                        vVector2 = (Vector2)value;
                        ptr = new IntPtr(&vVector2);
                        break;
                    case MaterialParameterType.Vector3:
                        vVector3 = (Vector3)value;
                        ptr = new IntPtr(&vVector3);
                        break;
                    case MaterialParameterType.Vector4:
                        vVector4 = (Vector4)value;
                        ptr = new IntPtr(&vVector4);
                        break;
                    case MaterialParameterType.Color:
                        vColor = (Color)value;
                        ptr = new IntPtr(&vColor);
                        break;

                    case MaterialParameterType.CubeTexture:
                        ptr = Object.GetUnmanagedPtr(value as CubeTexture);
                        break;

                    case MaterialParameterType.Texture:
                    case MaterialParameterType.NormalMap:
                        // TODO: add support for using render target as material input
                        ptr = Object.GetUnmanagedPtr(value as Texture);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                MaterialBase.Internal_SetParamValue(_material.unmanagedPtr, _index, ptr);
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
