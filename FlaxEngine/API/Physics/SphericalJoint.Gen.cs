// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Flags that control spherical joint options.
    /// </summary>
    [Flags]
    [Tooltip("Flags that control spherical joint options.")]
    public enum SphericalJointFlag
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// The joint cone range limit is enabled.
        /// </summary>
        [Tooltip("The joint cone range limit is enabled.")]
        Limit = 0x1,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that removes all translational degrees of freedom but allows all rotational degrees of freedom.
    /// Essentially this ensures that the anchor points of the two bodies are always coincident. Bodies are allowed to
    /// rotate around the anchor points, and their rotation can be limited by an elliptical cone.
    /// </summary>
    /// <seealso cref="Joint" />
    public unsafe partial class SphericalJoint : Joint
    {
        /// <inheritdoc />
        protected SphericalJoint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SphericalJoint"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SphericalJoint New()
        {
            return Internal_Create(typeof(SphericalJoint)) as SphericalJoint;
        }

        /// <summary>
        /// Gets or sets the joint mode flags. Controls joint behaviour.
        /// </summary>
        [EditorOrder(100), DefaultValue(SphericalJointFlag.Limit), EditorDisplay("Joint")]
        [Tooltip("The joint mode flags. Controls joint behaviour.")]
        public SphericalJointFlag Flags
        {
            get { return Internal_GetFlags(unmanagedPtr); }
            set { Internal_SetFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SphericalJointFlag Internal_GetFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFlags(IntPtr obj, SphericalJointFlag value);

        /// <summary>
        /// Gets or sets the joint limit properties.
        /// </summary>
        /// <remarks>
        /// Determines the limit of the joint. Limit constrains the motion to the specified angle range. You must enable the limit flag on the joint in order for this to be recognized.
        /// </remarks>
        [EditorOrder(110), EditorDisplay("Joint")]
        [Tooltip("The joint limit properties.")]
        public LimitConeRange Limit
        {
            get { Internal_GetLimit(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLimit(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimit(IntPtr obj, out LimitConeRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimit(IntPtr obj, ref LimitConeRange value);
    }
}
