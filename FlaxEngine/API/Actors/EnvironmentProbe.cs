// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class EnvironmentProbe
    {
        /// <summary>
        /// Gets a value indicating whether this instance has probe texture assigned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has probe; otherwise, <c>false</c>.
        /// </value>
        public bool HasProbe => Probe != null;

        /// <summary>
        /// Returns true if probe is using custom cube texture (not baked one).
        /// </summary>
        public bool IsUsingCustomProbe => CustomProbe != null;

        /// <summary>
        /// Gets the probe scaled radius parameter.
        /// </summary>
        public float ScaledRadius => Scale.MaxValue * Radius;
    }
}
