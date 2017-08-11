////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Indicates that a field or a property of a serializable class should not be serialized. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public sealed class NoSerializeAttribute : Attribute
    {
    }
}
