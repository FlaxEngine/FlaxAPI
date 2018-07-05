// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private bool _waitingForInput;
        private VisjectCMGroup _surfaceParametersGroup;
        private Panel _panel1;
        private VerticalPanel _panel2;
        private Func<List<SurfaceParameter>> _parametersGetter;

        /// <summary>
        /// The selected item
        /// </summary>
        public VisjectCMItem SelectedItem;

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
        /// <param name="parametersGetter">The surface parameters getter callback.</param>
        public VisjectCM(SurfaceType type, Func<List<SurfaceParameter>> parametersGetter)
        {
            Type = type;
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
            _panel2 = panel2;

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

                    if (type != SurfaceType.Material && (nodeArchetype.Flags & NodeFlags.MaterialOnly) != 0)
                        continue;

                    if (type != SurfaceType.AnimationGraph && (nodeArchetype.Flags & NodeFlags.AnimGraphOnly) != 0)
                        continue;

                    if (type != SurfaceType.Visject && (nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
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
                _groups[i].UpdateFilter(ReplaceNodeAlts(_searchBox.Text));
            if (SelectedItem == null || !SelectedItem.VisibleInHierarchy)
            {
                var group = _groups.Find(g => g.Visible);
                if (group != null)
                {
                    SelectedItem = group.Children.Find(c => c.Visible && c is VisjectCMItem item) as VisjectCMItem;
                }
            }
            if (SelectedItem != null) _panel1.ScrollViewTo(SelectedItem);
            PerformLayout();
            _searchBox.Focus();
        }

        //TODO: Ewww, refactor me
        private static readonly Dictionary<string, string> _nodeAlts = new Dictionary<string, string>()
        {
            {"*", "multiply" },
            {"/", "divide" },
            {"+", "add"},
            {"-", "subtract" },
            {"^", "power" },
            {"**", "power" },
            {"%", "modulo" }
        };

        //TODO: You're going to remove this and patiently wait for the shunting yard thing
        private class DataNodeAlt
        {
            public readonly Regex Regex;
            public readonly string Text;
            public readonly Func<string, object> ToData;

            public DataNodeAlt(Regex regex, string text, Func<string, object> toData)
            {
                Regex = regex;
                Text = text;
                ToData = toData;
            }
        }

        private static readonly List<DataNodeAlt> _dataNodeAlts = new List<DataNodeAlt>()
        {
            new DataNodeAlt(new Regex(@"[+-]?\d+(\.\d*)?"), "float", txt => float.Parse(txt)) //TODO: Multiple choices

        };
        // TODO: Refactor this to be somewhat decent
        private string ReplaceNodeAlts(string text)
        {
            foreach (var dNodeAlt in _dataNodeAlts)
            {
                text = dNodeAlt.Regex.Replace(text, dNodeAlt.Text);
            }
            foreach (var pair in _nodeAlts)
            {
                text = text.Replace(pair.Key, pair.Value);
            }
            return text;
        }

        private object[] GetData(string text)
        {
            //TODO: Use the shunting yard algorithm
            foreach (var dNodeAlt in _dataNodeAlts)
            {
                if (dNodeAlt.Regex.IsMatch(text))
                {
                    return new object[] { dNodeAlt.ToData(text) };
                }
            }
            return null;
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnClickItem(VisjectCMItem item)
        {
            item.Data = GetData(_searchBox.Text);
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
            var parameters = _parametersGetter();
            int count = parameters?.Count(x => x.IsPublic) ?? 0;
            if (count > 0)
            {
                // TODO: cache the allocated memory to reduce dynamic allocations
                var archetypes = new NodeArchetype[count];
                int archetypeIndex = 0;
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
                group.Parent = _panel2;
                _groups.Add(group);
                _surfaceParametersGroup = group;
            }
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
                if (SelectedItem != null) OnClickItem(SelectedItem);
                else Hide();
                return true;
            }
            else if (key == Keys.ArrowUp)
            {

                var nextSelectedItem = GetPrevious(SelectedItem);

                if (nextSelectedItem == null && SelectedItem != null)
                {
                    var group = GetPrevious(SelectedItem.Group);
                    if (group != null)
                    {
                        nextSelectedItem = group.Children.FindLast(c => c.Visible && c is VisjectCMItem item) as VisjectCMItem;
                    }
                }
                if (nextSelectedItem != null)
                {
                    SelectedItem = nextSelectedItem;
                    _panel1.ScrollViewTo(SelectedItem);
                }
                return true;
            }
            else if (key == Keys.ArrowDown)
            {

                var nextSelectedItem = GetNext(SelectedItem);
                if (nextSelectedItem == null && SelectedItem != null)
                {
                    var group = GetNext(SelectedItem.Group);
                    if (group != null)
                    {
                        nextSelectedItem = group.Children.Find(c => c.Visible && c is VisjectCMItem item) as VisjectCMItem;
                    }
                }
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

        private T GetNext<T>(T item) where T : Control
        {
            if (item == null) return null;
            var parent = item.Parent;
            for (int i = item.IndexInParent + 1; i < parent.ChildrenCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.Visible && child is T nextItem)
                {
                    return nextItem;
                }
            }
            return null;
        }

        private T GetPrevious<T>(T item) where T : Control
        {
            if (item == null) return null;
            var parent = item.Parent;
            for (int i = item.IndexInParent - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                if (child.Visible && child is T prevItem)
                {
                    return prevItem;
                }
            }
            return null;
        }
    }
}
