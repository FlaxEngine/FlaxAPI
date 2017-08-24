////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Eye adaptation technique.
    /// </summary>
    public enum EyeAdaptationTechnique
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The manual.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// The automatic.
        /// </summary>
        Auto = 2
    }

    /// <summary>
    /// Tone mapping techniques.
    /// </summary>
    public enum ToneMappingTechniqe
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The logarithmic.
        /// </summary>
        Logarithmic = 1,

        /// <summary>
        /// The exponential.
        /// </summary>
        Exponential = 2,

        /// <summary>
        /// The Drago-logarithmic.
        /// </summary>
        DragoLogarithmic = 3,

        /// <summary>
        /// The Reinhard.
        /// </summary>
        Reinhard = 4,

        /// <summary>
        /// The modified Reinhard.
        /// </summary>
        ReinhardModified = 5,

        /// <summary>
        /// The filmic ALU.
        /// </summary>
        FilmicALU = 6
    }

    /// <summary>
    /// Depth of field bokeh shape types.
    /// </summary>
    public enum BokehShapeType
    {
        /// <summary>
        /// The hexagon shape.
        /// </summary>
        Hexagon = 0,

        /// <summary>
        /// The octagon shape.
        /// </summary>
        Octagon = 1,

        /// <summary>
        /// The circle shape.
        /// </summary>
        Circle = 2,

        /// <summary>
        /// The cross shape.
        /// </summary>
        Cross = 3,

        /// <summary>
        /// The custom texture shape.
        /// </summary>
        Custom = 4
    }

    /// <summary>
    /// Contains settings for rendering advanced visual effects and post effects.
    /// </summary>
    public sealed class PostProcessSettings
    {
        /// <summary>
        /// Packed setings storage container used with C++ interop.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct Data
        {
            // DepthOfFieldSettings
            public bool DOF_Enabled;
            public float DOF_FocalDistance;
            public float DOF_FocalRegion;
            public float DOF_NearTransitionRange;
            public float DOF_FarTransitionRange;
            public float DOF_DepthLimit;
            public bool DOF_BokehEnabled;
            public float DOF_BokehSize;
            public BokehShapeType DOF_BokehShape;
            public Guid DOF_BokehShapeCustom;
            public float DOF_BokehBrightnessThreshold;
            public float DOF_BokehBlurThreshold;
            public float DOF_BokehFalloff;
            public float DOF_BokehDepthCutoff;
        }

        internal Data data;

        internal PostProcessSettings()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether Depth of Field is enabled.
        /// </summary>
        [EditorOrder(700), EditorDisplay("Depth of Field", "Enabled"), Tooltip("Enable depth of field effect")]
        public bool DOF_Enabled
        {
            get => data.DOF_Enabled;
            set => data.DOF_Enabled = value;
        }

        [EditorOrder(701), EditorDisplay("Depth of Field", "Focal Distance"), Tooltip("The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs"), Limit(0, 100000.0f)]
        public float DOF_FocalDistance
        {
            get => data.DOF_FocalDistance;
            set => data.DOF_FocalDistance = value;
        }

        [EditorOrder(702), EditorDisplay("Depth of Field", "Focal Region"), Tooltip("The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs"), Limit(0, 100000.0f, 1)]
        public float DOF_FocalRegion
        {
            get => data.DOF_FocalRegion;
            set => data.DOF_FocalRegion = value;
        }

        [EditorOrder(703), EditorDisplay("Depth of Field", "Near Transition Range"), Tooltip("The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred"), Limit(0, 500.0f)]
        public float DOF_NearTransitionRange
        {
            get => data.DOF_NearTransitionRange;
            set => data.DOF_NearTransitionRange = value;
        }

        [EditorOrder(704), EditorDisplay("Depth of Field", "Far Transition Range"), Tooltip("The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred"), Limit(0, 1000.0f)]
        public float DOF_FarTransitionRange
        {
            get => data.DOF_FarTransitionRange;
            set => data.DOF_FarTransitionRange = value;
        }

        [EditorOrder(705), EditorDisplay("Depth of Field", "Depth Limit"), Tooltip("The distance in World Units which describes border after that there is no blur (usefull to disable DoF on sky)"), Limit(50, 1000000.0f, 2)]
        public float DOF_DepthLimit
        {
            get => data.DOF_DepthLimit;
            set => data.DOF_DepthLimit = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Depth of Field bokeh shapes rendering is enabled.
        /// </summary>
        [EditorOrder(706), EditorDisplay("Depth of Field", "Bokeh Enable"), Tooltip("Enables/disables generating Bokeh shapes")]
        public bool DOF_BokehEnabled
        {
            get => data.DOF_BokehEnabled;
            set => data.DOF_BokehEnabled = value;
        }

        [EditorOrder(707), EditorDisplay("Depth of Field", "Bokeh Size"), Tooltip("Controls Bokeh shapes maximum size"), Limit(0, 100.0f, 0.1f)]
        public float DOF_BokehSize
        {
            get => data.DOF_BokehSize;
            set => data.DOF_BokehSize = value;
        }

        [EditorOrder(708), EditorDisplay("Depth of Field", "Bokeh Shape"), Tooltip("Bokeh shapes style")]
        public BokehShapeType DOF_BokehShape
        {
            get => data.DOF_BokehShape;
            set => data.DOF_BokehShape = value;
        }

        [EditorOrder(709), EditorDisplay("Depth of Field", "Bokeh Shape Custom Texture"), Tooltip("Custom texture for bokeh shapes")]
        public Texture DOF_BokehShapeCustom
        {
            get => Object.Find<Texture>(ref data.DOF_BokehShapeCustom);
            set => data.DOF_BokehShapeCustom = value?.ID ?? Guid.Empty;
        }

        [EditorOrder(710), EditorDisplay("Depth of Field", "Bokeh Brightness Threshold"), Tooltip("Controls Bokeh shapes generating minimum pixel brightness to appear"), Limit(0, 10.0f, 0.01f)]
        public float DOF_BokehBrightnessThreshold
        {
            get => data.DOF_BokehBrightnessThreshold;
            set => data.DOF_BokehBrightnessThreshold = value;
        }

        [EditorOrder(711), EditorDisplay("Depth of Field", "Bokeh Blur Threshold"), Tooltip("Controls Bokeh shapes blur threashold"), Limit(0, 1.0f, 0.001f)]
        public float DOF_BokehBlurThreshold
        {
            get => data.DOF_BokehBlurThreshold;
            set => data.DOF_BokehBlurThreshold = value;
        }

        [EditorOrder(712), EditorDisplay("Depth of Field", "Bokeh Falloff"), Tooltip("Controls Bokeh shapes brightness fallouff parameter"), Limit(0, 2.0f, 0.001f)]
        public float DOF_BokehFalloff
        {
            get => data.DOF_BokehFalloff;
            set => data.DOF_BokehFalloff = value;
        }

        [EditorOrder(713), EditorDisplay("Depth of Field", "Bokeh Depth Cutoff"), Tooltip("Controls Bokeh shapes depth cutoff parameter"), Limit(0, 5.0f, 0.001f)]
        public float DOF_BokehDepthCutoff
        {
            get => data.DOF_BokehDepthCutoff;
            set => data.DOF_BokehDepthCutoff = value;
        }
    }
}
