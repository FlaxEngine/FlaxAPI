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

        internal void Internal_SetData(IntPtr ptr)
        {
            // Updates cached Settings.data from unmanaged data
            Settings.data = (PostProcessSettings.Data)Marshal.PtrToStructure(ptr, typeof(PostProcessSettings.Data));
        }

        internal void Internal_GetData(IntPtr ptr)
        {
            // TODO: add state tracking on managed side and flush settings data only after change

            // Sends cached Settings.data to unmanaged data
            Marshal.StructureToPtr(Settings.data, ptr, false);
        }
    }
}
