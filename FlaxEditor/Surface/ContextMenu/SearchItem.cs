// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// The <see cref="ContentFinder"/> item.
    /// </summary>
    public class SearchItem : ContainerControl
    {
        private static Texture _flaxTexture = FlaxEngine.Content.Load<Texture>(Globals.EditorFolder + @"/miniLogo.flax");
        private ContentFinder _finder;

        public string Name;
        public string Type;
        public object Item;

        public SearchItem(string name, string type, object item, ContentFinder finder)
        {
            Name = name;
            Type = type;
            Item = item;
            _finder = finder;
        }

        public void Build(float itemHeight, float logoSize)
        {
            // TODO: Icon of asset editor if it's a third party app
            var image = AddChild<Image>();
            image.Brush = new TextureBrush(_flaxTexture);

            image.Size = new Vector2(logoSize);
            image.X = 5;
            image.Y = (itemHeight - logoSize) / 2;

            var nameLabel = AddChild<Label>();
            nameLabel.X = image.X + image.Width + 5;
            nameLabel.Height = 25;
            nameLabel.Y = (itemHeight - nameLabel.Height) / 2;
            nameLabel.Text = Name;
            nameLabel.HorizontalAlignment = TextAlignment.Near;

            var typeLabel = AddChild<Label>();
            typeLabel.Height = 25;
            typeLabel.Y = (itemHeight - nameLabel.Height) / 2;
            typeLabel.X = X + Width - typeLabel.Width - 17;
            typeLabel.HorizontalAlignment = TextAlignment.Far;
            typeLabel.Text = Type;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                _finder.Hide();
                Editor.Instance.ContentFinding.Open(Item);
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            base.OnMouseEnter(location);

            var root = RootWindow;
            if (root != null)
            {
                root.Cursor = CursorType.Hand;
            }

            _finder.SelectedItem = this;
            _finder.Hand = true;
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            var root = RootWindow;
            if (!_finder.Hand && root != null)
            {
                root.Cursor = CursorType.Default;
                _finder.SelectedItem = null;
            }
        }
    }
}
