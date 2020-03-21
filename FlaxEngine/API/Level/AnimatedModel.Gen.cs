// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Performs an animation and renders a skinned model.
    /// </summary>
    [Tooltip("Performs an animation and renders a skinned model.")]
    public unsafe partial class AnimatedModel : ModelInstanceActor
    {
        /// <inheritdoc />
        protected AnimatedModel() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="AnimatedModel"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static AnimatedModel New()
        {
            return Internal_Create(typeof(AnimatedModel)) as AnimatedModel;
        }

        /// <summary>
        /// The skinned model asset used for rendering.
        /// </summary>
        [EditorOrder(10), DefaultValue(null), EditorDisplay("Skinned Model")]
        [Tooltip("The skinned model asset used for rendering.")]
        public SkinnedModel SkinnedModel
        {
            get { return Internal_GetSkinnedModel(unmanagedPtr); }
            set { Internal_SetSkinnedModel(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedModel Internal_GetSkinnedModel(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSkinnedModel(IntPtr obj, IntPtr value);

        /// <summary>
        /// The animation graph asset used for the skinned mesh skeleton bones evaluation (controls the animation).
        /// </summary>
        [EditorOrder(15), DefaultValue(null), EditorDisplay("Skinned Model")]
        [Tooltip("The animation graph asset used for the skinned mesh skeleton bones evaluation (controls the animation).")]
        public AnimationGraph AnimationGraph
        {
            get { return Internal_GetAnimationGraph(unmanagedPtr); }
            set { Internal_SetAnimationGraph(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AnimationGraph Internal_GetAnimationGraph(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAnimationGraph(IntPtr obj, IntPtr value);

        /// <summary>
        /// If true, use per-bone motion blur on this skeletal model. It requires additional rendering, can be disabled to save performance.
        /// </summary>
        [EditorOrder(20), DefaultValue(true), EditorDisplay("Skinned Model")]
        [Tooltip("If true, use per-bone motion blur on this skeletal model. It requires additional rendering, can be disabled to save performance.")]
        public bool PerBoneMotionBlur
        {
            get { return Internal_GetPerBoneMotionBlur(unmanagedPtr); }
            set { Internal_SetPerBoneMotionBlur(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetPerBoneMotionBlur(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPerBoneMotionBlur(IntPtr obj, bool value);

        /// <summary>
        /// If true, animation speed will be affected by the global time scale parameter.
        /// </summary>
        [EditorOrder(30), DefaultValue(true), EditorDisplay("Skinned Model")]
        [Tooltip("If true, animation speed will be affected by the global time scale parameter.")]
        public bool UseTimeScale
        {
            get { return Internal_GetUseTimeScale(unmanagedPtr); }
            set { Internal_SetUseTimeScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseTimeScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseTimeScale(IntPtr obj, bool value);

        /// <summary>
        /// If true, the animation will be updated even when an actor cannot be seen by any camera. Otherwise, the animations themselves will also stop running when the actor is off-screen.
        /// </summary>
        [EditorOrder(40), DefaultValue(false), EditorDisplay("Skinned Model")]
        [Tooltip("If true, the animation will be updated even when an actor cannot be seen by any camera. Otherwise, the animations themselves will also stop running when the actor is off-screen.")]
        public bool UpdateWhenOffscreen
        {
            get { return Internal_GetUpdateWhenOffscreen(unmanagedPtr); }
            set { Internal_SetUpdateWhenOffscreen(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUpdateWhenOffscreen(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateWhenOffscreen(IntPtr obj, bool value);

        /// <summary>
        /// The animation update mode. Can be used to optimize the performance.
        /// </summary>
        [EditorOrder(50), DefaultValue(AnimationUpdateMode.Auto), EditorDisplay("Skinned Model")]
        [Tooltip("The animation update mode. Can be used to optimize the performance.")]
        public AnimationUpdateMode UpdateMode
        {
            get { return Internal_GetUpdateMode(unmanagedPtr); }
            set { Internal_SetUpdateMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AnimationUpdateMode Internal_GetUpdateMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUpdateMode(IntPtr obj, AnimationUpdateMode value);

        /// <summary>
        /// The master scale parameter for the actor bounding box. Helps reducing mesh flickering effect on screen edges.
        /// </summary>
        [EditorOrder(60), DefaultValue(1.5f), Limit(0), EditorDisplay("Skinned Model")]
        [Tooltip("The master scale parameter for the actor bounding box. Helps reducing mesh flickering effect on screen edges.")]
        public float BoundsScale
        {
            get { return Internal_GetBoundsScale(unmanagedPtr); }
            set { Internal_SetBoundsScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBoundsScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBoundsScale(IntPtr obj, float value);

        /// <summary>
        /// The custom bounds(in actor local space). If set to empty bounds then source skinned model bind pose bounds will be used.
        /// </summary>
        [EditorOrder(70), EditorDisplay("Skinned Model")]
        [Tooltip("The custom bounds(in actor local space). If set to empty bounds then source skinned model bind pose bounds will be used.")]
        public BoundingBox CustomBounds
        {
            get { Internal_GetCustomBounds(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetCustomBounds(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCustomBounds(IntPtr obj, out BoundingBox resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomBounds(IntPtr obj, ref BoundingBox value);

        /// <summary>
        /// The draw passes to use for rendering this object.
        /// </summary>
        [EditorOrder(75), DefaultValue(DrawPass.Default), EditorDisplay("Skinned Model")]
        [Tooltip("The draw passes to use for rendering this object.")]
        public DrawPass DrawModes
        {
            get { return Internal_GetDrawModes(unmanagedPtr); }
            set { Internal_SetDrawModes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DrawPass Internal_GetDrawModes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawModes(IntPtr obj, DrawPass value);

        /// <summary>
        /// The shadows casting mode.
        /// </summary>
        [EditorOrder(80), DefaultValue(ShadowsCastingMode.All), EditorDisplay("Skinned Model")]
        [Tooltip("The shadows casting mode.")]
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);

        /// <summary>
        /// The animation root motion apply target. If not specified the animated model will apply it itself.
        /// </summary>
        [EditorOrder(100), DefaultValue(null), EditorDisplay("Skinned Model")]
        [Tooltip("The animation root motion apply target. If not specified the animated model will apply it itself.")]
        public Actor RootMotionTarget
        {
            get { return Internal_GetRootMotionTarget(unmanagedPtr); }
            set { Internal_SetRootMotionTarget(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetRootMotionTarget(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRootMotionTarget(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets the anim graph instance parameters collection.
        /// </summary>
        [Tooltip("The anim graph instance parameters collection.")]
        public AnimGraphParameter[] Parameters
        {
            get { return Internal_GetParameters(unmanagedPtr, typeof(AnimGraphParameter)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AnimGraphParameter[] Internal_GetParameters(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Resets the animation state (clears the instance state data but preserves the instance parameters values).
        /// </summary>
        public void ResetAnimation()
        {
            Internal_ResetAnimation(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetAnimation(IntPtr obj);

        /// <summary>
        /// Performs the full animation update.
        /// </summary>
        public void UpdateAnimation()
        {
            Internal_UpdateAnimation(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateAnimation(IntPtr obj);

        /// <summary>
        /// Validates and creates a proper skinning data.
        /// </summary>
        public void SetupSkinningData()
        {
            Internal_SetupSkinningData(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetupSkinningData(IntPtr obj);

        /// <summary>
        /// Creates and setups the skinning data (writes the identity bones transformations).
        /// </summary>
        public void PreInitSkinningData()
        {
            Internal_PreInitSkinningData(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_PreInitSkinningData(IntPtr obj);

        /// <summary>
        /// Updates the child bone socket actors.
        /// </summary>
        public void UpdateSockets()
        {
            Internal_UpdateSockets(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateSockets(IntPtr obj);

        /// <summary>
        /// Gets the per-node final transformations in actor local-space.
        /// </summary>
        /// <param name="nodesTransformation">The output per-node final transformation matrices in actor local-space.</param>
        public void GetCurrentPose(out Matrix[] nodesTransformation)
        {
            Internal_GetCurrentPose(unmanagedPtr, out nodesTransformation, typeof(Matrix));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCurrentPose(IntPtr obj, out Matrix[] nodesTransformation, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the anim graph instance parameter value.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns>The value.</returns>
        public object GetParameterValue(string name)
        {
            return Internal_GetParameterValue(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetParameterValue(IntPtr obj, string name);

        /// <summary>
        /// Sets the anim graph instance parameter value.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The value to set.</param>
        public void SetParameterValue(string name, object value)
        {
            Internal_SetParameterValue(unmanagedPtr, name, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParameterValue(IntPtr obj, string name, object value);

        /// <summary>
        /// Gets the anim graph instance parameter value.
        /// </summary>
        /// <param name="id">The parameter id.</param>
        /// <returns>The value.</returns>
        public object GetParameterValue(Guid id)
        {
            return Internal_GetParameterValue1(unmanagedPtr, ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetParameterValue1(IntPtr obj, ref Guid id);

        /// <summary>
        /// Sets the anim graph instance parameter value.
        /// </summary>
        /// <param name="id">The parameter id.</param>
        /// <param name="value">The value to set.</param>
        public void SetParameterValue(Guid id, object value)
        {
            Internal_SetParameterValue1(unmanagedPtr, ref id, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParameterValue1(IntPtr obj, ref Guid id, object value);

        /// <summary>
        /// Describes the animation graph updates frequency for the animated model.
        /// </summary>
        [Tooltip("Describes the animation graph updates frequency for the animated model.")]
        public enum AnimationUpdateMode
        {
            /// <summary>
            /// The automatic updates will be used (based on platform capabilities, distance to the player, etc.).
            /// </summary>
            [Tooltip("The automatic updates will be used (based on platform capabilities, distance to the player, etc.).")]
            Auto = 0,

            /// <summary>
            /// Animation will be updated every game update.
            /// </summary>
            [Tooltip("Animation will be updated every game update.")]
            EveryUpdate = 1,

            /// <summary>
            /// Animation will be updated every second game update.
            /// </summary>
            [Tooltip("Animation will be updated every second game update.")]
            EverySecondUpdate = 2,

            /// <summary>
            /// Animation will be updated every fourth game update.
            /// </summary>
            [Tooltip("Animation will be updated every fourth game update.")]
            EveryFourthUpdate = 3,

            /// <summary>
            /// Animation can be updated manually by the user scripts.
            /// </summary>
            [Tooltip("Animation can be updated manually by the user scripts.")]
            Manual = 4,

            /// <summary>
            /// Animation won't be updated at all.
            /// </summary>
            [Tooltip("Animation won't be updated at all.")]
            Never = 5,
        }
    }
}
