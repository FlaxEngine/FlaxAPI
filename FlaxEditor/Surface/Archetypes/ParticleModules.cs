// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Particle Modules group.
    /// </summary>
    public static class ParticleModules
    {
        /// <summary>
        /// The particle emitter module types.
        /// </summary>
        public enum ModuleType
        {
            /// <summary>
            /// The spawn module.
            /// </summary>
            Spawn,

            /// <summary>
            /// The init module.
            /// </summary>
            Initialize,

            /// <summary>
            /// The update module.
            /// </summary>
            Update,

            /// <summary>
            /// The render module.
            /// </summary>
            Render,
        }

        /// <summary>
        /// The sprite rendering facing modes.
        /// </summary>
        public enum ParticleSpriteFacingMode
        {
            /// <summary>
            /// Particles will face camera position.
            /// </summary>
            FaceCameraPosition,

            /// <summary>
            /// Particles will face camera plane.
            /// </summary>
            FaceCameraPlane,

            /// <summary>
            /// Particles will orient along velocity vector.
            /// </summary>
            AlongVelocity,

            /// <summary>
            /// Particles will use the custom vector for facing.
            /// </summary>
            CustomFacingVector,
        }

        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for particle emitter module node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class ParticleModuleNode : SurfaceNode
        {
            private static readonly Color[] Colors =
            {
                Color.ForestGreen,
                Color.GreenYellow,
                Color.Violet,
                Color.Firebrick,
            };

            private CheckBox _enabled;
            private Rectangle _arrangeButtonRect;
            private bool _arrangeButtonInUse;

            /// <summary>
            /// Gets the particle emitter surface.
            /// </summary>
            public ParticleEmitterSurface ParticleSurface => (ParticleEmitterSurface)Surface;

            /// <summary>
            /// Gets or sets a value indicating whether the module is enabled.
            /// </summary>
            public bool ModuleEnabled
            {
                get => (bool)Values[0];
                set
                {
                    if (value != (bool)Values[0])
                    {
                        SetValue(0, value);
                        _enabled.State = value ? CheckBoxState.Checked : CheckBoxState.Default;
                    }
                }
            }

            /// <summary>
            /// Gets the type of the module.
            /// </summary>
            public ModuleType ModuleType { get; }

            /// <inheritdoc />
            public ParticleModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                _enabled = new CheckBox(0, 0, true, FlaxEditor.Surface.Constants.NodeCloseButtonSize)
                {
                    TooltipText = "Enabled/disables this module",
                    Parent = this,
                };

                ModuleType = (ModuleType)nodeArch.DefaultValues[1];

                Size = CalculateNodeSize(nodeArch.Size.X, nodeArch.Size.Y);
            }

            private void OnEnabledStateChanged(CheckBox control)
            {
                ModuleEnabled = control.State == CheckBoxState.Checked;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                var style = Style.Current;

                // Header
                var idx = (int)ModuleType;
                var headerRect = new Rectangle(0, 0, Width, 16.0f);
                //Render2D.FillRectangle(headerRect, Color.Red);
                Render2D.DrawText(style.FontMedium, Title, headerRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);

                DrawChildren();

                // Border
                Render2D.DrawRectangle(new Rectangle(1, 0, Width - 2, Height - 1), Colors[idx], 1.5f);

                // Close button
                float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));

                // Arrange button
                alpha = _arrangeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(Editor.Instance.Icons.DragBar12, _arrangeButtonRect, _arrangeButtonInUse ? Color.Orange : new Color(alpha));
                if (_arrangeButtonInUse && ArrangeAreaCheck(out _, out var arrangeTargetRect))
                {
                    Render2D.FillRectangle(arrangeTargetRect, Color.Orange * 0.8f);
                }

                // Disabled overlay
                if (!ModuleEnabled)
                {
                    Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), new Color(0, 0, 0, 0.4f));
                }
            }

            private bool ArrangeAreaCheck(out int index, out Rectangle rect)
            {
                var barSidesExtend = 20.0f;
                var barHeight = 10.0f;
                var barCheckAreaHeight = 40.0f;

                var pos = PointToParent(ref _mousePosition).Y + barCheckAreaHeight * 0.5f;
                var modules = Surface.Nodes.OfType<ParticleModuleNode>().Where(x => x.ModuleType == ModuleType).ToList();
                for (var i = 0; i < modules.Count; i++)
                {
                    var module = modules[i];
                    if (Mathf.IsInRange(pos, module.Top, module.Top + barCheckAreaHeight) || (i == 0 && pos < module.Top))
                    {
                        index = i;
                        var p1 = module.UpperLeft;
                        rect = new Rectangle(PointFromParent(ref p1) - new Vector2(barSidesExtend * 0.5f, barHeight * 0.5f), Width + barSidesExtend, barHeight);
                        return true;
                    }
                }

                var p2 = modules[modules.Count - 1].BottomLeft;
                if (pos > p2.Y)
                {
                    index = modules.Count;
                    rect = new Rectangle(PointFromParent(ref p2) - new Vector2(barSidesExtend * 0.5f, barHeight * 0.5f), Width + barSidesExtend, barHeight);
                    return true;
                }

                index = -1;
                rect = Rectangle.Empty;
                return false;
            }

            /// <inheritdoc />
            public override void OnEndMouseCapture()
            {
                base.OnEndMouseCapture();

                _arrangeButtonInUse = false;
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && _arrangeButtonRect.Contains(ref location))
                {
                    _arrangeButtonInUse = true;
                    Focus();
                    StartMouseCapture();
                    return true;
                }

                return base.OnMouseDown(location, buttons);
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && _arrangeButtonInUse)
                {
                    _arrangeButtonInUse = false;
                    EndMouseCapture();

                    if (ArrangeAreaCheck(out var index, out _))
                    {
                        var modules = Surface.Nodes.OfType<ParticleModuleNode>().Where(x => x.ModuleType == ModuleType).ToList();

                        foreach (var module in modules)
                        {
                            Surface.Nodes.Remove(module);
                        }

                        int oldIndex = modules.IndexOf(this);
                        modules.RemoveAt(oldIndex);
                        if (index < 0 || index >= modules.Count)
                            modules.Add(this);
                        else
                            modules.Insert(index, this);

                        foreach (var module in modules)
                        {
                            Surface.Nodes.Add(module);
                        }

                        foreach (var module in modules)
                        {
                            module.IndexInParent = int.MaxValue;
                        }

                        ParticleSurface.ArrangeModulesNodes();
                        Surface.MarkAsEdited();
                    }
                }

                return base.OnMouseUp(location, buttons);
            }

            /// <inheritdoc />
            public override void OnLostFocus()
            {
                if (_arrangeButtonInUse)
                {
                    _arrangeButtonInUse = false;
                    EndMouseCapture();
                }

                base.OnLostFocus();
            }

            /// <inheritdoc />
            protected sealed override Vector2 CalculateNodeSize(float width, float height)
            {
                return new Vector2(width + FlaxEditor.Surface.Constants.NodeMarginX * 2, height + FlaxEditor.Surface.Constants.NodeMarginY * 2 + 16.0f);
            }

            /// <inheritdoc />
            protected override void UpdateRectangles()
            {
                const float headerSize = 16.0f;
                const float closeButtonMargin = FlaxEditor.Surface.Constants.NodeCloseButtonMargin;
                const float closeButtonSize = FlaxEditor.Surface.Constants.NodeCloseButtonSize;
                _headerRect = new Rectangle(0, 0, Width, headerSize);
                _closeButtonRect = new Rectangle(Width - closeButtonSize - closeButtonMargin, closeButtonMargin, closeButtonSize, closeButtonSize);
                _footerRect = Rectangle.Empty;
                _enabled.Location = new Vector2(_closeButtonRect.X - _enabled.Width - 2, _closeButtonRect.Y);
                _arrangeButtonRect = new Rectangle(_enabled.X - closeButtonSize - closeButtonMargin, closeButtonMargin, closeButtonSize, closeButtonSize);
            }

            /// <inheritdoc />
            public override void OnSpawned()
            {
                base.OnSpawned();

                ParticleSurface.ArrangeModulesNodes();
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                _enabled.Checked = ModuleEnabled;
                _enabled.StateChanged += OnEnabledStateChanged;

                base.OnSurfaceLoaded();
            }

            /// <inheritdoc />
            public override void OnDeleted()
            {
                ParticleSurface.ArrangeModulesNodes();

                base.OnDeleted();
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                _enabled = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// The particle emitter module that can write to the particle attribute.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.Archetypes.ParticleModules.ParticleModuleNode" />
        public class SetParticleAttributeModuleNode : ParticleModuleNode
        {
            /// <inheritdoc />
            public SetParticleAttributeModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateOutputBoxType();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                // Update on type change
                if (index == 3)
                    UpdateOutputBoxType();
            }

            private void UpdateOutputBoxType()
            {
                ConnectionType type;
                switch ((Particles.ValueTypes)Values[3])
                {
                case Particles.ValueTypes.Float:
                    type = ConnectionType.Float;
                    break;
                case Particles.ValueTypes.Vector2:
                    type = ConnectionType.Vector2;
                    break;
                case Particles.ValueTypes.Vector3:
                    type = ConnectionType.Vector3;
                    break;
                case Particles.ValueTypes.Vector4:
                    type = ConnectionType.Vector4;
                    break;
                case Particles.ValueTypes.Int:
                    type = ConnectionType.Integer;
                    break;
                case Particles.ValueTypes.Uint:
                    type = ConnectionType.UnsignedInteger;
                    break;
                default: throw new ArgumentOutOfRangeException();
                }
                GetBox(0).CurrentType = type;
            }
        }

        /// <summary>
        /// The particle emitter module applies the sprite orientation.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.Archetypes.ParticleModules.ParticleModuleNode" />
        public class OrientSpriteNode : ParticleModuleNode
        {
            /// <inheritdoc />
            public OrientSpriteNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateInputBox();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                // Update on mode change
                if (index == 2)
                    UpdateInputBox();
            }

            private void UpdateInputBox()
            {
                GetBox(0).Enabled = (ParticleSpriteFacingMode)Values[2] == ParticleSpriteFacingMode.CustomFacingVector;
            }
        }

        private static SurfaceNode CreateParticleModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new ParticleModuleNode(id, context, arch, groupArch);
        }

        private static SurfaceNode CreateSetParticleAttributeModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new SetParticleAttributeModuleNode(id, context, arch, groupArch);
        }

        private static SurfaceNode CreateOrientSpriteNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new OrientSpriteNode(id, context, arch, groupArch);
        }

        /// <summary>
        /// The particle module node elements offset applied to controls to reduce default surface node header thickness.
        /// </summary>
        private const float NodeElementsOffset = 16.0f - Surface.Constants.NodeHeaderSize;

        private const NodeFlags DefaultModuleFlags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI | NodeFlags.NoMove;

        private static NodeArchetype GetParticleAttribute(ModuleType moduleType, ushort typeId, string title, string description, ConnectionType type, object defaultValue)
        {
            return new NodeArchetype
            {
                TypeID = typeId,
                Create = CreateParticleModuleNode,
                Title = title,
                Description = description,
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 1 * Surface.Constants.LayoutOffsetY),
                DefaultValues = new[]
                {
                    true,
                    (int)moduleType,
                    defaultValue,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(-0.5f, string.Empty, true, type, 0, 2),
                },
            };
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            // Spawn Modules
            new NodeArchetype
            {
                TypeID = 100,
                Create = CreateParticleModuleNode,
                Title = "Constant Spawn Rate",
                Description = "Emits constant amount of particles per second, depending of the rate property",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Spawn,
                    10.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(-0.5f, "Rate", true, ConnectionType.Float, 0, 2),
                },
            },
            // TODO: Variable Spawn Rate
            // TODO: Single Burst
            // TODO: Periodic Burst
            // TODO: Spawn On Event

            // Initialize
            new NodeArchetype
            {
                TypeID = 200,
                Create = CreateSetParticleAttributeModuleNode,
                Title = "Set Attribute",
                Description = "Sets the particle attribute value",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 2 * Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Initialize,
                    "Color",
                    (int)Particles.ValueTypes.Vector4,
                    Color.White,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, -10.0f, 80, 3, typeof(Particles.ValueTypes)),
                    NodeElementArchetype.Factory.TextBox(90, -10.0f, 120, TextBox.DefaultHeight, 2, false),
                    NodeElementArchetype.Factory.Input(-0.5f + 1.0f, string.Empty, true, ConnectionType.All, 0, 4),
                },
            },
            new NodeArchetype
            {
                TypeID = 201,
                Create = CreateOrientSpriteNode,
                Title = "Orient Sprite",
                Description = "Orientates the sprite particles in the space",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 2 * Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Initialize,
                    (int)ParticleSpriteFacingMode.FaceCameraPosition,
                    Vector3.Forward,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, -10.0f, 160, 2, typeof(ParticleSpriteFacingMode)),
                    NodeElementArchetype.Factory.Input(-0.5f + 1.0f, "Custom Vector", true, ConnectionType.Vector3, 0, 3),
                },
            },
            // TODO: Position (sphere/plane/circle/disc/box/cylinder/line/torus/depth)
            // TODO: Inherit Position/Velocity/..
            GetParticleAttribute(ModuleType.Initialize, 250, "Set Position", "Sets the particle position", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Initialize, 251, "Set Lifetime", "Sets the particle lifetime (in seconds)", ConnectionType.Float, 10.0f),
            GetParticleAttribute(ModuleType.Initialize, 252, "Set Age", "Sets the particle age (in seconds)", ConnectionType.Float, 0.0f),
            GetParticleAttribute(ModuleType.Initialize, 253, "Set Color", "Sets the particle color (RGBA)", ConnectionType.Vector4, Color.White),
            GetParticleAttribute(ModuleType.Initialize, 254, "Set Velocity", "Sets the particle velocity (position delta per second)", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Initialize, 255, "Set Sprite Size", "Sets the particle size (width and height of the sprite)", ConnectionType.Vector2, new Vector2(10.0f, 10.0f)),
            GetParticleAttribute(ModuleType.Initialize, 256, "Set Mass", "Sets the particle mass (in kilograms)", ConnectionType.Float, 1.0f),
            GetParticleAttribute(ModuleType.Initialize, 257, "Set Rotation", "Sets the particle rotation (in XYZ)", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Initialize, 258, "Set Angular Velocity", "Sets the angular particle velocity (rotation delta per second)", ConnectionType.Vector3, Vector3.Zero),

            // Update Modules
            new NodeArchetype
            {
                TypeID = 300,
                Create = CreateParticleModuleNode,
                Title = "Update Age",
                Description = "Increases particle age every frame, based on delta time",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 0),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Update,
                },
            },
            new NodeArchetype
            {
                TypeID = 301,
                Create = CreateParticleModuleNode,
                Title = "Gravity",
                Description = "Applies the gravity force to particle velocity",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Update,
                    new Vector3(0, -981.0f, 0),
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(-0.5f, "Force", true, ConnectionType.Vector3, 0, 2),
                },
            },
            new NodeArchetype
            {
                TypeID = 302,
                Create = CreateSetParticleAttributeModuleNode,
                Title = "Set Attribute",
                Description = "Sets the particle attribute value",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 2 * Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Update,
                    "Color",
                    (int)Particles.ValueTypes.Vector4,
                    Color.White,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, -10.0f, 80, 3, typeof(Particles.ValueTypes)),
                    NodeElementArchetype.Factory.TextBox(90, -10.0f, 120, TextBox.DefaultHeight, 2, false),
                    NodeElementArchetype.Factory.Input(-0.5f + 1.0f, string.Empty, true, ConnectionType.All, 0, 4),
                },
            },
            new NodeArchetype
            {
                TypeID = 303,
                Create = CreateOrientSpriteNode,
                Title = "Orient Sprite",
                Description = "Orientates the sprite particles in the space",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 2 * Surface.Constants.LayoutOffsetY),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Update,
                    (int)ParticleSpriteFacingMode.FaceCameraPosition,
                    Vector3.Forward,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.ComboBox(0, -10.0f, 160, 2, typeof(ParticleSpriteFacingMode)),
                    NodeElementArchetype.Factory.Input(-0.5f + 1.0f, "Custom Vector", true, ConnectionType.Vector3, 0, 3),
                },
            },
            // TODO: Collision (sphere/plane/box/cylinder/depth)
            // TODO: Conform to sphere
            // TODO: Force
            // TODO: Drag
            // TODO: Turbulence
            // TODO: Kill (box/sphere/custom)
            GetParticleAttribute(ModuleType.Update, 350, "Set Position", "Sets the particle position", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Update, 351, "Set Lifetime", "Sets the particle lifetime (in seconds)", ConnectionType.Float, 10.0f),
            GetParticleAttribute(ModuleType.Update, 352, "Set Age", "Sets the particle age (in seconds)", ConnectionType.Float, 0.0f),
            GetParticleAttribute(ModuleType.Update, 353, "Set Color", "Sets the particle color (RGBA)", ConnectionType.Vector4, Color.White),
            GetParticleAttribute(ModuleType.Update, 354, "Set Velocity", "Sets the particle velocity (position delta per second)", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Update, 355, "Set Sprite Size", "Sets the particle size (width and height of the sprite)", ConnectionType.Vector2, new Vector2(10.0f, 10.0f)),
            GetParticleAttribute(ModuleType.Update, 356, "Set Mass", "Sets the particle mass (in kilograms)", ConnectionType.Float, 1.0f),
            GetParticleAttribute(ModuleType.Update, 357, "Set Rotation", "Sets the particle rotation (in XYZ)", ConnectionType.Vector3, Vector3.Zero),
            GetParticleAttribute(ModuleType.Update, 358, "Set Angular Velocity", "Sets the angular particle velocity (rotation delta per second)", ConnectionType.Vector3, Vector3.Zero),

            // Render Modules
            new NodeArchetype
            {
                TypeID = 400,
                Create = CreateParticleModuleNode,
                Title = "Sprite Rendering",
                Description = "Draws quad-shaped sprite for every particle",
                Flags = DefaultModuleFlags,
                Size = new Vector2(200, 80),
                DefaultValues = new object[]
                {
                    true,
                    (int)ModuleType.Render,
                    Guid.Empty,
                },
                Elements = new[]
                {
                    // Material
                    NodeElementArchetype.Factory.Text(0, -10, "Material", 80.0f, 16.0f, "The material used for sprites rendering (quads). It must have Domain set to Particle."),
                    NodeElementArchetype.Factory.Asset(80, -10, 2, ContentDomain.Material),
                },
            },
            // TODO: Sort
            // TODO: Mesh Rendering
            // TODO: Ribbon Rendering
            // TODO: Light
        };
    }
}
