////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
	public static partial class Render2D
	{
	    /// <summary>
	    /// Calls drawing GUI to the texture.
	    /// </summary>
	    /// <param name="context">The GPU context to handle graphics commands.</param>
	    /// <param name="output">The output render target.</param>
	    /// <param name="guiRoot">The root control of the GUI to draw.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    [UnmanagedCall]
	    public static void CallDrawing(GPUContext context, RenderTarget output, Control guiRoot)
	    {
	        if (context == null || output == null || guiRoot == null)
	            throw new ArgumentNullException();

#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
	        if (Internal_DrawBegin(context.unmanagedPtr, output.unmanagedPtr))
	            throw new InvalidOperationException("Cannot perform GUI rendering.");
	        guiRoot.Draw();
	        Internal_DrawEnd();
#endif
	    }

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
	        Internal_DrawSprite(Object.GetUnmanagedPtr(sprite.Atlas), sprite.Index, ref rect, ref color, true);
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
	    public static void DrawSprite(Sprite sprite, Rectangle rect, Color color, bool withAlpha = true)
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
	    internal static extern bool Internal_DrawBegin(IntPtr context, IntPtr output);
        [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern void Internal_DrawEnd();
        [MethodImpl(MethodImplOptions.InternalCall)]
	    internal static extern void Internal_DrawSprite(IntPtr atlas, int index, ref Rectangle rect, ref Color color, bool withAlpha);
#endif
	    #endregion
    }
}
