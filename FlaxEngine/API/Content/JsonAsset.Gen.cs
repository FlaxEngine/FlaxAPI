// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all Json-format assets.
    /// </summary>
    /// <seealso cref="Asset" />
    [Tooltip("Base class for all Json-format assets.")]
    public abstract unsafe partial class JsonAssetBase : Asset
    {
        /// <inheritdoc />
        protected JsonAssetBase() : base()
        {
        }

        /// <summary>
        /// The data type name from the header. Allows to recognize the data type.
        /// </summary>
        [Tooltip("The data type name from the header. Allows to recognize the data type.")]
        public string DataTypeName
        {
            get { return Internal_GetDataTypeName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetDataTypeName(IntPtr obj);

        /// <summary>
        /// The serialized data engine build number. Can be used to convert/upgrade data between different formats across different engine versions.
        /// </summary>
        [Tooltip("The serialized data engine build number. Can be used to convert/upgrade data between different formats across different engine versions.")]
        public int DataEngineBuild
        {
            get { return Internal_GetDataEngineBuild(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetDataEngineBuild(IntPtr obj);

        /// <summary>
        /// The Json data (as string).
        /// </summary>
        [Tooltip("The Json data (as string).")]
        public string Data
        {
            get { return Internal_GetData(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetData(IntPtr obj);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Generic type of Json-format asset. It provides the managed representation of this resource data so it can be accessed via C# API.
    /// </summary>
    /// <seealso cref="JsonAssetBase" />
    [Tooltip("Generic type of Json-format asset. It provides the managed representation of this resource data so it can be accessed via C# API.")]
    public unsafe partial class JsonAsset : JsonAssetBase
    {
        /// <inheritdoc />
        protected JsonAsset() : base()
        {
        }
    }
}
