////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Overrides default editor provided for the target object/class/field/property. Allows to extend visuals and editing experaince of the objects.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class CustomEditorAttribute : Attribute
    {
        /// <summary>
        /// Custom editor class type.
        /// Note: if attribute is used on CustomEditor class it specifies object type to edit.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// If set to true custom editor contents will be inlined into the property value, otherwise will use expandable group area.
        /// </summary>
        public readonly bool IsInline;

        /// <summary>
        /// Overrides default editor provided for the target object.
        /// </summary>
        /// <param name="type">The custom editor class type.</param>
        /// <param name="isInline">True if use inline style for the editor layout.</param>
        public CustomEditorAttribute(Type type, bool isInline = false)
        {
            Type = type;
            IsInline = isInline;
        }
    }
}
