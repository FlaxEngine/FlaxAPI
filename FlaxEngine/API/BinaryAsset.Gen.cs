// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all binary assets.
    /// </summary>
    /// <seealso cref="Asset" />
    [Tooltip("Base class for all binary assets.")]
    public abstract partial class BinaryAsset : Asset
    {
        /// <inheritdoc />
        protected BinaryAsset() : base()
        {
        }

        /// <summary>
        /// Gets the imported file path from the asset metadata (can be empty if not available).
        /// </summary>
        [Tooltip("The imported file path from the asset metadata (can be empty if not available).")]
        public string ImportPath
        {
            get { return Internal_GetImportPath(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetImportPath(IntPtr obj);

        /// <summary>
        /// Reimports asset from the source file.
        /// </summary>
        public void Reimport()
        {
            Internal_Reimport(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Reimport(IntPtr obj);
    }
}
