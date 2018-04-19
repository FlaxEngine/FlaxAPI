// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Base class for drag and drop operation helpers.
    /// </summary>
    /// <typeparam name="T">Type of the objects to collect from drag data.</typeparam>
    public abstract class DragHelper<T>
    {
        /// <summary>
        /// The objects gathered.
        /// </summary>
        public readonly List<T> Objects = new List<T>();

        /// <summary>
        /// Gets a value indicating whether this instance has valid drag data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has valid drag data; otherwise, <c>false</c>.
        /// </value>
        public bool HasValidDrag => Objects.Count > 0;

        /// <summary>
        /// Gets the current drag effect.
        /// </summary>
        /// <value>
        /// The effect.
        /// </value>
        public DragDropEffect Effect => HasValidDrag ? DragDropEffect.Move : DragDropEffect.None;

        /// <summary>
        /// Invalids the drag data.
        /// </summary>
        public void InvalidDrag()
        {
            Objects.Clear();
        }

        /// <summary>
        /// Called when drag enters.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function. Check if gathered object is valid to drop it.</param>
        /// <returns>True if drag event is valid and can be performed, otherwise false.</returns>
        public bool OnDragEnter(DragData data, Func<T, bool> validateFunc)
        {
            if(data == null || validateFunc == null)
                throw new ArgumentNullException();

            Objects.Clear();

            if(data is DragDataText text)
                GetherObjects(text, validateFunc);
            else if(data is DragDataFiles files)
                GetherObjects(files, validateFunc);

            return HasValidDrag;
        }

        /// <summary>
        /// Called when drag leaves.
        /// </summary>
        public void OnDragLeave()
        {
            Objects.Clear();
        }

        /// <summary>
        /// Called when drag drops.
        /// </summary>
        public void OnDragDrop()
        {
            Objects.Clear();
        }

        /// <summary>
        /// Gethers the objects from the drag data (text).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function.</param>
        protected virtual void GetherObjects(DragDataText data, Func<T, bool> validateFunc)
        {
        }

        /// <summary>
        /// Gethers the objects from the drag data (files).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function.</param>
        protected virtual void GetherObjects(DragDataFiles data, Func<T, bool> validateFunc)
        {
        }
    }
}
