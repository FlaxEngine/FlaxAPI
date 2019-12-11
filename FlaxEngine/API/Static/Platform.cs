// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Threading;

namespace FlaxEngine
{
    public static partial class Platform
    {
        internal static bool _is64Bit;
        internal static bool _isEditor;
        internal static int _mainThreadId;
        internal static PlatformType _platform;

        /// <summary>
        /// Returns true if the current code is executed on a main application thread.
        /// </summary>
        public static bool IsInMainThread
        {
            get { return Thread.CurrentThread.ManagedThreadId == _mainThreadId; }
        }

        /// <summary>
        /// Returns true if is running 64 bit application (otherwise 32 bit).
        /// </summary>
        public static bool Is64bitApp
        {
            get { return _is64Bit; }
        }

        /// <summary>
        /// Returns true if the game is running in the Flax Editor; false if run from any deployment target.
        /// </summary>
        /// <remarks>
        /// Use this property to perform Editor-related actions.
        /// </remarks>
        public static bool IsEditor
        {
            get { return _isEditor; }
        }

        /// <summary>
        /// Gets the platform the game is running on.
        /// </summary>
        /// <remarks>
        /// Use this property to perform platform dependent actions.
        /// </remarks>
        public static PlatformType PlatformType
        {
            get { return _platform; }
        }

        /// <summary>
        /// Gets the DPI setting scale factor (1 is default).
        /// </summary>
        [UnmanagedCall]
#if UNIT_TEST_COMPILANT
        public static float DpiScale => 1.0f;
#else
        public static float DpiScale => Internal_GetDpi() / 96.0f;
#endif
    }
}
