// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.Rendering;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
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
        private enum NewParameterType
        {
            Float = ParameterType.Float,
            Bool = ParameterType.Bool,
            Integer = ParameterType.Integer,
            Vector2 = ParameterType.Vector2,
            Vector3 = ParameterType.Vector3,
            Vector4 = ParameterType.Vector4,
            Color = ParameterType.Color,
            Texture = ParameterType.Texture,
            CubeTexture = ParameterType.CubeTexture,
            NormalMap = ParameterType.NormalMap,
            RenderTarget = ParameterType.RenderTarget,
            RenderTargetArray = ParameterType.RenderTargetArray,
            RenderTargetCube = ParameterType.RenderTargetCube,
            RenderTargetVolume = ParameterType.RenderTargetVolume,
            Matrix = ParameterType.Matrix,
        }

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
            public MaterialWindow Window { get; set; }

            [HideInEditor, Serialize]
            // ReSharper disable once UnusedMember.Local
            public List<SurfaceParameter> Parameters
            {
                get => Window.Surface.Parameters;
                set => throw new Exception("No setter.");
            }

            /// <summary>
            /// Gathers parameters from the specified material.
            /// </summary>
            /// <param name="window">The window.</param>
            public void OnLoad(MaterialWindow window)
            {
                // Update cache
                var material = window.Asset;
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
                Window = window;
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
                Window = null;
            }
        }

        private readonly PropertiesProxy _properties;

        /// <inheritdoc />
        public MaterialWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            NewParameterTypes = typeof(NewParameterType);

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
        protected override void SetParameter(int index, object value)
        {
            Asset.Parameters[index].Value = value;

            base.SetParameter(index, value);
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
