////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
		[EditorOrder(0), Tooltip("Type of the imorted asset")]
		public ModelType Type { get; set; } = ModelType.Model;

        /// <summary>
        /// True if calculate model normals, otherwise will import them.
        /// </summary>
        [EditorOrder(20), EditorDisplay("Geometry"), Tooltip("Enable model normal vectors recalculating")]
        public bool CalculateNormals { get; set; } = true;

        /// <summary>
        /// Calculated normals smoothing angle.
        /// </summary>
        [EditorOrder(30), Limit(0, 175, 0.1f), EditorDisplay("Geometry"), Tooltip("Specifies the maximum angle (in degrees) that may be between two face normals at the same vertex position that their are smoothed together.")]
        public float SmoothigNormalsAngle { get; set; } = 175.0f;

        /// <summary>
        /// True if calculate model tangents, otherwise will import them.
        /// </summary>
        [EditorOrder(40), EditorDisplay("Geometry"), Tooltip("Enable model tangent vectors recalculating")]
        public bool CalculateTangents { get; set; } = true;

		/// <summary>
		/// Calculated normals smoothing angle.
		/// </summary>
		[EditorOrder(45), Limit(0, 45, 0.1f), EditorDisplay("Geometry"), Tooltip("Specifies the maximum angle (in degrees) that may be between two vertex tangents that their tangents and bi-tangents are smoothed.")]
		public float SmoothigTangentsAngle { get; set; } = 45.0f;

		/// <summary>
		/// Enable/disable meshes geometry optimization.
		/// </summary>
		[EditorOrder(50), EditorDisplay("Geometry"), Tooltip("Enable/disable meshes geometry optimization")]
        public bool OptimizeMeshes { get; set; } = true;

        /// <summary>
        /// Enable/disable geometry merge for meshes with the same materials.
        /// </summary>
        [EditorOrder(60), EditorDisplay("Geometry"), Tooltip("Enable/disable geometry merge for meshes with the same materials")]
        public bool MergeMeshes { get; set; } = true;
        
        /// <summary>
        /// Enable/disable importing meshes Level of Details.
        /// </summary>
        [EditorOrder(70), EditorDisplay("Geometry", "Import LODs"), Tooltip("Enable/disable importing meshes Level of Details")]
        public bool ImportLODs { get; set; } = true;

        /// <summary>
        /// Enable/disable importing vertex colors (channel 0 only).
        /// </summary>
        [EditorOrder(80), EditorDisplay("Geometry"), Tooltip("Enable/disable importing vertex colors (channel 0 only)")]
        public bool ImportVertexColors { get; set; } = true;

        /// <summary>
        /// The lighmap UVs source.
        /// </summary>
        [EditorOrder(90), EditorDisplay("Geometry", "Lighmap UVs Source"), Tooltip("Model lightmap UVs source")]
        public ModelLightmapUVsSource LighmapUVsSource { get; set; } = ModelLightmapUVsSource.Disable;

		/// <summary>
		/// Custom uniform import scale.
		/// </summary>
		[EditorOrder(500), EditorDisplay("Transform"), Tooltip("Custom uniform import scale")]
		public float Scale { get; set; } = 1.0f;

		/// <summary>
		/// Custom import geometry rotation.
		/// </summary>
		[EditorOrder(510), EditorDisplay("Transform"), Tooltip("Custom import geometry rotation")]
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		/// <summary>
		/// Custom import geometry offset.
		/// </summary>
		[EditorOrder(520), EditorDisplay("Transform"), Tooltip("Custom import geometry offse")]
		public Vector3 Translation { get; set; } = Vector3.Zero;

		/// <summary>
		/// The imported animation duration mode.
		/// </summary>
		[EditorOrder(1000), EditorDisplay("Animation"), Tooltip("Imported animation duration mode. Can use the original value or overriden by settings.")]
		public AnimationDuration Duration { get; set; } = AnimationDuration.Imported;

		/// <summary>
		/// The imported animation first frame index. Used only if Duration mode is set to Custom.
		/// </summary>
		[EditorOrder(1010), Limit(0), EditorDisplay("Animation"), Tooltip("Imported animation first frame index. Used only if Duration mode is set to Custom.")]
		public float FramesRangeStart { get; set; } = 0;

		/// <summary>
		/// The imported animation end frame index. Used only if Duration mode is set to Custom.
		/// </summary>
		[EditorOrder(1020), Limit(0), EditorDisplay("Animation"), Tooltip("Imported animation last frame index. Used only if Duration mode is set to Custom.")]
		public float FramesRangeEnd { get; set; } = 0;

		/// <summary>
		/// The imported animation sampling rate. If value is 0 then the original animation speed will be used.
		/// </summary>
		[EditorOrder(1030), Limit(0, 1000, 0.01f), EditorDisplay("Animation"), Tooltip("The imported animation sampling rate. If value is 0 then the original animation speed will be used.")]
		public float SamplingRate { get; set; } = 0.0f;

		/// <summary>
		/// The imported animation will have removed tracks with no keyframes or unspeficied data.
		/// </summary>
		[EditorOrder(1040), EditorDisplay("Animation"), Tooltip("The imported animation will have removed tracks with no keyframes or unspeficied data.")]
		public bool SkipEmptyCurves { get; set; } = true;

		/// <summary>
		/// The imported animation channels will be optimized to remove redundant keyframes.
		/// </summary>
		[EditorOrder(1050), EditorDisplay("Animation"), Tooltip("The imported animation channels will be optimized to remove redundant keyframes.")]
		public bool OptimizeKeyframes { get; set; } = true;
		
		/// <summary>
		/// Enables root motion extraction support from this animation.
		/// </summary>
		[EditorOrder(1060), EditorDisplay("Animation"), Tooltip("Enables root motion extraction support from this animation.")]
		public bool EnableRootMotion { get; set; } = false;

		/// <summary>
		/// The custom node name to be used as a root motion source. If not specified the actual root node will be used.
		/// </summary>
		[EditorOrder(1070), EditorDisplay("Animation"), Tooltip("The custom node name to be used as a root motion source. If not specified the actual root node will be used.")]
		public string RootNodeName { get; set; }

		[StructLayout(LayoutKind.Sequential)]
        internal struct InternalOptions
        {
            public ModelType Type;

	        // Geometry
            public bool CalculateNormals;
            public float SmoothigNormalsAngle;
            public float SmoothigTangentsAngle;
            public bool CalculateTangents;
            public bool OptimizeMeshes;
            public bool MergeMeshes;
            public bool ImportLODs;
            public bool ImportVertexColors;
            public ModelLightmapUVsSource LighmapUVsSource;

			// Transform
	        public float Scale;
	        public Quaternion Rotation;
	        public Vector3 Translation;

			// Animation
			public AnimationDuration Duration;
	        public float FramesRangeStart;
	        public float FramesRangeEnd;
	        public float SamplingRate;
	        public bool SkipEmptyCurves;
	        public bool OptimizeKeyframes;
	        public bool EnableRootMotion;
	        public string RootNodeName;
		}

		internal void ToInternal(out InternalOptions options)
        {
            options = new InternalOptions
            {
	            Type = Type,
                CalculateNormals = CalculateNormals,
                SmoothigNormalsAngle = SmoothigNormalsAngle,
	            SmoothigTangentsAngle = SmoothigTangentsAngle,
                CalculateTangents = CalculateTangents,
                OptimizeMeshes = OptimizeMeshes,
                MergeMeshes = MergeMeshes,
                ImportLODs = ImportLODs,
	            ImportVertexColors = ImportVertexColors,
                LighmapUVsSource = LighmapUVsSource,
	            Scale = Scale,
				Rotation = Rotation,
	            Translation = Translation,
				Duration = Duration,
	            FramesRangeStart = FramesRangeStart,
	            FramesRangeEnd = FramesRangeEnd,
	            SamplingRate = SamplingRate,
	            SkipEmptyCurves = SkipEmptyCurves,
	            OptimizeKeyframes = OptimizeKeyframes,
	            EnableRootMotion = EnableRootMotion,
	            RootNodeName = RootNodeName,
            };
        }
        
        internal void FromInternal(ref InternalOptions options)
        {
	        Type = options.Type;
            CalculateNormals = options.CalculateNormals;
            SmoothigNormalsAngle = options.SmoothigNormalsAngle;
	        SmoothigTangentsAngle = options.SmoothigTangentsAngle;
            CalculateTangents = options.CalculateTangents;
            OptimizeMeshes = options.OptimizeMeshes;
            MergeMeshes = options.MergeMeshes;
            ImportLODs = options.ImportLODs;
	        ImportVertexColors = options.ImportVertexColors;
            LighmapUVsSource = options.LighmapUVsSource;
	        Scale = options.Scale;
	        Rotation = options.Rotation;
	        Translation = options.Translation;
	        FramesRangeStart = options.FramesRangeStart;
	        FramesRangeEnd = options.FramesRangeEnd;
	        SamplingRate = options.SamplingRate;
	        SkipEmptyCurves = options.SkipEmptyCurves;
	        OptimizeKeyframes = options.OptimizeKeyframes;
	        EnableRootMotion = options.EnableRootMotion;
	        RootNodeName = options.RootNodeName;
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
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        public ModelImportEntry(string url, string resultUrl)
            : base(url, resultUrl)
        {
            // Try to restore target asset model import options (usefull for fast reimport)
            ModelImportSettings.TryRestore(ref _settings, resultUrl);
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
