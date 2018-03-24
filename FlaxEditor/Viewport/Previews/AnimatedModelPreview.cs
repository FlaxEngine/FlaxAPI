////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Viewport.Cameras;
using FlaxEngine;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
	/// <summary>
	/// Animated model asset preview editor viewport.
	/// </summary>
	/// <seealso cref="AssetPreview" />
	public class AnimatedModelPreview : AssetPreview
	{
		private AnimatedModel _previewModel;

		/// <summary>
		/// Gets or sets the skinned model asset to preview.
		/// </summary>
		public SkinnedModel SkinnedModel
		{
			get => _previewModel.SkinnedModel;
			set => _previewModel.SkinnedModel = value;
		}

		/// <summary>
		/// Gets the skinned model actor used to preview selected asset.
		/// </summary>
		public AnimatedModel PreviewModelActor => _previewModel;

		/// <summary>
		/// Gets or sets a value indicating whether play the animation in editor.
		/// </summary>
		public bool PlayAnimation { get; set; } = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="AnimatedModelPreview"/> class.
		/// </summary>
		/// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
		public AnimatedModelPreview(bool useWidgets)
			: base(useWidgets)
		{
			Task.Begin += OnBegin;
			
			// Setup preview scene
			_previewModel = AnimatedModel.New();
			_previewModel.UseTimeScale = false;
			_previewModel.UpdateWhenOffscreen = true;
			_previewModel.UpdateMode = AnimatedModel.AnimationUpdateMode.Manual;

			// Link actors for rendering
			Task.CustomActors.Add(_previewModel);
		}

		private void OnBegin(SceneRenderTask task)
		{
			// Update preview model scale to fit the preview
			var skinnedModel = SkinnedModel;
			if (skinnedModel && skinnedModel.IsLoaded)
			{
				float targetSize = 30.0f;
				float maxSize = Mathf.Max(0.001f, skinnedModel.Box.Size.MaxValue);
				_previewModel.Scale = new Vector3(targetSize / maxSize);
			}
		}

		/// <inheritdoc />
		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			if (PlayAnimation)
			{
				_previewModel.UpdateAnimation();
			}
		}

		/// <inheritdoc />
		public override void OnDestroy()
		{
			// Ensure to cleanup created actor objects
			Object.Destroy(ref _previewModel);

			base.OnDestroy();
		}
	}
}
