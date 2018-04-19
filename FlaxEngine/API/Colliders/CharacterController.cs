// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class CharacterController
    {
        /// <summary>
        /// Specifies which sides a character is colliding with.
        /// </summary>
        [Flags]
        public enum CollisionFlags
        {
            /// <summary>
            /// The character is not colliding.
            /// </summary>
            None = 0,

            /// <summary>
            /// The character is colliding to the sides.
            /// </summary>
            Sides = (1 << 0),

            /// <summary>
            /// The character has collision above.
            /// </summary>
            Above = (1 << 1),

            /// <summary>
            /// The character has collision below.
            /// </summary>
            Below = (1 << 2),
        }

        /// <summary>
        /// Gets a value indicating whether this characters is grounded.
        /// </summary>
        /// <remarks>
        /// Returns true is the CharacterController was touching the ground during the last move.
        /// </remarks>
        public bool IsGrounded => (Flags & CollisionFlags.Below) != 0;
    }
}
