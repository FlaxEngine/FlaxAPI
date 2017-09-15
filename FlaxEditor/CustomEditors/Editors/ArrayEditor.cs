////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit arrays.
    /// </summary>
    [CustomEditor(typeof(Array)), DefaultEditor]
    public class ArrayEditor : CollectionEditor
    {
        /// <inheritdoc />
        public override int Count => (Values[0] as Array)?.Length ?? 0;

        /// <inheritdoc />
        protected override void Resize(int newSize)
        {
            var array = Values[0] as Array;
            var oldSize = array?.Length ?? 0;

            if (oldSize != newSize)
            {
                // Allocate new array
                var arrayType = Values.Type;
                var elementType = arrayType.GetElementType();
                var newValues = Array.CreateInstance(elementType, newSize);

                var sharedCount = Mathf.Min(oldSize, newSize);
                if (array != null && sharedCount > 0)
                {
                    // Copy old values
                    Array.Copy(array, 0, newValues, 0, sharedCount);

                    // Fill new entries with the last value
                    for (int i = oldSize; i < newSize; i++)
                    {
                        Array.Copy(array, oldSize - 1, newValues, i, 1);
                    }
                }

                SetValue(newValues);
            }
        }
    }
}
