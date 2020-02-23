// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Actor that links to the animated model skeleton node transformation.
    /// </summary>
    [Tooltip("Actor that links to the animated model skeleton node transformation.")]
    public partial class BoneSocket : Actor
    {
        /// <inheritdoc />
        protected BoneSocket() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="BoneSocket"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static BoneSocket New()
        {
            return Internal_Create(typeof(BoneSocket)) as BoneSocket;
        }

        /// <summary>
        /// Gets or sets the target node name to link to it.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Bone Socket"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.SkeletonNodeEditor")]
        [Tooltip("The target node name to link to it.")]
        public string Node
        {
            get { return Internal_GetNode(unmanagedPtr); }
            set { Internal_SetNode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetNode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetNode(IntPtr obj, string name);

        /// <summary>
        /// Gets or sets the value indicating whenever use the target node scale. Otherwise won't override the actor scale.
        /// </summary>
        [EditorOrder(20), EditorDisplay("Bone Socket"), DefaultValue(false)]
        [Tooltip("The value indicating whenever use the target node scale. Otherwise won't override the actor scale.")]
        public bool UseScale
        {
            get { return Internal_GetUseScale(unmanagedPtr); }
            set { Internal_SetUseScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseScale(IntPtr obj, bool value);

        /// <summary>
        /// Updates the actor transformation based on a skeleton node.
        /// </summary>
        public void UpdateTransformation()
        {
            Internal_UpdateTransformation(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateTransformation(IntPtr obj);
    }
}
