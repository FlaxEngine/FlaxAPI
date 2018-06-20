// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// The root container conrol used to sort and manage child UICanvas controls. Helps with sending input events.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    internal sealed class CanvasContainer : ContainerControl
    {
        internal CanvasContainer()
        {
            DockStyle = DockStyle.Fill;
            CanFocus = false;
        }

        /// <summary>
        /// Sorts the canvases by order.
        /// </summary>
        public void SortCanvases()
        {
            Children.Sort(SortCanvas);
        }

        private int SortCanvas(Control a, Control b)
        {
            return ((CanvasRootControl)a).Canvas.Order - ((CanvasRootControl)b).Canvas.Order;
        }

        /// <inheritdoc />
        public override bool ContainsFocus => false;

        /// <inheritdoc />
        public override bool IsFocused => false;

        /// <inheritdoc />
        public override void OnChildrenChanged()
        {
            SortCanvases();

            base.OnChildrenChanged();
        }

        /// <inheritdoc />
        protected override void DrawChildren()
        {
            // Draw all screen space canvases
            for (int i = 0; i < _children.Count; i++)
            {
                var child = (CanvasRootControl)_children[i];

                if (child.Visible && child.Canvas.RenderMode == CanvasRenderMode.ScreenSpace)
                {
                    child.Draw();
                }
            }
        }
    }
}
