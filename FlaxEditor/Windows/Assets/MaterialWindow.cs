// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.GUI.Drag;
using FlaxEditor.History;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

// ReSharper disable MemberCanBePrivate.Local

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Material window allows to view and edit <see cref="Material"/> asset.
    /// </summary>
    /// <seealso cref="Material" />
    /// <seealso cref="MaterialSurface" />
    /// <seealso cref="MaterialPreview" />
    public sealed class MaterialWindow : VisjectSurfaceWindow<Material, MaterialSurface, MaterialPreview>
    {
        /// <summary>
        /// The material properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(10), EditorDisplay("General"), Tooltip("Material domain type")]
            public MaterialDomain Domain { get; set; }

            [EditorOrder(20), EditorDisplay("General"), Tooltip("Determinates how materials' color should be blended with the background colors")]
            public MaterialBlendMode BlendMode { get; set; }

            [EditorOrder(25), EditorDisplay("General"), Tooltip("Defines how material inputs and properties are combined to result the final surface color.")]
            public MaterialShadingModel ShadingModel { get; set; }

            [EditorOrder(30), EditorDisplay("General"), Tooltip("Indicates that material should be rendered without backface culling and normals should be fliped for the backfaces")]
            public bool TwoSided { get; set; }

            [EditorOrder(40), EditorDisplay("General"), Tooltip("True if render in wireframe mode")]
            public bool Wireframe { get; set; }

            [EditorOrder(100), EditorDisplay("Transparency"), Tooltip("Transparent materials lighting mode")]
            public MaterialTransparentLighting Lighting { get; set; }

            [EditorOrder(110), EditorDisplay("Transparency"), Tooltip("True if disable depth test when rendering material")]
            public bool DisableDepthTest { get; set; }

            [EditorOrder(120), EditorDisplay("Transparency"), Tooltip("True if disable reflections when rendering material")]
            public bool DisableReflections { get; set; }

            [HideInEditor, EditorOrder(130), EditorDisplay("Transparency"), Tooltip("True if disable atmosphere fog when rendering material")]
            public bool DisableFog { get; set; }

            [EditorOrder(140), EditorDisplay("Transparency"), Tooltip("True if disable distortion effect when rendering material")]
            public bool DisableDistortion { get; set; }

            [EditorOrder(150), EditorDisplay("Transparency"), Tooltip("Controls opacity values clipping point"), Limit(0.0f, 1.0f, 0.01f)]
            public float OpacityThreshold { get; set; }

            [EditorOrder(170), EditorDisplay("Tessellation"), Tooltip("Mesh tessellation method")]
            public TessellationMethod TessellationMode { get; set; }

            [EditorOrder(175), EditorDisplay("Tessellation"), Tooltip("Maximum triangle tessellation factor"), Limit(1, 60, 0.01f)]
            public int MaxTessellationFactor { get; set; }

            [EditorOrder(200), EditorDisplay("Misc"), Tooltip("True if disable depth buffer write when rendering material")]
            public bool DisableDepthWrite { get; set; }

            [EditorOrder(205), EditorDisplay("Misc"), Tooltip("If checked, material input normal will be assumed as world-space rather than tangent-space.")]
            public bool UseInputWorldSpaceNormal { get; set; }

            [EditorOrder(206), EditorDisplay("Misc", "Use Dithered LOD Transition"), Tooltip("If checked, material uses dithered model LOD transition for smoother LODs switching.")]
            public bool UseDitheredLODTransition { get; set; }

            [EditorOrder(210), EditorDisplay("Misc"), Tooltip("Controls mask values clipping point"), Limit(0.0f, 1.0f, 0.01f)]
            public float MaskThreshold { get; set; }

            [EditorOrder(215), EditorDisplay("Misc"), Tooltip("The decal material blending mode")]
            public MaterialDecalBlendingMode DecalBlendingMode { get; set; }

            [EditorOrder(220), EditorDisplay("Misc"), Tooltip("The post fx material rendering location")]
            public MaterialPostFxLocation PostFxLocation { get; set; }

            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor)), NoSerialize]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public MaterialWindow MaterialWinRef { get; set; }

            /// <summary>
            /// Custom editor for editing material parameters collection.
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
                    Texture = (int)ParameterType.Texture,
                    CubeTexture = (int)ParameterType.CubeTexture,
                    NormalMap = (int)ParameterType.NormalMap,
                    RenderTarget = (int)ParameterType.RenderTarget,
                    RenderTargetArray = (int)ParameterType.RenderTargetArray,
                    RenderTargetCube = (int)ParameterType.RenderTargetCube,
                    RenderTargetVolume = (int)ParameterType.RenderTargetVolume,
                    Matrix = (int)ParameterType.Matrix,
                }

                /// <inheritdoc />
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var materialWin = Values[0] as MaterialWindow;
                    var material = materialWin?.Asset;
                    if (material == null)
                    {
                        _parametersHash = -1;
                        layout.Label("No parameters");
                        return;
                    }
                    if (!material.IsLoaded)
                    {
                        _parametersHash = -2;
                        layout.Label("Loading...");
                        return;
                    }
                    _parametersHash = material._parametersHash;
                    var parameters = material.Parameters;

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
                        case MaterialParameterType.CubeTexture:
                            pType = typeof(CubeTexture);
                            break;
                        case MaterialParameterType.Texture:
                        case MaterialParameterType.NormalMap:
                            pType = typeof(Texture);
                            break;
                        case MaterialParameterType.RenderTarget:
                        case MaterialParameterType.RenderTargetArray:
                        case MaterialParameterType.RenderTargetCube:
                        case MaterialParameterType.RenderTargetVolume:
                            pType = typeof(RenderTarget);
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
                                // Get material parameter
                                var win = (MaterialWindow)instance;
                                return win.Asset.Parameters[pIndex].Value;
                            },
                            (instance, index, value) =>
                            {
                                // Set material parameter and surface parameter
                                var win = (MaterialWindow)instance;
                                var action = new EditParamAction
                                {
                                    Window = win,
                                    Index = pIndex,
                                    Before = win.Asset.Parameters[pIndex].Value,
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
                        propertyLabel.MouseLeftDoubleClick += (label, location) => StartParameterRenaming(pIndex, label);
                        propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(pIndex, label, ref location);
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
                    var win = (MaterialWindow)Values[0];
                    var material = win.Asset;
                    var parameter = material.Parameters[(int)label.Tag];
                    return DragNames.GetDragData(SurfaceParameter.DragPrefix, parameter.Name);
                }

                /// <summary>
                /// Shows the parameter context menu.
                /// </summary>
                /// <param name="index">The index.</param>
                /// <param name="label">The label control.</param>
                /// <param name="targetLocation">The target location.</param>
                private void ShowParameterMenu(int index, Control label, ref Vector2 targetLocation)
                {
                    var contextMenu = new ContextMenu();
                    contextMenu.AddButton("Rename", () => StartParameterRenaming(index, label));
                    contextMenu.AddButton("Delete", () => DeleteParameter(index));
                    contextMenu.Show(label, targetLocation);
                }

                /// <summary>
                /// Adds the parameter.
                /// </summary>
                /// <param name="type">The type.</param>
                private void AddParameter(ParameterType type)
                {
                    var win = Values[0] as MaterialWindow;
                    var material = win?.Asset;
                    if (material == null || !material.IsLoaded)
                        return;

                    var action = new AddRemoveParamAction
                    {
                        Window = win,
                        IsAdd = true,
                        Name = "New parameter",
                        Type = type,
                    };
                    win.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <summary>
                /// Starts renaming parameter.
                /// </summary>
                /// <param name="index">The index.</param>
                /// <param name="label">The label control.</param>
                private void StartParameterRenaming(int index, Control label)
                {
                    var win = (MaterialWindow)Values[0];
                    var material = win.Asset;
                    var parameter = material.Parameters[index];
                    var dialog = RenamePopup.Show(label, new Rectangle(0, 0, label.Width - 2, label.Height), parameter.Name, false);
                    dialog.Tag = index;
                    dialog.Renamed += OnParameterRenamed;
                }

                private void OnParameterRenamed(RenamePopup renamePopup)
                {
                    var index = (int)renamePopup.Tag;
                    var win = (MaterialWindow)Values[0];

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
                    var win = (MaterialWindow)Values[0];

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
                    base.Refresh();

                    var materialWin = Values[0] as MaterialWindow;
                    var material = materialWin?.Asset;
                    int parametersHash = -1;
                    if (material)
                    {
                        if (material.IsLoaded)
                        {
                            var parameters = material.Parameters; // need to ask for params here to sync valid hash   
                            parametersHash = material._parametersHash;
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
            /// Gathers parameters from the specified material.
            /// </summary>
            /// <param name="materialWin">The material window.</param>
            public void OnLoad(MaterialWindow materialWin)
            {
                // Update cache
                var material = materialWin.Asset;
                var info = material.Info;
                Wireframe = (info.Flags & MaterialFlags.Wireframe) != 0;
                TwoSided = (info.Flags & MaterialFlags.TwoSided) != 0;
                DisableDepthTest = (info.Flags & MaterialFlags.TransparentDisableDepthTest) != 0;
                DisableReflections = (info.Flags & MaterialFlags.TransparentDisableReflections) != 0;
                DisableFog = (info.Flags & MaterialFlags.TransparentDisableFog) != 0;
                DisableDepthWrite = (info.Flags & MaterialFlags.DisableDepthWrite) != 0;
                DisableDistortion = (info.Flags & MaterialFlags.TransparentDisableDistortion) != 0;
                UseInputWorldSpaceNormal = (info.Flags & MaterialFlags.InputWorldSpaceNormal) != 0;
                UseDitheredLODTransition = (info.Flags & MaterialFlags.UseDitheredLODTransition) != 0;
                OpacityThreshold = info.OpacityThreshold;
                TessellationMode = info.TessellationMode;
                MaxTessellationFactor = info.MaxTessellationFactor;
                MaskThreshold = info.MaskThreshold;
                DecalBlendingMode = info.DecalBlendingMode;
                PostFxLocation = info.PostFxLocation;
                BlendMode = info.BlendMode;
                ShadingModel = info.ShadingModel;
                Lighting = info.TransparentLighting;
                Domain = info.Domain;

                // Link
                MaterialWinRef = materialWin;
            }

            /// <summary>
            /// Saves the material properties to the material info structure.
            /// </summary>
            /// <param name="info">The material info.</param>
            public void OnSave(ref MaterialInfo info)
            {
                // Update flags
                if (Wireframe)
                    info.Flags |= MaterialFlags.Wireframe;
                if (TwoSided)
                    info.Flags |= MaterialFlags.TwoSided;
                if (DisableDepthTest)
                    info.Flags |= MaterialFlags.TransparentDisableDepthTest;
                if (DisableReflections)
                    info.Flags |= MaterialFlags.TransparentDisableReflections;
                if (DisableFog)
                    info.Flags |= MaterialFlags.TransparentDisableFog;
                if (DisableDepthWrite)
                    info.Flags |= MaterialFlags.DisableDepthWrite;
                if (DisableDistortion)
                    info.Flags |= MaterialFlags.TransparentDisableDistortion;
                if (UseInputWorldSpaceNormal)
                    info.Flags |= MaterialFlags.InputWorldSpaceNormal;
                if (UseDitheredLODTransition)
                    info.Flags |= MaterialFlags.UseDitheredLODTransition;
                info.OpacityThreshold = OpacityThreshold;
                info.TessellationMode = TessellationMode;
                info.MaxTessellationFactor = MaxTessellationFactor;
                info.MaskThreshold = MaskThreshold;
                info.DecalBlendingMode = DecalBlendingMode;
                info.PostFxLocation = PostFxLocation;
                info.BlendMode = BlendMode;
                info.ShadingModel = ShadingModel;
                info.TransparentLighting = Lighting;
                info.Domain = Domain;
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                MaterialWinRef = null;
            }
        }

        private readonly PropertiesProxy _properties;

        /// <inheritdoc />
        public MaterialWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Asset preview
            _preview = new MaterialPreview(true)
            {
                Parent = _split2.Panel1
            };

            // Asset properties proxy
            _properties = new PropertiesProxy();
            _propertiesEditor.Select(_properties);

            // Surface
            _surface = new MaterialSurface(this, Save, _undo)
            {
                Parent = _split1.Panel1,
                Enabled = false
            };

            // Toolstrip
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.BracketsSlash32, () => ShowSourceCode(_asset)).LinkTooltip("Show generated shader source code");
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/graphics/materials/index.html")).LinkTooltip("See documentation to learn more");
        }

        /// <summary>
        /// Shows the material source code window.
        /// </summary>
        /// <param name="material">The material asset.</param>
        public static void ShowSourceCode(Material material)
        {
            var source = Editor.GetMaterialShaderSourceCode(material);
            Utilities.Utils.ShowSourceCode(source, "Material Source");
        }

        /// <summary>
        /// Gets material info from UI.
        /// </summary>
        /// <param name="info">Output info.</param>
        public void FillMaterialInfo(out MaterialInfo info)
        {
            info = MaterialInfo.CreateDefault();
            _properties.OnSave(ref info);
        }

        /// <summary>
        /// Gets or sets the main material node.
        /// </summary>
        public Surface.Archetypes.Material.SurfaceNodeMaterial MainNode
        {
            get
            {
                var mainNode = _surface.FindNode(1, 1) as Surface.Archetypes.Material.SurfaceNodeMaterial;
                if (mainNode == null)
                {
                    // Error
                    Editor.LogError("Failed to find main material node.");
                }
                return mainNode;
            }
        }

        /// <inheritdoc />
        protected override void OnParamEditUndo(EditParamAction action, object value)
        {
            base.OnParamEditUndo(action, value);

            // Update the asset value to have nice live preview
            Asset.Parameters[action.Index].Value = value;
        }

        /// <inheritdoc />
        protected override void OnPropertyEdited()
        {
            base.OnPropertyEdited();

            // Refresh main node
            var mainNode = MainNode;
            mainNode?.UpdateBoxes();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Material = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Material = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override string SurfaceName => "Material";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface(true);
            set
            {
                // Create material info
                MaterialInfo info;
                FillMaterialInfo(out info);

                // Save data to the temporary material
                if (_asset.SaveSurface(value, info))
                {
                    // Error
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save material surface data");
                }
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            // Init material properties and parameters proxy
            _properties.OnLoad(this);

            // Load surface data from the asset
            byte[] data = _asset.LoadSurface(true);
            if (data == null)
            {
                // Error
                Editor.LogError("Failed to load material surface data.");
                return true;
            }

            // Load surface graph
            if (_surface.Load(data))
            {
                // Error
                Editor.LogError("Failed to load material surface.");
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override bool SaveSurface()
        {
            _surface.Save();
            return false;
        }

        /// <inheritdoc />
        protected override bool SaveToOriginal()
        {
            // Copy shader cache from the temporary Particle Emitter (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);

            return base.SaveToOriginal();
        }
    }
}
