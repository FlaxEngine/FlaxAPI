////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// <see cref="CustomEditor"/> properties list element.
    /// </summary>
    /// <seealso cref="LayoutElementsContainer"/>
    public class PropertiesListElement : LayoutElementsContainer
    {
        /// <summary>
        /// <see cref="CustomEditor"/> properties list control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.VerticalPanel" />
        public class PropertiesList : VerticalPanel
        {
            /// <summary>
            /// The spliter size (in pixels).
            /// </summary>
            public const int SpliterSize = 2;

            /// <summary>
            /// The splitter margin (in pixels).
            /// </summary>
            public const int SplitterMargin = 4;

            private const int SpliterSizeHalf = SpliterSize / 2;

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
                    value = Mathf.Clamp01(value);
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
            public PropertiesList()
            {
                _splitterValue = 0.4f;
                UpdateSplitRect();
            }

            private void UpdateSplitRect()
            {
                _splitterRect = new Rectangle(Mathf.Clamp(_splitterValue * Width - SpliterSizeHalf, 0.0f, Width), 0, SpliterSize, Height);
                LeftMargin = _splitterValue * Width + SplitterMargin;
            }
            
            // TODO: sync splitter for whole presenter
            // TODO: draw editors names
            // TODO: show editor tooltips
            // TODO: if name is too long to show in leftMargin space -> use tooltip to show it

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

                // Draw splitter
                var style = Style.Current;
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
            public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
            {
                if (buttons == MouseButtons.Left)
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
            public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
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
        }

        /// <summary>
        /// The child editors added to this elements container.
        /// </summary>
        protected readonly List<CustomEditor> _editors = new List<CustomEditor>();

        /// <summary>
        /// The list.
        /// </summary>
        public readonly PropertiesList Properties = new PropertiesList();

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Properties;

        /// <inheritdoc />
        protected override void OnAddEditor(CustomEditor editor)
        {
            base.OnAddEditor(editor);

            _editors.Add(editor);
        }
    }
}
