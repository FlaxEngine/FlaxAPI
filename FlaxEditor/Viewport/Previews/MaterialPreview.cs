// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Material or Material Instance asset preview editor viewport.
    /// </summary>
    /// <seealso cref="AssetPreview" />
    public class MaterialPreview : AssetPreview
    {
        private string[] Models =
        {
            "Sphere",
            "Cube",
            "Plane",
            "Cylinder",
            "Cone"
        };

        private StaticModel _previewModel;
        private Decal _decal;
        private MaterialBase _material;
        private int _selectedModelIndex;
        private Image _guiMaterialControl;
        private readonly MaterialBase[] _postFxMaterialsCache = new MaterialBase[1];

        /// <summary>
        /// Gets or sets the material asset to preview. It can be <see cref="FlaxEngine.Material"/> or <see cref="FlaxEngine.MaterialInstance"/>.
        /// </summary>
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
        /// Gets or sets the selected preview model index.
        /// </summary>
        public int SelectedModelIndex
        {
            get => _selectedModelIndex;
            set
            {
                if (value < 0 || value > Models.Length)
                    throw new ArgumentOutOfRangeException();

                _selectedModelIndex = value;
                _previewModel.Model = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Primitives/" + Models[value]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public MaterialPreview(bool useWidgets)
        : base(useWidgets)
        {
            // Setup preview scene
            _previewModel = StaticModel.New();
            _previewModel.Transform = new Transform(Vector3.Zero, Quaternion.RotationY(Mathf.Pi), new Vector3(0.45f));
            SelectedModelIndex = 0;

            // Link actors for rendering
            Task.CustomActors.Add(_previewModel);

            // TODO: don't wait for model but assign material in async on task begin or sth?
            // do it like in c++ editor
            _previewModel.Model?.WaitForLoaded();

            // Create context menu for primitive switching
            if (useWidgets && ViewWidgetButtonMenu != null)
            {
                ViewWidgetButtonMenu.AddSeparator();
                var modelSelect = ViewWidgetButtonMenu.AddChildMenu("Model").ContextMenu;

                // Fill out all models 
                for (int i = 0; i < Models.Length; i++)
                {
                    var button = modelSelect.AddButton(Models[i]);
                    button.Tag = i;
                }

                // Link the action
                modelSelect.ButtonClicked += (button) => SelectedModelIndex = (int)button.Tag;
            }
        }

        /// <inheritdoc />
        public override bool HasLoadedAssets => base.HasLoadedAssets && _previewModel.Model.IsLoaded;

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
            MaterialBase decalMaterial = null;
            MaterialBase guiMaterial = null;
            if (_material != null)
            {
                if (_material is MaterialInstance materialInstance && materialInstance.BaseMaterial == null)
                {
                    // Material instance without a base material should not be used
                }
                else
                {
                    switch (_material.Info.Domain)
                    {
                    case MaterialDomain.Surface:
                        surfaceMaterial = _material;
                        break;
                    case MaterialDomain.PostProcess:
                        postFxMaterial = _material;
                        break;
                    case MaterialDomain.Decal:
                        decalMaterial = _material;
                        break;
                    case MaterialDomain.GUI:
                        guiMaterial = _material;
                        break;
                    case MaterialDomain.Terrain:
                        break;
                    default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            // PostFx
            if (_previewModel.Model == null)
                throw new Exception("Missing preview model asset.");
            if (_previewModel.Model.WaitForLoaded())
                throw new Exception("Preview model asset failed to load.");
            var entries = _previewModel.Entries;
            if (entries.Length == 1)
                entries[0].Material = surfaceMaterial;
            _postFxMaterialsCache[0] = postFxMaterial;
            PostFxVolume.Settings.PostFxMaterials = _postFxMaterialsCache;

            // Decal
            if (decalMaterial && _decal == null)
            {
                _decal = Decal.New();
                _decal.Size = new Vector3(100.0f);
                _decal.LocalOrientation = Quaternion.RotationZ(Mathf.PiOverTwo);
                Task.CustomActors.Add(_decal);
            }
            if (_decal)
                _decal.Material = decalMaterial;

            // GUI
            if (guiMaterial && _guiMaterialControl == null)
            {
                _guiMaterialControl = new Image
                {
                    DockStyle = DockStyle.Fill,
                    KeepAspectRatio = false,
                    Brush = new MaterialBrush(),
                    Parent = this,
                    IndexInParent = 0,
                };
            }
            if (_guiMaterialControl != null)
            {
                ((MaterialBrush)_guiMaterialControl.Brush).Material = guiMaterial;
                _guiMaterialControl.Enabled = _guiMaterialControl.Visible = guiMaterial != null;
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _material = null;

            if (_guiMaterialControl != null)
            {
                _guiMaterialControl.Dispose();
                _guiMaterialControl = null;
            }

            // Ensure to cleanup created actor objects
            Object.Destroy(ref _previewModel);
            Object.Destroy(ref _decal);

            base.OnDestroy();
        }
    }
}
