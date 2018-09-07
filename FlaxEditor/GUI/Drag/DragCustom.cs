using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public abstract class DragCustom<T, U> : DragHelper<T>
        where U : DragEventArgs
    {
        protected DragCustom(Func<T, bool> validateFunction) : base(validateFunction)
        {
        }

        public abstract DragData ToDragData(T item);

        public abstract DragData ToDragData(IEnumerable<T> item);

        public abstract IEnumerable<T> FromDragData(DragData data);

        public abstract void DragDrop(U dragEventArgs, IEnumerable<T> item);

        ///<inheritdoc/>
        public sealed override bool OnDragEnter(DragData data)
        {
            if (data == null || ValidateFunction == null)
                throw new ArgumentNullException();

            Objects.Clear();
            var items = FromDragData(data);
            foreach (var item in items)
            {
                if (ValidateFunction(item))
                    Objects.Add(item);
            }

            return HasValidDrag;
        }

        ///<inheritdoc/>
        public sealed override void OnDragDrop()
        {
            if (HasValidDrag) DragDrop(null, Objects);
            base.OnDragDrop();
        }

        ///<inheritdoc/>
        public void OnDragDrop(U dragEventArgs)
        {
            if (HasValidDrag) DragDrop(dragEventArgs, Objects);
            base.OnDragDrop();
        }
    }
}
