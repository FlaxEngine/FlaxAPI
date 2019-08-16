// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI.ContextMenu;
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
        private ContextMenuButton _showDefaultSceneButton;

        /// <summary>
        /// The preview light. Allows to modify rendering settings.
        /// </summary>
        public DirectionalLight PreviewLight;

        /// <summary>
        /// The env probe. Allows to modify rendering settings.
        /// </summary>
        public EnvironmentProbe EnvProbe;

        /// <summary>
        /// The sky. Allows to modify rendering settings.
        /// </summary>
        public Sky Sky;

        /// <summary>
        /// The sky light. Allows to modify rendering settings.
        /// </summary>
        public SkyLight SkyLight;

        /// <summary>
        /// Gets the post fx volume. Allows to modify rendering settings.
        /// </summary>
        public PostFxVolume PostFxVolume;

        /// <summary>
        /// Gets or sets a value indicating whether show default scene actors (sky, env probe, skylight, directional light, etc.).
        /// </summary>
        public bool ShowDefaultSceneActors
        {
            get => PreviewLight.IsActive;
            set
            {
                if (ShowDefaultSceneActors != value)
                {
                    PreviewLight.IsActive = value;
                    EnvProbe.IsActive = value;
                    Sky.IsActive = value;
                    SkyLight.IsActive = value;

                    if (_showDefaultSceneButton != null)
                        _showDefaultSceneButton.Checked = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">If set to <c>true</c> use widgets for viewport, otherwise hide them.</param>
        /// <param name="orbitRadius">The initial orbit radius.</param>
        protected AssetPreview(bool useWidgets, float orbitRadius = 50.0f)
        : this(useWidgets, new ArcBallCamera(Vector3.Zero, orbitRadius))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">If set to <c>true</c> use widgets for viewport, otherwise hide them.</param>
        /// <param name="camera">The camera controller.</param>
        protected AssetPreview(bool useWidgets, ViewportCamera camera)
        : base(RenderTask.Create<SceneRenderTask>(), camera, useWidgets)
        {
            DockStyle = DockStyle.Fill;

            Task.Flags = ViewFlags.DefaultAssetPreview;
            Task.AllowGlobalCustomPostFx = false;

            var orbitRadius = 200.0f;
            if (camera is ArcBallCamera arcBallCamera)
                orbitRadius = arcBallCamera.OrbitRadius;
            camera.SerArcBallView(new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f), Vector3.Zero, orbitRadius);

            if (useWidgets)
            {
                // Show Default Scene
                _showDefaultSceneButton = ViewWidgetShowMenu.AddButton("Default Scene", () => ShowDefaultSceneActors = !ShowDefaultSceneActors);
                _showDefaultSceneButton.Checked = true;
            }

            // Setup preview scene
            PreviewLight = DirectionalLight.New();
            PreviewLight.Brightness = 8.0f;
            PreviewLight.ShadowsMode = ShadowsCastingMode.None;
            PreviewLight.Orientation = Quaternion.Euler(new Vector3(52.1477f, -109.109f, -111.739f));
            //
            EnvProbe = EnvironmentProbe.New();
            EnvProbe.AutoUpdate = false;
            EnvProbe.CustomProbe = FlaxEngine.Content.LoadAsyncInternal<CubeTexture>(EditorAssets.DefaultSkyCubeTexture);
            //
            Sky = Sky.New();
            Sky.SunLight = PreviewLight;
            Sky.SunPower = 9.0f;
            //
            SkyLight = SkyLight.New();
            SkyLight.Mode = SkyLight.Modes.CustomTexture;
            SkyLight.Brightness = 2.1f;
            SkyLight.CustomTexture = EnvProbe.CustomProbe;
            //
            PostFxVolume = PostFxVolume.New();
            PostFxVolume.IsBounded = false;
            PostFxVolume.Settings.Eye_MinLuminance = 0.1f;

            // Link actors for rendering
            Task.ActorsSource = ActorsSources.CustomActors;
            Task.CustomActors.Add(PreviewLight);
            Task.CustomActors.Add(EnvProbe);
            Task.CustomActors.Add(Sky);
            Task.CustomActors.Add(SkyLight);
            Task.CustomActors.Add(PostFxVolume);
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && Sky.HasContentLoaded && EnvProbe.Probe.IsLoaded && PostFxVolume.HasContentLoaded;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to cleanup created actor objects
            Object.Destroy(ref PreviewLight);
            Object.Destroy(ref EnvProbe);
            Object.Destroy(ref Sky);
            Object.Destroy(ref SkyLight);
            Object.Destroy(ref PostFxVolume);

            base.OnDestroy();
        }
    }
}
