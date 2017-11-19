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
        private bool _centerMouse;

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

            // Setup viewport
            _viewport = new RenderOutputControl(task)
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };
        }

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            _gameStartTime = Time.UnscaledTime;
            _centerMouse = true;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (_centerMouse)
            {
                _centerMouse = false;

                // Move mouse to the game window center (provides better usage when game is locking a cursor)
                Vector2 center = PointToWindow(Size * 0.5f);
                ParentWindow.MousePosition = center;
            }

        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (Editor.StateMachine.IsPlayMode)
            {
                var style = Style.Current;

                float time = Time.UnscaledTime - _gameStartTime;
                float timeout = 3.0f;
                float fadeOutTime = 1.0f;
                float animTime = time - timeout;
                if (animTime < 0)
                {
                    float alpha = Mathf.Clamp01(-animTime / fadeOutTime);
                    Rectangle rect = new Rectangle(new Vector2(6), Size - 12);
                    Render2D.DrawText(style.FontSmall, "Press Shift+F11 to unlock the mouse", rect + new Vector2(1.0f), Color.Black * alpha, TextAlignment.Near, TextAlignment.Far);
                    Render2D.DrawText(style.FontSmall, "Press Shift+F11 to unlock the mouse", rect, Color.White * alpha, TextAlignment.Near, TextAlignment.Far);
                }

                timeout = 1.0f;
                fadeOutTime = 0.6f;
                animTime = time - timeout;
                if (animTime < 0)
                {
                    float alpha = Mathf.Clamp01(-animTime / fadeOutTime);
                    Render2D.DrawRectangle(new Rectangle(new Vector2(4), Size - 8), Color.Orange * alpha, true);
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
