////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	public sealed partial class Camera
	{
        // TODO: ConvertMouseToRay

	    /// <summary>
	    /// Calculates the view and the projection matrices for the camera. Support using custom viewport.
	    /// </summary>
	    /// <param name="view">The result camera view matrix.</param>
	    /// <param name="projection">The result camera projection matrix.</param>
	    public void GetMatrices(out Matrix view, out Matrix projection)
	    {
	        Viewport emptyViewport = new Viewport(0, 0, 0, 0);
            Internal_GetMatrices(unmanagedPtr, out view, out projection, ref emptyViewport);
	    }

	    /// <summary>
	    /// Calculates the view and the projection matrices for the camera. Support using custom viewport.
	    /// </summary>
	    /// <param name="view">The result camera view matrix.</param>
	    /// <param name="projection">The result camera projection matrix.</param>
	    /// <param name="customViewport">The custom output viewport.</param>
	    public void GetMatrices(out Matrix view, out Matrix projection, ref Viewport customViewport)
	    {
	        Internal_GetMatrices(unmanagedPtr, out view, out projection, ref customViewport);
	    }

	    /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }

        // Hacky internal call to get proper camera preview model intersection (works only in editor)
	    [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern bool Internal_IntersectsItselfEditor(IntPtr obj, ref Ray ray, out float distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern bool Internal_GetMatrices(IntPtr obj, out Matrix view, out Matrix projection, ref Viewport customViewport);
    }
}
