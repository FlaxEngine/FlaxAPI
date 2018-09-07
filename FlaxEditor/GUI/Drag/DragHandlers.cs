using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _dragHelpers.Add(helper);
            return helper;
        }

        public DragDropEffect? OnDragEnter(/*ref Vector2 location, */DragData data)
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

        public void OnDragLeave()
        {
            foreach (var dragHelper in _dragHelpers)
            {
                dragHelper.OnDragLeave();
            }
        }

        public void OnDragDrop(DragEventArgs dragEventArgs)
        {
            foreach (var dragHelper in _dragHelpers)
            {
                dragHelper.OnDragDrop(dragEventArgs);
            }
        }

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

        public DragHelper WithValidDrag()
        {
            return _dragHelpers
                .DefaultIfEmpty()
                .First(helper => helper.HasValidDrag);
        }

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
