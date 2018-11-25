// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Importing model lightmap UVs source
    /// </summary>
    public enum ModelLightmapUVsSource : int
    {
        /// <summary>
        /// No lightmap UVs.
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Generate lightmap UVs from model geometry.
        /// </summary>
        Generate = 1,

        /// <summary>
        /// The texcoords channel 0.
        /// </summary>
        Channel0 = 2,

        /// <summary>
        /// The texcoords channel 1.
        /// </summary>
        Channel1 = 3,

        /// <summary>
        /// The texcoords channel 2.
        /// </summary>
        Channel2 = 4,

        /// <summary>
        /// The texcoords channel 3.
        /// </summary>
        Channel3 = 5
    }

    /// <summary>
    /// Declares the imported data type.
    /// </summary>
    public enum ModelType : int
    {
        /// <summary>
        /// The model asset.
        /// </summary>
        Model = 0,

        /// <summary>
        /// The skinned model asset.
        /// </summary>
        SkinnedModel = 1,

        /// <summary>
        /// The animation asset.
        /// </summary>
        Animation = 2,
    }

    /// <summary>
    /// Declares the imported animation clip duration.
    /// </summary>
    public enum AnimationDuration : int
    {
        /// <summary>
        /// The imported duration.
        /// </summary>
        Imported = 0,

        /// <summary>
        /// The custom duration specified via keyframes range.
        /// </summary>
        Custom = 1,
    }

    /// <summary>
    /// Proxy object to present model import settings in <see cref="ImportFilesDialog"/>.
    /// </summary>
    public class ModelImportSettings
    {
        /// <summary>
        /// Gets or sets the type of the imported asset.
        /// </summary>
        [EditorOrder(0), Tooltip("Type of the imported asset")]
        public ModelType Type { get; set; } = ModelType.Model;

        /// <summary>
        /// True if calculate model normals, otherwise will import them.
        /// </summary>
        [EditorOrder(20), DefaultValue(false), EditorDisplay("Geometry"), Tooltip("Enable model normal vectors recalculating")]
        public bool CalculateNormals { get; set; } = false;

        /// <summary>
        /// Calculated normals smoothing angle.
        /// </summary>
        [EditorOrder(30), DefaultValue(175.0f), Limit(0, 175, 0.1f), EditorDisplay("Geometry"), Tooltip("Specifies the maximum angle (in degrees) that may be between two face normals at the same vertex position that their are smoothed together. The default value is 175.")]
        public float SmoothingNormalsAngle { get; set; } = 175.0f;

        /// <summary>
        /// True if calculate model tangents, otherwise will import them.
        /// </summary>
        [EditorOrder(40), DefaultValue(true), EditorDisplay("Geometry"), Tooltip("Enable model tangent vectors recalculating")]
        public bool CalculateTangents { get; set; } = true;

        /// <summary>
        /// Calculated normals smoothing angle.
        /// </summary>
        [EditorOrder(45), DefaultValue(45.0f), Limit(0, 45, 0.1f), EditorDisplay("Geometry"), Tooltip("Specifies the maximum angle (in degrees) that may be between two vertex tangents that their tangents and bi-tangents are smoothed. The default value is 45.")]
        public float SmoothingTangentsAngle { get; set; } = 45.0f;

        /// <summary>
        /// Enable/disable meshes geometry optimization.
        /// </summary>
        [EditorOrder(50), DefaultValue(true), EditorDisplay("Geometry"), Tooltip("Enable/disable meshes geometry optimization")]
        public bool OptimizeMeshes { get; set; } = true;

        /// <summary>
        /// Enable/disable geometry merge for meshes with the same materials.
        /// </summary>
        [EditorOrder(60), DefaultValue(true), EditorDisplay("Geometry"), Tooltip("Enable/disable geometry merge for meshes with the same materials")]
        public bool MergeMeshes { get; set; } = true;

        /// <summary>
        /// Enable/disable importing meshes Level of Details.
        /// </summary>
        [EditorOrder(70), DefaultValue(true), EditorDisplay("Geometry", "Import LODs"), Tooltip("Enable/disable importing meshes Level of Details")]
        public bool ImportLODs { get; set; } = true;

        /// <summary>
        /// Enable/disable importing vertex colors (channel 0 only).
        /// </summary>
        [EditorOrder(80), DefaultValue(true), EditorDisplay("Geometry"), Tooltip("Enable/disable importing vertex colors (channel 0 only)")]
        public bool ImportVertexColors { get; set; } = true;

        /// <summary>
        /// The lightmap UVs source.
        /// </summary>
        [EditorOrder(90), DefaultValue(ModelLightmapUVsSource.Disable), EditorDisplay("Geometry", "Lightmap UVs Source"), Tooltip("Model lightmap UVs source")]
        public ModelLightmapUVsSource LightmapUVsSource { get; set; } = ModelLightmapUVsSource.Disable;

        /// <summary>
        /// Custom uniform import scale.
        /// </summary>
        [EditorOrder(500), DefaultValue(1.0f), EditorDisplay("Transform"), Tooltip("Custom uniform import scale")]
        public float Scale { get; set; } = 1.0f;

        /// <summary>
        /// Custom import geometry rotation.
        /// </summary>
        [DefaultValue(typeof(Quaternion), "0,0,0,1")]
        [EditorOrder(510), EditorDisplay("Transform"), Tooltip("Custom import geometry rotation")]
        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        /// <summary>
        /// Custom import geometry offset.
        /// </summary>
        [DefaultValue(typeof(Vector3), "0,0,0")]
        [EditorOrder(520), EditorDisplay("Transform"), Tooltip("Custom import geometry offset")]
        public Vector3 Translation { get; set; } = Vector3.Zero;

        /// <summary>
        /// If checked, the imported geometry will be shifted to the center of mass.
        /// </summary>
        [EditorOrder(530), DefaultValue(false), EditorDisplay("Transform"), Tooltip("If checked, the imported geometry will be shifted to the center of mass.")]
        public bool CenterGeometry { get; set; } = false;

        /// <summary>
        /// The imported animation duration mode.
        /// </summary>
        [EditorOrder(1000), DefaultValue(AnimationDuration.Imported), EditorDisplay("Animation"), Tooltip("Imported animation duration mode. Can use the original value or overriden by settings.")]
        public AnimationDuration Duration { get; set; } = AnimationDuration.Imported;

        /// <summary>
        /// The imported animation first frame index. Used only if Duration mode is set to Custom.
        /// </summary>
        [EditorOrder(1010), DefaultValue(0.0f), Limit(0), EditorDisplay("Animation"), Tooltip("Imported animation first frame index. Used only if Duration mode is set to Custom.")]
        public float FramesRangeStart { get; set; } = 0;

        /// <summary>
        /// The imported animation end frame index. Used only if Duration mode is set to Custom.
        /// </summary>
        [EditorOrder(1020), DefaultValue(0.0f), Limit(0), EditorDisplay("Animation"), Tooltip("Imported animation last frame index. Used only if Duration mode is set to Custom.")]
        public float FramesRangeEnd { get; set; } = 0;

        /// <summary>
        /// The imported animation default frame rate. Can specify the default frames per second amount for imported animation. If value is 0 then the original animation frame rate will be used.
        /// </summary>
        [EditorOrder(1025), DefaultValue(0.0f), Limit(0, 1000, 0.01f), EditorDisplay("Animation"), Tooltip("The imported animation default frame rate. Can specify the default frames per second amount for imported animation. If value is 0 then the original animation frame rate will be used.")]
        public float DefaultFrameRate { get; set; } = 0.0f;

        /// <summary>
        /// The imported animation sampling rate. If value is 0 then the original animation speed will be used.
        /// </summary>
        [EditorOrder(1030), DefaultValue(0.0f), Limit(0, 1000, 0.01f), EditorDisplay("Animation"), Tooltip("The imported animation sampling rate. If value is 0 then the original animation speed will be used.")]
        public float SamplingRate { get; set; } = 0.0f;

        /// <summary>
        /// The imported animation will have removed tracks with no keyframes or unspecified data.
        /// </summary>
        [EditorOrder(1040), DefaultValue(true), EditorDisplay("Animation"), Tooltip("The imported animation will have removed tracks with no keyframes or unspecified data.")]
        public bool SkipEmptyCurves { get; set; } = true;

        /// <summary>
        /// The imported animation channels will be optimized to remove redundant keyframes.
        /// </summary>
        [EditorOrder(1050), DefaultValue(true), EditorDisplay("Animation"), Tooltip("The imported animation channels will be optimized to remove redundant keyframes.")]
        public bool OptimizeKeyframes { get; set; } = true;

        /// <summary>
        /// Enables root motion extraction support from this animation.
        /// </summary>
        [EditorOrder(1060), DefaultValue(false), EditorDisplay("Animation"), Tooltip("Enables root motion extraction support from this animation.")]
        public bool EnableRootMotion { get; set; } = false;

        /// <summary>
        /// The custom node name to be used as a root motion source. If not specified the actual root node will be used.
        /// </summary>
        [EditorOrder(1070), DefaultValue(null), EditorDisplay("Animation"), Tooltip("The custom node name to be used as a root motion source. If not specified the actual root node will be used.")]
        public string RootNodeName { get; set; }

        /// <summary>
        /// The zero-based index for the animation clip to import. If the source file has more than one animation it can be used to pick a desire clip.
        /// </summary>
        [EditorOrder(1080), DefaultValue(-1), EditorDisplay("Animation"), Tooltip("The zero-based index for the animation clip to import. If the source file has more than one animation it can be used to pick a desire clip.")]
        public int AnimationIndex { get; set; } = -1;

        /// <summary>
        /// If checked, the importer will try to restore the model material slots.
        /// </summary>
        [EditorOrder(1100), DefaultValue(false), EditorDisplay("Miscellaneous", "Restore Materials On Reimport"), Tooltip("If checked, the importer will try to restore the model material slots.")]
        public bool RestoreMaterialsOnReimport { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalOptions
        {
            public ModelType Type;

            // Geometry
            public byte CalculateNormals;
            public float SmoothingNormalsAngle;
            public float SmoothingTangentsAngle;
            public byte CalculateTangents;
            public byte OptimizeMeshes;
            public byte MergeMeshes;
            public byte ImportLODs;
            public byte ImportVertexColors;
            public ModelLightmapUVsSource LightmapUVsSource;

            // Transform
            public float Scale;
            public Quaternion Rotation;
            public Vector3 Translation;
            public byte CenterGeometry;

            // Animation
            public AnimationDuration Duration;
            public float FramesRangeStart;
            public float FramesRangeEnd;
            public float DefaultFrameRate;
            public float SamplingRate;
            public byte SkipEmptyCurves;
            public byte OptimizeKeyframes;
            public byte EnableRootMotion;
            public string RootNodeName;
            public int AnimationIndex;

            // Misc
            public byte RestoreMaterialsOnReimport;
        }

        internal void ToInternal(out InternalOptions options)
        {
            options = new InternalOptions
            {
                Type = Type,
                CalculateNormals = (byte)(CalculateNormals ? 1 : 0),
                SmoothingNormalsAngle = SmoothingNormalsAngle,
                SmoothingTangentsAngle = SmoothingTangentsAngle,
                CalculateTangents = (byte)(CalculateTangents ? 1 : 0),
                OptimizeMeshes = (byte)(OptimizeMeshes ? 1 : 0),
                MergeMeshes = (byte)(MergeMeshes ? 1 : 0),
                ImportLODs = (byte)(ImportLODs ? 1 : 0),
                ImportVertexColors = (byte)(ImportVertexColors ? 1 : 0),
                LightmapUVsSource = LightmapUVsSource,
                Scale = Scale,
                Rotation = Rotation,
                Translation = Translation,
                CenterGeometry = (byte)(CenterGeometry ? 1 : 0),
                Duration = Duration,
                FramesRangeStart = FramesRangeStart,
                FramesRangeEnd = FramesRangeEnd,
                DefaultFrameRate = DefaultFrameRate,
                SamplingRate = SamplingRate,
                SkipEmptyCurves = (byte)(SkipEmptyCurves ? 1 : 0),
                OptimizeKeyframes = (byte)(OptimizeKeyframes ? 1 : 0),
                EnableRootMotion = (byte)(EnableRootMotion ? 1 : 0),
                RootNodeName = RootNodeName,
                AnimationIndex = AnimationIndex,
                RestoreMaterialsOnReimport = (byte)(RestoreMaterialsOnReimport ? 1 : 0),
            };
        }

        internal void FromInternal(ref InternalOptions options)
        {
            Type = options.Type;
            CalculateNormals = options.CalculateNormals != 0;
            SmoothingNormalsAngle = options.SmoothingNormalsAngle;
            SmoothingTangentsAngle = options.SmoothingTangentsAngle;
            CalculateTangents = options.CalculateTangents != 0;
            OptimizeMeshes = options.OptimizeMeshes != 0;
            MergeMeshes = options.MergeMeshes != 0;
            ImportLODs = options.ImportLODs != 0;
            ImportVertexColors = options.ImportVertexColors != 0;
            LightmapUVsSource = options.LightmapUVsSource;
            Scale = options.Scale;
            Rotation = options.Rotation;
            Translation = options.Translation;
            CenterGeometry = options.CenterGeometry != 0;
            FramesRangeStart = options.FramesRangeStart;
            FramesRangeEnd = options.FramesRangeEnd;
            DefaultFrameRate = options.DefaultFrameRate;
            SamplingRate = options.SamplingRate;
            SkipEmptyCurves = options.SkipEmptyCurves != 0;
            OptimizeKeyframes = options.OptimizeKeyframes != 0;
            EnableRootMotion = options.EnableRootMotion != 0;
            RootNodeName = options.RootNodeName;
            AnimationIndex = options.AnimationIndex;
            RestoreMaterialsOnReimport = options.RestoreMaterialsOnReimport != 0;
        }

        /// <summary>
        /// Tries the restore the asset import options from the target resource file.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="assetPath">The asset path.</param>
        /// <returns>True settings has been restored, otherwise false.</returns>
        public static bool TryRestore(ref ModelImportSettings options, string assetPath)
        {
            if (ModelImportEntry.Internal_GetModelImportOptions(assetPath, out var internalOptions))
            {
                // Restore settings
                options.FromInternal(ref internalOptions);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Model asset import entry.
    /// </summary>
    /// <seealso cref="AssetImportEntry" />
    public class ModelImportEntry : AssetImportEntry
    {
        private ModelImportSettings _settings = new ModelImportSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelImportEntry"/> class.
        /// </summary>
        /// <param name="request">The import request.</param>
        public ModelImportEntry(ref Request request)
        : base(ref request)
        {
            // Try to restore target asset model import options (useful for fast reimport)
            ModelImportSettings.TryRestore(ref _settings, ResultUrl);
        }

        /// <inheritdoc />
        public override object Settings => _settings;

        /// <inheritdoc />
        public override bool TryOverrideSettings(object settings)
        {
            if (settings is ModelImportSettings o)
            {
                _settings = o;
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Import()
        {
            return Editor.Import(SourceUrl, ResultUrl, _settings);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetModelImportOptions(string path, out ModelImportSettings.InternalOptions result);
#endif

        #endregion
    }
}
