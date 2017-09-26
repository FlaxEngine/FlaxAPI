////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI.Docking
{
    /// <summary>
    /// Proxy control used for docking <see cref="DockWindow"/> inside <see cref="DockPanel"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class DockPanelProxy : ContainerControl
    {
        private DockPanel _panel;
        public bool IsMouseDown;
        public bool IsMouseDownOverCross;
        public DockWindow MouseDownWindow;
        public Vector2 MousePosition;
        public DockWindow StartDragAsyncWindow;

        private Rectangle HeaderRectangle => new Rectangle(0, 0, Width, DockPanel.DefaultHeaderHeight);

        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanelProxy"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        internal DockPanelProxy(DockPanel panel)
            : base(0, 0, 64, 64)
        {
            CanFocus = false;

            _panel = panel;
            DockStyle = DockStyle.Fill;
        }

        private DockWindow getTabAtPos(Vector2 position, out bool closeButton)
        {
            DockWindow result = null;
            closeButton = false;

            var tabsCount = _panel.TabsCount;
            if (tabsCount == 1)
            {
                var crossRect = new Rectangle(Width - DockPanel.DefaultButtonsSize - DockPanel.DefaultButtonsMargin, (DockPanel.DefaultHeaderHeight - DockPanel.DefaultButtonsSize) / 2, DockPanel.DefaultButtonsSize, DockPanel.DefaultButtonsSize);
                if (HeaderRectangle.Contains(position))
                {
                    closeButton = crossRect.Contains(position);
                    result = _panel.GetTab(0);
                }
            }
            else
            {
                float x = 0;
                for (int i = 0; i < tabsCount; i++)
                {
                    var tab = _panel.GetTab(i);
                    var titleSize = tab.TitleSize;
                    float width = titleSize.X + DockPanel.DefaultButtonsSize + 2 * DockPanel.DefaultButtonsMargin + DockPanel.DefaultLeftTextMargin + DockPanel.DefaultRightTextMargin;
                    var tabRect = new Rectangle(x, 0, width, DockPanel.DefaultHeaderHeight);
                    bool isMouseOver = tabRect.Contains(position);
                    if (isMouseOver)
                    {
                        var crossRect = new Rectangle(x + width - DockPanel.DefaultButtonsSize - DockPanel.DefaultButtonsMargin, (DockPanel.DefaultHeaderHeight - DockPanel.DefaultButtonsSize) / 2, DockPanel.DefaultButtonsSize, DockPanel.DefaultButtonsSize);
                        closeButton = crossRect.Contains(position);
                        result = tab;
                        break;
                    }
                    x += width;
                }
            }

            return result;
        }

        private void getTabRect(DockWindow win, out Rectangle bounds)
        {
            Assertions.Assert.IsTrue(_panel.ContainsTab(win));

            var tabsCount = _panel.TabsCount;
            if (tabsCount == 1)
            {
                bounds = HeaderRectangle;
                return;
            }
            else
            {
                float x = 0;
                for (int i = 0; i < tabsCount; i++)
                {
                    var tab = _panel.GetTab(i);
                    var titleSize = tab.TitleSize;
                    float width = titleSize.X + DockPanel.DefaultButtonsSize + 2 * DockPanel.DefaultButtonsMargin + DockPanel.DefaultLeftTextMargin + DockPanel.DefaultRightTextMargin;
                    if (tab == win)
                    {
                        bounds = new Rectangle(x, 0, width, DockPanel.DefaultHeaderHeight);
                        return;
                    }
                    x += width;
                }
            }

            bounds = Rectangle.Empty;
        }

        private void startDrag(DockWindow win)
        {
            // Clear cache
            MouseDownWindow = null;
            StartDragAsyncWindow = win;

            // Register for late update in an async manner (to prevent from crash due to changing UI structure on window undock)
            Scripting.OnLateUpdate += startDragAsync;
        }

        private void startDragAsync()
        {
            Scripting.OnLateUpdate -= startDragAsync;

            if (StartDragAsyncWindow != null)
            {
                var win = StartDragAsyncWindow;
                StartDragAsyncWindow = null;

                // Check if has only one window docked and is floating
                if (_panel.ChildPanelsCount == 0 && _panel.TabsCount == 1 && _panel.IsFloating)
                {
                    // Create docking hint window but in an async manner
                    DockHintWindow.Create(_panel as FloatWindowDockPanel);
                }
                else
                {
                    // Select another tab
                    int index = _panel.GetTabIndex(win);
                    if (index == 0)
                        index = _panel.TabsCount;
                    _panel.SelectTab(index - 1);

                    // Create docking hint window
                    DockHintWindow.Create(win);
                }
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            var window = ParentWindow;
            bool containsFocus = ContainsFocus && window.NativeWindow.IsFocused;
            var headerRect = HeaderRectangle;
            var tabsCount = _panel.TabsCount;

            // Check if has only one window docked
            if (tabsCount == 1)
            {
                var tab = _panel.GetTab(0);

                // Draw header
                bool isMouseOver = IsMouseOver && headerRect.Contains(MousePosition);
                Render2D.FillRectangle(headerRect, containsFocus ? style.BackgroundSelected : isMouseOver ? style.BackgroundHighlighted : style.LightBackground);

                // Draw text
                Render2D.DrawText(
                    style.FontMedium,
                    tab.Title,
                    new Rectangle(DockPanel.DefaultLeftTextMargin, 0, Width - DockPanel.DefaultLeftTextMargin - DockPanel.DefaultButtonsSize - 2 * DockPanel.DefaultButtonsMargin, DockPanel.DefaultHeaderHeight),
                    style.Foreground,
                    TextAlignment.Near,
                    TextAlignment.Center);

                // Draw cross
                var crossRect = new Rectangle(Width - DockPanel.DefaultButtonsSize - DockPanel.DefaultButtonsMargin, (DockPanel.DefaultHeaderHeight - DockPanel.DefaultButtonsSize) / 2, DockPanel.DefaultButtonsSize, DockPanel.DefaultButtonsSize);
                bool isMouseOverCross = isMouseOver && crossRect.Contains(MousePosition);
                if (isMouseOverCross)
                    Render2D.FillRectangle(crossRect, (containsFocus ? style.BackgroundSelected : style.LightBackground) * 1.3f);
                Render2D.DrawSprite(style.Cross, crossRect, isMouseOverCross ? Color.White : new Color(0.8f));
            }
            else
            {
                // Draw background
                Render2D.FillRectangle(headerRect, style.LightBackground);

                // Render all tabs
                float x = 0;
                for (int i = 0; i < tabsCount; i++)
                {
                    // Cache data
                    var tab = _panel.GetTab(i);
                    Color tabColor = Color.Black;
                    var titleSize = tab.TitleSize;
                    float width = titleSize.X + DockPanel.DefaultButtonsSize + 2 * DockPanel.DefaultButtonsMargin + DockPanel.DefaultLeftTextMargin + DockPanel.DefaultRightTextMargin;
                    var tabRect = new Rectangle(x, 0, width, DockPanel.DefaultHeaderHeight);
                    bool isMouseOver = IsMouseOver && tabRect.Contains(MousePosition);
                    bool isSelected = _panel.SelectedTab == tab;

                    // Check if tab is selected
                    if (isSelected)
                    {
                        tabColor = containsFocus ? style.BackgroundSelected : style.BackgroundNormal;
                        Render2D.FillRectangle(tabRect, tabColor);
                        Render2D.FillRectangle(new Rectangle(0, DockPanel.DefaultHeaderHeight, Width, 2), tabColor);
                    }
                    // Check if mosue is over
                    else if (isMouseOver)
                    {
                        tabColor = style.BackgroundHighlighted;
                        Render2D.FillRectangle(tabRect, tabColor);
                    }

                    // Draw text
                    Render2D.DrawText(
                        style.FontMedium,
                        tab.Title,
                        new Rectangle(x + DockPanel.DefaultLeftTextMargin, 0, 10000, DockPanel.DefaultHeaderHeight),
                        style.Foreground,
                        TextAlignment.Near,
                        TextAlignment.Center);

                    // Draw cross
                    if (isSelected || isMouseOver)
                    {
                        var crossRect = new Rectangle(x + width - DockPanel.DefaultButtonsSize - DockPanel.DefaultButtonsMargin, (DockPanel.DefaultHeaderHeight - DockPanel.DefaultButtonsSize) / 2, DockPanel.DefaultButtonsSize, DockPanel.DefaultButtonsSize);
                        bool isMouseOverCross = isMouseOver && crossRect.Contains(MousePosition);
                        if (isMouseOverCross)
                            Render2D.FillRectangle(crossRect, tabColor * 1.3f);
                        Render2D.DrawSprite(style.Cross, crossRect, isMouseOverCross ? Color.White : new Color(0.8f));
                    }

                    // Move
                    x += width;
                }
            }
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear
            IsMouseDown = false;
            MouseDownWindow = null;

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            // Cache mouse
            MousePosition = location;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check buttons
            if (buttons == MouseButtons.Left)
            {
                // Cache data
                IsMouseDown = true;
                MouseDownWindow = getTabAtPos(location, out IsMouseDownOverCross);
                if (!IsMouseDownOverCross && MouseDownWindow != null)
                    _panel.SelectTab(MouseDownWindow);
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Check buttons
            if (buttons == MouseButtons.Left && IsMouseDown)
            {
                // Clear flag
                IsMouseDown = false;

                // Check tabs under mouse position at the begining and at the end
                bool overCross;
                var tab = getTabAtPos(location, out overCross);

                // Check if tabs are the same and cross was pressed
                if (tab != null && tab == MouseDownWindow && IsMouseDownOverCross && overCross)
                    tab.Close(ClosingReason.User);
                MouseDownWindow = null;
            }


            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Cache mouse
            MousePosition = location;

            // Check if mouse is down
            if (IsMouseDown)
            {
                // Check if mouse is outside the header
                if (!HeaderRectangle.Contains(location))
                {
                    // Clear flag
                    IsMouseDown = false;

                    // Check tab under the mouse
                    if (!IsMouseDownOverCross && MouseDownWindow != null)
                        startDrag(MouseDownWindow);
                    MouseDownWindow = null;
                }
                // Check if has more than one tab to change order
                else if (MouseDownWindow != null && _panel.TabsCount > 1)
                {
                    // Check if mouse left current tab rect
                    Rectangle currWinRect;
                    getTabRect(MouseDownWindow, out currWinRect);
                    if (!currWinRect.Contains(location))
                    {
                        int index = _panel.GetTabIndex(MouseDownWindow);

                        // Check if move right or left
                        if (location.X < currWinRect.X)
                        {
                            // Move left
                            _panel.MoveTabLeft(index);
                        }
                        else if (_panel.LastTab != MouseDownWindow)
                        {
                            // Move right
                            _panel.MoveTabRight(index);
                        }

                        // Update
                        _panel.PerformLayout();
                    }
                }
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Check if mouse is down
            if (IsMouseDown)
            {
                // Clear flag
                IsMouseDown = false;

                // Check tabs under mouse position
                if (!IsMouseDownOverCross && MouseDownWindow != null)
                    startDrag(MouseDownWindow);
                MouseDownWindow = null;
            }

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        protected override void GetDesireClientArea(out Rectangle rect)
        {
            rect = new Rectangle(0, DockPanel.DefaultHeaderHeight, Width, Height - DockPanel.DefaultHeaderHeight);
        }
    }
}
