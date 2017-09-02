////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using FlaxEngine.Utilities;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Provides in-editor play mode simulation.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class GameWindow : EditorWindow
    {
        private readonly RenderOutputControl _viewport;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public GameWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Game";

            var task = MainRenderTask.Instance;
            task.Begin += OnBegin;

            // Setup viewport
            _viewport = new RenderOutputControl(task);
            _viewport.DockStyle = DockStyle.Fill;
            _viewport.Parent = this;
        }

        private void OnBegin(SceneRenderTask sceneRenderTask)
        {
            var camera = sceneRenderTask.Camera;
            if (camera)
            {
                // Fix aspect ratio to fit the current output dimensions
                camera.CustomAspectRatio = Width / Height;
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            if (ParentWindow.GetKeyDown(KeyCode.F12))
            {
                Screenshot.Capture();
            }

            base.Update(deltaTime);
        }
    }
}
