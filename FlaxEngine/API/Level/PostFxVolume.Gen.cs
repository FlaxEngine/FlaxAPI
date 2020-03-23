// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A special type of volume that blends custom set of post process settings into the rendering.
    /// </summary>
    [Tooltip("A special type of volume that blends custom set of post process settings into the rendering.")]
    public unsafe partial class PostFxVolume : Actor
    {
        /// <inheritdoc />
        protected PostFxVolume() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="PostFxVolume"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static PostFxVolume New()
        {
            return Internal_Create(typeof(PostFxVolume)) as PostFxVolume;
        }

        /// <summary>
        /// The ambient occlusion effect settings.
        /// </summary>
        [EditorDisplay("Ambient Occlusion"), EditorOrder(100)]
        [Tooltip("The ambient occlusion effect settings.")]
        public AmbientOcclusionSettings AmbientOcclusion
        {
            get { Internal_GetAmbientOcclusion(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetAmbientOcclusion(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetAmbientOcclusion(IntPtr obj, out AmbientOcclusionSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAmbientOcclusion(IntPtr obj, ref AmbientOcclusionSettings value);

        /// <summary>
        /// The bloom effect settings.
        /// </summary>
        [EditorDisplay("Bloom"), EditorOrder(200)]
        [Tooltip("The bloom effect settings.")]
        public BloomSettings Bloom
        {
            get { Internal_GetBloom(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetBloom(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBloom(IntPtr obj, out BloomSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBloom(IntPtr obj, ref BloomSettings value);

        /// <summary>
        /// The tone mapping effect settings.
        /// </summary>
        [EditorDisplay("Tone Mapping"), EditorOrder(300)]
        [Tooltip("The tone mapping effect settings.")]
        public ToneMappingSettings ToneMapping
        {
            get { Internal_GetToneMapping(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetToneMapping(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetToneMapping(IntPtr obj, out ToneMappingSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetToneMapping(IntPtr obj, ref ToneMappingSettings value);

        /// <summary>
        /// The color grading effect settings.
        /// </summary>
        [EditorDisplay("Color Grading"), EditorOrder(400)]
        [Tooltip("The color grading effect settings.")]
        public ColorGradingSettings ColorGrading
        {
            get { Internal_GetColorGrading(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetColorGrading(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetColorGrading(IntPtr obj, out ColorGradingSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetColorGrading(IntPtr obj, ref ColorGradingSettings value);

        /// <summary>
        /// The eye adaptation effect settings.
        /// </summary>
        [EditorDisplay("Eye Adaptation"), EditorOrder(500)]
        [Tooltip("The eye adaptation effect settings.")]
        public EyeAdaptationSettings EyeAdaptation
        {
            get { Internal_GetEyeAdaptation(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetEyeAdaptation(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetEyeAdaptation(IntPtr obj, out EyeAdaptationSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEyeAdaptation(IntPtr obj, ref EyeAdaptationSettings value);

        /// <summary>
        /// The camera artifacts effect settings.
        /// </summary>
        [EditorDisplay("Camera Artifacts"), EditorOrder(600)]
        [Tooltip("The camera artifacts effect settings.")]
        public CameraArtifactsSettings CameraArtifacts
        {
            get { Internal_GetCameraArtifacts(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetCameraArtifacts(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCameraArtifacts(IntPtr obj, out CameraArtifactsSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCameraArtifacts(IntPtr obj, ref CameraArtifactsSettings value);

        /// <summary>
        /// The lens flares effect settings.
        /// </summary>
        [EditorDisplay("Lens Flares"), EditorOrder(700)]
        [Tooltip("The lens flares effect settings.")]
        public LensFlaresSettings LensFlares
        {
            get { Internal_GetLensFlares(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLensFlares(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLensFlares(IntPtr obj, out LensFlaresSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLensFlares(IntPtr obj, ref LensFlaresSettings value);

        /// <summary>
        /// The depth of field effect settings.
        /// </summary>
        [EditorDisplay("Depth Of Field"), EditorOrder(800)]
        [Tooltip("The depth of field effect settings.")]
        public DepthOfFieldSettings DepthOfField
        {
            get { Internal_GetDepthOfField(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDepthOfField(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDepthOfField(IntPtr obj, out DepthOfFieldSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDepthOfField(IntPtr obj, ref DepthOfFieldSettings value);

        /// <summary>
        /// The motion blur effect settings.
        /// </summary>
        [EditorDisplay("Motion Blur"), EditorOrder(900)]
        [Tooltip("The motion blur effect settings.")]
        public MotionBlurSettings MotionBlur
        {
            get { Internal_GetMotionBlur(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetMotionBlur(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMotionBlur(IntPtr obj, out MotionBlurSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMotionBlur(IntPtr obj, ref MotionBlurSettings value);

        /// <summary>
        /// The screen space reflections effect settings.
        /// </summary>
        [EditorDisplay("Screen Space Reflections"), EditorOrder(1000)]
        [Tooltip("The screen space reflections effect settings.")]
        public ScreenSpaceReflectionsSettings ScreenSpaceReflections
        {
            get { Internal_GetScreenSpaceReflections(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetScreenSpaceReflections(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetScreenSpaceReflections(IntPtr obj, out ScreenSpaceReflectionsSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScreenSpaceReflections(IntPtr obj, ref ScreenSpaceReflectionsSettings value);

        /// <summary>
        /// The anti-aliasing effect settings.
        /// </summary>
        [EditorDisplay("Anti Aliasing"), EditorOrder(1100)]
        [Tooltip("The anti-aliasing effect settings.")]
        public AntiAliasingSettings AntiAliasing
        {
            get { Internal_GetAntiAliasing(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetAntiAliasing(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetAntiAliasing(IntPtr obj, out AntiAliasingSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAntiAliasing(IntPtr obj, ref AntiAliasingSettings value);

        /// <summary>
        /// The PostFx materials rendering settings.
        /// </summary>
        [EditorDisplay("PostFx Materials"), NoAnimate, EditorOrder(1200)]
        [Tooltip("The PostFx materials rendering settings.")]
        public PostFxMaterialsSettings PostFxMaterials
        {
            get { Internal_GetPostFxMaterials(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetPostFxMaterials(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPostFxMaterials(IntPtr obj, out PostFxMaterialsSettings resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPostFxMaterials(IntPtr obj, ref PostFxMaterialsSettings value);

        /// <summary>
        /// Gets or sets the size of the volume (in the local space of the actor).
        /// </summary>
        [EditorDisplay("PostFx Volume"), EditorOrder(50)]
        [Tooltip("The size of the volume (in the local space of the actor).")]
        public Vector3 Size
        {
            get { Internal_GetSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetSize(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSize(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the order in which multiple volumes are blended together.
        /// The volume with the highest priority takes precedence over all other overlapping volumes.
        /// </summary>
        [EditorDisplay("PostFx Volume"), EditorOrder(60)]
        public int Priority
        {
            get { return Internal_GetPriority(unmanagedPtr); }
            set { Internal_SetPriority(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetPriority(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPriority(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the distance inside the volume at which blending with the volume's settings occurs.
        /// </summary>
        [EditorDisplay("PostFx Volume"), EditorOrder(70)]
        [Tooltip("The distance inside the volume at which blending with the volume's settings occurs.")]
        public float BlendRadius
        {
            get { return Internal_GetBlendRadius(unmanagedPtr); }
            set { Internal_SetBlendRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBlendRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBlendRadius(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the amount of influence the volume's properties have. 0 is no effect; 1 is full effect.
        /// </summary>
        [EditorDisplay("PostFx Volume"), EditorOrder(80)]
        [Tooltip("The amount of influence the volume's properties have. 0 is no effect; 1 is full effect.")]
        public float BlendWeight
        {
            get { return Internal_GetBlendWeight(unmanagedPtr); }
            set { Internal_SetBlendWeight(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBlendWeight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBlendWeight(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the value indicating whether the bounds of the volume are taken into account.
        /// If false, the volume affects the entire world, regardless of its bounds.
        /// If true, the volume only has an effect within its bounds.
        /// </summary>
        [EditorDisplay("PostFx Volume"), EditorOrder(90)]
        public bool IsBounded
        {
            get { return Internal_GetIsBounded(unmanagedPtr); }
            set { Internal_SetIsBounded(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsBounded(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsBounded(IntPtr obj, bool value);

        /// <summary>
        /// Gets the volume bounding box (oriented).
        /// </summary>
        [Tooltip("The volume bounding box (oriented).")]
        public OrientedBoundingBox OrientedBox
        {
            get { Internal_GetOrientedBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientedBox(IntPtr obj, out OrientedBoundingBox resultAsRef);

        /// <summary>
        /// Adds the post fx material to the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public void AddPostFxMaterial(MaterialBase material)
        {
            Internal_AddPostFxMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(material));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddPostFxMaterial(IntPtr obj, IntPtr material);

        /// <summary>
        /// Removes the post fx material from the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public void RemovePostFxMaterial(MaterialBase material)
        {
            Internal_RemovePostFxMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(material));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemovePostFxMaterial(IntPtr obj, IntPtr material);
    }
}
