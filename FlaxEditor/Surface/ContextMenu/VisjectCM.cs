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
        private readonly TextBox _searchBox;

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
            panel1.Bounds = new Rectangle(0, _searchBox.Bottom + 1, Width, Height - _searchBox.Bottom - 2);
            panel1.Parent = this;

            // Create second panel (for groups arrangement)
            var panel2 = new VerticalPanel();
            panel2.Width = panel1.Width;
            panel2.Parent = panel1;
            
            // Init groups
            var groups = NodeFactory.Groups;
            var nodes = new List<NodeArchetype>();
            foreach (var groupArchetype in groups)
            {
                // Get valid nodes
                nodes.Clear();
                foreach (var nodeArchetype in groupArchetype.Archetypes)
                {
                    if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0)
                        continue;
                    if (type == SurfaceType.Material)
                    {
                        if ((nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
                            continue;
                    }
                    else
                    {
                        if ((nodeArchetype.Flags & NodeFlags.MaterialOnly) != 0)
                            continue;
                    }

                    nodes.Add(nodeArchetype);
                }
                
                // Check if can create group for them
                if (nodes.Count > 0)
                {
                    var group = new VisjectCMGroup(this, groupArchetype);
                    group.Close();
                    group.EndAnimation();
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var item = new VisjectCMItem(group, nodes[i]);
                        item.Parent = group;
                    }
                    group.Parent = panel2;
                    _groups.Add(group);
                }
            }
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
            _searchBox.Focus();
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
            Focus();
            
            base.OnShow();
        }

        /// <inheritdoc />
        protected override void OnHide()
        {
            Focus(null);
            
            base.OnHide();
        }
    }
}
