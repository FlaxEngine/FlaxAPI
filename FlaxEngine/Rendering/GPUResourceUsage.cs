// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Identifies expected GPU resource use during rendering. The usage directly reflects whether a resource is accessible by the CPU and/or the GPU.	
    /// </summary>
    public enum GPUResourceUsage
    {
        /// <summary>
        /// A resource that requires read and write access by the GPU. 
        /// This is likely to be the most common usage choice. 
        /// Memory will be used on device only, so fast access from the device is preferred. 
        /// It usually means device-local GPU (video) memory.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written and read by device, e.g. images used as render targets.
        /// - Resources transferred from host once (immutable) or infrequently and read by
        ///   device multiple times, e.g. textures to be sampled, vertex buffers, constant
        ///   buffers, and majority of other types of resources used on GPU.
        /// </remarks>
        Default = 0,

        /// <summary>
        /// A resource that is accessible by both the GPU (read only) and the CPU (write only). 
        /// A dynamic resource is a good choice for a resource that will be updated by the CPU at least once per frame. 
        /// Dynamic buffers or textures are usually used to upload data to GPU and use it within a single frame.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written frequently by CPU (dynamic), read by device. 
        ///   E.g. textures, vertex buffers, uniform buffers updated every frame or every draw call.
        /// </remarks>
        Dynamic = 1,

        /// <summary>
        /// A resource that supports data transfer (copy) from the CPU to the GPU.  
        /// It usually means CPU (system) memory. Resources created in this pool may still be accessible to the device, but access to them can be slow.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Staging copy of resources used as transfer source.
        /// </remarks>
        StagingUpload = 2,

        /// <summary>
        /// A resource that supports data transfer (copy) from the GPU to the CPU. 
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written by device, read by host - results of some computations, e.g. screen capture, average scene luminance for HDR tone mapping.
        /// - Any resources read or accessed randomly on host, e.g. CPU-side copy of vertex buffer used as source of transfer, but also used for collision detection.
        /// </remarks>
        StagingReadback = 3
    }
}
