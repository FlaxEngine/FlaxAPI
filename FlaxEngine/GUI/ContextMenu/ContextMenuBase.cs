////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context menu popup directions.
    /// </summary>
    public enum ContextMenuDirection
    {
        RightDown,
        RightUp,
        LeftDown,
        LeftUp,
    }

    /// <summary>
    /// Base class for all context menu controls.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControlChildrenSized" />
    public class ContextMenuBase : ContainerControlChildrenSized
    {
        public const int DefaultItemsMargin = 2;
        public const int DefaultItemsLeftMargin = 16;

        private ContextMenuDirection _direction;
        private ContextMenuBase _parentCM;
        private ContextMenuBase _childCM;
        private Window _window;

        /// <summary>
        /// Returns true if context menu is opened
        /// </summary>
        /// <returns>True if opened, otherwise false</returns>
        public bool IsOpened => Parent != null;

        /// <summary>
        /// Gets the pipup direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public ContextMenuDirection Direction => _direction;

        /// <summary>
        /// Gets a value indicating whether any child context menu has been opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if any child context menu has been opened; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildCMOpened => _childCM != null;

        /// <summary>
        /// Gets the topmost context menu.
        /// </summary>
        /// <value>
        /// The topmost context menu.
        /// </value>
        public ContextMenuBase TopmostCM => _parentCM != null ? _parentCM.TopmostCM : this;

        /// <summary>
        /// Event fired when context menu visiblity changes (window gets shown or hidden.
        /// </summary>
        public event Action<ContextMenuBase> OnVisibilityChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuBase"/> class.
        /// </summary>
        public ContextMenuBase()
            : base(true, 0, 0, 120, 32)
        {
            _direction = ContextMenuDirection.RightDown;
            Visible = false;
        }

        /// <summary>
        /// Show context menu over given control.
        /// </summary>
        /// <param name="parent">Parent control to attach to it.</param>
        /// <param name="location">Popup menu orgin location in parent control coordinates.</param>
        public virtual void Show(Control parent, Vector2 location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hide popup menu and all child menus.
        /// </summary>
        public virtual void Hide()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows new child context menu.
        /// </summary>
        /// <param name="child">The child menu.</param>
        /// <param name="location">The child menu initial location.</param>
        public void ShowChild(ContextMenuBase child, Vector2 location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hides child popup menu if any opened.
        /// </summary>
        public void HideChild()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the size of the window to match context menu dimensions.
        /// </summary>
        protected void UpdateWindowSize()
        {
            throw new NotImplementedException();
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
    }
}
