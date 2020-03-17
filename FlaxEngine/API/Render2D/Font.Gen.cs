// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The text range.
    /// </summary>
    [Tooltip("The text range.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct TextRange
    {
        /// <summary>
        /// The start index.
        /// </summary>
        [Tooltip("The start index.")]
        public int StartIndex;

        /// <summary>
        /// The end index.
        /// </summary>
        [Tooltip("The end index.")]
        public int EndIndex;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The font line info generated during text processing.
    /// </summary>
    [Tooltip("The font line info generated during text processing.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct FontLineCache
    {
        /// <summary>
        /// The root position of the line (upper left corner).
        /// </summary>
        [Tooltip("The root position of the line (upper left corner).")]
        public Vector2 Location;

        /// <summary>
        /// The line bounds (width and height).
        /// </summary>
        [Tooltip("The line bounds (width and height).")]
        public Vector2 Size;

        /// <summary>
        /// The first character index (from the input text).
        /// </summary>
        [Tooltip("The first character index (from the input text).")]
        public int FirstCharIndex;

        /// <summary>
        /// The last character index (from the input text).
        /// </summary>
        [Tooltip("The last character index (from the input text).")]
        public int LastCharIndex;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The cached font character entry (read for rendering and further processing).
    /// </summary>
    [Tooltip("The cached font character entry (read for rendering and further processing).")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct FontCharacterEntry
    {
        /// <summary>
        /// The character represented by this entry.
        /// </summary>
        [Tooltip("The character represented by this entry.")]
        public char Character;

        /// <summary>
        /// True if entry is valid, otherwise false.
        /// </summary>
        [Tooltip("True if entry is valid, otherwise false.")]
        public bool IsValid;

        /// <summary>
        /// The index to a specific texture in the font cache.
        /// </summary>
        [Tooltip("The index to a specific texture in the font cache.")]
        public byte TextureIndex;

        /// <summary>
        /// The left bearing expressed in integer pixels.
        /// </summary>
        [Tooltip("The left bearing expressed in integer pixels.")]
        public short OffsetX;

        /// <summary>
        /// The top bearing expressed in integer pixels.
        /// </summary>
        [Tooltip("The top bearing expressed in integer pixels.")]
        public short OffsetY;

        /// <summary>
        /// The amount to advance in X before drawing the next character in a string.
        /// </summary>
        [Tooltip("The amount to advance in X before drawing the next character in a string.")]
        public short AdvanceX;

        /// <summary>
        /// The distance from baseline to glyph top most point.
        /// </summary>
        [Tooltip("The distance from baseline to glyph top most point.")]
        public short BearingY;

        /// <summary>
        /// The height in pixels of the glyph.
        /// </summary>
        [Tooltip("The height in pixels of the glyph.")]
        public short Height;

        /// <summary>
        /// The start location of the character in the texture (in texture coordinates space).
        /// </summary>
        [Tooltip("The start location of the character in the texture (in texture coordinates space).")]
        public Vector2 UV;

        /// <summary>
        /// The size the character in the texture (in texture coordinates space).
        /// </summary>
        [Tooltip("The size the character in the texture (in texture coordinates space).")]
        public Vector2 UVSize;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents font object that can be using during text rendering (it uses Font Asset but with pre-cached data for chosen font properties).
    /// </summary>
    [Tooltip("Represents font object that can be using during text rendering (it uses Font Asset but with pre-cached data for chosen font properties).")]
    public sealed unsafe partial class Font : FlaxEngine.Object
    {
        private Font() : base()
        {
        }

        /// <summary>
        /// Gets parent font asset that contains font family used by this font.
        /// </summary>
        [Tooltip("Gets parent font asset that contains font family used by this font.")]
        public FontAsset Asset
        {
            get { return Internal_GetAsset(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FontAsset Internal_GetAsset(IntPtr obj);

        /// <summary>
        /// Gets font size.
        /// </summary>
        [Tooltip("Gets font size.")]
        public int Size
        {
            get { return Internal_GetSize(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetSize(IntPtr obj);

        /// <summary>
        /// Gets characters height.
        /// </summary>
        [Tooltip("Gets characters height.")]
        public int Height
        {
            get { return Internal_GetHeight(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetHeight(IntPtr obj);

        /// <summary>
        /// Gets the largest vertical distance below the baseline for any character in the font.
        /// </summary>
        [Tooltip("The largest vertical distance below the baseline for any character in the font.")]
        public int Descender
        {
            get { return Internal_GetDescender(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetDescender(IntPtr obj);

        /// <summary>
        /// Gets the line gap property.
        /// </summary>
        [Tooltip("The line gap property.")]
        public int LineGap
        {
            get { return Internal_GetLineGap(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLineGap(IntPtr obj);

        /// <summary>
        /// Gets the kerning amount for a pair of characters.
        /// </summary>
        /// <param name="first">The first character in the pair.</param>
        /// <param name="second">The second character in the pair.</param>
        /// <returns>The kerning amount or 0 if no kerning.</returns>
        public int GetKerning(char first, char second)
        {
            return Internal_GetKerning(unmanagedPtr, first, second);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetKerning(IntPtr obj, char first, char second);

        /// <summary>
        /// Caches the given text to prepared for the rendering.
        /// </summary>
        /// <param name="text">The text witch characters to cache.</param>
        public void CacheText(string text)
        {
            Internal_CacheText(unmanagedPtr, text);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CacheText(IntPtr obj, string text);

        /// <summary>
        /// Invalidates all cached dynamic font atlases using this font. Can be used to reload font characters after changing font asset options.
        /// </summary>
        public void Invalidate()
        {
            Internal_Invalidate(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Invalidate(IntPtr obj);

        /// <summary>
        /// Processes text to get cached lines for rendering.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="layout">The layout properties.</param>
        /// <returns>The output lines list.</returns>
        public FontLineCache[] ProcessText(string text, ref TextLayoutOptions layout)
        {
            return Internal_ProcessText(unmanagedPtr, text, ref layout, typeof(FontLineCache));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FontLineCache[] Internal_ProcessText(IntPtr obj, string text, ref TextLayoutOptions layout, System.Type resultArrayItemType0);

        /// <summary>
        /// Processes text to get cached lines for rendering.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <returns>The output lines list.</returns>
        public FontLineCache[] ProcessText(string text)
        {
            return Internal_ProcessText1(unmanagedPtr, text, typeof(FontLineCache));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FontLineCache[] Internal_ProcessText1(IntPtr obj, string text, System.Type resultArrayItemType0);

        /// <summary>
        /// Measures minimum size of the rectangle that will be needed to draw given text.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="layout">The layout properties.</param>
        /// <returns>The minimum size for that text and fot to render properly.</returns>
        public Vector2 MeasureText(string text, ref TextLayoutOptions layout)
        {
            Internal_MeasureText(unmanagedPtr, text, ref layout, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_MeasureText(IntPtr obj, string text, ref TextLayoutOptions layout, out Vector2 resultAsRef);

        /// <summary>
        /// Measures minimum size of the rectangle that will be needed to draw given text
        /// </summary>.
        /// <param name="text">The input text to test.</param>
        /// <returns>The minimum size for that text and fot to render properly.</returns>
        public Vector2 MeasureText(string text)
        {
            Internal_MeasureText1(unmanagedPtr, text, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_MeasureText1(IntPtr obj, string text, out Vector2 resultAsRef);

        /// <summary>
        /// Calculates hit character index at given location.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="location">The input location to test.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <returns>The selected character position index (can be equal to text length if location is outside of the layout rectangle).</returns>
        public int HitTestText(string text, ref TextRange textRange, Vector2 location, ref TextLayoutOptions layout)
        {
            return Internal_HitTestText(unmanagedPtr, text, ref textRange, ref location, ref layout);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_HitTestText(IntPtr obj, string text, ref TextRange textRange, ref Vector2 location, ref TextLayoutOptions layout);

        /// <summary>
        /// Calculates hit character index at given location.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="location">The input location to test.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <returns>The selected character position index (can be equal to text length if location is outside of the layout rectangle).</returns>
        public int HitTestText(string text, Vector2 location, ref TextLayoutOptions layout)
        {
            return Internal_HitTestText1(unmanagedPtr, text, ref location, ref layout);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_HitTestText1(IntPtr obj, string text, ref Vector2 location, ref TextLayoutOptions layout);

        /// <summary>
        /// Calculates hit character index at given location.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="location">The input location to test.</param>
        /// <returns>The selected character position index (can be equal to text length if location is outside of the layout rectangle).</returns>
        public int HitTestText(string text, Vector2 location)
        {
            return Internal_HitTestText2(unmanagedPtr, text, ref location);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_HitTestText2(IntPtr obj, string text, ref Vector2 location);

        /// <summary>
        /// Calculates hit character index at given location.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="location">The input location to test.</param>
        /// <returns>The selected character position index (can be equal to text length if location is outside of the layout rectangle).</returns>
        public int HitTestText(string text, ref TextRange textRange, Vector2 location)
        {
            return Internal_HitTestText3(unmanagedPtr, text, ref textRange, ref location);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_HitTestText3(IntPtr obj, string text, ref TextRange textRange, ref Vector2 location);

        /// <summary>
        /// Calculates character position for given text and character index.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="index">The text position to get coordinates of.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <returns>The character position (upper left corner which can be used for a caret position).</returns>
        public Vector2 GetCharPosition(string text, int index, ref TextLayoutOptions layout)
        {
            Internal_GetCharPosition(unmanagedPtr, text, index, ref layout, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition(IntPtr obj, string text, int index, ref TextLayoutOptions layout, out Vector2 resultAsRef);

        /// <summary>
        /// Calculates character position for given text and character index.
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="index">The text position to get coordinates of.</param>
        /// <param name="layout">The text layout properties.</param>
        /// <returns>The character position (upper left corner which can be used for a caret position).</returns>
        public Vector2 GetCharPosition(string text, ref TextRange textRange, int index, ref TextLayoutOptions layout)
        {
            Internal_GetCharPosition1(unmanagedPtr, text, ref textRange, index, ref layout, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition1(IntPtr obj, string text, ref TextRange textRange, int index, ref TextLayoutOptions layout, out Vector2 resultAsRef);

        /// <summary>
        /// Calculates character position for given text and character index
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="index">The text position to get coordinates of.</param>
        /// <returns>The character position (upper left corner which can be used for a caret position).</returns>
        public Vector2 GetCharPosition(string text, int index)
        {
            Internal_GetCharPosition2(unmanagedPtr, text, index, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition2(IntPtr obj, string text, int index, out Vector2 resultAsRef);

        /// <summary>
        /// Calculates character position for given text and character index
        /// </summary>
        /// <param name="text">The input text to test.</param>
        /// <param name="textRange">The input text range (substring range of the input text parameter).</param>
        /// <param name="index">The text position to get coordinates of.</param>
        /// <returns>The character position (upper left corner which can be used for a caret position).</returns>
        public Vector2 GetCharPosition(string text, ref TextRange textRange, int index)
        {
            Internal_GetCharPosition3(unmanagedPtr, text, ref textRange, index, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCharPosition3(IntPtr obj, string text, ref TextRange textRange, int index, out Vector2 resultAsRef);
    }
}
