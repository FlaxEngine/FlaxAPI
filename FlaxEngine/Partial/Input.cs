// Flax Engine scripting API

using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// The interface to get input information from Flax.
    /// </summary>
    public static partial class Input
    {
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
#endif
        }

        // TODO: maybe reduce amount of calls and let GetKey and GetMosueButtons functions be internal calls?

#if !UNIT_TEST_COMPILANT

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePosition(out Vector2 result);

#endif
    }
}
