// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Animation graph parameters types.
    /// </summary>
    public enum AnimationGraphParameterType : byte
    {
        /// <summary>
        /// The boolean value.
        /// </summary>
        Bool = 0,

        /// <summary>
        /// The integer value.
        /// </summary>
        Integer = 1,

        /// <summary>
        /// The floating point value (single precision).
        /// </summary>
        Float = 2,

        /// <summary>
        /// The Vector2 structure.
        /// </summary>
        Vector2 = 3,

        /// <summary>
        /// The Vector3 structure.
        /// </summary>
        Vector3 = 4,

        /// <summary>
        /// The Vector4 structure.
        /// </summary>
        Vector4 = 5,

        /// <summary>
        /// The Color structure (RGBA, normalized, 32bit per channel).
        /// </summary>
        Color = 6,

        /// <summary>
        /// The quaternion structure.
        /// </summary>
        Rotation = 11,

        /// <summary>
        /// The transform structure (translation, rotation and scale).
        /// </summary>
        Transform = 12,

        /// <summary>
        /// The asset reference.
        /// </summary>
        Asset = 13,
    }

    /// <summary>
    /// Animation graph variable object. Allows to modify graph parameter at runtime.
    /// </summary>
    public sealed class AnimationGraphParameter
    {
        private int _hash;
        private AnimatedModel _owner;
        private int _index;
        private AnimationGraphParameterType _type;
        private bool _isPublic;

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public AnimationGraphParameterType Type => _type;

        /// <summary>
        /// Gets a value indicating whether this parameter is public.
        /// </summary>
        public bool IsPublic => _isPublic;

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name
        {
            // If your game is using graph parameter names a lot (lookups, searching by name, etc.) cache name in the constructor fo better performance
            get
            {
                // Validate the hash
                if (_hash != _owner._parametersHash)
                    throw new InvalidOperationException("Cannot use invalid animation graph parameter.");

                return AnimatedModel.Internal_GetParamName(_owner.unmanagedPtr, _index);
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
                if (_owner._parametersHash != _hash)
                    throw new InvalidOperationException("Cannot use invalid animation graph parameter.");

                IntPtr ptr;
                bool vBool = false;
                int vInt = 0;
                float vFloat = 0;
                Vector2 vVector2 = new Vector2();
                Vector3 vVector3 = new Vector3();
                Vector4 vVector4 = new Vector4();
                Color vColor = new Color();
                Quaternion vRotation = new Quaternion();
                Transform vTransform = new Transform();
                Guid vGuid = new Guid();

                switch (_type)
                {
                case AnimationGraphParameterType.Bool:
                    ptr = new IntPtr(&vBool);
                    break;
                case AnimationGraphParameterType.Integer:
                    ptr = new IntPtr(&vInt);
                    break;
                case AnimationGraphParameterType.Float:
                    ptr = new IntPtr(&vFloat);
                    break;
                case AnimationGraphParameterType.Vector2:
                    ptr = new IntPtr(&vVector2);
                    break;
                case AnimationGraphParameterType.Vector3:
                    ptr = new IntPtr(&vVector3);
                    break;
                case AnimationGraphParameterType.Vector4:
                    ptr = new IntPtr(&vVector4);
                    break;
                case AnimationGraphParameterType.Color:
                    ptr = new IntPtr(&vColor);
                    break;
                case AnimationGraphParameterType.Rotation:
                    ptr = new IntPtr(&vRotation);
                    break;
                case AnimationGraphParameterType.Transform:
                    ptr = new IntPtr(&vTransform);
                    break;

                case AnimationGraphParameterType.Asset:
                    ptr = new IntPtr(&vGuid);
                    break;

                default: throw new ArgumentOutOfRangeException();
                }

                AnimatedModel.Internal_GetParamValue(_owner.unmanagedPtr, _index, ptr);

                switch (_type)
                {
                case AnimationGraphParameterType.Bool: return vBool;
                case AnimationGraphParameterType.Integer: return vInt;
                case AnimationGraphParameterType.Float: return vFloat;
                case AnimationGraphParameterType.Vector2: return vVector2;
                case AnimationGraphParameterType.Vector3: return vVector3;
                case AnimationGraphParameterType.Vector4: return vVector4;
                case AnimationGraphParameterType.Color: return vColor;
                case AnimationGraphParameterType.Rotation: return vRotation;
                case AnimationGraphParameterType.Transform: return vTransform;

                case AnimationGraphParameterType.Asset: return Content.LoadAsync(vGuid);

                default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                // Validate the hash
                if (_owner._parametersHash != _hash)
                    throw new InvalidOperationException("Cannot use invalid animation graph parameter.");

                IntPtr ptr;
                bool vBool;
                int vInt;
                float vFloat;
                Vector2 vVector2;
                Vector3 vVector3;
                Vector4 vVector4;
                Color vColor;
                Quaternion vRotation;
                Transform vTransform;

                switch (_type)
                {
                case AnimationGraphParameterType.Bool:
                    vBool = (bool)value;
                    ptr = new IntPtr(&vBool);
                    break;
                case AnimationGraphParameterType.Integer:
                {
                    if (value is int)
                        vInt = (int)value;
                    else if (value is float)
                        vInt = (int)(float)value;
                    else
                        throw new InvalidCastException();
                    ptr = new IntPtr(&vInt);
                    break;
                }
                case AnimationGraphParameterType.Float:
                {
                    if (value is int)
                        vFloat = (int)value;
                    else if (value is float)
                        vFloat = (float)value;
                    else
                        throw new InvalidCastException();
                    ptr = new IntPtr(&vFloat);
                    break;
                }
                case AnimationGraphParameterType.Vector2:
                    vVector2 = (Vector2)value;
                    ptr = new IntPtr(&vVector2);
                    break;
                case AnimationGraphParameterType.Vector3:
                    vVector3 = (Vector3)value;
                    ptr = new IntPtr(&vVector3);
                    break;
                case AnimationGraphParameterType.Vector4:
                    vVector4 = (Vector4)value;
                    ptr = new IntPtr(&vVector4);
                    break;
                case AnimationGraphParameterType.Color:
                    vColor = (Color)value;
                    ptr = new IntPtr(&vColor);
                    break;
                case AnimationGraphParameterType.Rotation:
                    vRotation = (Quaternion)value;
                    ptr = new IntPtr(&vRotation);
                    break;
                case AnimationGraphParameterType.Transform:
                    vTransform = (Transform)value;
                    ptr = new IntPtr(&vTransform);
                    break;

                case AnimationGraphParameterType.Asset:
                    ptr = Object.GetUnmanagedPtr(value as Object);
                    break;

                default: throw new ArgumentOutOfRangeException();
                }

                AnimatedModel.Internal_SetParamValue(_owner.unmanagedPtr, _index, ptr);
            }
        }

        internal AnimationGraphParameter(int hash, AnimatedModel owner, int index, AnimationGraphParameterType type, bool isPublic)
        {
            _hash = hash;
            _owner = owner;
            _index = index;
            _type = type;
            _isPublic = isPublic;
        }
    }
}
