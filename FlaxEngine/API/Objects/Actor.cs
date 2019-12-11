// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all actor types on the scene
    /// </summary>
    public abstract partial class Actor : ITransformable, ISceneObject
    {
        /// <summary>
        /// Returns true if object is fully static on the scene
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        public bool IsStatic
        {
#if UNIT_TEST_COMPILANT
            get { return StaticFlags == StaticFlags.FullyStatic; }
#else
            get { return Internal_GetStaticFlags(unmanagedPtr) == StaticFlags.FullyStatic; }
#endif
        }

        /// <summary>
        /// The rotation as Euler angles in degrees.
        /// </summary>
        /// <remarks>
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees
        /// around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </remarks>
        [UnmanagedCall]
        [HideInEditor]
        [NoSerialize, NoAnimate]
        public Vector3 EulerAngles
        {
#if UNIT_TEST_COMPILANT
			get { return Orientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Orientation = orientation;
            }
#else
            get { return Orientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetOrientation(unmanagedPtr, ref orientation);
            }
#endif
        }

        /// <summary>
        /// The local rotation as Euler angles in degrees.
        /// </summary>
        /// <remarks>
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees
        /// around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </remarks>
        [UnmanagedCall]
        [HideInEditor]
        [NoSerialize, NoAnimate]
        public Vector3 LocalEulerAngles
        {
#if UNIT_TEST_COMPILANT
			get { return LocalOrientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                LocalOrientation = orientation;
            }
#else
            get { return LocalOrientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetLocalOrientation(unmanagedPtr, ref orientation);
            }
#endif
        }

        /// <summary>
        /// Gets or sets the actor direction vector (aka forward direction).
        /// </summary>
        [HideInEditor]
        [NoSerialize, NoAnimate]
        public Vector3 Direction
        {
            get => Vector3.Forward * Orientation;
            set
            {
                if (Vector3.Dot(value, Vector3.Up) >= 0.999f)
                {
                    Orientation = Quaternion.RotationAxis(Vector3.Left, Mathf.PiOverTwo);
                }
                else
                {
                    Vector3 right = Vector3.Cross(value, Vector3.Up);
                    Vector3 up = Vector3.Cross(right, value);
                    Orientation = Quaternion.LookRotation(value, up);
                }
            }
        }

        /// <summary>
        /// Gets a list of all scripts attached to this object. It's read-only array. Use AddScript/RemoveScript to modify
        /// collection.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor, NoAnimate]
        [EditorDisplay("Scripts", EditorDisplayAttribute.InlineStyle)]
        [EditorOrder(-5)]
        [MemberCollection(ReadOnly = true, NotNullItems = true, CanReorderItems = false)]
        public Script[] Scripts
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get
            {
                if (Internal_GetScriptsCount(unmanagedPtr) == 0)
                    return Utils.GetEmptyArray<Script>();
                return Internal_GetScripts(unmanagedPtr);
            }
            internal set { }
#endif
        }

        /// <summary>
        /// Returns true if actor has parent
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        [NoSerialize]
        public bool HasParent
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetParent(unmanagedPtr) != null; }
#endif
        }

        /// <summary>
        /// Returns true if actor has any children
        /// </summary>
        [UnmanagedCall]
        [HideInEditor]
        [NoSerialize]
        public bool HasChildren
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetChildCount(unmanagedPtr) > 0; }
#endif
        }

        /// <summary>
        /// Resets actor local transform
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetLocalTransform()
        {
            LocalTransform = Transform.Identity;
        }

        /// <summary>
        /// Rotates the actor so the forward vector points at target's current position.
        /// </summary>
        /// <param name="target">The target object to point towards.</param>
        public void LookAt(Actor target)
        {
            if (target == null)
                throw new ArgumentNullException();
            var pos = target.Position;
            Internal_LookAt1(unmanagedPtr, ref pos);
        }

        /// <summary>
        /// Rotates the actor so the forward vector points at target's current position.
        /// </summary>
        /// <param name="target">The target object to point towards.</param>
        /// <param name="worldUp">The upward direction vector (in world space).</param>
        public void LookAt(Actor target, Vector3 worldUp)
        {
            if (target == null)
                throw new ArgumentNullException();
            var pos = target.Position;
            Internal_LookAt2(unmanagedPtr, ref pos, ref worldUp);
        }

        /// <summary>
        /// Rotates the actor so the forward vector points at target's current position.
        /// </summary>
        /// <param name="worldPosition">The target point to look at.</param>
        public void LookAt(Vector3 worldPosition)
        {
            Internal_LookAt1(unmanagedPtr, ref worldPosition);
        }

        /// <summary>
        /// Rotates the actor so the forward vector points at target's current position.
        /// </summary>
        /// <param name="worldPosition">The target point to look at.</param>
        /// <param name="worldUp">The upward direction vector (in world space).</param>
        public void LookAt(ref Vector3 worldPosition, ref Vector3 worldUp)
        {
            Internal_LookAt2(unmanagedPtr, ref worldPosition, ref worldUp);
        }

        /// <summary>
        /// Casts this actor instance to the given actor type.
        /// </summary>
        /// <typeparam name="T">The actor type.</typeparam>
        /// <returns>The actor instance cast to the given actor type.</returns>
        public T As<T>() where T : Actor
        {
            return (T)this;
        }

        /// <summary>
        /// Returns true if actor object has child actor with given name
        /// </summary>
        /// <param name="name">Actor name</param>
        /// <returns></returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public bool HasChild(string name)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetChildWithName(unmanagedPtr, name) != null;
#endif
        }

        /// <summary>
        /// Tries to find the actor of the given type in this actor tree (checks this actor and all children trees).
        /// </summary>
        /// <typeparam name="T">The type of the actor to find.</typeparam>
        /// <returns>Actor instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T FindActor<T>() where T : Actor
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_FindActorByType(unmanagedPtr, typeof(T)) as T;
#endif
        }

        /// <summary>
        /// Tries to find the script of the given type in this script tree (checks this actor and all children trees).
        /// </summary>
        /// <typeparam name="T">The type of the script to find.</typeparam>
        /// <returns>Script instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T FindScript<T>() where T : Script
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_FindScriptByType(unmanagedPtr, typeof(T)) as T;
#endif
        }

        /// <summary>
        /// Tries to find the actor of the given type in all the loaded scenes.
        /// </summary>
        /// <typeparam name="T">The type of the actor to find.</typeparam>
        /// <returns>Actor instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T Find<T>() where T : Actor
        {
#if UNIT_TEST_COMPILANT
            return null;
#else
            return Internal_FindByType(typeof(T)) as T;
#endif
        }

        /// <summary>
        /// Sets actor parent to this object
        /// </summary>
        /// <param name="actor">Actor to link</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void AddChild(Actor actor, bool worldPositionStays = true)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            actor?.SetParent(this, worldPositionStays);
#endif
        }

        /// <summary>
        /// Creates a new child actor of the given type.
        /// </summary>
        /// <param name="type">Type of the actor.</param>
        /// <returns>The child actor.</returns>
        public Actor AddChild(Type type)
        {
            var result = (Actor)New(type);
            result.SetParent(this, false);
            return result;
        }

        /// <summary>
        /// Creates a new child actor of the given type.
        /// </summary>
        /// <typeparam name="T">Type of the actor.</typeparam>
        /// <returns>The child actor.</returns>
        public T AddChild<T>() where T : Actor
        {
            var result = New<T>();
            result.SetParent(this, false);
            return result;
        }

        /// <summary>
        /// Finds the child actor of the given type or creates a new one.
        /// </summary>
        /// <typeparam name="T">Type of the actor.</typeparam>
        /// <returns>The child actor.</returns>
        public T GetOrAddChild<T>() where T : Actor
        {
            var result = GetChild<T>();
            if (result == null)
            {
                result = New<T>();
                result.SetParent(this, false);
            }
            return result;
        }

        /// <summary>
        /// Creates a new script of a specific type and adds it to the actor.
        /// </summary>
        /// <param name="type">Type of the script to create.</param>
        /// <returns>The created script instance, null otherwise.</returns>
        public Script AddScript(Type type)
        {
            var script = (Script)New(type);
            AddScript(script);
            return script;
        }

        /// <summary>
        /// Creates a new script of a specific type and adds it to the actor.
        /// </summary>
        /// <typeparam name="T">Type of the script to create.</typeparam>
        /// <returns>The created script instance, null otherwise.</returns>
        public T AddScript<T>() where T : Script
        {
            var script = New<T>();
            AddScript(script);
            return script;
        }

        /// <summary>
        /// Determines if there is an intersection between the actor and a ray.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection (if any valid).</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public bool IntersectsItself(ref Ray ray, out float distance)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_IntersectsItself(unmanagedPtr, ref ray, out distance, out _);
#endif
        }

        /// <summary>
        /// Determines if there is an intersection between the actor and a ray.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public bool IntersectsItself(ref Ray ray, out float distance, out Vector3 normal)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_IntersectsItself(unmanagedPtr, ref ray, out distance, out normal);
#endif
        }

        /// <summary>
        /// Serializes the actor object to the raw bytes. Serialized are actor properties and scripts but no child actors.
        /// Serializes references to the other objects in a proper way using IDs.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>The bytes array with serialized actor data. Returns null if fails.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static byte[] ToBytes(Actor actor)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_ToBytes1(GetUnmanagedPtr(actor));
#endif
        }

        /// <summary>
        /// Serializes the actor object to the Json string. Serialized are only actor properties but no child actors nor scripts. 
        /// Serializes references to the other objects in a proper way using IDs.
        /// </summary>
        /// <param name="actor">The actor to serialize.</param>
        /// <returns>The Json container with serialized actor data. Returns null if fails.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static string Serialize(Actor actor)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_Serialize(GetUnmanagedPtr(actor));
#endif
        }

        /// <summary>
        /// Deserializes the actor object to the Json string. Deserializes are only actor properties but no child actors nor scripts. 
        /// </summary>
        /// <param name="actor">The actor to deserialize.</param>
        /// <param name="data">The serialized actor data (state).</param>
        /// <returns>The Json container with serialized actor data. Returns null if fails.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static void Deserialize(Actor actor, string data)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_Deserialize(GetUnmanagedPtr(actor), data);
#endif
        }

        /// <summary>
        /// Serializes the actor objects to the raw bytes. Serialized are actor properties and scripts but no child actors.
        /// Serializes references to the other objects in a proper way using IDs.
        /// </summary>
        /// <param name="actors">The actors.</param>
        /// <returns>The bytes array with serialized actors data. Returns null if fails.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static byte[] ToBytes(Actor[] actors)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_ToBytes2(Array.ConvertAll(actors, GetUnmanagedPtr));
#endif
        }

        /// <summary>
        /// Deserializes the actor objects from the raw bytes. Deserialized are actor properties and scripts but no child actors.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="idsMapping">
        /// The serialized objects ids mapping table used to change object ids and keep valid reference
        /// links. Use null value to skip ids mapping. To generate a new ids for the loaded objects use <see cref="TryGetSerializedObjectsIds"/> to extract the object ids from the data.
        /// </param>
        /// <returns>Spawned actors deserialized from the data. Returns null if fails.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public static Actor[] FromBytes(byte[] data, Dictionary<Guid, Guid> idsMapping = null)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Guid[] keys = idsMapping?.Keys.ToArray();
            Guid[] values = idsMapping?.Values.ToArray();
            return Internal_FromBytes(data, keys, values);
#endif
        }

        /// <summary>
        /// Searches for a child actor of a specific type. If there are multiple actors matching the type, only the first one found is returned.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
        /// <returns>Actor instance if found, null otherwise</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T GetChild<T>() where T : Actor
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_GetChild(unmanagedPtr, typeof(T));
#endif
        }

        /// <summary>
        /// Searches for all actors of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
        /// <returns>All actors matching the specified type</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T[] GetChildren<T>() where T : Actor
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // TODO: use a proper array allocation and convertion on backend to reduce memory allocations
            return Array.ConvertAll(Internal_GetChildrenPerType(unmanagedPtr, typeof(T)), x => (T)x);
#endif
        }

        /// <summary>
        /// Searches for a child script of a specific type. If there are multiple scripts matching the type, only the first one found is returned.
        /// </summary>
        /// <typeparam name="T">Type of the script to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>Script instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T GetScript<T>() where T : Script
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_GetScript(unmanagedPtr, typeof(T));
#endif
        }

        /// <summary>
        /// Searches for all scripts of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of the scripts to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>All scripts matching the specified type.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T[] GetScripts<T>() where T : Script
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // TODO: use a proper array allocation and convertion on backend to reduce memory allocations
            return Array.ConvertAll(Internal_GetScriptsPerType(unmanagedPtr, typeof(T)), x => (T)x);
#endif
        }

        /// <summary>
        /// Searches for a child script of a specific type in this actor or any of its children. If there are multiple scripts matching the type, only the first one found is returned.
        /// </summary>
        /// <param name="includeDisabled">Determines whether include disabled scripts into results (disabled scripts and/or inactive actors).</param>
        /// <typeparam name="T">Type of the script to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>Script instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T GetScriptInChildren<T>(bool includeDisabled = false) where T : Script
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return (T)Internal_GetScriptInChildren(unmanagedPtr, typeof(T), includeDisabled);
#endif
        }

        /// <summary>
        /// Searches for all scripts of a specific type in this actor and any of its children.
        /// </summary>
        /// <param name="includeDisabled">Determines whether include inactive scripts into results (disabled scripts and/or inactive actors).</param>
        /// <typeparam name="T">Type of the scripts to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>All scripts matching the specified type and query options.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public T[] GetScriptsInChildren<T>(bool includeDisabled = false) where T : Script
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // TODO: use a proper array allocation and convertion on backend to reduce memory allocations
            return Array.ConvertAll(Internal_GetScriptsInChildrenPerType(unmanagedPtr, typeof(T), includeDisabled), x => (T)x);
#endif
        }

        /// <summary>
        /// Destroys the children. Calls Object.Destroy on every child actor and unlink them for the parent.
        /// </summary>
        /// <param name="timeLeft">The time left to destroy object (in seconds).</param>
        [NoAnimate]
        public void DestroyChildren(float timeLeft = 0.0f)
        {
            Actor[] children = GetChildren();
            for (var i = 0; i < children.Length; i++)
            {
                children[i].Parent = null;
                Destroy(children[i], timeLeft);
            }
        }

        /// <summary>
        /// Gets the matrix that transforms a point from the world space to local space of the actor.
        /// </summary>
        public Matrix WorldToLocalMatrix
        {
            get
            {
                Matrix worldToLocal;
                Internal_WorldToLocal(unmanagedPtr, out worldToLocal);
                return worldToLocal;
            }
        }

        /// <summary>
        /// Gets the matrix that transforms a point from the world space to local space of the actor.
        /// </summary>
        /// <param name="worldToLocal">The world to local matrix.</param>
        public void GetWorldToLocalMatrix(out Matrix worldToLocal)
        {
            Internal_WorldToLocal(unmanagedPtr, out worldToLocal);
        }

        /// <summary>
        /// Gets the matrix that transforms a point from the local space of the actor to world space.
        /// </summary>
        public Matrix LocalToWorldMatrix
        {
            get
            {
                Matrix localToWorld;
                Internal_LocalToWorld(unmanagedPtr, out localToWorld);
                return localToWorld;
            }
        }

        /// <summary>
        /// Gets the matrix that transforms a point from the local space of the actor to world space.
        /// </summary>
        /// <param name="localToWorld">The world to local matrix.</param>
        public void GetLocalToWorldMatrix(out Matrix localToWorld)
        {
            Internal_LocalToWorld(unmanagedPtr, out localToWorld);
        }

        /// <inheritdoc />
        [UnmanagedCall]
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script[] Internal_GetScripts(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LookAt1(IntPtr obj, ref Vector3 worldPos);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LookAt2(IntPtr obj, ref Vector3 worldPos, ref Vector3 worldUp);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_WorldToLocal(IntPtr obj, out Matrix matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LocalToWorld(IntPtr obj, out Matrix matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsItself(IntPtr obj, ref Ray ray, out float distance, out Vector3 normal);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_Serialize(IntPtr actor);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Deserialize(IntPtr actor, string data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_ToBytes1(IntPtr actor);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_ToBytes2(IntPtr[] actors);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_FromBytes(byte[] data, Guid[] idsMappingKeys, Guid[] idsMappingValues);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LinkPrefab(IntPtr obj, ref Guid prefabId, ref Guid prefabObjectId);
#endif

        #endregion
    }
}
