////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
	/// <summary>
	/// A <see cref="Animation"/> asset proxy object.
	/// </summary>
	/// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
	public class AnimationProxy : BinaryAssetProxy
	{
		/// <inheritdoc />
		public override string Name => "Animation";

		/// <inheritdoc />
		public override bool CanReimport(ContentItem item)
		{
			return true;
		}

		/// <inheritdoc />
		public override EditorWindow Open(Editor editor, ContentItem item)
		{
			throw new NotImplementedException();
			//return new AnimationWindow(editor, item as AssetItem);
		}

		/// <inheritdoc />
		public override Color AccentColor => Color.FromRGB(0xB37200);

		/// <inheritdoc />
		public override ContentDomain Domain => ContentDomain.Animation;

		/// <inheritdoc />
		public override string TypeName => typeof(Animation).FullName;
	}
}
