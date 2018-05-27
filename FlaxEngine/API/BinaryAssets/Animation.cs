// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class Animation
    {
        /// <summary>
        /// Contains basic information about the animation asset contents.
        /// </summary>
        public struct Info
        {
            /// <summary>
            /// Length of the animation in seconds.
            /// </summary>
            public float Length;

            /// <summary>
            /// Amount of animation frames (some curve tracks may use less keyframes).
            /// </summary>
            public int FramesCount;

            /// <summary>
            /// Amount of animation channel tracks.
            /// </summary>
            public int ChannelsCount;

            /// <summary>
            /// The total amount of keyframes in the animation tracks.
            /// </summary>
            public int KeyframesCount;
        }

        /// <summary>
        /// Gets the animation contents information.
        /// </summary>
        /// <param name="info">The output data with info.</param>
        public void GetInfo(out Info info)
        {
            Internal_GetInfo(unmanagedPtr, out info);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInfo(IntPtr obj, out Info info);
#endif

        #endregion
    }
}
