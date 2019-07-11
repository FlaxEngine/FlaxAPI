// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

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
                Task.CustomActors.Add(_floorModel);

                // Enable shadows
                PreviewLight.ShadowsMode = ShadowsCastingMode.All;
                PreviewLight.CascadeCount = 2;
                PreviewLight.ShadowsDistance = 1000.0f;
                Task.Flags |= ViewFlags.Shadows;
            }

            private void OnShowFloorModelClicked(ContextMenuButton obj)
            {
                _floorModel.IsActive = !_floorModel.IsActive;
                _showFloorButton.Icon = _floorModel.IsActive ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;
                if (_window.Asset == null || !_window.Asset.IsLoaded)
                {
                    Render2D.DrawText(style.FontLarge, "Loading...", new Rectangle(Vector2.Zero, Size), Color.White, TextAlignment.Center, TextAlignment.Center);
                }
                if (_window._properties.BaseModel == null)
                {
                    Render2D.DrawText(style.FontLarge, "Missing Base Model", new Rectangle(Vector2.Zero, Size), Color.Red, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords);
                }
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                FlaxEngine.Object.Destroy(ref _floorModel);
                _showFloorButton = null;

                base.Dispose();
            }
        }

        /// <summary>
        /// The graph properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            private SkinnedModel _baseModel;

            [EditorOrder(10), EditorDisplay("General"), Tooltip("The base model used to preview the animation and prepare the graph (skeleton bones sstructure must match in instanced AnimationModel actors)")]
            public SkinnedModel BaseModel
            {
                get => _baseModel;
                set
                {
                    if (_baseModel != value)
                    {
                        _baseModel = value;
                        if (WindowReference != null)
                            WindowReference.PreviewActor.SkinnedModel = _baseModel;
                    }
                }
            }

            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor)), NoSerialize]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public AnimationGraphWindow WindowReference { get; set; }

            /// <summary>
            /// Custom editor for editing graph parameters collection.
            /// </summary>
            /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
            public class ParametersEditor : CustomEditor
            {
                private static readonly object[] DefaultAttributes = { new LimitAttribute(float.MinValue, float.MaxValue, 0.1f) };
                private int _parametersHash;

                private enum NewParameterType
                {
                    Bool = (int)ParameterType.Bool,
                    Integer = (int)ParameterType.Integer,
                    Float = (int)ParameterType.Float,
                    Vector2 = (int)ParameterType.Vector2,
                    Vector3 = (int)ParameterType.Vector3,
                    Vector4 = (int)ParameterType.Vector4,
                    Color = (int)ParameterType.Color,
                    Rotation = (int)ParameterType.Rotation,
                    Transform = (int)ParameterType.Transform,
                }

                /// <inheritdoc />
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var window = Values[0] as AnimationGraphWindow;
                    var asset = window?.Asset;
                    if (asset == null)
                    {
                        _parametersHash = -1;
                        layout.Label("No parameters");
                        return;
                    }
                    if (!asset.IsLoaded)
                    {
                        _parametersHash = -2;
                        layout.Label("Loading...");
                        return;
                    }
                    var parameters = window.PreviewActor.Parameters;
                    if (parameters == null || parameters.Length == 0)
                    {
                        _parametersHash = -1;
                        layout.Label("No parameters");
                        return;
                    }
                    _parametersHash = window.PreviewActor._parametersHash;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];
                        if (!p.IsPublic)
                            continue;

                        var pIndex = i;
                        var pValue = p.Value;
                        Type pType;
                        object[] attributes = null;
                        switch (p.Type)
                        {
                        case AnimationGraphParameterType.Asset:
                            pType = typeof(Asset);
                            break;
                        default:
                            pType = p.Value.GetType();
                            // TODO: support custom attributes with defined value range for parameter (min, max)
                            attributes = DefaultAttributes;
                            break;
                        }

                        var propertyValue = new CustomValueContainer(
                            pType,
                            pValue,
                            (instance, index) =>
                            {
                                // Get parameter
                                var win = (AnimationGraphWindow)instance;
                                return win.PreviewActor.Parameters[pIndex].Value;
                            },
                            (instance, index, value) =>
                            {
                                // Set parameter and surface parameter
                                var win = (AnimationGraphWindow)instance;
                                var action = new EditParamAction
                                {
                                    Window = win,
                                    Index = pIndex,
                                    Before = win.PreviewActor.Parameters[pIndex].Value,
                                    After = value,
                                };
                                win.Surface.Undo.AddAction(action);
                                action.Do();
                                win._paramValueChange = true;
                            },
                            attributes
                        );

                        var propertyLabel = new DragablePropertyNameLabel(p.Name);
                        propertyLabel.Tag = pIndex;
                        propertyLabel.MouseLeftDoubleClick += (label, location) => StartParameterRenaming(label);
                        propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(label, ref location);
                        propertyLabel.Drag = DragParameter;
                        var property = layout.AddPropertyItem(propertyLabel);
                        property.Object(propertyValue);
                    }

                    if (parameters.Length > 0)
                        layout.Space(10);

                    // Parameters creating
                    var paramType = layout.Enum(typeof(NewParameterType));
                    paramType.Value = (int)NewParameterType.Float;
                    var newParam = layout.Button("Add parameter");
                    newParam.Button.Clicked += () => AddParameter((ParameterType)paramType.Value);
                }

                private DragData DragParameter(DragablePropertyNameLabel label)
                {
                    var win = (AnimationGraphWindow)Values[0];
                    var animatedModel = win.PreviewActor;
                    var parameter = animatedModel.Parameters[(int)label.Tag];
                    return DragNames.GetDragData(SurfaceParameter.DragPrefix, parameter.Name);
                }

                /// <summary>
                /// Shows the parameter context menu.
                /// </summary>
                /// <param name="label">The label control.</param>
                /// <param name="targetLocation">The target location.</param>
                private void ShowParameterMenu(ClickablePropertyNameLabel label, ref Vector2 targetLocation)
                {
                    var contextMenu = new ContextMenu();
                    contextMenu.AddButton("Rename", () => StartParameterRenaming(label));
                    contextMenu.AddButton("Delete", () => DeleteParameter((int)label.Tag));
                    contextMenu.Show(label, targetLocation);
                }

                /// <summary>
                /// Adds the parameter.
                /// </summary>
                /// <param name="type">The type.</param>
                private void AddParameter(ParameterType type)
                {
                    var window = Values[0] as AnimationGraphWindow;
                    var asset = window?.Asset;
                    if (asset == null || !asset.IsLoaded)
                        return;

                    var action = new AddRemoveParamAction
                    {
                        Window = window,
                        IsAdd = true,
                        Name = "New parameter",
                        Type = type,
                    };
                    window.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <summary>
                /// Starts renaming parameter.
                /// </summary>
                /// <param name="label">The label control.</param>
                private void StartParameterRenaming(ClickablePropertyNameLabel label)
                {
                    var win = (AnimationGraphWindow)Values[0];
                    var animatedModel = win.PreviewActor;
                    var parameter = animatedModel.Parameters[(int)label.Tag];
                    var dialog = RenamePopup.Show(label, new Rectangle(0, 0, label.Width - 2, label.Height), parameter.Name, false);
                    dialog.Tag = label;
                    dialog.Renamed += OnParameterRenamed;
                }

                private void OnParameterRenamed(RenamePopup renamePopup)
                {
                    var win = (AnimationGraphWindow)Values[0];
                    var label = (ClickablePropertyNameLabel)renamePopup.Tag;
                    var index = (int)label.Tag;

                    var action = new RenameParamAction
                    {
                        Window = win,
                        Index = index,
                        Before = win.Surface.Parameters[index].Name,
                        After = renamePopup.Text,
                    };
                    win.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <summary>
                /// Removes the parameter.
                /// </summary>
                /// <param name="index">The index.</param>
                private void DeleteParameter(int index)
                {
                    var win = (AnimationGraphWindow)Values[0];

                    var action = new AddRemoveParamAction
                    {
                        Window = win,
                        IsAdd = false,
                        Index = index,
                    };
                    win.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <inheritdoc />
                public override void Refresh()
                {
                    var window = Values[0] as AnimationGraphWindow;
                    var asset = window?.Asset;
                    int parametersHash = -1;
                    if (asset)
                    {
                        if (asset.IsLoaded)
                        {
                            var parameters = window.PreviewActor.Parameters; // need to ask for params here to sync valid hash   
                            parametersHash = window.PreviewActor._parametersHash;
                        }
                        else
                        {
                            parametersHash = -2;
                        }
                    }

                    if (parametersHash != _parametersHash)
                    {
                        // Parameters has been modified (loaded/unloaded/edited)
                        RebuildLayout();
                    }
                }
            }

            /// <summary>
            /// Gathers parameters from the graph.
            /// </summary>
            /// <param name="window">The graph window.</param>
            public void OnLoad(AnimationGraphWindow window)
            {
                WindowReference = window;

                var model = window.PreviewActor;
                var param = model.GetParam(BaseModelId);
                BaseModel = param?.Value as SkinnedModel;
            }

            /// <summary>
            /// Saves the properties to the graph.
            /// </summary>
            /// <param name="window">The graph window.</param>
            public void OnSave(AnimationGraphWindow window)
            {
                var model = window.PreviewActor;
                var param = model.GetParam(BaseModelId);
                if (param != null)
                    param.Value = BaseModel;
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
                WindowReference = null;
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
            _toolstrip.AddButton(editor.Icons.Bone32, () => _preview.ShowBones = !_preview.ShowBones).SetAutoCheck(true).LinkTooltip("Show animated model bones debug view");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/animation/anim-graph/index.html")).LinkTooltip("See documentation to learn more");

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
        protected override void OnParamEditUndo(EditParamAction action, object value)
        {
            base.OnParamEditUndo(action, value);

            PreviewActor.Parameters[action.Index].Value = value;
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
