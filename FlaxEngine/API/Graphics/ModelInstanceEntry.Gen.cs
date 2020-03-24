// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The model instance entry that describes how to draw it.
    /// </summary>
    [Tooltip("The model instance entry that describes how to draw it.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ModelInstanceEntry
    {
        /// <summary>
        /// The mesh surface material used for the rendering. If not assigned the default value will be used from the model asset.
        /// </summary>
        [Tooltip("The mesh surface material used for the rendering. If not assigned the default value will be used from the model asset.")]
        public MaterialBase Material;

        /// <summary>
        /// The shadows casting mode.
        /// </summary>
        [Tooltip("The shadows casting mode.")]
        public ShadowsCastingMode ShadowsMode;

        /// <summary>
        /// Determines whenever this mesh is visible.
        /// </summary>
        [Tooltip("Determines whenever this mesh is visible.")]
        public bool Visible;

        /// <summary>
        /// Determines whenever this mesh can receive decals.
        /// </summary>
        [Tooltip("Determines whenever this mesh can receive decals.")]
        public bool ReceiveDecals;
    }
}
