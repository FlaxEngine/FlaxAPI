using System;

namespace FlaxEngine
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public sealed class CustomClassEditorAttribute : Attribute 
    {
        /// <summary>
        /// Custom editor targeted style
        /// </summary>
        public Type TargetTargetType { get; set; }

        /// <summary>
        /// Overrides default editor visuals provided for class with provided style
        /// </summary>
        /// <seealso cref="TODO CustomClassEditor"/>
        public CustomClassEditorAttribute(Type targetType)
        {
            TargetTargetType = targetType;
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct 
        | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class CustomGlobalEditorAttribute : Attribute
    {
        /// <summary>
        /// Custom editor targeted style
        /// </summary>
        public Type TargetTargetType { get; set; }

        /// <summary>
        /// Overrides default editor visuals for class based on overriden context with provided style
        /// </summary>
        /// <seealso cref="TODO CustomGlobalEditor"/>
        public CustomGlobalEditorAttribute(Type targetType)
        {
            TargetTargetType = targetType;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class CustomModelEditorAttribute : Attribute
    {
        /// <summary>
        /// Custom editor targeted style
        /// </summary>
        public Type TargetTargetType { get; set; }

        /// <summary>
        /// Overrides default editor visuals for model with provided style
        /// </summary>
        /// <seealso cref="TODO CustomModelEditor"/>
        public CustomModelEditorAttribute(Type targetType)
        {
            TargetTargetType = targetType;
        }
    }
}
