////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content proxy for <see cref="SceneItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetProxy" />
    public sealed class SceneProxy : JsonAssetProxy
    {
        /// <summary>
        /// The scene files extension.
        /// </summary>
        public static readonly string Extension = "scene";

        /// <inheritdoc />
        public override string Name => "Scene";

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Scene;

        /// <inheritdoc />
        public override string FileExtension => Extension;

        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is SceneItem;
        }

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath)
        {
            Editor.Instance.Scene.CreateSceneFile(outputPath);
        }

        /// <inheritdoc />
        public override EditorWindow Open(ContentItem item)
        {
            // Load scene
            Editor.Instance.Scene.OpenScene((item as SceneItem).ID);

            return null;
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xbb37ef);

    }
}
