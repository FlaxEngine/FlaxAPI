// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The scene root object that contains a hierarchy of actors.
    /// </summary>
    [Tooltip("The scene root object that contains a hierarchy of actors.")]
    public sealed unsafe partial class Scene : Actor
    {
        private Scene() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Scene"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Scene New()
        {
            return Internal_Create(typeof(Scene)) as Scene;
        }

        /// <summary>
        /// Gets path to the scene file
        /// </summary>
        [Tooltip("Gets path to the scene file")]
        public string Path
        {
            get { return Internal_GetPath(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetPath(IntPtr obj);

        /// <summary>
        /// Gets filename of the scene file
        /// </summary>
        [Tooltip("Gets filename of the scene file")]
        public string Filename
        {
            get { return Internal_GetFilename(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetFilename(IntPtr obj);

        /// <summary>
        /// Gets path to the scene data folder
        /// </summary>
        [Tooltip("Gets path to the scene data folder")]
        public string DataFolderPath
        {
            get { return Internal_GetDataFolderPath(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetDataFolderPath(IntPtr obj);

        /// <summary>
        /// Removes all baked lightmap textures from the scene.
        /// </summary>
        public void ClearLightmaps()
        {
            Internal_ClearLightmaps(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClearLightmaps(IntPtr obj);

        /// <summary>
        /// Builds the CSG geometry for the given scene.
        /// </summary>
        /// <remarks>Requests are enqueued till the next game scripts update.</remarks>
        /// <param name="timeoutMs">The timeout to wait before building CSG (in milliseconds).</param>
        public void BuildCSG(float timeoutMs = 50)
        {
            Internal_BuildCSG(unmanagedPtr, timeoutMs);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BuildCSG(IntPtr obj, float timeoutMs);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The scene asset.
    /// </summary>
    [Tooltip("The scene asset.")]
    public unsafe partial class SceneAsset : JsonAsset
    {
        /// <inheritdoc />
        protected SceneAsset() : base()
        {
        }
    }
}
