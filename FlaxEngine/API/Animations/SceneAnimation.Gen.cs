// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Scene animation timeline for animating objects and playing cut-scenes.
    /// </summary>
    [Tooltip("Scene animation timeline for animating objects and playing cut-scenes.")]
    public sealed unsafe partial class SceneAnimation : BinaryAsset
    {
        private SceneAnimation() : base()
        {
        }

        /// <summary>
        /// The frames amount per second of the timeline animation.
        /// </summary>
        [Tooltip("The frames amount per second of the timeline animation.")]
        public float FramesPerSecond
        {
            get { return Internal_GetFramesPerSecond(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFramesPerSecond(IntPtr obj);

        /// <summary>
        /// The animation duration (in frames).
        /// </summary>
        [Tooltip("The animation duration (in frames).")]
        public int DurationFrames
        {
            get { return Internal_GetDurationFrames(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetDurationFrames(IntPtr obj);

        /// <summary>
        /// Gets the animation duration (in seconds).
        /// </summary>
        [Tooltip("The animation duration (in seconds).")]
        public float Duration
        {
            get { return Internal_GetDuration(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDuration(IntPtr obj);

        /// <summary>
        /// Gets the serialized timeline data.
        /// </summary>
        /// <returns>The output timeline data container. Empty if failed to load.</returns>
        public byte[] LoadTimeline()
        {
            return Internal_LoadTimeline(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadTimeline(IntPtr obj);

        /// <summary>
        /// Saves the serialized timeline data to the asset.
        /// </summary>
        /// <remarks>
        /// The cannot be used by virtual assets.
        /// </remarks>
        /// <param name="data">The timeline data container.</param>
        /// <returns><c>true</c> failed to save data; otherwise, <c>false</c>.</returns>
        public bool SaveTimeline(byte[] data)
        {
            return Internal_SaveTimeline(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveTimeline(IntPtr obj, byte[] data);
    }
}
