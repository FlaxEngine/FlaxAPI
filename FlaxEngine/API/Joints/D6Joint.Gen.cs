// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.
// This code was generated by a tool. Changes to this file may cause
// incorrect behavior and will be lost if the code is regenerated.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Physics joint that is the most customizable type of joint. This joint type can be used to create all other built-in joint types, and to design your own custom ones, but is less intuitive to use. Allows a specification of a linear constraint (for example for a slider), twist constraint (rotating around X) and swing constraint (rotating around Y and Z). It also allows you to constrain limits to only specific axes or completely lock specific axes.
    /// </summary>
    [Serializable]
    public sealed partial class D6Joint : Joint
    {
        /// <summary>
        /// Creates new <see cref="D6Joint"/> object.
        /// </summary>
        private D6Joint() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="D6Joint"/> object.
        /// </summary>
        /// <returns>Created object.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static D6Joint New()
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_Create(typeof(D6Joint)) as D6Joint;
#endif
        }

        /// <summary>
        /// Gets the motion type around the specified axis.
        /// </summary>
        /// <remarks>
        /// Each axis may independently specify that the degree of freedom is locked (blocking relative movement along or around this axis), limited by the corresponding limit, or free.
        /// </remarks>
        /// <param name="axis">The axis the degree of freedom around which the motion type is specified.</param>
        /// <returns>The value.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public D6JointMotion GetMotion(D6JointAxis axis)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetMotion(unmanagedPtr, axis);
#endif
        }

        /// <summary>
        /// Sets the motion type around the specified axis.
        /// </summary>
        /// <remarks>
        /// Each axis may independently specify that the degree of freedom is locked (blocking relative movement along or around this axis), limited by the corresponding limit, or free.
        /// </remarks>
        /// <param name="axis">The axis the degree of freedom around which the motion type is specified.</param>
        /// <param name="value">The value.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void SetMotion(D6JointAxis axis, D6JointMotion value)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_SetMotion(unmanagedPtr, axis, value);
#endif
        }

        /// <summary>
        /// Gets the drive parameters for the specified drive type.
        /// </summary>
        /// <param name="index">The type of drive being specified.</param>
        /// <returns>The value.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public D6JointDrive GetDrive(D6JointDriveType index)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            D6JointDrive resultAsRef;
            Internal_GetDrive(unmanagedPtr, index, out resultAsRef);
            return resultAsRef;
#endif
        }

        /// <summary>
        /// Sets the drive parameters for the specified drive type.
        /// </summary>
        /// <param name="index">The type of drive being specified.</param>
        /// <param name="value">The value.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void SetDrive(D6JointDriveType index, D6JointDrive value)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_SetDrive(unmanagedPtr, index, ref value);
#endif
        }

        /// <summary>
        /// Determines the linear limit used for constraining translation degrees of freedom.
        /// </summary>
        [UnmanagedCall]
        [EditorOrder(200), EditorDisplay("Joint"), Tooltip("Determines the linear limit used for constraining translation degrees of freedom.")]
        public LimitLinear LimitLinear
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { LimitLinear resultAsRef; Internal_GetLimitLinear(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetLimitLinear(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Determines the angular limit used for constraining the twist (rotation around X) degree of freedom.
        /// </summary>
        [UnmanagedCall]
        [EditorOrder(210), EditorDisplay("Joint"), Tooltip("Determines the angular limit used for constraining the twist (rotation around X) degree of freedom.")]
        public LimitAngularRange LimitTwist
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { LimitAngularRange resultAsRef; Internal_GetLimitTwist(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetLimitTwist(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Determines the cone limit used for constraining the swing (rotation around Y and Z) degree of freedom.
        /// </summary>
        [UnmanagedCall]
        [EditorOrder(220), EditorDisplay("Joint"), Tooltip("Determines the cone limit used for constraining the swing (rotation around Y and Z) degree of freedom.")]
        public LimitConeRange LimitSwing
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { LimitConeRange resultAsRef; Internal_GetLimitSwing(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetLimitSwing(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Gets or sets the drive's target position relative to the joint's first body.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        public Vector3 DrivePosition
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { Vector3 resultAsRef; Internal_GetDrivePosition(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetDrivePosition(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Gets or sets the drive's target rotation relative to the joint's first body.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        public Quaternion DriveRotation
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { Quaternion resultAsRef; Internal_GetDriveRotation(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetDriveRotation(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Gets or sets the drive's target linear velocity.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        public Vector3 DriveLinearVelocity
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { Vector3 resultAsRef; Internal_GetDriveLinearVelocity(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetDriveLinearVelocity(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Gets or sets the drive's target angular velocity.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        public Vector3 DriveAngularVelocity
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { Vector3 resultAsRef; Internal_GetDriveAngularVelocity(unmanagedPtr, out resultAsRef); return resultAsRef; }
            set { Internal_SetDriveAngularVelocity(unmanagedPtr, ref value); }
#endif
        }

        /// <summary>
        /// Gets the twist angle of the joint.
        /// </summary>
        [UnmanagedCall]
        public float CurrentTwist
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { return Internal_GetCurrentTwist(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the current swing angle of the joint from the Y axis.
        /// </summary>
        [UnmanagedCall]
        public float CurrentSwingYAngle
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { return Internal_GetCurrentSwingYAngle(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the current swing angle of the joint from the Z axis.
        /// </summary>
        [UnmanagedCall]
        public float CurrentSwingZAngle
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get { return Internal_GetCurrentSwingZAngle(unmanagedPtr); }
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern D6JointMotion Internal_GetMotion(IntPtr obj, D6JointAxis axis);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMotion(IntPtr obj, D6JointAxis axis, D6JointMotion value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDrive(IntPtr obj, D6JointDriveType index, out D6JointDrive resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrive(IntPtr obj, D6JointDriveType index, ref D6JointDrive value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitLinear(IntPtr obj, out LimitLinear resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitLinear(IntPtr obj, ref LimitLinear val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitTwist(IntPtr obj, out LimitAngularRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitTwist(IntPtr obj, ref LimitAngularRange val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLimitSwing(IntPtr obj, out LimitConeRange resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLimitSwing(IntPtr obj, ref LimitConeRange val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDrivePosition(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrivePosition(IntPtr obj, ref Vector3 val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveRotation(IntPtr obj, out Quaternion resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveRotation(IntPtr obj, ref Quaternion val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveLinearVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveLinearVelocity(IntPtr obj, ref Vector3 val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDriveAngularVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDriveAngularVelocity(IntPtr obj, ref Vector3 val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentTwist(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentSwingYAngle(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCurrentSwingZAngle(IntPtr obj);
#endif

        #endregion
    }
}
