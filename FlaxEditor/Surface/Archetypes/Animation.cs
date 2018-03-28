////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
	/// <summary>
	/// Contains archetypes for nodes from the Animation group.
	/// </summary>
	public static class Animation
	{
		/// <summary>
		/// Customized <see cref="SurfaceNode"/> for the main animation graph node.
		/// </summary>
		/// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
		public class Output : SurfaceNode
		{
			/// <inheritdoc />
			public Output(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
				: base(id, surface, nodeArch, groupArch)
			{
			}
		}

		/// <summary>
		/// Customized <see cref="SurfaceNode"/> for the animation sampling nodes
		/// </summary>
		/// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
		public class Sample : SurfaceNode
		{
			/// <inheritdoc />
			public Sample(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
				: base(id, surface, nodeArch, groupArch)
			{
			}

			/// <inheritdoc />
			public override void SetValue(int index, object value)
			{
				base.SetValue(index, value);
				UpdateTitle();
			}

			/// <inheritdoc />
			public override void OnSurfaceLoaded()
			{
				base.OnSurfaceLoaded();

				UpdateTitle();
			}

			private void UpdateTitle()
			{
				var asset = Editor.Instance.ContentDatabase.Find((Guid)Values[0]);
				Name = asset?.ShortName ?? "Animation";
				var style = Style.Current;
				Resize(Mathf.Max(230, style.FontLarge.MeasureText(Name).X + 20), 160);
			}
		}

		/// <summary>
		/// The bone transformation modes.
		/// </summary>
		public enum BoneTransformMode
		{
			/// <summary>
			/// No transformation.
			/// </summary>
			None = 0,

			/// <summary>
			/// Applies the transformation.
			/// </summary>
			Add = 1,

			/// <summary>
			/// Replaces the transformation.
			/// </summary>
			Replace = 2,
		}

		/// <summary>
		/// The nodes for that group.
		/// </summary>
		public static NodeArchetype[] Nodes =
		{
			new NodeArchetype
			{
				TypeID = 1,
				Create = (id, surface, arch, groupArch) => new Output(id, surface, arch, groupArch),
				Title = "Animation Output",
				Description = "Main animation graph output node",
				Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI,
				Size = new Vector2(200, 100),
				Elements = new[]
				{
					NodeElementArchetype.Factory.Input(2, "Animation", true, ConnectionType.Impulse, 0),
				}
			},
			new NodeArchetype
			{
				TypeID = 2,
				Create = (id, surface, arch, groupArch) => new Sample(id, surface, arch, groupArch),
				Title = "Animation",
				Description = "Animation sampling",
				Flags = NodeFlags.AnimGraphOnly,
				Size = new Vector2(230, 160),
				DefaultValues = new object[]
				{
					Guid.Empty,
					1.0f,
					true,
					0.0f,
				},
				Elements = new[]
				{
					NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
					NodeElementArchetype.Factory.Output(1, "Normalized Time", ConnectionType.Float, 1),
					NodeElementArchetype.Factory.Output(2, "Time", ConnectionType.Float, 1),
					NodeElementArchetype.Factory.Output(3, "Length", ConnectionType.Float, 3),
					NodeElementArchetype.Factory.Output(4, "Is Playing", ConnectionType.Bool, 4),
					NodeElementArchetype.Factory.Input(0, "Speed", true, ConnectionType.Float, 5, 1),
					NodeElementArchetype.Factory.Input(1, "Loop", true, ConnectionType.Bool, 6, 2),
					NodeElementArchetype.Factory.Input(2, "Start Position", true, ConnectionType.Float, 7, 3),
					NodeElementArchetype.Factory.Asset(0, Surface.Constants.LayoutOffsetY * 3, 0, ContentDomain.Animation),
				}
			},
			new NodeArchetype
			{
				TypeID = 3,
				Title = "Transform Bone (local space)",
				Description = "Trnsforms the skeleton root bone",
				Flags = NodeFlags.AnimGraphOnly,
				Size = new Vector2(260, 130),
				DefaultValues = new object[]
				{
					0,
					(int)BoneTransformMode.Add,
				},
				Elements = new[]
				{
					NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
					NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Impulse, 1),
					NodeElementArchetype.Factory.Input(1, "Translation", true, ConnectionType.Vector3, 2),
					NodeElementArchetype.Factory.Input(2, "Rotation", true, ConnectionType.Rotation, 3),
					NodeElementArchetype.Factory.Input(3, "Scale", true, ConnectionType.Vector3, 4),
					NodeElementArchetype.Factory.SkeletonNodeSelect(40, Surface.Constants.LayoutOffsetY * 4, 120, 0),
					NodeElementArchetype.Factory.ComboBox(40, Surface.Constants.LayoutOffsetY * 5, 120, 1, typeof(BoneTransformMode)),
					NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 4, "Bone:"),
					NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 5, "Mode:"),
				}
			},
		};
	}
}
