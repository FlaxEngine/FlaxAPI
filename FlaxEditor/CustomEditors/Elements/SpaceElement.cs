////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The spacer element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class SpaceElement : LayoutElement
    {
        /// <summary>
        /// The spacer.
        /// </summary>
        public readonly Spacer Spacer = new Spacer(0, 0);

        /// <summary>
        /// Initializes the element.
        /// </summary>
        /// <param name="height">The height.</param>
        public void Init(float height)
        {
            Spacer.Height = height;
        }

        /// <inheritdoc />
        public override Control Control => Spacer;
    }
}
