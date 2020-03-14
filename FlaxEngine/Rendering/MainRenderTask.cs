// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// The main game rendering task used by the engine.
    /// </summary>
    /// <remarks>
    /// For Main Render Task its <see cref="SceneRenderTask.Output"/> may be null because game can be rendered directly to the native window backbuffer.
    /// This allows to increase game rendering performance (reduced memory usage and data transfer).
    /// User should use post effects pipeline to modify the final frame.
    /// </remarks>
    /// <seealso cref="FlaxEngine.SceneRenderTask" />
    public sealed class MainRenderTask : SceneRenderTask
    {
        /// <summary>
        /// Gets the main game rendering task. Use it to plug custom rendering logic for your game.
        /// </summary>
        public static MainRenderTask Instance { get; internal set; }

        internal MainRenderTask()
        {
        }

        /// <inheritdoc />
        protected override void OnBegin(GPUContext context)
        {
            // Use the main camera for the game
            Camera = Camera.MainCamera;

            if (!Platform.IsEditor)
            {
                // Sync render buffers size with the backbuffer
                Buffers.Size = Screen.Size;
            }

            base.OnBegin(context);
        }

        internal override bool OnBegin(out IntPtr outputPtr)
        {
            bool result = base.OnBegin(out outputPtr);

            if (!Platform.IsEditor)
            {
                // Standalone build uses hidden, internal output (native window backbuffer in most cases)
                // So pass rendering even with missing output
                result = true;
            }

            return result;
        }
    }
}
