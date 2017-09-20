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
        private readonly List<ModelsToHighlight> _highlights;
        private MaterialBase _highlightMaterial;

        private struct ModelsToHighlight
        {
            public ModelActor Model;
            public int EntryIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportDebugDrawData"/> class.
        /// </summary>
        /// <param name="actorsCapacity">The actors capacity.</param>
        public ViewportDebugDrawData(int actorsCapacity = 0)
        {
            _actors = new List<IntPtr>(actorsCapacity);
            _highlights = new List<ModelsToHighlight>(actorsCapacity);
            _highlightMaterial = FlaxEngine.Content.LoadAsync<MaterialBase>(EditorAssets.HighlightMaterial);
        }

        internal IntPtr[] ActorsPtrs => _actors.Count > 0 ? _actors.ToArray() : null;

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

            var entries = model.Entries;
            for (int i = 0; i < entries.Length; i++)
            {
                HighlightModel(model, i);
            }
        }

        /// <summary>
        /// Highlights the model entry.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entryIndex">Index of the entry to highlight.</param>
        public void HighlightModel(ModelActor model, int entryIndex)
        {
            _highlights.Add(new ModelsToHighlight
            {
                Model = model,
                EntryIndex = entryIndex
            });
        }

        /// <summary>
        /// Called when task calls <see cref="SceneRenderTask.Draw"/> event.
        /// </summary>
        /// <param name="collector">The draw calls collector.</param>
        public virtual void OnDraw(DrawCallsCollector collector)
        {
            Matrix m1, m2, world;
            for (int i = 0; i < _highlights.Count; i++)
            {
                var hightlight = _highlights[i];
                var model = hightlight.Model;
                if (model.Model == null)
                    continue;

                model.Transform.GetWorld(out m1);
                model.Entries[hightlight.EntryIndex].Transform.GetWorld(out m2);
                Matrix.Multiply(ref m2, ref m1, out world);
                BoundingSphere bounds = BoundingSphere.FromBox(model.Box);

                collector.AddDrawCall(model.Model, hightlight.EntryIndex, _highlightMaterial, ref bounds, ref world);
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
    }
}
