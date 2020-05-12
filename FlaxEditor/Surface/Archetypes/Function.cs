// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
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
            private Box[] _inputs;
            private Box[] _outputs;
            private Asset _asset; // Keep reference to the asset to keep it loaded and handle function signature changes reload event
            private bool _isRegistered;

            public static int MaxInputs = 16;
            public static int MaxOutputs = 16;

            protected FunctionNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            protected abstract Asset LoadSignature(Guid id, out int[] types, out string[] names);

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                FlaxEngine.Content.AssetReloading += OnAssetReloading;
                FlaxEngine.Content.AssetDisposing += OnContentAssetDisposing;
                _isRegistered = true;

                _assetPicker = GetChild<AssetPicker>();
                _assetPicker.Bounds = new Rectangle(4, 32.0f, Width - 8, 48.0f);
                UpdateUI();
                _assetPicker.SelectedItemChanged += OnAssetPickerSelectedItemChanged;
            }

            private void OnContentAssetDisposing(Asset asset)
            {
                // Ensure to clear reference if need to
                if (asset == _asset)
                    _asset = null;
            }

            private void OnAssetReloading(Asset asset)
            {
                // Update when used function gets modified (signature might be modified)
                if (_asset == asset)
                {
                    UpdateUI();
                    Surface.MarkAsEdited();
                }
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

            private void TryRestoreConnections(Box box, Box[] prevBoxes, ref NodeElementArchetype arch)
            {
                if (prevBoxes == null)
                    return;

                for (int j = 0; j < prevBoxes.Length; j++)
                {
                    var prevBox = prevBoxes[j];
                    if (prevBox != null &&
                        prevBox.HasAnyConnection &&
                        prevBox.Archetype.Text == arch.Text &&
                        box.CanUseType(prevBox.Connections[0].CurrentType))
                    {
                        box.Connections.AddRange(prevBox.Connections);
                        prevBox.Connections.Clear();
                        foreach (var connection in box.Connections)
                        {
                            connection.Connections.Remove(prevBox);
                            connection.Connections.Add(box);
                        }
                        box.ConnectionTick();
                        prevBox.ConnectionTick();
                        foreach (var connection in box.Connections)
                            connection.ConnectionTick();
                        break;
                    }
                }
            }

            private void UpdateUI()
            {
                var prevInputs = _inputs;
                var prevOutputs = _outputs;
                float width, height;

                // Extract function signature parameters (inputs and outputs packed)
                _asset = LoadSignature(_assetPicker.SelectedID, out var types, out var names);
                if (types != null && names != null)
                {
                    // Count inputs and outputs
                    int inputsCount = 0;
                    for (var i = 0; i < MaxInputs; i++)
                    {
                        if (!string.IsNullOrEmpty(names[i]))
                            inputsCount++;
                    }
                    int outputsCount = 0;
                    for (var i = MaxInputs; i < MaxInputs + MaxOutputs; i++)
                    {
                        if (!string.IsNullOrEmpty(names[i]))
                            outputsCount++;
                    }

                    // Inputs
                    _inputs = new Box[inputsCount];
                    for (var i = 0; i < inputsCount; i++)
                    {
                        var arch = NodeElementArchetype.Factory.Input(i + 3, names[i], true, (ConnectionType)types[i], i);
                        var box = new InputBox(this, arch);
                        TryRestoreConnections(box, prevInputs, ref arch);
                        _inputs[i] = box;
                    }

                    // Outputs
                    _outputs = new Box[outputsCount];
                    for (var i = 0; i < outputsCount; i++)
                    {
                        var arch = NodeElementArchetype.Factory.Output(i + 3, names[i + MaxInputs], (ConnectionType)types[i + MaxInputs], i + MaxInputs);
                        var box = new OutputBox(this, arch);
                        TryRestoreConnections(box, prevOutputs, ref arch);
                        _outputs[i] = box;
                    }

                    Title = _assetPicker.SelectedItem.ShortName;
                    var style = Style.Current;
                    width = Mathf.Max(Archetype.Size.X, style.FontLarge.MeasureText(Title).X + 30);
                    height = 60.0f + Mathf.Max(inputsCount, outputsCount) * 20.0f;
                }
                else
                {
                    _inputs = null;
                    _outputs = null;
                    Title = Archetype.Title;
                    width = Archetype.Size.X;
                    height = 60.0f;
                }

                // Remove previous boxes
                if (prevInputs != null)
                {
                    for (int i = 0; i < prevInputs.Length; i++)
                        RemoveElement(prevInputs[i]);
                }
                if (prevOutputs != null)
                {
                    for (int i = 0; i < prevOutputs.Length; i++)
                        RemoveElement(prevOutputs[i]);
                }

                // Add new boxes
                if (_inputs != null)
                {
                    for (int i = 0; i < _inputs.Length; i++)
                        AddElement(_inputs[i]);
                }
                if (_outputs != null)
                {
                    for (int i = 0; i < _outputs.Length; i++)
                        AddElement(_outputs[i]);
                }

                Resize(width, height);
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] is OutputBox box)
                    {
                        box.Location = box.Archetype.Position + new Vector2(width, 0);
                    }
                }
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _assetPicker = null;
                _asset = null;
                if (_isRegistered)
                {
                    _isRegistered = false;
                    FlaxEngine.Content.AssetReloading -= OnAssetReloading;
                    FlaxEngine.Content.AssetDisposing -= OnContentAssetDisposing;
                }

                base.OnDestroy();
            }
        }

        private class FunctionInputOutputNode : SurfaceNode
        {
            protected Label _nameField;

            /// <summary>
            /// Gets or sets the function input/output name.
            /// </summary>
            public string SignatureName
            {
                get => (string)Values[1];
                set
                {
                    if (!string.Equals(value, (string)Values[1], StringComparison.Ordinal))
                    {
                        SetValue(1, value);
                        _nameField.Text = value;
                    }
                }
            }

            /// <inheritdoc />
            public FunctionInputOutputNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                _nameField = new Label
                {
                    Width = 140.0f,
                    TextColorHighlighted = Style.Current.ForegroundGrey,
                    HorizontalAlignment = TextAlignment.Near,
                    Parent = this,
                };
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                _nameField.Text = SignatureName;
            }

            /// <inheritdoc />
            public override void OnSpawned()
            {
                base.OnSpawned();

                // Ensure to have unique name
                var name = SignatureName;
                var value = name;
                int count = 1;
                while (!OnRenameValidate(null, value))
                {
                    value = name + " " + count++;
                }
                Values[1] = value;
                _nameField.Text = value;

                // Let user pick a name
                StartRenaming();
            }

            /// <inheritdoc />
            public override bool OnMouseDoubleClick(Vector2 location, MouseButton button)
            {
                if (base.OnMouseDoubleClick(location, button))
                    return true;

                if (_nameField.Bounds.Contains(ref location))
                {
                    StartRenaming();
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public override void OnShowSecondaryContextMenu(FlaxEditor.GUI.ContextMenu.ContextMenu menu, Vector2 location)
            {
                base.OnShowSecondaryContextMenu(menu, location);

                menu.AddButton("Rename", StartRenaming);
            }

            /// <summary>
            /// Starts the function input/output parameter renaming by showing a rename popup to the user.
            /// </summary>
            private void StartRenaming()
            {
                Surface.Select(this);
                var dialog = RenamePopup.Show(this, _nameField.Bounds, SignatureName, false);
                dialog.Validate += OnRenameValidate;
                dialog.Renamed += OnRenamed;
            }

            private bool OnRenameValidate(RenamePopup popup, string value)
            {
                if (string.IsNullOrEmpty(value))
                    return false;
                return Context.Nodes.All(node =>
                {
                    if (node != this && node is FunctionInputOutputNode inputOutputNode)
                        return inputOutputNode.SignatureName != value;
                    return true;
                });
            }

            private void OnRenamed(RenamePopup renamePopup)
            {
                SignatureName = renamePopup.Text;
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                _nameField.Text = SignatureName;
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _nameField = null;

                base.OnDestroy();
            }
        }

        private sealed class FunctionInputNode : FunctionInputOutputNode
        {
            private ConnectionType[] _types;
            private ComboBox _typePicker;
            private Box _outputBox;
            private Box _defaultValueBox;

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
                _nameField.Location = new Vector2(_typePicker.Right + 2.0f, _typePicker.Y);
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
            }

            private void OnTypePickerSelectedIndexChanged(ComboBox picker)
            {
                SetValue(0, (int)_types[picker.SelectedIndex]);
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                _outputBox.CurrentType = (ConnectionType)(int)Values[0];
                _defaultValueBox.CurrentType = _outputBox.CurrentType;
                _typePicker.SelectedIndex = Array.IndexOf(_types, _outputBox.CurrentType);
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _types = null;
                _typePicker = null;
                _outputBox = null;
                _defaultValueBox = null;

                base.OnDestroy();
            }
        }

        private sealed class FunctionOutputNode : FunctionInputOutputNode
        {
            private ConnectionType[] _types;
            private ComboBox _typePicker;
            private Box _inputBox;

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
                _nameField.Location = new Vector2(_typePicker.Right + 2.0f, _typePicker.Y);
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                _inputBox = GetBox(0);
                _inputBox.CurrentType = (ConnectionType)(int)Values[0];
                _typePicker.SelectedIndex = Array.IndexOf(_types, _inputBox.CurrentType);
                _typePicker.SelectedIndexChanged += OnTypePickerSelectedIndexChanged;
            }

            private void OnTypePickerSelectedIndexChanged(ComboBox picker)
            {
                SetValue(0, (int)_types[picker.SelectedIndex]);
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                _inputBox.CurrentType = (ConnectionType)(int)Values[0];
                _typePicker.SelectedIndex = Array.IndexOf(_types, _inputBox.CurrentType);
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                _types = null;
                _typePicker = null;
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
