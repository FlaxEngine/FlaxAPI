// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Json asset that stores the collection of scene objects including actors and scripts. In general it can serve as any grouping of scene objects (for example a level) or be used as a form of a template instantiated and reused throughout the scene.
    /// </summary>
    /// <seealso cref="JsonAssetBase" />
    [Tooltip("Json asset that stores the collection of scene objects including actors and scripts. In general it can serve as any grouping of scene objects (for example a level) or be used as a form of a template instantiated and reused throughout the scene.")]
    public unsafe partial class Prefab : JsonAssetBase
    {
        /// <inheritdoc />
        protected Prefab() : base()
        {
        }

        /// <summary>
        /// Requests the default prefab object instance. Deserializes the prefab objects from the asset. Skips if already done.
        /// </summary>
        /// <returns>The root of the prefab object loaded from the prefab. Contains the default values. It's not added to gameplay but deserialized with postLoad and init event fired.</returns>
        public Actor GetDefaultInstance()
        {
            return Internal_GetDefaultInstance(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetDefaultInstance(IntPtr obj);

        /// <summary>
        /// Requests the default prefab object instance. Deserializes the prefab objects from the asset. Skips if already done.
        /// </summary>
        /// <param name="objectId">The ID of the object to get from prefab default object. It can be one of the child-actors or any script that exists in the prefab. Methods returns root if id is empty.</param>
        /// <returns>The object of the prefab loaded from the prefab. Contains the default values. It's not added to gameplay but deserialized with postLoad and init event fired.</returns>
        public SceneObject GetDefaultInstance(ref Guid objectId)
        {
            return Internal_GetDefaultInstance1(unmanagedPtr, ref objectId);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SceneObject Internal_GetDefaultInstance1(IntPtr obj, ref Guid objectId);
    }
}
