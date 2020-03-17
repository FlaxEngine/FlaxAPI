// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Graphics device object for rendering on GPU.
    /// </summary>
    [Tooltip("Graphics device object for rendering on GPU.")]
    public sealed unsafe partial class GPUDevice : FlaxEngine.Object
    {
        private GPUDevice() : base()
        {
        }

        /// <summary>
        /// The singleton instance of the graphics device.
        /// </summary>
        [Tooltip("The singleton instance of the graphics device.")]
        public static GPUDevice Instance
        {
            get { return Internal_GetInstance(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUDevice Internal_GetInstance();

        /// <summary>
        /// The total amount of graphics memory in bytes.
        /// </summary>
        [Tooltip("The total amount of graphics memory in bytes.")]
        public ulong TotalGraphicsMemory
        {
            get { return Internal_GetTotalGraphicsMemory(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetTotalGraphicsMemory(IntPtr obj);

        /// <summary>
        /// The GPU limits.
        /// </summary>
        [Tooltip("The GPU limits.")]
        public GPULimits Limits
        {
            get { Internal_GetLimits(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimits(IntPtr obj, out GPULimits resultAsRef);

        /// <summary>
        /// Gets the device renderer type.
        /// </summary>
        [Tooltip("The device renderer type.")]
        public RendererType RendererType
        {
            get { return Internal_GetRendererType(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RendererType Internal_GetRendererType(IntPtr obj);

        /// <summary>
        /// Gets device shader profile type.
        /// </summary>
        [Tooltip("Gets device shader profile type.")]
        public ShaderProfile ShaderProfile
        {
            get { return Internal_GetShaderProfile(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShaderProfile Internal_GetShaderProfile(IntPtr obj);

        /// <summary>
        /// Gets device feature level type.
        /// </summary>
        [Tooltip("Gets device feature level type.")]
        public FeatureLevel FeatureLevel
        {
            get { return Internal_GetFeatureLevel(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FeatureLevel Internal_GetFeatureLevel(IntPtr obj);

        /// <summary>
        /// Gets the main GPU context.
        /// </summary>
        [Tooltip("The main GPU context.")]
        public GPUContext MainContext
        {
            get { return Internal_GetMainContext(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUContext Internal_GetMainContext(IntPtr obj);

        /// <summary>
        /// Gets the adapter device.
        /// </summary>
        [Tooltip("The adapter device.")]
        public GPUAdapter Adapter
        {
            get { return Internal_GetAdapter(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUAdapter Internal_GetAdapter(IntPtr obj);

        /// <summary>
        /// Gets the native pointer to the underlying graphics device. It's a low-level platform-specific handle.
        /// </summary>
        [Tooltip("The native pointer to the underlying graphics device. It's a low-level platform-specific handle.")]
        public IntPtr NativePtr
        {
            get { return Internal_GetNativePtr(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr Internal_GetNativePtr(IntPtr obj);

        /// <summary>
        /// Gets the amount of memory usage by all the GPU resources (in bytes).
        /// </summary>
        [Tooltip("The amount of memory usage by all the GPU resources (in bytes).")]
        public ulong MemoryUsage
        {
            get { return Internal_GetMemoryUsage(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetMemoryUsage(IntPtr obj);

        /// <summary>
        /// Gets the supported features for the specified format (index is the pixel format value).
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The format features flags.</returns>
        public FormatFeatures GetFormatFeatures(PixelFormat format)
        {
            Internal_GetFormatFeatures(unmanagedPtr, format, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetFormatFeatures(IntPtr obj, PixelFormat format, out FormatFeatures resultAsRef);

        /// <summary>
        /// Creates the texture.
        /// </summary>
        /// <param name="name">The resource name.</param>
        /// <returns>The texture.</returns>
        public GPUTexture CreateTexture(string name = null)
        {
            return Internal_CreateTexture(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_CreateTexture(IntPtr obj, string name);
    }
}
