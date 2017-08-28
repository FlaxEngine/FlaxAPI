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
        [EditorDisplay("PostFx Settings"), EditorOrder(100)]
        public PostProcessSettings Settings;

        /// <summary>
        /// Updates cached Settings.data from unmanaged data
        /// </summary>
        /// <param name="ptr">The unmanaged data pointer.</param>
        internal unsafe void Internal_SetData(IntPtr ptr)
        {
            Settings.data = (PostProcessSettings.Data)Marshal.PtrToStructure(ptr, typeof(PostProcessSettings.Data));

            var postFxCount = Settings.data.PostFxMaterialsCount;
            Settings.postFxMaterials = new MaterialBase[postFxCount];
            fixed (Guid* postFxMaterials = &Settings.data.PostFxMaterial0)
            {
                for (int i = 0; i < postFxCount; i++)
                {
                    Settings.postFxMaterials[i] = Content.LoadAsync<MaterialBase>(postFxMaterials[i]);
                }
            }
        }

        /// <summary>
        /// Sends cached Settings.data to unmanaged data
        /// </summary>
        /// <param name="ptr">The unmanaged data pointer.</param>
        /// <returns>True if data has been modified, otherwise false.</returns>
        internal unsafe bool Internal_GetData(IntPtr ptr)
        {
            var postFx = Settings.postFxMaterials;
            var postFxLength = postFx?.Length ?? 0;

            
            Guid[] postFxMaterialsIds = new Guid[postFxLength];
            for (int i = 0; i < postFxLength; i++)
            {
                postFxMaterialsIds[i] = postFx[i]?.ID ?? Guid.Empty;
            }

            bool posFxMaterialsChanged = false;
            fixed (Guid* postFxMaterials = &Settings.data.PostFxMaterial0)
            {
                for (int i = 0; i < Settings.data.PostFxMaterialsCount; i++)
                {
                    if (postFxMaterials[i] != postFxMaterialsIds[i])
                    {
                        posFxMaterialsChanged = true;
                    }
                    postFxMaterials[i] = postFxMaterialsIds[i];
                }
                Settings.data.PostFxMaterialsCount = postFxLength;
            }

            if (!Settings.isDataDirty && !posFxMaterialsChanged)
                return false;

            Settings.isDataDirty = false;
            Marshal.StructureToPtr(Settings.data, ptr, false);
            return true;
        }
    }
}
