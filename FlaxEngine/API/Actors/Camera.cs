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
	public sealed partial class Camera
	{
        // TODO: getMainCamera
        // TODO: customAspectRatio, customViewport
        // TODO: ConvertMouseToRay

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }

        // Hacky internal call to get proper camera preview model intersection (works only in editor)
	    [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern bool Internal_IntersectsItselfEditor(IntPtr obj, ref Ray ray, out float distance);
    }
}
