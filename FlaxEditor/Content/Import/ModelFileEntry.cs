////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
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
        /// Enable/disable drawing model two sided by force.
        /// </summary>
        [EditorOrder(50), Tooltip("Enable/disable drawing model two sided by force")]
        public bool ForceTwoSided { get; set; }

        /// <summary>
        /// Enable/disable meshes geometry optimization.
        /// </summary>
        [EditorOrder(60), Tooltip("Enable/disable meshes geometry optimization")]
        public bool OptimizeMeshes { get; set; } = true;

        /// <summary>
        /// The lighmap UVs source.
        /// </summary>
        [EditorOrder(70), Tooltip("Model lightmap UVs source")]
        public ModelLightmapUVsSource LighmapUVsSource { get; set; } = ModelLightmapUVsSource.Disable;
    }

    /// <summary>
    /// Model asset import entry.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.Import.AssetFileEntry" />
    public class ModelFileEntry : AssetFileEntry
    {
        private ModelImportSettings _settings = new ModelImportSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        public ModelFileEntry(string url, string resultUrl)
            : base(url, resultUrl)
        {
            // Try to restore target asset model import options (usefull for fast reimport)
            InternalOptions options;
            if (Internal_GetModelImportOptions(resultUrl, out options))
            {
                // Restore settings
                _settings.Scale = options.Scale;
                _settings.CalculateNormals = options.CalculateNormals;
                _settings.SmoothigNormalsAngle = options.SmoothigNormalsAngle;
                _settings.CalculateTangents = options.CalculateTangents;
                _settings.ForceTwoSided = options.ForceTwoSided;
                _settings.OptimizeMeshes = options.OptimizeMeshes;
                _settings.LighmapUVsSource = options.LighmapUVsSource;
            }
        }

        /// <inheritdoc />
        public override bool HasSettings => true;

        /// <inheritdoc />
        public override object Settings => _settings;

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalOptions
        {
            public float Scale;
            public bool CalculateNormals;
            public float SmoothigNormalsAngle;
            public bool CalculateTangents;
            public bool ForceTwoSided;
            public bool OptimizeMeshes;
            public ModelLightmapUVsSource LighmapUVsSource;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetModelImportOptions(string path, out InternalOptions result);
#endif

        #endregion
    }
}
