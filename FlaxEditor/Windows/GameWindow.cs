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
        private float _gameStartTime;

        /// <summary>
        /// Gets the viewport.
        /// </summary>
        public RenderOutputControl Viewport => _viewport;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public GameWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Game";
            CanFocus = true;

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
                //camera.CustomAspectRatio = Width / Height;
            }
        }

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            _gameStartTime = Time.UnscaledTime;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (Editor.StateMachine.IsPlayMode)
            {
                float time = Time.UnscaledTime - _gameStartTime;
                float timeout = 3.0f;
                float fadeOutTime = 1.0f;
                time -= timeout;
                if (time < 0)
                {
                    float alpha = Mathf.Clamp01(-time / fadeOutTime);
                    Render2D.DrawText(Style.Current.FontSmall, "Press Shift+F11 to unlock the mouse", new Rectangle(new Vector2(1.0f, 1.0f), Size), Color.Black * alpha, TextAlignment.Near, TextAlignment.Far);
                    Render2D.DrawText(Style.Current.FontSmall, "Press Shift+F11 to unlock the mouse", new Rectangle(Vector2.Zero, Size), Color.White * alpha, TextAlignment.Near, TextAlignment.Far);
                }
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            if (key == KeyCode.F12)
            {
                Screenshot.Capture();
            }
            else if (key == KeyCode.F11)
            {
                if (ParentWindow.GetKey(KeyCode.Shift))
                {
                    // Unlock mouse in game mode
                    if (Editor.StateMachine.IsPlayMode)
                    {
                        Focus();
                        Defocus();
                        if (Editor.Windows.PropertiesWin.IsDocked)
                            Editor.Windows.PropertiesWin.Focus();
                    }
                }
            }

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            base.OnLostFocus();

            // Restore cursor visibility (could be hidden by the game)
            Screen.CursorVisible = true;
        }
    }
}
