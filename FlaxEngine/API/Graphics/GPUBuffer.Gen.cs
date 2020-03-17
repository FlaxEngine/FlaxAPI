// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Defines a view for the <see cref="GPUBuffer"/>. Used to bind buffer to the shaders (for input as shader resource or for input/output as unordered access).
    /// </summary>
    [Tooltip("Defines a view for the <see cref=\"GPUBuffer\"/>. Used to bind buffer to the shaders (for input as shader resource or for input/output as unordered access).")]
    public sealed unsafe partial class GPUBufferView : GPUResourceView
    {
        private GPUBufferView() : base()
        {
        }
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// All-in-one GPU buffer class. This class is able to create index buffers, vertex buffers, structured buffer and argument buffers.
    /// </summary>
    /// <seealso cref="GPUResource" />
    [Tooltip("All-in-one GPU buffer class. This class is able to create index buffers, vertex buffers, structured buffer and argument buffers.")]
    public sealed unsafe partial class GPUBuffer : GPUResource
    {
        private GPUBuffer() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="GPUBuffer"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static GPUBuffer New()
        {
            return Internal_Create(typeof(GPUBuffer)) as GPUBuffer;
        }

        /// <summary>
        /// Gets a value indicating whether this buffer has been allocated.
        /// </summary>
        [Tooltip("Gets a value indicating whether this buffer has been allocated.")]
        public bool IsAllocated
        {
            get { return Internal_IsAllocated(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAllocated(IntPtr obj);

        /// <summary>
        /// Gets buffer size in bytes.
        /// </summary>
        [Tooltip("Gets buffer size in bytes.")]
        public uint Size
        {
            get { return Internal_GetSize(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern uint Internal_GetSize(IntPtr obj);

        /// <summary>
        /// Gets buffer stride in bytes.
        /// </summary>
        [Tooltip("Gets buffer stride in bytes.")]
        public uint Stride
        {
            get { return Internal_GetStride(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern uint Internal_GetStride(IntPtr obj);

        /// <summary>
        /// Gets buffer data format (if used).
        /// </summary>
        [Tooltip("Gets buffer data format (if used).")]
        public PixelFormat Format
        {
            get { return Internal_GetFormat(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_GetFormat(IntPtr obj);

        /// <summary>
        /// Gets buffer elements count (size divided by the stride).
        /// </summary>
        [Tooltip("Gets buffer elements count (size divided by the stride).")]
        public uint ElementsCount
        {
            get { return Internal_GetElementsCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern uint Internal_GetElementsCount(IntPtr obj);

        /// <summary>
        /// Checks if buffer is a staging buffer (supports CPU readback).
        /// </summary>
        [Tooltip("Checks if buffer is a staging buffer (supports CPU readback).")]
        public bool IsStaging
        {
            get { return Internal_IsStaging(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsStaging(IntPtr obj);

        /// <summary>
        /// Checks if buffer is a staging buffer (supports CPU readback).
        /// </summary>
        [Tooltip("Checks if buffer is a staging buffer (supports CPU readback).")]
        public bool IsDynamic
        {
            get { return Internal_IsDynamic(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsDynamic(IntPtr obj);

        /// <summary>
        /// Gets a value indicating whether this buffer is a shader resource.
        /// </summary>
        [Tooltip("Gets a value indicating whether this buffer is a shader resource.")]
        public bool IsShaderResource
        {
            get { return Internal_IsShaderResource(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsShaderResource(IntPtr obj);

        /// <summary>
        /// Gets a value indicating whether this buffer is a unordered access.
        /// </summary>
        [Tooltip("Gets a value indicating whether this buffer is a unordered access.")]
        public bool IsUnorderedAccess
        {
            get { return Internal_IsUnorderedAccess(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsUnorderedAccess(IntPtr obj);

        /// <summary>
        /// Gets the view for the whole buffer.
        /// </summary>
        public GPUBufferView View()
        {
            return Internal_View(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUBufferView Internal_View(IntPtr obj);
    }
}
