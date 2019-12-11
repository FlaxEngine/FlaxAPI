// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Menu strip with child buttons.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public sealed class MainMenu : ContainerControl
    {
        private bool _useCustomWindowSystem;
        private Image _icon;
        private Label _title;
        private MainMenuButton _selected;
        private Button _closeButton;
        private Button _minimizeButton;
        private Button _maximizeButton;
        private Window _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        public MainMenu(RootControl mainWindow)
        : base(0, 0, 1, 20)
        {
            _useCustomWindowSystem = !Editor.Instance.Options.Options.Interface.UseNativeWindowSystem;
            AutoFocus = false;
            DockStyle = DockStyle.Top;

            if (_useCustomWindowSystem)
            {
                BackgroundColor = Style.Current.LightBackground;
                Height = 28;

                var windowIcon = FlaxEngine.Content.LoadInternal<Texture>(EditorAssets.WindowIcon);
                FontAsset windowIconsFont = FlaxEngine.Content.LoadInternal<FontAsset>(EditorAssets.WindowIconsFont);
                Font iconFont = windowIconsFont?.CreateFont(9);

                _window = mainWindow.RootWindow.Window;
                _window.HitTest += OnHitTest;
                _window.Closed += OnWindowClosed;

                _icon = new Image
                {
                    Margin = new Margin(6, 6, 6, 6),
                    Brush = new TextureBrush(windowIcon),
                    KeepAspectRatio = false,
                    TooltipText = string.Format("{0}\nVersion {1}\nGraphics {2}", _window.Title, Globals.Version, GPUDevice.RendererType),
                    Parent = this,
                };

                _title = new Label(0, 0, Width, Height)
                {
                    Text = _window.Title,
                    HorizontalAlignment = TextAlignment.Center,
                    VerticalAlignment = TextAlignment.Center,
                    ClipText = true,
                    TextColor = new Color(0.7f, 0.7f, 0.7f, 1.0f),
                    TextColorHighlighted = new Color(0.7f, 0.7f, 0.7f, 1.0f),
                    Parent = this,
                };

                _closeButton = new Button
                {
                    Text = ((char)0xE711).ToString(),
                    Font = new FontReference(iconFont),
                    BackgroundColor = Color.Transparent,
                    BorderColor = Color.Transparent,
                    BorderColorHighlighted = Color.Transparent,
                    BorderColorSelected = Color.Transparent,
                    TextColor = Color.White,
                    Width = 46,
                    BackgroundColorHighlighted = Color.Red,
                    BackgroundColorSelected = Color.Red.RGBMultiplied(1.3f),
                    Parent = this,
                };
                _closeButton.Clicked += () => _window.Close(ClosingReason.User);

                _minimizeButton = new Button
                {
                    Text = ((char)0xE738).ToString(),
                    Font = new FontReference(iconFont),
                    BackgroundColor = Color.Transparent,
                    BorderColor = Color.Transparent,
                    BorderColorHighlighted = Color.Transparent,
                    BorderColorSelected = Color.Transparent,
                    TextColor = Color.White,
                    Width = 46,
                    BackgroundColorHighlighted = Style.Current.LightBackground.RGBMultiplied(1.3f),
                    Parent = this,
                };
                _minimizeButton.Clicked += () => _window.Minimize();

                _maximizeButton = new Button
                {
                    Text = ((char)0xE923).ToString(),
                    Font = new FontReference(iconFont),
                    BackgroundColor = Color.Transparent,
                    BorderColor = Color.Transparent,
                    BorderColorHighlighted = Color.Transparent,
                    BorderColorSelected = Color.Transparent,
                    TextColor = Color.White,
                    Width = 46,
                    BackgroundColorHighlighted = Style.Current.LightBackground.RGBMultiplied(1.3f),
                    Parent = this,
                };
                _maximizeButton.Clicked += () =>
                {
                    if (_window.IsMaximized)
                        _window.Restore();
                    else
                        _window.Maximize();
                };
            }
            else
            {
                BackgroundColor = Style.Current.LightBackground;
            }
        }

        private void OnWindowClosed()
        {
            if (_window != null)
            {
                _window.HitTest = null;
                _window = null;
            }
        }

        private WindowHitCodes OnHitTest(ref Vector2 mouse)
        {
            var pos = _window.ScreenToClient(mouse);

            if (_window.IsMinimized)
            {
                return WindowHitCodes.NoWhere;
            }

            if (!_window.IsMaximized)
            {
                var rootWindow = RootWindow;

                if (pos.Y > rootWindow.Height - 5 && pos.X < 5)
                    return WindowHitCodes.BottomLeft;

                if (pos.X > rootWindow.Width - 5 && pos.Y > rootWindow.Height - 5)
                    return WindowHitCodes.BottomRight;

                if (pos.Y < 5 && pos.X < 5)
                    return WindowHitCodes.TopLeft;

                if (pos.Y < 5 && pos.X > rootWindow.Width - 5)
                    return WindowHitCodes.TopRight;

                if (pos.X > rootWindow.Width - 5)
                    return WindowHitCodes.Right;

                if (pos.X < 5)
                    return WindowHitCodes.Left;

                if (pos.Y < 5)
                    return WindowHitCodes.Top;

                if (pos.Y > rootWindow.Height - 5)
                    return WindowHitCodes.Bottom;
            }

            var menuPos = PointFromWindow(pos);
            var controlUnderMouse = GetChildAt(menuPos);
            var isMouseOverSth = controlUnderMouse != null && controlUnderMouse != _title;
            if (new Rectangle(Vector2.Zero, Size).Contains(ref menuPos) && !isMouseOverSth)
                return WindowHitCodes.Caption;

            return WindowHitCodes.Client;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>Created button control.</returns>
        public MainMenuButton AddButton(string text)
        {
            return AddChild(new MainMenuButton(text));
        }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>The button or null if missing.</returns>
        public MainMenuButton GetButton(string text)
        {
            MainMenuButton result = null;
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is MainMenuButton button && string.Equals(button.Text, text, StringComparison.OrdinalIgnoreCase))
                {
                    result = button;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Selects the specified button. Used by <see cref="MainMenuButton"/>.
        /// </summary>
        /// <param name="button">The button.</param>
        internal void Select(MainMenuButton button)
        {
            // Check if popup menu has been already hidden
            if (_selected != null && !_selected.ContextMenu.Visible)
                _selected = null;

            if (_selected != button)
            {
                _selected = button;

                if (_selected != null && _selected.ContextMenu.HasChildren)
                {
                    _selected.ContextMenu.Show(_selected, new Vector2(0, _selected.Height));
                }
            }
            else if (_selected != null)
            {
                _selected.ContextMenu.Hide();
                _selected = null;
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDoubleClick(location, buttons))
                return true;

            if (_useCustomWindowSystem)
            {
                if (_window.IsMaximized)
                    _window.Restore();
                else
                    _window.Maximize();
            }

            return true;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            float x = 0;

            if (_useCustomWindowSystem)
            {
                // Icon
                _icon.X = x;
                _icon.Size = new Vector2(Height);
                x += _icon.Width;
            }

            // Arrange controls
            MainMenuButton rightMostButton = null;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c is MainMenuButton b && c.Visible)
                {
                    b.Height = Height;
                    b.Location = new Vector2(x, 0);

                    if (rightMostButton == null)
                        rightMostButton = b;
                    else if (rightMostButton.X < b.X)
                        rightMostButton = b;

                    x += b.Width;
                }
            }

            if (_useCustomWindowSystem)
            {
                // Buttons
                _closeButton.Height = Height;
                _closeButton.X = Width - _closeButton.Width;
                _maximizeButton.Height = Height;
                _maximizeButton.X = _closeButton.X - _maximizeButton.Width;
                _minimizeButton.Height = Height;
                _minimizeButton.X = _maximizeButton.X - _minimizeButton.Width;

                // Title
                _title.Bounds = new Rectangle(x + 2, 0, _minimizeButton.Left - x - 4, Height);
                //_title.Text = _title.Width < 300.0f ? Editor.Instance.ProjectInfo.Name : _window.Title;
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_window != null)
            {
                _window.Closed -= OnWindowClosed;
                OnWindowClosed();
            }
        }
    }
}
