// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Root control implementation used by the <see cref="UICanvas"/> actor.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.RootControl" />
    [HideInEditor]
    public sealed class CanvasRootControl : RootControl
    {
        private UICanvas _canvas;

        /// <summary>
        /// Gets the owning canvas.
        /// </summary>
        public UICanvas Canvas => _canvas;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRootControl"/> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal CanvasRootControl(UICanvas canvas)
        {
            _canvas = canvas;
        }

        /// <inheritdoc />
        public override CursorType Cursor
        {
            get => CursorType.Default;
            set { }
        }

        /// <inheritdoc />
        public override Vector2 TrackingMouseOffset => Vector2.Zero;

        /// <inheritdoc />
        public override Vector2 MousePosition
        {
            get => Vector2.Zero;
            set { }
        }

        /// <inheritdoc />
        public override bool GetKey(Keys key)
        {
            return Input.GetKey(key);
        }

        /// <inheritdoc />
        public override bool GetKeyDown(Keys key)
        {
            return Input.GetKeyDown(key);
        }

        /// <inheritdoc />
        public override bool GetKeyUp(Keys key)
        {
            return Input.GetKeyUp(key);
        }

        /// <inheritdoc />
        public override bool GetMouseButton(MouseButton button)
        {
            return Input.GetMouseButton(button);
        }

        /// <inheritdoc />
        public override bool GetMouseButtonDown(MouseButton button)
        {
            return Input.GetMouseButtonDown(button);
        }

        /// <inheritdoc />
        public override bool GetMouseButtonUp(MouseButton button)
        {
            return Input.GetMouseButtonUp(button);
        }

        /// <inheritdoc />
        public override Vector2 ScreenToClient(Vector2 location)
        {
            return location;
        }

        /// <inheritdoc />
        public override Vector2 ClientToScreen(Vector2 location)
        {
            return location;
        }

        /// <inheritdoc />
        public override void Focus()
        {
        }

        /// <inheritdoc />
        public override void DoDragDrop(DragData data)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), Color.Red);

            base.Draw();
        }
    }
}
