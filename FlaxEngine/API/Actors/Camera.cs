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
	public sealed partial class Camera
	{
        // TODO: getMainCamera
        // TODO: get/edit camera params
        // TODO: customAspectRatio, customViewport
        // TODO: ConvertMouseToRay

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }
    }
}
