////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
	public sealed partial class AudioSource
	{
		/// <summary>
		/// Valid states in which AudioSource can be in.
		/// </summary>
		public enum States
		{
			/// <summary>
			/// The source is currently playing.
			/// </summary>
			Playing = 0,

			/// <summary>
			/// The source is currently paused (play will resume from paused point).
			/// </summary>
			Paused = 1,

			/// <summary>
			/// The source is currently stopped (play will resume from start).
			/// </summary>
			Stopped = 2
		}
	}
}
