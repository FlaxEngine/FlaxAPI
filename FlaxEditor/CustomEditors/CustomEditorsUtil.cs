////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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

            // Special case for array
            if (targetType.IsArray)
            {
                // TODO: dedicated array editor
                return new GenericEditor();
            }

            // Use custom editor
            var type = Internal_GetCustomEditor(targetType);
            if (type != null)
                return (CustomEditor)Activator.CreateInstance(type);

            // Use generic one
            return new GenericEditor();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Type Internal_GetCustomEditor(Type targetType);
    }
}
