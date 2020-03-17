// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Editor viewports icons rendering service.
    /// </summary>
    [Tooltip("Editor viewports icons rendering service.")]
    public static unsafe partial class ViewportIconsRenderer
    {
        /// <summary>
        /// Draws the icons for the actors in the given scene.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="scene">The scene.</param>
        public static void DrawIcons(ref RenderContext renderContext, Scene scene)
        {
            Internal_DrawIcons(ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(scene));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawIcons(ref RenderContext renderContext, IntPtr scene);
    }
}
