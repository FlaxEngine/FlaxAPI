// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Handles a list of <see cref="DragHelper{T, U}"/>s
    /// </summary>
    public class DragHandlers
    {
        private readonly List<DragHelper> _dragHelpers = new List<DragHelper>();

        /// <summary>
        /// Adds a <see cref="DragHelper{T, U}"/>
        /// </summary>
        /// <param name="helper">The drag helper to add</param>
        /// <returns>The drag helper that was just added</returns>
        public DragHelper Add(DragHelper helper)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            _dragHelpers.Add(helper);
            return helper;
        }

        /// <summary>
        /// Called when drag enter.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The result.</returns>
        public DragDropEffect? OnDragEnter( /*ref Vector2 location, */ DragData data)
        {
            DragDropEffect? effect = null;
            foreach (var dragHelper in _dragHelpers)
            {
                if (dragHelper.OnDragEnter(data))
                {
                    effect = dragHelper.Effect;
                }
            }

            return effect;
        }

        /// <summary>
        /// Called when drag leaves.
        /// </summary>
        public void OnDragLeave()
        {
            foreach (var dragHelper in _dragHelpers)
            {
                dragHelper.OnDragLeave();
            }
        }

        /// <summary>
        /// Called when drag drops.
        /// </summary>
        /// <param name="dragEventArgs">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        public void OnDragDrop(DragEventArgs dragEventArgs)
        {
            foreach (var dragHelper in _dragHelpers)
            {
                dragHelper.OnDragDrop(dragEventArgs);
            }
        }

        /// <summary>
        /// Determines whether has valid drag handler to handle the drag request.
        /// </summary>
        /// <returns>True if has valid drag handler to handle the drag request, otherwise false.</returns>
        public bool HasValidDrag()
        {
            foreach (var dragHelper in _dragHelpers)
            {
                if (dragHelper.HasValidDrag)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the first valid drag helper.
        /// </summary>
        /// <returns>The drag helper.</returns>
        public DragHelper WithValidDrag()
        {
            return _dragHelpers
                   .DefaultIfEmpty()
                   .First(helper => helper.HasValidDrag);
        }

        /// <summary>
        /// Gets teh valid drag effect to use.
        /// </summary>
        /// <returns>The effect.</returns>
        public DragDropEffect? Effect()
        {
            foreach (var dragHelper in _dragHelpers)
            {
                if (dragHelper.HasValidDrag)
                {
                    return dragHelper.Effect;
                }
            }

            return null;
        }
    }
}
