using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public abstract class DragCustom<T> : DragHelper<T> where T : class
    {
        protected DragCustom(Func<T, bool> validateFunction) : base(validateFunction)
        {
        }

        public abstract DragData ToDragData(T item);

        public abstract T FromDragData(DragData data);

        public abstract void DragDrop(T item);

        ///<inheritdoc/>
        public sealed override bool OnDragEnter(DragData data)
        {
            if (data == null || ValidateFunction == null)
                throw new ArgumentNullException();

            Objects.Clear();

            var item = FromDragData(data);
            if (item != null && ValidateFunction(item))
                Objects.Add(item);

            return HasValidDrag;
        }

        ///<inheritdoc/>
        public sealed override void OnDragDrop()
        {
            if (HasValidDrag) DragDrop(Objects[0]);
            base.OnDragDrop();
        }
    }
}
