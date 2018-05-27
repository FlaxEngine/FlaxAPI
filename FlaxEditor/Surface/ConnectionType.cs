// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Graph boxes connection type.
    /// </summary>
    [Flags]
    public enum ConnectionType : uint
    {
        // Note: this must match C++ side!

        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Digital signal
        /// </summary>
        Impulse = 1 << 0,

        /// <summary>
        /// Boolean value, true or false
        /// </summary>
        Bool = 1 << 1,

        /// <summary>
        /// Integer value
        /// </summary>
        Integer = 1 << 2,

        /// <summary>
        /// Floating point value
        /// </summary>
        Float = 1 << 3,

        /// <summary>
        /// Vector2 - 2 floating point values
        /// </summary>
        Vector2 = 1 << 4,

        /// <summary>
        /// Vector3 - 3 floating point values
        /// </summary>
        Vector3 = 1 << 5,

        /// <summary>
        /// Vector4 - 4 floating point values
        /// </summary>
        Vector4 = 1 << 6,

        /// <summary>
        /// String
        /// </summary>
        String = 1 << 7,

        /// <summary>
        /// Any type of object like: actor, actor element etc.
        /// </summary>
        Object = 1 << 8,

        /// <summary>
        /// 3D rotation transform based on 4 component quaternion
        /// </summary>
        Rotation = 1 << 9,

        /// <summary>
        /// Full 3D transform: position, rotation and scale
        /// </summary>
        Transform = 1 << 10,

        /// <summary>
        /// Axis aligned bounding box(two Vector3s)
        /// </summary>
        Box = 1 << 11,

        /// <summary>
        /// Digital signal (secondary)
        /// </summary>
        ImpulseSecondary = 1 << 12,

        /// <summary>
        /// Bool, Int, Float
        /// </summary>
        Scalar = Bool | Integer | Float,

        /// <summary>
        /// Vector2, Vector3, Vector4
        /// </summary>
        Vector = Vector2 | Vector3 | Vector4,

        /// <summary>
        /// All variables
        /// </summary>
        Variable = Scalar | Vector | String | Object | Rotation | Transform | Box,

        /// <summary>
        /// All possible connections types
        /// </summary>
        All = Variable | Impulse,
    }
}
