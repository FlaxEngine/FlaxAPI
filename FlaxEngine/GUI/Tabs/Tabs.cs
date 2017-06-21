////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.GUI.Tabs
{
    /// <summary>
    /// Represents control which contains collection of <see cref="Tab"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Tabs : ContainerControl
    {
        protected readonly List<Tab> _tabs = new List<Tab>();
        protected int _selectedIndex;
        protected Vector2 _mosuePosition;
        protected Vector2 _mouseDownPos;
        protected bool _isMouseDown;
        protected Orientation _orientation;

        /// <summary>
        /// Gets the size of the tabs.
        /// </summary>
        /// <value>
        /// The size of the tabs.
        /// </value>
        public virtual Vector2 TabsSize => new Vector2(70, 16);

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
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
        /// <value>
        /// The color of the tab strip.
        /// </value>
        public Color TabStripColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tabs"/> class.
        /// </summary>
        public Tabs()
            : base(false)
        {
            _selectedIndex = -1;
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
            Assertions.Assert.IsFalse(_tabs.Contains(tab));

            // Add
            _tabs.Add(tab);
            tab.Parent = this;

            // Check if has no selected tab
            if (_selectedIndex == -1)
                SelectTab(tab);

            return tab;
        }

        /// <summary>
        /// Selects the tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        public void SelectTab(Tab tab)
        {
            SelectTab(_tabs.IndexOf(tab));
        }

        /// <summary>
        /// Selects the tab.
        /// </summary>
        /// <param name="tabIndex">Index of the tab.</param>
        public void SelectTab(int tabIndex)
        {
            // Clamp index
            if (tabIndex < -1)
                tabIndex = -1;
            else if (tabIndex >= _tabs.Count)
                tabIndex = _tabs.Count - 1;

            // Check if index will change
            if (tabIndex != _selectedIndex)
            {
                // Change selected index
                _selectedIndex = tabIndex;

                // Update
                PerformLayout();

                // Fire events
                OnSelectedTabChanged();
            }
        }

        /// <summary>
        /// Called when selected tab gets changed.
        /// </summary>
        protected virtual void OnSelectedTabChanged()
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();
            
            // Cache data
            var style = Style.Current;
            var tabSize = TabsSize;
            var tabRect = new Rectangle(Vector2.Zero, tabSize);
            Rectangle tabStripRect;
            Vector2 tabStripOffset;
            if (_orientation == Orientation.Horizontal)
            {
                tabStripRect = new Rectangle(0, 0, Width, tabSize.Y);
                tabStripOffset = new Vector2(tabSize.X, 0);
            }
            else
            {
                tabStripRect = new Rectangle(0, 0, tabSize.X, Height);
                tabStripOffset = new Vector2(0, tabSize.Y);
            }

            // Draw tab strip background
            Render2D.FillRectangle(tabStripRect, TabStripColor);

            // Draw all tabs
            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i];
                bool isTabSelected = i == _selectedIndex;
                bool isMouseOverTab = IsMouseOver && tabRect.MakeExpanded(-1).Contains(_mosuePosition);

                // Draw bar
                if (isTabSelected)
                    Render2D.FillRectangle(tabRect, style.BackgroundSelected);
                else if (isMouseOverTab)
                    Render2D.FillRectangle(tabRect, style.BackgroundHighlighted);

                // Draw icon
                if (tab.Icon.IsValid)
                {
                    Render2D.DrawSprite(tab.Icon, tabRect.MakeExpanded(-8));
                }

                // Draw text
                if (!string.IsNullOrEmpty(tab.Text))
                {

                }

                // Move
                tabRect.Offset(tabStripOffset);
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
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check if mosue is over the header
            var tabSize = TabsSize;
            if (location.Y <= tabSize.Y)
            {
                // TODO: drag and drop pages like ContentItem or TreeNode....

                // Handled
                Focus();
                return true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Check if mosue is over the header
            var tabSize = TabsSize;
            if (location.Y <= tabSize.Y)
            {
                // Check if any tab is being selected
                int index = Mathf.FloorToInt(location.X / tabSize.X);
                if (index < _tabs.Count)
                    SelectTab(index);

                // Handled
                Focus();
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Hide all pages except selected one
            var tabSize = TabsSize;
            var clientArea = _orientation == Orientation.Horizontal
                ? new Rectangle(0, tabSize.Y, Width, Height - tabSize.Y)
                : new Rectangle(tabSize.X, 0, Width - tabSize.X, Height);
            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i];

                // Check if is selected or not
                if (i == _selectedIndex)
                {
                    // Show and fit size
                    tab.Bounds = clientArea;
                    tab.UnlockChildrenRecursive();
                    tab.Visible = true;
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
