////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Window used to show and edit current gaephics rendering settings via <see cref="GraphicsQuality"/>.
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
            /// Screen Space Reflections quality.
            /// </summary>
            [EditorOrder(1000), EditorDisplay("Quality", "SSR Quality"), Tooltip("Screen Space Reflections quality.")]
            public Quality SSRQuality
            {
                get => GraphicsQuality.SSRQuality;
                set => GraphicsQuality.SSRQuality = value;
            }

            /// <summary>
            /// Screen Space Ambient Occlusion quality setting.
            /// </summary>
            [EditorOrder(1010), EditorDisplay("Quality", "SSAO Quality"), Tooltip("Screen Space Ambient Occlusion quality setting.")]
            public Quality SSAOQuality
            {
                get => GraphicsQuality.SSAOQuality;
                set => GraphicsQuality.SSAOQuality = value;
            }

            /// <summary>
            /// The shadows quality.
            /// </summary>
            [EditorOrder(1020), EditorDisplay("Quality", "Shadows Quality"), Tooltip("The shadows quality.")]
            public Quality ShadowsQuality
            {
                get => GraphicsQuality.ShadowsQuality;
                set => GraphicsQuality.ShadowsQuality = value;
            }

            /// <summary>
            /// The shadow maps quality (textures resolution).
            /// </summary>
            [EditorOrder(1030), EditorDisplay("Quality", "Shadow Maps Quality"), Tooltip("The shadow maps quality (textures resolution).")]
            public Quality ShadowMapsQuality
            {
                get => GraphicsQuality.ShadowMapsQuality;
                set => GraphicsQuality.ShadowMapsQuality = value;
            }

            /// <summary>
            /// Enables cascades splits blending for directional light shadows.
            /// </summary>
            [EditorOrder(1040), EditorDisplay("Quality", "Allow CSM Blending"), Tooltip("Enables cascades splits blending for directional light shadows.")]
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
