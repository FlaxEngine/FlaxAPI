////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.Assertions;

namespace FlaxEngine.GUI.Docking
{
    /// <summary>
    /// Dockable window UI control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Panel" />
    public class DockWindow : Panel
    {
        private string _title;
        private Vector2 _titleSize;

        protected MasterDockPanel _masterPanel;
        protected DockPanel _dockedTo;

        /// <summary>
        /// Gets or sets a value indicating whether hide window on close.
        /// </summary>
        public bool HideOnClose { get; protected set; }

        /// <summary>
        /// Gets the master panel.
        /// </summary>
        public MasterDockPanel MasterPanel => _masterPanel;

        /// <summary>
        /// Gets the parent dock panel.
        /// </summary>
        public DockPanel ParentDockPanel
        {
            get => _dockedTo;
            internal set { _dockedTo = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this window is docked.
        /// </summary>
        public bool IsDocked => _dockedTo != null;

        /// <summary>
        /// Gets a value indicating whether this window is selected.
        /// </summary>
        public bool IsSelected => _dockedTo?.SelectedTab == this;

        /// <summary>
        /// Gets the default window size.
        /// </summary>
        public virtual Vector2 DefaultSize => new Vector2(900, 580);

        /// <summary>
        /// Gets the serialization typename.
        /// </summary>
        public virtual string SerializationTypename => "::" + GetType().FullName;

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title
        {
            get => _title;
            protected set
            {
                _title = value;

                // Invalidate cached title size
                _titleSize = new Vector2(-1);
                PerformLayoutSelf();

                // Check if is docked to the floating window and is selected so update window title
                if (IsSelected && _dockedTo is FloatWindowDockPanel floatPanel)
                {
                    floatPanel.Window.Title = Title;
                }
            }
        }

        /// <summary>
        /// Gets the size of the title.
        /// </summary>
        public Vector2 TitleSize => _titleSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockWindow"/> class.
        /// </summary>
        /// <param name="masterPanel">The master docking panel.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        public DockWindow(MasterDockPanel masterPanel, bool hideOnClose, ScrollBars scrollBars)
            : base(scrollBars, true)
        {
            _masterPanel = masterPanel;
            HideOnClose = hideOnClose;
            DockStyle = DockStyle.Fill;
        }

        /// <summary>
        /// Shows the window in a floating state.
        /// </summary>
        /// <param name="position">Window location.</param>
        public void ShowFloating(WindowStartPosition position = WindowStartPosition.CenterParent)
        {
            ShowFloating(Vector2.Zero, position);
        }

        /// <summary>
        /// Shows the window in a floating state.
        /// </summary>
        /// <param name="size">Window size, set Vector2.Zero to use default.</param>
        /// <param name="position">Window location.</param>
        public void ShowFloating(Vector2 size, WindowStartPosition position = WindowStartPosition.CenterParent)
        {
            // Undock
            Undock();

            // Create window
            var winSize = size.LengthSquared > 4 ? size : DefaultSize;
            var window = FloatWindowDockPanel.CreateFloatWindow(_masterPanel.ParentWindow, new Vector2(200, 200), winSize, position, _title);
            var windowGUI = window.GUI;

            // Create dock panel for the window
            var dockPanel = new FloatWindowDockPanel(_masterPanel, windowGUI);
            dockPanel.DockWindowInternal(DockState.DockFill, this);

            Visible = true;

            // Perform layout
            windowGUI.UnlockChildrenRecursive();
            windowGUI.PerformLayout();

            // Show
            window.Show();
            window.BringToFront();
            window.Focus();
            OnShow();

            // Perform layout again
            windowGUI.PerformLayout();
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="state">Initial window state.</param>
        /// <param name="toDock">Panel to dock to it.</param>
        public void Show(DockState state = DockState.Float, DockPanel toDock = null)
        {
            if (state == DockState.Hidden)
            {
                Hide();
            }
            else if (state == DockState.Float)
            {
                ShowFloating();
            }
            else
            {
                Visible = true;

                // Undock first
                Undock();

                // Then dock
                (toDock ?? _masterPanel).DockWindowInternal(state, this);
                OnShow();
                PerformLayout();
            }
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="state">Initial window state.</param>
        /// <param name="toDock">Window to dock to it.</param>
        public void Show(DockState state, DockWindow toDock)
        {
            Show(state, toDock?.ParentDockPanel);
        }

        /// <summary>
        /// Focuses or shows the window.
        /// </summary>
        /// <param name="state">The state.</param>
        public void FocusOrShow(DockState state = DockState.Float)
        {
            if (Visible)
            {
                SelectTab();
                Focus();
            }
            else
            {
                Show(state);
            }
        }
        
        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            // Undock
            Undock();

            Visible = false;

            // Ensure that dock panel has no parent
            Assert.IsFalse(HasParent);
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="reason">Window closing reason.</param>
        /// <returns>True if action has been cancelled (due to window internal logic).</returns>
        public bool Close(ClosingReason reason = ClosingReason.CloseEvent)
        {
            // Fire events
            if (OnClosing(reason))
                return true;
            OnClose();

            // Check if window should be hidden on close event
            if (HideOnClose)
            {
                // Hide
                Hide();
            }
            else
            {
                // Undock
                Undock();

                // Delete itself
                Dispose();
            }

            // Done
            return false;
        }

        /// <summary>
        /// Selects this tab page.
        /// </summary>
        public void SelectTab()
        {
            _dockedTo?.SelectTab(this);
        }

        internal void OnUnlinkInternal()
        {
            OnUnlink();
        }

        /// <summary>
        /// Called when window is unlinked from the master panel.
        /// </summary>
        protected virtual void OnUnlink()
        {
            _masterPanel = null;
        }

        /// <summary>
        /// Undocks this window.
        /// </summary>
        protected virtual void Undock()
        {
            // Defocus itself
            if (ContainsFocus)
                Focus();
            Defocus();

            // Call undock
            if (_dockedTo != null)
            {
                _dockedTo.UndockWindowInternal(this);
                Assert.IsNull(_dockedTo);
            }
        }

        /// <summary>
        /// Called when window is closing. Operation can be cancelled.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns>True if cancel, otherise false to allow.</returns>
        protected virtual bool OnClosing(ClosingReason reason)
        {
            // Allow
            return false;
        }

        /// <summary>
        /// Called when window is closed.
        /// </summary>
        protected virtual void OnClose()
        {
            // Nothing to do
        }

        /// <summary>
        /// Called when window shows.
        /// </summary>
        protected virtual void OnShow()
        {
            // Nothing to do
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Undock();
            
            // Unlink from the master panel
            _masterPanel?.unlinkWindow(this);

            // Ensure that dock panel has no parent
            Assert.IsFalse(HasParent);

            base.OnDestroy();
        }

        /// <inheritdoc />
        public override void Focus()
        {
            base.Focus();

            SelectTab();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Cache window title dimensions
            if (_titleSize.X <= 0)
            {
                var style = Style.Current;
                if (style?.FontMedium != null)
                    _titleSize = style.FontMedium.MeasureText(_title);
            }

            base.PerformLayoutSelf();
        }
    }
}
