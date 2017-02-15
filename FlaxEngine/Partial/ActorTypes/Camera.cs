// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Camera actor is a device through which the player views the world.
    /// </summary>
    public partial class Camera
    {
        // TODO: getMainCamera
        // TODO: get/edit camera params
        // TODO: customAspectRatio, customViewport
        // TODO: ConvertMouseToRay

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }
    }
}
