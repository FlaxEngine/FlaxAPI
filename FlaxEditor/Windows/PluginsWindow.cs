// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Utilities;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor tool window for plugins management using <see cref="PluginManager"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class PluginsWindow : EditorWindow
    {
        private Tabs _tabs;
        private readonly List<CategoryEntry> _categories = new List<CategoryEntry>();
        private readonly Dictionary<Plugin, PluginEntry> _entries = new Dictionary<Plugin, PluginEntry>();

        /// <summary>
        /// Plugin entry control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        public class PluginEntry : ContainerControl
        {
            /// <summary>
            /// The plugin.
            /// </summary>
            public readonly Plugin Plugin;

            /// <summary>
            /// The category.
            /// </summary>
            public readonly CategoryEntry Category;

            /// <summary>
            /// Initializes a new instance of the <see cref="PluginEntry"/> class.
            /// </summary>
            /// <param name="plugin">The plugin.</param>
            /// <param name="category">The category.</param>
            /// <param name="desc">Plugin description</param>
            public PluginEntry(Plugin plugin, CategoryEntry category, ref PluginDescription desc)
            {
                Plugin = plugin;
                Category = category;

                float margin = 4;
                float iconSize = 64;

                var iconImage = new Image(margin, margin, iconSize, iconSize)
                {
                    Brush = new SpriteBrush(Editor.Instance.Icons.Plugin64),
                    Parent = this,
                };

                var icon = PluginUtils.TryGetPluginIcon(plugin);
                if (icon)
                    iconImage.Brush = new TextureBrush(icon);

                Width = 300;
                Height = 100;

                float tmp1 = iconImage.Right + margin;
                var nameLabel = new Label(tmp1, margin, Width - tmp1 - margin, 28)
                {
                    HorizontalAlignment = TextAlignment.Near,
                    AnchorStyle = AnchorStyle.Upper,
                    Text = desc.Name,
                    Font = new FontReference(Style.Current.FontLarge),
                    Parent = this,
                };

                tmp1 = nameLabel.Bottom + margin + 8;
                var descLabel = new Label(nameLabel.X, tmp1, nameLabel.Width, Height - tmp1 - margin)
                {
                    HorizontalAlignment = TextAlignment.Near,
                    VerticalAlignment = TextAlignment.Near,
                    Wrapping = TextWrapping.WrapWords,
                    AnchorStyle = AnchorStyle.Upper,
                    Text = desc.Description,
                    Parent = this,
                };

                string versionString = string.Empty;
                if (desc.IsAlpha)
                    versionString = "ALPHA ";
                else if (desc.IsBeta)
                    versionString = "BETA ";
                versionString += "Version ";
                versionString += desc.Version != null ? desc.Version.ToString() : "1.0";
                var versionLabel = new Label(Width - 140 - margin, margin, 140, 14)
                {
                    HorizontalAlignment = TextAlignment.Far,
                    VerticalAlignment = TextAlignment.Center,
                    AnchorStyle = AnchorStyle.UpperRight,
                    Text = versionString,
                    Parent = this,
                };

                string url = null;
                if (!string.IsNullOrEmpty(desc.AuthorUrl))
                    url = desc.AuthorUrl;
                else if (!string.IsNullOrEmpty(desc.HomepageUrl))
                    url = desc.HomepageUrl;
                else if (!string.IsNullOrEmpty(desc.RepositoryUrl))
                    url = desc.RepositoryUrl;
                versionLabel.Font.Font.WaitForLoaded();
                var font = versionLabel.Font.GetFont();
                var authorWidth = font.MeasureText(desc.Author).X + 8;
                var authorLabel = new ClickLabel(Width - authorWidth - margin, versionLabel.Bottom + margin, authorWidth, 14)
                {
                    HorizontalAlignment = TextAlignment.Far,
                    VerticalAlignment = TextAlignment.Center,
                    AnchorStyle = AnchorStyle.UpperRight,
                    Text = desc.Author,
                    Parent = this,
                };
                if (url != null)
                {
                    authorLabel.TextColorHighlighted = Style.Current.BackgroundSelected;
                    authorLabel.DoubleClick = () => Application.StartProcess(url);
                }
            }

            private class ClickLabel : Label
            {
                /// <summary>
                /// The double click event.
                /// </summary>
                public Action DoubleClick;

                public ClickLabel(float x, float y, float width, float height)
                : base(x, y, width, height)
                {
                }

                /// <inheritdoc />
                public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
                {
                    DoubleClick?.Invoke();

                    return base.OnMouseDoubleClick(location, buttons);
                }
            }
        }

        /// <summary>
        /// Plugins category control.
        /// </summary>
        /// <seealso cref="FlaxEditor.GUI.Tab" />
        public class CategoryEntry : Tab
        {
            /// <summary>
            /// The panel for the plugin entries.
            /// </summary>
            public VerticalPanel Panel;

            /// <summary>
            /// Initializes a new instance of the <see cref="CategoryEntry"/> class.
            /// </summary>
            /// <param name="text">The text.</param>
            public CategoryEntry(string text)
            : base(text)
            {
                var scroll = new Panel(ScrollBars.Vertical)
                {
                    DockStyle = DockStyle.Fill,
                    Parent = this,
                };
                var panel = new VerticalPanel
                {
                    DockStyle = DockStyle.Top,
                    IsScrollable = true,
                    Parent = scroll,
                };
                Panel = panel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public PluginsWindow(Editor editor)
        : base(editor, true, ScrollBars.None)
        {
            Title = "Plugins";

            _tabs = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = this
            };

            // Check already loaded plugins
            PluginManager.GamePlugins.ForEach(OnPluginLoaded);
            PluginManager.EditorPlugins.ForEach(OnPluginLoaded);

            // Register for events
            PluginManager.PluginLoaded += OnPluginLoaded;
            PluginManager.PluginUnloading += OnPluginUnloading;
        }

        private void OnPluginLoaded(Plugin plugin)
        {
            var entry = GetPluginEntry(plugin);
            if (entry != null)
                return;

            // Special case for editor plugins (merge with game plugin if has linked)
            if (plugin is EditorPlugin editorPlugin && GetPluginEntry(editorPlugin.GamePluginType) != null)
                return;

            var desc = plugin.Description;
            var category = _categories.Find(x => string.Equals(x.Text, desc.Category, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                category = new CategoryEntry(desc.Category)
                {
                    DockStyle = DockStyle.Fill,
                    Parent = _tabs
                };
                _categories.Add(category);
                category.UnlockChildrenRecursive();
            }

            entry = new PluginEntry(plugin, category, ref desc)
            {
                Parent = category.Panel,
            };
            _entries.Add(plugin, entry);
            entry.UnlockChildrenRecursive();

            if (_tabs.SelectedTab == null)
                _tabs.SelectedTab = category;
        }

        private void OnPluginUnloading(Plugin plugin)
        {
            var entry = GetPluginEntry(plugin);
            if (entry == null)
                return;

            var category = entry.Category;
            _entries.Remove(plugin);
            entry.Dispose();

            // If category is not used anymore
            if (_entries.Values.Count(x => x.Category == category) == 0)
            {
                if (_tabs.SelectedTab == category)
                    _tabs.SelectedTab = null;

                category.Dispose();
                _categories.Remove(category);

                if (_tabs.SelectedTab == null && _categories.Count > 0)
                    _tabs.SelectedTab = _categories[0];
            }
        }

        /// <summary>
        /// Gets the plugin entry control.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>The entry.</returns>
        public PluginEntry GetPluginEntry(Plugin plugin)
        {
            _entries.TryGetValue(plugin, out var e);
            return e;
        }

        /// <summary>
        /// Gets the plugin entry control.
        /// </summary>
        /// <param name="pluginType">The plugin type.</param>
        /// <returns>The entry.</returns>
        public PluginEntry GetPluginEntry(Type pluginType)
        {
            if (pluginType == null)
                return null;

            foreach (var e in _entries.Keys)
            {
                if (e.GetType() == pluginType)
                    return _entries[e];
            }

            return null;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            PluginManager.PluginLoaded -= OnPluginLoaded;
            PluginManager.PluginUnloading -= OnPluginUnloading;

            base.Dispose();
        }
    }
}
