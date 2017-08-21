////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlaxEditor.CustomEditors.Editors;

namespace FlaxEditor.CustomEditors
{
    internal static class CustomEditorsUtil
    {
        internal static CustomEditor CreateEditor(MemberInfo member)
        {
            // Select target value type
            Type targetType;
            if (member is PropertyInfo propertyInfo)
            {
                targetType = propertyInfo.PropertyType;
            }
            else if (member is FieldInfo fieldInfo)
            {
                targetType = fieldInfo.FieldType;
            }
            else
            {
                throw new NotImplementedException();
            }

            // Use custom editor
            var type = Internal_GetCustomEditor(targetType);
            if (type != null)
                return (CustomEditor)Activator.CreateInstance(type);

            // Select default editor (based on type)
            if (targetType.IsArray)
            {
                // TODO: dedicated array editor
                return new GenericEditor();
            }
            if (targetType.IsEnum)
            {
                return new EnumEditor();
            }
            if (targetType.IsGenericType)
            {
                if (targetType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // TODO: dedicated List<T> editor
                    return new GenericEditor();
                }
                if (targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    // TODO: dedicated Dictionary<T> editor
                    return new GenericEditor();
                }
            }
            if (targetType.IsSubclassOf(typeof(FlaxEngine.Object)))
            {
                return new FlaxObjectRefEditor();
            }

            // The most generic editor
            return new GenericEditor();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Type Internal_GetCustomEditor(Type targetType);
    }
}
