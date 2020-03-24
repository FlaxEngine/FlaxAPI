// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Audio data storage format used by the runtime.
    /// </summary>
    [Tooltip("Audio data storage format used by the runtime.")]
    public enum AudioFormat
    {
        /// <summary>
        /// The raw PCM data.
        /// </summary>
        [Tooltip("The raw PCM data.")]
        Raw,

        /// <summary>
        /// The Vorbis data.
        /// </summary>
        [Tooltip("The Vorbis data.")]
        Vorbis,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Meta-data describing a chunk of audio.
    /// </summary>
    [Tooltip("Meta-data describing a chunk of audio.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct AudioDataInfo
    {
        /// <summary>
        /// The total number of audio samples in the audio data (includes all channels).
        /// </summary>
        [Tooltip("The total number of audio samples in the audio data (includes all channels).")]
        public uint NumSamples;

        /// <summary>
        /// The number of audio samples per second, per channel.
        /// </summary>
        [Tooltip("The number of audio samples per second, per channel.")]
        public uint SampleRate;

        /// <summary>
        /// The number of channels. Each channel has its own set of samples.
        /// </summary>
        [Tooltip("The number of channels. Each channel has its own set of samples.")]
        public uint NumChannels;

        /// <summary>
        /// The number of bits per sample.
        /// </summary>
        [Tooltip("The number of bits per sample.")]
        public uint BitDepth;
    }
}
