////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit lists.
    /// </summary>
    [CustomEditor(typeof(List<>)), DefaultEditor]
    public sealed class ListEditor : CollectionEditor
    {
        /// <inheritdoc />
        public override int Count => (Values[0] as IList)?.Count ?? 0;

        /// <inheritdoc />
        protected override void Resize(int newSize)
        {
            var list = Values[0] as IList;
            var oldSize = list?.Count ?? 0;

            if (oldSize != newSize)
            {
                // Allocate new list
                var listType = Values.Type;
                var newValues = (IList)Activator.CreateInstance(listType);

                var sharedCount = Mathf.Min(oldSize, newSize);
                if (list != null && sharedCount > 0)
                {
                    // Copy old values
                    for (int i = 0; i < sharedCount; i++)
                    {
                        newValues.Add(list[i]);
                    }

                    // Fill new entries with the last value
                    for (int i = oldSize; i < newSize; i++)
                    {
                        newValues.Add(list[oldSize - 1]);
                    }
                }

                SetValue(newValues);
            }
        }
    }
}
