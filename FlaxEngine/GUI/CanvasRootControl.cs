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
        public override bool ContainsFocus => false; // If canvas is linked to the Window GUI then it steals some events, disable it.

        /// <inheritdoc />
        public override bool IsFocused => false; // If canvas is linked to the Window GUI then it steals some events, disable it.

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
        public override bool OnCharInput(char c)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnCharInput(c);
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            if (!_canvas.ReceivesEvents)
                return DragDropEffect.None;

            return base.OnDragDrop(ref location, data);
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            if (!_canvas.ReceivesEvents)
                return DragDropEffect.None;

            return base.OnDragEnter(ref location, data);
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            if (!_canvas.ReceivesEvents)
                return;

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            if (!_canvas.ReceivesEvents)
                return DragDropEffect.None;

            return base.OnDragMove(ref location, data);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        public override void OnKeyUp(Keys key)
        {
            if (!_canvas.ReceivesEvents)
                return;

            base.OnKeyUp(key);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnMouseDoubleClick(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            if (!_canvas.ReceivesEvents)
                return;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            if (!_canvas.ReceivesEvents)
                return;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            if (!_canvas.ReceivesEvents)
                return;

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            if (!_canvas.ReceivesEvents)
                return false;

            return base.OnMouseWheel(location, delta);
        }

        /// <inheritdoc />
        public override void Focus()
        {
        }

        /// <inheritdoc />
        public override void DoDragDrop(DragData data)
        {
        }
    }
}
