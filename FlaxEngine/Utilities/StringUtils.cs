////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// String utilities class.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Parse hexadecimals digit to value.
        /// </summary>
        /// <param name="c">The hex character.</param>
        /// <returns>Value.</returns>
        public static int HexDigit(char c)
        {
            int result;

            if (c >= '0' && c <= '9')
            {
                result = c - '0';
            }
            else if (c >= 'a' && c <= 'f')
            {
                result = c + 10 - 'a';
            }
            else if (c >= 'A' && c <= 'F')
            {
                result = c + 10 - 'A';
            }
            else
            {
                result = 0;
            }

            return result;
        }

    }
}
