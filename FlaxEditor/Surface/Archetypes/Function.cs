// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.GUI;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Function group.
    /// </summary>
    public static class Function
    {
        /// <summary>
        /// The interface for Visject surfaces that can contain function nodes (eg. material functions).
        /// </summary>
        public interface IFunctionSurface
        {
            /// <summary>
            /// Gets the allowed types for function inputs/outputs.
            /// </summary>
            ConnectionType[] FunctionTypes { get; }
        }

        internal abstract class FunctionNode : SurfaceNode
        {
            private AssetPicker _assetPicker;
            private readonly List<ISurfaceNodeElement> _dynamicChildren = new List<ISurfaceNodeElement>();

            /// <inheritdoc />
            public FunctionNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            protected abstract void LoadSignature(Guid id, out int[] types, out string[] names);

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                _assetPicker = GetChild<AssetPicker>();
                _assetPicker.Bounds = new Rectangle(4, 32.0f, Width - 8, 48.0f);
                UpdateUI();
                _assetPicker.SelectedItemChanged += OnAssetPickerSelectedItemChanged;
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                UpdateUI();
            }

            private void OnAssetPickerSelectedItemChanged()
            {
                SetValue(0, _assetPicker.SelectedID);
            }

            private void UpdateUI()
            {
                // Clear existing dynamic UI
                for (int i = 0; i < _dynamicChildren.Count; i++)
                {
                    RemoveElement(_dynamicChildren[i]);
                }
                _dynamicChildren.Clear();

                // Extract function signature parameters (inputs and outputs packed)
                LoadSignature(_assetPicker.SelectedID, out var types, out var names);
                if (types != null && names != null)
                {
                    int inputsCount = 0;
                    int outputsCount = 0;

                    // Inputs
                    for (var i = 0; i < 8; i++)
                    {
                        if (string.IsNullOrEmpty(names[i]))
                            break;

                        var arch = NodeElementArchetype.Factory.Input(inputsCount + 3, names[i], true, (ConnectionType)types[i], i);
                        var element = AddElement(arch);
                        _dynamicChildren.Add(element);
                        inputsCount++;
                    }

                    // Outputs
                    for (var i = 8; i < 16; i++)
                    {
                        if (string.IsNullOrEmpty(names[i]))
                            break;

                        var arch = NodeElementArchetype.Factory.Output(outputsCount + 3, names[i], (ConnectionType)types[i], i);
                        var element = AddElement(arch);
                        _dynamicChildren.Add(element);
                        outputsCount++;
                    }

                    Title = _assetPicker.SelectedItem.ShortName;
                    var style = Style.Current;
                    var width = Mathf.Max(Archetype.Size.X, style.FontLarge.MeasureText(Title).X + 30);
                    var height = 60.0f + Mathf.Max(inputsCount, outputsCount) * 20.0f;
                    Resize(width, height);
                }
                else
                {
                    Resize(Archetype.Size.X, 60.0f);
                    Title = Archetype.Title;
                }
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _assetPicker = null;

                base.OnDestroy();
            }
        }

        private sealed class FunctionInputNode : SurfaceNode
        {
            private ConnectionType[] _types;
            private ComboBox _typePicker;
            private TextBox _nameField;
            private Elements.Box _outputBox;
            private Elements.Box _defaultValueBox;

            /// <inheritdoc />
            public FunctionInputNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                _types = ((IFunctionSurface)Surface).FunctionTypes;
                _typePicker = new ComboBox
                {
                    Location = new Vector2(4, 32),
                    Width = 80.0f,
                    Parent = this,
                };
                for (int i = 0; i < _types.Length; i++)
                    _typePicker.AddItem(Surface.GetConnectionTypeName(_types[i]));
                _nameField = new TextBox
                {
                    Location = new Vector2(_typePicker.Right + 2.0f, _typePicker.Y),
                    Width = 140.0f,
                    Parent = this,
                };
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                _outputBox = GetBox(0);
                _outputBox.CurrentType = (ConnectionType)(int)Values[0];
                _defaultValueBox = GetBox(1);
                _defaultValueBox.CurrentType = _outputBox.CurrentType;
                _typePicker.SelectedIndex = Array.IndexOf(_types, _outputBox.CurrentType);
                _typePicker.SelectedIndexChanged += OnTypePickerSelectedIndexChanged;
                _nameField.Text = (string)Values[1];
                _nameField.TextChanged += OnNameFieldTextChanged;
            }

            private void OnTypePickerSelectedIndexChanged(ComboBox picker)
            {
                SetValue(0, (int)_types[picker.SelectedIndex]);
            }

            private void OnNameFieldTextChanged()
            {
                SetValue(1, _nameField.Text);
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                _outputBox.CurrentType = (ConnectionType)(int)Values[0];
                _defaultValueBox.CurrentType = _outputBox.CurrentType;
                _typePicker.SelectedIndex = Array.IndexOf(_types, _outputBox.CurrentType);
                _nameField.Text = (string)Values[1];
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _types = null;
                _typePicker = null;
                _nameField = null;
                _outputBox = null;
                _defaultValueBox = null;

                base.OnDestroy();
            }
        }

        private sealed class FunctionOutputNode : SurfaceNode
        {
            private ConnectionType[] _types;
            private ComboBox _typePicker;
            private TextBox _nameField;
            private Elements.Box _inputBox;

            /// <inheritdoc />
            public FunctionOutputNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                _types = ((IFunctionSurface)Surface).FunctionTypes;
                _typePicker = new ComboBox
                {
                    Location = new Vector2(24, 32),
                    Width = 80.0f,
                    Parent = this,
                };
                for (int i = 0; i < _types.Length; i++)
                    _typePicker.AddItem(Surface.GetConnectionTypeName(_types[i]));
                _nameField = new TextBox
                {
                    Location = new Vector2(_typePicker.Right + 2.0f, _typePicker.Y),
                    Width = 140.0f,
                    Parent = this,
                };
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                _inputBox = GetBox(0);
                _inputBox.CurrentType = (ConnectionType)(int)Values[0];
                _typePicker.SelectedIndex = Array.IndexOf(_types, _inputBox.CurrentType);
                _typePicker.SelectedIndexChanged += OnTypePickerSelectedIndexChanged;
                _nameField.Text = (string)Values[1];
                _nameField.TextChanged += OnNameFieldTextChanged;
            }

            private void OnTypePickerSelectedIndexChanged(ComboBox picker)
            {
                SetValue(0, (int)_types[picker.SelectedIndex]);
            }

            private void OnNameFieldTextChanged()
            {
                SetValue(1, _nameField.Text);
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                _inputBox.CurrentType = (ConnectionType)(int)Values[0];
                _typePicker.SelectedIndex = Array.IndexOf(_types, _inputBox.CurrentType);
                _nameField.Text = (string)Values[1];
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _types = null;
                _typePicker = null;
                _nameField = null;
                _inputBox = null;

                base.OnDestroy();
            }
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Create = (id, context, arch, groupArch) => new FunctionInputNode(id, context, arch, groupArch),
                Title = "Function Input",
                Description = "The graph function input data",
                Flags = NodeFlags.AllGraphs | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(240, 60),
                DefaultValues = new object[]
                {
                    (int)ConnectionType.Float,
                    "Input",
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Input(1.5f, "Default Value", true, ConnectionType.Float, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Create = (id, context, arch, groupArch) => new FunctionOutputNode(id, context, arch, groupArch),
                Title = "Function Output",
                Description = "The graph function output data",
                Flags = NodeFlags.AllGraphs | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(240, 60),
                DefaultValues = new object[]
                {
                    (int)ConnectionType.Float,
                    "Output",
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Float, 0),
                }
            },
        };
    }
}
