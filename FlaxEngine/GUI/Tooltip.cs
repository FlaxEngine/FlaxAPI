// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// The tooltip popup.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    [HideInEditor]
    public class Tooltip : ContainerControl
    {
        private float _timeToPopupLeft;
        private Control _lastTarget;
        private Control _showTarget;
        private string _currentText;
        private Window _window;

        /// <summary>
        /// Gets or sets the time in seconds that mouse have to be over the target to show the tooltip.
        /// </summary>
        public float TimeToShow { get; set; } = 0.3f; // 300ms by default

        /// <summary>
        /// Initializes a new instance of the <see cref="Tooltip"/> class.
        /// </summary>
        public Tooltip()
        : base(0, 0, 300, 24)
        {
            Visible = false;
            CanFocus = false;
        }

        /// <summary>
        /// Shows tooltip over given control.
        /// </summary>
        /// <param name="target">The parent control to attach to it.</param>
        /// <param name="location">Popup menu origin location in parent control coordinates.</param>
        /// <param name="targetArea">Tooltip target area of interest.</param>
        public void Show(Control target, Vector2 location, Rectangle targetArea)
        {
            if (target == null)
                throw new ArgumentNullException();

            // Ensure to be closed
            Hide();

            // Unlock and perform controls update
            UnlockChildrenRecursive();
            PerformLayout();

            // Calculate popup direction and initial location
            var parentWin = target.Root;
            if (parentWin == null)
                return;
            Vector2 locationWS = target.PointToWindow(location);
            Vector2 locationSS = parentWin.ClientToScreen(locationWS);
            Vector2 screenSize = Application.VirtualDesktopSize;
            Vector2 rightBottomLocationSS = locationSS + Size;
            if (screenSize.Y < rightBottomLocationSS.Y)
            {
                // Direction: up
                locationSS.Y -= Height;
            }
            if (screenSize.X < rightBottomLocationSS.X)
            {
                // Direction: left
                locationSS.X -= Width;
            }
            _showTarget = target;

            // Create window
            var desc = CreateWindowSettings.Default;
            desc.StartPosition = WindowStartPosition.Manual;
            desc.Position = locationSS;
            desc.Size = Size;
            desc.Fullscreen = false;
            desc.HasBorder = false;
            desc.SupportsTransparency = false;
            desc.ShowInTaskbar = false;
            desc.ActivateWhenFirstShown = false;
            desc.AllowInput = true;
            desc.AllowMinimize = false;
            desc.AllowMaximize = false;
            desc.AllowDragAndDrop = false;
            desc.IsTopmost = true;
            desc.IsRegularWindow = false;
            desc.HasSizingFrame = false;
            _window = FlaxEngine.Window.Create(desc);
            if (_window == null)
                throw new InvalidOperationException("Failed to create tooltip window.");

            // Attach to the window and focus
            Parent = _window.GUI;
            Visible = true;
            _window.Show();
        }

        /// <summary>
        /// Hides the popup.
        /// </summary>
        public void Hide()
        {
            if (!Visible)
                return;

            // Unlink
            IsLayoutLocked = true;
            Parent = null;

            // Close window
            if (_window)
            {
                var win = _window;
                _window = null;
                win.Close();
            }

            // Hide
            Visible = false;
        }

        /// <summary>
        /// Called when mouse enters a control.
        /// </summary>
        /// <param name="target">The target.</param>
        public void OnMouseEnterControl(Control target)
        {
            _lastTarget = target;
            _timeToPopupLeft = TimeToShow;
        }

        /// <summary>
        /// Called when mouse is over a control.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="dt">The delta time.</param>
        public void OnMouseOverControl(Control target, float dt)
        {
            if (!Visible)
            {
                _lastTarget = target;
                _timeToPopupLeft -= dt;

                if (_timeToPopupLeft <= 0.0f)
                {
                    Vector2 location;
                    Rectangle area;
                    if (_lastTarget.OnShowTooltip(out _currentText, out location, out area))
                    {
                        Show(_lastTarget, location, area);
                    }
                }
            }
        }

        /// <summary>
        /// Called when mouse leaves a control.
        /// </summary>
        /// <param name="target">The target.</param>
        public void OnMouseLeaveControl(Control target)
        {
            _lastTarget = null;
        }

        private void UpdateWindowSize()
        {
            if (_window)
            {
                _window.ClientSize = Size;
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Auto hide if mouse leaves control area
            Vector2 mousePos = Application.MousePosition;
            Vector2 location = _showTarget.ScreenToClient(mousePos);
            if (!_showTarget.OnTestTooltipOverControl(ref location))
            {
                // Mouse left or sth
                Hide();
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;

            // Background
            Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), Color.Lerp(style.BackgroundSelected, style.Background, 0.6f));
            Render2D.FillRectangle(new Rectangle(1.1f, 1.1f, Width - 2, Height - 2), style.Background);

            // Tooltip text
            Render2D.DrawText(style.FontMedium, _currentText, GetClientArea(), Color.White, TextAlignment.Center, TextAlignment.Center);

            base.Draw();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Cache data
            var prevSize = Size;
            var style = Style.Current;

            // Calculate size of the tooltip
            float width = 24.0f;
            if (style.FontMedium)
                width += style.FontMedium.MeasureText(_currentText).X;
            Size = new Vector2(width, 24);

            // Check if is visible size get changed
            if (Visible && prevSize != Size)
            {
                UpdateWindowSize();
            }
        }

        /// <inheritdoc />
        public override bool OnShowTooltip(out string text, out Vector2 location, out Rectangle area)
        {
            base.OnShowTooltip(out text, out location, out area);

            // It's better not to show tooltip for a tooltip.
            // It would be kind of tooltipness.
            return false;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            Hide();

            base.OnDestroy();
        }
    }
}
