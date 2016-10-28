// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Camera actor is a device through which the player views the world.
    /// </summary>
    public sealed class Camera : Actor
    {
        // TODO: getMainCamera
        // TODO: get/edit camera params
        // TODO: customAspectRatio, customViewport
        // TODO: ConvertMouseToRay

        /// <summary>
        /// Gets or sets value indicating if camera should use perspective rendering mode, otherwise it will use orthographic projection.
        /// </summary>
        public bool UsePerspective
        {
            get { return Internal_GetUsePerspective(unmanagedPtr); }
            set { Internal_SetUsePerspective(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets camera's field of view (in degrees)
        /// </summary>
        public float FieldOfView
        {
            get { return Internal_GetFOV(unmanagedPtr); }
            set { Internal_SetFOV(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets camera's near plane distance
        /// </summary>
        public float NearPlane
        {
            get { return Internal_GetNearPlane(unmanagedPtr); }
            set { Internal_SetNearPlane(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets camera's far plane distance
        /// </summary>
        public float FarPlane
        {
            get { return Internal_GetFarPlane(unmanagedPtr); }
            set { Internal_SetFarPlane(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets camera viewport
        /// </summary>
        public Viewport Viewport
        {
            get { return Internal_GetViewport(unmanagedPtr); }
        }
        
        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool Internal_GetUsePerspective(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetUsePerspective(IntPtr obj, bool value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetFOV(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetFOV(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetNearPlane(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetNearPlane(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetFarPlane(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetFarPlane(IntPtr obj, float value);

         //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Viewport Internal_GetViewport(IntPtr obj);

        #endregion
    }
}
