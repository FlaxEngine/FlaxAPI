// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Enumeration that determines the way in which two material properties will be combined to yield a friction or restitution coefficient for a collision.
    /// </summary>
    /// <remarks>
    /// Physics doesn't have any inherent combinations because the coefficients are determined empirically on a case by case basis.
    /// However, simulating this with a pairwise lookup table is often impractical.
    /// The effective combine mode for the pair is maximum(material0.combineMode, material1.combineMode).
    /// </remarks>
    [Tooltip("Enumeration that determines the way in which two material properties will be combined to yield a friction or restitution coefficient for a collision.")]
    public enum PhysicsCombineMode
    {
        /// <summary>
        /// Uses the average value of the touching materials: (a+b)/2.
        /// </summary>
        [Tooltip("Uses the average value of the touching materials: (a+b)/2.")]
        Average = 0,

        /// <summary>
        /// Uses the smaller value of the touching materials: min(a,b)
        /// </summary>
        [Tooltip("Uses the smaller value of the touching materials: min(a,b)")]
        Minimum = 1,

        /// <summary>
        /// Multiplies the values of the touching materials: a*b
        /// </summary>
        [Tooltip("Multiplies the values of the touching materials: a*b")]
        Multiply = 2,

        /// <summary>
        /// Uses the larger value of the touching materials: max(a, b)
        /// </summary>
        [Tooltip("Uses the larger value of the touching materials: max(a, b)")]
        Maximum = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Force mode type determines the exact operation that is carried out when applying the force on a rigidbody.
    /// </summary>
    [Tooltip("Force mode type determines the exact operation that is carried out when applying the force on a rigidbody.")]
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
        Acceleration,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Dynamic rigidbodies movement and rotation locking flags. Provide a mechanism to lock motion along/around a specific axis or set of axes to constrain object motion.
    /// </summary>
    [Flags]
    [Tooltip("Dynamic rigidbodies movement and rotation locking flags. Provide a mechanism to lock motion along/around a specific axis or set of axes to constrain object motion.")]
    public enum RigidbodyConstraints
    {
        /// <summary>
        /// No constraints.
        /// </summary>
        [Tooltip("No constraints.")]
        None = 0,

        /// <summary>
        /// Freeze motion along the X-axis.
        /// </summary>
        [Tooltip("Freeze motion along the X-axis.")]
        LockPositionX = (1 << 0),

        /// <summary>
        /// Freeze motion along the Y-axis.
        /// </summary>
        [Tooltip("Freeze motion along the Y-axis.")]
        LockPositionY = (1 << 1),

        /// <summary>
        /// Freeze motion along the Z-axis.
        /// </summary>
        [Tooltip("Freeze motion along the Z-axis.")]
        LockPositionZ = (1 << 2),

        /// <summary>
        /// Freeze rotation along the X-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the X-axis.")]
        LockRotationX = (1 << 3),

        /// <summary>
        /// Freeze rotation along the Y-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the Y-axis.")]
        LockRotationY = (1 << 4),

        /// <summary>
        /// Freeze rotation along the Z-axis.
        /// </summary>
        [Tooltip("Freeze rotation along the Z-axis.")]
        LockRotationZ = (1 << 5),

        /// <summary>
        /// Freeze motion along all axes.
        /// </summary>
        [Tooltip("Freeze motion along all axes.")]
        LockPosition = LockPositionX | LockPositionY | LockPositionZ,

        /// <summary>
        /// Freeze rotation along all axes.
        /// </summary>
        [Tooltip("Freeze rotation along all axes.")]
        LockRotation = LockRotationX | LockRotationY | LockRotationZ,

        /// <summary>
        /// Freeze rotation and motion along all axes.
        /// </summary>
        [Tooltip("Freeze rotation and motion along all axes.")]
        LockAll = LockPosition | LockRotation,
    }
}
