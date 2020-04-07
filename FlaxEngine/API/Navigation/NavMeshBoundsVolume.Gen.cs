// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A special type of volume that defines the areas of the scene in which navigation meshes are generated.
    /// </summary>
    [Tooltip("A special type of volume that defines the areas of the scene in which navigation meshes are generated.")]
    public unsafe partial class NavMeshBoundsVolume : BoxVolume
    {
        /// <inheritdoc />
        protected NavMeshBoundsVolume() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="NavMeshBoundsVolume"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static NavMeshBoundsVolume New()
        {
            return Internal_Create(typeof(NavMeshBoundsVolume)) as NavMeshBoundsVolume;
        }
    }
}
