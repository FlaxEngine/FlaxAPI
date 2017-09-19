////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// The custom data container used during collecting draw data for drawing debug visuals of selected objects.
    /// </summary>
    public class ViewportDebugDrawData
    {
        private readonly List<IntPtr> _actors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportDebugDrawData"/> class.
        /// </summary>
        /// <param name="actorsCapacity">The actors capacity.</param>
        public ViewportDebugDrawData(int actorsCapacity = 0)
        {
            _actors = new List<IntPtr>(actorsCapacity);
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
        /// Clears this data collector.
        /// </summary>
        public virtual void Clear()
        {
            _actors.Clear();
        }
    }
}
