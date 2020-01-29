// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
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
        [CustomEditor(typeof(PropertiesProxyEditor))]
        private sealed class PropertiesProxy
        {
            public GameplayGlobals Asset;
            public Dictionary<string, object> DefaultValues;
            public Dictionary<string, object> Values;

            public void Set(GameplayGlobals asset)
            {
                Asset = asset;
                DefaultValues = asset.DefaultValues;
                Values = asset.Values;
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
                    // TODO: copy/paste values

                    var valueContainer = new VariableValueContainer(_proxy, e.Key, e.Value);
                    layout.Property(e.Key, valueContainer);
                }

                // TODO: finish this
                layout.Space(40);
                var addParamPanel = layout.Space(30).ContainerControl;
                var addParamButton = addParamPanel.AddChild<Button>();
                addParamButton.AnchorStyle = AnchorStyle.CenterRight;
                addParamButton.Width = 50.0f;
                addParamButton.Text = "Add";
                addParamButton.Clicked += OnAddParamButtonClicked;
                var addParamType = addParamPanel.AddChild<ComboBox>();
                addParamType.AnchorStyle = AnchorStyle.CenterLeft;
                addParamType.Width = addParamPanel.Width - addParamButton.Width - 4.0f;
                addParamType.Items = AllowedTypes.Select(x => x.ToString()).ToList();
                addParamType.SelectedIndex = 0;
                _addParamType = addParamType;
            }

            private void OnAddParamButtonClicked()
            {
                // TODO: impl this
                MessageBox.Show("Add " + AllowedTypes[_addParamType.SelectedIndex]);
            }

            /// <inheritdoc />
            public override void Refresh()
            {
                if (_proxy?.Asset != null)
                {
                    _proxy.DefaultValues = _proxy.Asset.DefaultValues;
                    _proxy.Values = _proxy.Asset.Values;
                }

                base.Refresh();
            }
        }

        private CustomEditorPresenter _propertiesEditor;
        private PropertiesProxy _proxy;
        private ToolStripButton _saveButton;
        private ToolStripButton _resetButton;
        private Undo _undo;

        /// <inheritdoc />
        public GameplayGlobalsWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            _undo = new Undo();
            _propertiesEditor = new CustomEditorPresenter(_undo);
            _propertiesEditor.Panel.Parent = this;
            _propertiesEditor.Modified += MarkAsEdited;
            _proxy = new PropertiesProxy();
            _propertiesEditor.Select(_proxy);
            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save asset");
            _resetButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Rotate32, Reset).LinkTooltip("Resets the variables values to the default values");
        }

        private void Reset()
        {
            _asset.ResetValues();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _undo.Clear();
            _proxy.Set(Asset);
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
            _resetButton = null;
        }
    }
}
