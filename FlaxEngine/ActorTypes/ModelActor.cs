// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed class ModelActor : Actor
    {
        // TODO: Model
        // TODO: ModelInstanceBuffer
        // TODO: easy materials changing wihout need to use InstanceBuffer manually

        /// <summary>
        /// Gets or sets model scale in lightmap parameter
        /// </summary>
        public float ScaleInLightmap
        {
            get { return Internal_GetScaleInLightmap(unmanagedPtr); }
            set { Internal_SetScaleInLightmap(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets model asset
        /// </summary>
        public Model Model
        {
            get { return Internal_GetModel(unmanagedPtr); }
            set { Internal_SetModel(unmanagedPtr, GetUnmanagedPtr(value)); }
        }
        
        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern float Internal_GetScaleInLightmap(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetScaleInLightmap(IntPtr obj, float value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Model Internal_GetModel(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_SetModel(IntPtr obj, IntPtr value);
        
        #endregion
    }
}
