////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class ScrollableControl : ContainerControl
    {
        /// <summary>
        /// The view offset. Usefull to offset contents of the container (used by the scrollbars and drop panels).
        /// </summary>
        protected Vector2 _viewOffset;

        /// <summary>
        ///     Gets current view offset for all the controls (used by the scroll bars).
        /// </summary>
        public Vector2 ViewOffset => _viewOffset;

        /// <inheritdoc />
        public ScrollableControl(bool canFocus)
            : base(canFocus)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
        }

        /// <inheritdoc />
        protected override void DrawChildren()
        {
            // Draw all visible child controls
            bool hasViewOffset = !_viewOffset.IsZero;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];

                if (child.Visible)
                {
                    Matrix3x3 transform = child._cachedTransform;
                    if (hasViewOffset && child.IsScrollable)
                    {
                        transform.M31 += _viewOffset.X;
                        transform.M32 += _viewOffset.Y;
                    }

                    Render2D.PushTransform(ref transform);
                    child.Draw();
                    Render2D.PopTransform();
                }
            }
        }

        /// <inheritdoc />
        protected override bool IntersectsChildContent(Control child, Vector2 location, out Vector2 childSpaceLocation)
        {
            // Apply offset on scrollable controls
            if (child.IsScrollable)
                location -= _viewOffset;

            return child.IntersectsContent(ref location, out childSpaceLocation);
        }

        /// <inheritdoc />
        public override bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
        {
            // Little workaround to prevent applying offset when performing intersection test with this scrollable control.
            // Note that overriden PointFromParent applyies view offset.
            location = base.PointFromParent(locationParent);
            return ContainsPoint(ref location);
        }

        /// <inheritdoc />
        public override Vector2 PointToParent(Vector2 location)
        {
            return base.PointToParent(location) + _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 PointFromParent(Vector2 location)
        {
            return base.PointFromParent(location) - _viewOffset;
        }
    }
}
