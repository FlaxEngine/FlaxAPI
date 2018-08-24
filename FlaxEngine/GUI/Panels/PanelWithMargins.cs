// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Helper control class for other panels.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class PanelWithMargins : ContainerControl
    {
        /// <summary>
        /// The panel area margins.
        /// </summary>
        protected Margin _margin = new Margin(2.0f);

        /// <summary>
        /// The space between the items.
        /// </summary>
        protected float _spacing = 2;

        /// <summary>
        /// The auto size flag.
        /// </summary>
        protected bool _autoSize = true;

        /// <summary>
        /// The control offset.
        /// </summary>
        protected Vector2 _offset;

        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        [HideInEditor, NoSerialize]
        public float LeftMargin
        {
            get => _margin.Left;
            set
            {
                _margin.Left = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the right margin.
        /// </summary>
        [HideInEditor, NoSerialize]
        public float RightMargin
        {
            get => _margin.Right;
            set
            {
                _margin.Right = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the top margin.
        /// </summary>
        [HideInEditor, NoSerialize]
        public float TopMargin
        {
            get => _margin.Top;
            set
            {
                _margin.Top = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the bottom margin.
        /// </summary>
        [HideInEditor, NoSerialize]
        public float BottomMargin
        {
            get => _margin.Bottom;
            set
            {
                _margin.Bottom = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the child controls spacing.
        /// </summary>
        [EditorOrder(10), Tooltip("The child controls spacing (the space between controls).")]
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
        [EditorOrder(20), Tooltip("The child controls offset (additive).")]
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
        /// Gets or sets the value indicating whenever the panel size will be based on a children dimensions.
        /// </summary>
        [EditorOrder(30), Tooltip("If checked, the panel size will be based on a children dimensions.")]
        public bool AutoSize
        {
            get => _autoSize;
            set
            {
                _autoSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the panel area margin.
        /// </summary>
        [EditorOrder(40), Tooltip("The panel area margin.")]
        public Margin Margin
        {
            get => _margin;
            set
            {
                _margin = value;
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
