// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public partial class Input
    {
        public static event Action<KeyCode> OnKeyPressed;

        /// <summary>
        /// Gets current mouse position in pixel coordinates.
        /// </summary>
        [UnmanagedCall]
        public static Vector2 MousePosition
        {
#if UNIT_TEST_COMPILANT
            get; set;
#else
            get
            {
                Vector2 result;
                Internal_GetMousePosition(out result);
                return result;
            }
            set
            {
                Internal_SetMousePosition(ref value);
            }
#endif
        }

        // TODO: maybe reduce amount of calls and let GetKey and GetMosueButtons functions be internal calls?

#if !UNIT_TEST_COMPILANT

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePosition(out Vector2 result);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMousePosition(ref Vector2 result);
#endif
    }
}
