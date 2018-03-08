////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.Assertions;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context menu popup directions.
    /// </summary>
    public enum ContextMenuDirection
    {
        /// <summary>
        /// The right down.
        /// </summary>
        RightDown,

        /// <summary>
        /// The right up.
        /// </summary>
        RightUp,

        /// <summary>
        /// The left down.
        /// </summary>
        LeftDown,

        /// <summary>
        /// The left up.
        /// </summary>
        LeftUp,
    }

    /// <summary>
    /// Base class for all context menu controls.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class ContextMenuBase : ContainerControl
    {
        private ContextMenuDirection _direction;
        private ContextMenuBase _parentCM;
        private ContextMenuBase _childCM;
        private FlaxEngine.Window _window;

        /// <summary>
        /// Returns true if context menu is opened
        /// </summary>
        /// <returns>True if opened, otherwise false</returns>
        public bool IsOpened => Parent != null;

        /// <summary>
        /// Gets the popup direction.
        /// </summary>
        public ContextMenuDirection Direction => _direction;

        /// <summary>
        /// Gets a value indicating whether any child context menu has been opened.
        /// </summary>
        public bool HasChildCMOpened => _childCM != null;

        /// <summary>
        /// Gets the topmost context menu.
        /// </summary>
        public ContextMenuBase TopmostCM => _parentCM != null ? _parentCM.TopmostCM : this;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuBase"/> class.
        /// </summary>
        public ContextMenuBase()
            : base(0, 0, 120, 32)
        {
            _direction = ContextMenuDirection.RightDown;
            Visible = false;

            _performChildrenLayoutFirst = true;
        }

        /// <summary>
        /// Show context menu over given control.
        /// </summary>
        /// <param name="parent">Parent control to attach to it.</param>
        /// <param name="location">Popup menu orgin location in parent control coordinates.</param>
        public virtual void Show(Control parent, Vector2 location)
        {
            Assert.IsNotNull(parent);

            // Ensure to be closed
            Hide();

            // Unlock and perform controls update
            UnlockChildrenRecursive();
            PerformLayout();

            // Calculate popup directinon and initial location (fit on a single monitor)
            var parentWin = parent.ParentWindow;
            if (parentWin == null)
                return;
            Vector2 locationWS = parent.PointToWindow(location);
            Vector2 locationSS = parentWin.ClientToScreen(locationWS);
            Location = Vector2.Zero;
	        Rectangle monitorBounds = Application.GetMonitorBounds(locationSS);
            Vector2 rightBottomLocationSS = locationSS + Size;
	        bool isUp = false, isLeft = false;
            if (monitorBounds.Bottom < rightBottomLocationSS.Y)
            {
				// Direction: up
	            isUp = true;
				locationSS.Y -= Height;
            }
            if (monitorBounds.Right < rightBottomLocationSS.X)
            {
                // Direction: left
	            isLeft = true;
                locationSS.X -= Width;
            }

			// Update direction flag
	        if (isUp)
		        _direction = isLeft ? ContextMenuDirection.LeftUp : ContextMenuDirection.RightUp;
	        else
		        _direction = isLeft ? ContextMenuDirection.LeftDown : ContextMenuDirection.RightDown;

			// Create window
			var desc = CreateWindowSettings.Default;
            desc.Position = locationSS;
            desc.StartPosition = WindowStartPosition.Manual;
            desc.Size = Size;
            desc.Fullscreen = false;
            desc.HasBorder = false;
            desc.SupportsTransparency = false;
            desc.ShowInTaskbar = false;
            desc.ActivateWhenFirstShown = true;
            desc.AllowInput = true;
            desc.AllowMinimize = false;
            desc.AllowMaximize = false;
            desc.AllowDragAndDrop = false;
            desc.IsTopmost = true;
            desc.IsRegularWindow = false;
            desc.HasSizingFrame = false;
            _window = FlaxEngine.Window.Create(desc);
            _window.OnLostFocus += onWindowLostFocus;

            // Attach to the window
            _parentCM = parent as ContextMenuBase;
            Parent = _window.GUI;

            // Show
            Visible = true;
            _window.Show();
            PerformLayout();
            Focus();
            OnShow();
        }

        /// <summary>
        /// Hide popup menu and all child menus.
        /// </summary>
        public virtual void Hide()
        {
            if (!Visible)
                return;

            // Lock update
            IsLayoutLocked = true;

            // Close child
            HideChild();

            // Unlink
            _parentCM = null;
            Parent = null;

            // Close window
            if (_window != null)
            {
                var win = _window;
                _window = null;
                win.Close();
            }

            // Hide
            Visible = false;
            OnHide();
        }

        /// <summary>
        /// Shows new child context menu.
        /// </summary>
        /// <param name="child">The child menu.</param>
        /// <param name="location">The child menu initial location.</param>
        public void ShowChild(ContextMenuBase child, Vector2 location)
        {
            // Hide current child
            HideChild();

            // Set child
            _childCM = child;
            _childCM._parentCM = this;

            // Show child
            _childCM.Show(this, location);
        }

        /// <summary>
        /// Hides child popup menu if any opened.
        /// </summary>
        public void HideChild()
        {
            if (_childCM != null)
            {
                _childCM.Hide();
                _childCM = null;
            }
        }

        /// <summary>
        /// Updates the size of the window to match context menu dimensions.
        /// </summary>
        protected void UpdateWindowSize()
        {
            if (_window != null)
            {
                _window.ClientSize = Size;
            }
        }

        /// <summary>
        /// Called on context menu show.
        /// </summary>
        protected virtual void OnShow()
        {
            // Nothing to do
        }

        /// <summary>
        /// Called on context menu hide.
        /// </summary>
        protected virtual void OnHide()
        {
            // Nothing to do
        }

        private void onWindowLostFocus()
        {
            // Skip for parent menus (child should hanndle lost of focus)
            if (_childCM != null)
                return;

            // Check if user stopped using that popup menu
            var root = TopmostCM;
            if (_parentCM != null)
            {
                root.Hide();
            }
            else if (!HasChildCMOpened)
            {
                Hide();
            }
        }

        /// <inheritdoc />
        public override bool IsMouseOver
        {
            get
            {
                bool result = false;
                for (int i = 0; i < _children.Count; i++)
                {
                    var c = _children[i];
                    if (c.Visible && c.IsMouseOver)
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Draw background
            var style = Style.Current;
            Render2D.FillRectangle(new Rectangle(0, 0, Width, Height), style.Background);
            Render2D.DrawRectangle(new Rectangle(0, 0, Width - 1f, Height - 1f), Color.LerpUnclamped(style.BackgroundSelected, style.Background, 0.6f));

            base.Draw();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            base.OnMouseDown(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            base.OnMouseUp(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to be hidden
            Hide();

            base.OnDestroy();
        }
    }
}
