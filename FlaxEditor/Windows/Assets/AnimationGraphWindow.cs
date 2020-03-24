// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Local

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Animation Graph window allows to view and edit <see cref="AnimationGraph"/> asset.
    /// </summary>
    /// <seealso cref="AnimationGraph" />
    /// <seealso cref="AnimGraphSurface" />
    /// <seealso cref="AnimatedModelPreview" />
    public sealed class AnimationGraphWindow : VisjectSurfaceWindow<AnimationGraph, AnimGraphSurface, AnimatedModelPreview>
    {
        internal static Guid BaseModelId = new Guid(1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        private enum NewParameterType
        {
            Float = ParameterType.Float,
            Bool = ParameterType.Bool,
            Integer = ParameterType.Integer,
            Vector2 = ParameterType.Vector2,
            Vector3 = ParameterType.Vector3,
            Vector4 = ParameterType.Vector4,
            Color = ParameterType.Color,
            Rotation = ParameterType.Rotation,
            Transform = ParameterType.Transform,
        }

        private sealed class AnimationGraphPreview : AnimatedModelPreview
        {
            private readonly AnimationGraphWindow _window;
            private ContextMenuButton _showFloorButton;
            private StaticModel _floorModel;

            public AnimationGraphPreview(AnimationGraphWindow window)
            : base(true)
            {
                _window = window;

                // Show floor widget
                _showFloorButton = ViewWidgetShowMenu.AddButton("Floor", OnShowFloorModelClicked);
                _showFloorButton.Icon = Style.Current.CheckBoxTick;
                _showFloorButton.IndexInParent = 1;

                // Floor model
                _floorModel = StaticModel.New();
                _floorModel.Position = new Vector3(0, -25, 0);
                _floorModel.Scale = new Vector3(5, 0.5f, 5);
                _floorModel.Model = FlaxEngine.Content.LoadAsync<Model>(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cube.flax"));
                Task.AddCustomActor(_floorModel);

                // Enable shadows
                PreviewLight.ShadowsMode = ShadowsCastingMode.All;
                PreviewLight.CascadeCount = 2;
                PreviewLight.ShadowsDistance = 1000.0f;
                Task.ViewFlags |= ViewFlags.Shadows;
            }

            private void OnShowFloorModelClicked(ContextMenuButton obj)
            {
                _floorModel.IsActive = !_floorModel.IsActive;
                _showFloorButton.Icon = _floorModel.IsActive ? Style.Current.CheckBoxTick : SpriteHandle.Invalid;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                if (_window.Asset == null || !_window.Asset.IsLoaded)
                {
                    Render2D.DrawText(style.FontLarge, "Loading...", new Rectangle(Vector2.Zero, Size), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
                }
                if (_window._properties.BaseModel == null)
                {
                    Render2D.DrawText(style.FontLarge, "Missing Base Model", new Rectangle(Vector2.Zero, Size), Color.Red, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords);
                }
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                FlaxEngine.Object.Destroy(ref _floorModel);
                _showFloorButton = null;

                base.OnDestroy();
            }
        }

        /// <summary>
        /// The graph properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            private SkinnedModel _baseModel;

            [EditorOrder(10), EditorDisplay("General"), Tooltip("The base model used to preview the animation and prepare the graph (skeleton bones structure must match in instanced AnimationModel actors)")]
            public SkinnedModel BaseModel
            {
                get => _baseModel;
                set
                {
                    if (_baseModel != value)
                    {
                        _baseModel = value;
                        if (Window != null)
                            Window.PreviewActor.SkinnedModel = _baseModel;
                    }
                }
            }

            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor)), NoSerialize]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public AnimationGraphWindow Window { get; set; }

            [HideInEditor, Serialize]
            // ReSharper disable once UnusedMember.Local
            public List<SurfaceParameter> Parameters
            {
                get => Window.Surface.Parameters;
                set => throw new Exception("No setter.");
            }

            /// <summary>
            /// Gathers parameters from the graph.
            /// </summary>
            /// <param name="window">The graph window.</param>
            public void OnLoad(AnimationGraphWindow window)
            {
                Window = window;
                BaseModel = window.PreviewActor.GetParameterValue(BaseModelId) as SkinnedModel;
            }

            /// <summary>
            /// Saves the properties to the graph.
            /// </summary>
            /// <param name="window">The graph window.</param>
            public void OnSave(AnimationGraphWindow window)
            {
                var model = window.PreviewActor;
                model.SetParameterValue(BaseModelId, BaseModel);
                var surfaceParam = window.Surface.GetParameter(BaseModelId);
                if (surfaceParam != null)
                    surfaceParam.Value = BaseModel?.ID ?? Guid.Empty;
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                Window = null;
            }
        }

        private readonly NavigationBar _navigationBar;
        private readonly PropertiesProxy _properties;

        /// <summary>
        /// Gets the animated model actor used for the animation preview.
        /// </summary>
        public AnimatedModel PreviewActor => _preview.PreviewActor;

        /// <inheritdoc />
        public AnimationGraphWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            NewParameterTypes = typeof(NewParameterType);

            // Asset preview
            _preview = new AnimationGraphPreview(this)
            {
                ViewportCamera = new FPSCamera(),
                ScaleToFit = false,
                PlayAnimation = true,
                Parent = _split2.Panel1
            };

            // Asset properties proxy
            _properties = new PropertiesProxy();
            _propertiesEditor.Select(_properties);

            // Surface
            _surface = new AnimGraphSurface(this, Save, _undo)
            {
                Parent = _split1.Panel1,
                Enabled = false
            };
            _surface.ContextChanged += OnSurfaceContextChanged;

            // Toolstrip
            _toolstrip.AddButton(editor.Icons.Bone32, () => _preview.ShowNodes = !_preview.ShowNodes).SetAutoCheck(true).LinkTooltip("Show animated model nodes debug view");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Platform.OpenUrl(Utilities.Constants.DocsUrl + "manual/animation/anim-graph/index.html")).LinkTooltip("See documentation to learn more");

            // Navigation bar
            _navigationBar = new NavigationBar
            {
                Parent = this
            };
        }

        private void OnSurfaceContextChanged(VisjectSurfaceContext context)
        {
            _surface.UpdateNavigationBar(_navigationBar, _toolstrip);
        }

        /// <inheritdoc />
        protected override void SetParameter(int index, object value)
        {
            try
            {
                var param = Surface.Parameters[index];
                PreviewActor.SetParameterValue(param.ID, value);
            }
            catch
            {
                // Ignored
            }

            base.SetParameter(index, value);
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            if (PreviewActor != null)
                PreviewActor.AnimationGraph = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            PreviewActor.AnimationGraph = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override string SurfaceName => "Anim Graph";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface();
            set
            {
                if (value == null)
                {
                    // Error
                    Editor.LogError("Failed to save animation graph surface");
                    return;
                }

                // Save data to the temporary asset
                if (_asset.SaveSurface(value))
                {
                    // Error
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save animation graph surface data");
                    return;
                }
                _asset.Reload();

                // Reset any root motion
                _preview.PreviewActor.ResetLocalTransform();
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            // Load surface data from the asset
            byte[] data = _asset.LoadSurface();
            if (data == null)
            {
                // Error
                Editor.LogError("Failed to load animation graph surface data.");
                return true;
            }

            // Load surface graph
            if (_surface.Load(data))
            {
                // Error
                Editor.LogError("Failed to load animation graph surface.");
                return true;
            }

            // Init properties and parameters proxy
            _properties.OnLoad(this);

            // Update navbar
            _surface.UpdateNavigationBar(_navigationBar, _toolstrip);

            return false;
        }

        /// <inheritdoc />
        protected override bool SaveSurface()
        {
            // Sync edited parameters
            _properties.OnSave(this);

            // Save surface (will call SurfaceData setter)
            _surface.Save();

            return false;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            _navigationBar?.UpdateBounds(_toolstrip);
        }
    }
}
