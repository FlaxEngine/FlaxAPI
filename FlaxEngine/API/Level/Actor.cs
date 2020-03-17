// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    partial class Actor : ITransformable, ISceneObject
    {
        /// <summary>
        /// The rotation as Euler angles in degrees.
        /// </summary>
        /// <remarks>
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </remarks>
        [HideInEditor, NoSerialize, NoAnimate]
        public Vector3 EulerAngles
        {
            get => Orientation.EulerAngles;
            set
            {
                Quaternion.Euler(ref value, out var orientation);
                Internal_SetOrientation(unmanagedPtr, ref orientation);
            }
        }

        /// <summary>
        /// The local rotation as Euler angles in degrees.
        /// </summary>
        /// <remarks>
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </remarks>
        [HideInEditor, NoSerialize, NoAnimate]
        public Vector3 LocalEulerAngles
        {
            get => LocalOrientation.EulerAngles;
            set
            {
                Quaternion.Euler(ref value, out var orientation);
                Internal_SetLocalOrientation(unmanagedPtr, ref orientation);
            }
        }

        /*/// <summary>
        /// Gets a list of all scripts attached to this object. It's read-only array. Use AddScript/RemoveScript to modify collection.
        /// </summary>
        [UnmanagedCall]
        [HideInEditor, NoAnimate, EditorDisplay("Scripts", EditorDisplayAttribute.InlineStyle), EditorOrder(-5), MemberCollection(ReadOnly = true, NotNullItems = true, CanReorderItems = false)]
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
        }*/

        /// <summary>
        /// Returns true if actor has any children
        /// </summary>
        [HideInEditor, NoSerialize]
        public bool HasChildren => Internal_GetChildrenCount(unmanagedPtr) != 0;

        /// <summary>
        /// Resets actor local transform.
        /// </summary>
        public void ResetLocalTransform()
        {
            LocalTransform = Transform.Identity;
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
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
        /// <returns>The child actor.</returns>
        public T AddChild<T>() where T : Actor
        {
            var result = New<T>();
            result.SetParent(this, false);
            return result;
        }

        /// <summary>
        /// Finds the child actor of the given type.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
        /// <returns>The child actor or null if failed to find.</returns>
        public T GetChild<T>() where T : Actor
        {
            return GetChild(typeof(T)) as T;
        }

        /// <summary>
        /// Finds the child actor of the given type or creates a new one.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
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
            script.Parent = this;
            return script;
        }

        /// <summary>
        /// Creates a new script of a specific type and adds it to the actor.
        /// </summary>
        /// <typeparam name="T">Type of the script to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>The created script instance, null otherwise.</returns>
        public T AddScript<T>() where T : Script
        {
            var script = New<T>();
            script.Parent = this;
            return script;
        }

        /// <summary>
        /// Finds the script of the given type.
        /// </summary>
        /// <typeparam name="T">Type of the script to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>The script or null if failed to find.</returns>
        public T GetScript<T>() where T : Script
        {
            return GetScript(typeof(T)) as T;
        }

        /*/// <summary>
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
        }*/

        /// <summary>
        /// Searches for all actors of a specific type in this actor children list.
        /// </summary>
        /// <typeparam name="T">Type of the actor to search for. Includes any actors derived from the type.</typeparam>
        /// <returns>All actors matching the specified type</returns>
        public T[] GetChildren<T>() where T : Actor
        {
            // TODO: use a proper array allocation and converting on backend to reduce memory allocations
            var children = GetChildren(typeof(T));
            var output = new T[children.Length];
            for (int i = 0; i < children.Length; i++)
                output[i] = (T)children[i];
            return output;
        }

        /// <summary>
        /// Searches for all scripts of a specific type.
        /// </summary>
        /// <typeparam name="T">Type of the scripts to search for. Includes any scripts derived from the type.</typeparam>
        /// <returns>All scripts matching the specified type.</returns>
        public T[] GetScripts<T>() where T : Script
        {
            // TODO: use a proper array allocation and converting on backend to reduce memory allocations
            var scripts = GetScripts(typeof(T));
            var output = new T[scripts.Length];
            for (int i = 0; i < scripts.Length; i++)
                output[i] = (T)scripts[i];
            return output;
        }

        /// <summary>
        /// Destroys the children. Calls Object.Destroy on every child actor and unlink them for the parent.
        /// </summary>
        /// <param name="timeLeft">The time left to destroy object (in seconds).</param>
        [NoAnimate]
        public void DestroyChildren(float timeLeft = 0.0f)
        {
            Actor[] children = Children;
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
                GetWorldToLocalMatrix(out var worldToLocal);
                return worldToLocal;
            }
        }

        /// <summary>
        /// Gets the matrix that transforms a point from the local space of the actor to world space.
        /// </summary>
        public Matrix LocalToWorldMatrix
        {
            get
            {
                GetLocalToWorldMatrix(out var localToWorld);
                return localToWorld;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }
    }
}
