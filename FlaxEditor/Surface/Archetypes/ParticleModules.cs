// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Particle Modules group.
    /// </summary>
    public static partial class ParticleModules
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
        /// Customized <see cref="SurfaceNode"/> for particle emitter module node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class ParticleModuleNode : SurfaceNode
        {
            private CheckBox _enabled;
            private ModuleType _type;

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
                        _enabled.State = value ? CheckBoxState.Checked : CheckBoxState.Intermediate;
                    }
                }
            }

            /// <summary>
            /// Gets the type of the module.
            /// </summary>
            public ModuleType ModuleType => _type;

            /// <inheritdoc />
            public ParticleModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                _enabled = new CheckBox(0, 0, true, FlaxEditor.Surface.Constants.NodeCloseButtonSize)
                {
                    TooltipText = "Enabled/disables this module",
                    Parent = this,
                };
                _enabled.StateChanged += OnEnabledStateChanged;

                if (nodeArch.TypeID - 100 < 100)
                    _type = ModuleType.Spawn;
                else if (nodeArch.TypeID - 200 < 100)
                    _type = ModuleType.Initialize;
                else if (nodeArch.TypeID - 300 < 100)
                    _type = ModuleType.Update;
                else if (nodeArch.TypeID - 400 < 100)
                    _type = ModuleType.Render;
            }

            private void OnEnabledStateChanged(CheckBox control)
            {
                ModuleEnabled = control.State == CheckBoxState.Checked;
            }

            /// <inheritdoc />
            protected override void UpdateRectangles()
            {
                base.UpdateRectangles();

                _enabled.Location = new Vector2(_closeButtonRect.X - _enabled.Width - 2, _closeButtonRect.Y);
            }

            /// <inheritdoc />
            public override void OnSpawned()
            {
                base.OnSpawned();

                ParticleSurface.ArrangeModulesNodes();
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

        private static SurfaceNode CreateParticleModuleNode(uint id, VisjectSurfaceContext context, NodeArchetype arch, GroupArchetype groupArch)
        {
            return new ParticleModuleNode(id, context, arch, groupArch);
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            // Spawn Modules
            // TODO: Single Burst
            // TODO: Periodic Burst
            // TODO: Constant Spawn Rate
            // TODO: Custom Spawn
            // TODO: On Event Spawn

            // Initialize
            // TODO: Set Velocity/Lifetime/..
            // TODO: Position (sphere/plane/circle/disc/box/cylinder/line/torus/depth)
            // TODO: Inherit Position/Velocity/..

            // Update Modules
            new NodeArchetype
            {
                TypeID = 300,
                Create = CreateParticleModuleNode,
                Title = "Age",
                Description = "Increases particle age every frame, based on delta time",
                Flags = NodeFlags.ParticleEmitterGraph, // | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(200, 0),
                DefaultValues = new object[]
                {
                    true,
                },
            },
            // TODO: Euler Movement
            // TODO: Collision (sphere/plane/box/cylinder/depth)
            // TODO: Set Position/Velocity/Color/…
            // TODO: Conform to sphere
            // TODO: Force
            // TODO: Drag
            // TODO: Gravity
            // TODO: Turbulence
            // TODO: Kill (box/sphere/custom)

            // Render Modules
            // TODO: Sort
            // TODO: Sprite Rendering
            // TODO: Mesh Rendering
            // TODO: Trail Rendering
        };
    }
}
