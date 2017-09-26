////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor
{
    /// <summary>
    /// The custom data container used during collecting draw data for drawing debug visuals of selected objects.
    /// </summary>
    public class ViewportDebugDrawData
    {
        private readonly List<IntPtr> _actors;
        private readonly List<HighlightData> _highlights;
        private MaterialBase _highlightMaterial;

        internal IntPtr[] ActorsPtrs => _actors.Count > 0 ? _actors.ToArray() : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportDebugDrawData" /> class.
        /// </summary>
        /// <param name="actorsCapacity">The actors capacity.</param>
        public ViewportDebugDrawData(int actorsCapacity = 0)
        {
            _actors = new List<IntPtr>(actorsCapacity);
            _highlights = new List<HighlightData>(actorsCapacity);
            _highlightMaterial = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(EditorAssets.HighlightMaterial);
        }

        /// <summary>
        /// Adds the specified actor to draw it's debug visuals.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public void Add(Actor actor)
        {
            _actors.Add(actor.unmanagedPtr);
        }

        /// <summary>
        /// Highlights the model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void HighlightModel(ModelActor model)
        {
            if (model.Model == null)
                return;

            ModelEntryInfo[] entries = model.Entries;
            for (var i = 0; i < entries.Length; i++)
                HighlightModel(model, i);
        }

        /// <summary>
        /// Highlights the model entry.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entryIndex">Index of the entry to highlight.</param>
        public void HighlightModel(ModelActor model, int entryIndex)
        {
            _highlights.Add(new HighlightData
            {
                Target = model,
                EntryIndex = entryIndex
            });
        }

        /// <summary>
        /// Highlights the brush surface.
        /// </summary>
        /// <param name="surface">The surface.</param>
        public void HighlightBrushSurface(BrushSurface surface)
        {
            _highlights.Add(new HighlightData
            {
                Target = surface
            });
        }

        /// <summary>
        /// Called when task calls <see cref="SceneRenderTask.Draw" /> event.
        /// </summary>
        /// <param name="collector">The draw calls collector.</param>
        public virtual void OnDraw(DrawCallsCollector collector)
        {
            if (_highlightMaterial == null)
                return;
            
            Matrix m1, m2, world;
            for (var i = 0; i < _highlights.Count; i++)
            {
                HighlightData highlight = _highlights[i];
                if (highlight.Target is ModelActor modelActor)
                {
                    if (modelActor.Model == null)
                        continue;

                    modelActor.Transform.GetWorld(out m1);
                    modelActor.Entries[highlight.EntryIndex].Transform.GetWorld(out m2);
                    Matrix.Multiply(ref m2, ref m1, out world);
                    BoundingSphere bounds = BoundingSphere.FromBox(modelActor.Box);

                    collector.AddDrawCall(modelActor.Model, highlight.EntryIndex, _highlightMaterial, ref bounds, ref world);
                }
                else if (highlight.Target is BrushSurface brushSurface)
                {
                    collector.AddDrawCall(brushSurface, _highlightMaterial);
                }
            }
        }

        /// <summary>
        /// Clears this data collector.
        /// </summary>
        public virtual void Clear()
        {
            _actors.Clear();
            _highlights.Clear();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public virtual void Dispose()
        {
            _highlightMaterial = null;
        }

        private struct HighlightData
        {
            public object Target;
            public int EntryIndex;
        }
    }
}
