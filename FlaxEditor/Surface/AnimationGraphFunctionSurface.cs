// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Surface.Archetypes;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the animation graph functions editor.
    /// </summary>
    /// <seealso cref="AnimGraphSurface" />
    /// <seealso cref="Function.IFunctionSurface" />
    public class AnimationGraphFunctionSurface : AnimGraphSurface, Function.IFunctionSurface
    {
        private static readonly ConnectionType[] AnimationGraphFunctionTypes =
        {
            ConnectionType.Bool,
            ConnectionType.Integer,
            ConnectionType.Float,
            ConnectionType.Vector2,
            ConnectionType.Vector3,
            ConnectionType.Vector4,
            ConnectionType.Impulse,
            ConnectionType.ImpulseSecondary,
        };

        /// <inheritdoc />
        public AnimationGraphFunctionSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
        {
        }

        /// <inheritdoc />
        public override bool CanUseNodeType(NodeArchetype nodeArchetype)
        {
            if (nodeArchetype.Title == "Function Input")
                return true;

            // Allow to use Function Output only in the root graph (not in state machine sub-graphs)
            if (Context == RootContext && nodeArchetype.Title == "Function Output")
                return true;

            return base.CanUseNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        public ConnectionType[] FunctionTypes => AnimationGraphFunctionTypes;
    }
}
