// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents single Level Of Detail for the Model.
    /// Contains collection of meshes.
    /// </summary>
    public sealed class ModelLOD
    {
        internal readonly Model _model;
        internal readonly int _lodIndex;

        /// <summary>
        /// Gets the parent model asset.
        /// </summary>
        /// <value>
        /// The parent model.
        /// </value>
        public Model ParentModel => _model;

        /// <summary>
        /// The meshes array.
        /// </summary>
        public readonly Mesh[] Meshes;

        /// <summary>
        /// Gets or sets the minimum model screen size to switch LODs. Bottom limit of the model screen size to render this LOD.
        /// </summary>
        public float ScreenSize
        {
            get => Internal_GetScreenSize(_model.unmanagedPtr, _lodIndex);
            set => Internal_SetScreenSize(_model.unmanagedPtr, _lodIndex, value);
        }

        /// <summary>
        /// Gets the bounding box combined for all meshes in this model LOD.
        /// </summary>
        public BoundingBox Bounds
        {
            get
            {
                BoundingBox result;
                Internal_GetBounds(_model.unmanagedPtr, _lodIndex, out result);
                return result;
            }
        }

        internal ModelLOD(Model model, int lodIndex, int meshesCount)
        {
            _model = model;
            _lodIndex = lodIndex;
            Meshes = new Mesh[meshesCount];
            for (int i = 0; i < meshesCount; i++)
            {
                Meshes[i] = new Mesh(model, lodIndex, i);
            }
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScreenSize(IntPtr obj, int lodIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScreenSize(IntPtr obj, int lodIndex, float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBounds(IntPtr obj, int lodIndex, out BoundingBox result);
#endif
    }
}
