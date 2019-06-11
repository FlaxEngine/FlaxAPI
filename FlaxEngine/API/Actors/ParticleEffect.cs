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

        internal enum GraphParamType
        {
            Bool = 0,
            Integer = 1,
            Float = 2,
            Vector2 = 3,
            Vector3 = 4,
            Vector4 = 5,
            Color = 6,
            Texture = 7,
            NormalMap = 8,
            String = 9,
            Box = 10,
            Rotation = 11,
            Transform = 12,
            Asset = 13,
            Actor = 14,
            Rectangle = 15,
            CubeTexture = 16,
            SceneTexture = 17,
            RenderTarget = 18,
            Matrix = 19,
            RenderTargetArray = 20,
            RenderTargetVolume = 21,
            RenderTargetCube = 22,
        }

        /// <summary>
        /// Sets the particle emitter parameter.
        /// </summary>
        /// <param name="emitterName">The name of the emitter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value.</param>
        public unsafe void SetParameter(string emitterName, string paramName, object value)
        {
            Internal_GetParameterIdx(unmanagedPtr, emitterName, paramName, out var emitterIndex, out var paramIndex, out var type);

            if (emitterIndex == -1)
                throw new ArgumentException("Unknown emitter name.", nameof(emitterName));
            if (paramIndex == -1)
                throw new ArgumentException("Unknown parameter name.", nameof(paramName));

            IntPtr ptr;
            bool vBool;
            int vInt;
            float vFloat;
            Vector2 vVector2;
            Vector3 vVector3;
            Vector4 vVector4;
            Color vColor;
            Matrix vMatrix;

            switch (type)
            {
            case GraphParamType.Bool:
                vBool = Convert.ToBoolean(value);
                ptr = new IntPtr(&vBool);
                break;
            case GraphParamType.Integer:
                vInt = Convert.ToInt32(value);
                ptr = new IntPtr(&vInt);
                break;
            case GraphParamType.Float:
                vFloat = Convert.ToSingle(value);
                ptr = new IntPtr(&vFloat);
                break;
            case GraphParamType.Vector2:
                vVector2 = (Vector2)value;
                ptr = new IntPtr(&vVector2);
                break;
            case GraphParamType.Vector3:
                vVector3 = (Vector3)value;
                ptr = new IntPtr(&vVector3);
                break;
            case GraphParamType.Vector4:
                vVector4 = (Vector4)value;
                ptr = new IntPtr(&vVector4);
                break;
            case GraphParamType.Color:
                vColor = (Color)value;
                ptr = new IntPtr(&vColor);
                break;
            case GraphParamType.Matrix:
                vMatrix = (Matrix)value;
                ptr = new IntPtr(&vMatrix);
                break;
            case GraphParamType.CubeTexture:
            case GraphParamType.Texture:
            case GraphParamType.NormalMap:
            case GraphParamType.RenderTarget:
            case GraphParamType.RenderTargetArray:
            case GraphParamType.RenderTargetCube:
            case GraphParamType.RenderTargetVolume:
                ptr = Object.GetUnmanagedPtr(value as Object);
                break;
            default: throw new ArgumentOutOfRangeException();
            }

            Internal_SetParameter(unmanagedPtr, emitterIndex, paramIndex, ptr);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParameterIdx(IntPtr obj, string emitterName, string paramName, out int emitterIndex, out int paramIndex, out GraphParamType type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParameter(IntPtr obj, int emitterIndex, int paramIndex, IntPtr ptr);
#endif

        #endregion
    }
}
