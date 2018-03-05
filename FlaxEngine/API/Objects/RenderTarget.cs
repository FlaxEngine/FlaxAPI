////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.Rendering
{
    public partial class RenderTarget
    {
        /// <summary>
        /// Returns true if texture has size that is power of two.
        /// </summary>
        public bool IsPowerOfTwo => Mathf.IsPowerOfTwo(Width) && Mathf.IsPowerOfTwo(Height);

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

        private static readonly List<Temporary> _tmpRenderTargets = new List<Temporary>(8);

        /// <summary>
        /// The timout value for unused temporary render targets (in seconds).
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
            for (int i = 0; i < _tmpRenderTargets.Count; i++)
            {
                if (_tmpRenderTargets[i].TryReuse(format, width, height, flags, msaa))
                {
                    return _tmpRenderTargets[i].OnUse();
                }
            }

            // Allocate new
            var target = new Temporary(format, width, height, flags, msaa);
            _tmpRenderTargets.Add(target);
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
            for (int i = 0; i < _tmpRenderTargets.Count; i++)
            {
                if (_tmpRenderTargets[i].Texture == temp)
                {
                    _tmpRenderTargets[i].IsFree = true;
                    return;
                }
            }

            throw new InvalidOperationException("Cannot release render target.");
        }

        static RenderTarget()
        {
            Scripting.Update += Update;
        }

        private static void Update()
        {
	        Profiler.BeginEvent("RenderTarget.Update");

            // Flush old unused render targets
            var time = Time.UnscaledGameTime;
            for (int i = 0; i < _tmpRenderTargets.Count; i++)
            {
                if (time - _tmpRenderTargets[i].LastUsage >= UnusedTemporaryRenderTargetLifeTime)
                {
                    // Recycle
                    Destroy(_tmpRenderTargets[i].Texture);
                    _tmpRenderTargets.RemoveAt(i);
                }
            }

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
