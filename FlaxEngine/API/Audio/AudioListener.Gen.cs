// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a listener that hears audio sources. For spatial audio the volume and pitch of played audio is determined by the distance, orientation and velocity differences between the source and the listener.
    /// </summary>
    [Tooltip("Represents a listener that hears audio sources. For spatial audio the volume and pitch of played audio is determined by the distance, orientation and velocity differences between the source and the listener.")]
    public unsafe partial class AudioListener : Actor
    {
        /// <inheritdoc />
        protected AudioListener() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="AudioListener"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static AudioListener New()
        {
            return Internal_Create(typeof(AudioListener)) as AudioListener;
        }

        /// <summary>
        /// Gets the velocity of the listener. Determines pitch in relation to AudioListener's position.
        /// </summary>
        [Tooltip("The velocity of the listener. Determines pitch in relation to AudioListener's position.")]
        public Vector3 Velocity
        {
            get { Internal_GetVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVelocity(IntPtr obj, out Vector3 resultAsRef);
    }
}
