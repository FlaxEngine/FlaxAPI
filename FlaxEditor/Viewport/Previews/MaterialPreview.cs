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
    /// Material or Material Instance asset preview editor viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewportArcBallCam" />
    public class MaterialPreview : EditorViewportArcBallCam
    {
        private ModelActor _previewModel;
        private DirectionalLight _previewLight;
        private EnvironmentProbe _envProbe;
        private Sky _sky;
        private SkyLight _skyLight;
        private PostFxVolume _postFxVolume;
        private MaterialBase _material;

        /// <summary>
        /// Gets or sets the material asset to preview. It can be <see cref="FlaxEngine.Material"/> or <see cref="FlaxEngine.MaterialInstance"/>.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public MaterialBase Material
        {
            get => _material;
            set
            {
                if (_material != value)
                {
                    _material = value;
                    UpdateMaterial();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public MaterialPreview(bool useWidgets)
            : base(RenderTask.Create<SceneRenderTask>(), useWidgets, 50, Vector3.Zero)
        {
            DockStyle = DockStyle.Fill;

            Task.Flags = ViewFlags.DefaultMaterialPreview;

            SetView(new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f));

            // Setup preview scene
            _previewModel = ModelActor.New();
            _previewModel.Transform = new Transform(Vector3.Zero, Quaternion.Identity, new Vector3(0.45f));
            _previewModel.Model = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Primitives/Sphere");
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
            _skyLight.Brightness = 0.8f;
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

            // TODO: don't wait for model but assign material in async on task begin or sth?
            // do it like in c++ editor
            _previewModel.Model?.WaitForLoaded();
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && _sky.HasContentLoaded && _previewModel.Model.IsLoaded && _envProbe.Probe.IsLoaded && _postFxVolume.HasContentLoaded;

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            // If material is a surface link it to the preview model.
            // Otherwise use postFx volume to render custom postFx material.
            MaterialBase surfaceMaterial = null;
            MaterialBase postFxMaterial = null;
            if (_material != null)
            {
                if (_material is MaterialInstance materialInstance && materialInstance.BaseMaterial == null)
                {
                    // Material instance without a base material should not be used
                }
                else
                {
                    if (_material.IsPostFx)
                        postFxMaterial = _material;
                    else
                        surfaceMaterial = _material;
                }
            }
            var entries = _previewModel.Entries;
            if (entries.Length == 1)
                entries[0].Material = surfaceMaterial;
            _postFxVolume.Settings.PostFxMaterials = new [] { postFxMaterial };
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _material = null;

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
