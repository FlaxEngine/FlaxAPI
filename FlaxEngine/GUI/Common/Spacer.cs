////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Helper control used to insert blank space into the layout.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public sealed class Spacer : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Spacer"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Spacer(float width, float height)
            : base(false, 0, 0, width, height)
        {
        }
    }
}
