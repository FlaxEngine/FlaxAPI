////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Events timeline control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Panel" />
    public class Timeline : Panel
    {
        /// <summary>
        /// Single timeline event control.
        /// </summary>
        /// <seealso cref="ContainerControl" />
        public class Event : ContainerControl
        {
            private static readonly Color[] Colors =
            {
                new Color(0.8f, 0.894117653f, 0.709803939f, 1f),
                new Color(0.1254902f, 0.698039234f, 0.6666667f, 1f),
                new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f),
                new Color(0.3827448f, 0.2886272f, 0.5239216f, 1f),
                new Color(0.8f, 0.4423528f, 0f, 1f),
                new Color(0.4486272f, 0.4078432f, 0.050196f, 1f),
                new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f),
                new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f),
                new Color(0.2070592f, 0.5333336f, 0.6556864f, 1f),
                new Color(0.8f, 0.4423528f, 0f, 1f),
                new Color(0.4486272f, 0.4078432f, 0.050196f, 1f),
                new Color(0.7749016f, 0.6368624f, 0.0250984f, 1f),
                new Color(0.5333336f, 0.16f, 0.0282352f, 1f),
                new Color(0.3827448f, 0.2886272f, 0.5239216f, 1f),
                new Color(0.478431374f, 0.482352942f, 0.117647059f, 1f),
                new Color(0.9411765f, 0.5019608f, 0.5019608f, 1f),
                new Color(0.6627451f, 0.6627451f, 0.6627451f, 1f),
                new Color(0.545098066f, 0f, 0.545098066f, 1f),
            };

            private Color _color;

            /// <summary>
            /// Initializes a new instance of the <see cref="Event"/> class.
            /// </summary>
            /// <param name="x">The x position.</param>
            /// <param name="width">The width.</param>
            public Event(float x, float width)
                : base(x, 0, width, 24)
            {
            }

            /// <inheritdoc />
            protected override void OnParentChangedInternal()
            {
                base.OnParentChangedInternal();

                int key = IndexInParent * (string.IsNullOrEmpty(Name) ? 1 : Name[0]);
                _color = Colors[key % Colors.Length];
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                var bounds = new Rectangle(Vector2.Zero, Size);
                Color color = _color;
                if (IsMouseOver)
                    color *= 1.1f;

                Render2D.FillRectangle(bounds, color);
                Render2D.PushClip(bounds);
                Render2D.DrawText(style.FontMedium, Name, bounds, Color.White, TextAlignment.Center, TextAlignment.Center);
                Render2D.PopClip();
            }
        }

        /// <summary>
        /// Gets the events container control. Use it to remvoe/add events to the timeline.
        /// </summary>
        public ContainerControl EventsContainer => this;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timeline"/> class.
        /// </summary>
        public Timeline()
            : base(ScrollBars.Both)
        {
        }
    }
}
