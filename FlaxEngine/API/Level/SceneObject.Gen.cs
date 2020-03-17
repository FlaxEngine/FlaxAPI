// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for objects that are parts of the scene (actors and scripts).
    /// </summary>
    [Tooltip("Base class for objects that are parts of the scene (actors and scripts).")]
    public abstract unsafe partial class SceneObject : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected SceneObject() : base()
        {
        }

        /// <summary>
        /// Returns true if object has a parent assigned.
        /// </summary>
        [Tooltip("Returns true if object has a parent assigned.")]
        public bool HasParent
        {
            get { return Internal_HasParent(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasParent(IntPtr obj);

        /// <summary>
        /// Gets or sets the parent actor (or null if object has no parent).
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The parent actor (or null if object has no parent).")]
        public Actor Parent
        {
            get { return Internal_GetParent(unmanagedPtr); }
            set { Internal_SetParent(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetParent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParent(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets or sets zero-based index in parent actor children list (scripts or child actors).
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("Gets zero-based index in parent actor children list (scripts or child actors).")]
        public int OrderInParent
        {
            get { return Internal_GetOrderInParent(unmanagedPtr); }
            set { Internal_SetOrderInParent(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetOrderInParent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOrderInParent(IntPtr obj, int index);

        /// <summary>
        /// Gets a value indicating whether this object has a valid linkage to the prefab asset.
        /// </summary>
        [Tooltip("Gets a value indicating whether this object has a valid linkage to the prefab asset.")]
        public bool HasPrefabLink
        {
            get { return Internal_HasPrefabLink(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasPrefabLink(IntPtr obj);

        /// <summary>
        /// Gets the prefab asset ID. Empty if no prefab link exists.
        /// </summary>
        [Tooltip("The prefab asset ID. Empty if no prefab link exists.")]
        public Guid PrefabID
        {
            get { Internal_GetPrefabID(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPrefabID(IntPtr obj, out Guid resultAsRef);

        /// <summary>
        /// Gets the ID of the object within a prefab that is used for synchronization with this object. Empty if no prefab link exists.
        /// </summary>
        [Tooltip("The ID of the object within a prefab that is used for synchronization with this object. Empty if no prefab link exists.")]
        public Guid PrefabObjectID
        {
            get { Internal_GetPrefabObjectID(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPrefabObjectID(IntPtr obj, out Guid resultAsRef);

        /// <summary>
        /// Sets the parent actor.
        /// </summary>
        /// <param name="value">The new parent.</param>
        /// <param name="canBreakPrefabLink">True if can break prefab link on changing the parent.</param>
        public void SetParent(Actor value, bool canBreakPrefabLink)
        {
            Internal_SetParent1(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value), canBreakPrefabLink);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParent1(IntPtr obj, IntPtr value, bool canBreakPrefabLink);

        /// <summary>
        /// Links scene object instance to the prefab asset and prefab object. Warning! This applies to the only this object (not scripts or child actors).
        /// </summary>
        /// <param name="prefabId">The prefab asset identifier.</param>
        /// <param name="prefabObjectId">The prefab object identifier.</param>
        public void LinkPrefab(Guid prefabId, Guid prefabObjectId)
        {
            Internal_LinkPrefab(unmanagedPtr, ref prefabId, ref prefabObjectId);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LinkPrefab(IntPtr obj, ref Guid prefabId, ref Guid prefabObjectId);

        /// <summary>
        /// Breaks the prefab linkage for this object, all its scripts, and all child actors.
        /// </summary>
        [NoAnimate]
        public void BreakPrefabLink()
        {
            Internal_BreakPrefabLink(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BreakPrefabLink(IntPtr obj);
    }
}
