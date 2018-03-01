////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Viewport.Previews
{
	/// <summary>
	/// Audio clip PCM data editor preview.
	/// </summary>
	/// <seealso cref="FlaxEngine.GUI.ContainerControl" />
	public class AudioClipPreview : ContainerControl
	{
		// TODO: implement drawing AudioClip PCM data

		private AudioClip _asset;

		/// <summary>
		/// Gets or sets the clip to preview.
		/// </summary>
		public AudioClip Asset
		{
			get => _asset;
			set
			{
				if (_asset != value)
				{
					_asset = value;
				}
			}
		}

		/// <inheritdoc />
		public AudioClipPreview()
		{
			DockStyle = DockStyle.Fill;
		}
	}
}
