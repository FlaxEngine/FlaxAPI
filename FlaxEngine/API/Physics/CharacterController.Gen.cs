// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Physical objects that allows to easily do player movement constrained by collisions without having to deal with a rigidbody.
    /// </summary>
    /// <seealso cref="Collider" />
    [Tooltip("Physical objects that allows to easily do player movement constrained by collisions without having to deal with a rigidbody.")]
    public unsafe partial class CharacterController : Collider
    {
        /// <inheritdoc />
        protected CharacterController() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="CharacterController"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static CharacterController New()
        {
            return Internal_Create(typeof(CharacterController)) as CharacterController;
        }

        /// <summary>
        /// Gets or sets the radius of the sphere, measured in the object's local space. The sphere radius will be scaled by the actor's world scale.
        /// </summary>
        [EditorOrder(100), DefaultValue(50.0f), EditorDisplay("Collider")]
        [Tooltip("The radius of the sphere, measured in the object's local space. The sphere radius will be scaled by the actor's world scale.")]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRadius(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the height of the capsule, measured in the object's local space. The capsule height will be scaled by the actor's world scale.
        /// </summary>
        [EditorOrder(110), DefaultValue(150.0f), EditorDisplay("Collider")]
        [Tooltip("The height of the capsule, measured in the object's local space. The capsule height will be scaled by the actor's world scale.")]
        public float Height
        {
            get { return Internal_GetHeight(unmanagedPtr); }
            set { Internal_SetHeight(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetHeight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetHeight(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the slope limit (in degrees). Limits the collider to only climb slopes that are less steep (in degrees) than the indicated value.
        /// </summary>
        [EditorOrder(210), DefaultValue(45.0f), Limit(0, 100), EditorDisplay("Character Controller")]
        [Tooltip("The slope limit (in degrees). Limits the collider to only climb slopes that are less steep (in degrees) than the indicated value.")]
        public float SlopeLimit
        {
            get { return Internal_GetSlopeLimit(unmanagedPtr); }
            set { Internal_SetSlopeLimit(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSlopeLimit(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSlopeLimit(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the non-walkable mode for the character controller.
        /// </summary>
        [EditorOrder(215), DefaultValue(NonWalkableModes.PreventClimbing), EditorDisplay("Character Controller")]
        [Tooltip("The non-walkable mode for the character controller.")]
        public NonWalkableModes NonWalkableMode
        {
            get { return Internal_GetNonWalkableMode(unmanagedPtr); }
            set { Internal_SetNonWalkableMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern NonWalkableModes Internal_GetNonWalkableMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetNonWalkableMode(IntPtr obj, NonWalkableModes value);

        /// <summary>
        /// Gets or sets the step height. The character will step up a stair only if it is closer to the ground than the indicated value. This should not be greater than the Character Controller’s height or it will generate an error.
        /// </summary>
        [EditorOrder(220), DefaultValue(30.0f), Limit(0), EditorDisplay("Character Controller")]
        [Tooltip("The step height. The character will step up a stair only if it is closer to the ground than the indicated value. This should not be greater than the Character Controller’s height or it will generate an error.")]
        public float StepOffset
        {
            get { return Internal_GetStepOffset(unmanagedPtr); }
            set { Internal_SetStepOffset(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetStepOffset(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStepOffset(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the minimum move distance of the character controller. The minimum travelled distance to consider. If travelled distance is smaller, the character doesn't move. This is used to stop the recursive motion algorithm when remaining distance to travel is small.
        /// </summary>
        [EditorOrder(230), DefaultValue(0.0f), Limit(0, 1000), EditorDisplay("Character Controller")]
        [Tooltip("The minimum move distance of the character controller. The minimum travelled distance to consider. If travelled distance is smaller, the character doesn't move. This is used to stop the recursive motion algorithm when remaining distance to travel is small.")]
        public float MinMoveDistance
        {
            get { return Internal_GetMinMoveDistance(unmanagedPtr); }
            set { Internal_SetMinMoveDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMinMoveDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMinMoveDistance(IntPtr obj, float value);

        /// <summary>
        /// Gets the linear velocity of the Character Controller. This allows tracking how fast the character is actually moving, for instance when it is stuck at a wall this value will be the near zero vector.
        /// </summary>
        [Tooltip("The linear velocity of the Character Controller. This allows tracking how fast the character is actually moving, for instance when it is stuck at a wall this value will be the near zero vector.")]
        public Vector3 Velocity
        {
            get { Internal_GetVelocity(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVelocity(IntPtr obj, out Vector3 resultAsRef);

        /// <summary>
        /// Gets a value indicating whether this character was grounded during last move call grounded.
        /// </summary>
        [Tooltip("Gets a value indicating whether this character was grounded during last move call grounded.")]
        public bool IsGrounded
        {
            get { return Internal_IsGrounded(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsGrounded(IntPtr obj);

        /// <summary>
        /// Gets the current collision flags. Tells which parts of the character capsule collided with the environment during the last move call. It can be used to trigger various character animations.
        /// </summary>
        [Tooltip("The current collision flags. Tells which parts of the character capsule collided with the environment during the last move call. It can be used to trigger various character animations.")]
        public CollisionFlags Flags
        {
            get { return Internal_GetFlags(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CollisionFlags Internal_GetFlags(IntPtr obj);

        /// <summary>
        /// Moves the character with the given speed. Gravity is automatically applied. It will slide along colliders. Result collision flags is the summary of collisions that occurred during the Move.
        /// </summary>
        /// <param name="speed">The movement speed (in units/s).</param>
        /// <returns>The collision flags. It can be used to trigger various character animations.</returns>
        public CollisionFlags SimpleMove(Vector3 speed)
        {
            return Internal_SimpleMove(unmanagedPtr, ref speed);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CollisionFlags Internal_SimpleMove(IntPtr obj, ref Vector3 speed);

        /// <summary>
        /// Moves the character using a 'collide-and-slide' algorithm. Attempts to move the controller by the given displacement vector, the motion will only be constrained by collisions. It will slide along colliders. Result collision flags is the summary of collisions that occurred during the Move. This function does not apply any gravity.
        /// </summary>
        /// <param name="displacement">The displacement vector (in world units).</param>
        /// <returns>The collision flags. It can be used to trigger various character animations.</returns>
        public CollisionFlags Move(Vector3 displacement)
        {
            return Internal_Move(unmanagedPtr, ref displacement);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CollisionFlags Internal_Move(IntPtr obj, ref Vector3 displacement);

        /// <summary>
        /// Specifies which sides a character is colliding with.
        /// </summary>
        [Flags]
        [Tooltip("Specifies which sides a character is colliding with.")]
        public enum CollisionFlags
        {
            /// <summary>
            /// The character is not colliding.
            /// </summary>
            [Tooltip("The character is not colliding.")]
            None = 0,

            /// <summary>
            /// The character is colliding to the sides.
            /// </summary>
            [Tooltip("The character is colliding to the sides.")]
            Sides = 1 << 0,

            /// <summary>
            /// The character has collision above.
            /// </summary>
            [Tooltip("The character has collision above.")]
            Above = 1 << 1,

            /// <summary>
            /// The character has collision below.
            /// </summary>
            [Tooltip("The character has collision below.")]
            Below = 1 << 2,
        }

        /// <summary>
        /// Specifies how a character controller interacts with non-walkable parts.
        /// </summary>
        [Tooltip("Specifies how a character controller interacts with non-walkable parts.")]
        public enum NonWalkableModes
        {
            /// <summary>
            /// Stops character from climbing up non-walkable slopes, but doesn't move it otherwise.
            /// </summary>
            [Tooltip("Stops character from climbing up non-walkable slopes, but doesn't move it otherwise.")]
            PreventClimbing,

            /// <summary>
            /// Stops character from climbing up non-walkable slopes, and forces it to slide down those slopes.
            /// </summary>
            [Tooltip("Stops character from climbing up non-walkable slopes, and forces it to slide down those slopes.")]
            PreventClimbingAndForceSliding,
        }
    }
}
