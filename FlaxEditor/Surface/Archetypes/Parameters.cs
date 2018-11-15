// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Parameters group.
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Surface node type for parameters group Get node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class SurfaceNodeParamsGet : SurfaceNode, IParametersDependantNode
        {
            private ComboBoxElement _combobox;
            private readonly List<ISurfaceNodeElement> _dynamicChildren = new List<ISurfaceNodeElement>();
            private bool _isUpdateLocked;
            private float _layoutHeight;
            private ParameterType _layoutType;

            static NodeElementArchetype[] Prototypes =
            {
                // 0: Bool
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Bool, 0),
                // 1: Integer
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Integer, 0),
                // 2: Float
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Float, 0),
                // 3: Vector2
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector2, 0),
                NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                // 6: Vector3
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector3, 0),
                NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                // 10: Vector4
                NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector4, 0),
                NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                NodeElementArchetype.Factory.Output(5, "W", ConnectionType.Float, 4),
                // 15: Color
                NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 0),
                NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 1),
                NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 2),
                NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 3),
                NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 4),
                // 20: Texture/Cube Texture/Render Target
                NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector2, 0, -1),
                NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                // 27: Normal Map
                NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector2, 0, -1),
                NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                NodeElementArchetype.Factory.Output(2, "Normal", ConnectionType.Vector3, 1),
                NodeElementArchetype.Factory.Output(3, "X", ConnectionType.Float, 2),
                NodeElementArchetype.Factory.Output(4, "Y", ConnectionType.Float, 3),
                NodeElementArchetype.Factory.Output(5, "Z", ConnectionType.Float, 4),
                // 33: Matrix
                NodeElementArchetype.Factory.Output(1, "Row0", ConnectionType.Vector4, 0),
                NodeElementArchetype.Factory.Output(2, "Row1", ConnectionType.Vector4, 1),
                NodeElementArchetype.Factory.Output(3, "Row2", ConnectionType.Vector4, 2),
                NodeElementArchetype.Factory.Output(4, "Row3", ConnectionType.Vector4, 3),
            };

            /// <inheritdoc />
            public SurfaceNodeParamsGet(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                // Force first layout init for every param type
                _layoutType = (ParameterType)int.MaxValue;
            }

            private void UpdateLayout()
            {
                // Add elements and calculate node size if type changes
                float height = 60;
                var selected = GetSelected();
                if (selected != null && _layoutType != selected.Type)
                {
                    // Clean
                    ClearDynamicElements();

                    // Build layout
                    switch (selected.Type)
                    {
                    case ParameterType.Bool:
                        AddOutput(Prototypes[0]);
                        break;
                    case ParameterType.Integer:
                        AddOutput(Prototypes[1]);
                        break;
                    case ParameterType.Float:
                        AddOutput(Prototypes[2]);
                        break;
                    case ParameterType.Vector2:
                        AddOutput(Prototypes[3]);
                        AddOutput(Prototypes[4]);
                        AddOutput(Prototypes[5]);
                        height = 80;
                        break;
                    case ParameterType.Vector3:
                        AddOutput(Prototypes[6]);
                        AddOutput(Prototypes[7]);
                        AddOutput(Prototypes[8]);
                        AddOutput(Prototypes[9]);
                        height = 100;
                        break;
                    case ParameterType.Vector4:
                        AddOutput(Prototypes[11]);
                        AddOutput(Prototypes[12]);
                        AddOutput(Prototypes[13]);
                        AddOutput(Prototypes[14]);
                        AddOutput(Prototypes[15]);
                        height = 120;
                        break;
                    case ParameterType.Color:
                        AddOutput(Prototypes[15]);
                        AddOutput(Prototypes[16]);
                        AddOutput(Prototypes[17]);
                        AddOutput(Prototypes[18]);
                        AddOutput(Prototypes[19]);
                        height = 120;
                        break;
                    case ParameterType.Texture:
                    case ParameterType.CubeTexture:
                    case ParameterType.RenderTarget:
                    case ParameterType.RenderTargetArray:
                    case ParameterType.RenderTargetCube:
                    case ParameterType.RenderTargetVolume:
                        AddInput(Prototypes[20]);
                        AddOutput(Prototypes[21]);
                        AddOutput(Prototypes[22]);
                        AddOutput(Prototypes[23]);
                        AddOutput(Prototypes[24]);
                        AddOutput(Prototypes[25]);
                        AddOutput(Prototypes[26]);
                        height = 140;
                        break;
                    case ParameterType.NormalMap:
                        AddInput(Prototypes[27]);
                        AddOutput(Prototypes[28]);
                        AddOutput(Prototypes[29]);
                        AddOutput(Prototypes[30]);
                        AddOutput(Prototypes[31]);
                        AddOutput(Prototypes[32]);
                        height = 140;
                        break;
                    case ParameterType.Matrix:
                        AddOutput(Prototypes[33]);
                        AddOutput(Prototypes[34]);
                        AddOutput(Prototypes[35]);
                        AddOutput(Prototypes[36]);
                        height = 100;
                        break;

                    // TODO: finish this
                    case ParameterType.String: break;
                    case ParameterType.Box: break;
                    case ParameterType.Rotation: break;
                    case ParameterType.Transform: break;
                    case ParameterType.Asset: break;
                    case ParameterType.Actor: break;
                    case ParameterType.Rectangle: break;
                    default: break;
                    }

                    // Cache state
                    _layoutType = selected.Type;
                    _layoutHeight = height;
                }

                UpdateTitle();
            }

            private void UpdateCombo()
            {
                // Check if is locked
                if (_isUpdateLocked)
                    return;

                // Lock
                _isUpdateLocked = true;

                // Cache combo box control
                if (_combobox == null)
                {
                    _combobox = (ComboBoxElement)_children[0];
                    _combobox.SelectedIndexChanged += OnSelectedChanged;
                }

                // Update items
                int toSelect = -1;
                Guid loadedSelected = (Guid)Values[0];
                _combobox.ClearItems();
                int index = 0;
                for (int i = 0; i < Surface.Parameters.Count; i++)
                {
                    var param = Surface.Parameters[i];
                    if (param.IsUIVisible)
                    {
                        _combobox.AddItem(param.Name);

                        if (param.ID == loadedSelected)
                        {
                            toSelect = index;
                        }

                        index++;
                    }
                }
                _combobox.SelectedIndex = toSelect;

                // Unlock
                _isUpdateLocked = false;
            }

            private void OnSelectedChanged(ComboBox cb)
            {
                // Check if is locked
                if (_isUpdateLocked)
                    return;

                var selected = GetSelected();
                Guid selectedID = selected != null ? selected.ID : Guid.Empty;
                if (selectedID != (Guid)Values[0])
                {
                    SetValue(0, selectedID);
                    UpdateLayout();
                }
            }

            private SurfaceParameter GetSelected()
            {
                SurfaceParameter result = null;
                int index = 0;
                for (int i = 0; i < Surface.Parameters.Count; i++)
                {
                    var param = Surface.Parameters[i];
                    if (param.IsUIVisible)
                    {
                        if (index == _combobox.SelectedIndex)
                        {
                            result = param;
                            break;
                        }

                        index++;
                    }
                }
                return result;
            }

            private void ClearDynamicElements()
            {
                for (int i = 0; i < _dynamicChildren.Count; i++)
                {
                    RemoveElement(_dynamicChildren[i]);
                }
                _dynamicChildren.Clear();
            }

            private void AddInput(NodeElementArchetype arch)
            {
                var element = new InputBox(this, arch);
                AddElement(element);
                _dynamicChildren.Add(element);
            }

            private void AddOutput(NodeElementArchetype arch)
            {
                var element = new OutputBox(this, arch);
                AddElement(element);
                _dynamicChildren.Add(element);
            }

            /// <inheritdoc />
            public void OnParamCreated(SurfaceParameter param)
            {
                // Update
                UpdateCombo();
            }

            /// <inheritdoc />
            public void OnParamRenamed(SurfaceParameter param)
            {
                // Update
                UpdateCombo();
                UpdateTitle();
            }

            /// <inheritdoc />
            public void OnParamDeleted(SurfaceParameter param)
            {
                // Check if that parameter is selected
                if ((Guid)Values[0] == param.ID)
                {
                    // Deselect
                    _combobox.SelectedIndex = -1;
                }

                // Update
                UpdateCombo();
            }

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                // Setup
                UpdateCombo();
                UpdateLayout();
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                // Setup
                UpdateTitle();
            }

            private void UpdateTitle()
            {
                if (_layoutHeight < 10)
                    return;

                var selected = GetSelected();
                Title = selected != null ? "Get " + selected.Name : "Get Parameter";

                var style = Style.Current;
                var width = Mathf.Max(140, style.FontLarge.MeasureText(Title).X + 30);
                Resize(width, _layoutHeight);
            }
        }

        /// <summary>
        /// Creates the get node.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="context">The surface.</param>
        /// <param name="arch">The node archetype.</param>
        /// <param name="groupArch">The group archetype.</param>
        /// <returns>The created node.</returns>
        public static SurfaceNode CreateGetNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new SurfaceNodeParamsGet(id, context, arch, groupArch);
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Create = CreateGetNode,
                Title = "Get Parameter",
                Description = "Parameter value getter",
                Size = new Vector2(140, 60),
                DefaultValues = new object[]
                {
                    Guid.Empty
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(2, 0, 116)
                }
            },
        };
    }
}
