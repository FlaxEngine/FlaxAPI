// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents single Level Of Detail for the model. Contains a collection of the meshes.
    /// </summary>
    [Tooltip("Represents single Level Of Detail for the model. Contains a collection of the meshes.")]
    public unsafe partial class ModelLOD : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected ModelLOD() : base()
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
        public Mesh[] Meshes
        {
            get { return Internal_GetMeshes(unmanagedPtr, typeof(Mesh)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Mesh[] Internal_GetMeshes(IntPtr obj, System.Type resultArrayItemType0);

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

        /// <summary>
        /// Draws the meshes from the model LOD.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="world">The world transformation of the model.</param>
        /// <param name="flags">The object static flags.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void Draw(ref RenderContext renderContext, MaterialBase material, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            Internal_Draw(unmanagedPtr, ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(material), ref world, flags, receiveDecals);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw(IntPtr obj, ref RenderContext renderContext, IntPtr material, ref Matrix world, StaticFlags flags, bool receiveDecals);
    }
}
