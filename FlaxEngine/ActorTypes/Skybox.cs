// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Skybox actor can render sky using custom cube texture or material
    /// </summary>
    public sealed class Skybox : Actor
    {
        // TODO: CubeTexture, CustomMaterial

        /// <summary>
        /// Gets or sets value indicating if visual element affects the world
        /// </summary>
        public bool AffectsWorld
        {
            get { return Internal_GetAffectsWorld(unmanagedPtr); }
            set { Internal_SetAffectsWorld(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets skybox color
        /// </summary>
        public Color Color
        {
            get { return Internal_GetColor(unmanagedPtr); }
            set { Internal_SetColor(unmanagedPtr, ref value); }
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

        #endregion
    }
}
