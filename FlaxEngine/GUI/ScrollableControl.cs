////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.GUI
{
    public class ScrollableControl : ContainerControl
    {
        /// <inheritdoc />
        public ScrollableControl(bool canFocus)
            : base(canFocus)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        /// <inheritdoc />
        public ScrollableControl(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
        }
    }
}
