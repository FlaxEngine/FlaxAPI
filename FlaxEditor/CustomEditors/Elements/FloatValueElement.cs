////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The floating point value element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class FloatValueElement : LayoutElement
    {
        /// <summary>
        /// The float value box.
        /// </summary>
        public readonly FloatValueBox FloatValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatValueElement"/> class.
        /// </summary>
        public FloatValueElement()
        {
            FloatValue = new FloatValueBox(0);
        }

        /// <inheritdoc />
        public override Control Control => FloatValue;
    }
}
