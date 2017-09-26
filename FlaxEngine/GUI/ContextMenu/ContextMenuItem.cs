////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context Menu child control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public abstract class ContextMenuItem : Control
    {
        /// <summary>
        /// Gets the parent context menu.
        /// </summary>
        /// <value>
        /// The parent context menu.
        /// </value>
        public ContextMenu ParentContextMenu { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuItem"/> class.
        /// </summary>
        /// <param name="parent">The parent context menu.</param>
        /// <param name="width">The initial width.</param>
        /// <param name="height">The initial height.</param>
        protected ContextMenuItem(ContextMenu parent, float width, float height)
            : base(0, 0, width, height)
        {
            CanFocus = false;
            ParentContextMenu = parent;
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            ParentContextMenu?.HideChild();

            base.OnMouseEnter(location);
        }
    }
}
