// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all actor types on the scene
    /// </summary>
    public abstract class Actor : Object
    {
        // TODO: set direction
        // TODO: Instantiate from prefab
        // TODO: Destroy
        // TODO: TransformDirection, TranformPoint
        // TODO: InverseTransformDirection, InverseTransformPoint
        // TODO: LootAt, Translate, Rotate

        /// <summary>
        /// Gets or sets parent actor (or null if actor has no parent)
        /// </summary>
        public Actor Parent
        {
            get { return Internal_GetParent(unmanagedPtr); }
            set { Internal_SetParent(unmanagedPtr, GetUnmanagedPtr(value), false); }
        }

        /// <summary>
        /// Returns true if actor has parent
        /// </summary>
        public bool HasParent
        {
            get { return Internal_GetParent(unmanagedPtr) != null; }
        }

        /// <summary>
        /// Gets or sets actor name
        /// </summary>
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
            set { Internal_SetName(unmanagedPtr, value); }
        }

        /// <summary>
        /// Sets parent actor
        /// </summary>
        /// <param name="newParent">New parent to assign</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
        public void SetParent(Actor newParent, bool worldPositionStays = true)
        {
            Internal_SetParent(unmanagedPtr, GetUnmanagedPtr(newParent), worldPositionStays);
        }

        /// <summary>
        /// Gets or sets actor static fags
        /// </summary>
        public StaticFlags StaticFlags
        {
            get { return Internal_GetStaticFlags(unmanagedPtr); }
            set { Internal_SetStaticFlags(unmanagedPtr, value); }
        }

        /// <summary>
        /// Returns true if object is fully static on the scene
        /// </summary>
        public bool IsStatic
        {
            get { return Internal_GetStaticFlags(unmanagedPtr) == StaticFlags.FullyStatic; }
        }

        public bool IsActive
        {
            get { return Internal_GetIsActive(unmanagedPtr); }
            set { Internal_SetIsActive(unmanagedPtr, value); }
        }

        public bool IsActiveInHierarchy
        {
            get { return Internal_GetIsActiveInHierarchy(unmanagedPtr); }
        }

        #region Transformation

        public Vector3 Position
        {
            get { return Internal_GetPosition(unmanagedPtr); }
            set { Internal_SetPosition(unmanagedPtr, ref value); }
        }

        public Quaternion Orientation
        {
            get { return Internal_GetOrientation(unmanagedPtr); }
            set { Internal_SetOrientation(unmanagedPtr, ref value); }
        }

        /// <summary>
        /// The rotation as Euler angles in degrees.
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </summary>
        public Vector3 EulerAngles
        {
            get { return Internal_GetOrientation(unmanagedPtr).EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetOrientation(unmanagedPtr, ref orientation);
            }
        }

        public Vector3 Scale
        {
            get { return Internal_GetScale(unmanagedPtr); }
            set { Internal_SetScale(unmanagedPtr, ref value); }
        }

        public Transform Transform
        {
            get { return Internal_GetTransform(unmanagedPtr); }
            set { Internal_SetTransform(unmanagedPtr, ref value); }
        }

        public Vector3 LocalPosition
        {
            get { return Internal_GetLocalPosition(unmanagedPtr); }
            set { Internal_SetLocalPosition(unmanagedPtr, ref value); }
        }

        public Quaternion LocalOrientation
        {
            get { return Internal_GetLocalOrientation(unmanagedPtr); }
            set { Internal_SetLocalOrientation(unmanagedPtr, ref value); }
        }

        /// <summary>
        /// The local rotation as Euler angles in degrees.
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </summary>
        public Vector3 LocaEulerAngles
        {
            get { return Internal_GetLocalOrientation(unmanagedPtr).EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetLocalOrientation(unmanagedPtr, ref orientation);
            }
        }

        public Vector3 LocalScale
        {
            get { return Internal_GetLocalScale(unmanagedPtr); }
            set { Internal_SetLocalScale(unmanagedPtr, ref value); }
        }

        public Transform LocalTransform
        {
            get { return Internal_GetLocalTransform(unmanagedPtr); }
            set { Internal_SetLocalTransform(unmanagedPtr, ref value); }
        }

        /// <summary>
        /// Gets actor direction vector
        /// </summary>
        public Vector3 Direction
        {
            get { return Vector3.ForwardLH * Orientation; }
        }

        /// <summary>
        /// Resets actor local transform
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetLocalTransform()
        {
            LocalTransform = Transform.Identity;
        }

        #endregion

        #region Children

        /// <summary>
        /// Returns true if actor has any children
        /// </summary>
        public bool HasChildren
        {
            get { return Internal_GetChildCount(unmanagedPtr) > 0; }
        }

        /// <summary>
        /// Gest amount of children
        /// </summary>
        public int ChildCount
        {
            get { return Internal_GetChildCount(unmanagedPtr); }
        }

        /// <summary>
        /// Sets actor parent to this object
        /// </summary>
        /// <param name="actor">Actor to link</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
        public void AddChild(Actor actor, bool worldPositionStays = true)
        {
            if (actor != null)
                actor.SetParent(this);
        }

        /// <summary>
        /// Gets child actor at given index
        /// </summary>
        /// <param name="index">Child actor index</param>
        /// <returns>Child actor</returns>
        public Actor GetChild(int index)
        {
            return Internal_GetChildAtIndex(unmanagedPtr, index);
        }

        /// <summary>
        /// Gets child actor with given name
        /// </summary>
        /// <param name="name">Child actor name</param>
        /// <returns>Child actor</returns>
        public Actor GetChild(string name)
        {
            return Internal_GetChildWithName(unmanagedPtr, name);
        }

        /// <summary>
        /// Returns true if actor object has child actor with given name
        /// </summary>
        /// <param name="name">Actor name</param>
        /// <returns></returns>
        public bool HasChild(string name)
        {
            return Internal_GetChildWithName(unmanagedPtr, name) != null;
        }
        
        /// <summary>
        /// Tries to find child actor with given name
        /// </summary>
        /// <param name="name">Actor to find name</param>
        /// <returns>Found child actor or null if cannot find actor with specified name</returns>
        public Actor FindChild(string name)
        {
            return Internal_GetChildWithName(unmanagedPtr, name);
        }

        /// <summary>
        /// Tries to find actor with given name in this actor tree
        /// </summary>
        /// <param name="name">Actor to find name</param>
        /// <returns>Found actor or null if cannot find actor with specified name</returns>
        public Actor FindActor(string name)
        {
            return Internal_FindActorWithName(unmanagedPtr, name);
        }

        /// <summary>
        /// Tries to find actor with given name on the scene
        /// </summary>
        /// <param name="name">Actor to find name</param>
        /// <returns>Found actor or null if cannot find actor with specified name</returns>
        public static Actor Find(string name)
        {
            return Internal_FindActor(name);
        }

        /// <summary>
        /// Searches for a child actor of a specific type. If there are multiple actors matching the type, only the first one found is returned.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.
        /// </typeparam>
        /// <returns>Actor instance if found, null otherwise.</returns>
        public T GetChild<T>() where T : Actor
        {
            return (T)Internal_GetChild(unmanagedPtr, typeof(T));
        }

        /// <summary>
        /// Searches for all actors of a specific type. 
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.
        /// </typeparam>
        /// <returns>All actors matching the specified type.</returns>
        public T[] GetChildren<T>() where T : Actor
        {
            Actor[] actors = Internal_GetChildrenPerType(unmanagedPtr, typeof(T));
            return Array.ConvertAll(actors, x => (T)x);
        }

        /// <summary>
        /// Returns a list of all actors attached to this object.
        /// </summary>
        /// <returns>All actors attached to this object.</returns>
        public Actor[] GetChildren()
        {
            return Internal_GetChildren(unmanagedPtr);
        }

        #endregion

        #region Scripts


        /// <summary>
        /// Searches for a child script of a specific type. If there are multiple scripts matching the type, only the first one found is returned.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any scripts derived from the type.
        /// </typeparam>
        /// <returns>Script instance if found, null otherwise.</returns>
        public T GetScript<T>() where T : Script
        {
            return (T)Internal_GetScript(unmanagedPtr, typeof(T));
        }

        /// <summary>
        /// Searches for all scripts of a specific type. 
        /// </summary>
        /// <typeparam name="T">Type of the script to search for. Includes any scripts derived from the type.
        /// </typeparam>
        /// <returns>All scripts matching the specified type.</returns>
        public T[] GetScripts<T>() where T : Script
        {
            Script[] scripts = Internal_GetScriptsPerType(unmanagedPtr, typeof(T));
            return Array.ConvertAll(scripts, x => (T)x);
        }

        /// <summary>
        /// Returns a list of all scripts attached to this object.
        /// </summary>
        /// <returns>All scripts attached to this object.</returns>
        public Script[] GetScripts()
        {
            return Internal_GetScripts(unmanagedPtr);
        }

        #endregion

        /// <summary>
        /// Gets bounding box that contains actor object (single actor, no children included)
        /// </summary>
        public BoundingBox Box
        {
            get { return Internal_GetBox(unmanagedPtr); }
        }

        /// <summary>
        /// Gets bounding box that contains actor object and all it's children (children included in recursive way)
        /// </summary>
        public BoundingBox BoxWithChildren
        {
            get { return Internal_GetBoxWithChildren(unmanagedPtr); }
        }

        /// <summary>
        /// Returns true if actor has loaded content
        /// </summary>
        public bool HasContentLoaded
        {
            get { return Internal_HasContentLoaded(unmanagedPtr); }
        }

        /// <summary>
        /// Returns true if actor has fully loaded content
        /// </summary>
        public bool HasContentFullyLoaded
        {
            get { return Internal_HasContentFullyLoaded(unmanagedPtr); }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1})", Internal_GetName(unmanagedPtr), GetType().Name);
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetParent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParent(IntPtr obj, IntPtr newParent, bool worldPositionStays);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, string value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern StaticFlags Internal_GetStaticFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStaticFlags(IntPtr obj, StaticFlags value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsActive(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsActive(IntPtr obj, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsActiveInHierarchy(IntPtr obj);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Vector3 Internal_GetPosition(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPosition(IntPtr obj, ref Vector3 value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quaternion Internal_GetOrientation(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOrientation(IntPtr obj, ref Quaternion value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Vector3 Internal_GetScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScale(IntPtr obj, ref Vector3 value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Transform Internal_GetTransform(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTransform(IntPtr obj, ref Transform value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Vector3 Internal_GetLocalPosition(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalPosition(IntPtr obj, ref Vector3 value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Quaternion Internal_GetLocalOrientation(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalOrientation(IntPtr obj, ref Quaternion value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Vector3 Internal_GetLocalScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalScale(IntPtr obj, ref Vector3 value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Transform Internal_GetLocalTransform(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalTransform(IntPtr obj, ref Transform value);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetChildCount(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChildAtIndex(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChildWithName(IntPtr obj, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActorWithName(IntPtr obj, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor(string name);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChild(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_GetChildrenPerType(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_GetChildren(IntPtr obj);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_GetScript(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script[] Internal_GetScriptsPerType(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script[] Internal_GetScripts(IntPtr obj);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern BoundingBox Internal_GetBox(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern BoundingBox Internal_GetBoxWithChildren(IntPtr obj);

        //

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasContentLoaded(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasContentFullyLoaded(IntPtr obj);

        #endregion
    }
}
