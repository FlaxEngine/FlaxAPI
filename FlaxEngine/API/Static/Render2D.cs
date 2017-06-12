////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	public static partial class Render2D
	{
	    /// <summary>
	    /// Draws sprite.
	    /// </summary>
	    /// <param name="sprite">Sprite to draw.</param>
	    /// <param name="rect">Rectangle to draw.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    [UnmanagedCall]
	    public static void DrawSprite(Sprite sprite, Rectangle rect)
	    {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Color color = Color.White;
	        Internal_DrawSprite(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color, false);
#endif
	    }

	    /// <summary>
	    /// Draws sprite.
	    /// </summary>
	    /// <param name="sprite">Sprite to draw.</param>
	    /// <param name="rect">Rectangle to draw.</param>
	    /// <param name="color">Color to multiply all texture pixels.</param>
	    /// <param name="withAlpha">True if use alpha blending, otherwise it will be disabled.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    [UnmanagedCall]
	    public static void DrawSprite(Sprite sprite, Rectangle rect, Color color, bool withAlpha = false)
	    {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
	        Internal_DrawSprite(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color, withAlpha);
#endif
	    }

        #region Internal Calls
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern void Internal_DrawSprite(IntPtr atlas, int index, ref Rectangle rect, ref Color color, bool withAlpha);
#endif
	    #endregion
    }
}
