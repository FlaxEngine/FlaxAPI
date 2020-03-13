// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;

namespace FlaxEngine
{
    partial class RenderTask
    {
        /// <summary>
        /// The custom tag object value linked to the rendering task.
        /// </summary>
        public object Tag;

        /// <summary>
        /// The action called on rendering begin.
        /// </summary>
        public event Action<RenderTask, GPUContext> Begin;

        /// <summary>
        /// The action called on rendering.
        /// </summary>
        public event Action<RenderTask, GPUContext> Render;

        /// <summary>
        /// The action called on rendering end.
        /// </summary>
        public event Action<RenderTask, GPUContext> End;

        internal void OnBegin(GPUContext context)
        {
            Begin?.Invoke(this, context);
        }

        internal void OnRender(GPUContext context)
        {
            Render?.Invoke(this, context);
        }

        internal void OnEnd(GPUContext context)
        {
            End?.Invoke(this, context);
        }
    }

    partial class SceneRenderTask
    {
        /// <summary>
        /// Gets or sets the view flags (via <see cref="View"/> property).
        /// </summary>
        public ViewFlags ViewFlags
        {
            get => View.Flags;
            set
            {
                var view = View;
                view.Flags = value;
                View = view;
            }
        }

        /// <summary>
        /// Gets or sets the view mode (via <see cref="View"/> property).
        /// </summary>
        public ViewMode ViewMode
        {
            get => View.Mode;
            set
            {
                var view = View;
                view.Mode = value;
                View = view;
            }
        }

        /// <summary>
        /// The global custom post processing effects applied to all <see cref="SceneRenderTask"/> (applied to tasks that have <see cref="AllowGlobalCustomPostFx"/> turned on).
        /// </summary>
        public static readonly HashSet<PostProcessEffect> GlobalCustomPostFx = new HashSet<PostProcessEffect>();

        /// <summary>
        /// The custom post processing effects.
        /// </summary>
        public readonly HashSet<PostProcessEffect> CustomPostFx = new HashSet<PostProcessEffect>();

        /// <summary>
        /// True if allow using global custom PostFx when rendering this task.
        /// </summary>
        public bool AllowGlobalCustomPostFx = true;

        /// <summary>
        /// The action called on view rendering to collect draw calls. It allows to extend rendering pipeline and draw custom geometry non-existing in the scene or custom actors set.
        /// </summary>
        public event Action<RenderContext> CollectDrawCalls;

        internal void OnCollectDrawCalls(ref RenderContext renderContext)
        {
            CollectDrawCalls?.Invoke(renderContext);
        }

        /// <summary>
        /// The action called after scene rendering. Can be used to render additional visual elements to the output.
        /// </summary>
        public event Action<GPUContext, RenderContext> PostRender;

        internal void OnPostRender(GPUContext context, ref RenderContext renderContext)
        {
            PostRender?.Invoke(context, renderContext);
        }

        private static List<PostProcessEffect> _postFx;
        private static IntPtr[] _postFxPtr;
        private static readonly PostFxComparer _postFxComparer = new PostFxComparer();

        internal sealed class PostFxComparer : IComparer<PostProcessEffect>
        {
            public int Compare(PostProcessEffect x, PostProcessEffect y)
            {
                return x.Order - y.Order;
            }
        }

        internal IntPtr[] GetPostFx(out int count)
        {
            if (_postFx == null)
                _postFx = new List<PostProcessEffect>();

            // Get custom post effects to render (registered ones and from the current camera)

            foreach (var postFx in CustomPostFx)
            {
                if (postFx.CanRender)
                    _postFx.Add(postFx);
            }
            if (AllowGlobalCustomPostFx)
            {
                foreach (var postFx in GlobalCustomPostFx)
                {
                    if (postFx.CanRender)
                        _postFx.Add(postFx);
                }
            }
            var camera = Camera;
            if (camera != null)
            {
                var perCameraPostFx = camera.GetScripts<PostProcessEffect>();
                for (int i = 0; i < perCameraPostFx.Length; i++)
                {
                    var postFx = perCameraPostFx[i];
                    if (postFx.CanRender)
                        _postFx.Add(postFx);
                }
            }

            // Sort postFx
            _postFx.Sort(_postFxComparer);

            // Convert into unmanaged objects
            count = _postFx.Count;
            if (_postFxPtr == null || _postFxPtr.Length < count)
                _postFxPtr = new IntPtr[_postFx.Capacity];
            for (int i = 0; i < count; i++)
                _postFxPtr[i] = GetUnmanagedPtr(_postFx[i]);

            // Release references to managed objects
            _postFx.Clear();

            return _postFxPtr;
        }
    }
}
