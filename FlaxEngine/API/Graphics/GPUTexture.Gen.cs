// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Defines a view for the <see cref="GPUTexture"/> surface, full resource or any of the sub-parts. Can be used to define a single subresource of the texture, volume texture or texture array. Used to render to the texture and/or use textures in the shaders.
    /// </summary>
    [Tooltip("Defines a view for the <see cref=\"GPUTexture\"/> surface, full resource or any of the sub-parts. Can be used to define a single subresource of the texture, volume texture or texture array. Used to render to the texture and/or use textures in the shaders.")]
    public sealed unsafe partial class GPUTextureView : GPUResourceView
    {
        private GPUTextureView() : base()
        {
        }

        /// <summary>
        /// Gets parent GPU resource owning that view.
        /// </summary>
        [Tooltip("Gets parent GPU resource owning that view.")]
        public GPUResource Parent
        {
            get { return Internal_GetParent(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUResource Internal_GetParent(IntPtr obj);

        /// <summary>
        /// Gets the view format.
        /// </summary>
        [Tooltip("The view format.")]
        public PixelFormat Format
        {
            get { return Internal_GetFormat(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_GetFormat(IntPtr obj);

        /// <summary>
        /// Gets view MSAA level.
        /// </summary>
        [Tooltip("Gets view MSAA level.")]
        public MSAALevel MSAA
        {
            get { return Internal_GetMSAA(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MSAALevel Internal_GetMSAA(IntPtr obj);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The GPU texture resource object. This class is able to create 2D/3D textures, volume textures and render targets.
    /// </summary>
    [Tooltip("The GPU texture resource object. This class is able to create 2D/3D textures, volume textures and render targets.")]
    public sealed unsafe partial class GPUTexture : GPUResource
    {
        private GPUTexture() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="GPUTexture"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static GPUTexture New()
        {
            return Internal_Create(typeof(GPUTexture)) as GPUTexture;
        }

        /// <summary>
        /// Gets a value indicating whether this texture has any resided mip (data already uploaded to the GPU).
        /// </summary>
        [Tooltip("Gets a value indicating whether this texture has any resided mip (data already uploaded to the GPU).")]
        public bool HasResidentMip
        {
            get { return Internal_HasResidentMip(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasResidentMip(IntPtr obj);

        /// <summary>
        /// Gets a value indicating whether this texture has been allocated.
        /// </summary>
        [Tooltip("Gets a value indicating whether this texture has been allocated.")]
        public bool IsAllocated
        {
            get { return Internal_IsAllocated(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAllocated(IntPtr obj);

        /// <summary>
        /// Gets texture width (in texels).
        /// </summary>
        [Tooltip("Gets texture width (in texels).")]
        public int Width
        {
            get { return Internal_Width(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_Width(IntPtr obj);

        /// <summary>
        /// Gets texture height (in texels).
        /// </summary>
        [Tooltip("Gets texture height (in texels).")]
        public int Height
        {
            get { return Internal_Height(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_Height(IntPtr obj);

        /// <summary>
        /// Gets texture depth (in texels).
        /// </summary>
        [Tooltip("Gets texture depth (in texels).")]
        public int Depth
        {
            get { return Internal_Depth(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_Depth(IntPtr obj);

        /// <summary>
        /// Gets number of textures in the array.
        /// </summary>
        [Tooltip("Gets number of textures in the array.")]
        public int ArraySize
        {
            get { return Internal_ArraySize(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ArraySize(IntPtr obj);

        /// <summary>
        /// Gets multi-sampling parameters for the texture.
        /// </summary>
        [Tooltip("Gets multi-sampling parameters for the texture.")]
        public MSAALevel MultiSampleLevel
        {
            get { return Internal_MultiSampleLevel(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MSAALevel Internal_MultiSampleLevel(IntPtr obj);

        /// <summary>
        /// Gets number of mipmap levels in the texture.
        /// </summary>
        [Tooltip("Gets number of mipmap levels in the texture.")]
        public int MipLevels
        {
            get { return Internal_MipLevels(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_MipLevels(IntPtr obj);

        /// <summary>
        /// Gets the number of resident mipmap levels in the texture. (already uploaded to the GPU).
        /// </summary>
        [Tooltip("The number of resident mipmap levels in the texture. (already uploaded to the GPU).")]
        public int ResidentMipLevels
        {
            get { return Internal_ResidentMipLevels(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ResidentMipLevels(IntPtr obj);

        /// <summary>
        /// Gets the index of the highest resident mip map (may be equal to MipLevels if no mip has been uploaded). Note: mip=0 is the highest (top quality).
        /// </summary>
        [Tooltip("The index of the highest resident mip map (may be equal to MipLevels if no mip has been uploaded). Note: mip=0 is the highest (top quality).")]
        public int HighestResidentMipIndex
        {
            get { return Internal_HighestResidentMipIndex(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_HighestResidentMipIndex(IntPtr obj);

        /// <summary>
        /// Gets texture data format.
        /// </summary>
        [Tooltip("Gets texture data format.")]
        public PixelFormat Format
        {
            get { return Internal_Format(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_Format(IntPtr obj);

        /// <summary>
        /// Gets flags of the texture.
        /// </summary>
        [Tooltip("Gets flags of the texture.")]
        public GPUTextureFlags Flags
        {
            get { return Internal_Flags(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureFlags Internal_Flags(IntPtr obj);

        /// <summary>
        /// Gets texture dimensions.
        /// </summary>
        [Tooltip("Gets texture dimensions.")]
        public TextureDimensions Dimensions
        {
            get { return Internal_Dimensions(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern TextureDimensions Internal_Dimensions(IntPtr obj);

        /// <summary>
        /// Gets texture description structure.
        /// </summary>
        [Tooltip("Gets texture description structure.")]
        public GPUTextureDescription Description
        {
            get { Internal_GetDescription(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDescription(IntPtr obj, out GPUTextureDescription resultAsRef);

        /// <summary>
        /// Gets the texture total size in pixels.
        /// </summary>
        [Tooltip("The texture total size in pixels.")]
        public Vector2 Size
        {
            get { Internal_Size(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Size(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the texture total size in pixels (with depth).
        /// </summary>
        [Tooltip("The texture total size in pixels (with depth).")]
        public Vector3 Size3
        {
            get { Internal_Size3(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Size3(IntPtr obj, out Vector3 resultAsRef);

        /// <summary>
        /// Returns true if texture has size that is power of two.
        /// </summary>
        [Tooltip("Returns true if texture has size that is power of two.")]
        public bool IsPowerOfTwo
        {
            get { return Internal_IsPowerOfTwo(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPowerOfTwo(IntPtr obj);

        /// <summary>
        /// Gets the native pointer to the underlying resource. It's a low-level platform-specific handle.
        /// </summary>
        [Tooltip("The native pointer to the underlying resource. It's a low-level platform-specific handle.")]
        public IntPtr NativePtr
        {
            get { return Internal_GetNativePtr(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr Internal_GetNativePtr(IntPtr obj);

        /// <summary>
        /// Gets the texture mip map dimensions.
        /// </summary>
        /// <param name="mipLevelIndex">Mip level index (zero-based where 0 is top texture surface).</param>
        /// <param name="mipWidth">The calculated mip level width (in pixels).</param>
        /// <param name="mipHeight">The calculated mip level height (in pixels).</param>
        public void GetMipSize(int mipLevelIndex, out int mipWidth, out int mipHeight)
        {
            Internal_GetMipSize(unmanagedPtr, mipLevelIndex, out mipWidth, out mipHeight);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMipSize(IntPtr obj, int mipLevelIndex, out int mipWidth, out int mipHeight);

        /// <summary>
        /// Gets the texture mip map dimensions.
        /// </summary>
        /// <param name="mipLevelIndex">Mip level index (zero-based where 0 is top texture surface).</param>
        /// <param name="mipWidth">The calculated mip level width (in pixels).</param>
        /// <param name="mipHeight">The calculated mip level height (in pixels).</param>
        /// <param name="mipDepth">The calculated mip level depth (in pixels).</param>
        public void GetMipSize(int mipLevelIndex, out int mipWidth, out int mipHeight, out int mipDepth)
        {
            Internal_GetMipSize1(unmanagedPtr, mipLevelIndex, out mipWidth, out mipHeight, out mipDepth);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMipSize1(IntPtr obj, int mipLevelIndex, out int mipWidth, out int mipHeight, out int mipDepth);

        /// <summary>
        /// Gets the view to the first surface (only for 2D textures).
        /// </summary>
        /// <returns>The view to the main texture surface.</returns>
        public GPUTextureView View()
        {
            return Internal_View(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_View(IntPtr obj);

        /// <summary>
        /// Gets the view to the surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per depth/array slice view you need to specify the <see cref="GPUTextureFlags.PerSliceViews"/> when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <returns>The view to the surface at index in an array.</returns>
        public GPUTextureView View(int arrayOrDepthIndex)
        {
            return Internal_View1(unmanagedPtr, arrayOrDepthIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_View1(IntPtr obj, int arrayOrDepthIndex);

        /// <summary>
        /// Gets the view to the mip map surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per mip map view you need to specify the <see cref="GPUTextureFlags.PerMipViews"/> when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <param name="mipMapIndex">Index of the mip level.</param>
        /// <returns>The view to the surface at index in an array.</returns>
        public GPUTextureView View(int arrayOrDepthIndex, int mipMapIndex)
        {
            return Internal_View2(unmanagedPtr, arrayOrDepthIndex, mipMapIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_View2(IntPtr obj, int arrayOrDepthIndex, int mipMapIndex);

        /// <summary>
        /// Gets the view to the array of surfaces
        /// </summary>
        /// <remarks>
        /// To use array texture view you need to create render target as an array.
        /// </remarks>
        /// <returns>The view to the array of surfaces.</returns>
        public GPUTextureView ViewArray()
        {
            return Internal_ViewArray(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_ViewArray(IntPtr obj);

        /// <summary>
        /// Gets the view to the volume texture (3D).
        /// </summary>
        /// <remarks>
        /// To use volume texture view you need to create render target as a volume resource (3D texture with Depth > 1).
        /// </remarks>
        /// <returns>The view to the volume texture.</returns>
        public GPUTextureView ViewVolume()
        {
            return Internal_ViewVolume(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_ViewVolume(IntPtr obj);

        /// <summary>
        /// Gets the view to the texture as read-only depth/stencil buffer. Valid only if graphics device supports it and the texture uses depth/stencil.
        /// </summary>
        /// <returns>The view to the depth-stencil resource descriptor as read-only depth.</returns>
        public GPUTextureView ViewReadOnlyDepth()
        {
            return Internal_ViewReadOnlyDepth(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_ViewReadOnlyDepth(IntPtr obj);

        /// <summary>
        /// Initializes a texture resource (allocates the GPU memory and performs the resource setup).
        /// </summary>
        /// <param name="desc">The texture description.</param>
        /// <returns>True if cannot create texture, otherwise false.</returns>
        public bool Init(ref GPUTextureDescription desc)
        {
            return Internal_Init(unmanagedPtr, ref desc);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Init(IntPtr obj, ref GPUTextureDescription desc);

        /// <summary>
        /// Resizes the texture. It must be created first.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public bool Resize(int width, int height)
        {
            return Internal_Resize(unmanagedPtr, width, height);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Resize(IntPtr obj, int width, int height);

        /// <summary>
        /// Resizes the texture. It must be created first.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public bool Resize(int width, int height, int depth)
        {
            return Internal_Resize1(unmanagedPtr, width, height, depth);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Resize1(IntPtr obj, int width, int height, int depth);
    }
}
