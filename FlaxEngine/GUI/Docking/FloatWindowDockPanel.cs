// Flax Engine scripting API

using System;

namespace FlaxEngine.GUI.Docking
{
    /// <summary>
    /// Floating Window Dock Panel control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Docking.DockPanel" />
    public class FloatWindowDockPanel : DockPanel
    {
        protected MasterDockPanel _masterPanel;
        protected Window _window;

        /// <summary>
        /// Gets the master panel.
        /// </summary>
        /// <value>
        /// The master panel.
        /// </value>
        public MasterDockPanel MasterPanel => _masterPanel;

        /// <summary>
        /// Gets the window.
        /// </summary>
        /// <value>
        /// The window.
        /// </value>
        public Window Window => _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatWindowDockPanel"/> class.
        /// </summary>
        /// <param name="masterPanel">The master panel.</param>
        /// <param name="window">The window.</param>
        public FloatWindowDockPanel(MasterDockPanel masterPanel, Window window)
            : base(null)
        {
            _masterPanel = masterPanel;
            _window = window;

            // Link
            _masterPanel.FloatingPanels.Add(this);
            Parent = window;
            window.NativeWindow.OnClosing += onClosing;
            window.NativeWindow.OnLButtonHit += onLButtonHit;
        }

        /// <summary>
        /// Begin drag operation on the window
        /// </summary>
        public void BeginDrag()
        {
            // Filter invalid events
            if (_window == null)
                return;

            // Check if window is maximized
            if (_window.IsMaximized)
                return;

            // Create docking hint window
            throw new NotImplementedException("Add DockHintWindow");
            //var hintWindow = new DockHintWindow(this);
        }

        /// <summary>
        /// Creates a floating window.
        /// </summary>
        /// <param name="parent">Parent window handle.</param>
        /// <param name="location">Client area location.</param>
        /// <param name="size">Window client area size.</param>
        /// <param name="startPosition">Window start position.</param>
        /// <param name="title">Initial window title.</param>
        internal static Window CreateFloatWindow(Window parent, Vector2 location, Vector2 size, WindowStartPosition startPosition, string title)
        {
            throw new NotImplementedException("Add CreateFloatWindow");
            return null;
            /*// Setup initial window settings
            CreateWindowSettings settings;
            settings.Parent = parent;
            settings.Title = title;
            settings.Size = size;
            settings.Position = location;
            settings.SizeMinWidth = settings.SizeMinHeight = 1;
            settings.SizeMaxWidth = settings.SizeMaxHeight = 2000;
            settings.Fullscreen = false;
            settings.HasBorder = true;
            settings.SupportsTransparency = false;
            settings.ActivateWhenFirstShown = true;
            settings.AllowInput = true;
            settings.AllowMinimize = true;
            settings.AllowMaximize = true;
            settings.AllowDragAndDrop = true;
            settings.IsTopmost = false;
            settings.IsRegularWindow = true;
            settings.HasSizingFrame = true;
            settings.ShowAfterFirstPaint = false;
            settings.ShowInTaskbar = false;
            settings.StartPosition = startPosition;

            // Create window
            return Window::Create(settings);*/
        }

        private bool onLButtonHit(WindowHitCodes hitTest)
        {
            if (hitTest == WindowHitCodes.Caption)
            {
                BeginDrag();
                return true;
            }

            return false;
        }

        private void onClosing(ClosingReason reason, ref bool cancel)
        {
            // Close all docked windows
            while (_tabs.Count > 0)
            {
                if (_tabs[0].Close(reason))
                {
                    // Cancel
                    cancel = true;
                    return;
                }
            }

            // Unlink
            _window.NativeWindow.OnClosing -= onClosing;
            _window.NativeWindow.OnLButtonHit = null;
            _window = null;
        }

        /// <inheritdoc />
        public override bool IsFloating => true;

        /// <inheritdoc />
        public override DockState TryGetDockState(ref float splitterValue)
        {
            return DockState.Float;
        }

        protected override void OnLastTabRemoved()
        {
            // Close window
            if (_window != null)
                _window.Close(ClosingReason.CloseEvent);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _masterPanel?.FloatingPanels.Remove(this);
        
            base.OnDestroy();
        }
    }
}
