// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Texture asset contains an image that is usually stored on a GPU and is used during rendering graphics.
    /// </summary>
    [Tooltip("Texture asset contains an image that is usually stored on a GPU and is used during rendering graphics.")]
    public partial class Texture : TextureBase
    {
        /// <inheritdoc />
        protected Texture() : base()
        {
        }

        /// <summary>
        /// Returns true if texture is a normal map.
        /// </summary>
        [Tooltip("Returns true if texture is a normal map.")]
        public bool IsNormalMap
        {
            get { return Internal_IsNormalMap(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsNormalMap(IntPtr obj);

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
        /// Loads the texture from the image file. Supported file formats depend on a runtime platform. All platform support loading PNG, BMP, TGA, HDR and JPEG files.
        /// </summary>
        /// <remarks>Valid only for virtual assets.</remarks>
        /// <param name="path">The source image file path.</param>
        /// <param name="generateMips">True if generate mipmaps for the imported texture.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public bool LoadFile(string path, bool generateMips = false)
        {
            return Internal_LoadFile(unmanagedPtr, path, generateMips);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_LoadFile(IntPtr obj, string path, bool generateMips);

        /// <summary>
        /// Loads the texture from the image file and creates the virtual texture asset for it. Supported file formats depend on a runtime platform. All platform support loading PNG, BMP, TGA, HDR and JPEG files.
        /// </summary>
        /// <param name="path">The source image file path.</param>
        /// <param name="generateMips">True if generate mipmaps for the imported texture.</param>
        /// <returns>The loaded texture (virtual asset) or null if fails.</returns>
        public static Texture FromFile(string path, bool generateMips = false)
        {
            return Internal_FromFile(path, generateMips);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Texture Internal_FromFile(string path, bool generateMips);
    }
}
