// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Directional Light can emmit light from direction in space
    /// </summary>
    public sealed class DirectionalLight : Actor
    {
        /// <summary>
        /// Gets or sets value indicating if visual element affects the world
        /// </summary>
        public bool AffectsWorld
        {
            get { return Internal_GetAffectsWorld(unmanagedPtr); }
            set { Internal_SetAffectsWorld(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets light color
        /// </summary>
        public Color Color
        {
            get { return Internal_GetColor(unmanagedPtr); }
            set { Internal_SetColor(unmanagedPtr, ref value); }
        }

        /// <summary>
        /// Gets or sets light brightness parameter
        /// </summary>
        public float Brightness
        {
            get { return Internal_GetBrightness(unmanagedPtr); }
            set { Internal_SetBrightness(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets light shadows casting distance from view
        /// </summary>
        public float ShadowsDistance
        {
            get { return Internal_GetShadowsDistance(unmanagedPtr); }
            set { Internal_SetShadowsDistance(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets light shadows fade off distance
        /// </summary>
        public float ShadowsFadeDistance
        {
            get { return Internal_GetShadowsFadeDistance(unmanagedPtr); }
            set { Internal_SetShadowsFadeDistance(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets value indicating if how visual element casts shadows
        /// </summary>
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets light source minimum roughness parameter
        /// </summary>
        public float MinimumRoughness
        {
            get { return Internal_GetMinimumRoughness(unmanagedPtr); }
            set { Internal_SetMinimumRoughness(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets light direction vector
        /// </summary>
        public Vector3 Direction
        {
            get { return Vector3.ForwardLH * Orientation; }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetAffectsWorld(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetAffectsWorld(IntPtr obj, bool value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Color Internal_GetColor(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetColor(IntPtr obj, ref Color value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetBrightness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetBrightness(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetShadowsDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetShadowsDistance(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetShadowsFadeDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetShadowsFadeDistance(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetMinimumRoughness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetMinimumRoughness(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);

        #endregion
    }
}
