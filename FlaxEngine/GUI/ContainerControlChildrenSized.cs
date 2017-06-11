////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Container Control that bounds depends on children dimensions so should perform itself layout after children.
    /// </summary>
    public class ContainerControlChildrenSized : ContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControlChildrenSized"/> class.
        /// </summary>
        /// <param name="canFocus">Enable/disable auto focus.</param>
        protected ContainerControlChildrenSized(bool canFocus)
            : base(canFocus)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControlChildrenSized"/> class.
        /// </summary>
        /// <param name="canFocus">Enable/disable auto focus.</param>
        /// <param name="x">The control x position.</param>
        /// <param name="y">The control y position.</param>
        /// <param name="width">The control width.</param>
        /// <param name="height">The control height.</param>
        protected ContainerControlChildrenSized(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControlChildrenSized"/> class.
        /// </summary>
        /// <param name="canFocus">Enable/disable auto focus.</param>
        /// <param name="location">The control location.</param>
        /// <param name="size">The control size.</param>
        protected ContainerControlChildrenSized(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControlChildrenSized"/> class.
        /// </summary>
        /// <param name="canFocus">Enable/disable auto focus.</param>
        /// <param name="bounds">The control bounds.</param>
        protected ContainerControlChildrenSized(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
            IsLayoutLocked = true;
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            // Note: does the same as ContainerControl.PerformLayout but first update child controls and then itself.

            // Check if update is locked
            if (IsLayoutLocked)
                return;

            IsLayoutLocked = true;
            
            // Update children
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].PerformLayout();
            }
            
            // Update itself
            PerformLayoutSelf();

            IsLayoutLocked = false;
        }
    }
}
