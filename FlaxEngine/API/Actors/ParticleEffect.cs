// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class ParticleEffect
    {
        /// <summary>
        /// The particles simulation update modes.
        /// </summary>
        public enum SimulationUpdateMode
        {
            /// <summary>
            /// Use realtime simulation updates. Updates particles during every game logic update.
            /// </summary>
            Realtime = 0,

            /// <summary>
            /// Use fixed timestep delta time to update particles simulation with a custom frequency.
            /// </summary>
            FixedTimestep = 1,
        }

        private bool IsFixedTimestep => UpdateMode == SimulationUpdateMode.FixedTimestep;

        /// <summary>
        /// Particle effect parameter types.
        /// </summary>
        public enum ParameterType : byte
        {
            /// <summary>
            /// The bool.
            /// </summary>
            Bool = 0,

            /// <summary>
            /// The integer.
            /// </summary>
            Integer = 1,

            /// <summary>
            /// The float.
            /// </summary>
            Float = 2,

            /// <summary>
            /// The vector2.
            /// </summary>
            Vector2 = 3,

            /// <summary>
            /// The vector3.
            /// </summary>
            Vector3 = 4,

            /// <summary>
            /// The vector4.
            /// </summary>
            Vector4 = 5,

            /// <summary>
            /// The color.
            /// </summary>
            Color = 6,

            /// <summary>
            /// The texture.
            /// </summary>
            Texture = 7,

            /// <summary>
            /// The normal map.
            /// </summary>
            NormalMap = 8,

            /// <summary>
            /// The string.
            /// </summary>
            String = 9,

            /// <summary>
            /// The box.
            /// </summary>
            Box = 10,

            /// <summary>
            /// The rotation.
            /// </summary>
            Rotation = 11,

            /// <summary>
            /// The transform.
            /// </summary>
            Transform = 12,

            /// <summary>
            /// The asset.
            /// </summary>
            Asset = 13,

            /// <summary>
            /// The actor.
            /// </summary>
            Actor = 14,

            /// <summary>
            /// The rectangle.
            /// </summary>
            Rectangle = 15,

            /// <summary>
            /// The cube texture.
            /// </summary>
            CubeTexture = 16,

            /// <summary>
            /// The scene texture.
            /// </summary>
            SceneTexture = 17,

            /// <summary>
            /// The GPU texture (created from code).
            /// </summary>
            GPUTexture = 18,

            /// <summary>
            /// The matrix.
            /// </summary>
            Matrix = 19,

            /// <summary>
            /// The GPU texture array (created from code).
            /// </summary>
            GPUTextureArray = 20,

            /// <summary>
            /// The GPU volume texture (created from code).
            /// </summary>
            GPUTextureVolume = 21,

            /// <summary>
            /// The GPU cube texture (created from code).
            /// </summary>
            GPUTextureCube = 22,
        }

        /// <summary>
        /// Particle effect variable object. Allows to modify particle effect parameter at runtime.
        /// </summary>
        public sealed class Parameter
        {
            private ParticleEffect _effect;
            private int _hash;
            private int _emitterIndex;
            private int _paramIndex;
            private bool _isPublic;
            private ParameterType _type;

            /// <summary>
            /// Gets the parent effect.
            /// </summary>
            public ParticleEffect Effect => _effect;

            /// <summary>
            /// Gets the emitter asset.
            /// </summary>
            public ParticleEmitter Emitter => ParticleEffect.Internal_GetParamEmitter(_effect.unmanagedPtr, _emitterIndex);

            /// <summary>
            /// Gets the index of the emitter.
            /// </summary>
            public int EmitterIndex => _emitterIndex;

            /// <summary>
            /// Gets the index of the parameter (in the emitter parameters list).
            /// </summary>
            public int ParamIndex => _paramIndex;

            /// <summary>
            /// Gets the parameter type.
            /// </summary>
            public ParameterType Type => _type;

            /// <summary>
            /// Gets a value indicating whether this parameter is public.
            /// </summary>
            public bool IsPublic => _isPublic;

            /// <summary>
            /// Gets the parameter name.
            /// </summary>
            public string Name
            {
                // If your game is using particle parameter names a lot (lookups, searching by name, etc.) cache name in the constructor for better performance
                get
                {
                    // Validate the hash
                    if (_hash != _effect._parametersHash)
                        throw new InvalidOperationException("Cannot use invalid particle effect parameter.");

                    return ParticleEffect.Internal_GetParamName(_effect.unmanagedPtr, _emitterIndex, _paramIndex);
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
                    if (_effect._parametersHash != _hash)
                        throw new InvalidOperationException("Cannot use invalid particle parameter.");

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
                    case ParameterType.Bool:
                        ptr = new IntPtr(&vBool);
                        break;
                    case ParameterType.Integer:
                        ptr = new IntPtr(&vInt);
                        break;
                    case ParameterType.Float:
                        ptr = new IntPtr(&vFloat);
                        break;
                    case ParameterType.Vector2:
                        ptr = new IntPtr(&vVector2);
                        break;
                    case ParameterType.Vector3:
                        ptr = new IntPtr(&vVector3);
                        break;
                    case ParameterType.Vector4:
                        ptr = new IntPtr(&vVector4);
                        break;
                    case ParameterType.Color:
                        ptr = new IntPtr(&vColor);
                        break;
                    case ParameterType.Matrix:
                        ptr = new IntPtr(&vMatrix);
                        break;

                    case ParameterType.CubeTexture:
                    case ParameterType.Texture:
                    case ParameterType.NormalMap:
                    case ParameterType.GPUTexture:
                    case ParameterType.GPUTextureArray:
                    case ParameterType.GPUTextureCube:
                    case ParameterType.GPUTextureVolume:
                        ptr = new IntPtr(&vGuid);
                        break;

                    default: throw new ArgumentOutOfRangeException();
                    }

                    ParticleEffect.Internal_GetParamValue(_effect.unmanagedPtr, _emitterIndex, _paramIndex, ptr);

                    switch (_type)
                    {
                    case ParameterType.Bool: return vBool;
                    case ParameterType.Integer: return vInt;
                    case ParameterType.Float: return vFloat;
                    case ParameterType.Vector2: return vVector2;
                    case ParameterType.Vector3: return vVector3;
                    case ParameterType.Vector4: return vVector4;
                    case ParameterType.Color: return vColor;
                    case ParameterType.Matrix: return vMatrix;

                    case ParameterType.CubeTexture:
                    case ParameterType.Texture:
                    case ParameterType.NormalMap: return Object.Find<Object>(ref vGuid);
                    case ParameterType.GPUTextureArray:
                    case ParameterType.GPUTextureCube:
                    case ParameterType.GPUTextureVolume:
                    case ParameterType.GPUTexture: return Object.TryFind<Object>(ref vGuid);

                    default: throw new ArgumentOutOfRangeException();
                    }
                }
                set
                {
                    // Validate the hash
                    if (_effect._parametersHash != _hash)
                        throw new InvalidOperationException("Cannot use invalid particle parameter.");
                    if (!_isPublic)
                        throw new InvalidOperationException("Cannot set private particle parameters.");

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
                    case ParameterType.Bool:
                        vBool = Convert.ToBoolean(value);
                        ptr = new IntPtr(&vBool);
                        break;
                    case ParameterType.Integer:
                    {
                        vInt = Convert.ToInt32(value);
                        ptr = new IntPtr(&vInt);
                        break;
                    }
                    case ParameterType.Float:
                    {
                        vFloat = Convert.ToSingle(value);
                        ptr = new IntPtr(&vFloat);
                        break;
                    }
                    case ParameterType.Vector2:
                        vVector2 = (Vector2)value;
                        ptr = new IntPtr(&vVector2);
                        break;
                    case ParameterType.Vector3:
                        vVector3 = (Vector3)value;
                        ptr = new IntPtr(&vVector3);
                        break;
                    case ParameterType.Vector4:
                        vVector4 = (Vector4)value;
                        ptr = new IntPtr(&vVector4);
                        break;
                    case ParameterType.Color:
                        vColor = (Color)value;
                        ptr = new IntPtr(&vColor);
                        break;
                    case ParameterType.Matrix:
                        vMatrix = (Matrix)value;
                        ptr = new IntPtr(&vMatrix);
                        break;

                    case ParameterType.CubeTexture:
                    case ParameterType.Texture:
                    case ParameterType.NormalMap:
                    case ParameterType.GPUTexture:
                    case ParameterType.GPUTextureArray:
                    case ParameterType.GPUTextureCube:
                    case ParameterType.GPUTextureVolume:
                        ptr = Object.GetUnmanagedPtr(value as Object);
                        break;

                    default: throw new ArgumentOutOfRangeException();
                    }

                    ParticleEffect.Internal_SetParamValue(_effect.unmanagedPtr, _emitterIndex, _paramIndex, ptr);
                }
            }

            internal Parameter(int hash, ParticleEffect effect, int emitterIndex, int paramIndex, ParameterType type, bool isPublic)
            {
                _hash = hash;
                _effect = effect;
                _emitterIndex = emitterIndex;
                _paramIndex = paramIndex;
                _type = type;
                _isPublic = isPublic;
            }
        }

        /// <summary>
        /// Helper value used to keep parameters collection in sync with actual backend data.
        /// </summary>
        internal int _parametersHash;

        private Parameter[] _parameters;

        /// <summary>
        /// Gets the effect parameters collection. Those parameters are instanced from the <see cref="ParticleSystem"/> that contains a linear list of emitters and every emitter has a list of parameters.
        /// </summary>
        public Parameter[] Parameters
        {
            get
            {
                // Check if has cached value or has missing system
                if (_parameters != null || !ParticleSystem || ParticleSystem.WaitForLoaded())
                    return _parameters;

                // Get next hash #hashtag
                _parametersHash++;

                // Get parameters metadata from the backend
                var parametersCount = Internal_GetParamsCount(unmanagedPtr, -1);
                if (parametersCount > 0)
                {
                    _parameters = new Parameter[parametersCount];
                    int parametersCounter = 0;
                    int emitterIndex = 0;
                    while (parametersCounter < parametersCount)
                    {
                        var paramsCount = Internal_GetParamsCount(unmanagedPtr, emitterIndex);
                        for (int paramIndex = 0; paramIndex < paramsCount; paramIndex++)
                        {
                            Internal_GetParam(unmanagedPtr, emitterIndex, paramIndex, out var type, out var isPublic);
                            _parameters[parametersCounter++] = new Parameter(_parametersHash, this, emitterIndex, paramIndex, type, isPublic);
                        }

                        emitterIndex++;
                    }
                }
                else
                {
                    // No parameters at all
                    _parameters = Utils.GetEmptyArray<Parameter>();
                }

                return _parameters;
            }
        }

        /// <summary>
        /// Gets the particle emitter parameter.
        /// </summary>
        /// <param name="emitterName">The name of the emitter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The found parameter or null if missing.</returns>
        public Parameter GetParam(string emitterName, string paramName)
        {
            var parameters = Parameters;
            Internal_GetParamIndexByName(unmanagedPtr, emitterName, paramName, out var index);
            return index == -1 ? null : parameters[index];
        }

        /// <summary>
        /// Occurs when particle effect parameters collection gets changed.
        /// It's called on <see cref="ParticleSystem"/> asset changed or when one of the emitters gets reloaded (eg. after edit in editor).
        /// </summary>
        public event Action<ParticleEffect> ParametersChanged;

        internal void Internal_ClearParams()
        {
            _parametersHash++;
            _parameters = null;
            ParametersChanged?.Invoke(this);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParamsCount(IntPtr obj, int emitterIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParam(IntPtr obj, int emitterIndex, int paramIndex, out ParameterType type, out bool isPublic);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ParticleEmitter Internal_GetParamEmitter(IntPtr obj, int emitterIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamName(IntPtr obj, int emitterIndex, int paramIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParamIndexByName(IntPtr obj, string emitterName, string paramName, out int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParamValue(IntPtr obj, int emitterIndex, int paramIndex, IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParamValue(IntPtr obj, int emitterIndex, int paramIndex, IntPtr ptr);

#endif

        #endregion
    }
}
