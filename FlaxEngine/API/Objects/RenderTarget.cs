// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Defines a view for the <see cref="RenderTarget"/> surface or 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderTargetView
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

        internal RenderTargetView(IntPtr pointer, Types type)
        {
            Pointer = pointer;
            Type = type;
            MipMapIndex = 0;
            ArrayOrDepthIndex = 0;
        }

        internal RenderTargetView(IntPtr pointer, Types type, int mipMapIndex, int arrayOrDepthIndex)
        {
            Pointer = pointer;
            Type = type;
            MipMapIndex = (byte)mipMapIndex;
            ArrayOrDepthIndex = (ushort)arrayOrDepthIndex;
        }
    }

    public partial class RenderTarget
    {
        /// <summary>
        /// Returns true if texture has size that is power of two.
        /// </summary>
        public bool IsPowerOfTwo => Mathf.IsPowerOfTwo(Width) && Mathf.IsPowerOfTwo(Height);

        /// <summary>
        /// Gets the view to the first surface (only for 2D textures).
        /// </summary>
        /// <returns>The view for the render target.</returns>
        public RenderTargetView View()
        {
            return new RenderTargetView(unmanagedPtr, RenderTargetView.Types.Full);
        }

        /// <summary>
        /// Gets the view to the surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per depth/array slice view you need to specify the TextureFlags.PerSliceHandles when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <returns>The view for the render target.</returns>
        public RenderTargetView View(int arrayOrDepthIndex)
        {
            return new RenderTargetView(unmanagedPtr, RenderTargetView.Types.Slice, 0, arrayOrDepthIndex);
        }

        /// <summary>
        /// Gets the view to the surface at index in an array.
        /// </summary>
        /// <remarks>
        /// To use per mip map view you need to specify the TextureFlags.PerMipHandles when creating the resource.
        /// </remarks>
        /// <param name="arrayOrDepthIndex">The index of the surface in an array (or depth slice index).</param>
        /// <param name="mipMapIndex">The index of the mip level.</param>
        /// <returns>The view for the render target.</returns>
        public RenderTargetView View(int arrayOrDepthIndex, int mipMapIndex)
        {
            return new RenderTargetView(unmanagedPtr, RenderTargetView.Types.Mip, mipMapIndex, arrayOrDepthIndex);
        }

        /// <summary>
        /// Gets the view to the array of surfaces.
        /// </summary>
        /// <remarks>
        /// To use array texture view you need to create render target as an array.
        /// </remarks>
        /// <returns>The view for the render target.</returns>
        public RenderTargetView ViewArray()
        {
            return new RenderTargetView(unmanagedPtr, RenderTargetView.Types.Array);
        }

        /// <summary>
        /// Gets the view to the volume texture (3D).
        /// </summary>
        /// <remarks>
        /// To use volume texture view you need to create render target as a volume resource (3D texture with Depth > 1).
        /// </remarks>
        /// <returns>The view for the render target.</returns>
        public RenderTargetView ViewVolume()
        {
            return new RenderTargetView(unmanagedPtr, RenderTargetView.Types.Volume);
        }

        private class Temporary
        {
            public RenderTarget Texture;
            public bool IsFree;
            public float LastUsage;

            public Temporary(PixelFormat format, int width, int height, TextureFlags flags, MSAALevel msaa)
            {
                Texture = New();
                Texture.Init(format, width, height, flags, 1, msaa);
            }

            public bool TryReuse(PixelFormat format, int width, int height, TextureFlags flags, MSAALevel msaa)
            {
                return IsFree
                       && Texture.Format == format
                       && Texture.Width == width
                       && Texture.Height == height
                       && Texture.Flags == flags
                       && Texture.MultiSampleLevel == msaa;
            }

            public RenderTarget OnUse()
            {
                IsFree = false;
                LastUsage = Time.UnscaledGameTime;
                return Texture;
            }
        }

        private static readonly List<Temporary> Pool = new List<Temporary>(8);

        /// <summary>
        /// The timeout value for unused temporary render targets (in seconds).
        /// When render target is not used for a given amount of time, it's being released.
        /// </summary>
        public static float UnusedTemporaryRenderTargetLifeTime = 5.0f;

        /// <summary>
        /// Allocates a temporary render target.
        /// </summary>
        /// <param name="format">The texture format.</param>
        /// <param name="width">The width in pixels.</param>
        /// <param name="height">The height in pixels.</param>
        /// <param name="flags">The texture usage flags.</param>
        /// <param name="msaa">The texture multisampling level.</param>
        /// <returns>Created texture.</returns>
        public static RenderTarget GetTemporary(PixelFormat format, int width, int height, TextureFlags flags = TextureFlags.ShaderResource | TextureFlags.RenderTarget, MSAALevel msaa = MSAALevel.None)
        {
            // Try reuse
            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i].TryReuse(format, width, height, flags, msaa))
                {
                    return Pool[i].OnUse();
                }
            }

            // Allocate new
            var target = new Temporary(format, width, height, flags, msaa);
            Pool.Add(target);
            return target.OnUse();
        }

        /// <summary>
        /// Releases a temporary render target allocated using <see cref="GetTemporary"/>.
        /// Later calls to <see cref="GetTemporary"/> will reuse the RenderTexture created earlier if possible.
        /// When no one has requested the temporary RenderTexture for a few frames it will be destroyed.
        /// </summary>
        /// <param name="temp">The temporary.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ReleaseTemporary(RenderTarget temp)
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i].Texture == temp)
                {
                    Pool[i].IsFree = true;
                    return;
                }
            }

            throw new InvalidOperationException("Cannot release render target.");
        }

        static RenderTarget()
        {
            Scripting.Update += Update;
            Scripting.Exit += Exit;
        }

        private static void Update()
        {
            Profiler.BeginEvent("RenderTarget.Update");

            // Flush old unused render targets
            var time = Time.UnscaledGameTime;
            for (int i = 0; i < Pool.Count; i++)
            {
                if (time - Pool[i].LastUsage >= UnusedTemporaryRenderTargetLifeTime)
                {
                    // Recycle
                    Destroy(Pool[i].Texture);
                    Pool.RemoveAt(i);
                }
            }

            Profiler.EndEvent();
        }

        private static void Exit()
        {
            Profiler.BeginEvent("RenderTarget.Exit");

            // Cleanup render targets
            for (int i = 0; i < Pool.Count; i++)
            {
                if (!Pool[i].IsFree)
                {
                    Debug.LogError("Render Target Pool item is still in use while engine is exiting.");
                }

                Destroy(Pool[i].Texture);
            }
            Pool.Clear();

            Profiler.EndEvent();
        }

        /// <summary>
        /// Initializes render target texture.
        /// </summary>
        /// <param name="format">The surface pixels format.</param>
        /// <param name="size">The surface size in pixels (width, height).</param>
        /// <param name="flags">The surface usage flags.</param>
        /// <param name="mipMaps">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="multiSampleLevel">The surface multisampling level.</param>
        public void Init(PixelFormat format, Vector2 size, TextureFlags flags = TextureFlags.ShaderResource | TextureFlags.RenderTarget, int mipMaps = 1, MSAALevel multiSampleLevel = MSAALevel.None)
        {
            Init(format, (int)size.X, (int)size.Y, flags, mipMaps, multiSampleLevel);
        }
    }
}
