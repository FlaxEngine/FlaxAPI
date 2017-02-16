////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents font object that can be using durng text rendering (it uses Font Asset but with precached data for chosen font properties)
    /// </summary>
    public sealed partial class Font : Object
    {
        // TODO: provide ProcessText APIs
        // TODO: provide GetCharacter APIs
        // TODO: expose CharacterEntry and ability to get per font character info

        /// <summary>
        /// Measures minimum size of the rectangle that will be needed to draw given text.
        /// </summary>
        /// <param name="text">Text to test.</param>
        /// <returns>Minimum size for that text and fot to render properly.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public Vector2 MeasureText(string text)
        {
#if UNIT_TEST_COMPILANT
		    throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Vector2 result;
            Internal_MeasureText(unmanagedPtr, text, out result);
            return result;
#endif
        }

        /// <summary>
        /// Calculates character position for given text and character index
        /// </summary>
        /// <param name="text">Input text to test</param>
        /// <param name="index">The text position to get coordinates of</param>
        /// <param name="layout">Layout properties</param>
        /// <returns>Character position (upper left corner which can be used for a caret position)</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public Vector2 GetCharPosition(string text, int index, TextLayoutOptions layout)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Vector2 result;
            Internal_GetCharPosition1(unmanagedPtr, text, index, ref layout, out result);
            return result;
#endif
        }

        /// <summary>
        /// Calculates character position for given text and character index
        /// </summary>
        /// <param name="text">Input text to test</param>
        /// <param name="index">The text position to get coordinates of</param>
        /// <returns>Character position (upper left corner which can be used for a caret position)</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public Vector2 GetCharPosition(string text, int index)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Vector2 result;
            Internal_GetCharPosition2(unmanagedPtr, text, index, out result);
            return result;
#endif
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_MeasureText(IntPtr obj, string text, out Vector2 result);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition1(IntPtr obj, string text, int index, ref TextLayoutOptions layout, out Vector2 result);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition2(IntPtr obj, string text, int index, out Vector2 result);
#endif
    }
}
