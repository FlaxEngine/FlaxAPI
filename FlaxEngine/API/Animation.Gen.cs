// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Asset that contains an animation spline represented by a set of keyframes, each representing an endpoint of a linear curve.
    /// </summary>
    [Tooltip("Asset that contains an animation spline represented by a set of keyframes, each representing an endpoint of a linear curve.")]
    public partial class Animation : BinaryAsset
    {
        /// <inheritdoc />
        protected Animation() : base()
        {
        }

        /// <summary>
        /// Gets the length of the animation (in seconds).
        /// </summary>
        [Tooltip("The length of the animation (in seconds).")]
        public float Length
        {
            get { return Internal_GetLength(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetLength(IntPtr obj);

        /// <summary>
        /// Gets the duration of the animation (in frames).
        /// </summary>
        [Tooltip("The duration of the animation (in frames).")]
        public float Duration
        {
            get { return Internal_GetDuration(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDuration(IntPtr obj);

        /// <summary>
        /// Gets the amount of the animation frames per second.
        /// </summary>
        [Tooltip("The amount of the animation frames per second.")]
        public float FramesPerSecond
        {
            get { return Internal_GetFramesPerSecond(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFramesPerSecond(IntPtr obj);

        /// <summary>
        /// Gets the animation clip info.
        /// </summary>
        [Tooltip("The animation clip info.")]
        public InfoData Info
        {
            get { Internal_GetInfo(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInfo(IntPtr obj, out InfoData resultAsRef);

        /// <summary>
        /// Contains basic information about the animation asset contents.
        /// </summary>
        [Tooltip("Contains basic information about the animation asset contents.")]
        [StructLayout(LayoutKind.Sequential)]
        public struct InfoData
        {
            /// <summary>
            /// Length of the animation in seconds.
            /// </summary>
            [Tooltip("Length of the animation in seconds.")]
            public float Length;

            /// <summary>
            /// Amount of animation frames (some curve tracks may use less keyframes).
            /// </summary>
            [Tooltip("Amount of animation frames (some curve tracks may use less keyframes).")]
            public int FramesCount;

            /// <summary>
            /// Amount of animation channel tracks.
            /// </summary>
            [Tooltip("Amount of animation channel tracks.")]
            public int ChannelsCount;

            /// <summary>
            /// The total amount of keyframes in the animation tracks.
            /// </summary>
            [Tooltip("The total amount of keyframes in the animation tracks.")]
            public int KeyframesCount;
        }
    }
}
