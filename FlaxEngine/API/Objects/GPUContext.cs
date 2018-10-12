// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Defines actors to draw sources.
    /// </summary>
    public enum ActorsSources
    {
        /// <summary>
        /// The actors from the loaded scenes.
        /// </summary>
        Scenes = 1,

        /// <summary>
        /// The actors from the custom collection.
        /// </summary>
        CustomActors = 2,

        /// <summary>
        /// The actors from the loaded scenes and custom collection.
        /// </summary>
        ScenesAndCustomActors = Scenes | CustomActors,
    }

    public partial class GPUContext
    {
        private static IntPtr[] _cachedActors;
        private static List<PostProcessEffect> _cachedPostFxA;
        private static IntPtr[] _cachedPostFxB;
        private static PostFxComparer _postFxComparer = new PostFxComparer();

        internal sealed class PostFxComparer : IComparer<PostProcessEffect>
        {
            public int Compare(PostProcessEffect x, PostProcessEffect y)
            {
                return x.Order - y.Order;
            }
        }

        private IntPtr[] GetActors(Actor[] customActors, out int actorsCount)
        {
            IntPtr[] actors = null;
            actorsCount = 0;

            if (customActors != null && customActors.Length > 0)
            {
                actorsCount = customActors.Length;
                if (_cachedActors == null || _cachedActors.Length < actorsCount)
                    _cachedActors = new IntPtr[Mathf.NextPowerOfTwo(actorsCount)];
                actors = _cachedActors;

                for (int i = 0; i < actorsCount; i++)
                {
                    _cachedActors[i] = GetUnmanagedPtr(customActors[i]);
                }
            }

            return actors;
        }

        private IntPtr[] GetActors(List<Actor> customActors, out int actorsCount)
        {
            IntPtr[] actors = null;
            actorsCount = 0;

            if (customActors != null && customActors.Count > 0)
            {
                actorsCount = customActors.Count;
                if (_cachedActors == null || _cachedActors.Length < actorsCount)
                    _cachedActors = new IntPtr[Mathf.NextPowerOfTwo(actorsCount)];
                actors = _cachedActors;

                for (int i = 0; i < actorsCount; i++)
                {
                    _cachedActors[i] = GetUnmanagedPtr(customActors[i]);
                }
            }

            return actors;
        }

        private IntPtr[] GetPostFx(HashSet<PostProcessEffect> customPostFx, out int postFxCount)
        {
            IntPtr[] postFx = null;
            postFxCount = 0;

            if (customPostFx != null && customPostFx.Count > 0)
            {
                if (_cachedPostFxA == null)
                    _cachedPostFxA = new List<PostProcessEffect>();
                _cachedPostFxA.Capacity = Mathf.Max(_cachedPostFxA.Capacity, Mathf.NextPowerOfTwo(customPostFx.Count));

                foreach (var e in customPostFx)
                {
                    if (e && e.CanRender)
                        _cachedPostFxA.Add(e);
                }

                _cachedPostFxA.Sort(_postFxComparer);

                postFxCount = _cachedPostFxA.Count;
                if (_cachedPostFxB == null || _cachedPostFxB.Length < postFxCount)
                    _cachedPostFxB = new IntPtr[_cachedPostFxA.Capacity];
                postFx = _cachedPostFxB;

                for (int i = 0; i < postFxCount; i++)
                {
                    _cachedPostFxB[i] = GetUnmanagedPtr(_cachedPostFxA[i]);
                }

                _cachedPostFxA.Clear();
            }

            return postFx;
        }

        /// <summary>
        /// Draws sprite atlas texture to render target. Copies contents with resizing and format conversion support. Uses linear texture sampling.
        /// </summary>
        /// <param name="dst">Target surface.</param>
        /// <param name="src">Source sprite atlas.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void Draw(RenderTarget dst, SpriteAtlas src)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_Draw2(unmanagedPtr, Object.GetUnmanagedPtr(dst), Object.GetUnmanagedPtr(src));
#endif
        }

        /// <summary>
        /// Draws scene.
        /// </summary>
        /// <param name="task">Calling render task.</param>
        /// <param name="output">Output texture.</param>
        /// <param name="buffers">Frame rendering buffers.</param>
        /// <param name="view">Rendering view description structure.</param>
        /// <param name="flags">Custom view flags collection.</param>
        /// <param name="mode">Custom view mode option.</param>
        /// <param name="customActors">Custom set of actors to render.</param>
        /// <param name="actorsSource">Actors source to use during rendering.</param>
        /// <param name="customPostFx">The set of custom post effects to use during rendering. Use null to skip it.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void DrawScene(RenderTask task, RenderTarget output, RenderBuffers buffers, RenderView view, ViewFlags flags, ViewMode mode, Actor[] customActors = null, ActorsSources actorsSource = ActorsSources.ScenesAndCustomActors, HashSet<PostProcessEffect> customPostFx = null)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged actors
            var actors = GetActors(customActors, out int actorsCount);

            // Get unmanaged postFx
            var postFx = GetPostFx(customPostFx, out int postFxCount);

            Internal_DrawScene(unmanagedPtr, GetUnmanagedPtr(task), GetUnmanagedPtr(output), GetUnmanagedPtr(buffers), ref view, flags, mode, actors, actorsCount, actorsSource, postFx, postFxCount);
#endif
        }

        /// <summary>
        /// Draws scene.
        /// </summary>
        /// <param name="task">Calling render task.</param>
        /// <param name="output">Output texture.</param>
        /// <param name="buffers">Frame rendering buffers.</param>
        /// <param name="view">Rendering view description structure.</param>
        /// <param name="flags">Custom view flags collection.</param>
        /// <param name="mode">Custom view mode option.</param>
        /// <param name="customActors">Custom set of actors to render.</param>
        /// <param name="actorsSource">Actors source to use during rendering.</param>
        /// <param name="customPostFx">The set of custom post effects to use during rendering. Use null to skip it.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void DrawScene(RenderTask task, RenderTarget output, RenderBuffers buffers, RenderView view, ViewFlags flags, ViewMode mode, List<Actor> customActors = null, ActorsSources actorsSource = ActorsSources.ScenesAndCustomActors, HashSet<PostProcessEffect> customPostFx = null)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged actors
            var actors = GetActors(customActors, out int actorsCount);

            // Get unmanaged postFx
            var postFx = GetPostFx(customPostFx, out int postFxCount);

            Internal_DrawScene(unmanagedPtr, GetUnmanagedPtr(task), GetUnmanagedPtr(output), GetUnmanagedPtr(buffers), ref view, flags, mode, actors, actorsCount, actorsSource, postFx, postFxCount);
#endif
        }

        /// <summary>
        /// Draws scene objects depth (to the output Z buffer).
        /// </summary>
        /// <param name="task">Calling render task. Uses it's cache, buffers and the view properties.</param>
        /// <param name="output">Output depth buffer.</param>
        /// <param name="drawTransparency">True if render both opaque and semi-transparent objects.</param>
        /// <param name="customActors">Custom set of actors to render. If set to null default scene will be rendered.</param>
        /// <param name="actorsSource">Actors source to use during rendering.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void DrawSceneDepth(SceneRenderTask task, RenderTarget output, bool drawTransparency = true, Actor[] customActors = null, ActorsSources actorsSource = ActorsSources.Scenes)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged actors
            var actors = GetActors(customActors, out int actorsCount);

            Internal_DrawSceneDepth(unmanagedPtr, GetUnmanagedPtr(task), GetUnmanagedPtr(output), drawTransparency, actors, actorsCount, actorsSource);
#endif
        }

        /// <summary>
        /// Clears texture surface with a color.
        /// </summary>
        /// <param name="renderTarget">The render target to clear. Must be valid and created.</param>
        /// <param name="color">Clear color.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void Clear(RenderTarget renderTarget, Color color)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            if (renderTarget == null)
                throw new ArgumentNullException(nameof(renderTarget));
            Internal_Clear(unmanagedPtr, renderTarget.View(), ref color);
#endif
        }

        /// <summary>
        /// Draws scene objects depth (to the output Z buffer).
        /// </summary>
        /// <param name="task">Calling render task. Uses it's cache, buffers and the view properties.</param>
        /// <param name="output">Output depth buffer.</param>
        /// <param name="drawTransparency">True if render both opaque and semi-transparent objects.</param>
        /// <param name="customActors">Custom set of actors to render. If set to null default scene will be rendered.</param>
        /// <param name="actorsSource">Actors source to use during rendering.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void DrawSceneDepth(SceneRenderTask task, RenderTarget output, bool drawTransparency = true, List<Actor> customActors = null, ActorsSources actorsSource = ActorsSources.Scenes)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged actors
            var actors = GetActors(customActors, out int actorsCount);

            Internal_DrawSceneDepth(unmanagedPtr, GetUnmanagedPtr(task), GetUnmanagedPtr(output), drawTransparency, actors, actorsCount, actorsSource);
#endif
        }

        /// <summary>
        /// Draws postFx material to the render target.
        /// </summary>
        /// <param name="material">The material to render. It must be a post fx material.</param>
        /// <param name="output">The output texture. Must be valid and created.</param>
        /// <param name="input">The input texture. It's optional.</param>
        /// <param name="sceneRenderTask">Render task to use it's view description and the render buffers.</param>
        public void DrawPostFxMaterial(MaterialBase material, RenderTarget output, RenderTarget input, SceneRenderTask sceneRenderTask)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_DrawPostFxMaterial1(unmanagedPtr, GetUnmanagedPtr(material), GetUnmanagedPtr(output), GetUnmanagedPtr(input), ref sceneRenderTask.View, Object.GetUnmanagedPtr(sceneRenderTask.Buffers));
#endif
        }

        /// <summary>
        /// Draws postFx material to the render target.
        /// </summary>
        /// <param name="material">The material to render. It must be a post fx material.</param>
        /// <param name="output">The output texture. Must be valid and created.</param>
        /// <param name="input">The input texture. It's optional.</param>
        public void DrawPostFxMaterial(MaterialBase material, RenderTarget output, RenderTarget input = null)
        {
            DrawPostFxMaterial(material, output.View(), input);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawScene(IntPtr obj, IntPtr task, IntPtr output, IntPtr buffers, ref RenderView view, ViewFlags flags, ViewMode mode, IntPtr[] customActors, int customActorsCount, ActorsSources actorsSource, IntPtr[] customPostFx, int customPostFxCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSceneDepth(IntPtr obj, IntPtr task, IntPtr output, bool drawTransparency, IntPtr[] customActors, int customActorsCount, ActorsSources actorsSource);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ExecuteDrawCalls(IntPtr obj, IntPtr task, IntPtr output, RenderTask.DrawCall[] drawCalls, RenderPass pass);
#endif

        #endregion
    }
}
