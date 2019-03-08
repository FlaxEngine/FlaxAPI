// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
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
            /// Particles will orient to look at the given position vector.
            /// </summary>
            LookAtPosition,

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
                Render2D.DrawText(style.FontMedium, Title, headerRect, ModuleEnabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);

                // Close button
                float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));

                DrawChildren();

                // Border
                Render2D.DrawRectangle(new Rectangle(1, 1, Width - 2, Height - 2), Colors[idx], 1.5f);
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
        /// The particle emitter module that can wrtie to the particle attribute.
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
                if (index == 1)
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

        private static SurfaceNode CreateParticleModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new ParticleModuleNode(id, context, arch, groupArch);
        }

        private static SurfaceNode CreateSetParticleAttributeModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new SetParticleAttributeModuleNode(id, context, arch, groupArch);
        }

        /// <summary>
        /// The particle module node elements offset applied to controls to reduce default surface node header thickness.
        /// </summary>
        public const float NodeElementsOffset = 16.0f - Surface.Constants.NodeHeaderSize;

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
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
            // TODO: On Event Spawn

            // Initialize
            new NodeArchetype
            {
                TypeID = 200,
                Create = CreateSetParticleAttributeModuleNode,
                Title = "Set Attribute",
                Description = "Sets the particle attribute value",
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
            // TODO: Set Velocity/Lifetime/..
            // TODO: Position (sphere/plane/circle/disc/box/cylinder/line/torus/depth)
            // TODO: Inherit Position/Velocity/..

            // Update Modules
            new NodeArchetype
            {
                TypeID = 300,
                Create = CreateParticleModuleNode,
                Title = "Update Age",
                Description = "Increases particle age every frame, based on delta time",
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
            // TODO: Collision (sphere/plane/box/cylinder/depth)
            // TODO: Set Position/Velocity/Color/…
            // TODO: Conform to sphere
            // TODO: Force
            // TODO: Drag
            // TODO: Turbulence
            // TODO: Kill (box/sphere/custom)

            // Render Modules
            new NodeArchetype
            {
                TypeID = 400,
                Create = CreateParticleModuleNode,
                Title = "Sprite Rendering",
                Description = "Draws quad-shaped sprite for every particle",
                Flags = NodeFlags.ParticleEmitterGraph | NodeFlags.NoSpawnViaGUI,
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
            // TODO: Orient
            // TODO: Sort
            // TODO: Mesh Rendering
            // TODO: Ribbon Rendering
            // TODO: Light
        };
    }
}
