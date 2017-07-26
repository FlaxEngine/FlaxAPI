////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using FlaxEngine;
using FlaxEngine.GUI;
using Window = FlaxEngine.GUI.Window;

namespace FlaxEditor.GUI.Dialogs
{
    /// <summary>
    /// Helper class for showing user dialogs.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class Dialog : ContainerControl
    {
        internal static List<Dialog> Dialogs = new List<Dialog>();

        private string _title;
        
        protected long _isWaitingForDialog;
        protected FlaxEngine.Window _window;
        protected DialogResult _result;

        /// <summary>
        /// Gets the dialog result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public DialogResult Result => _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dialog"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        protected Dialog(string title)
            : base(true, new Rectangle(0, 0, 300, 100))
        {
            DockStyle = DockStyle.Fill;
            ClipChildren = false;

            _title = title;
            _result = DialogResult.None;

            Dialogs.Add(this);
        }

        /// <summary>
        /// Shows the dialog and waits for the result.
        /// </summary>
        /// <returns>The dialog result.</returns>
        public DialogResult ShowDialog()
        {
            return ShowDialog(Editor.Instance.Windows.MainWindow);
        }

        /// <summary>
        /// Shows the dialog and waits for the result.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <returns>The dialog result.</returns>
        public DialogResult ShowDialog(Window parentWindow)
        {
            return ShowDialog(parentWindow?.NativeWindow);
        }

        /// <summary>
        /// Shows the dialog and waits for the result.
        /// </summary>
        /// <param name="control">The control calling.</param>
        /// <returns>The dialog result.</returns>
        public DialogResult ShowDialog(Control control)
        {
            return ShowDialog(control?.ParentWindow);
        }

        /// <summary>
        /// Shows the dialog and waits for the result.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <returns>The dialog result.</returns>
        public DialogResult ShowDialog(FlaxEngine.Window parentWindow)
        {
            // Show window
            Show(parentWindow);

            // Set wait flag
            Interlocked.Increment(ref _isWaitingForDialog);

            // Wait for the closing
            do
            {
                Thread.Sleep(1);
            } while (_window);

            // Clear wait flag
            Interlocked.Decrement(ref _isWaitingForDialog);

            return _result;
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void Show()
        {
            Show(Editor.Instance.Windows.MainWindow);
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        public void Show(Window parentWindow)
        {
            Show(parentWindow?.NativeWindow);
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="control">The control calling.</param>
        public void Show(Control control)
        {
            Show(control?.ParentWindow);
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        public void Show(FlaxEngine.Window parentWindow)
        {
            // Setup initial window settings
            CreateWindowSettings settings = CreateWindowSettings.Default;
            settings.Title = _title;
            settings.Size = Size;
            settings.AllowMaximize = false;
            settings.AllowMinimize = false;
            settings.HasSizingFrame = false;
            settings.StartPosition = WindowStartPosition.CenterParent;
            settings.Parent = parentWindow;
            SetupWindowSettings(ref settings);

            // Create window
            _window = FlaxEngine.Window.Create(settings);
            var windowGUI = _window.GUI;

            // Attach events
            _window.OnClosing += OnClosing;
            _window.OnClosed += OnClosed;

            // Link to the window
            Parent = windowGUI;

            // Show
            _window.Show();
            _window.Focus();
            _window.FlashWindow();

            // Perform layout
            windowGUI.UnlockChildrenRecursive();
            windowGUI.PerformLayout();

            OnShow();
        }

        private void OnClosing(ClosingReason reason, ref bool cancel)
        {
            // Check if can close window
            if (CanCloseWindow(reason))
            {
                if (reason == ClosingReason.User)
                    _result = DialogResult.Cancel;

                // Clean up
                _window = null;

                // Check if any thead is blocked during ShowDialog, then wait for it
                bool wait = true;
                while (wait)
                {
                    wait = Interlocked.Read(ref _isWaitingForDialog) > 0;
                    Thread.Sleep(1);
                }

                // Close window
                return;
            }

            // Supress closing
            cancel = true;
        }

        private void OnClosed()
        {
            _window = null;
        }

        /// <summary>
        /// Closes this dialog.
        /// </summary>
        public void Close()
        {
            if (_window != null)
            {
                var win = _window;
                _window = null;
                win.Close();
            }
        }

        /// <summary>
        /// Closes dialog with the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        protected void Close(DialogResult result)
        {
            _result = result;
            Close();
        }

        /// <summary>
        /// Setups the window settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        protected virtual void SetupWindowSettings(ref CreateWindowSettings settings)
        {
        }

        /// <summary>
        /// Called when dialogs popups.
        /// </summary>
        protected virtual void OnShow()
        {
        }
        
        /// <summary>
        /// Determines whether this dialog can be closed.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns>
        ///   <c>true</c> if this dialog can be closed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanCloseWindow(ClosingReason reason)
        {
            return true;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Dialogs.Remove(this);

            base.OnDestroy();
        }
    }
}
