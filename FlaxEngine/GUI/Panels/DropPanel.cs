////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Drop Panel arranges control vertically and provides feature to collapse contents.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class DropPanel : ContainerControl
    {
        protected float _headerHeight = 14.0f, _headerMargin = 2.0f;
        protected bool _isClosed;
        protected bool _mouseOverHeader;
        protected bool _mouseDown;
        protected float _animationProgress = 1.0f;
        protected float _cachedHeight = 16.0f;
        
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>
        /// The header text.
        /// </value>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the height of the header.
        /// </summary>
        /// <value>
        /// The height of the header.
        /// </value>
        public float HeaderHeight
        {
            get => _headerHeight;
            set
            {
                _headerHeight = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the header margin.
        /// </summary>
        /// <value>
        /// The header margin.
        /// </value>
        public float HeaderMargin
        {
            get => _headerMargin;
            set
            {
                _headerMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropPanel"/> class.
        /// </summary>
        /// <param name="text">The header text.</param>
        public DropPanel(string text)
            : base(false, 0, 0, 64, 16.0f)
        {
            _performChildrenLayoutFirst = true;
            
            HeaderText = text;
        }

        // TODO: finish this control
    }
}
