// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Defines a view for the <see cref="GPUTexture"/> surface or full resource or any of the sub-parts. Can be used to define a single subresource of the texture, volume texture or texture array.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GPUTextureView
    {
        internal enum Types : byte
        {
            Full = 0,
            Slice = 1,
            Mip = 2,
            Array = 3,
            Volume = 4,
        }

        private readonly Types Type;
        private readonly byte MipMapIndex;
        private readonly ushort ArrayOrDepthIndex;
        private readonly IntPtr Pointer;

        internal GPUTextureView(IntPtr pointer, Types type)
        {
            Pointer = pointer;
            Type = type;
            MipMapIndex = 0;
            ArrayOrDepthIndex = 0;
        }

        internal GPUTextureView(IntPtr pointer, Types type, int mipMapIndex, int arrayOrDepthIndex)
        {
            Pointer = pointer;
            Type = type;
            MipMapIndex = (byte)mipMapIndex;
            ArrayOrDepthIndex = (ushort)arrayOrDepthIndex;
        }
    }

    public sealed partial class GPUTexture
    {
        /// <summary>
        /// Gets the view to the first surface (only for 2D textures).
        /// </summary>
        /// <returns>The view for the render target.</returns>
        public GPUTextureView View()
        {
            return new GPUTextureView(unmanagedPtr, GPUTextureView.Types.Full);
        }

        /// <summary>
        /// Gets the view to the surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per depth/array slice view you need to specify the <see cref="GPUTextureFlags.PerSliceViews"/> when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <returns>The view for the render target.</returns>
        public GPUTextureView View(int arrayOrDepthIndex)
        {
            return new GPUTextureView(unmanagedPtr, GPUTextureView.Types.Slice, 0, arrayOrDepthIndex);
        }

        /// <summary>
        /// Gets the view to the surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per mip map view you need to specify the <see cref="GPUTextureFlags.PerMipViews"/> when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <param name="mipMapIndex">The index of the mip level.</param>
        /// <returns>The view for the render target.</returns>
        public GPUTextureView View(int arrayOrDepthIndex, int mipMapIndex)
        {
            return new GPUTextureView(unmanagedPtr, GPUTextureView.Types.Mip, mipMapIndex, arrayOrDepthIndex);
        }

        /// <summary>
        /// Gets the view to the array of surfaces.
        /// </summary>
        /// <remarks>
        /// To use array texture view you need to create render target as an array.
        /// </remarks>
        /// <returns>The view for the render target.</returns>
        public GPUTextureView ViewArray()
        {
            return new GPUTextureView(unmanagedPtr, GPUTextureView.Types.Array);
        }

        /// <summary>
        /// Gets the view to the volume texture (3D).
        /// </summary>
        /// <remarks>
        /// To use volume texture view you need to create render target as a volume resource (3D texture with Depth > 1).
        /// </remarks>
        /// <returns>The view for the render target.</returns>
        public GPUTextureView ViewVolume()
        {
            return new GPUTextureView(unmanagedPtr, GPUTextureView.Types.Volume);
        }
    }
}
