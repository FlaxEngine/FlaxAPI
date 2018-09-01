// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Editor control that allows user to select a platform icon.
    /// </summary>
    public class PlatformSelector : TilesPanel
    {
        private struct PlatformData
        {
            public PlatformType PlatformType;
            public Sprite Icon;

            public PlatformData(PlatformType type, Sprite icon)
            {
                PlatformType = type;
                Icon = icon;
            }
        }

        private Color _mouseOverColor;
        private Color _selectedColor;
        private Color _defaultColor;
        private PlatformType _selected;

        /// <summary>
        /// Gets or sets the selected platform.
        /// </summary>
        public PlatformType Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    bool isValid = false;
                    for (int i = 0; i < _children.Count; i++)
                    {
                        if (_children[i] is Image img)
                        {
                            if ((PlatformType)img.Tag == value)
                            {
                                isValid = true;
                                img.Color = _selectedColor;
                                img.MouseOverColor = _selectedColor;
                            }
                            else
                            {
                                img.Color = _defaultColor;
                                img.MouseOverColor = _mouseOverColor;
                            }
                        }
                    }

                    if (isValid)
                    {
                        _selected = value;
                        SelectedChanged?.Invoke(_selected);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when selected platform gets changed.
        /// </summary>
        public event Action<PlatformType> SelectedChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformSelector"/> class.
        /// </summary>
        public PlatformSelector()
        {
            var icons = Editor.Instance.Icons;
            var platforms = new[]
            {
                new PlatformData(PlatformType.Windows, icons.Windows),
                new PlatformData(PlatformType.XboxOne, icons.XboxOne),
                new PlatformData(PlatformType.WindowsStore, icons.WindowsStore),
            };

            const float IconSize = 48.0f;
            TileSize = new Vector2(IconSize);
            AutoResize = true;

            _mouseOverColor = Color.White;
            _selectedColor = Color.White;
            _defaultColor = new Color(0.6f);

            for (int i = 0; i < platforms.Length; i++)
            {
                var tile = new Image
                {
                    Brush = new SpriteBrush(platforms[i].Icon),
                    MouseOverColor = _mouseOverColor,
                    Color = _defaultColor,
                    Tag = platforms[i].PlatformType,
                    TooltipText = CustomEditors.CustomEditorsUtil.GetPropertyNameUI(platforms[i].PlatformType.ToString()),
                    Parent = this,
                };
                tile.Clicked += OnTileClicked;
            }

            // Select the first platform
            _selected = platforms[0].PlatformType;
            ((Image)Children[0]).Color = _selectedColor;
            ((Image)Children[0]).MouseOverColor = _selectedColor;
        }

        private void OnTileClicked(Image image, MouseButton MouseButton)
        {
            if (MouseButton == MouseButton.Left)
            {
                Selected = (PlatformType)image.Tag;
            }
        }
    }
}
