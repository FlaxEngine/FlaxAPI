// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Helper collection of Flax Editor in-build asset names.
    /// </summary>
    public static class EditorAssets
    {
        internal class Cache
        {
            private static MaterialInstance _highlightMaterial;

            /// <summary>
            /// Gets the highlight material instance.
            /// </summary>
            public static MaterialInstance HighlightMaterialInstance
            {
                get
                {
                    if (!_highlightMaterial)
                    {
                        var material = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(HighlightMaterial);
                        if (material && !material.WaitForLoaded())
                        {
                            _highlightMaterial = material.CreateVirtualInstance();
                            OnEditorOptionsChanged(Editor.Instance.Options.Options);
                        }
                    }
                    return _highlightMaterial;
                }
            }

            public static void OnEditorOptionsChanged(Options.EditorOptions options)
            {
                var param = _highlightMaterial?.GetParam("Color");
                if (param != null)
                    param.Value = options.Visual.HighlightColor;
            }
        }

        /// <summary>
        /// The icons atlas.
        /// </summary>
        public static string IconsAtlas = "Editor/IconsAtlas";

        /// <summary>
        /// The primary font.
        /// </summary>
        public static string PrimaryFont = "Editor/Roboto-Regular";

        /// <summary>
        /// The Inconsolata Regular font.
        /// </summary>
        public static string InconsolataRegularFont = "Editor/Fonts/Inconsolata-Regular";

        /// <summary>
        /// The default font material.
        /// </summary>
        public static string DefaultFontMaterial = "Editor/DefaultFontMaterial";

        /// <summary>
        /// The highlight material.
        /// </summary>
        public static string HighlightMaterial = "Editor/Highlight Material";

        /// <summary>
        /// The highlight terrain material.
        /// </summary>
        public static string HighlightTerrainMaterial = "Editor/Terrain/Highlight Terrain Material";

        /// <summary>
        /// The terrain circle brush material.
        /// </summary>
        public static string TerrainCircleBrushMaterial = "Editor/Terrain/Circle Brush Material";

        /// <summary>
        /// The debug material (wireframe).
        /// </summary>
        public static string WiresDebugMaterial = "Editor/Wires Debug Material";

        /// <summary>
        /// The default sky cube texture.
        /// </summary>
        public static string DefaultSkyCubeTexture = "Editor/SimplySky";

        /// <summary>
        /// The IES Profile assets preview material.
        /// </summary>
        public static string IesProfilePreviewMaterial = "Editor/IesProfilePreviewMaterial";

        /// <summary>
        /// The foliage painting brush material.
        /// </summary>
        public static string FoliageBrushMaterial = "Editor/Gizmo/FoliageBrushMaterial";
    }
}
