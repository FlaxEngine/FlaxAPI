// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Custom <see cref="ValueContainer"/> for read-only values.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.ValueContainer" />
    public sealed class ReadOnlyValueContainer : ValueContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyValueContainer"/> class.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public ReadOnlyValueContainer(object value)
        : base(null, typeof(object))
        {
            Add(value);
        }

        /// <inheritdoc />
        public override void Refresh(ValueContainer instanceValues)
        {
            // Not supported
        }

        /// <inheritdoc />
        public override void Set(ValueContainer instanceValues, object value)
        {
            // Not supported
        }

        /// <inheritdoc />
        public override void Set(ValueContainer instanceValues)
        {
            // Not supported
        }

        /// <inheritdoc />
        public override void RefreshReferenceValue(object instanceValue)
        {
            // Not supported
        }
    }
}
