// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Physics simulation driven object.
    /// </summary>
    /// <seealso cref="PhysicsActor" />
    [Tooltip("Physics simulation driven object.")]
    public unsafe partial class RigidBody : PhysicsActor
    {
        /// <inheritdoc />
        protected RigidBody() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RigidBody"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static RigidBody New()
        {
            return Internal_Create(typeof(RigidBody)) as RigidBody;
        }

        /// <summary>
        /// Enables kinematic mode for the rigidbody.
        /// </summary>
        /// <remarks>
        /// Kinematic rigidbodies are special dynamic actors that are not influenced by forces(such as gravity), and have no momentum.
        /// They are considered to have infinite mass and can push regular dynamic actors out of the way.
        /// Kinematics will not collide with static or other kinematic objects.
        /// <para>
        /// Kinematic rigidbodies are great for moving platforms or characters, where direct motion control is desired.
        /// </para>
        /// <para>
        /// Kinematic rigidbodies are incompatible with CCD.
        /// </para>
        /// </remarks>
        [EditorOrder(10), DefaultValue(false), EditorDisplay("Rigid Body")]
        [Tooltip("Enables kinematic mode for the rigidbody.")]
        public bool IsKinematic
        {
            get { return Internal_GetIsKinematic(unmanagedPtr); }
            set { Internal_SetIsKinematic(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsKinematic(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsKinematic(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the 'drag' force added to reduce linear movement.
        /// </summary>
        /// <remarks>
        /// Linear damping can be used to slow down an object. The higher the drag the more the object slows down.
        /// </remarks>
        [EditorOrder(60), DefaultValue(0.01f), Limit(0), EditorDisplay("Rigid Body")]
        [Tooltip("The 'drag' force added to reduce linear movement.")]
        public float LinearDamping
        {
            get { return Internal_GetLinearDamping(unmanagedPtr); }
            set { Internal_SetLinearDamping(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetLinearDamping(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLinearDamping(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the 'drag' force added to reduce angular movement.
        /// </summary>
        /// <remarks>
        /// Angular damping can be used to slow down the rotation of an object. The higher the drag the more the rotation slows down.
        /// </remarks>
        [EditorOrder(70), DefaultValue(0.05f), Limit(0), EditorDisplay("Rigid Body")]
        [Tooltip("The 'drag' force added to reduce angular movement.")]
        public float AngularDamping
        {
            get { return Internal_GetAngularDamping(unmanagedPtr); }
            set { Internal_SetAngularDamping(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAngularDamping(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAngularDamping(IntPtr obj, float value);

        /// <summary>
        /// If true simulation and collisions detection will be enabled for the rigidbody.
        /// </summary>
        [EditorOrder(20), DefaultValue(true), EditorDisplay("Rigid Body")]
        [Tooltip("If true simulation and collisions detection will be enabled for the rigidbody.")]
        public bool EnableSimulation
        {
            get { return Internal_GetEnableSimulation(unmanagedPtr); }
            set { Internal_SetEnableSimulation(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnableSimulation(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnableSimulation(IntPtr obj, bool value);

        /// <summary>
        /// If true Continuous Collision Detection (CCD) will be used for this component.
        /// </summary>
        [EditorOrder(30), DefaultValue(false), EditorDisplay("Rigid Body", "Use CCD")]
        [Tooltip("If true Continuous Collision Detection (CCD) will be used for this component.")]
        public bool UseCCD
        {
            get { return Internal_GetUseCCD(unmanagedPtr); }
            set { Internal_SetUseCCD(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseCCD(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseCCD(IntPtr obj, bool value);

        /// <summary>
        /// If object should have the force of gravity applied.
        /// </summary>
        [EditorOrder(40), DefaultValue(true), EditorDisplay("Rigid Body")]
        [Tooltip("If object should have the force of gravity applied.")]
        public bool EnableGravity
        {
            get { return Internal_GetEnableGravity(unmanagedPtr); }
            set { Internal_SetEnableGravity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnableGravity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnableGravity(IntPtr obj, bool value);

        /// <summary>
        /// If object should start awake, or if it should initially be sleeping.
        /// </summary>
        [EditorOrder(50), DefaultValue(true), EditorDisplay("Rigid Body")]
        [Tooltip("If object should start awake, or if it should initially be sleeping.")]
        public bool StartAwake
        {
            get { return Internal_GetStartAwake(unmanagedPtr); }
            set { Internal_SetStartAwake(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetStartAwake(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStartAwake(IntPtr obj, bool value);

        /// <summary>
        /// If true, it will update mass when actor scale changes.
        /// </summary>
        [EditorOrder(130), DefaultValue(false), EditorDisplay("Rigid Body")]
        [Tooltip("If true, it will update mass when actor scale changes.")]
        public bool UpdateMassWhenScaleChanges
        {
            get { return Internal_GetUpdateMassWhenScaleChanges(unmanagedPtr); }
            set { Internal_SetUpdateMassWhenScaleChanges(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUpdateMassWhenScaleChanges(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateMassWhenScaleChanges(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the maximum angular velocity that a simulated object can achieve.
        /// </summary>
        /// <remarks>
        /// The angular velocity of rigidbodies is clamped to MaxAngularVelocity to avoid numerical instability with fast rotating bodies.
        /// Because this may prevent intentional fast rotations on objects such as wheels, you can override this value per rigidbody.
        /// </remarks>
        [EditorOrder(90), DefaultValue(7.0f), Limit(0), EditorDisplay("Rigid Body")]
        [Tooltip("The maximum angular velocity that a simulated object can achieve.")]
        public float MaxAngularVelocity
        {
            get { return Internal_GetMaxAngularVelocity(unmanagedPtr); }
            set { Internal_SetMaxAngularVelocity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMaxAngularVelocity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaxAngularVelocity(IntPtr obj, float value);

        /// <summary>
        /// Override the auto computed mass.
        /// </summary>
        [HideInEditor]
        [Tooltip("Override the auto computed mass.")]
        public bool OverrideMass
        {
            get { return Internal_GetOverrideMass(unmanagedPtr); }
            set { Internal_SetOverrideMass(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetOverrideMass(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOverrideMass(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the mass value measured in kilograms (use override value only if EnableOverrideMass is enabled).
        /// </summary>
        [EditorOrder(110), Limit(0), EditorDisplay("Rigid Body")]
        [Tooltip("The mass value measured in kilograms (use override value only if EnableOverrideMass is enabled).")]
        public float Mass
        {
            get { return Internal_GetMass(unmanagedPtr); }
            set { Internal_SetMass(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMass(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMass(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the per-instance scaling of the mass.
        /// </summary>
        [EditorOrder(120), DefaultValue(1.0f), Limit(0.001f, 100.0f), EditorDisplay("Rigid Body")]
        [Tooltip("The per-instance scaling of the mass.")]
        public float MassScale
        {
            get { return Internal_GetMassScale(unmanagedPtr); }
            set { Internal_SetMassScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMassScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMassScale(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the user specified offset for the center of mass of this object, from the calculated location.
        /// </summary>
        [EditorOrder(140), DefaultValue(typeof(Vector3), "0,0,0"), EditorDisplay("Rigid Body", "Center Of Mass Offset")]
        [Tooltip("The user specified offset for the center of mass of this object, from the calculated location.")]
        public Vector3 CenterOfMassOffset
        {
            get { Internal_GetCenterOfMassOffset(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetCenterOfMassOffset(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCenterOfMassOffset(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCenterOfMassOffset(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the object movement constraint flags that define degrees of freedom are allowed for the simulation of object.
        /// </summary>
        [EditorOrder(150), DefaultValue(RigidbodyConstraints.None), EditorDisplay("Rigid Body")]
        [Tooltip("The object movement constraint flags that define degrees of freedom are allowed for the simulation of object.")]
        public RigidbodyConstraints Constraints
        {
            get { return Internal_GetConstraints(unmanagedPtr); }
            set { Internal_SetConstraints(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RigidbodyConstraints Internal_GetConstraints(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetConstraints(IntPtr obj, RigidbodyConstraints value);

        /// <summary>
        /// Gets or sets the linear velocity of the rigidbody.
        /// </summary>
        /// <remarks>
        /// It's used mostly to get the current velocity. Manual modifications may result in unrealistic behaviour.
        /// </remarks>
        [HideInEditor]
        [Tooltip("The linear velocity of the rigidbody.")]
        public Vector3 LinearVelocity
        {
            get { Internal_GetLinearVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLinearVelocity(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLinearVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLinearVelocity(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the angular velocity of the rigidbody measured in radians per second.
        /// </summary>
        /// <remarks>
        /// It's used mostly to get the current angular velocity. Manual modifications may result in unrealistic behaviour.
        /// </remarks>
        [HideInEditor]
        [Tooltip("The angular velocity of the rigidbody measured in radians per second.")]
        public Vector3 AngularVelocity
        {
            get { Internal_GetAngularVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetAngularVelocity(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetAngularVelocity(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAngularVelocity(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the maximum depenetration velocity when rigidbody moving out of penetrating state.
        /// </summary>
        /// <remarks>
        /// This value controls how much velocity the solver can introduce to correct for penetrations in contacts.
        /// Using this property can smooth objects moving out of colliding state and prevent unstable motion.
        /// </remarks>
        [HideInEditor]
        [Tooltip("The maximum depenetration velocity when rigidbody moving out of penetrating state.")]
        public float MaxDepenetrationVelocity
        {
            get { return Internal_GetMaxDepenetrationVelocity(unmanagedPtr); }
            set { Internal_SetMaxDepenetrationVelocity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMaxDepenetrationVelocity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaxDepenetrationVelocity(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the mass-normalized kinetic energy threshold below which an actor may go to sleep.
        /// </summary>
        /// <remarks>
        /// Actors whose kinetic energy divided by their mass is below this threshold will be candidates for sleeping.
        /// </remarks>
        [HideInEditor]
        [Tooltip("The mass-normalized kinetic energy threshold below which an actor may go to sleep.")]
        public float SleepThreshold
        {
            get { return Internal_GetSleepThreshold(unmanagedPtr); }
            set { Internal_SetSleepThreshold(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSleepThreshold(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSleepThreshold(IntPtr obj, float value);

        /// <summary>
        /// Gets the center of the mass in the local space.
        /// </summary>
        [Tooltip("The center of the mass in the local space.")]
        public Vector3 CenterOfMass
        {
            get { Internal_GetCenterOfMass(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCenterOfMass(IntPtr obj, out Vector3 resultAsRef);

        /// <summary>
        /// Determines whether this rigidbody is sleeping.
        /// </summary>
        [Tooltip("Determines whether this rigidbody is sleeping.")]
        public bool IsSleeping
        {
            get { return Internal_IsSleeping(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsSleeping(IntPtr obj);

        /// <summary>
        /// Forces a rigidbody to sleep (for at least one frame).
        /// </summary>
        public void Sleep()
        {
            Internal_Sleep(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Sleep(IntPtr obj);

        /// <summary>
        /// Forces a rigidbody to wake up.
        /// </summary>
        public void WakeUp()
        {
            Internal_WakeUp(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_WakeUp(IntPtr obj);

        /// <summary>
        /// Updates the actor's mass (auto calculated mass from density and inertia tensor or overriden value).
        /// </summary>
        public void UpdateMass()
        {
            Internal_UpdateMass(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateMass(IntPtr obj);

        /// <summary>
        /// Applies a force (or impulse) defined in the world space to the rigidbody at its center of mass.
        /// </summary>
        /// <remarks>
        /// This will not induce a torque
        /// <para>
        /// ForceMode determines if the force is to be conventional or impulsive.
        /// </para>
        /// <para>
        /// Each actor has an acceleration and a velocity change accumulator which are directly modified using the modes ForceMode.Acceleration
        /// and ForceMode.VelocityChange respectively. The modes ForceMode.Force and ForceMode.Impulse also modify these same
        /// accumulators and are just short hand for multiplying the vector parameter by inverse mass and then using ForceMode.Acceleration and
        /// ForceMode.VelocityChange respectively.
        /// </para>
        /// </remarks>
        /// <param name="force">The force/impulse to apply defined in the world space.</param>
        /// <param name="mode">The mode to use when applying the force/impulse.</param>
        public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            Internal_AddForce(unmanagedPtr, ref force, mode);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddForce(IntPtr obj, ref Vector3 force, ForceMode mode);

        /// <summary>
        /// Applies a force (or impulse) defined in the local space of the rigidbody (relative to its coordinate system) at its center of mass.
        /// </summary>
        /// <remarks>
        /// This will not induce a torque
        /// <para>
        /// ForceMode determines if the force is to be conventional or impulsive.
        /// </para>
        /// <para>
        /// Each actor has an acceleration and a velocity change accumulator which are directly modified using the modes ForceMode.Acceleration
        /// and ForceMode.VelocityChange respectively. The modes ForceMode.Force and ForceMode.Impulse also modify these same
        /// accumulators and are just short hand for multiplying the vector parameter by inverse mass and then using ForceMode.Acceleration and
        /// ForceMode.VelocityChange respectively.
        /// </para>
        /// </remarks>
        /// <param name="force">The force/impulse to apply defined in the local space.</param>
        /// <param name="mode">The mode to use when applying the force/impulse.</param>
        public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            Internal_AddRelativeForce(unmanagedPtr, ref force, mode);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddRelativeForce(IntPtr obj, ref Vector3 force, ForceMode mode);

        /// <summary>
        /// Applies an impulsive torque defined in the world space to the rigidbody.
        /// </summary>
        /// <remarks>
        /// ForceMode determines if the force is to be conventional or impulsive.
        /// <para>
        /// Each actor has an angular acceleration and an angular velocity change accumulator which are directly modified using the modes
        /// ForceMode.Acceleration and ForceMode.VelocityChange respectively.The modes ForceMode.Force and ForceMode.Impulse
        /// also modify these same accumulators and are just short hand for multiplying the vector parameter by inverse inertia and then
        /// using ForceMode.Acceleration and ForceMode.VelocityChange respectively.
        /// </para>
        /// </remarks>
        /// <param name="torque">The torque to apply defined in the world space.</param>
        /// <param name="mode">The mode to use when applying the force/impulse.</param>
        public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
        {
            Internal_AddTorque(unmanagedPtr, ref torque, mode);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddTorque(IntPtr obj, ref Vector3 torque, ForceMode mode);

        /// <summary>
        /// Applies an impulsive torque defined in the local space of the rigidbody (relative to its coordinate system).
        /// </summary>
        /// <remarks>
        /// ForceMode determines if the force is to be conventional or impulsive.
        /// <para>
        /// Each actor has an angular acceleration and an angular velocity change accumulator which are directly modified using the modes
        /// ForceMode.Acceleration and ForceMode.VelocityChange respectively.The modes ForceMode.Force and ForceMode.Impulse
        /// also modify these same accumulators and are just short hand for multiplying the vector parameter by inverse inertia and then
        /// using ForceMode.Acceleration and ForceMode.VelocityChange respectively.
        /// </para>
        /// </remarks>
        /// <param name="torque">The torque to apply defined in the local space.</param>
        /// <param name="mode">The mode to use when applying the force/impulse.</param>
        public void AddRelativeTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
        {
            Internal_AddRelativeTorque(unmanagedPtr, ref torque, mode);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddRelativeTorque(IntPtr obj, ref Vector3 torque, ForceMode mode);

        /// <summary>
        /// Sets the solver iteration counts for the rigidbody.
        /// </summary>
        /// <remarks>
        /// The solver iteration count determines how accurately joints and contacts are resolved.
        /// If you are having trouble with jointed bodies oscillating and behaving erratically,
        /// then setting a higher position iteration count may improve their stability.
        /// <para>
        /// If intersecting bodies are being depenetrated too violently, increase the number of velocity
        /// iterations. More velocity iterations will drive the relative exit velocity of the intersecting
        /// objects closer to the correct value given the restitution.
        /// </para>
        /// <para>
        /// Default: 4 position iterations, 1 velocity iteration.
        /// </para>
        /// </remarks>
        /// <param name="minPositionIters">The minimum number of position iterations the solver should perform for this body.</param>
        /// <param name="minVelocityIters">The minimum number of velocity iterations the solver should perform for this body.</param>
        public void SetSolverIterationCounts(int minPositionIters, int minVelocityIters)
        {
            Internal_SetSolverIterationCounts(unmanagedPtr, minPositionIters, minVelocityIters);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSolverIterationCounts(IntPtr obj, int minPositionIters, int minVelocityIters);

        /// <summary>
        /// Gets a point on one of the colliders attached to the attached that is closest to a given location. Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <param name="result">The result point on the rigidbody shape that is closest to the specified location.</param>
        public void ClosestPoint(Vector3 position, out Vector3 result)
        {
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);
    }
}
