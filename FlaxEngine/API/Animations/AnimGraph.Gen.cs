// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Animation graph parameter.
    /// </summary>
    /// <seealso cref="GraphParameter" />
    [Tooltip("Animation graph parameter.")]
    public unsafe partial class AnimGraphParameter : GraphParameter
    {
        /// <inheritdoc />
        protected AnimGraphParameter() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="AnimGraphParameter"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static AnimGraphParameter New()
        {
            return Internal_Create(typeof(AnimGraphParameter)) as AnimGraphParameter;
        }
    }
}
