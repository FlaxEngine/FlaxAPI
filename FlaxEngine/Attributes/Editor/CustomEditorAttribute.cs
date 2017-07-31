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
        /// </summary>
        public readonly Type Type;
        
        /// <summary>
        /// Overrides default editor provided for the target object.
        /// </summary>
        /// <param name="type">The custom editor class type.</param>
        public CustomEditorAttribute(Type type)
        {
            Type = type;
        }
    }
}
