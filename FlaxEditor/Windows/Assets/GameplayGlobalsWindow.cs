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

        [CustomEditor(typeof(PropertiesProxyEditor))]
        private sealed class PropertiesProxy
        {
            public GameplayGlobalsWindow Window;
            public GameplayGlobals Asset;
            public Dictionary<string, object> DefaultValues;
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

            /// <inheritdoc />
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
                    // TODO: editing default value
                    // TODO: editing value
                    // TODO: renaming variables
                    // TODO: removing variable
                    // TODO: copy/paste values

                    var name = e.Key;
                    var valueContainer = new VariableValueContainer(_proxy, name, e.Value);
                    var propertyLabel = new ClickablePropertyNameLabel(name)
                    {
                        Tag = name,
                    };
                    propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(name, label, ref location);
                    var property = layout.AddPropertyItem(propertyLabel);
                    property.Object(valueContainer);
                }

                // TODO: improve the UI
                layout.Space(40);
                var addParamType = layout.ComboBox().ComboBox;
                addParamType.Items = AllowedTypes.Select(x => CustomEditorsUtil.GetTypeNameUI(x)).ToList();
                addParamType.SelectedIndex = 0;
                _addParamType = addParamType;
                var addParamButton = layout.Button("Add").Button;
                addParamButton.Clicked += OnAddParamButtonClicked;
            }

            private void OnAddParamButtonClicked()
            {
                AddParameter(AllowedTypes[_addParamType.SelectedIndex]);
            }

            /// <inheritdoc />
            public override void Refresh()
            {
                if (_proxy?.Asset != null)
                {
                    // Get the current values
                    _proxy.Values = _proxy.Asset.Values;
                }

                base.Refresh();
            }

            /// <summary>
            /// Shows the parameter context menu.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="label">The label control.</param>
            /// <param name="targetLocation">The target location.</param>
            private void ShowParameterMenu(string name, Control label, ref Vector2 targetLocation)
            {
                var contextMenu = new ContextMenu();
                contextMenu.AddButton("Delete", () => DeleteParameter(name));
                contextMenu.Show(label, targetLocation);
            }

            /// <summary>
            /// Adds the parameter.
            /// </summary>
            /// <param name="type">The type.</param>
            private void AddParameter(Type type)
            {
                var material = _proxy?.Asset;
                if (material == null || !material.IsLoaded)
                    return;
                var action = new AddRemoveParamAction
                {
                    Proxy = _proxy,
                    IsAdd = true,
                    Name = "New parameter",
                    DefaultValue = Utilities.Utils.GetDefaultValue(type),
                };
                _proxy.Window.Undo.AddAction(action);
                action.Do();
            }
            
            /// <summary>
            /// Removes the parameter.
            /// </summary>
            /// <param name="name">The name.</param>
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
