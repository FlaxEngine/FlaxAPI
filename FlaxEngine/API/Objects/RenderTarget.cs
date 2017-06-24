////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.Rendering
{
    public partial class RenderTarget
    {
        /// <summary>
        /// Returns true if texture has size that is power of two.
        /// </summary>
        public bool IsPowerOfTwwo
        {
            get { return Mathf.IsPowerOfTwo(Width) && Mathf.IsPowerOfTwo(Height); }
        }
    }
}
