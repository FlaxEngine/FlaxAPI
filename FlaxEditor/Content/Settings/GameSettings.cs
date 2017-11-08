////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The game settings asset archetype. Allows to edit asset via editor.
    /// </summary>
    public sealed class GameSettings : SettingsBase
    {
        /// <summary>
        /// The product full name.
        /// </summary>
        [EditorOrder(0), EditorDisplay("General"), Tooltip("The name of your product.")]
        public string ProductName;

        /// <summary>
        /// The company full name.
        /// </summary>
        [EditorOrder(10), EditorDisplay("General"), Tooltip("The name of you company.")]
        public string CompanyName;

        /// <summary>
        /// The product version. Separated with dots: major.minor.build.revision.
        /// </summary>
        [EditorOrder(20), EditorDisplay("General"), Tooltip("The product version. Separated with dots: major.minor.build.revision.")]
        public string Version;

        /// <summary>
        /// The default application icon.
        /// </summary>
        [EditorOrder(30), EditorDisplay("General"), Tooltip("The default icon of the application.")]
        public Texture Icon;

        /// <summary>
        /// Reference to the first scene to load on a game startup.
        /// </summary>
        [EditorOrder(900), EditorDisplay("Startup"), AssetReference(SceneItem.SceneAssetTypename, true), Tooltip("Reference to the first scene to load on a game startup")]
        public JsonAssetItem FirstScene;

        /// <summary>
        /// Reference to <see cref="TimeSettings"/> asset.
        /// </summary>
        [EditorOrder(1010), EditorDisplay("Other Settings"), AssetReference(typeof(TimeSettings), true), Tooltip("Reference to Time Settings asset")]
        public JsonAsset Time;

        /// <summary>
        /// Reference to <see cref="LayersAndTagsSettings"/> asset.
        /// </summary>
        [EditorOrder(1020), EditorDisplay("Other Settings"), AssetReference(typeof(LayersAndTagsSettings), true), Tooltip("Reference to Layers & Tags Settings asset")]
        public JsonAsset LayersAndTags;

        /// <summary>
        /// Reference to <see cref="PhysicsSettings"/> asset.
        /// </summary>
        [EditorOrder(1030), EditorDisplay("Other Settings"), AssetReference(typeof(PhysicsSettings), true), Tooltip("Reference to Physics Settings asset")]
        public JsonAsset Physics;

        /// <summary>
        /// Reference to <see cref="GraphicsSettings"/> asset.
        /// </summary>
        [EditorOrder(1040), EditorDisplay("Other Settings"), AssetReference(typeof(GraphicsSettings), true), Tooltip("Reference to Graphics Settings asset")]
        public JsonAsset Graphics;
        
        /// <summary>
        /// The custom settings to use with a game. Can be specified by the user to define game-specific options and be used by the external plugins (used as key-value pair).
        /// </summary>
        [EditorOrder(1100), EditorDisplay("Other Settings"), Tooltip("The custom settings to use with a game. Can be specified by the user to define game-specific options and be used by the external plugins (used as key-value pair).")]
        public Dictionary<string, JsonAsset> CustomSettings;

        /// <summary>
        /// Reference to <see cref="WindowsPlatformSettings"/> asset. Used to apply configuration on Windows platform.
        /// </summary>
        [EditorOrder(2010), EditorDisplay("Platform Settings", "Windows"), AssetReference(typeof(WindowsPlatformSettings), true), Tooltip("Reference to Windows Platform Settings asset")]
        public JsonAsset WindowsPlatform;
        
        /// <summary>
        /// Reference to <see cref="UWPPlatformSettings"/> asset. Used to apply configuration on Universal Windows Platform.
        /// </summary>
        [EditorOrder(2020), EditorDisplay("Platform Settings", "Universal Windows Platform"), AssetReference(typeof(UWPPlatformSettings), true), Tooltip("Reference to Universal Windows Platform Settings asset")]
        public JsonAsset UWPPlatform;
    }
}
