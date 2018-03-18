////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface.Archetypes
{
	/// <summary>
	/// Contains archetypes for nodes from the Animation group.
	/// </summary>
	public static class Animation
	{
		/// <summary>
		/// Customized <see cref="SurfaceNode"/> for main animation graph node.
		/// </summary>
		/// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
		public class SurfaceNodeAnimOutput : SurfaceNode
		{
			/// <inheritdoc />
			public SurfaceNodeAnimOutput(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
				: base(id, surface, nodeArch, groupArch)
			{
			}
		}

		/// <summary>
		/// The nodes for that group.
		/// </summary>
		public static NodeArchetype[] Nodes =
		{
			new NodeArchetype
			{
				TypeID = 1,
				Create = (id, surface, arch, groupArch) => new SurfaceNodeAnimOutput(id, surface, arch, groupArch),
				Title = "Animation Output",
				Description = "Main animation graph output node",
				Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI,
				Size = new Vector2(200, 100),
				Elements = new[]
				{
					NodeElementArchetype.Factory.Input(2, "Animation", true, ConnectionType.Impulse, 0),
				}
			},
		};
	}
}
