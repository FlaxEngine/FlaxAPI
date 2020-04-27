// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Graphics device manager that creates, manages and releases graphics device and related objects.
    /// </summary>
    [Tooltip("Graphics device manager that creates, manages and releases graphics device and related objects.")]
    public static unsafe partial class Graphics
    {
        /// <summary>
        /// Enables rendering synchronization with the refresh rate of the display device to avoid "tearing" artifacts.
        /// </summary>
        [Tooltip("Enables rendering synchronization with the refresh rate of the display device to avoid \"tearing\" artifacts.")]
        public static bool UseVSync
        {
            get { return Internal_GetUseVSync(); }
            set { Internal_SetUseVSync(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseVSync();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseVSync(bool value);

        /// <summary>
        /// Anti Aliasing quality setting.
        /// </summary>
        [Tooltip("Anti Aliasing quality setting.")]
        public static Quality AAQuality
        {
            get { return Internal_GetAAQuality(); }
            set { Internal_SetAAQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetAAQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAAQuality(Quality value);

        /// <summary>
        /// Screen Space Reflections quality setting.
        /// </summary>
        [Tooltip("Screen Space Reflections quality setting.")]
        public static Quality SSRQuality
        {
            get { return Internal_GetSSRQuality(); }
            set { Internal_SetSSRQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetSSRQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSSRQuality(Quality value);

        /// <summary>
        /// Screen Space Ambient Occlusion quality setting.
        /// </summary>
        [Tooltip("Screen Space Ambient Occlusion quality setting.")]
        public static Quality SSAOQuality
        {
            get { return Internal_GetSSAOQuality(); }
            set { Internal_SetSSAOQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetSSAOQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSSAOQuality(Quality value);

        /// <summary>
        /// Volumetric Fog quality setting.
        /// </summary>
        [Tooltip("Volumetric Fog quality setting.")]
        public static Quality VolumetricFogQuality
        {
            get { return Internal_GetVolumetricFogQuality(); }
            set { Internal_SetVolumetricFogQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetVolumetricFogQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogQuality(Quality value);

        /// <summary>
        /// The shadows quality.
        /// </summary>
        [Tooltip("The shadows quality.")]
        public static Quality ShadowsQuality
        {
            get { return Internal_GetShadowsQuality(); }
            set { Internal_SetShadowsQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetShadowsQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsQuality(Quality value);

        /// <summary>
        /// The shadow maps quality (textures resolution).
        /// </summary>
        [Tooltip("The shadow maps quality (textures resolution).")]
        public static Quality ShadowMapsQuality
        {
            get { return Internal_GetShadowMapsQuality(); }
            set { Internal_SetShadowMapsQuality(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quality Internal_GetShadowMapsQuality();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowMapsQuality(Quality value);

        /// <summary>
        /// Enables cascades splits blending for directional light shadows.
        /// </summary>
        [Tooltip("Enables cascades splits blending for directional light shadows.")]
        public static bool AllowCSMBlending
        {
            get { return Internal_GetAllowCSMBlending(); }
            set { Internal_SetAllowCSMBlending(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAllowCSMBlending();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAllowCSMBlending(bool value);
    }
}
