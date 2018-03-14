////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
	public sealed partial class AnimatedModel
	{
		/// <summary>
		/// Describes the animation graph updates frequency for the animated model.
		/// </summary>
		public enum AnimationUpdateMode
		{
			/// <summary>
			/// The automatic updates will be used (based on platform capabilities, distance to the player, etc.).
			/// </summary>
			Auto = 0,

			/// <summary>
			/// Animation will be updated every physics update (fixed update).
			/// </summary>
			EveryFixedUpdate = 1,

			/// <summary>
			/// Animation will be updated every second physics update (fixed update).
			/// </summary>
			EverySecondFixedUpdate = 2,

			/// <summary>
			/// Animation will be updated every fourth physics update (fixed update).
			/// </summary>
			EveryThirdFixedUpdate = 3,

			/// <summary>
			/// Animation will be updated every game update.
			/// </summary>
			EveryUpdate = 4,

			/// <summary>
			/// Animation will be updated every second game update.
			/// </summary>
			EverySecondUpdate = 5,

			/// <summary>
			/// Animation will be updated every fourth game update.
			/// </summary>
			EveryFourthUpdate = 6,

			/// <summary>
			/// Animation can be updated manually by the user scripts. Use <see cref="AnimatedModel.UpdateAnimation"/> method.
			/// </summary>
			Manual = 7,

			/// <summary>
			/// Animation won't be updated at all.
			/// </summary>
			Never = 8,
		}
	}
}
