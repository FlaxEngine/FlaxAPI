////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
	public abstract partial class Script
	{
		/// <summary>
		/// Gets or sets the world space transformation of the actors owning this script.
		/// </summary>
		[HideInEditor, NoSerialize]
		public Transform Transform
		{
			get => Actor.Transform;
			set => Actor.Transform = value;
		}

		/// <summary>
		/// Gets or sets the local space transformation of the actors owning this script.
		/// </summary>
		[HideInEditor, NoSerialize]
		public Transform LocalTransform
		{
			get => Actor.LocalTransform;
			set => Actor.LocalTransform = value;
		}
	}
}
