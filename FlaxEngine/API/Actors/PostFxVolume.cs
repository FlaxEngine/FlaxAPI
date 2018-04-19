// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    public sealed partial class PostFxVolume
    {
        /// <summary>
        /// Gets the post processing settings.
        /// </summary>
        [EditorDisplay("PostFx Settings"), EditorOrder(100)]
        public PostProcessSettings Settings;

        /// <summary>
        /// Updates cached Settings.data from unmanaged data
        /// </summary>
        /// <param name="ptr">The unmanaged data pointer.</param>
        internal void Internal_SetData(IntPtr ptr)
        {
            Settings.data = (PostProcessSettings.Data)Marshal.PtrToStructure(ptr, typeof(PostProcessSettings.Data));
        }

        /// <summary>
        /// Sends cached Settings.data to unmanaged data
        /// </summary>
        /// <param name="ptr">The unmanaged data pointer.</param>
        /// <param name="forceGet">True if get data by force, even if no change has been registered.</param>
        /// <returns>True if data has been modified, otherwise false.</returns>
        internal bool Internal_GetData(IntPtr ptr, bool forceGet)
        {
            if (!Settings.isDataDirty && !forceGet)
                return false;

            Settings.isDataDirty = false;
            Marshal.StructureToPtr(Settings.data, ptr, false);
            return true;
        }
    }
}
