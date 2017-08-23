////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FlaxEditor.CustomEditors.Editors;

namespace FlaxEditor.CustomEditors
{
    internal static class CustomEditorsUtil
    {
        /// <summary>
        /// Gets the property name for UI. Removes unnecessary characters and filters text. Makes it more user-friendly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The result.</returns>
        public static string GetPropertyNameUI(string name)
        {
            int length = name.Length;
            StringBuilder sb = new StringBuilder(length + 4);

            for (int i = 0; i < length; i++)
            {
                var c = name[i];

                if (char.IsUpper(c) && i > 0)
                {
                    if (i + 2 < length && !char.IsUpper(name[i + 1]) && !char.IsUpper(name[i + 2]))
                        sb.Append(' ');
                }
                else if (c == '_')
                {
                    if (sb.Length > 0)
                        sb.Append(' ');
                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        internal static CustomEditor CreateEditor(Type targetType)
        {
            if (targetType.IsArray)
            {
                return new ArrayEditor();
            }

            // Use custom editor
            var type = Internal_GetCustomEditor(targetType);
            if (type != null)
                return (CustomEditor)Activator.CreateInstance(type);

            // Select default editor (based on type)
            if (targetType.IsEnum)
            {
                return new EnumEditor();
            }
            if (targetType.IsGenericType)
            {
                if (targetType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return new ListEditor();
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
