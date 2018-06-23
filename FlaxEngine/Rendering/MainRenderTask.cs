// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// The main game rendering task used by the engine.
    /// </summary>
    /// <remarks>
    /// For Main Render Task its <see cref="SceneRenderTask.Output"/> may be null because game can be rendered directly to the native window backbuffer.
    /// This allows to increase game rendering performance (reduced memory usage and data transfer).
    /// User should use post effects pipeline to modify the final frame.
    /// </remarks>
    /// <seealso cref="FlaxEngine.Rendering.SceneRenderTask" />
    public sealed class MainRenderTask : SceneRenderTask
    {
        /// <summary>
        /// Gets the main game rendering task. Use it to plug custom rendering logic for your game.
        /// </summary>
        public static MainRenderTask Instance { get; internal set; }

        // TODO: add API to override main camera

        internal MainRenderTask()
        {
        }

        /// <inheritdoc />
        protected override void OnBegin(GPUContext context)
        {
            // Use the main camera for the game
            Camera = Camera.MainCamera;

            if (!Application.IsEditor)
            {
                // Sync render buffers size with the backbuffer
                Buffers.Size = Screen.Size;
            }

            base.OnBegin(context);
        }

        internal override bool Internal_Begin(out IntPtr outputPtr)
        {
            bool result = base.Internal_Begin(out outputPtr);

            if (!Application.IsEditor)
            {
                // Standalone build uses hidden, internal output (native window backbuffer in most cases)
                // So pass rendering even with missing output
                result = true;
            }

            return result;
        }
    }
}
