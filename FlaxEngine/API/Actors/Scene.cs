////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    public sealed partial class Scene
    {
        /// <summary>
        /// Saves this scene to the asset.
        /// </summary>
        /// <returns>True if action fails, otherwise false.</returns>
        public bool SaveScene(Scene scene)
        {
            return SceneManager.SaveScene(this);
        }

        /// <summary>
        /// Saves this scene to the asset. Done in the background.
        /// </summary>
        public void SaveSceneAsync(Scene scene)
        {
            SceneManager.SaveSceneAsync(this);
        }

        /// <summary>
        /// Unloads this scene.
        /// </summary>
        /// <returns>True if action fails, otherwise false.</returns>
        public bool UnloadScene(Scene scene)
        {
            return SceneManager.UnloadScene(this);
        }

        /// <summary>
        /// Unloads this scene. Done in the background.
        /// </summary>
        public void UnloadSceneAsync(Scene scene)
        {
            SceneManager.UnloadSceneAsync(this);
        }

        /// <summary>
        /// Gets or sets the lightmap settings (per scene).
        /// </summary>
        [EditorDisplay("Lightmap Settings", EditorDisplayAttribute.InlineStyle)]
        public LightmapSettings LightmapSettings
        {
            get
            {
                Internal_GetLightmapSettings(unmanagedPtr, out var data);
                return LightmapSettings.FromInternal(ref data);
            }
            set
            {
                var data = LightmapSettings.ToInternal(ref value);
                Internal_SetLightmapSettings(unmanagedPtr, ref data);
            }
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLightmapSettings(IntPtr obj, out LightmapSettings.Internal data);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLightmapSettings(IntPtr obj, ref LightmapSettings.Internal data);
#endif

        #endregion
    }
}
