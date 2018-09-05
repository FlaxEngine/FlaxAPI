using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public class DragHandlers
    {
        public readonly List<DragHelper> DragHelpers = new List<DragHelper>();

        public DragDropEffect? OnDragEnter(/*ref Vector2 location, */DragData data)
        {
            DragDropEffect? effect = null;
            foreach (var dragHelper in DragHelpers)
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
            foreach (var dragHelper in DragHelpers)
            {
                dragHelper.OnDragLeave();
            }
        }

        public bool HasValidDrag()
        {
            foreach (var dragHelper in DragHelpers)
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
            return DragHelpers
                .DefaultIfEmpty()
                .First(helper => helper.HasValidDrag);
        }

        public DragHelper<T> WithValidDrag<T>()
        {
            return DragHelpers
                .DefaultIfEmpty()
                .OfType<DragHelper<T>>()
                .First(helper => helper.HasValidDrag);
        }

        public DragDropEffect? Effect()
        {
            foreach (var dragHelper in DragHelpers)
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
