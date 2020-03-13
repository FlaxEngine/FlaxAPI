// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The base class for all GPU resources.
    /// </summary>
    [Tooltip("The base class for all GPU resources.")]
    public abstract unsafe partial class GPUResource : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected GPUResource() : base()
        {
        }

        /// <summary>
        /// Gets amount of GPU memory used by this resource (in bytes).
        /// It's a rough estimation. GPU memory may be fragmented, compressed or sub-allocated so the actual memory pressure from this resource may vary (also depends on the current graphics backend).
        /// </summary>
        public ulong MemoryUsage
        {
            get { return Internal_GetMemoryUsage(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetMemoryUsage(IntPtr obj);

        /// <summary>
        /// Releases GPU resource data.
        /// </summary>
        public void ReleaseGPU()
        {
            Internal_ReleaseGPU(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ReleaseGPU(IntPtr obj);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Interface for GPU resources views. Shared base class for texture and buffer views.
    /// </summary>
    [Tooltip("Interface for GPU resources views. Shared base class for texture and buffer views.")]
    public abstract unsafe partial class GPUResourceView : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected GPUResourceView() : base()
        {
        }
    }
}
