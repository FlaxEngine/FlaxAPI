using System;

namespace CelelejEngine
{
    /// <summary>
    /// Makes a variable not show up in the inspector but be serialized.
    /// </summary>
    public sealed class HideInInspector : Attribute
    {
    }
}
