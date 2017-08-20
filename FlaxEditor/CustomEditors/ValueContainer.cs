////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Editable object values.
    /// </summary>
    public sealed class ValueContainer : List<object>
    {
        /// <summary>
        /// The values source information from reflection. Used to update values.
        /// </summary>
        public readonly MemberInfo Info;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The member info.</param>
        public ValueContainer(MemberInfo info)
        {
            Info = info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The member info.</param>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public ValueContainer(MemberInfo info, int capacity)
            : base(capacity)
        {
            Info = info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="instanceValues">The parent values.</param>
        public ValueContainer(MemberInfo info, ValueContainer instanceValues)
        {
            Info = info;
            Capacity = instanceValues.Count;

            if (Info is PropertyInfo propertyInfo)
            {
                for (int i = 0; i < instanceValues.Count; i++)
                    Add(propertyInfo.GetValue(instanceValues[i]));
            }
            else
            {
                var fieldInfo = (FieldInfo)Info;
                for (int i = 0; i < instanceValues.Count; i++)
                    Add(fieldInfo.GetValue(instanceValues[i]));
            }
        }

        /// <summary>
        /// Refreshes the specified instance values.
        /// </summary>
        /// <param name="instanceValues">The parent values.</param>
        public void Refresh(ValueContainer instanceValues)
        {
            if (instanceValues == null || instanceValues.Count != Count)
                throw new ArgumentException();

            if (Info is PropertyInfo propertyInfo)
            {
                for (int i = 0; i < Count; i++)
                    this[i] = propertyInfo.GetValue(instanceValues[i]);
            }
            else
            {
                var fieldInfo = (FieldInfo)Info;
                for (int i = 0; i < Count; i++)
                    this[i] = fieldInfo.GetValue(instanceValues[i]);
            }
        }
    }
}
