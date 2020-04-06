// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class from which every every script derives.
    /// </summary>
    public abstract partial class Script : SceneObject, ISceneObject
    {
        /// <summary>
        /// Gets the scene object which contains this script.
        /// </summary>
        [HideInEditor, NoSerialize]
        public Scene Scene
        {
            get
            {
                var parent = Actor;
                return parent ? parent.Scene : null;
            }
        }

        /// <summary>
        /// Gets or sets the world space transformation of the actors owning this script.
        /// </summary>
        [HideInEditor, NoSerialize, NoAnimate]
        public Transform Transform
        {
            get => Actor.Transform;
            set => Actor.Transform = value;
        }

        /// <summary>
        /// Gets or sets the local space transformation of the actors owning this script.
        /// </summary>
        [HideInEditor, NoSerialize, NoAnimate]
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
        public static T Find<T>() where T : Script
        {
            return Internal_FindByType(typeof(T)) as T;
        }

        /// <summary>
        /// Called after the object is loaded.
        /// </summary>
        [NoAnimate]
        public virtual void OnAwake()
        {
        }

        /// <summary>
        /// Called when object becomes enabled and active.
        /// </summary>
        [NoAnimate]
        public virtual void OnEnable()
        {
        }

        /// <summary>
        /// Called when object becomes disabled and inactive.
        /// </summary>
        [NoAnimate]
        public virtual void OnDisable()
        {
        }

        /// <summary>
        /// Called before the object will be destroyed..
        /// </summary>
        [NoAnimate]
        public virtual void OnDestroy()
        {
        }

        /// <summary>
        /// Called when a script is enabled just before any of the Update methods is called for the first time.
        /// </summary>
        [NoAnimate]
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// Called every frame if object is enabled.
        /// </summary>
        [NoAnimate]
        public virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Called every frame (after gameplay Update) if object is enabled.
        /// </summary>
        [NoAnimate]
        public virtual void OnLateUpdate()
        {
        }

        /// <summary>
        /// Called every fixed framerate frame if object is enabled.
        /// </summary>
        [NoAnimate]
        public virtual void OnFixedUpdate()
        {
        }

        /// <summary>
        /// Called during drawing debug shapes in editor. Use <see cref="DebugDraw"/> to draw debug shapes and other visualization.
        /// </summary>
        [NoAnimate]
        public virtual void OnDebugDraw()
        {
        }

        /// <summary>
        /// Called during drawing debug shapes in editor when object is selected. Use <see cref="DebugDraw"/> to draw debug shapes and other visualization.
        /// </summary>
        [NoAnimate]
        public virtual void OnDebugDrawSelected()
        {
        }

        /// <summary>
        /// Enable/disable script updates.
        /// </summary>
        [HideInEditor]
        public bool Enabled
        {
            get { return Internal_GetEnabled(unmanagedPtr); }
            set { Internal_SetEnabled(unmanagedPtr, value); }
        }

        /// <summary>
        /// Gets or sets the actor owning that script.
        /// </summary>
        /// <remarks>
        /// Changing script parent breaks any existing prefab links.
        /// </remarks>
        [HideInEditor, NoAnimate]
        public Actor Actor
        {
            get { return Internal_GetActor(unmanagedPtr); }
            set { Internal_SetActor(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        /// <summary>
        /// Tries to find the script of the given type in all the loaded scenes.
        /// </summary>
        /// <param name="type">The type of the script to find.</param>
        /// <returns>Script instance if found, null otherwise.</returns>
        public static Script Find(Type type)
        {
            return Internal_FindByType(type);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnabled(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnabled(IntPtr obj, bool val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetActor(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetActor(IntPtr obj, IntPtr val);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Script Internal_FindByType(Type type);
#endif

        #endregion
    }
}
