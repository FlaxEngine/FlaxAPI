////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Tool strip with child items.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class ToolStrip : ContainerControl
    {
        public const int DefaultMarginV = 1;
        public const int DefaultMarginH = 2;
        public const int DefaultHeight = 34;

        /// <summary>
        /// Event fired when button gets clicked.
        /// </summary>
        public Action<int> OnButtonClicked;

        /// <summary>
        /// Tries to get the last button.
        /// </summary>
        public ToolStripButton LastButton
        {
            get
            {
                for (int i = _children.Count - 1; i >= 0; i--)
                {
                    if (_children[i] is ToolStripButton button)
                        return button;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets amount of buttons that has been added
        /// </summary>
        /// <returns>Buttons count</returns>
        public int ButtonsCount
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i] is ToolStripButton)
                        result++;
                }
                return result;
            }
        }

	    /// <summary>
	    /// Gets the height for the items.
	    /// </summary>
	    public float ItemsHeight => Height - 2 * DefaultMarginV;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStrip"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        public ToolStrip(float height = DefaultHeight)
            : base(false, 0, 0, 100, height)
        {
            DockStyle = DockStyle.Top;
        }

        /// <summary>
        /// Tries to get button with given id.
        /// </summary>
        /// <param name="id">The button id.</param>
        /// <returns>Found button or null.</returns>
        public ToolStripButton GetButton(int id)
        {
            // TODO: we could build cache for buttons to access O(1) from lookup by id

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ToolStripButton button && button.ID == id)
                {
                    return button;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sprite">The icon sprite.</param>
        /// <returns>The button.</returns>
        public ToolStripButton AddButton(int id, Sprite sprite)
        {
            return AddChild(new ToolStripButton(ItemsHeight, id, ref sprite));
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sprite">The icon sprite.</param>
        /// <param name="text">The text.</param>
        /// <returns>The button.</returns>
        public ToolStripButton AddButton(int id, Sprite sprite, string text)
        {
            return AddChild(new ToolStripButton(ItemsHeight, id, ref sprite, ref text));
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        /// <returns>The button.</returns>
        public ToolStripButton AddButton(int id, string text)
        {
            return AddChild(new ToolStripButton(ItemsHeight, id, ref Sprite.Invalid, ref text));
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <returns>The separator.</returns>
        public ToolStripSeparator AddSeparator()
        {
            return AddChild(new ToolStripSeparator(ItemsHeight));
        }

        internal void OnButtonClickedInternal(int id)
        {
            OnButtonClicked?.Invoke(id);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            BackgroundColor = Style.Current.LightBackground;
            base.Draw();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Arrange controls
            float x = DefaultMarginH;
            float h = ItemsHeight;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible)
                {
                    var w = c.Width;
                    c.Bounds = new Rectangle(x, DefaultMarginV, w, h);
                    x += w + DefaultMarginH;
                }
            }
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }
    }
}
