////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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
            /// <summary>
            /// Initializes a new instance of the <see cref="Event"/> class.
            /// </summary>
            /// <param name="name">The event name</param>
            /// <param name="x">The x position.</param>
            /// <param name="width">The width.</param>
            public Event(string name, float x, float width)
                : base(x, 0, width, 24)
            {
                Name = TooltipText = name;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                var bounds = new Rectangle(Vector2.Zero, Size);
                Color color;
                if(ContainsFocus)
                    color = style.BackgroundHighlighted * (IsMouseOver ?  1.5f : 1.0f);
                else
                    color = IsMouseOver ? style.BackgroundNormal : style.LightBackground;
                
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
