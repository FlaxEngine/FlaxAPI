////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    public sealed partial class CharacterController
    {
        /// <summary>
        /// Specifies which sides a character is colliding with.
        /// </summary>
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
    }
}
