// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.Options;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Utilities;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Editor window for changing the options.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed class EditorOptionsWindow : EditorWindow
    {
        private bool _isDataDirty;
        private Tabs _tabs;
        private EditorOptions _options;
        private ToolStripButton _saveButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorOptionsWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public EditorOptionsWindow(Editor editor)
        : base(editor, true, ScrollBars.None)
        {
            Title = "Editor Options";

            var toolstrip = new ToolStrip
            {
                Parent = this
            };
            _saveButton = (ToolStripButton)toolstrip.AddButton(editor.Icons.Save32, SaveData).LinkTooltip("Save");
            _saveButton.Enabled = false;

            _tabs = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = this
            };

            CreateTab("General", () => _options.General);
            CreateTab("Interface", () => _options.Interface);
            CreateTab("Visual", () => _options.Visual);
            CreateTab("Source Code", () => _options.SourceCode);

            GatherData();

            _tabs.SelectedTabIndex = 0;
        }

        private void CreateTab(string name, Func<object> getValue)
        {
            var tab = _tabs.AddTab(new Tab(name));

            var panel = new Panel(ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };

            var settings = new CustomEditorPresenter(null);
            settings.Panel.Parent = panel;
            settings.Panel.Tag = getValue;
            settings.Modified += MarkAsEdited;
        }

        private void MarkAsEdited()
        {
            if (!_isDataDirty)
            {
                _saveButton.Enabled = true;
                _isDataDirty = true;
            }
        }

        private void ClearDirtyFlag()
        {
            if (_isDataDirty)
            {
                _saveButton.Enabled = false;
                _isDataDirty = false;
            }
        }

        /// <summary>
        /// Load the editor options data.
        /// </summary>
        private void GatherData()
        {
            // Clone options (edit cloned version, not the current ones)
            _options = new EditorOptions();
            _options.General = Editor.Options.Options.General.DeepClone();
            _options.Interface = Editor.Options.Options.Interface.DeepClone();
            _options.Visual = Editor.Options.Options.Visual.DeepClone();
            _options.SourceCode = Editor.Options.Options.SourceCode.DeepClone();

            // Refresh tabs
            foreach (var c in _tabs.Children)
            {
                if (c is Tab tab)
                {
                    var panel = tab.GetChild<Panel>();
                    var settingsPanel = panel.GetChild<CustomEditorPresenter.PresenterPanel>();
                    var getValue = (Func<object>)settingsPanel.Tag;
                    var settings = settingsPanel.Presenter;
                    settings.Select(getValue());
                }
            }

            ClearDirtyFlag();
        }

        /// <summary>
        /// Saves the editor options data.
        /// </summary>
        private void SaveData()
        {
            if (_options == null || !_isDataDirty)
                return;

            Editor.Options.Apply(_options);

            ClearDirtyFlag();
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            GatherData();
        }
    }
}
