////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// The Visject Surface dedicated context menu for nodes spawning.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
    public sealed class VisjectCM : ContextMenuBase
    {
        private readonly List<VisjectCMGroup> _groups = new List<VisjectCMGroup>(16);
        private TextBox _searchBox;

        /// <summary>
        /// The type of the surface.
        /// </summary>
        public readonly SurfaceType Type;

        /// <summary>
        /// Event fired when any item in this popup menu gets clicked.
        /// </summary>
        public event Action<VisjectCMItem> OnItemClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectCM"/> class.
        /// </summary>
        /// <param name="type">The surface type.</param>
        public VisjectCM(SurfaceType type)
        {
            Type = type;

            // Context menu dimensions
            Size = new Vector2(320, 220);

            // Search box
            _searchBox = new TextBox(false, 1, 1);
            _searchBox.Width = Width - 3;
            _searchBox.WatermarkText = "Search...";
            _searchBox.TextChanged += OnSearchFilterChanged;
            _searchBox.Parent = this;

            // Create first panel (for scrollbar)
            var panel1 = new Panel(ScrollBars.Vertical);
            panel1.DockStyle = DockStyle.Fill;
            panel1.Parent = this;

            // Create second panel (for groups arrangement)
            var panel2 = new VerticalPanel();
            panel2.Width = panel1.Width;
            panel2.Parent = panel1;
        }

        private void OnSearchFilterChanged()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // Update groups
            for (int i = 0; i < _groups.Count; i++)
                _groups[i].UpdateFilter(_searchBox.Text);
            PerformLayout();
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnClickItem(VisjectCMItem item)
        {
            Hide();
            OnItemClicked?.Invoke(item);
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            for (int i = 0; i < _groups.Count; i++)
                _groups[i].ResetView();

            _searchBox.Clear();

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Prepare
            ResetView();

            base.OnShow();
        }
    }
}
