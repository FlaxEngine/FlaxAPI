// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Editable object values.
    /// </summary>
    public class ValueContainer : List<object>
    {
        /// <summary>
        /// The has default value flag. Set if <see cref="_defaultValue"/> is valid and assigned.
        /// </summary>
        protected bool _hasDefaultValue;

        /// <summary>
        /// The default value used to show difference in the UI compared to the default object values. Used to revert modified properties.
        /// </summary>
        protected object _defaultValue;

        /// <summary>
        /// The has reference value flag. Set if <see cref="_referenceValue"/> is valid and assigned.
        /// </summary>
        protected bool _hasReferenceValue;

        /// <summary>
        /// The reference value used to show difference in the UI compared to the other object. Used by the prefabs system.
        /// </summary>
        protected object _referenceValue;

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
        /// Gets a value indicating whether selected objects are different values.
        /// </summary>
        public bool HasDifferentValues
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
        /// Gets a value indicating whether selected objects are different types.
        /// </summary>
        public bool HasDifferentTypes
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
        /// Gets a value indicating whether any value in the collection is null.
        /// </summary>
        public bool HasNull
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] == null)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this values container type is array.
        /// </summary>
        public bool IsArray => Type != null && Type.IsArray;

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
        /// Gets a value indicating whether this instance has reference value assigned (see <see cref="ReferenceValue"/>).
        /// </summary>
        public bool HasReferenceValue => _hasReferenceValue;

        /// <summary>
        /// Gets the reference value used to show difference in the UI compared to the other object. Used by the prefabs system.
        /// </summary>
        public object ReferenceValue => _referenceValue;

        /// <summary>
        /// Gets a value indicating whether this instance has reference value and the any of the values in the contains is modified (compared to the reference).
        /// </summary>
        /// <remarks>
        /// For prefabs system it means that object property has been modified compared to the prefab value.
        /// </remarks>
        public bool IsReferenceValueModified
        {
            get
            {
                if (_hasReferenceValue)
                {
                    if (_referenceValue is ISceneObject referenceSceneObject && referenceSceneObject.HasPrefabLink)
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            if (this[i] == referenceSceneObject)
                                continue;

                            if (this[i] == null || (this[i] is ISceneObject valueSceneObject && valueSceneObject.PrefabObjectID != referenceSceneObject.PrefabObjectID))
                                return true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            if (!Equals(this[i], _referenceValue))
                                return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has default value assigned (see <see cref="DefaultValue"/>).
        /// </summary>
        public bool HasDefaultValue => _hasDefaultValue;

        /// <summary>
        /// Gets the default value used to show difference in the UI compared to the default value object. Used to revert modified properties.
        /// </summary>
        public object DefaultValue => _defaultValue;

        /// <summary>
        /// Gets a value indicating whether this instance has default value and the any of the values in the contains is modified (compared to the reference).
        /// </summary>
        public bool IsDefaultValueModified
        {
            get
            {
                if (_hasDefaultValue)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (!Equals(this[i], _defaultValue))
                            return true;
                    }
                }
                return false;
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
                {
                    Add(propertyInfo.GetValue(instanceValues[i]));
                }

                if (instanceValues._hasDefaultValue)
                {
                    _defaultValue = propertyInfo.GetValue(instanceValues._defaultValue);
                    _hasDefaultValue = true;
                }
                else
                {
                    var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null)
                    {
                        _defaultValue = defaultValueAttribute.Value;
                        _hasDefaultValue = true;
                    }
                }
                if (instanceValues._hasReferenceValue)
                {
                    _referenceValue = propertyInfo.GetValue(instanceValues._referenceValue);
                    _hasReferenceValue = true;
                }
            }
            else
            {
                var fieldInfo = (FieldInfo)Info;
                for (int i = 0; i < instanceValues.Count; i++)
                {
                    Add(fieldInfo.GetValue(instanceValues[i]));
                }

                if (instanceValues._hasDefaultValue)
                {
                    _defaultValue = fieldInfo.GetValue(instanceValues._defaultValue);
                    _hasDefaultValue = true;
                }
                else
                {
                    var defaultValueAttribute = fieldInfo.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null)
                    {
                        _defaultValue = defaultValueAttribute.Value;
                        _hasDefaultValue = true;
                    }
                }
                if (instanceValues._hasReferenceValue)
                {
                    _referenceValue = fieldInfo.GetValue(instanceValues._referenceValue);
                    _hasReferenceValue = true;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer"/> class.
        /// </summary>
        /// <param name="customType">The target custom type of the container values. Used to override the data.</param>
        /// <param name="other">The other values container to clone.</param>
        public ValueContainer(Type customType, ValueContainer other)
        {
            Capacity = other.Capacity;
            AddRange(other);
            Info = other.Info;
            Type = customType;
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
        /// Gets the custom attributes defined for the values source member.
        /// </summary>
        /// <returns>The attributes objects array.</returns>
        public virtual object[] GetAttributes()
        {
            return Info?.GetCustomAttributes(true);
        }

        /// <summary>
        /// Refreshes the specified instance values.
        /// </summary>
        /// <param name="instanceValues">The parent values.</param>
        public virtual void Refresh(ValueContainer instanceValues)
        {
            if (instanceValues == null)
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

        /// <summary>
        /// Sets the default value of the container.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void SetDefaultValue(object value)
        {
            _defaultValue = value;
            _hasDefaultValue = true;
        }

        /// <summary>
        /// Refreshes the default value of the container.
        /// </summary>
        /// <param name="instanceValue">The parent value.</param>
        public virtual void RefreshDefaultValue(object instanceValue)
        {
            if (Info is PropertyInfo propertyInfo)
            {
                _defaultValue = propertyInfo.GetValue(instanceValue);
            }
            else
            {
                _defaultValue = ((FieldInfo)Info).GetValue(instanceValue);
            }

            _hasDefaultValue = true;
        }

        /// <summary>
        /// Clears the default value of the container.
        /// </summary>
        public virtual void ClearDefaultValue()
        {
            _defaultValue = null;
            _hasDefaultValue = false;
        }

        /// <summary>
        /// Sets the reference value of the container.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void SetReferenceValue(object value)
        {
            _referenceValue = value;
            _hasReferenceValue = true;
        }

        /// <summary>
        /// Refreshes the reference value of the container.
        /// </summary>
        /// <param name="instanceValue">The parent value.</param>
        public virtual void RefreshReferenceValue(object instanceValue)
        {
            if (Info is PropertyInfo propertyInfo)
            {
                _referenceValue = propertyInfo.GetValue(instanceValue);
            }
            else
            {
                _referenceValue = ((FieldInfo)Info).GetValue(instanceValue);
            }

            _hasReferenceValue = true;
        }

        /// <summary>
        /// Clears the reference value of the container.
        /// </summary>
        public virtual void ClearReferenceValue()
        {
            _referenceValue = null;
            _hasReferenceValue = false;
        }
    }
}
