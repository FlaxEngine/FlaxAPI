// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents single Level Of Detail for the skinned model. Contains a collection of the meshes.
    /// </summary>
    [Tooltip("Represents single Level Of Detail for the skinned model. Contains a collection of the meshes.")]
    public unsafe partial class SkinnedModelLOD : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected SkinnedModelLOD() : base()
        {
        }

        /// <summary>
        /// The screen size to switch LODs. Bottom limit of the model screen size to render this LOD.
        /// </summary>
        [Tooltip("The screen size to switch LODs. Bottom limit of the model screen size to render this LOD.")]
        public float ScreenSize
        {
            get { return Internal_GetScreenSize(unmanagedPtr); }
            set { Internal_SetScreenSize(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScreenSize(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScreenSize(IntPtr obj, float value);

        /// <summary>
        /// The meshes array.
        /// </summary>
        [Tooltip("The meshes array.")]
        public SkinnedMesh[] Meshes
        {
            get { return Internal_GetMeshes(unmanagedPtr, typeof(SkinnedMesh)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedMesh[] Internal_GetMeshes(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the bounding box combined for all meshes in this model LOD.
        /// </summary>
        [Tooltip("The bounding box combined for all meshes in this model LOD.")]
        public BoundingBox Box
        {
            get { Internal_GetBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox(IntPtr obj, out BoundingBox resultAsRef);
    }
}
