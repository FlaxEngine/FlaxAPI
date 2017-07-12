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

        /// <summary>
        /// Gets or sets the material asset to preview. It can be <see cref="FlaxEngine.Material"/> or <see cref="FlaxEngine.MaterialInstance"/>.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public MaterialBase Material
        {
            get => _previewModel.Meshes[0].Material;
            set => _previewModel.Meshes[0].Material = value;
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
            _envProbe.SetCustomProbe(FlaxEngine.Content.LoadAsyncInternal<CubeTexture>("Editor/SimplySky"));
            //
            _sky = Sky.New();
            _sky.SunLight = _previewLight;

            // Link actors for rendering
            Task.CustomActors.Add(_previewModel);
            Task.CustomActors.Add(_previewLight);
            Task.CustomActors.Add(_envProbe);
            Task.CustomActors.Add(_sky);

            // Update actors
            for (int i = 0; i < Task.CustomActors.Count; i++)
                Task.CustomActors[i].UpdateCache();

            // TODO: don't wait for model but assign material in async on task begin or sth?
            // do it like in c++ editor
            _previewModel.Model?.WaitForLoaded();
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && _previewModel.Model.IsLoaded && _envProbe.Probe.IsLoaded;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to cleanup created actor objects
            Object.Destroy(ref _previewModel);
            Object.Destroy(ref _previewLight);
            Object.Destroy(ref _envProbe);
            Object.Destroy(ref _sky);

            base.OnDestroy();
        }
    }
}
