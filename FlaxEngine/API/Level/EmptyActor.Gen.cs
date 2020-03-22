// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The empty actor that is useful to create hierarchy and/or hold scripts. See <see cref="Script"/>.
    /// </summary>
    [Tooltip("The empty actor that is useful to create hierarchy and/or hold scripts. See <see cref=\"Script\"/>.")]
    public unsafe partial class EmptyActor : Actor
    {
        /// <inheritdoc />
        protected EmptyActor() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="EmptyActor"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static EmptyActor New()
        {
            return Internal_Create(typeof(EmptyActor)) as EmptyActor;
        }
    }
}
