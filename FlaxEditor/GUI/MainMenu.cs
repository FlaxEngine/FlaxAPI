// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Security.Permissions;
using FlaxEditor.Options;
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
        private Image _icon;
        private MainMenuButton _selected;
        private Button _closeButton;
        private Button _minimizeButton;
        private Button _maximizeButton;
        private Rectangle _moveRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        : base(0, 0, 1, 28)
        {
            AutoFocus = false;
            DockStyle = DockStyle.Top;

            if (!Editor.Instance.Options.Options.Interface.UseNativeWindowSystem)
            {
                BackgroundColor = Editor.Instance.Options.Options.Interface.TitleBarColor;
                _icon = new Image();
                _closeButton = new Button();
                _closeButton.Text = ((char)0xE711).ToString();
                _closeButton.BackgroundColor = Color.Transparent;
                _closeButton.BorderColor = Color.Transparent;
                _closeButton.BorderColorHighlighted = Color.Transparent;
                _closeButton.BorderColorSelected = Color.Transparent;
                _closeButton.TextColor = Style.Current.Foreground;
                _closeButton.Width = 46;
                _closeButton.TextColor = Color.White;
                _closeButton.BackgroundColorHighlighted = Color.Red;
                _closeButton.BackgroundColorSelected = _closeButton.BackgroundColorHighlighted.RGBMultiplied(1.3f);
                _closeButton.Clicked += () => { Editor.Instance.Exit(); };

                _minimizeButton = new Button();
                _minimizeButton.Text = ((char)0xE738).ToString();
                _minimizeButton.BackgroundColor = Color.Transparent;
                _minimizeButton.BorderColor = Color.Transparent;
                _minimizeButton.BorderColorHighlighted = Color.Transparent;
                _minimizeButton.BorderColorSelected = Color.Transparent;
                _minimizeButton.TextColor = Style.Current.Foreground;
                _minimizeButton.Width = 46;
                _minimizeButton.TextColor = Color.White;
                _minimizeButton.BackgroundColorHighlighted = Style.Current.BackgroundSelected.RGBMultiplied(1.3f);
                _minimizeButton.Clicked += () => { Editor.Instance.Windows.MainWindow.Minimize(); };

                _maximizeButton = new Button();
                _maximizeButton.Text = ((char)0xE923).ToString();
                _maximizeButton.BackgroundColor = Color.Transparent;
                _maximizeButton.BorderColor = Color.Transparent;
                _maximizeButton.BorderColorHighlighted = Color.Transparent;
                _maximizeButton.BorderColorSelected = Color.Transparent;
                _maximizeButton.TextColor = Style.Current.Foreground;
                _maximizeButton.Width = 46;
                _maximizeButton.TextColor = Color.White;
                _maximizeButton.BackgroundColorHighlighted = Style.Current.BackgroundSelected.RGBMultiplied(1.3f);
                _maximizeButton.Clicked += () =>
                {
                    if (Editor.Instance.Windows.MainWindow.IsMaximized)
                        Editor.Instance.Windows.MainWindow.Restore();
                    else
                        Editor.Instance.Windows.MainWindow.Maximize();
                };
            }
        }

        /// <summary>
        /// Initializes custom title bar system.
        /// </summary>
        public void Init(RootControl mainWindow)
        {
            if (!Editor.Instance.Options.Options.Interface.UseNativeWindowSystem)
            {
                AddChild(_icon);
                var tb = FlaxEngine.Content.Load<Texture>(Globals.EditorFolder + "/Icons/Textures/Flax.flax");
                _icon.Brush = new TextureBrush(tb);

                FontAsset fontAsset = FlaxEngine.Content.Load<FontAsset>(Globals.EditorFolder + EditorAssets.WindowIconsFont);
                Font iconFont = fontAsset.CreateFont(9);
                _closeButton.Font = new FontReference(iconFont);
                Font iconFont2 = fontAsset.CreateFont(8);
                _maximizeButton.Font = new FontReference(iconFont2);
                _minimizeButton.Font = new FontReference(iconFont);
                AddChild(_closeButton);
                AddChild(_maximizeButton);
                AddChild(_minimizeButton);

                mainWindow.RootWindow.Window.HitTest += HitTest;
            }
        }

        private WindowHitCodes HitTest(ref Vector2 mouse)
        {
            Vector2 mpos = RootWindow.Window.ScreenToClient(mouse);

            if (mpos.Y > RootWindow.Height - 5 && mpos.X < 5)
                return WindowHitCodes.BottomLeft;

            if (mpos.X > RootWindow.Width - 5 && mpos.Y > RootWindow.Height - 5)
                return WindowHitCodes.BottomRight;
            
            if (mpos.Y < 5 && mpos.X < 5)
                return WindowHitCodes.TopLeft;

            if (mpos.Y < 5 && mpos.X > RootWindow.Width - 5)
                return WindowHitCodes.TopRight;

            if (mpos.X > RootWindow.Width - 5)
                return WindowHitCodes.Right;

            if (mpos.X < 5)
                return WindowHitCodes.Left;

            if (mpos.Y < 5)
                return WindowHitCodes.Top;

            if (mpos.Y > RootWindow.Height - 5)
                return WindowHitCodes.Bottom;

            if (_moveRect.Contains(mpos))
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
        protected override void PerformLayoutSelf()
        {
            Width = RootWindow.Width;
        }

        /// <inheritdoc />
        public override void PerformLayout(bool force = false)
        {
            base.PerformLayout(force);

            float x = 0;

            if (!Editor.Instance.Options.Options.Interface.UseNativeWindowSystem)
            {
                // Icon
                _icon.X = x;
                _icon.KeepAspectRatio = false;
                _icon.Margin = new Margin(6, 6, 6, 6);
                _icon.Width = Height;
                _icon.Height = Height;
                x += _icon.Width;
            }
            
            // Arrange controls
            MainMenuButton _rightestButton = null;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c is MainMenuButton b && c.Visible)
                {
                    b.Height = Height;
                    b.Location = new Vector2(x, 0);

                    if (_rightestButton == null)
                        _rightestButton = b;
                    else if (_rightestButton.X < b.X)
                        _rightestButton = b;

                    x += b.Width;
                }
            }

            if (!Editor.Instance.Options.Options.Interface.UseNativeWindowSystem)
            {
                _closeButton.Height = Height;
                _closeButton.X = Width - _closeButton.Width;

                _maximizeButton.Height = Height;
                _maximizeButton.X = _closeButton.X - _maximizeButton.Width;

                _minimizeButton.Height = Height;
                _minimizeButton.X = _maximizeButton.X - _minimizeButton.Width;

                _moveRect = new Rectangle(_rightestButton?.Right ?? _icon.Right, 0, _minimizeButton.X - _rightestButton?.Right ?? _icon.Right, Height);
            }
        }
    }
}
