// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Material parameters types.
    /// </summary>
    public enum MaterialParameterType : byte
    {
        /// <summary>
        /// The invalid type.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The bool.
        /// </summary>
        Bool = 1,

        /// <summary>
        /// The integer.
        /// </summary>
        Integer = 2,

        /// <summary>
        /// The float.
        /// </summary>
        Float = 3,

        /// <summary>
        /// The vector2
        /// </summary>
        Vector2 = 4,

        /// <summary>
        /// The vector3.
        /// </summary>
        Vector3 = 5,

        /// <summary>
        /// The vector4.
        /// </summary>
        Vector4 = 6,

        /// <summary>
        /// The color.
        /// </summary>
        Color = 7,

        /// <summary>
        /// The texture.
        /// </summary>
        Texture = 8,

        /// <summary>
        /// The cube texture.
        /// </summary>
        CubeTexture = 9,

        /// <summary>
        /// The normal map texture.
        /// </summary>
        NormalMap = 10,

        /// <summary>
        /// The scene texture.
        /// </summary>
        SceneTexture = 11,

        /// <summary>
        /// The render target (created from code).
        /// </summary>
        RenderTarget = 12,

        /// <summary>
        /// The matrix.
        /// </summary>
        Matrix = 13,

        /// <summary>
        /// The render target array (created from code).
        /// </summary>
        RenderTargetArray = 14,

        /// <summary>
        /// The volume render target (created from code).
        /// </summary>
        RenderTargetVolume = 15,

        /// <summary>
        /// The cube render target (created from code).
        /// </summary>
        RenderTargetCube = 16,
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
        public MaterialBase Material => _material;

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public MaterialParameterType Type => _type;

        /// <summary>
        /// Gets a value indicating whether this parameter is public.
        /// </summary>
        public bool IsPublic => _isPublic;

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name
        {
            // If your game is using material parameter names a lot (lookups, searching by name, etc.) cache name in the constructor fo better performance
            get
            {
                // Validate the hash
                if (_hash != _material._parametersHash)
                    throw new InvalidOperationException("Cannot use invalid material parameter.");

                return MaterialBase.Internal_GetParamName(_material.unmanagedPtr, _index);
            }
        }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        public unsafe object Value
        {
            get
            {
                // Validate the hash
                if (_material._parametersHash != _hash)
                    throw new InvalidOperationException("Cannot use invalid material parameter.");

                IntPtr ptr;
                bool vBool = false;
                int vInt = 0;
                float vFloat = 0;
                Vector2 vVector2 = new Vector2();
                Vector3 vVector3 = new Vector3();
                Vector4 vVector4 = new Vector4();
                Color vColor = new Color();
                Guid vGuid = new Guid();
                Matrix vMatrix = new Matrix();

                switch (_type)
                {
                case MaterialParameterType.Bool:
                    ptr = new IntPtr(&vBool);
                    break;
                case MaterialParameterType.Integer:
                    ptr = new IntPtr(&vInt);
                    break;
                case MaterialParameterType.Float:
                    ptr = new IntPtr(&vFloat);
                    break;
                case MaterialParameterType.Vector2:
                    ptr = new IntPtr(&vVector2);
                    break;
                case MaterialParameterType.Vector3:
                    ptr = new IntPtr(&vVector3);
                    break;
                case MaterialParameterType.Vector4:
                    ptr = new IntPtr(&vVector4);
                    break;
                case MaterialParameterType.Color:
                    ptr = new IntPtr(&vColor);
                    break;
                case MaterialParameterType.Matrix:
                    ptr = new IntPtr(&vMatrix);
                    break;

                case MaterialParameterType.CubeTexture:
                case MaterialParameterType.Texture:
                case MaterialParameterType.NormalMap:
                case MaterialParameterType.RenderTarget:
                case MaterialParameterType.RenderTargetArray:
                case MaterialParameterType.RenderTargetCube:
                case MaterialParameterType.RenderTargetVolume:
                    ptr = new IntPtr(&vGuid);
                    break;

                default: throw new ArgumentOutOfRangeException();
                }

                MaterialBase.Internal_GetParamValue(_material.unmanagedPtr, _index, ptr);

                switch (_type)
                {
                case MaterialParameterType.Bool: return vBool;
                case MaterialParameterType.Integer: return vInt;
                case MaterialParameterType.Float: return vFloat;
                case MaterialParameterType.Vector2: return vVector2;
                case MaterialParameterType.Vector3: return vVector3;
                case MaterialParameterType.Vector4: return vVector4;
                case MaterialParameterType.Color: return vColor;
                case MaterialParameterType.Matrix: return vMatrix;

                case MaterialParameterType.CubeTexture:
                case MaterialParameterType.Texture:
                case MaterialParameterType.NormalMap:
                case MaterialParameterType.RenderTargetArray:
                case MaterialParameterType.RenderTargetCube:
                case MaterialParameterType.RenderTargetVolume:
                case MaterialParameterType.RenderTarget: return Object.Find<Object>(ref vGuid);

                default: throw new ArgumentOutOfRangeException();
                }
            }
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
                Matrix vMatrix;

                switch (_type)
                {
                case MaterialParameterType.Bool:
                    vBool = Convert.ToBoolean(value);
                    ptr = new IntPtr(&vBool);
                    break;
                case MaterialParameterType.Integer:
                {
                    vInt = Convert.ToInt32(value);
                    ptr = new IntPtr(&vInt);
                    break;
                }
                case MaterialParameterType.Float:
                {
                    vFloat = Convert.ToSingle(value);
                    ptr = new IntPtr(&vFloat);
                    break;
                }
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
                case MaterialParameterType.Matrix:
                    vMatrix = (Matrix)value;
                    ptr = new IntPtr(&vMatrix);
                    break;

                case MaterialParameterType.CubeTexture:
                case MaterialParameterType.Texture:
                case MaterialParameterType.NormalMap:
                case MaterialParameterType.RenderTarget:
                case MaterialParameterType.RenderTargetArray:
                case MaterialParameterType.RenderTargetCube:
                case MaterialParameterType.RenderTargetVolume:
                    ptr = Object.GetUnmanagedPtr(value as Object);
                    break;

                default: throw new ArgumentOutOfRangeException();
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
