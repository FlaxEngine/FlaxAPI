////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Bool value element.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.SurfaceNodeElementControl" />
    public sealed class BoolValue : SurfaceNodeElementControl
    {
        private bool _mouseDown;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public bool Value
        {
            get => (bool)ParentNode.Values[Archetype.ValueIndex];
            set
            {
                ParentNode.Values[Archetype.ValueIndex] = value;
                ParentNode.Surface.MarkAsEdited();
            }
        }

        /// <summary>
        /// Toggle sate
        /// </summary>
        public void Toggle()
        {
            Value = !Value;
        }

        /// <inheritdoc />
        public BoolValue(SurfaceNode parentNode, NodeElementArchetype archetype)
            : base(parentNode, archetype, 16, 16, true)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            var box = new Rectangle(Vector2.Zero, Size);
            bool value = Value;

            // Background
            Color backgroundColor;
            if (IsMouseOver)
                backgroundColor = style.TextBoxBackgroundSelected;
            else
                backgroundColor = style.TextBoxBackground;
            Render2D.FillRectangle(box, backgroundColor, true);

            // Border
            Color borderColor = style.BorderNormal;
            if (!Enabled)
                borderColor *= 0.5f;
            else if (_mouseDown)
                borderColor = style.BorderSelected;
            Render2D.DrawRectangle(box, borderColor);

            // Icon
            if (value)
                Render2D.DrawSprite(style.CheckBoxTick, box, Enabled ? style.BorderSelected * 1.2f : style.ForegroundDisabled);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            // Check mouse down
            if (buttons == MouseButton.Left)
            {
                // Set flag
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (_mouseDown && buttons == MouseButton.Left)
            {
                _mouseDown = false;
                Toggle();
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clar flag
            _mouseDown = false;

            base.OnMouseLeave();
        }
    }
}
