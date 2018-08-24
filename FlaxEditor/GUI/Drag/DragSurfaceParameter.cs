// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling dropping the surface parameter on the Visject Surface.
    /// </summary>
    public class DragSurfaceParameter
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="FlaxEditor.Surface.SurfaceParameter"/>.
        /// </summary>
        public const string DragPrefix = "SURFPARAM!?";

        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Parameter;

        /// <summary>
        /// Gets a value indicating whether this instance has valid drag data.
        /// </summary>
        public bool HasValidDrag => Parameter != null;

        /// <summary>
        /// Gets the current drag effect.
        /// </summary>
        public DragDropEffect Effect => HasValidDrag ? DragDropEffect.Move : DragDropEffect.None;

        /// <summary>
        /// Invalids the drag data.
        /// </summary>
        public void InvalidDrag()
        {
            Parameter = null;
        }

        /// <summary>
        /// Called when drag enters.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function. Check if gathered object is valid to drop it.</param>
        /// <returns>True if drag event is valid and can be performed, otherwise false.</returns>
        public bool OnDragEnter(DragData data, Func<string, bool> validateFunc)
        {
            if (data == null || validateFunc == null)
                throw new ArgumentNullException();

            Parameter = null;

            if (data is DragDataText text)
                GetherObjects(text, validateFunc);

            return HasValidDrag;
        }

        /// <summary>
        /// Called when drag leaves.
        /// </summary>
        public void OnDragLeave()
        {
            Parameter = null;
        }

        /// <summary>
        /// Called when drag drops.
        /// </summary>
        public void OnDragDrop()
        {
            Parameter = null;
        }

        /// <summary>
        /// Gathers the objects from the drag data (text).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="validateFunc">The validate function. Check if gathered object is valid to drop it.</param>
        protected virtual void GetherObjects(DragDataText data, Func<string, bool> validateFunc)
        {
            if (data.Text.StartsWith(DragPrefix))
            {
                // Remove prefix and parse splitted names
                var name = data.Text.Remove(0, DragPrefix.Length);
                if (validateFunc(name))
                {
                    Parameter = name;
                }
            }
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(string parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + parameter);
        }
    }
}
