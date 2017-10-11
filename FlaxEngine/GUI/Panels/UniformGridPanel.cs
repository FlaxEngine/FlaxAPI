////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// A panel that evenly divides up available space between all of its children.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class UniformGridPanel : ContainerControl
    {
        private Margin _slotPadding;
        private int _slotsV, _slotsH;

        /// <summary>
        /// Gets or sets the padding given to each slot.
        /// </summary>
        public Margin SlotPadding
        {
            get => _slotPadding;
            set
            {
                _slotPadding = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the amount of slots horizontally. Use 0 to don't limit it.
        /// </summary>
        public int SlotsHorizontally
        {
            get => _slotsH;
            set
            {
                value = Mathf.Clamp(value, 0, 1000000);
                if (value != _slotsH)
                {
                    _slotsH = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of slots vertically. Use 0 to don't limit it.
        /// </summary>
        public int SlotsVertically
        {
            get => _slotsV;
            set
            {
                value = Mathf.Clamp(value, 0, 1000000);
                if (value != _slotsV)
                {
                    _slotsV = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformGridPanel"/> class.
        /// </summary>
        public UniformGridPanel()
            : this(2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniformGridPanel"/> class.
        /// </summary>
        /// <param name="slotPadding">The slot padding.</param>
        public UniformGridPanel(float slotPadding = 2)
        {
            SlotPadding = new Margin(slotPadding);
            _slotsH = _slotsV = 5;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();
            
            int slotsV = _slotsV;
            int slotsH = _slotsH;
            Vector2 slotSize;
            if (_slotsV + _slotsH == 0)
            {
                // TODO: what to do here?
                return;
            }
            else if (slotsH == 0)
            {
                float size = Height / slotsV;
                slotSize = new Vector2(size);
            }
            else if (slotsV == 0)
            {
                float size = Width / slotsH;
                slotSize = new Vector2(size);
            }
            else
            {
                slotSize = new Vector2(Width / slotsH, Height / slotsV);
            }

            int i = 0;
            for (int y = 0; y < slotsH; y++)
            {
                int end = Mathf.Min(slotsH, _children.Count - i);
                if (end <= 0)
                    break;

                for (int x = 0; x < end; x++)
                {
                    var slotBounds = new Rectangle(slotSize.X * x, slotSize.Y * y, slotSize.X, slotSize.Y);
                    _slotPadding.ShrinkRectangle(ref slotBounds);

                    var c = _children[i++];
                    c.Bounds = slotBounds;
                }
            }
        }
    }
}
