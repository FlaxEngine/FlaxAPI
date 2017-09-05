////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit arrays/list.
    /// </summary>
    public abstract class CollectionEditor : CustomEditor
    {
        private IntegerValueElement _size;
        private int _elementsCount;
        private bool _readOnly;
        private bool _canReorderItems;
        private bool _notNullItems;

        /// <summary>
        /// Gets the length of the collection.
        /// </summary>
        public abstract int Count { get; }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _readOnly = false;
            _canReorderItems = true;
            _notNullItems = false;

            // No support for different collections for now
            if (HasDiffrentValues || HasDiffrentTypes)
                return;

            var type = Values.Type;
            var size = Count;

            // Try get MemberCollectionAttribute for collection editor meta
            if (Values.Info != null)
            {
                var attributes = Values.Info.GetCustomAttributes(true);
                var memberCollection = (MemberCollectionAttribute)attributes.FirstOrDefault(x => x is MemberCollectionAttribute);
                if (memberCollection != null)
                {
                    // TODO: handle ReadOnly and NotNullItems by filtering child editors SetValue
                    // TODO: handle CanReorderItems

                    _readOnly = memberCollection.ReadOnly;
                    _canReorderItems = memberCollection.CanReorderItems;
                    _notNullItems = memberCollection.NotNullItems;
                }
            }

            // Size
            if (_readOnly)
            {
                layout.Label("Size", size.ToString());
            }
            else
            {
                _size = layout.IntegerValue("Size");
                _size.IntValue.MinValue = 0;
                _size.IntValue.MaxValue = ushort.MaxValue;
                _size.IntValue.Value = size;
                _size.IntValue.ValueChanged += OnSizeChanged;
            }

            // Elements
            if (size > 0)
            {
                var elementType = type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();
                for (int i = 0; i < size; i++)
                {
                    layout.Object("Element " + i, new ListValueContainer(elementType, i, Values));
                }
            }
            _elementsCount = size;
        }

        private void OnSizeChanged()
        {
            if (IsSetBlocked)
                return;

            Resize(_size.IntValue.Value);
        }

        /// <summary>
        /// Resizes collection to the specified new size.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        protected abstract void Resize(int newSize);
        
        /// <inheritdoc />
        public override void Refresh()
        {
            // Check if collection has been resized (by UI or from external source)
            if (Count != _elementsCount)
            {
                RebuildLayout();
            }
        }
    }
}
