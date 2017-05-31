////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public static partial class Input
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
