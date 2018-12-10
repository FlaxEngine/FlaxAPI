// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

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

        /// <summary>
        /// Gets the type of the collection elements.
        /// </summary>
        public Type ElementType
        {
            get
            {
                var type = Values.Type;
                return type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();
            }
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _readOnly = false;
            _canReorderItems = true;
            _notNullItems = false;

            // No support for different collections for now
            if (HasDifferentValues || HasDifferentTypes)
                return;

            var type = Values.Type;
            var size = Count;

            // Try get MemberCollectionAttribute for collection editor meta
            var attributes = Values.GetAttributes();
            if (attributes != null)
            {
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
                var elementType = ElementType;
                for (int i = 0; i < size; i++)
                {
                    layout.Object("Element " + i, new ListValueContainer(elementType, i, Values));
                }
            }
            _elementsCount = size;

            // Add/Remove buttons
            if (!_readOnly)
            {
                var area = layout.Space(20);
                var addButton = new Button(area.ContainerControl.Width - (16 + 16 + 2 + 2), 2, 16, 16)
                {
                    Text = "+",
                    TooltipText = "Add new item",
                    AnchorStyle = AnchorStyle.UpperRight,
                    Parent = area.ContainerControl
                };
                addButton.Clicked += () =>
                {
                    if (IsSetBlocked)
                        return;

                    Resize(Count + 1);
                };
                var removeButton = new Button(addButton.Right + 2, addButton.Y, 16, 16)
                {
                    Text = "-",
                    TooltipText = "Remove last item",
                    AnchorStyle = AnchorStyle.UpperRight,
                    Parent = area.ContainerControl
                };
                removeButton.Enabled = Count > 0;
                removeButton.Clicked += () =>
                {
                    if (IsSetBlocked)
                        return;

                    Resize(Count - 1);
                };
            }
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
            base.Refresh();

            // No support for different collections for now
            if (HasDifferentValues || HasDifferentTypes)
                return;

            // Check if collection has been resized (by UI or from external source)
            if (Count != _elementsCount)
            {
                RebuildLayout();
            }
        }
    }
}
