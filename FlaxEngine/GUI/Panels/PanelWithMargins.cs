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
        protected float _leftMargin = 2, _rightMargin = 2;
        protected float _topMargin = 2, _bottomMargin = 2;
        protected float _spacing = 2;

        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        /// <value>
        /// The left margin.
        /// </value>
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
        /// <value>
        /// The right margin.
        /// </value>
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
        /// <value>
        /// The top margin.
        /// </value>
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
        /// <value>
        /// The bottom margin.
        /// </value>
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
        /// <value>
        /// The spacing.
        /// </value>
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
        /// Initializes a new instance of the <see cref="PanelWithMargins"/> class.
        /// </summary>
        /// <param name="canFocus">True if panel can got focused, otherwise false. Default is false.</param>
        protected PanelWithMargins(bool canFocus = false)
            : base(canFocus, 0, 0, 64, 64)
        {
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }
    }
}
