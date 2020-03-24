// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The audio service used for music and sound effects playback.
    /// </summary>
    [Tooltip("The audio service used for music and sound effects playback.")]
    public static unsafe partial class Audio
    {
        /// <summary>
        /// The all audio devices.
        /// </summary>
        [Tooltip("The all audio devices.")]
        public static AudioDevice[] Devices
        {
            get { return Internal_GetDevices(typeof(AudioDevice)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AudioDevice[] Internal_GetDevices(System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the active device.
        /// </summary>
        [Tooltip("The active device.")]
        public static AudioDevice ActiveDevice
        {
            get { return Internal_GetActiveDevice(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AudioDevice Internal_GetActiveDevice();

        /// <summary>
        /// Gets or sets the index of the active device.
        /// </summary>
        [Tooltip("The index of the active device.")]
        public static int ActiveDeviceIndex
        {
            get { return Internal_GetActiveDeviceIndex(); }
            set { Internal_SetActiveDeviceIndex(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetActiveDeviceIndex();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetActiveDeviceIndex(int index);

        /// <summary>
        /// Gets or sets the master volume applied to all the audio sources (normalized to range 0-1).
        /// </summary>
        [Tooltip("The master volume applied to all the audio sources (normalized to range 0-1).")]
        public static float MasterVolume
        {
            get { return Internal_GetMasterVolume(); }
            set { Internal_SetMasterVolume(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMasterVolume();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMasterVolume(float value);

        /// <summary>
        /// Gets the actual master volume (including all side effects and mute effectors).
        /// </summary>
        [Tooltip("The actual master volume (including all side effects and mute effectors).")]
        public static float Volume
        {
            get { return Internal_GetVolume(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolume();

        /// <summary>
        /// Sets the doppler effect factor. Scale for source and listener velocities. Default is 1.
        /// </summary>
        /// <param name="value">The value.</param>
        [Tooltip("Sets the doppler effect factor. Scale for source and listener velocities. Default is 1.")]
        public static float DopplerFactor
        {
            set { Internal_SetDopplerFactor(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDopplerFactor(float value);
    }
}
