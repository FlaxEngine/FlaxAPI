// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The off-mesh link objects used to define a custom point-to-point edge within the navigation graph.
    /// An off-mesh connection is a user defined traversable connection made up to two vertices, at least one of which resides within a navigation mesh polygon allowing movement outside the navigation mesh.
    /// </summary>
    public unsafe partial class NavLink : Actor
    {
        /// <inheritdoc />
        protected NavLink() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="NavLink"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static NavLink New()
        {
            return Internal_Create(typeof(NavLink)) as NavLink;
        }

        /// <summary>
        /// The start location which transform is representing link start position. It is defined in local-space of the actor.
        /// </summary>
        [EditorDisplay("Nav Link"), DefaultValue(typeof(Vector3), "0,0,0"), EditorOrder(0)]
        [Tooltip("The start location which transform is representing link start position. It is defined in local-space of the actor.")]
        public Vector3 Start
        {
            get { Internal_GetStart(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetStart(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetStart(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStart(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// The end location which transform is representing link end position. It is defined in local-space of the actor.
        /// </summary>
        [EditorDisplay("Nav Link"), DefaultValue(typeof(Vector3), "0,0,0"), EditorOrder(10)]
        [Tooltip("The end location which transform is representing link end position. It is defined in local-space of the actor.")]
        public Vector3 End
        {
            get { Internal_GetEnd(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetEnd(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetEnd(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnd(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// The maximum radius of the agents that can go through the link.
        /// </summary>
        [EditorDisplay("Nav Link"), DefaultValue(30.0f), EditorOrder(20)]
        [Tooltip("The maximum radius of the agents that can go through the link.")]
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
        /// Flag used to define links that can be traversed in both directions. When set to false the link can only be traversed from start to end.
        /// </summary>
        [EditorDisplay("Nav Link"), DefaultValue(true), EditorOrder(30)]
        [Tooltip("Flag used to define links that can be traversed in both directions. When set to false the link can only be traversed from start to end.")]
        public bool BiDirectional
        {
            get { return Internal_GetBiDirectional(unmanagedPtr); }
            set { Internal_SetBiDirectional(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetBiDirectional(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBiDirectional(IntPtr obj, bool value);
    }
}
