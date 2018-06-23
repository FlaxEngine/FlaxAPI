// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Utilities
{
    public static class GuidExtensions
    {
        public static string ToUpperString(this Guid guid)
        {
            return guid.ToString().ToUpper();
        }

        public static string ToUpperCurlyString(this Guid guid)
        {
            return $"{{{guid.ToString().ToUpper()}}}";
        }
    }
}
