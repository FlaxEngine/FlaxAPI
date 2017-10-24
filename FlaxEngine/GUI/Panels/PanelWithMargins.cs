////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Helper control class for other panels.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class PanelWithMargins : ContainerControl
    {
        /// <summary>
        /// The left margin.
        /// </summary>
        protected float _leftMargin = 2;

        /// <summary>
        /// The right margin.
        /// </summary>
        protected float _rightMargin = 2;

        /// <summary>
        /// The top margin.
        /// </summary>
        protected float _topMargin = 2;

        /// <summary>
        /// The bottom margin.
        /// </summary>
        protected float _bottomMargin = 2;

        /// <summary>
        /// The space between the items.
        /// </summary>
        protected float _spacing = 2;

        /// <summary>
        /// The ocontorls ffset.
        /// </summary>
        protected Vector2 _offset;

        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        public float LeftMargin
        {
            get => _leftMargin;
            set
            {
                _leftMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the right margin.
        /// </summary>
        public float RightMargin
        {
            get => _rightMargin;
            set
            {
                _rightMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the top margin.
        /// </summary>
        public float TopMargin
        {
            get => _topMargin;
            set
            {
                _topMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the bottom margin.
        /// </summary>
        public float BottomMargin
        {
            get => _bottomMargin;
            set
            {
                _bottomMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the child controls spacing.
        /// </summary>
        public float Spacing
        {
            get => _spacing;
            set
            {
                _spacing = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the child controls offset (additive).
        /// </summary>
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelWithMargins"/> class.
        /// </summary>
        protected PanelWithMargins()
            : base(0, 0, 64, 64)
        {
            CanFocus = false;
            _performChildrenLayoutFirst = true;
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }
    }
}
