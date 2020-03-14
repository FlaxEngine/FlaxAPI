// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Force mode type determines the exact operation that is carried out when applying the force on a rigidbody.
    /// </summary>
    public enum ForceMode
    {
        /// <summary>
        /// Add a continuous force to the rigidbody, using its mass. The parameter has unit of mass * distance / time^2, i.e. a force.
        /// </summary>
        [Tooltip("Add a continuous force to the rigidbody, using its mass. The parameter has unit of mass * distance / time^2, i.e. a force.")]
        Force,

        /// <summary>
        /// Add an instant force impulse to the rigidbody, using its mass. The parameter has unit of mass * distance / time.
        /// </summary>
        [Tooltip("Add an instant force impulse to the rigidbody, using its mass. The parameter has unit of mass * distance / time.")]
        Impulse,

        /// <summary>
        /// Add an instant velocity change to the rigidbody, ignoring its mass. The parameter has unit of distance / time, i.e. the effect is mass independent: a velocity change.
        /// </summary>
        [Tooltip("Add an instant velocity change to the rigidbody, ignoring its mass. The parameter has unit of distance / time, i.e. the effect is mass independent: a velocity change.")]
        VelocityChange,

        /// <summary>
        /// Add a continuous acceleration to the rigidbody, ignoring its mass. The parameter has unit of distance / time^2, i.e. an acceleration. It gets treated just like a force except the mass is not divided out before integration.
        /// </summary>
        [Tooltip("Add a continuous acceleration to the rigidbody, ignoring its mass. The parameter has unit of distance / time^2, i.e. an acceleration. It gets treated just like a force except the mass is not divided out before integration.")]
        Acceleration
    }
}
