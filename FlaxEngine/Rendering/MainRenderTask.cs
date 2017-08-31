////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// The main game rendering task used by the engine.
    /// </summary>
    /// <seealso cref="FlaxEngine.Rendering.SceneRenderTask" />
    public sealed class MainRenderTask : SceneRenderTask
    {
        /// <summary>
        /// Gets the main game rendering task. Use it to plug custom rendering logic for your game.
        /// </summary>
        public static MainRenderTask Instance
        {
            get;
            internal set;
        }

        // TODO: add API to override main camera
        
        internal MainRenderTask()
        {
        }

        /// <inheritdoc />
        protected override void OnBegin()
        {
            // Use the main camera for the game
            Camera = Camera.MainCamera;

            base.OnBegin();
        }
    }
}
