// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.GUI;
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

            /// <summary>
            /// The prototypes to use for this node to setup elements based on the parameter type.
            /// </summary>
            public Dictionary<ParameterType, NodeElementArchetype[]> Prototypes = DefaultPrototypes;

            /// <summary>
            /// The default prototypes for thr node elements to use for the given parameter type.
            /// </summary>
            public static readonly Dictionary<ParameterType, NodeElementArchetype[]> DefaultPrototypes = new Dictionary<ParameterType, NodeElementArchetype[]>
            {
                {
                    ParameterType.Bool,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Bool, 0),
                    }
                },
                {
                    ParameterType.Integer,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Integer, 0),
                    }
                },
                {
                    ParameterType.Float,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Float, 0),
                    }
                },
                {
                    ParameterType.Vector2,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector2, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                    }
                },
                {
                    ParameterType.Vector3,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector3, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                    }
                },
                {
                    ParameterType.Vector4,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "W", ConnectionType.Float, 4),
                    }
                },
                {
                    ParameterType.Color,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 4),
                    }
                },
                {
                    ParameterType.Texture,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector2, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.CubeTexture,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector3, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.NormalMap,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector2, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Vector", ConnectionType.Vector3, 1),
                        NodeElementArchetype.Factory.Output(3, "X", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "Y", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "Z", ConnectionType.Float, 4),
                    }
                },
                {
                    ParameterType.RenderTarget,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector2, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.RenderTargetArray,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector3, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.RenderTargetCube,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector3, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.RenderTargetVolume,
                    new[]
                    {
                        NodeElementArchetype.Factory.Input(1, "UVs", true, ConnectionType.Vector3, 0, -1),
                        NodeElementArchetype.Factory.Output(1, "", ConnectionType.Object, 6),
                        NodeElementArchetype.Factory.Output(2, "Color", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "R", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "G", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "B", ConnectionType.Float, 4),
                        NodeElementArchetype.Factory.Output(6, "A", ConnectionType.Float, 5),
                    }
                },
                {
                    ParameterType.Matrix,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Row 0", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "Row 1", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "Row 2", ConnectionType.Vector4, 2),
                        NodeElementArchetype.Factory.Output(4, "Row 3", ConnectionType.Vector4, 3),
                    }
                },
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
                    if (Prototypes.TryGetValue(selected.Type, out var elements))
                    {
                        for (var i = 0; i < elements.Length; i++)
                        {
                            var element = AddElement(elements[i]);
                            _dynamicChildren.Add(element);
                        }
                        height += Mathf.Max(0, elements.Length - 2) * 20.0f;
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

            /// <inheritdoc />
            public void OnParamCreated(SurfaceParameter param)
            {
                UpdateCombo();
            }

            /// <inheritdoc />
            public void OnParamRenamed(SurfaceParameter param)
            {
                UpdateCombo();
                UpdateTitle();
            }

            /// <inheritdoc />
            public void OnParamDeleted(SurfaceParameter param)
            {
                // Deselect if that parameter is selected
                if ((Guid)Values[0] == param.ID)
                {
                    _combobox.SelectedIndex = -1;
                }

                UpdateCombo();
            }

            /// <inheritdoc />
            public override void OnLoaded()
            {
                base.OnLoaded();

                UpdateCombo();
                UpdateLayout();
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateTitle();
            }

            /// <inheritdoc />
            public override void OnValuesChanged()
            {
                base.OnValuesChanged();

                UpdateCombo();
                UpdateLayout();
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
        /// Surface node type for parameters group Get node for Particle Emitter graph.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class SurfaceNodeParamsGetParticleEmitter : SurfaceNodeParamsGet
        {
            /// <summary>
            /// The <see cref="SurfaceNodeParamsGet.DefaultPrototypes"/> implementation for Particle Emitter graph.
            /// </summary>
            public static readonly Dictionary<ParameterType, NodeElementArchetype[]> DefaultPrototypesParticleEmitter = new Dictionary<ParameterType, NodeElementArchetype[]>
            {
                {
                    ParameterType.Bool,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Bool, 0),
                    }
                },
                {
                    ParameterType.Integer,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Integer, 0),
                    }
                },
                {
                    ParameterType.Float,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Float, 0),
                    }
                },
                {
                    ParameterType.Vector2,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector2, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                    }
                },
                {
                    ParameterType.Vector3,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector3, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                    }
                },
                {
                    ParameterType.Vector4,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Value", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "X", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "Y", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "Z", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "W", ConnectionType.Float, 4),
                    }
                },
                {
                    ParameterType.Color,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Color", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "R", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(3, "G", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(4, "B", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(5, "A", ConnectionType.Float, 4),
                    }
                },
                {
                    ParameterType.Texture,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, string.Empty, ConnectionType.Object, 0),
                    }
                },
                {
                    ParameterType.RenderTarget,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, string.Empty, ConnectionType.Object, 0),
                    }
                },
                {
                    ParameterType.RenderTargetArray,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, string.Empty, ConnectionType.Object, 0),
                    }
                },
                {
                    ParameterType.RenderTargetCube,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, string.Empty, ConnectionType.Object, 0),
                    }
                },
                {
                    ParameterType.RenderTargetVolume,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, string.Empty, ConnectionType.Object, 0),
                    }
                },
                {
                    ParameterType.Matrix,
                    new[]
                    {
                        NodeElementArchetype.Factory.Output(1, "Row 0", ConnectionType.Vector4, 0),
                        NodeElementArchetype.Factory.Output(2, "Row 1", ConnectionType.Vector4, 1),
                        NodeElementArchetype.Factory.Output(3, "Row 2", ConnectionType.Vector4, 2),
                        NodeElementArchetype.Factory.Output(4, "Row 3", ConnectionType.Vector4, 3),
                    }
                },
            };

            /// <inheritdoc />
            public SurfaceNodeParamsGetParticleEmitter(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                Prototypes = DefaultPrototypesParticleEmitter;
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
                Create = (id, context, arch, groupArch) => new SurfaceNodeParamsGet(id, context, arch, groupArch),
                Title = "Get Parameter",
                Description = "Parameter value getter",
                Flags = NodeFlags.MaterialGraph | NodeFlags.AnimGraph,
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
            new NodeArchetype
            {
                TypeID = 2,
                Create = (id, context, arch, groupArch) => new SurfaceNodeParamsGetParticleEmitter(id, context, arch, groupArch),
                Title = "Get Parameter",
                Description = "Parameter value getter",
                Flags = NodeFlags.ParticleEmitterGraph,
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
