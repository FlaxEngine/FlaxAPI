////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The inteager value element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class IntegerValueElement : LayoutElement
    {
        /// <summary>
        /// The inteager value box.
        /// </summary>
        public readonly IntValueBox IntValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerValueElement"/> class.
        /// </summary>
        public IntegerValueElement()
        {
            IntValue = new IntValueBox(0);
        }

        /// <inheritdoc />
        public override Control Control => IntValue;
    }
}
