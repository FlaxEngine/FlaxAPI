////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Editable object values.
    /// </summary>
    public class ValueContainer : List<object>
    {
        /// <summary>
        /// The values source information from reflection. Used to update values.
        /// </summary>
        public readonly MemberInfo Info;

        /// <summary>
        /// Gets the values type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets a value indicating whether single object is selected.
        /// </summary>
        public bool IsSingleObject => Count == 1;

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent values.
        /// </summary>
        public bool HasDiffrentValues
        {
            get
            {
                for (int i = 1; i < Count; i++)
                {
                    if (!Equals(this[0], this[i]))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent types.
        /// </summary>
        public bool HasDiffrentTypes
        {
            get
            {
                if (Count < 2)
                    return false;
                var theFirstType = this[0]?.GetType();
                for (int i = 1; i < Count; i++)
                {
                    if (theFirstType != this[1]?.GetType())
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the values types array (without duplicates).
        /// </summary>
        public Type[] ValuesTypes
        {
            get
            {
                if (Count == 1)
                    return new[] { this[0].GetType() };
                return ConvertAll(x => x.GetType()).Distinct().ToArray();
            }
        }

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
        /// <param name="info">The member info.</param>
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
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="info">The member info.</param>
        /// <param name="type">The type.</param>
        protected ValueContainer(MemberInfo info, Type type)
        {
            Info = info;
            Type = type;
        }

        /// <summary>
        /// Refreshes the specified instance values.
        /// </summary>
        /// <param name="instanceValues">The parent values.</param>
        public virtual void Refresh(ValueContainer instanceValues)
        {
            if(instanceValues == null)
                throw new ArgumentNullException();
            if (instanceValues.Count != Count)
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
        public virtual void Set(ValueContainer instanceValues, object value)
        {
            if (instanceValues == null)
                throw new ArgumentNullException();
            if (instanceValues.Count != Count)
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
        public virtual void Set(ValueContainer instanceValues)
        {
            if (instanceValues == null)
                throw new ArgumentNullException();
            if (instanceValues.Count != Count)
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
