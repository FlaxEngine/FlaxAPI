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
            
            // TODO: sync splitter for whole presenter
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

                var style = Style.Current;

                // Draw properties names
                float namesWidth = _splitterValue * Width - SplitterMargin;
                Render2D.PushClip(new Rectangle(0, 0, namesWidth + SplitterMargin, Height));
                int count = _element.Entrie.Count;
                float[] yStarts = new float[count + 1];
                for (int i = 0; i < count; i++)
                {
                    var entry = _element.Entrie[i];
                    yStarts[i] = _children[entry.FirstChildControlIndex].Top;
                }
                yStarts[count] = Height;
                for (int i = 0; i < count; i++)
                {
                    var entry = _element.Entrie[i];

                    var rect = new Rectangle(SplitterMargin, yStarts[i], namesWidth, yStarts[i + 1] - yStarts[i]);
                    Render2D.DrawText(style.FontMedium, entry.Name, rect, style.Foreground, TextAlignment.Near, TextAlignment.Center);
                }
                Render2D.PopClip();

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
        /// The list.
        /// </summary>
        public readonly PropertiesList Properties;

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesListElement"/> class.
        /// </summary>
        public PropertiesListElement()
        {
            Properties = new PropertiesList(this);
        }

        internal struct PropertyEntry
        {
            /// <summary>
            /// Helper value used by the <see cref="PropertiesList"/> to draw property name.
            /// </summary>
            public string Name;

            /// <summary>
            /// Helper value used by the <see cref="PropertiesList"/> to draw property names in a proper area.
            /// </summary>
            public int FirstChildControlIndex;
        }

        internal readonly List<PropertyEntry> Entrie = new List<PropertyEntry>();

        internal void OnAddProperty(string name)
        {
            Entrie.Add(new PropertyEntry
            {
                Name = name,
                FirstChildControlIndex = Children.Count
            });
        }
    }
}
