// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public abstract class DragHelper
    {
        public abstract bool HasValidDrag { get; }
        public abstract DragDropEffect Effect { get; }

        public abstract bool OnDragEnter(DragData data);

        public abstract void OnDragLeave();
    }

    /// <summary>
    /// Base class for drag and drop operation helpers.
    /// </summary>
    /// <typeparam name="T">Type of the objects to collect from drag data.</typeparam>
    public abstract class DragHelper<T> : DragHelper
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
        public override bool HasValidDrag => Objects.Count > 0;

        /// <summary>
        /// Gets the current drag effect.
        /// </summary>
        /// <value>
        /// The effect.
        /// </value>
        public override DragDropEffect Effect => HasValidDrag ? DragDropEffect.Move : DragDropEffect.None;

        /// <summary>
        /// The validation function
        /// </summary>
        public Func<T, bool> ValidateFunction { get; set; }

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragHelper(Func<T, bool> validateFunction)
        {
            ValidateFunction = validateFunction;
        }

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
        /// <returns>True if drag event is valid and can be performed, otherwise false.</returns>
        public override bool OnDragEnter(DragData data)
        {
            if (data == null || ValidateFunction == null)
                throw new ArgumentNullException();

            Objects.Clear();

            if (data is DragDataText text)
                GetherObjects(text, ValidateFunction);
            else if (data is DragDataFiles files)
                GetherObjects(files, ValidateFunction);

            return HasValidDrag;
        }

        /// <summary>
        /// Called when drag leaves.
        /// </summary>
        public override void OnDragLeave()
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
        /// Gathers the objects from the drag data (text).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function.</param>
        protected virtual void GetherObjects(DragDataText data, Func<T, bool> validateFunc)
        {
        }

        /// <summary>
        /// Gathers the objects from the drag data (files).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function.</param>
        protected virtual void GetherObjects(DragDataFiles data, Func<T, bool> validateFunc)
        {
        }
    }
}
