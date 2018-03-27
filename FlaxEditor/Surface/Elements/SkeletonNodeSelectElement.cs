////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
	/// <summary>
	/// The Visject surface node element used to pick a skeleton node with a combo box.
	/// </summary>
	public class SkeletonNodeSelectElement : ComboBoxElement
	{
		/// <inheritdoc />
		public SkeletonNodeSelectElement(SurfaceNode parentNode, NodeElementArchetype archetype)
			: base(parentNode, archetype)
		{
		}
	}
}
