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
        /// Gets the values type.
        /// </summary>
        /// <value>
        /// The values type.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The member info.</param>
        public ValueContainer(MemberInfo info)
        {
            Info = info;

            if (Info is PropertyInfo propertyInfo)
            {
                Type = propertyInfo.PropertyType;
            }
            else if (Info is FieldInfo fieldInfo)
            {
                Type = fieldInfo.FieldType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="instanceValues">The parent values.</param>
        public ValueContainer(MemberInfo info, ValueContainer instanceValues)
            : this(info)
        {
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

        /// <summary>
        /// Sets the specified instance values. Refreshes this values container.
        /// </summary>
        /// <param name="instanceValues">The parent values.</param>
        /// <param name="value">The value.</param>
        public void Set(ValueContainer instanceValues, object value)
        {
            if (instanceValues == null || instanceValues.Count != Count)
                throw new ArgumentException();

            if (Info is PropertyInfo propertyInfo)
            {
                for (int i = 0; i < Count; i++)
                {
                    propertyInfo.SetValue(instanceValues[i], value);
                    this[i] = propertyInfo.GetValue(instanceValues[i]);
                }
            }
            else
            {
                var fieldInfo = (FieldInfo)Info;
                for (int i = 0; i < Count; i++)
                {
                    fieldInfo.SetValue(instanceValues[i], value);
                    this[i] = fieldInfo.GetValue(instanceValues[i]);
                }
            }
        }

        /// <summary>
        /// Sets the specified instance values with the container values.
        /// </summary>
        /// <param name="instanceValues">The parent values.</param>
        public void Set(ValueContainer instanceValues)
        {
            if (instanceValues == null || instanceValues.Count != Count)
                throw new ArgumentException();

            if (Info is PropertyInfo propertyInfo)
            {
                for (int i = 0; i < Count; i++)
                {
                    propertyInfo.SetValue(instanceValues[i], this[i]);
                }
            }
            else
            {
                var fieldInfo = (FieldInfo)Info;
                for (int i = 0; i < Count; i++)
                {
                    fieldInfo.SetValue(instanceValues[i], this[i]);
                }
            }
        }
    }
}
