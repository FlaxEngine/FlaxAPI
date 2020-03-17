// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all actor objects on the scene.
    /// </summary>
    [Tooltip("Base class for all actor objects on the scene.")]
    public abstract unsafe partial class Actor : SceneObject
    {
        /// <inheritdoc />
        protected Actor() : base()
        {
        }

        /// <summary>
        /// List with all child actors attached to the actor (readonly). All items are valid (not null).
        /// </summary>
        [Tooltip("List with all child actors attached to the actor (readonly). All items are valid (not null).")]
        public Actor[] Children
        {
            get { return Internal_GetChildren(unmanagedPtr, typeof(Actor)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_GetChildren(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// List with all scripts attached to the actor (readonly). All items are valid (not null).
        /// </summary>
        [Tooltip("List with all scripts attached to the actor (readonly). All items are valid (not null).")]
        public Script[] Scripts
        {
            get { return Internal_GetScripts(unmanagedPtr, typeof(Script)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script[] Internal_GetScripts(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// The hide flags.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The hide flags.")]
        public HideFlags HideFlags
        {
            get { return Internal_GetHideFlags(unmanagedPtr); }
            set { Internal_SetHideFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern HideFlags Internal_GetHideFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetHideFlags(IntPtr obj, HideFlags value);

        /// <summary>
        /// Gets or sets the object layer (index). Can be used for selective rendering or ignoring raycasts.
        /// </summary>
        [NoAnimate, EditorDisplay("General"), EditorOrder(-69)]
        [Tooltip("The object layer (index). Can be used for selective rendering or ignoring raycasts.")]
        public int Layer
        {
            get { return Internal_GetLayer(unmanagedPtr); }
            set { Internal_SetLayer(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLayer(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLayer(IntPtr obj, int layerIndex);

        /// <summary>
        /// Gets the name of the layer.
        /// </summary>
        [Tooltip("The name of the layer.")]
        public string LayerName
        {
            get { return Internal_GetLayerName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetLayerName(IntPtr obj);

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        [NoAnimate, EditorDisplay("General"), EditorOrder(-68)]
        [Tooltip("The name of the tag.")]
        public string Tag
        {
            get { return Internal_GetTag(unmanagedPtr); }
            set { Internal_SetTag(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetTag(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTag(IntPtr obj, string tagName);

        /// <summary>
        /// Gets or sets the actor name.
        /// </summary>
        [NoAnimate, EditorDisplay("General"), EditorOrder(-100)]
        [Tooltip("The actor name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
            set { Internal_SetName(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, string value);

        /// <summary>
        /// Gets the scene object which contains this actor.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The scene object which contains this actor.")]
        public Scene Scene
        {
            get { return Internal_GetScene(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Scene Internal_GetScene(IntPtr obj);

        /// <summary>
        /// Gets amount of child actors.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets amount of child actors.")]
        public int ChildrenCount
        {
            get { return Internal_GetChildrenCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetChildrenCount(IntPtr obj);

        /// <summary>
        /// Gets amount of scripts.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets amount of scripts.")]
        public int ScriptsCount
        {
            get { return Internal_GetScriptsCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetScriptsCount(IntPtr obj);

        /// <summary>
        /// Gets or sets value indicating if actor is active in the scene.
        /// </summary>
        [EditorDisplay("General"), DefaultValue(true), EditorOrder(-70)]
        [Tooltip("Gets value indicating if actor is active in the scene.")]
        public bool IsActive
        {
            get { return Internal_GetIsActive(unmanagedPtr); }
            set { Internal_SetIsActive(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsActive(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsActive(IntPtr obj, bool value);

        /// <summary>
        /// Gets value indicating if actor is active in the scene graph. It must be active as well as that of all it's parents.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets value indicating if actor is active in the scene graph. It must be active as well as that of all it's parents.")]
        public bool IsActiveInHierarchy
        {
            get { return Internal_IsActiveInHierarchy(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsActiveInHierarchy(IntPtr obj);

        /// <summary>
        /// Returns true if object is fully static on the scene, otherwise false.
        /// </summary>
        [Tooltip("Returns true if object is fully static on the scene, otherwise false.")]
        public bool IsStatic
        {
            get { return Internal_IsStatic(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsStatic(IntPtr obj);

        /// <summary>
        /// Returns true if object has static transform.
        /// </summary>
        [Tooltip("Returns true if object has static transform.")]
        public bool IsTransformStatic
        {
            get { return Internal_IsTransformStatic(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsTransformStatic(IntPtr obj);

        /// <summary>
        /// Gets or sets the actor static fags.
        /// </summary>
        [NoAnimate, EditorDisplay("General"), EditorOrder(-80)]
        [Tooltip("The actor static fags.")]
        public StaticFlags StaticFlags
        {
            get { return Internal_GetStaticFlags(unmanagedPtr); }
            set { Internal_SetStaticFlags(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern StaticFlags Internal_GetStaticFlags(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStaticFlags(IntPtr obj, StaticFlags value);

        /// <summary>
        /// Gets or sets the actor's world transformation.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The actor's world transformation.")]
        public Transform Transform
        {
            get { Internal_GetTransform(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetTransform(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetTransform(IntPtr obj, out Transform resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetTransform(IntPtr obj, ref Transform value);

        /// <summary>
        /// Gets or sets the actor's world transform position.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The actor's world transform position.")]
        public Vector3 Position
        {
            get { Internal_GetPosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPosition(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPosition(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets actor orientation in 3D space
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets actor orientation in 3D space")]
        public Quaternion Orientation
        {
            get { Internal_GetOrientation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetOrientation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientation(IntPtr obj, out Quaternion resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOrientation(IntPtr obj, ref Quaternion value);

        /// <summary>
        /// Gets or sets actor scale in 3D space
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets actor scale in 3D space")]
        public Vector3 Scale
        {
            get { Internal_GetScale(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetScale(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetScale(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScale(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets actor rotation matrix
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets actor rotation matrix")]
        public Matrix Rotation
        {
            get { Internal_GetRotation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetRotation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetRotation(IntPtr obj, out Matrix resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRotation(IntPtr obj, ref Matrix value);

        /// <summary>
        /// Gets the random per-instance value (normalized to range 0-1).
        /// </summary>
        [Tooltip("The random per-instance value (normalized to range 0-1).")]
        public float PerInstanceRandom
        {
            get { return Internal_GetPerInstanceRandom(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetPerInstanceRandom(IntPtr obj);

        /// <summary>
        /// Gets or sets actor direction vector (forward vector).
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets actor direction vector (forward vector).")]
        public Vector3 Direction
        {
            get { Internal_GetDirection(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDirection(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDirection(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDirection(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets local transform of the actor in parent actor space.
        /// </summary>
        [HideInEditor, NoAnimate]
        [Tooltip("Gets local transform of the actor in parent actor space.")]
        public Transform LocalTransform
        {
            get { Internal_GetLocalTransform(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLocalTransform(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalTransform(IntPtr obj, out Transform resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalTransform(IntPtr obj, ref Transform value);

        /// <summary>
        /// Gets or sets local position of the actor in parent actor space.
        /// </summary>
        [EditorDisplay("Transform", "Position"), DefaultValue(typeof(Vector3), "0,0,0"), EditorOrder(-30), NoSerialize]
        [Tooltip("Gets local position of the actor in parent actor space.")]
        public Vector3 LocalPosition
        {
            get { Internal_GetLocalPosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLocalPosition(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalPosition(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalPosition(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets local rotation of the actor in parent actor space.
        /// </summary>
        [EditorDisplay("Transform", "Rotation"), DefaultValue(typeof(Quaternion), "0,0,0,1"), EditorOrder(-20), NoSerialize]
        [Tooltip("Gets local rotation of the actor in parent actor space.")]
        public Quaternion LocalOrientation
        {
            get { Internal_GetLocalOrientation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLocalOrientation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalOrientation(IntPtr obj, out Quaternion resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalOrientation(IntPtr obj, ref Quaternion value);

        /// <summary>
        /// Gets or sets local scale vector of the actor in parent actor space.
        /// </summary>
        [EditorDisplay("Transform", "Scale"), DefaultValue(typeof(Vector3), "1,1,1"), Limit(float.MinValue, float.MaxValue, 0.01f), EditorOrder(-10), NoSerialize]
        [Tooltip("Gets local scale vector of the actor in parent actor space.")]
        public Vector3 LocalScale
        {
            get { Internal_GetLocalScale(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLocalScale(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalScale(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLocalScale(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets actor bounding sphere that defines 3D space intersecting with the actor (for determination of the visibility for actor).
        /// </summary>
        [Tooltip("Gets actor bounding sphere that defines 3D space intersecting with the actor (for determination of the visibility for actor).")]
        public BoundingSphere Sphere
        {
            get { Internal_GetSphere(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSphere(IntPtr obj, out BoundingSphere resultAsRef);

        /// <summary>
        /// Gets actor bounding box that defines 3D space intersecting with the actor (for determination of the visibility for actor).
        /// </summary>
        [Tooltip("Gets actor bounding box that defines 3D space intersecting with the actor (for determination of the visibility for actor).")]
        public BoundingBox Box
        {
            get { Internal_GetBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets actor bounding box of the actor including all child actors (children included in recursive way)
        /// </summary>
        [Tooltip("Gets actor bounding box of the actor including all child actors (children included in recursive way)")]
        public BoundingBox BoxWithChildren
        {
            get { Internal_GetBoxWithChildren(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBoxWithChildren(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets actor bounding box (single actor, no children included) for editor tools.
        /// </summary>
        [Tooltip("Gets actor bounding box (single actor, no children included) for editor tools.")]
        public BoundingBox EditorBox
        {
            get { Internal_GetEditorBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetEditorBox(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets actor bounding box of the actor including all child actors for editor tools.
        /// </summary>
        [Tooltip("Gets actor bounding box of the actor including all child actors for editor tools.")]
        public BoundingBox EditorBoxChildren
        {
            get { Internal_GetEditorBoxChildren(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetEditorBoxChildren(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Returns true if actor has loaded content.
        /// </summary>
        [Tooltip("Returns true if actor has loaded content.")]
        public bool HasContentLoaded
        {
            get { return Internal_HasContentLoaded(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasContentLoaded(IntPtr obj);

        /// <summary>
        /// Gets a value indicating whether this actor is a prefab instance root object.
        /// </summary>
        [Tooltip("Gets a value indicating whether this actor is a prefab instance root object.")]
        public bool IsPrefabRoot
        {
            get { return Internal_IsPrefabRoot(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPrefabRoot(IntPtr obj);

        /// <summary>
        /// Determines whether this actor has tag assigned.
        /// </summary>
        public bool HasTag()
        {
            return Internal_HasTag(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasTag(IntPtr obj);

        /// <summary>
        /// Determines whether this actor has given tag assigned.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        public bool HasTag(string tag)
        {
            return Internal_HasTag1(unmanagedPtr, tag);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasTag1(IntPtr obj, string tag);

        /// <summary>
        /// Gets the child actor at the given index.
        /// </summary>
        /// <param name="index">The child actor index.</param>
        /// <returns>The child actor (always valid).</returns>
        public Actor GetChild(int index)
        {
            return Internal_GetChild(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChild(IntPtr obj, int index);

        /// <summary>
        /// Gets the child actor with the given name.
        /// </summary>
        /// <param name="name">The child actor name.</param>
        /// <returns>The child actor or null.</returns>
        public Actor GetChild(string name)
        {
            return Internal_GetChild1(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChild1(IntPtr obj, string name);

        /// <summary>
        /// Gets the child actor of the given type.
        /// </summary>
        /// <param name="type">Type of the actor to search for. Includes any actors derived from the type.</param>
        /// <returns>The child actor or null.</returns>
        public Actor GetChild(System.Type type)
        {
            return Internal_GetChild2(unmanagedPtr, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetChild2(IntPtr obj, System.Type type);

        /// <summary>
        /// Gets the child actors of the given type.
        /// </summary>
        /// <param name="type">Type of the actor to search for. Includes any actors derived from the type.</param>
        /// <returns>The child actors.</returns>
        public Actor[] GetChildren(System.Type type)
        {
            return Internal_GetChildren1(unmanagedPtr, type, typeof(Actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_GetChildren1(IntPtr obj, System.Type type, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the script at the given index.
        /// </summary>
        /// <param name="index">The script index.</param>
        /// <returns>The script (always valid).</returns>
        public Script GetScript(int index)
        {
            return Internal_GetScript(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_GetScript(IntPtr obj, int index);

        /// <summary>
        /// Gets the script of the given type.
        /// </summary>
        /// <param name="type">Type of the script to search for. Includes any scripts derived from the type.</param>
        /// <returns>The script or null.</returns>
        public Script GetScript(System.Type type)
        {
            return Internal_GetScript1(unmanagedPtr, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_GetScript1(IntPtr obj, System.Type type);

        /// <summary>
        /// Gets the scripts of the given type.
        /// </summary>
        /// <param name="type">Type of the script to search for. Includes any scripts derived from the type.</param>
        /// <returns>The scripts.</returns>
        public Script[] GetScripts(System.Type type)
        {
            return Internal_GetScripts1(unmanagedPtr, type, typeof(Script));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script[] Internal_GetScripts1(IntPtr obj, System.Type type, System.Type resultArrayItemType0);

        /// <summary>
        /// Adds the actor static flags.
        /// </summary>
        /// <param name="flags">The flags to add.</param>
        public void AddStaticFlags(StaticFlags flags)
        {
            Internal_AddStaticFlags(unmanagedPtr, flags);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddStaticFlags(IntPtr obj, StaticFlags flags);

        /// <summary>
        /// Removes the actor static flags.
        /// </summary>
        /// <param name="flags">The flags to remove.</param>
        public void RemoveStaticFlags(StaticFlags flags)
        {
            Internal_RemoveStaticFlags(unmanagedPtr, flags);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveStaticFlags(IntPtr obj, StaticFlags flags);

        /// <summary>
        /// Sets a single static flag to the desire value.
        /// </summary>
        /// <param name="flag">The flag to change.</param>
        /// <param name="value">The target value of the flag.</param>
        public void SetStaticFlag(StaticFlags flag, bool value)
        {
            Internal_SetStaticFlag(unmanagedPtr, flag, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStaticFlag(IntPtr obj, StaticFlags flag, bool value);

        /// <summary>
        /// Moves the actor (also can rotate it) in world space.
        /// </summary>
        /// <param name="translation">The translation vector.</param>
        public void AddMovement(Vector3 translation)
        {
            Internal_AddMovement(unmanagedPtr, ref translation);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddMovement(IntPtr obj, ref Vector3 translation);

        /// <summary>
        /// Moves the actor (also can rotate it) in world space.
        /// </summary>
        /// <param name="translation">The translation vector.</param>
        /// <param name="rotation">The rotation quaternion.</param>
        public void AddMovement(Vector3 translation, Quaternion rotation)
        {
            Internal_AddMovement1(unmanagedPtr, ref translation, ref rotation);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddMovement1(IntPtr obj, ref Vector3 translation, ref Quaternion rotation);

        /// <summary>
        /// Gets the matrix that transforms a point from the world space to local space of the actor.
        /// </summary>
        /// <param name="worldToLocal">The world to local matrix.</param>
        public void GetWorldToLocalMatrix(out Matrix worldToLocal)
        {
            Internal_GetWorldToLocalMatrix(unmanagedPtr, out worldToLocal);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetWorldToLocalMatrix(IntPtr obj, out Matrix worldToLocal);

        /// <summary>
        /// Gets the matrix that transforms a point from the local space of the actor to world space.
        /// </summary>
        /// <param name="localToWorld">The world to local matrix.</param>
        public void GetLocalToWorldMatrix(out Matrix localToWorld)
        {
            Internal_GetLocalToWorldMatrix(unmanagedPtr, out localToWorld);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalToWorldMatrix(IntPtr obj, out Matrix localToWorld);

        /// <summary>
        /// Tries to find the actor with the given name in this actor hierarchy (checks this actor and all children hierarchy).
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        /// <returns>Actor instance if found, null otherwise.</returns>
        public Actor FindActor(string name)
        {
            return Internal_FindActor(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor(IntPtr obj, string name);

        /// <summary>
        /// Tries to find the actor of the given type in this actor hierarchy (checks this actor and all children hierarchy).
        /// </summary>
        /// <param name="type">Type of the actor to search for. Includes any actors derived from the type.</param>
        /// <returns>Actor instance if found, null otherwise.</returns>
        public Actor FindActor(System.Type type)
        {
            return Internal_FindActor1(unmanagedPtr, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_FindActor1(IntPtr obj, System.Type type);

        /// <summary>
        /// Tries to find the script of the given type in this actor hierarchy (checks this actor and all children hierarchy).
        /// </summary>
        /// <param name="type">Type of the actor to search for. Includes any actors derived from the type.</param>
        /// <returns>Script instance if found, null otherwise.</returns>
        public Script FindScript(System.Type type)
        {
            return Internal_FindScript(unmanagedPtr, type);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_FindScript(IntPtr obj, System.Type type);

        /// <summary>
        /// Try to find actor in hierarchy structure.
        /// </summary>
        /// <param name="a">The actor to find.</param>
        /// <returns>Found actor or null.</returns>
        public bool HasActorInHierarchy(Actor a)
        {
            return Internal_HasActorInHierarchy(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(a));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasActorInHierarchy(IntPtr obj, IntPtr a);

        /// <summary>
        /// Try to find actor in child actors structure.
        /// </summary>
        /// <param name="a">The actor to find.</param>
        /// <returns>Found actor or null.</returns>
        public bool HasActorInChildren(Actor a)
        {
            return Internal_HasActorInChildren(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(a));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasActorInChildren(IntPtr obj, IntPtr a);

        /// <summary>
        /// Determines if there is an intersection between the current object and a Ray.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <returns>True whether the two objects intersected, otherwise false.</returns>
        public bool IntersectsItself(Ray ray, out float distance, out Vector3 normal)
        {
            return Internal_IntersectsItself(unmanagedPtr, ref ray, out distance, out normal);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsItself(IntPtr obj, ref Ray ray, out float distance, out Vector3 normal);

        /// <summary>
        /// Determines if there is an intersection between the current object or any it's child and a ray.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <returns>The target hit actor that is the closest to the ray.</returns>
        public Actor Intersects(Ray ray, out float distance, out Vector3 normal)
        {
            return Internal_Intersects(unmanagedPtr, ref ray, out distance, out normal);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_Intersects(IntPtr obj, ref Ray ray, out float distance, out Vector3 normal);

        /// <summary>
        /// Rotates actor to orient it towards the specified world position.
        /// </summary>
        /// <param name="worldPos">The world position to orient towards.</param>
        public void LookAt(Vector3 worldPos)
        {
            Internal_LookAt(unmanagedPtr, ref worldPos);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LookAt(IntPtr obj, ref Vector3 worldPos);

        /// <summary>
        /// Rotates actor to orient it towards the specified world position with upwards direction.
        /// </summary>
        /// <param name="worldPos">The world position to orient towards.</param>
        /// <param name="worldUp">The up direction that Constrains y axis orientation to a plane this vector lies on. This rule might be broken if forward and up direction are nearly parallel.</param>
        public void LookAt(Vector3 worldPos, Vector3 worldUp)
        {
            Internal_LookAt1(unmanagedPtr, ref worldPos, ref worldUp);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LookAt1(IntPtr obj, ref Vector3 worldPos, ref Vector3 worldUp);

        /// <summary>
        /// Performs actors serialization to the raw bytes.
        /// </summary>
        /// <param name="actors">The actors to serialize.</param>
        /// <returns>The output data, empty if failed.</returns>
        public static byte[] ToBytes(Actor[] actors)
        {
            return Internal_ToBytes(actors, typeof(byte));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_ToBytes(Actor[] actors, System.Type resultArrayItemType0);

        /// <summary>
        /// Performs actors deserialization from the raw bytes.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>The output actors.</returns>
        public static Actor[] FromBytes(byte[] data)
        {
            return Internal_FromBytes(data, typeof(Actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_FromBytes(byte[] data, System.Type resultArrayItemType0);

        /// <summary>
        /// Performs actors deserialization from the raw bytes.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <param name="idsMapping">The serialized objects Ids mapping. Can be used to convert the spawned objects ids and references to them.</param>
        /// <returns>The output actors.</returns>
        public static Actor[] FromBytes(byte[] data, Dictionary<Guid, Guid> idsMapping)
        {
            return Internal_FromBytes1(data, idsMapping, typeof(Actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor[] Internal_FromBytes1(byte[] data, Dictionary<Guid, Guid> idsMapping, System.Type resultArrayItemType0);

        /// <summary>
        /// Tries the get serialized objects ids from the raw bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The output array of serialized object ids.</returns>
        public static Guid[] TryGetSerializedObjectsIds(byte[] data)
        {
            return Internal_TryGetSerializedObjectsIds(data, typeof(Guid));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Guid[] Internal_TryGetSerializedObjectsIds(byte[] data, System.Type resultArrayItemType0);

        /// <summary>
        /// Serializes the actor object to the Json string. Serialized are only this actor properties but no child actors nor scripts. Serializes references to the other objects in a proper way using IDs.
        /// </summary>
        /// <returns>The Json container with serialized actor data.</returns>
        public string ToJson()
        {
            return Internal_ToJson(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_ToJson(IntPtr obj);

        /// <summary>
        /// Deserializes the actor object to the Json string. Deserialized are only this actor properties but no child actors nor scripts.
        /// </summary>
        /// <param name="json">The serialized actor data (state).</param>
        public void FromJson(string json)
        {
            Internal_FromJson(unmanagedPtr, json);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FromJson(IntPtr obj, string json);
    }
}
