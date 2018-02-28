////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
	/// <summary>
	/// Scene tree node for <see cref="AudioListener"/> actor type.
	/// </summary>
	/// <seealso cref="ActorNodeWithIcon" />
	public sealed class AudioListenerNode : ActorNodeWithIcon
	{
		/// <inheritdoc />
		public AudioListenerNode(Actor actor)
			: base(actor)
		{
		}
	}
}
