// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Box Brush actor
    /// </summary>
    public sealed class BoxBrush : Actor
    {
        // TODO: get brush surfaces data
        // TODO: changing each surface material

        /// <summary>
        /// Gets or sets brush surfaces scale in lightmap parameter
        /// </summary>
        public float ScaleInLightmap
        {
            get { return Internal_GetScaleInLightmap(unmanagedPtr); }
            set { Internal_SetScaleInLightmap(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets brush size (unscaled)
        /// </summary>
        public Vector3 Size
        {
            get { return Internal_GetSize(unmanagedPtr); }
            set { Internal_SetSize(unmanagedPtr, ref value); }
        }
        
        /// <summary>
        /// Gets or sets brush mode
        /// </summary>
        public BrushMode Mode
        {
            get { return Internal_GetMode(unmanagedPtr); }
            set { Internal_SetMode(unmanagedPtr, value); }
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetScaleInLightmap(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetScaleInLightmap(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Vector3 Internal_GetSize(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetSize(IntPtr obj, ref Vector3 value);
       
        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern BrushMode Internal_GetMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetMode(IntPtr obj, BrushMode value);
        
        #endregion
    }
}
