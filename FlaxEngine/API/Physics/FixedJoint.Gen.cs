// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that maintains a fixed distance and orientation between its two attached bodies.
    /// </summary>
    /// <seealso cref="Joint" />
    [Tooltip("Physics joint that maintains a fixed distance and orientation between its two attached bodies.")]
    public unsafe partial class FixedJoint : Joint
    {
        /// <inheritdoc />
        protected FixedJoint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="FixedJoint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static FixedJoint New()
        {
            return Internal_Create(typeof(FixedJoint)) as FixedJoint;
        }
    }
}
