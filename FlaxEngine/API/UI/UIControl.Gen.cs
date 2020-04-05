// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Contains a single GUI control (on C# side).
    /// </summary>
    [Tooltip("Contains a single GUI control (on C# side).")]
    public unsafe partial class UIControl : Actor
    {
        /// <inheritdoc />
        protected UIControl() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="UIControl"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static UIControl New()
        {
            return Internal_Create(typeof(UIControl)) as UIControl;
        }
    }
}
