////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Model asset preview editor viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewportArcBallCam" />
    public class ModelPreview : EditorViewportArcBallCam
    {
        private ModelActor _previewModel;
        private DirectionalLight _previewLight;
        private EnvironmentProbe _envProbe;
        private Sky _sky;
        private SkyLight _skyLight;
        private PostFxVolume _postFxVolume;

        /// <summary>
        /// Gets or sets the model asset to preview.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public Model Model
        {
            get => _previewModel.Model;
            set => _previewModel.Model = value;
        }

        /// <summary>
        /// Gets the model actor used to preview selected asset.
        /// </summary>
        public ModelActor PreviewModelActor => _previewModel;

        /// <summary>
        /// Gets the post fx volume. Allows to modify rendering settings.
        /// </summary>
        public PostFxVolume PostFxVolume => _postFxVolume;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public ModelPreview(bool useWidgets)
            : base(RenderTask.Create<SceneRenderTask>(), useWidgets, 50, Vector3.Zero)
        {
            DockStyle = DockStyle.Fill;
            
            Task.Flags = ViewFlags.DefaultModelPreview;
            Task.Begin += OnBegin;

            SetView(new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f));

            // Setup preview scene
            _previewModel = ModelActor.New();
            //
            _previewLight = DirectionalLight.New();
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
            _postFxVolume.IsBounded = true;
            _postFxVolume.Settings.Eye_MinLuminance = 0.1f;

            // Link actors for rendering
            Task.ActorsSource = ActorsSources.CustomActors;
            Task.CustomActors.Add(_previewModel);
            Task.CustomActors.Add(_previewLight);
            Task.CustomActors.Add(_envProbe);
            Task.CustomActors.Add(_sky);
            Task.CustomActors.Add(_skyLight);
            Task.CustomActors.Add(_postFxVolume);

            // Update actors
            for (int i = 0; i < Task.CustomActors.Count; i++)
                Task.CustomActors[i].UpdateCache();
        }

        private void OnBegin(SceneRenderTask task)
        {
            // Update preview model scale to fit the preview
            var model = Model;
            if (model && model.IsLoaded)
            {
                float targetSize = 30.0f;
                float maxSize = Mathf.Max(0.001f, model.Box.Size.MaxValue);
                _previewModel.Scale = new Vector3(targetSize / maxSize);
            }
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && _envProbe.Probe.IsLoaded && _sky.HasContentLoaded && _postFxVolume.HasContentLoaded;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to cleanup created actor objects
            Object.Destroy(ref _previewModel);
            Object.Destroy(ref _previewLight);
            Object.Destroy(ref _envProbe);
            Object.Destroy(ref _sky);
            Object.Destroy(ref _skyLight);
            Object.Destroy(ref _postFxVolume);

            base.OnDestroy();
        }
    }
}
