// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Audio clip stores audio data in a compressed or uncompressed format using a binary asset. Clips can be provided to audio sources or other audio methods to be played.
    /// </summary>
    /// <seealso cref="BinaryAsset" />
    /// <seealso cref="StreamableResource" />
    [Tooltip("Audio clip stores audio data in a compressed or uncompressed format using a binary asset. Clips can be provided to audio sources or other audio methods to be played.")]
    public unsafe partial class AudioClip : BinaryAsset
    {
        /// <inheritdoc />
        protected AudioClip() : base()
        {
        }

        /// <summary>
        /// Gets the audio data format.
        /// </summary>
        [Tooltip("The audio data format.")]
        public AudioFormat Format
        {
            get { return Internal_Format(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AudioFormat Internal_Format(IntPtr obj);

        /// <summary>
        /// Gets the audio data info metadata.
        /// </summary>
        [Tooltip("The audio data info metadata.")]
        public AudioDataInfo Info
        {
            get { Internal_Info(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Info(IntPtr obj, out AudioDataInfo resultAsRef);

        /// <summary>
        /// Returns true if the sound source is three dimensional (volume and pitch varies based on listener distance and velocity).
        /// </summary>
        [Tooltip("Returns true if the sound source is three dimensional (volume and pitch varies based on listener distance and velocity).")]
        public bool Is3D
        {
            get { return Internal_Is3D(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Is3D(IntPtr obj);

        /// <summary>
        /// Returns true if the sound is using data streaming.
        /// </summary>
        [Tooltip("Returns true if the sound is using data streaming.")]
        public bool IsStreamable
        {
            get { return Internal_IsStreamable(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsStreamable(IntPtr obj);

        /// <summary>
        /// Returns true if the sound data is during streaming by an async task.
        /// </summary>
        [Tooltip("Returns true if the sound data is during streaming by an async task.")]
        public bool IsStreamingTaskActive
        {
            get { return Internal_IsStreamingTaskActive(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsStreamingTaskActive(IntPtr obj);

        /// <summary>
        /// Gets the length of the audio clip (in seconds).
        /// </summary>
        [Tooltip("The length of the audio clip (in seconds).")]
        public float Length
        {
            get { return Internal_GetLength(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetLength(IntPtr obj);

        /// <summary>
        /// Extracts the source audio data from the asset storage. Loads the whole asset. The result data is in an asset format.
        /// </summary>
        /// <param name="resultData">The result data.</param>
        /// <param name="resultDataInfo">The result data format header info.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool ExtractData(out byte[] resultData, out AudioDataInfo resultDataInfo)
        {
            return Internal_ExtractData(unmanagedPtr, out resultData, out resultDataInfo, typeof(byte));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ExtractData(IntPtr obj, out byte[] resultData, out AudioDataInfo resultDataInfo, System.Type resultArrayItemType0);

        /// <summary>
        /// Extracts the raw audio data (PCM format) from the asset storage and converts it to the normalized float format (in range [-1;1]). Loads the whole asset.
        /// </summary>
        /// <param name="resultData">The result data.</param>
        /// <param name="resultDataInfo">The result data format header info. That output data has 32 bits float data not the signed PCM data.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool ExtractDataFloat(out float[] resultData, out AudioDataInfo resultDataInfo)
        {
            return Internal_ExtractDataFloat(unmanagedPtr, out resultData, out resultDataInfo, typeof(float));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ExtractDataFloat(IntPtr obj, out float[] resultData, out AudioDataInfo resultDataInfo, System.Type resultArrayItemType0);

        /// <summary>
        /// Extracts the raw audio data (PCM format) from the asset storage. Loads the whole asset.
        /// </summary>
        /// <param name="resultData">The result data.</param>
        /// <param name="resultDataInfo">The result data format header info.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool ExtractDataRaw(out byte[] resultData, out AudioDataInfo resultDataInfo)
        {
            return Internal_ExtractDataRaw(unmanagedPtr, out resultData, out resultDataInfo, typeof(byte));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ExtractDataRaw(IntPtr obj, out byte[] resultData, out AudioDataInfo resultDataInfo, System.Type resultArrayItemType0);
    }
}
