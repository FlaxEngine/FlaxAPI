// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Window used to show and edit current graphics rendering settings via <see cref="GraphicsQuality"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class GraphicsQualityWindow : EditorWindow
    {
        private readonly CustomEditorPresenter _presenter;

        /// <summary>
        /// MVC or something. https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller
        /// </summary>
        private class ViewModel
        {
            /// <summary>
            /// Enables rendering synchronization with the refresh rate of the display device to avoid "tearing" artifacts.
            /// </summary>
            [EditorOrder(0), EditorDisplay("Rendering", "Use V-Sync"), Tooltip("Enables rendering synchronization with the refresh rate of the display device to avoid \"tearing\" artifacts.")]
            public bool UseVSync
            {
                get => GraphicsQuality.UseVSync;
                set => GraphicsQuality.UseVSync = value;
            }

            /// <summary>
            /// Anti Aliasing quality setting.
            /// </summary>
            [EditorOrder(1000), EditorDisplay("Quality", "AA Quality"), Tooltip("Anti Aliasing quality.")]
            public Quality AAQuality
            {
                get => GraphicsQuality.AAQuality;
                set => GraphicsQuality.AAQuality = value;
            }

            /// <summary>
            /// Screen Space Reflections quality.
            /// </summary>
            [EditorOrder(1100), EditorDisplay("Quality", "SSR Quality"), Tooltip("Screen Space Reflections quality.")]
            public Quality SSRQuality
            {
                get => GraphicsQuality.SSRQuality;
                set => GraphicsQuality.SSRQuality = value;
            }

            /// <summary>
            /// Screen Space Ambient Occlusion quality setting.
            /// </summary>
            [EditorOrder(1200), EditorDisplay("Quality", "SSAO Quality"), Tooltip("Screen Space Ambient Occlusion quality setting.")]
            public Quality SSAOQuality
            {
                get => GraphicsQuality.SSAOQuality;
                set => GraphicsQuality.SSAOQuality = value;
            }

            /// <summary>
            /// Volumetric Fog quality setting.
            /// </summary>
            [EditorOrder(1250), EditorDisplay("Quality", "Volumetric Fog Quality"), Tooltip("Volumetric Fog quality setting.")]
            public Quality VolumetricFogQuality
            {
                get => GraphicsQuality.VolumetricFogQuality;
                set => GraphicsQuality.VolumetricFogQuality = value;
            }

            /// <summary>
            /// The shadows quality.
            /// </summary>
            [EditorOrder(1300), EditorDisplay("Quality", "Shadows Quality"), Tooltip("The shadows quality.")]
            public Quality ShadowsQuality
            {
                get => GraphicsQuality.ShadowsQuality;
                set => GraphicsQuality.ShadowsQuality = value;
            }

            /// <summary>
            /// The shadow maps quality (textures resolution).
            /// </summary>
            [EditorOrder(1310), EditorDisplay("Quality", "Shadow Maps Quality"), Tooltip("The shadow maps quality (textures resolution).")]
            public Quality ShadowMapsQuality
            {
                get => GraphicsQuality.ShadowMapsQuality;
                set => GraphicsQuality.ShadowMapsQuality = value;
            }

            /// <summary>
            /// Enables cascades splits blending for directional light shadows.
            /// </summary>
            [EditorOrder(1320), EditorDisplay("Quality", "Allow CSM Blending"), Tooltip("Enables cascades splits blending for directional light shadows.")]
            public bool AllowCSMBlending
            {
                get => GraphicsQuality.AllowCSMBlending;
                set => GraphicsQuality.AllowCSMBlending = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public GraphicsQualityWindow(Editor editor)
        : base(editor, true, ScrollBars.Vertical)
        {
            Title = "Graphics Quality";

            _presenter = new CustomEditorPresenter(null);
            _presenter.Panel.Parent = this;
            _presenter.Select(new ViewModel());
        }
    }
}
