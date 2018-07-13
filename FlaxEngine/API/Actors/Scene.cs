// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    public sealed partial class Scene
    {
        /// <summary>
        /// The scene asset typename. Type of the serialized scene asset data. Hidden class for the scene assets. Actors deserialization rules are strictly controlled under the hood by the C++ core parts. Mostly because scene asset has the same ID as scene root actor so loading both managed objects for scene asset and scene will crash (due to object ids conflict). This type does not exist in the engine assembly.
        /// </summary>
        public const string AssetTypename = "FlaxEngine.SceneAsset";

        /// <summary>
        /// The scene asset typename used by the Editor asset picker control. Use it for asset reference picker filter.
        /// </summary>
        public const string EditorPickerTypename = "FlaxEditor.Content.SceneItem";

        /// <summary>
        /// Saves this scene to the asset.
        /// </summary>
        /// <returns>True if action fails, otherwise false.</returns>
        public bool Save()
        {
            return SceneManager.SaveScene(this);
        }

        /// <summary>
        /// Saves this scene to the asset. Done in the background.
        /// </summary>
        public void SaveAsync()
        {
            SceneManager.SaveSceneAsync(this);
        }

        /// <summary>
        /// Unloads this scene.
        /// </summary>
        /// <returns>True if action fails, otherwise false.</returns>
        public bool Unload()
        {
            return SceneManager.UnloadScene(this);
        }

        /// <summary>
        /// Unloads this scene. Done in the background.
        /// </summary>
        public void UnloadAsync()
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
