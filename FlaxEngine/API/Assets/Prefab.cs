// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public partial class Prefab
    {
        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetDefaultInstance(IntPtr obj, ref Guid objectId);
#endif

        #endregion
    }
}
