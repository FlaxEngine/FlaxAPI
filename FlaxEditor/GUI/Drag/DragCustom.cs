using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public sealed class DragCustom<T> : DragHelper<T> where T : class
    {
        private Func<DragData, T> _fromDragData { get; }

        private Action<T> _spawn { get; }

        public DragCustom(Func<T, DragData> toDragData, Func<DragData, T> fromDragData, Action<T> spawn) : this(_ => true, toDragData, fromDragData, spawn)
        {
        }

        public DragCustom(Func<T, bool> validateFunction, Func<T, DragData> toDragData, Func<DragData, T> fromDragData, Action<T> spawn) : base(validateFunction)
        {
            ToDragData = toDragData;
            _fromDragData = fromDragData;
            _spawn = spawn;
        }

        public Func<T, DragData> ToDragData { get; }

        ///<inheritdoc/>
        public override bool OnDragEnter(DragData data)
        {
            if (data == null || ValidateFunction == null)
                throw new ArgumentNullException();

            Objects.Clear();

            var item = _fromDragData(data);
            if (item != null && ValidateFunction(item))
                Objects.Add(item);

            return HasValidDrag;
        }

        ///<inheritdoc/>
        public override void OnDragDrop()
        {
            if (HasValidDrag) _spawn(Objects[0]);
            base.OnDragDrop();
        }
    }
}
