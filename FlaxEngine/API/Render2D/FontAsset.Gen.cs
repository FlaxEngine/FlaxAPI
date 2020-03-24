// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The font hinting used when rendering characters.
    /// </summary>
    [Tooltip("The font hinting used when rendering characters.")]
    public enum FontHinting : byte
    {
        /// <summary>
        /// Use the default hinting specified in the font.
        /// </summary>
        [Tooltip("Use the default hinting specified in the font.")]
        Default,

        /// <summary>
        /// Force the use of an automatic hinting algorithm (over the font's native hinter).
        /// </summary>
        [Tooltip("Force the use of an automatic hinting algorithm (over the font's native hinter).")]
        Auto,

        /// <summary>
        /// Force the use of an automatic light hinting algorithm, optimized for non-monochrome displays.
        /// </summary>
        [Tooltip("Force the use of an automatic light hinting algorithm, optimized for non-monochrome displays.")]
        AutoLight,

        /// <summary>
        /// Force the use of an automatic hinting algorithm optimized for monochrome displays.
        /// </summary>
        [Tooltip("Force the use of an automatic hinting algorithm optimized for monochrome displays.")]
        Monochrome,

        /// <summary>
        /// Do not use hinting. This generally generates 'blurrier' bitmap glyphs when the glyph are rendered in any of the anti-aliased modes.
        /// </summary>
        [Tooltip("Do not use hinting. This generally generates 'blurrier' bitmap glyphs when the glyph are rendered in any of the anti-aliased modes.")]
        None,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The font flags used when rendering characters.
    /// </summary>
    [Flags]
    [Tooltip("The font flags used when rendering characters.")]
    public enum FontFlags : byte
    {
        /// <summary>
        /// No options.
        /// </summary>
        [Tooltip("No options.")]
        None = 0,

        /// <summary>
        /// Enables using anti-aliasing for font characters. Otherwise font will use monochrome data.
        /// </summary>
        [Tooltip("Enables using anti-aliasing for font characters. Otherwise font will use monochrome data.")]
        AntiAliasing = 1,

        /// <summary>
        /// Enables artificial embolden effect.
        /// </summary>
        [Tooltip("Enables artificial embolden effect.")]
        Bold = 2,

        /// <summary>
        /// Enables slant effect, emulating italic style.
        /// </summary>
        [Tooltip("Enables slant effect, emulating italic style.")]
        Italic = 4,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The font asset options.
    /// </summary>
    [Tooltip("The font asset options.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct FontOptions
    {
        /// <summary>
        /// The hinting.
        /// </summary>
        [Tooltip("The hinting.")]
        public FontHinting Hinting;

        /// <summary>
        /// The flags.
        /// </summary>
        [Tooltip("The flags.")]
        public FontFlags Flags;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Font asset contains glyph collection and cached data used to render text.
    /// </summary>
    [Tooltip("Font asset contains glyph collection and cached data used to render text.")]
    public unsafe partial class FontAsset : BinaryAsset
    {
        /// <inheritdoc />
        protected FontAsset() : base()
        {
        }

        /// <summary>
        /// Gets the font family name.
        /// </summary>
        [Tooltip("The font family name.")]
        public string FamilyName
        {
            get { return Internal_GetFamilyName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetFamilyName(IntPtr obj);

        /// <summary>
        /// Gets the font style name.
        /// </summary>
        [Tooltip("The font style name.")]
        public string StyleName
        {
            get { return Internal_GetStyleName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetStyleName(IntPtr obj);

        /// <summary>
        /// Gets or sets the font options.
        /// </summary>
        [Tooltip("The font options.")]
        public FontOptions Options
        {
            get { Internal_GetOptions(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetOptions(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOptions(IntPtr obj, out FontOptions resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOptions(IntPtr obj, ref FontOptions value);

        /// <summary>
        /// Creates the font object of given characters size.
        /// </summary>
        /// <param name="size">The font characters size.</param>
        /// <returns>The created font object.</returns>
        public Font CreateFont(int size)
        {
            return Internal_CreateFont(unmanagedPtr, size);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Font Internal_CreateFont(IntPtr obj, int size);

        /// <summary>
        /// Saves this asset to the file. Supported only in Editor.
        /// </summary>
        /// <param name="path">The custom asset path to use for the saving. Use empty value to save this asset to its own storage location. Can be used to duplicate asset. Must be specified when saving virtual asset.</param>
        /// <returns>True if cannot save data, otherwise false.</returns>
        public bool Save(string path = null)
        {
            return Internal_Save(unmanagedPtr, path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Save(IntPtr obj, string path);

        /// <summary>
        /// Invalidates all cached dynamic font atlases using this font. Can be used to reload font characters after changing font asset options.
        /// </summary>
        public void Invalidate()
        {
            Internal_Invalidate(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Invalidate(IntPtr obj);
    }
}
