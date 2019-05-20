using System;
using FlaxEditor;
using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    public class FinderItem : Panel
    {
        private static Texture _flaxTexture = FlaxEngine.Content.Load<Texture>(Globals.EditorFolder+@"/miniLogo.flax");
        private int ItemNumber;
        public string Name;
        public string TypeName;
        public ContentItem Item;
        private ContentFinder _finder;
        public FinderItem(int count, string name, string typeName, ContentItem item, ContentFinder finder)
        {
            ItemNumber = count;
            Name = name;
            TypeName = typeName;
            Item = item;
            _finder = finder;
        }

        public void Build(float itemHeight, float logoSize)
        {
            Y = ItemNumber * itemHeight;
            Width = Parent.Width;
            Height = itemHeight;
            
            // TODO: Icon of asset editor if it's a third party app
            var image = AddChild<Image>();
            image.Brush = new TextureBrush(_flaxTexture);
            
            image.Size = new Vector2(logoSize);
            image.X = 5;
            image.Y = (itemHeight - logoSize)/2;

            var nameLabel = AddChild<Label>();
            nameLabel.X = image.X + image.Width + 5;
            nameLabel.Height = 25 ;
            nameLabel.Y = (itemHeight - nameLabel.Height) / 2;
            nameLabel.Text = Name;
            nameLabel.HorizontalAlignment = TextAlignment.Near;

            var typeLabel = AddChild<Label>();
            typeLabel.Height = 25;
            typeLabel.Y = (itemHeight - nameLabel.Height) / 2;
            typeLabel.X = X + Width - typeLabel.Width - 17;
            typeLabel.HorizontalAlignment = TextAlignment.Far;
            typeLabel.Text = TypeName;
        }

        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                _finder.Hide();
                Editor.Instance.ContentEditing.Open(Item);
            }
            
            return base.OnMouseUp(location, buttons);
        }

        public override void OnMouseEnter(Vector2 location)
        {
            base.OnMouseEnter(location);
            base.RootWindow.Cursor = CursorType.Hand;
            _finder.SelectedItem = this;
            _finder.Hand = true;
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();
            if (!_finder.Hand && base.RootWindow != null)
            {
                base.RootWindow.Cursor = CursorType.Default;
                _finder.SelectedItem = null;
            }
                
        }
    }
}
