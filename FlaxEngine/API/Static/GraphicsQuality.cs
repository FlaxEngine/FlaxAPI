// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.CompilerServices;

namespace FlaxEngine.Rendering
{
    public static partial class GraphicsQuality
    {
        /// <summary>
        /// Enables rendering synchronization with the refresh rate of the display device to avoid "tearing" artifacts.
        /// </summary>
        public static bool UseVSync
        {
            get { return Internal_GetValue(0) != 0; }
            set { Internal_SetValue(0, value ? 1 : 0); }
        }

        /// <summary>
        /// Anti Aliasing quality.
        /// </summary>
        public static Quality AAQuality
        {
            get { return (Quality)Internal_GetValue(6); }
            set { Internal_SetValue(6, (int)value); }
        }

        /// <summary>
        /// Screen Space Reflections quality.
        /// </summary>
        public static Quality SSRQuality
        {
            get { return (Quality)Internal_GetValue(1); }
            set { Internal_SetValue(1, (int)value); }
        }

        /// <summary>
        /// Screen Space Ambient Occlusion quality setting.
        /// </summary>
        public static Quality SSAOQuality
        {
            get { return (Quality)Internal_GetValue(2); }
            set { Internal_SetValue(2, (int)value); }
        }

        /// <summary>
        /// Volumetric Fog quality setting.
        /// </summary>
        public static Quality VolumetricFogQuality
        {
            get { return (Quality)Internal_GetValue(7); }
            set { Internal_SetValue(7, (int)value); }
        }

        /// <summary>
        /// The shadows quality.
        /// </summary>
        public static Quality ShadowsQuality
        {
            get { return (Quality)Internal_GetValue(3); }
            set { Internal_SetValue(3, (int)value); }
        }

        /// <summary>
        /// The shadow maps quality (textures resolution).
        /// </summary>
        public static Quality ShadowMapsQuality
        {
            get { return (Quality)Internal_GetValue(4); }
            set { Internal_SetValue(4, (int)value); }
        }

        /// <summary>
        /// Enables cascades splits blending for directional light shadows.
        /// </summary>
        public static bool AllowCSMBlending
        {
            get { return Internal_GetValue(5) != 0; }
            set { Internal_SetValue(5, value ? 1 : 0); }
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetValue(int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValue(int index, int value);
#endif

        #endregion
    }
}
