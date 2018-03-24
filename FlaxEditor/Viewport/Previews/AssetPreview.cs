////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Viewport.Cameras;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
	/// <summary>
	/// Generic asset preview editor viewport base class.
	/// </summary>
	/// <seealso cref="FlaxEditor.Viewport.EditorViewport" />
	public abstract class AssetPreview : EditorViewport
	{
		protected DirectionalLight _previewLight;
		protected EnvironmentProbe _envProbe;
		protected Sky _sky;
		protected SkyLight _skyLight;
		protected PostFxVolume _postFxVolume;

		/// <summary>
		/// Gets the post fx volume. Allows to modify rendering settings.
		/// </summary>
		public PostFxVolume PostFxVolume => _postFxVolume;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssetPreview"/> class.
		/// </summary>
		/// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
		public AssetPreview(bool useWidgets)
			: base(RenderTask.Create<SceneRenderTask>(), new ArcBallCamera(Vector3.Zero, 50), useWidgets)
		{
			DockStyle = DockStyle.Fill;

			Task.Flags = ViewFlags.DefaulAssetPreview;

			((ArcBallCamera)ViewportCamera).SetView(new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f));

			// Setup preview scene
			_previewLight = DirectionalLight.New();
			_previewLight.Brightness = 3.0f;
			_previewLight.ShadowsMode = ShadowsCastingMode.None;
			_previewLight.Orientation = Quaternion.Euler(new Vector3(52.1477f, -109.109f, -111.739f));
			//
			_envProbe = EnvironmentProbe.New();
			_envProbe.AutoUpdate = false;
			_envProbe.CustomProbe = FlaxEngine.Content.LoadAsyncInternal<CubeTexture>(EditorAssets.DefaultSkyCubeTexture);
			//
			_sky = Sky.New();
			_sky.SunLight = _previewLight;
			//
			_skyLight = SkyLight.New();
			_skyLight.Mode = SkyLight.Modes.CustomTexture;
			_skyLight.Brightness = 1.1f;
			_skyLight.CustomTexture = _envProbe.CustomProbe;
			//
			_postFxVolume = PostFxVolume.New();
			_postFxVolume.IsBounded = false;
			_postFxVolume.Settings.Eye_MinLuminance = 0.1f;

			// Link actors for rendering
			Task.ActorsSource = ActorsSources.CustomActors;
			Task.CustomActors.Add(_previewLight);
			Task.CustomActors.Add(_envProbe);
			Task.CustomActors.Add(_sky);
			Task.CustomActors.Add(_skyLight);
			Task.CustomActors.Add(_postFxVolume);
		}

		/// <inheritdoc />
		public override bool HasLoadedAssets => base.HasLoadedAssets && _sky.HasContentLoaded && _envProbe.Probe.IsLoaded && _postFxVolume.HasContentLoaded;

		/// <inheritdoc />
		public override void OnDestroy()
		{
			// Ensure to cleanup created actor objects
			Object.Destroy(ref _previewLight);
			Object.Destroy(ref _envProbe);
			Object.Destroy(ref _sky);
			Object.Destroy(ref _skyLight);
			Object.Destroy(ref _postFxVolume);

			base.OnDestroy();
		}
	}
}
