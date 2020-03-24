// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Interface for GPU device context that can record and send graphics commands to the GPU in a sequence.
    /// </summary>
    /// <seealso cref="PersistentScriptingObject" />
    [Tooltip("Interface for GPU device context that can record and send graphics commands to the GPU in a sequence.")]
    public sealed unsafe partial class GPUContext : FlaxEngine.Object
    {
        private GPUContext() : base()
        {
        }

        /// <summary>
        /// Clears texture surface with a color. Supports volumetric textures and texture arrays (including cube textures).
        /// </summary>
        /// <param name="rt">The target surface.</param>
        /// <param name="color">The clear color.</param>
        public void Clear(GPUTextureView rt, Color color)
        {
            Internal_Clear(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(rt), ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Clear(IntPtr obj, IntPtr rt, ref Color color);

        /// <summary>
        /// Clears depth buffer.
        /// </summary>
        /// <param name="depthBuffer">The depth buffer to clear.</param>
        /// <param name="depthValue">The clear depth value.</param>
        public void ClearDepth(GPUTextureView depthBuffer, float depthValue = 1.0f)
        {
            Internal_ClearDepth(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), depthValue);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClearDepth(IntPtr obj, IntPtr depthBuffer, float depthValue);

        /// <summary>
        /// Clears an unordered access resource with a float value.
        /// </summary>
        /// <param name="buf">The buffer to clear.</param>
        /// <param name="value">The clear value.</param>
        public void ClearUA(GPUBuffer buf, Vector4 value)
        {
            Internal_ClearUA(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(buf), ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClearUA(IntPtr obj, IntPtr buf, ref Vector4 value);

        /// <summary>
        /// Updates the buffer data.
        /// </summary>
        /// <param name="buffer">The destination buffer to write to.</param>
        /// <param name="data">The pointer to the data.</param>
        /// <param name="size">The data size (in bytes) to write.</param>
        /// <param name="offset">The offset (in bytes) from the buffer start to copy data to.</param>
        public void UpdateBuffer(GPUBuffer buffer, IntPtr data, uint size, uint offset = 0)
        {
            Internal_UpdateBuffer(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(buffer), data, size, offset);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateBuffer(IntPtr obj, IntPtr buffer, IntPtr data, uint size, uint offset);

        /// <summary>
        /// Copies the buffer data.
        /// </summary>
        /// <param name="dstBuffer">The destination buffer to write to.</param>
        /// <param name="srcBuffer">The source buffer to read from.</param>
        /// <param name="size">The size of data to copy (in bytes).</param>
        /// <param name="dstOffset">The offset (in bytes) from the destination buffer start to copy data to.</param>
        /// <param name="srcOffset">The offset (in bytes) from the source buffer start to copy data from.</param>
        public void CopyBuffer(GPUBuffer dstBuffer, GPUBuffer srcBuffer, uint size, uint dstOffset = 0, uint srcOffset = 0)
        {
            Internal_CopyBuffer(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dstBuffer), FlaxEngine.Object.GetUnmanagedPtr(srcBuffer), size, dstOffset, srcOffset);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyBuffer(IntPtr obj, IntPtr dstBuffer, IntPtr srcBuffer, uint size, uint dstOffset, uint srcOffset);

        /// <summary>
        /// Updates the texture data.
        /// </summary>
        /// <param name="texture">The destination texture.</param>
        /// <param name="arrayIndex">The destination surface index in the texture array.</param>
        /// <param name="mipIndex">The absolute index of the mip map to update.</param>
        /// <param name="data">The pointer to the data.</param>
        /// <param name="rowPitch">The row pitch (in bytes) of the input data.</param>
        /// <param name="slicePitch">The slice pitch (in bytes) of the input data.</param>
        public void UpdateTexture(GPUTexture texture, int arrayIndex, int mipIndex, IntPtr data, uint rowPitch, uint slicePitch)
        {
            Internal_UpdateTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(texture), arrayIndex, mipIndex, data, rowPitch, slicePitch);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateTexture(IntPtr obj, IntPtr texture, int arrayIndex, int mipIndex, IntPtr data, uint rowPitch, uint slicePitch);

        /// <summary>
        /// Copies region of the texture.
        /// </summary>
        /// <param name="dstResource">The destination resource.</param>
        /// <param name="dstSubresource">The destination subresource index.</param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region.</param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region.</param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region.</param>
        /// <param name="srcResource">The source resource.</param>
        /// <param name="srcSubresource">The source subresource index.</param>
        public void CopyTexture(GPUTexture dstResource, uint dstSubresource, uint dstX, uint dstY, uint dstZ, GPUTexture srcResource, uint srcSubresource)
        {
            Internal_CopyTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dstResource), dstSubresource, dstX, dstY, dstZ, FlaxEngine.Object.GetUnmanagedPtr(srcResource), srcSubresource);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyTexture(IntPtr obj, IntPtr dstResource, uint dstSubresource, uint dstX, uint dstY, uint dstZ, IntPtr srcResource, uint srcSubresource);

        /// <summary>
        /// Resets the counter buffer to zero (hidden by the driver).
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void ResetCounter(GPUBuffer buffer)
        {
            Internal_ResetCounter(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(buffer));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetCounter(IntPtr obj, IntPtr buffer);

        /// <summary>
        /// Copies the counter buffer value.
        /// </summary>
        /// <param name="dstBuffer">The destination buffer.</param>
        /// <param name="dstOffset">The destination aligned byte offset.</param>
        /// <param name="srcBuffer">The source buffer.</param>
        public void CopyCounter(GPUBuffer dstBuffer, uint dstOffset, GPUBuffer srcBuffer)
        {
            Internal_CopyCounter(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dstBuffer), dstOffset, FlaxEngine.Object.GetUnmanagedPtr(srcBuffer));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyCounter(IntPtr obj, IntPtr dstBuffer, uint dstOffset, IntPtr srcBuffer);

        /// <summary>
        /// Copies the resource data (whole resource).
        /// </summary>
        /// <param name="dstResource">The destination resource.</param>
        /// <param name="srcResource">The source resource.</param>
        public void CopyResource(GPUResource dstResource, GPUResource srcResource)
        {
            Internal_CopyResource(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dstResource), FlaxEngine.Object.GetUnmanagedPtr(srcResource));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopyResource(IntPtr obj, IntPtr dstResource, IntPtr srcResource);

        /// <summary>
        /// Copies the subresource data.
        /// </summary>
        /// <param name="dstResource">The destination resource.</param>
        /// <param name="dstSubresource">The destination subresource index.</param>
        /// <param name="srcResource">The source resource.</param>
        /// <param name="srcSubresource">The source subresource index.</param>
        public void CopySubresource(GPUResource dstResource, uint dstSubresource, GPUResource srcResource, uint srcSubresource)
        {
            Internal_CopySubresource(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dstResource), dstSubresource, FlaxEngine.Object.GetUnmanagedPtr(srcResource), srcSubresource);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CopySubresource(IntPtr obj, IntPtr dstResource, uint dstSubresource, IntPtr srcResource, uint srcSubresource);

        /// <summary>
        /// Unbinds all the render targets and flushes the change with the driver (used to prevent driver detection of resource hazards, eg. when down-scaling the texture).
        /// </summary>
        public void ResetRenderTarget()
        {
            Internal_ResetRenderTarget(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetRenderTarget(IntPtr obj);

        /// <summary>
        /// Sets the render target to the output.
        /// </summary>
        /// <param name="rt">The render target.</param>
        public void SetRenderTarget(GPUTextureView rt)
        {
            Internal_SetRenderTarget(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(rt));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRenderTarget(IntPtr obj, IntPtr rt);

        /// <summary>
        /// Sets the render target and the depth buffer to the output.
        /// </summary>
        /// <param name="depthBuffer">The depth buffer.</param>
        /// <param name="rt">The render target.</param>
        public void SetRenderTarget(GPUTextureView depthBuffer, GPUTextureView rt)
        {
            Internal_SetRenderTarget1(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), FlaxEngine.Object.GetUnmanagedPtr(rt));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRenderTarget1(IntPtr obj, IntPtr depthBuffer, IntPtr rt);

        /// <summary>
        /// Sets the render targets and the depth buffer to the output.
        /// </summary>
        /// <param name="depthBuffer">The depth buffer (can be null).</param>
        /// <param name="rts">The array with render targets to bind.</param>
        public void SetRenderTarget(GPUTextureView depthBuffer, GPUTextureView[] rts)
        {
            Internal_SetRenderTarget2(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), rts);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRenderTarget2(IntPtr obj, IntPtr depthBuffer, GPUTextureView[] rts);

        /// <summary>
        /// Sets the render target and unordered access output.
        /// </summary>
        /// <param name="rt">The render target to bind to output.</param>
        /// <param name="uaOutput">The unordered access buffer to bind to output.</param>
        public void SetRenderTarget(GPUTextureView rt, GPUBuffer uaOutput)
        {
            Internal_SetRenderTarget3(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(rt), FlaxEngine.Object.GetUnmanagedPtr(uaOutput));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRenderTarget3(IntPtr obj, IntPtr rt, IntPtr uaOutput);

        /// <summary>
        /// Resolves the multisampled texture by performing a copy of the resource into a non-multisampled resource.
        /// </summary>
        /// <param name="sourceMultisampleTexture">The source multisampled texture. Must be multisampled.</param>
        /// <param name="destTexture">The destination texture. Must be single-sampled.</param>
        /// <param name="sourceSubResource">The source sub-resource index.</param>
        /// <param name="destSubResource">The destination sub-resource index.</param>
        /// <param name="format">The format. Indicates how the multisampled resource will be resolved to a single-sampled resource.</param>
        public void ResolveMultisample(GPUTexture sourceMultisampleTexture, GPUTexture destTexture, int sourceSubResource = 0, int destSubResource = 0, PixelFormat format = PixelFormat.Unknown)
        {
            Internal_ResolveMultisample(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(sourceMultisampleTexture), FlaxEngine.Object.GetUnmanagedPtr(destTexture), sourceSubResource, destSubResource, format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResolveMultisample(IntPtr obj, IntPtr sourceMultisampleTexture, IntPtr destTexture, int sourceSubResource, int destSubResource, PixelFormat format);

        /// <summary>
        /// Draws the fullscreen triangle (using single triangle). Use instance count parameter to render more than one instance of the triangle.
        /// </summary>
        /// <param name="instanceCount">The instance count. Use SV_InstanceID in vertex shader to detect volume slice plane index.</param>
        public void DrawFullscreenTriangle(int instanceCount = 1)
        {
            Internal_DrawFullscreenTriangle(unmanagedPtr, instanceCount);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawFullscreenTriangle(IntPtr obj, int instanceCount);

        /// <summary>
        /// Draws the specified source texture to destination render target (using fullscreen triangle). Copies contents with resizing and format conversion support. Uses linear texture sampling.
        /// </summary>
        /// <param name="dst">The destination texture.</param>
        /// <param name="src">The source texture.</param>
        public void Draw(GPUTexture dst, GPUTexture src)
        {
            Internal_Draw(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(dst), FlaxEngine.Object.GetUnmanagedPtr(src));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw(IntPtr obj, IntPtr dst, IntPtr src);

        /// <summary>
        /// Draws the specified texture to render target (using fullscreen triangle). Copies contents with resizing and format conversion support. Uses linear texture sampling.
        /// </summary>
        /// <param name="rt">The texture.</param>
        public void Draw(GPUTexture rt)
        {
            Internal_Draw1(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(rt));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw1(IntPtr obj, IntPtr rt);

        /// <summary>
        /// Draws the specified texture to render target (using fullscreen triangle). Copies contents with resizing and format conversion support. Uses linear texture sampling.
        /// </summary>
        /// <param name="rt">The texture view.</param>
        public void Draw(GPUTextureView rt)
        {
            Internal_Draw2(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(rt));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw2(IntPtr obj, IntPtr rt);

        /// <summary>
        /// Draws non-indexed, non-instanced primitives.
        /// </summary>
        /// <param name="startVertex">A value added to each index before reading a vertex from the vertex buffer.</param>
        /// <param name="verticesCount">The vertices count.</param>
        public void Draw(uint startVertex, uint verticesCount)
        {
            Internal_Draw3(unmanagedPtr, startVertex, verticesCount);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw3(IntPtr obj, uint startVertex, uint verticesCount);

        /// <summary>
        /// Draws the instanced primitives.
        /// </summary>
        /// <param name="verticesCount">The vertices count.</param>
        /// <param name="instanceCount">Number of instances to draw.</param>
        /// <param name="startInstance">A value added to each index before reading per-instance data from a vertex buffer.</param>
        /// <param name="startVertex">A value added to each index before reading a vertex from the vertex buffer.</param>
        public void DrawInstanced(uint verticesCount, uint instanceCount, int startInstance = 0, int startVertex = 0)
        {
            Internal_DrawInstanced(unmanagedPtr, verticesCount, instanceCount, startInstance, startVertex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawInstanced(IntPtr obj, uint verticesCount, uint instanceCount, int startInstance, int startVertex);

        /// <summary>
        /// Draws the indexed primitives.
        /// </summary>
        /// <param name="indicesCount">The indices count.</param>
        /// <param name="startVertex">A value added to each index before reading a vertex from the vertex buffer.</param>
        /// <param name="startIndex">The location of the first index read by the GPU from the index buffer.</param>
        public void DrawIndexed(uint indicesCount, int startVertex = 0, int startIndex = 0)
        {
            Internal_DrawIndexed(unmanagedPtr, indicesCount, startVertex, startIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawIndexed(IntPtr obj, uint indicesCount, int startVertex, int startIndex);

        /// <summary>
        /// Draws the indexed, instanced primitives.
        /// </summary>
        /// <param name="indicesCount">The indices count.</param>
        /// <param name="instanceCount">Number of instances to draw.</param>
        /// <param name="startInstance">A value added to each index before reading per-instance data from a vertex buffer.</param>
        /// <param name="startVertex">A value added to each index before reading a vertex from the vertex buffer.</param>
        /// <param name="startIndex">The location of the first index read by the GPU from the index buffer.</param>
        public void DrawIndexedInstanced(uint indicesCount, uint instanceCount, int startInstance = 0, int startVertex = 0, int startIndex = 0)
        {
            Internal_DrawIndexedInstanced(unmanagedPtr, indicesCount, instanceCount, startInstance, startVertex, startIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawIndexedInstanced(IntPtr obj, uint indicesCount, uint instanceCount, int startInstance, int startVertex, int startIndex);

        /// <summary>
        /// Draws the instanced GPU-generated primitives.
        /// </summary>
        /// <param name="bufferForArgs">The buffer with drawing arguments.</param>
        /// <param name="offsetForArgs">The aligned byte offset for arguments.</param>
        public void DrawInstancedIndirect(GPUBuffer bufferForArgs, uint offsetForArgs)
        {
            Internal_DrawInstancedIndirect(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(bufferForArgs), offsetForArgs);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawInstancedIndirect(IntPtr obj, IntPtr bufferForArgs, uint offsetForArgs);

        /// <summary>
        /// Draws the instanced GPU-generated indexed primitives.
        /// </summary>
        /// <param name="bufferForArgs">The buffer with drawing arguments.</param>
        /// <param name="offsetForArgs">The aligned byte offset for arguments.</param>
        public void DrawIndexedInstancedIndirect(GPUBuffer bufferForArgs, uint offsetForArgs)
        {
            Internal_DrawIndexedInstancedIndirect(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(bufferForArgs), offsetForArgs);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawIndexedInstancedIndirect(IntPtr obj, IntPtr bufferForArgs, uint offsetForArgs);

        /// <summary>
        /// Sets the rendering viewport.
        /// </summary>
        /// <param name="width">The width (in pixels).</param>
        /// <param name="height">The height (in pixels).</param>
        public void SetViewport(float width, float height)
        {
            Internal_SetViewport(unmanagedPtr, width, height);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetViewport(IntPtr obj, float width, float height);

        /// <summary>
        /// Sets the rendering viewport.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        public void SetViewport(ref Viewport viewport)
        {
            Internal_SetViewport1(unmanagedPtr, ref viewport);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetViewport1(IntPtr obj, ref Viewport viewport);

        /// <summary>
        /// Sets the scissor rectangle.
        /// </summary>
        /// <param name="scissorRect">The scissor rectangle.</param>
        public void SetScissorRect(ref Rectangle scissorRect)
        {
            Internal_SetScissorRect(unmanagedPtr, ref scissorRect);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScissorRect(IntPtr obj, ref Rectangle scissorRect);
    }
}
