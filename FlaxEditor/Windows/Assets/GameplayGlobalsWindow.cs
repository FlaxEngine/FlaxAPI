// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="GameplayGlobals"/> asset.
    /// </summary>
    /// <seealso cref="GameplayGlobals" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class GameplayGlobalsWindow : AssetEditorWindowBase<GameplayGlobals>
    {
        private class AddRemoveParamAction : IUndoAction
        {
            public PropertiesProxy Proxy;
            public bool IsAdd;
            public string Name;
            public object DefaultValue;

            /// <inheritdoc />
            public string ActionString => IsAdd ? "Add parameter" : "Remove parameter";

            /// <inheritdoc />
            public void Do()
            {
                if (IsAdd)
                    Add();
                else
                    Remove();
            }

            /// <inheritdoc />
            public void Undo()
            {
                if (IsAdd)
                    Remove();
                else
                    Add();
            }

            private void Add()
            {
                Proxy.DefaultValues[Name] = DefaultValue;
                Proxy.Values[Name] = DefaultValue;

                Proxy.Window._propertiesEditor.BuildLayoutOnUpdate();
            }

            private void Remove()
            {
                DefaultValue = Proxy.DefaultValues[Name];

                Proxy.Values.Remove(Name);
                Proxy.DefaultValues.Remove(Name);

                Proxy.Window._propertiesEditor.BuildLayoutOnUpdate();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                DefaultValue = null;
                Proxy = null;
            }
        }

        private class RenameParamAction : IUndoAction
        {
            public PropertiesProxy Proxy;
            public string Before;
            public string After;

            /// <inheritdoc />
            public string ActionString => "Rename parameter";

            /// <inheritdoc />
            public void Do()
            {
                Rename(Before, After);
            }

            /// <inheritdoc />
            public void Undo()
            {
                Rename(After, Before);
            }

            private void Rename(string from, string to)
            {
                var defaultValue = Proxy.DefaultValues[from];

                Proxy.DefaultValues.Remove(from);
                Proxy.Values.Remove(from);

                Proxy.DefaultValues[to] = defaultValue;
                Proxy.Values[to] = defaultValue;

                Proxy.Window._propertiesEditor.BuildLayoutOnUpdate();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Before = null;
                After = null;
            }
        }

        [CustomEditor(typeof(PropertiesProxyEditor))]
        private sealed class PropertiesProxy
        {
            [NoSerialize]
            public GameplayGlobalsWindow Window;

            [NoSerialize]
            public GameplayGlobals Asset;

            public Dictionary<string, object> DefaultValues;

            [NoSerialize]
            public Dictionary<string, object> Values;

            public void Init(GameplayGlobalsWindow window)
            {
                Window = window;
                Asset = window.Asset;
                DefaultValues = Asset.DefaultValues;
                Values = Asset.Values;
            }
        }

        private sealed class VariableValueContainer : ValueContainer
        {
            private readonly PropertiesProxy _proxy;
            private readonly string _name;

            public VariableValueContainer(PropertiesProxy proxy, string name, object value)
            : base(null, value.GetType())
            {
                _proxy = proxy;
                _name = name;

                Add(value);
            }

            private object Getter(object instance, int index)
            {
                return _proxy.DefaultValues[_name];
            }

            private void Setter(object instance, int index, object value)
            {
                _proxy.DefaultValues[_name] = value;
            }

            /// <inheritdoc />
            public override void Refresh(ValueContainer instanceValues)
            {
                if (instanceValues == null || instanceValues.Count != Count)
                    throw new ArgumentException();

                for (int i = 0; i < Count; i++)
                {
                    var v = instanceValues[i];
                    this[i] = Getter(v, i);
                }
            }

            /// <inheritdoc />
            public override void Set(ValueContainer instanceValues, object value)
            {
                if (instanceValues == null || instanceValues.Count != Count)
                    throw new ArgumentException();

                for (int i = 0; i < Count; i++)
                {
                    var v = instanceValues[i];
                    Setter(v, i, value);
                    this[i] = value;
                }
            }

            /// <inheritdoc />
            public override void Set(ValueContainer instanceValues, ValueContainer values)
            {
                if (instanceValues == null || instanceValues.Count != Count)
                    throw new ArgumentException();
                if (values == null || values.Count != Count)
                    throw new ArgumentException();

                for (int i = 0; i < Count; i++)
                {
                    var v = instanceValues[i];
                    var value = ((CustomValueContainer)values)[i];
                    Setter(v, i, value);
                    this[i] = value;
                }
            }

            /// <inheritdoc />
            public override void Set(ValueContainer instanceValues)
            {
                if (instanceValues == null || instanceValues.Count != Count)
                    throw new ArgumentException();

                for (int i = 0; i < Count; i++)
                {
                    var v = instanceValues[i];
                    Setter(v, i, Getter(v, i));
                }
            }

            /// <inheritdoc />
            public override void RefreshReferenceValue(object instanceValue)
            {
                // Not supported
            }
        }

        private sealed class PropertiesProxyEditor : CustomEditor
        {
            private PropertiesProxy _proxy;
            private ComboBox _addParamType;

            private static readonly Type[] AllowedTypes =
            {
                typeof(float),
                typeof(bool),
                typeof(int),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Color),
                typeof(Quaternion),
                typeof(Transform),
                typeof(BoundingBox),
                typeof(BoundingSphere),
                typeof(Rectangle),
                typeof(Matrix),
                typeof(string),
            };

            public override void Initialize(LayoutElementsContainer layout)
            {
                _proxy = (PropertiesProxy)Values[0];
                if (_proxy?.DefaultValues == null)
                {
                    layout.Label("Loading...", TextAlignment.Center);
                    return;
                }

                foreach (var e in _proxy.DefaultValues)
                {
                    // TODO: editing value
                    // TODO: copy/paste values

                    var name = e.Key;
                    var valueContainer = new VariableValueContainer(_proxy, name, e.Value);
                    var propertyLabel = new ClickablePropertyNameLabel(name)
                    {
                        Tag = name,
                    };
                    propertyLabel.MouseLeftDoubleClick += (label, location) => StartParameterRenaming(name, label);
                    propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(name, label, ref location);
                    var property = layout.AddPropertyItem(propertyLabel);
                    property.Object(valueContainer);
                }

                // TODO: improve the UI
                layout.Space(40);
                var addParamType = layout.ComboBox().ComboBox;
                addParamType.Items = AllowedTypes.Select(CustomEditorsUtil.GetTypeNameUI).ToList();
                addParamType.SelectedIndex = 0;
                _addParamType = addParamType;
                var addParamButton = layout.Button("Add").Button;
                addParamButton.Clicked += OnAddParamButtonClicked;
            }

            public override void Refresh()
            {
                if (_proxy?.Asset != null)
                {
                    // Get the current values
                    _proxy.Values = _proxy.Asset.Values;
                }

                base.Refresh();
            }

            private void OnAddParamButtonClicked()
            {
                AddParameter(AllowedTypes[_addParamType.SelectedIndex]);
            }

            private void ShowParameterMenu(string name, Control label, ref Vector2 targetLocation)
            {
                var contextMenu = new ContextMenu();
                contextMenu.AddButton("Rename", () => StartParameterRenaming(name, label));
                contextMenu.AddButton("Delete", () => DeleteParameter(name));
                contextMenu.Show(label, targetLocation);
            }

            private void AddParameter(Type type)
            {
                var asset = _proxy?.Asset;
                if (asset == null || asset.WaitForLoaded())
                    return;
                var action = new AddRemoveParamAction
                {
                    Proxy = _proxy,
                    IsAdd = true,
                    Name = StringUtils.IncrementNameNumber("New parameter", x => OnParameterRenameValidate(null, x)),
                    DefaultValue = Utilities.Utils.GetDefaultValue(type),
                };
                _proxy.Window.Undo.AddAction(action);
                action.Do();
            }

            private void StartParameterRenaming(string name, Control label)
            {
                var dialog = RenamePopup.Show(label, new Rectangle(0, 0, label.Width - 2, label.Height), name, false);
                dialog.Tag = name;
                dialog.Validate += OnParameterRenameValidate;
                dialog.Renamed += OnParameterRenamed;
            }

            private bool OnParameterRenameValidate(RenamePopup popup, string value)
            {
                return !string.IsNullOrWhiteSpace(value) && !_proxy.DefaultValues.ContainsKey(value);
            }

            private void OnParameterRenamed(RenamePopup renamePopup)
            {
                var name = (string)renamePopup.Tag;
                var action = new RenameParamAction
                {
                    Proxy = _proxy,
                    Before = name,
                    After = renamePopup.Text,
                };
                _proxy.Window.Undo.AddAction(action);
                action.Do();
            }

            private void DeleteParameter(string name)
            {
                var action = new AddRemoveParamAction
                {
                    Proxy = _proxy,
                    IsAdd = false,
                    Name = name,
                };
                _proxy.Window.Undo.AddAction(action);
                action.Do();
            }
        }

        private CustomEditorPresenter _propertiesEditor;
        private PropertiesProxy _proxy;
        private ToolStripButton _saveButton;
        private ToolStripButton _undoButton;
        private ToolStripButton _redoButton;
        private ToolStripButton _resetButton;
        private Undo _undo;

        /// <summary>
        /// Gets the undo for asset editing actions.
        /// </summary>
        public Undo Undo => _undo;

        /// <inheritdoc />
        public GameplayGlobalsWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            _undo = new Undo();
            _undo.ActionDone += OnUndo;
            _undo.UndoDone += OnUndo;
            _undo.RedoDone += OnUndo;
            _propertiesEditor = new CustomEditorPresenter(_undo);
            _propertiesEditor.Panel.Parent = this;
            _propertiesEditor.Modified += MarkAsEdited;
            _proxy = new PropertiesProxy();
            _propertiesEditor.Select(_proxy);

            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save asset");
            _toolstrip.AddSeparator();
            _undoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Undo32, _undo.PerformUndo).LinkTooltip("Undo (Ctrl+Z)");
            _redoButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Redo32, _undo.PerformRedo).LinkTooltip("Redo (Ctrl+Y)");
            _toolstrip.AddSeparator();
            _resetButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Rotate32, Reset).LinkTooltip("Resets the variables values to the default values");

            InputActions.Add(options => options.Save, Save);
            InputActions.Add(options => options.Undo, _undo.PerformUndo);
            InputActions.Add(options => options.Redo, _undo.PerformRedo);
        }

        private void OnUndo(IUndoAction action)
        {
            UpdateToolstrip();
            MarkAsEdited();
        }

        private void Reset()
        {
            _asset.ResetValues();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _undo.Clear();
            _proxy.Init(this);
            _propertiesEditor.BuildLayoutOnUpdate();
            UpdateToolstrip();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _undo.Dispose();

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;
            _undoButton.Enabled = _undo.CanUndo;
            _redoButton.Enabled = _undo.CanRedo;
            _resetButton.Enabled = _asset != null;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        public override void Save()
        {
            if (!IsEdited)
                return;

            if (Asset.Save())
            {
                Editor.LogError("Cannot save asset.");
                return;
            }

            ClearEditedFlag();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            _undo = null;
            _propertiesEditor = null;
            _proxy = null;
            _saveButton = null;
            _undoButton = null;
            _redoButton = null;
            _resetButton = null;
        }
    }
}
