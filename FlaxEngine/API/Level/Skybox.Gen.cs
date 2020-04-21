// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Skybox actor renders sky using custom cube texture or material.
    /// </summary>
    [Tooltip("Skybox actor renders sky using custom cube texture or material.")]
    public unsafe partial class Skybox : Actor
    {
        /// <inheritdoc />
        protected Skybox() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Skybox"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Skybox New()
        {
            return Internal_Create(typeof(Skybox)) as Skybox;
        }

        /// <summary>
        /// The cube texture to draw.
        /// </summary>
        [EditorOrder(10), DefaultValue(null), EditorDisplay("Skybox")]
        [Tooltip("The cube texture to draw.")]
        public CubeTexture CubeTexture
        {
            get { return Internal_GetCubeTexture(unmanagedPtr); }
            set { Internal_SetCubeTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CubeTexture Internal_GetCubeTexture(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCubeTexture(IntPtr obj, IntPtr value);

        /// <summary>
        /// The panoramic texture to draw.
        /// </summary>
        [EditorOrder(20), DefaultValue(null), EditorDisplay("Skybox")]
        [Tooltip("The panoramic texture to draw.")]
        public Texture PanoramicTexture
        {
            get { return Internal_GetPanoramicTexture(unmanagedPtr); }
            set { Internal_SetPanoramicTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Texture Internal_GetPanoramicTexture(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPanoramicTexture(IntPtr obj, IntPtr value);

        /// <summary>
        /// The skybox custom material used to override default (domain set to surface). Must have CubeTexture, Color, PanoramicTexture and IsPanoramic parameters like Engine/SkyboxMaterial material.
        /// </summary>
        [EditorOrder(30), DefaultValue(null), EditorDisplay("Skybox")]
        [Tooltip("The skybox custom material used to override default (domain set to surface). Must have CubeTexture, Color, PanoramicTexture and IsPanoramic parameters like Engine/SkyboxMaterial material.")]
        public MaterialBase CustomMaterial
        {
            get { return Internal_GetCustomMaterial(unmanagedPtr); }
            set { Internal_SetCustomMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetCustomMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// The skybox texture tint color.
        /// </summary>
        [EditorOrder(40), DefaultValue(typeof(Color), "1,1,1,1"), EditorDisplay("Skybox")]
        [Tooltip("The skybox texture tint color.")]
        public Color Color
        {
            get { Internal_GetColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetColor(IntPtr obj, ref Color value);

        /// <summary>
        /// The skybox texture exposure value. Can be used to make skybox brighter or dimmer.
        /// </summary>
        [EditorOrder(50), DefaultValue(0.0f), Limit(-100, 100, 0.01f), EditorDisplay("Skybox")]
        [Tooltip("The skybox texture exposure value. Can be used to make skybox brighter or dimmer.")]
        public float Exposure
        {
            get { return Internal_GetExposure(unmanagedPtr); }
            set { Internal_SetExposure(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetExposure(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetExposure(IntPtr obj, float value);
    }
}
