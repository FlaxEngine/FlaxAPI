////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.GUI
{
    /// <summary>
    /// <see cref="CustomEditor"/> properties list control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.PanelWithMargins" />
    public class PropertiesList : PanelWithMargins
    {
        // TODO: sync splitter for whole presenter

        /// <summary>
        /// The spliter size (in pixels).
        /// </summary>
        public const int SpliterSize = 2;

        /// <summary>
        /// The splitter margin (in pixels).
        /// </summary>
        public const int SplitterMargin = 4;

        private const int SpliterSizeHalf = SpliterSize / 2;

        private PropertiesListElement _element;
        private float _splitterValue;
        private Rectangle _splitterRect;
        private bool _splitterClicked, _mouseOverSplitter;

        /// <summary>
        /// Gets or sets the splitter value (always in range [0; 1]).
        /// </summary>
        /// <value>
        /// The splitter value (always in range [0; 1]).
        /// </value>
        public float SplitterValue
        {
            get => _splitterValue;
            set
            {
                value = Mathf.Clamp(value, 0.05f, 0.95f);
                if (!Mathf.NearEqual(_splitterValue, value))
                {
                    // Set new value
                    _splitterValue = value;

                    // Calculate rectangle and update panels
                    UpdateSplitRect();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesList"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public PropertiesList(PropertiesListElement element)
        {
            _element = element;
            _splitterValue = 0.4f;
            BottomMargin = TopMargin = RightMargin = SplitterMargin;
            UpdateSplitRect();
        }

        private void UpdateSplitRect()
        {
            _splitterRect = new Rectangle(Mathf.Clamp(_splitterValue * Width - SpliterSizeHalf, 0.0f, Width), 0, SpliterSize, Height);
            LeftMargin = _splitterValue * Width + SplitterMargin;
        }

        private void StartTracking()
        {
            // Start move
            _splitterClicked = true;

            // Start capturing mouse
            ParentWindow.StartTrackingMouse(false);
        }

        private void EndTracking()
        {
            if (_splitterClicked)
            {
                // Clear flag
                _splitterClicked = false;

                // End capturing mouse
                ParentWindow.EndTrackingMouse();
            }
        }

        /// <inheritdoc />
        public override bool HasMouseCapture => _splitterClicked || base.HasMouseCapture;

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;

            // Draw splitter
            Render2D.FillRectangle(_splitterRect, _splitterClicked ? style.BackgroundSelected : _mouseOverSplitter ? style.BackgroundHighlighted : style.Background * 0.8f);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            EndTracking();

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mouseOverSplitter = _splitterRect.Contains(location);

            if (_splitterClicked)
            {
                SplitterValue = location.X / Width;
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                if (_splitterRect.Contains(location))
                {
                    // Start moving splitter
                    StartTracking();
                    return false;
                }
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (_splitterClicked)
            {
                EndTracking();
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _mouseOverSplitter = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnLostMouseCapture()
        {
            EndTracking();
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            base.SetSizeInternal(size);

            // Refresh
            UpdateSplitRect();
            PerformLayout();
        }
        
        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Sort controls from up to down into two columns: one for labels and one for the rest of the stuff

            float y = _topMargin;
            float w = Width - _leftMargin - _rightMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible && !(c is PropertyNameLabel))
                {
                    var h = c.Height;
                    c.Bounds = new Rectangle(_leftMargin, y + _spacing, w, h);
                    y = c.Bottom;
                }
            }
            y += _bottomMargin;

            float namesWidth = _splitterValue * Width;
            int count = _element.Labels.Count;
            float[] yStarts = new float[count + 1];
            for (int i = 1; i < count; i++)
            {
                var label = _element.Labels[i];

                if (label.FirstChildControlIndex < 0)
                    yStarts[i] = 0;
                else if (_children.Count <= label.FirstChildControlIndex)
                    yStarts[i] = y;
                else
                    yStarts[i] = _children[label.FirstChildControlIndex].Top;
            }
            yStarts[count] = y;
            for (int i = 0; i < count; i++)
            {
                var label = _element.Labels[i];

                var rect = new Rectangle(0, yStarts[i] + 1, namesWidth, yStarts[i + 1] - yStarts[i] - 2);
                //label.Parent = this;
                label.Bounds = rect;
            }

            Height = y;
        }
    }
}
