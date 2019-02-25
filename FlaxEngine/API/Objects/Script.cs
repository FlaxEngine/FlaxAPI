// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public abstract partial class Script : ISceneObject
    {
        /// <summary>
        /// Gets the scene object which contains this script.
        /// </summary>
        [HideInEditor, NoSerialize]
        public Scene Scene
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get
            {
                var parent = Actor;
                return parent ? parent.Scene : null;
            }
#endif
        }

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

        /// <summary>
        /// Tries to find the script of the given type in all the loaded scenes.
        /// </summary>
        /// <typeparam name="T">The type of the script to find.</typeparam>
        /// <returns>Script instance if found, null otherwise.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static T Find<T>() where T : Script
        {
#if UNIT_TEST_COMPILANT
            return null;
#else
            return Internal_FindByType(typeof(T)) as T;
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_LinkPrefab(IntPtr obj, ref Guid prefabId, ref Guid prefabObjectId);
#endif

        #endregion
    }
}
