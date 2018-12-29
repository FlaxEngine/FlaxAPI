// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Options;
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
        private readonly GameRoot _guiRoot;
        private bool _showGUI = true;
        private float _gameStartTime;

        /// <summary>
        /// Gets the viewport.
        /// </summary>
        public RenderOutputControl Viewport => _viewport;

        /// <summary>
        /// Gets or sets a value indicating whether show game GUI in the view or keep it hidden.
        /// </summary>
        public bool ShowGUI
        {
            get => _showGUI;
            set
            {
                if (value != _showGUI)
                {
                    _showGUI = value;

                    // Update root if it's in game
                    if (Editor.StateMachine.IsPlayMode)
                        _guiRoot.Visible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether center mouse position on window focus in play mode. Helps when working with games that lock mouse cursor.
        /// </summary>
        public bool CenterMouseOnFocus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether auto-focus game window on play mode start.
        /// </summary>
        public bool FocusOnPlay { get; set; }

        private class GameRoot : ContainerControl
        {
            public bool EnableEvents => SceneManager.IsGameLogicRunning;

            public override bool OnCharInput(char c)
            {
                if (!EnableEvents)
                    return false;

                return base.OnCharInput(c);
            }

            public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
            {
                if (!EnableEvents)
                    return DragDropEffect.None;

                return base.OnDragDrop(ref location, data);
            }

            public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
            {
                if (!EnableEvents)
                    return DragDropEffect.None;

                return base.OnDragEnter(ref location, data);
            }

            public override void OnDragLeave()
            {
                if (!EnableEvents)
                    return;

                base.OnDragLeave();
            }

            public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
            {
                if (!EnableEvents)
                    return DragDropEffect.None;

                return base.OnDragMove(ref location, data);
            }

            public override bool OnKeyDown(Keys key)
            {
                if (!EnableEvents)
                    return false;

                return base.OnKeyDown(key);
            }

            public override void OnKeyUp(Keys key)
            {
                if (!EnableEvents)
                    return;

                base.OnKeyUp(key);
            }

            public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
            {
                if (!EnableEvents)
                    return false;

                return base.OnMouseDoubleClick(location, buttons);
            }

            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (!EnableEvents)
                    return false;

                return base.OnMouseDown(location, buttons);
            }

            public override void OnMouseEnter(Vector2 location)
            {
                if (!EnableEvents)
                    return;

                base.OnMouseEnter(location);
            }

            public override void OnMouseLeave()
            {
                if (!EnableEvents)
                    return;

                base.OnMouseLeave();
            }

            public override void OnMouseMove(Vector2 location)
            {
                if (!EnableEvents)
                    return;

                base.OnMouseMove(location);
            }

            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (!EnableEvents)
                    return false;

                return base.OnMouseUp(location, buttons);
            }

            public override bool OnMouseWheel(Vector2 location, float delta)
            {
                if (!EnableEvents)
                    return false;

                return base.OnMouseWheel(location, delta);
            }
        }

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
                CanFocus = false,
                Parent = this
            };

            // Override the game GUI root
            _guiRoot = new GameRoot
            {
                DockStyle = DockStyle.Fill,
                //Visible = false,
                CanFocus = false,
                Parent = _viewport
            };
            RootControl.GameRoot = _guiRoot;
            Editor.StateMachine.PlayingState.SceneDuplicating += PlayingStateOnSceneDuplicating;
            Editor.StateMachine.PlayingState.SceneRestored += PlayingStateOnSceneRestored;

            // Link editor options
            var options = Editor.Options;
            options.OptionsChanged += OnOptionsChanged;
            OnOptionsChanged(options.Options);
        }

        private void OnOptionsChanged(EditorOptions options)
        {
            CenterMouseOnFocus = options.Interface.CenterMouseOnGameWinFocus;
            FocusOnPlay = options.Interface.FocusGameWinOnPlay;
        }

        private void PlayingStateOnSceneDuplicating()
        {
            // Remove reaming GUI controls so loaded scene can add own GUI
            //_guiRoot.DisposeChildren();

            // Show GUI
            //_guiRoot.Visible = _showGUI;
        }

        private void PlayingStateOnSceneRestored()
        {
            // Remove reaming GUI controls so loaded scene can add own GUI
            //_guiRoot.DisposeChildren();

            // Hide GUI
            //_guiRoot.Visible = false;
        }

        /// <summary>
        /// Sets the mouse position (handles call from the game scripts). Skips if editor is not focused on a game to prevent mouse position stealing.
        /// </summary>
        /// <param name="val">The value.</param>
        public void SetGameMousePosition(ref Vector2 val)
        {
            if (Root is WindowRootControl win && win.Window.IsFocused && win.ContainsFocus && Viewport.IsMouseOver)
            {
                win.MousePosition = Viewport.PointToWindow(val);
            }
        }

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            _gameStartTime = Time.UnscaledGameTime;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (Camera.MainCamera == null)
            {
                var style = Style.Current;

                Render2D.DrawText(style.FontLarge, "No camera", new Rectangle(Vector2.Zero, Size), Color.White, TextAlignment.Center, TextAlignment.Center);
            }

            if (Editor.StateMachine.IsPlayMode)
            {
                var style = Style.Current;

                float time = Time.UnscaledGameTime - _gameStartTime;
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
                    Render2D.DrawRectangle(new Rectangle(new Vector2(4), Size - 8), Color.Orange * alpha);
                }
            }
        }

        /// <summary>
        /// Unlocks the mouse if game window is focused during play mode.
        /// </summary>
        public void UnlockMouseInPlay()
        {
            if (Editor.StateMachine.IsPlayMode)
            {
                Screen.CursorVisible = true;
                Focus(null);
                Editor.Windows.MainWindow.Focus();
                if (Editor.Windows.PropertiesWin.IsDocked)
                    Editor.Windows.PropertiesWin.Focus();
                Screen.CursorVisible = true;
            }
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            switch (key)
            {
            case Keys.Pause:
                Editor.Simulation.RequestResumeOrPause();
                UnlockMouseInPlay();
                break;
            case Keys.F12:
                Screenshot.Capture();
                break;
            case Keys.F11:
            {
                if (Root.GetKey(Keys.Shift))
                {
                    // Unlock mouse in game mode
                    UnlockMouseInPlay();
                }
                break;
            }
            }

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        public override void OnStartContainsFocus()
        {
            base.OnStartContainsFocus();

            // Center mouse in play mode
            if (CenterMouseOnFocus && Editor.StateMachine.IsPlayMode)
            {
                Vector2 center = PointToWindow(Size * 0.5f);
                Root.MousePosition = center;
            }
        }

        /// <inheritdoc />
        public override void OnEndContainsFocus()
        {
            base.OnEndContainsFocus();

            // Restore cursor visibility (could be hidden by the game)
            Screen.CursorVisible = true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            var result = base.OnMouseDown(location, buttons);

            // Catch user focus
            if (!ContainsFocus)
                Focus();

            return result;
        }
    }
}
