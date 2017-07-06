////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Model asset preview editor viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewportArcBallCam" />
    public class ModelPreview : EditorViewportArcBallCam
    {
        private ModelActor _previewModel;
        private DirectionalLight _previewLight;
        private EnvironmentProbe _envProbe;
        private Sky _sky;

        /// <summary>
        /// Sets the model asset to preview.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public Model Model
        {
            set
            {
                // TODO: ....
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ModelPreview(bool useWidgets)
            : base(RenderTask.Create<SceneRenderTask>(), useWidgets, 50, Vector3.Zero)
        {
            DockStyle = DockStyle.Fill;

            Task.Enabled = false;
            Task.Flags = ViewFlags.DefaultModelPreview;

            SetView(new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f));

            _previewModel = new ModelActor();
        }

        /// <summary>
        /// Enables this preview rendering.
        /// </summary>
        public void Enable()
        {
            Task.Enabled = true;
        }

        /// <summary>
        /// Disables this preview rendering.
        /// </summary>
        public void Disable()
        {
            Task.Enabled = false;
        }


    }
}
