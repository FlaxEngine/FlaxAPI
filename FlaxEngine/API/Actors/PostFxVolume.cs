////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        [EditorDisplay(null, "PostFx Settings"), EditorOrder(100)]
        public readonly PostProcessSettings Settings = new PostProcessSettings();

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
        /// <returns>True if data has been modified, otherwise false.</returns>
        internal bool Internal_GetData(IntPtr ptr)
        {
            if (!Settings.isDataDirty)
                return false;

            Settings.isDataDirty = false;
            Marshal.StructureToPtr(Settings.data, ptr, false);
            return true;
        }
    }
}
