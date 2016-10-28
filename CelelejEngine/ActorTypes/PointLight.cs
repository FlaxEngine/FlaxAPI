// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Point Light can emmit light from point in space
    /// </summary>
    public sealed class PointLight : Actor
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
        /// Gets or sets light radius parameter
        /// </summary>
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets light scaled radius parameter
        /// </summary>
        public float ScaledRadius
        {
            get { return Scale.MaxValue * Radius; }
        }

        /// <summary>
        /// Gets or sets light source bulb radius parameter
        /// </summary>
        public float SourceRadius
        {
            get { return Internal_GetSourceRadius(unmanagedPtr); }
            set { Internal_SetSourceRadius(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets light source bulb length parameter
        /// </summary>
        public float SourceLength
        {
            get { return Internal_GetSourceLength(unmanagedPtr); }
            set { Internal_SetSourceLength(unmanagedPtr, value); }
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
        /// Gets or sets value indicating if how visual element casts shadows
        /// </summary>
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
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
        private static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetRadius(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetSourceRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetSourceRadius(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetSourceLength(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetSourceLength(IntPtr obj, float value);

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
