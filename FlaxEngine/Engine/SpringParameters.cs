// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Controls spring parameters for a physics joint limits. If a limit is soft (body bounces back due to restitution when 
    /// the limit is reached) the spring will pull the body back towards the limit using the specified parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpringParameters
    {
        /// <summary>
        /// The spring strength. Force proportional to the position error.
        /// </summary>
        [EditorOrder(0), Limit(0.0f), Tooltip("The spring strength. Force proportional to the position error.")]
        public float Stiffness;

        /// <summary>
        /// Damping strength. Force proportional to the velocity error.
        /// </summary>
        [EditorOrder(10), Limit(0.0f), Tooltip("Damping strength. Force proportional to the velocity error.")]
        public float Damping;

        /// <summary>
        /// The default <see cref="SpringParameters"/> structure.
        /// </summary>
        public static readonly SpringParameters Default = new SpringParameters(0.0f, 0.0f);

        /// <summary>
        /// Constructs a spring.
        /// </summary>
        /// <param name="stiffness">Spring strength. Force proportional to the position error.</param>
        /// <param name="damping">Damping strength. Force proportional to the velocity error.</param>
        public SpringParameters(float stiffness, float damping)
        {
            Stiffness = stiffness;
            Damping = damping;
        }
    }
}
