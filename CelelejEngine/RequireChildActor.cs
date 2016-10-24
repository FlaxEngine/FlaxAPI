// Celelej Game Engine scripting API

using System;

namespace CelelejEngine
{
    /// <summary>
    /// The RequireChildActor attribute automatically adds required child actor as dependencies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireChildActor : Attribute
    {
        public Type Type;

        /// <summary>
        /// Require an child.
        /// </summary>
        /// <param name="requiredChild"></param>
        public RequireChildActor(Type requiredChild)
        {
            Type = requiredChild;
        }
    }
}
