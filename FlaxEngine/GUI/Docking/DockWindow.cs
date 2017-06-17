// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <value>
        ///   <c>true</c> if hide window on close; otherwise, <c>false</c>.
        /// </value>
        public bool HideOnClose { get; protected set; }

        /// <summary>
        /// Gets the master panel.
        /// </summary>
        /// <value>
        /// The master panel.
        /// </value>
        public MasterDockPanel MasterPanel => _masterPanel;

        /// <summary>
        /// Gets the parent dock panel.
        /// </summary>
        /// <value>
        /// The parent dock panel.
        /// </value>
        public DockPanel ParentDockPanel
        {
            get => _dockedTo;
            internal set { _dockedTo = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this window is docked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is docked; otherwise, <c>false</c>.
        /// </value>
        public bool IsDocked => _dockedTo != null;

        /// <summary>
        /// Gets a value indicating whether this window is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected => _dockedTo?.SelectedTab == this;

        /// <summary>
        /// Gets the default window size.
        /// </summary>
        /// <value>
        /// The default window size.
        /// </value>
        public virtual Vector2 DefaultSize => new Vector2(900, 580);

        /// <summary>
        /// Gets the serialization typename.
        /// </summary>
        /// <value>
        /// The serialization typename.
        /// </value>
        public virtual string SerializationTypename => "::" + GetType().FullName;

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        /// <value>
        /// The window title.
        /// </value>
        public string Title
        {
            get => _title;
            protected set
            {
                _title = value;

                // Invalidate cached title size
                _titleSize = new Vector2(-1);

                // Check if is docked to the floating window
                if (_dockedTo is FloatWindowDockPanel floatPanel)
                {
                    // Check if is the first window in the floating panel
                    if (floatPanel.GetTab(0) == this)
                    {
                        // Update window title
                        floatPanel.Window.Title = Title;
                    }

                    // Update tabs
                    floatPanel.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DockWindow"/> class.
        /// </summary>
        /// <param name="masterPanel">The master docking panel.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        public DockWindow(MasterDockPanel masterPanel, bool hideOnClose, ScrollBars scrollBars)
            : base(scrollBars)
        {
            _masterPanel = masterPanel;
            HideOnClose = hideOnClose;
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
            
            // Create dock panel for the window
            var dockPanel = new FloatWindowDockPanel(_masterPanel, window);
            dockPanel.DockWindowInternal(DockState.DockFill, this);

            Visible = true;
            
            // Perform layout
            window.UnlockChildrenRecursive();
            window.PerformLayout();

            // Show
            window.Show();
            window.Focus();
            OnShow();

            // Perform layout again
            window.PerformLayout();
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

        protected virtual void OnUnlink()
        {
            _masterPanel = null;
        }

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

        protected virtual bool OnClosing(ClosingReason reason)
        {
            // Unlink window
            //_window = nullptr;

            //if (reason == ClosingReason::EngineExit)
            //	return true;

            // Check if unlink from window and prevent destruction
            /*if(HideOnClose || (reason != ClosingReason::User && reason != ClosingReason::CloseEvent))
            {
                SetParent(nullptr);
            }*/

            // Allow
            return false;
        }

        protected virtual void OnClose()
        {
            // Nothing to do
        }

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
            if (_titleSize.X < 0)
            {
                var style = Style.Current;
                if (style?.FontMedium != null)
                    _titleSize = style.FontMedium.MeasureText(_title);
            }

            base.PerformLayoutSelf();
        }
    }
}
