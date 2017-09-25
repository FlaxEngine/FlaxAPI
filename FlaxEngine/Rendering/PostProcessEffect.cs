////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Custom postFx which can modify final image by processing it with material based filters.
    /// The base class for all post process effects used by the graphics pipeline.
    /// Allows to extend frame rendering logic and apply custom effects such as outline, nighvision, contrast etc.
    /// </summary>
    /// <remarks>
    /// Override this class and implement custom post fx logic.
    /// Use <b>MainRenderTask.Instance.CustomPostFx.Add(myPostFx)</b> to attach your script to rendering.
    /// Or add script to camera.
    /// </remarks>
    public abstract class PostProcessEffect : Script
    {
        /// <summary>
        /// Gets a value indicating whether this effect can be rendered.
        /// </summary>
        public virtual bool CanRender => Enabled;

        /// <summary>
        /// Performs custom postFx rendering.
        /// </summary>
        /// <param name="context">The GPU commands context.</param>
        /// <param name="task">The current rendering task.</param>
        /// <param name="input">The input texture.</param>
        /// <param name="output">The output texture.</param>
        public abstract void Render(GPUContext context, SceneRenderTask task, RenderTarget input, RenderTarget output);
    }
}
