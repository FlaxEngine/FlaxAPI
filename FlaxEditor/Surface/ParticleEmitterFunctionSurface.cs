// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Surface.Archetypes;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the particle emitter functions editor.
    /// </summary>
    /// <seealso cref="ParticleEmitterSurface" />
    /// <seealso cref="Function.IFunctionSurface" />
    public class ParticleEmitterFunctionSurface : ParticleEmitterSurface, Function.IFunctionSurface
    {
        private static readonly ConnectionType[] ParticleEmitterFunctionTypes =
        {
            ConnectionType.Bool,
            ConnectionType.Integer,
            ConnectionType.Float,
            ConnectionType.Vector2,
            ConnectionType.Vector3,
            ConnectionType.Vector4,
            ConnectionType.Object,
        };

        /// <inheritdoc />
        public ParticleEmitterFunctionSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
        {
        }

        /// <inheritdoc />
        public override bool CanUseNodeType(NodeArchetype nodeArchetype)
        {
            if (nodeArchetype.Title == "Function Input" ||
                nodeArchetype.Title == "Function Output")
                return true;

            return base.CanUseNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        public ConnectionType[] FunctionTypes => ParticleEmitterFunctionTypes;
    }
}
