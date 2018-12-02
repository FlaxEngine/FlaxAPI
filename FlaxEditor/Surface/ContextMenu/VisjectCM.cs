// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// The Visject Surface dedicated context menu for nodes spawning.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
    public class VisjectCM : ContextMenuBase
    {
        /// <summary>
        /// Visject context menu item clicked delegate.
        /// </summary>
        /// <param name="clickedItem">The item that was clicked</param>
        /// <param name="selectedBox">The currently user-selected box. Can be null.</param>
        public delegate void ItemClickedDelegate(VisjectCMItem clickedItem, Elements.Box selectedBox);

        private readonly List<VisjectCMGroup> _groups = new List<VisjectCMGroup>(16);
        private readonly TextBox _searchBox;
        private bool _waitingForInput;
        private VisjectCMGroup _surfaceParametersGroup;
        private Panel _panel1;
        private VerticalPanel _groupsPanel;
        private readonly Func<List<SurfaceParameter>> _parametersGetter;
        private Elements.Box _selectedBox;

        /// <summary>
        /// The selected item
        /// </summary>
        public VisjectCMItem SelectedItem;

        /// <summary>
        /// Event fired when any item in this popup menu gets clicked.
        /// </summary>
        public event ItemClickedDelegate OnItemClicked;

        /// <summary>
        /// Gets or sets a value indicating whether show groups expanded or collapsed.
        /// </summary>
        public bool ShowExpanded { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectCM"/> class.
        /// </summary>
        /// <param name="groups">The group archetypes. Cannot be null.</param>
        /// <param name="canSpawnNodeType">The surface node type validation helper. Cannot be null.</param>
        /// <param name="parametersGetter">The surface parameters getter callback. Can be null.</param>
        public VisjectCM(List<GroupArchetype> groups, Func<NodeArchetype, bool> canSpawnNodeType, Func<List<SurfaceParameter>> parametersGetter = null)
        {
            if (groups == null)
                throw new ArgumentNullException(nameof(groups));
            if (canSpawnNodeType == null)
                throw new ArgumentNullException(nameof(canSpawnNodeType));
            _parametersGetter = parametersGetter;

            // Context menu dimensions
            Size = new Vector2(320, 220);

            // Search box
            _searchBox = new TextBox(false, 1, 1)
            {
                Width = Width - 3,
                WatermarkText = "Search...",
                Parent = this
            };
            _searchBox.TextChanged += OnSearchFilterChanged;

            // Create first panel (for scrollbar)
            var panel1 = new Panel(ScrollBars.Vertical)
            {
                Bounds = new Rectangle(0, _searchBox.Bottom + 1, Width, Height - _searchBox.Bottom - 2),
                Parent = this
            };

            _panel1 = panel1;

            // Create second panel (for groups arrangement)
            var panel2 = new VerticalPanel
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = panel1
            };
            _groupsPanel = panel2;

            // Init groups
            int index = 0;
            var nodes = new List<NodeArchetype>();
            foreach (var groupArchetype in groups)
            {
                // Get valid nodes
                nodes.Clear();
                foreach (var nodeArchetype in groupArchetype.Archetypes)
                {
                    if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0 || !canSpawnNodeType(nodeArchetype))
                        continue;

                    nodes.Add(nodeArchetype);
                }

                // Check if can create group for them
                if (nodes.Count > 0)
                {
                    var group = new VisjectCMGroup(this, groupArchetype);
                    group.Close(false);
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var item = new VisjectCMItem(group, nodes[i]);
                        item.Parent = group;
                    }
                    group.SortChildren();
                    group.Parent = panel2;
                    group.DefaultIndex = index++;
                    _groups.Add(group);
                }
            }
        }

        /// <summary>
        /// Adds the group archetype to add to the menu.
        /// </summary>
        /// <param name="groupArchetype">The group.</param>
        public void AddGroup(GroupArchetype groupArchetype)
        {
            // Get valid nodes
            var nodes = new List<NodeArchetype>();
            foreach (var nodeArchetype in groupArchetype.Archetypes)
            {
                if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0)
                    continue;

                nodes.Add(nodeArchetype);
            }

            // Check if can create group for them
            if (nodes.Count > 0)
            {
                var group = new VisjectCMGroup(this, groupArchetype);
                group.Close(false);
                for (int i = 0; i < nodes.Count; i++)
                {
                    var item = new VisjectCMItem(group, nodes[i]);
                    item.Parent = group;
                }
                group.SortChildren();
                group.Parent = _groupsPanel;
                group.DefaultIndex = _groups.Count;
                _groups.Add(group);
            }
        }

        private void OnSearchFilterChanged()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // Update groups
            for (int i = 0; i < _groups.Count; i++)
            {
                _groups[i].UpdateFilter(_searchBox.Text);
                _groups[i].UpdateItemSort(_selectedBox);
            }

            SortGroups();

            // If no item is selected (or it's not visible anymore), select the top one
            if (SelectedItem == null || !SelectedItem.VisibleInHierarchy)
            {
                SelectedItem = _groups.Find(g => g.Visible)?.Children.Find(c => c.Visible && c is VisjectCMItem) as VisjectCMItem;
            }
            if (SelectedItem != null)
                _panel1.ScrollViewTo(SelectedItem);
            PerformLayout();
            _searchBox.Focus();
        }

        /// <summary>
        /// Sort the groups and keeps <see cref="_groups"/> in sync
        /// </summary>
        private void SortGroups()
        {
            // Sort groups
            _groupsPanel.SortChildren();
            // Synchronize with _groups[]
            for (int i = 0, groupsIndex = 0; i < _groupsPanel.ChildrenCount; i++)
            {
                if (_groupsPanel.Children[i] is VisjectCMGroup group)
                {
                    _groups[groupsIndex] = group;
                    groupsIndex++;
                }
            }
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnClickItem(VisjectCMItem item)
        {
            Hide();
            OnItemClicked?.Invoke(item, _selectedBox);
        }

        /// <summary>
        /// Expands all the groups.
        /// </summary>
        /// <param name="animate">Enable/disable animation feature.</param>
        public void ExpandAll(bool animate = false)
        {
            for (int i = 0; i < _groups.Count; i++)
                _groups[i].Open(animate);
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

            SortGroups();
            _searchBox.Clear();
            SelectedItem = null;
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Updates the surface parameters group.
        /// </summary>
        private void UpdateSurfaceParametersGroup()
        {
            // Remove the old one
            if (_surfaceParametersGroup != null)
            {
                _groups.Remove(_surfaceParametersGroup);
                _surfaceParametersGroup.Dispose();
                _surfaceParametersGroup = null;
            }

            // Check if surface has any parameters
            var parameters = _parametersGetter != null ? _parametersGetter() : null;
            int count = parameters?.Count(x => x.IsPublic) ?? 0;
            if (count > 0)
            {
                // TODO: cache the allocated memory to reduce dynamic allocations
                var archetypes = new NodeArchetype[count];
                int archetypeIndex = 0;

                // ReSharper disable once PossibleNullReferenceException
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (!parameters[i].IsPublic)
                        continue;

                    archetypes[archetypeIndex++] = new NodeArchetype
                    {
                        TypeID = 1,
                        Create = Archetypes.Parameters.CreateGetNode,
                        Title = "Get " + parameters[i].Name,
                        Description = "Parameter value getter",
                        Size = new Vector2(140, 60),
                        DefaultValues = new object[]
                        {
                            parameters[i].ID
                        },
                        Elements = new[]
                        {
                            NodeElementArchetype.Factory.ComboBox(2, 0, 116)
                        }
                    };
                }

                var groupArchetype = new GroupArchetype
                {
                    GroupID = 6,
                    Name = "Surface Parameters",
                    Color = new Color(52, 73, 94),
                    Archetypes = archetypes
                };

                var group = new VisjectCMGroup(this, groupArchetype);
                group.Close(false);
                archetypeIndex = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (!parameters[i].IsPublic)
                        continue;

                    var item = new VisjectCMItem(group, archetypes[archetypeIndex++]);
                    item.Parent = group;
                }
                group.SortChildren();
                group.UnlockChildrenRecursive();
                group.Parent = _groupsPanel;
                _groups.Add(group);
                _surfaceParametersGroup = group;
            }
        }

        /// <inheritdoc />
        public override void Show(Control parent, Vector2 location)
        {
            Show(parent, location, null);
        }

        /// <summary>
        /// Show context menu over given control.
        /// </summary>
        /// <param name="parent">Parent control to attach to it.</param>
        /// <param name="location">Popup menu origin location in parent control coordinates.</param>
        /// <param name="startBox">The currently selected box that the new node will get connected to. Can be null</param>
        public void Show(Control parent, Vector2 location, Elements.Box startBox)
        {
            _selectedBox = startBox;
            base.Show(parent, location);
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Prepare
            UpdateSurfaceParametersGroup();
            ResetView();
            Focus();
            _waitingForInput = true;

            base.OnShow();
        }

        /// <inheritdoc />
        public override void Hide()
        {
            Focus(null);

            base.Hide();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                Hide();
                return true;
            }
            else if (key == Keys.Return)
            {
                if (SelectedItem != null)
                    OnClickItem(SelectedItem);
                else
                    Hide();
                return true;
            }
            else if (key == Keys.ArrowUp)
            {
                if (SelectedItem == null)
                    return true;

                var previousSelectedItem = GetPreviousSiblings<VisjectCMItem>(SelectedItem).FirstOrDefault(c => c.Visible) ??
                                           (GetPreviousSiblings<VisjectCMGroup>(SelectedItem.Group).FirstOrDefault(c => c.Visible)?.Children
                                                                                                   .FindLast(c => c.Visible && c is VisjectCMItem) as VisjectCMItem);

                if (previousSelectedItem != null)
                {
                    SelectedItem = previousSelectedItem;

                    // Scroll into view (without smoothing)
                    _panel1.VScrollBar.SmoothingScale = 0;
                    _panel1.ScrollViewTo(SelectedItem);
                    _panel1.VScrollBar.SmoothingScale = 1;
                }
                return true;
            }
            else if (key == Keys.ArrowDown)
            {
                if (SelectedItem == null)
                    return true;

                var nextSelectedItem = GetNextSiblings<VisjectCMItem>(SelectedItem).FirstOrDefault(c => c.Visible) ??
                                       (GetNextSiblings<VisjectCMGroup>(SelectedItem.Group).FirstOrDefault(c => c.Visible)?.Children
                                                                                           .OfType<VisjectCMItem>().FirstOrDefault(c => c.Visible));

                if (nextSelectedItem != null)
                {
                    SelectedItem = nextSelectedItem;
                    _panel1.ScrollViewTo(SelectedItem);
                }
                return true;
            }

            if (_waitingForInput)
            {
                _waitingForInput = false;
                _searchBox.Focus();
                return _searchBox.OnKeyDown(key);
            }

            return base.OnKeyDown(key);
        }

        /// <summary>
        /// Gets the next siblings of a control.
        /// </summary>
        /// <param name="item">A control that is attached to a parent</param>
        /// <returns>An <see cref="IEnumerable{Control}"/> with the siblings that come after the current one.</returns>
        private IEnumerable<Control> GetNextSiblings(Control item)
        {
            if (item?.Parent == null)
                yield break;

            var parent = item.Parent;
            for (int i = item.IndexInParent + 1; i < parent.ChildrenCount; i++)
            {
                yield return parent.GetChild(i);
            }
        }

        /// <summary>
        /// Gets the next siblings of a control that have a specific type.
        /// </summary>
        /// <typeparam name="T">The type that the controls should have.</typeparam>
        /// <param name="item">A control that is attached to a parent</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the siblings that come after the current one.</returns>
        private IEnumerable<T> GetNextSiblings<T>(Control item) where T : Control
        {
            return GetNextSiblings(item).OfType<T>();
        }

        /// <summary>
        /// Gets the previous siblings of a control.
        /// </summary>
        /// <param name="item">A control that is attached to a parent</param>
        /// <returns>An <see cref="IEnumerable{Control}"/> with the siblings that come before the current one.</returns>
        private IEnumerable<Control> GetPreviousSiblings(Control item)
        {
            if (item == null || item.Parent == null)
                yield break;

            var parent = item.Parent;
            for (int i = item.IndexInParent - 1; i >= 0; i--)
            {
                yield return parent.GetChild(i);
            }
        }

        /// <summary>
        /// Gets the previous sibling of a control that have a specific type.
        /// </summary>
        /// <typeparam name="T">The type that the controls should have.</typeparam>
        /// <param name="item">A control that is attached to a parent</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the siblings that come before the current one.</returns>
        private IEnumerable<T> GetPreviousSiblings<T>(Control item) where T : Control
        {
            return GetPreviousSiblings(item).OfType<T>();
        }
    }
}
