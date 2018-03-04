////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
	/// <summary>
	/// Audio data storage format used by the runtime.
	/// </summary>
	public enum AudioFormat
	{
		/// <summary>
		/// The raw PCM data.
		/// </summary>
		Raw = 0,

		/// <summary>
		/// The compressed audio data stored in the Vorbis format.
		/// </summary>
		Vorbis = 1,
	}

	public sealed partial class AudioClip
	{
		/// <summary>
		/// Meta-data describing a chunk of audio.
		/// </summary>
		public struct AudioDataInfo
		{
			/// <summary>
			/// The total number of audio samples in the audio data (includes all channels).
			/// </summary>
			public uint NumSamples;

			/// <summary>
			/// The number of audio samples per second, per channel.
			/// </summary>
			public uint SampleRate;

			/// <summary>
			/// The number of channels. Each channel has its own set of samples.
			/// </summary>
			public uint NumChannels;

			/// <summary>
			/// The number of bits per sample.
			/// </summary>
			public uint BitDepth;
		}

		/// <summary>
		/// Gets the length of the audio clip (in seconds).
		/// </summary>
		/// <returns>The value.</returns>
		public float Length
		{
			get
			{
				AudioDataInfo info;
				GetInfo(out info);
				return info.NumSamples / (float)Math.Max(1U, info.SampleRate * info.NumChannels);
			}
		}
	}
}
