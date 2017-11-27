////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI.Tabs
{
    /// <summary>
    /// Represents control which contains collection of <see cref="Tab"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Tabs : ContainerControl
    {
        private Vector2 _mosuePosition;

        /// <summary>
        /// The selected tab index.
        /// </summary>
        protected int _selectedIndex;

        /// <summary>
        /// The tabs size.
        /// </summary>
        protected Vector2 _tabsSize;

        /// <summary>
        /// The orientation.
        /// </summary>
        protected Orientation _orientation;

        /// <summary>
        /// Gets the size of the tabs.
        /// </summary>
        public Vector2 TabsSize
        {
            get => _tabsSize;
            set
            {
                _tabsSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the color of the tab strip background.
        /// </summary>
        public Color TabStripColor { get; set; }

        /// <summary>
        /// Occurs when selected tab gets changed.
        /// </summary>
        public event Action<Tabs> SelectedTabChanged;

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        public Tab SelectedTab
        {
            get => _selectedIndex == -1 ? null : Children[_selectedIndex] as Tab;
            set => SelectedTabIndex = Children.IndexOf(value);
        }

        /// <summary>
        /// Gets or sets the selected tab index.
        /// </summary>
        public int SelectedTabIndex
        {
            get => _selectedIndex;
            set
            {
                var index = value;

                // Clamp index
                if (index < -1)
                    index = -1;
                else if (index >= Children.Count)
                    index = Children.Count - 1;

                // Check if index will change
                if (_selectedIndex != index)
                {
                    // Change selected index
                    _selectedIndex = index;

                    // Update
                    PerformLayout();

                    // Fire events
                    OnSelectedTabChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tabs"/> class.
        /// </summary>
        public Tabs()
        {
            CanFocus = false;
            
            _selectedIndex = -1;
            _tabsSize = new Vector2(70, 16);
            _orientation = Orientation.Horizontal;

            BackgroundColor = Style.Current.Background;
            TabStripColor = Style.Current.LightBackground;
        }

        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <typeparam name="T">Tab control type.</typeparam>
        /// <param name="tab">The tab.</param>
        /// <returns>The tab.</returns>
        public T AddTab<T>(T tab) where T : Tab
        {
            if (tab == null)
                throw new ArgumentNullException();
            Assertions.Assert.IsFalse(Children.Contains(tab));

            // Add
            tab.Parent = this;

            // Check if has no selected tab
            if (_selectedIndex == -1)
                SelectedTab = tab;

            return tab;
        }

        /// <summary>
        /// Called when selected tab gets changed.
        /// </summary>
        protected virtual void OnSelectedTabChanged()
        {
            SelectedTabChanged?.Invoke(this);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            var tabRect = new Rectangle(Vector2.Zero, _tabsSize);
            Rectangle tabStripRect;
            Vector2 tabStripOffset;
            if (_orientation == Orientation.Horizontal)
            {
                tabStripRect = new Rectangle(0, 0, Width, _tabsSize.Y);
                tabStripOffset = new Vector2(_tabsSize.X, 0);
            }
            else
            {
                tabStripRect = new Rectangle(0, 0, _tabsSize.X, Height);
                tabStripOffset = new Vector2(0, _tabsSize.Y);
            }

            // Draw tab strip background
            Render2D.FillRectangle(tabStripRect, TabStripColor);

            // Draw all tabs
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Tab tab)
                {
                    bool isTabSelected = i == _selectedIndex;
                    bool isMouseOverTab = IsMouseOver && tabRect.MakeExpanded(-1).Contains(_mosuePosition);

                    // Draw bar
                    if (isTabSelected)
                    {
                        if (_orientation == Orientation.Horizontal)
                        {
                            Render2D.FillRectangle(tabRect, style.BackgroundSelected);
                        }
                        else
                        {
                            const float lefEdgeWidth = 4;
                            var leftEdgeRect = tabRect;
                            leftEdgeRect.Size.X = lefEdgeWidth;
                            var fillRect = tabRect;
                            fillRect.Size.X -= lefEdgeWidth;
                            fillRect.Location.X += lefEdgeWidth;
                            Render2D.FillRectangle(fillRect, style.Background);
                            Render2D.FillRectangle(leftEdgeRect, style.BackgroundSelected);
                        }
                    }
                    else if (isMouseOverTab)
                    {
                        Render2D.FillRectangle(tabRect, style.BackgroundHighlighted);
                    }

                    // Draw icon
                    if (tab.Icon.IsValid)
                    {
                        Render2D.DrawSprite(tab.Icon, tabRect.MakeExpanded(-8));
                    }

                    // Draw text
                    if (!string.IsNullOrEmpty(tab.Text))
                    {
                        Render2D.DrawText(style.FontMedium, tab.Text, new Rectangle(tabRect.X + 8, tabRect.Y, tabRect.Width - 8, tabRect.Height), Color.White, TextAlignment.Near, TextAlignment.Center);
                    }

                    // Move
                    tabRect.Offset(tabStripOffset);
                }
            }
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            _mosuePosition = location;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mosuePosition = location;

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            if (_orientation == Orientation.Horizontal)
            {
                if (location.Y <= _tabsSize.Y)
                {
                    int index = Mathf.FloorToInt(location.X / _tabsSize.X);
                    if (index < Children.Count)
                        SelectedTabIndex = index;
                    Focus();
                    return true;
                }
            }
            else
            {
                if (location.X <= _tabsSize.X)
                {
                    int index = Mathf.FloorToInt(location.Y / _tabsSize.Y);
                    if (index < Children.Count)
                        SelectedTabIndex = index;
                    Focus();
                    return true;
                }
            }
            
            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Hide all pages except selected one
            var clientArea = _orientation == Orientation.Horizontal
                ? new Rectangle(0, _tabsSize.Y, Width, Height - _tabsSize.Y)
                : new Rectangle(_tabsSize.X, 0, Width - _tabsSize.X, Height);
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Tab tab)
                {
                    // Check if is selected or not
                    if (i == _selectedIndex)
                    {
                        // Show and fit size
                        tab.Visible = true;
                        tab.Bounds = clientArea;
                    }
                    else
                    {
                        // Hide
                        tab.Visible = false;
                    }
                }
            }
        }
    }
}
