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
    public enum ModelLightmapUVsSource
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
    /// Proxy object to present model import settings in <see cref="ImportFilesDialog"/>.
    /// </summary>
    public class ModelImportSettings
    {
        /// <summary>
        /// Custom import scale.
        /// </summary>
        [EditorOrder(10), Tooltip("Custom import scale")]
        public float Scale { get; set; } = 1.0f;

        /// <summary>
        /// True if calculate model normals, otherwise will import them.
        /// </summary>
        [EditorOrder(20), Tooltip("Enable model normal vectors recalculating")]
        public bool CalculateNormals { get; set; } = true;

        /// <summary>
        /// Calculated normals smoothing angle.
        /// </summary>
        [EditorOrder(30), Tooltip("Generated normal vector smoothing angle")]
        public float SmoothigNormalsAngle { get; set; } = 60.0f;

        /// <summary>
        /// True if calculate model tangents, otherwise will import them.
        /// </summary>
        [EditorOrder(40), Tooltip("Enable model tangent vectors recalculating")]
        public bool CalculateTangents { get; set; } = true;
        
        /// <summary>
        /// Enable/disable meshes geometry optimization.
        /// </summary>
        [EditorOrder(50), Tooltip("Enable/disable meshes geometry optimization")]
        public bool OptimizeMeshes { get; set; } = true;

        /// <summary>
        /// Enable/disable geometry merge for meshes with the same materials.
        /// </summary>
        [EditorOrder(60), Tooltip("Enable/disable geometry merge for meshes with the same materials")]
        public bool MergeMeshes { get; set; } = true;
        
        /// <summary>
        /// Enable/disable importing meshes Level of Details.
        /// </summary>
        [EditorOrder(70), EditorDisplay(null, "Import LODs"), Tooltip("Enable/disable importing meshes Level of Details")]
        public bool ImportLODs { get; set; } = true;

        /// <summary>
        /// Enable/disable importing vertex colors (channel 0 only).
        /// </summary>
        [EditorOrder(80), Tooltip("Enable/disable importing vertex colors (channel 0 only)")]
        public bool ImportVertexColors { get; set; } = true;

        /// <summary>
        /// The lighmap UVs source.
        /// </summary>
        [EditorOrder(90), EditorDisplay(null, "Lighmap UVs Source"), Tooltip("Model lightmap UVs source")]
        public ModelLightmapUVsSource LighmapUVsSource { get; set; } = ModelLightmapUVsSource.Disable;

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalOptions
        {
            public float Scale;
            public bool CalculateNormals;
            public float SmoothigNormalsAngle;
            public bool CalculateTangents;
            public bool OptimizeMeshes;
            public bool MergeMeshes;
            public bool ImportLODs;
            public bool ImportVertexColors;
            public ModelLightmapUVsSource LighmapUVsSource;
        }

        internal void ToInternal(out InternalOptions options)
        {
            options = new InternalOptions
            {
                Scale = Scale,
                CalculateNormals = CalculateNormals,
                SmoothigNormalsAngle = SmoothigNormalsAngle,
                CalculateTangents = CalculateTangents,
                OptimizeMeshes = OptimizeMeshes,
                MergeMeshes = MergeMeshes,
                ImportLODs = ImportLODs,
	            ImportVertexColors = ImportVertexColors,
                LighmapUVsSource = LighmapUVsSource
            };
        }
        
        internal void FromInternal(ref InternalOptions options)
        {
            Scale = options.Scale;
            CalculateNormals = options.CalculateNormals;
            SmoothigNormalsAngle = options.SmoothigNormalsAngle;
            CalculateTangents = options.CalculateTangents;
            OptimizeMeshes = options.OptimizeMeshes;
            MergeMeshes = options.MergeMeshes;
            ImportLODs = options.ImportLODs;
	        ImportVertexColors = options.ImportVertexColors;
            LighmapUVsSource = options.LighmapUVsSource;
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
