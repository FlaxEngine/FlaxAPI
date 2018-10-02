// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        private readonly List<Vector3> _highlightTriangles = new List<Vector3>(64);
        private Vector3[] _highlightTrianglesSet;
        private int[] _highlightIndicesSet;
        private Model _highlightTrianglesModel;

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
            _highlightTrianglesModel = FlaxEngine.Content.CreateVirtualAsset<Model>();
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
        public void HighlightModel(StaticModel model)
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
        public void HighlightModel(StaticModel model, int entryIndex)
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
            var vertices = surface.GetVertices();
            if (vertices.Length > 0)
            {
                _highlightTriangles.AddRange(vertices);
            }
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
                if (highlight.Target is StaticModel staticModel)
                {
                    if (staticModel.Model == null)
                        continue;

                    staticModel.Transform.GetWorld(out m1);
                    staticModel.Entries[highlight.EntryIndex].Transform.GetWorld(out m2);
                    Matrix.Multiply(ref m2, ref m1, out world);
                    BoundingSphere bounds = BoundingSphere.FromBox(staticModel.Box);

                    collector.AddDrawCall(staticModel.Model, highlight.EntryIndex, _highlightMaterial, ref bounds, ref world);
                }
            }

            if (_highlightTriangles.Count > 0)
            {
                var mesh = _highlightTrianglesModel.LODs[0].Meshes[0];
                if (!Utils.ArraysEqual(_highlightTrianglesSet, _highlightTriangles))
                {
                    _highlightIndicesSet = new int[_highlightTriangles.Count];
                    for (int i = 0; i < _highlightIndicesSet.Length; i++)
                        _highlightIndicesSet[i] = i;
                    _highlightTrianglesSet = _highlightTriangles.ToArray();
                    mesh.UpdateMesh(_highlightTrianglesSet, _highlightIndicesSet);
                }

                world = Matrix.Identity;
                collector.AddDrawCall(mesh, _highlightMaterial, ref world);
            }
        }

        /// <summary>
        /// Clears this data collector.
        /// </summary>
        public virtual void Clear()
        {
            _actors.Clear();
            _highlights.Clear();
            _highlightTriangles.Clear();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public virtual void Dispose()
        {
            _highlightMaterial = null;
            FlaxEngine.Object.Destroy(ref _highlightTrianglesModel);
        }

        private struct HighlightData
        {
            public object Target;
            public int EntryIndex;
        }
    }
}
