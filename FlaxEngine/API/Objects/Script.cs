// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public abstract partial class Script
    {
        /// <summary>
        /// Gets or sets the world space transformation of the actors owning this script.
        /// </summary>
        [HideInEditor, NoSerialize]
        public Transform Transform
        {
            get => Actor.Transform;
            set => Actor.Transform = value;
        }

        /// <summary>
        /// Gets or sets the local space transformation of the actors owning this script.
        /// </summary>
        [HideInEditor, NoSerialize]
        public Transform LocalTransform
        {
            get => Actor.LocalTransform;
            set => Actor.LocalTransform = value;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LinkPrefab(IntPtr obj, ref Guid prefabObjectId);
    }
}
