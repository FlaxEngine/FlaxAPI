// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    public static class RenderTarget
    {
        private class Temporary
        {
            public GPUTexture Texture;
            public bool IsFree;
            public float LastUsage;

            public Temporary(PixelFormat format, int width, int height, GPUTextureFlags flags, MSAALevel msaa)
            {
                Texture = GPUDevice.CreateTexture();
                var desc = GPUTextureDescription.New2D(width, height, format, flags, 1, 1, msaa);
                Texture.Init(ref desc);
            }

            public bool TryReuse(PixelFormat format, int width, int height, GPUTextureFlags flags, MSAALevel msaa)
            {
                return IsFree
                       && Texture.Format == format
                       && Texture.Width == width
                       && Texture.Height == height
                       && Texture.Flags == flags
                       && Texture.MultiSampleLevel == msaa;
            }

            public GPUTexture OnUse()
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
        public static GPUTexture GetTemporary(PixelFormat format, int width, int height, GPUTextureFlags flags = GPUTextureFlags.ShaderResource | GPUTextureFlags.RenderTarget, MSAALevel msaa = MSAALevel.None)
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
        public static void ReleaseTemporary(GPUTexture temp)
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
            Profiler.BeginEvent("GPUTexture.Update");

            // Flush old unused render targets
            var time = Time.UnscaledGameTime;
            for (int i = 0; i < Pool.Count; i++)
            {
                if (time - Pool[i].LastUsage >= UnusedTemporaryRenderTargetLifeTime)
                {
                    // Recycle
                    var t = Pool[i].Texture;
                    t.ReleaseGPU();
                    Object.Destroy(t);
                    Pool.RemoveAt(i);
                }
            }

            Profiler.EndEvent();
        }

        private static void Exit()
        {
            Profiler.BeginEvent("GPUTexture.Exit");

            // Cleanup render targets
            for (int i = 0; i < Pool.Count; i++)
            {
                if (!Pool[i].IsFree)
                {
                    Debug.LogError("Render Target Pool item is still in use while engine is exiting.");
                }

                var t = Pool[i].Texture;
                t.ReleaseGPU();
            }
            Pool.Clear();

            Profiler.EndEvent();
        }
    }
}
