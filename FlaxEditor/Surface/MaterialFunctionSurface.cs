// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Archetypes;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the material functions editor.
    /// </summary>
    /// <seealso cref="MaterialSurface" />
    /// <seealso cref="Function.IFunctionSurface" />
    public class MaterialFunctionSurface : MaterialSurface, Function.IFunctionSurface
    {
        private static readonly ConnectionType[] MaterialFunctionTypes =
        {
            ConnectionType.Bool,
            ConnectionType.Integer,
            ConnectionType.Float,
            ConnectionType.Vector2,
            ConnectionType.Vector3,
            ConnectionType.Vector4,
            ConnectionType.Object,
            ConnectionType.Impulse,
        };

        /// <inheritdoc />
        public MaterialFunctionSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
        {
        }

        /// <inheritdoc />
        public override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            if (nodeArchetype.Title == "Function Input" ||
                nodeArchetype.Title == "Function Output")
                return true;

            return base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        public ConnectionType[] FunctionTypes => MaterialFunctionTypes;
    }
}
