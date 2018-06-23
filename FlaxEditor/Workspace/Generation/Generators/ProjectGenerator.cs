// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Workspace.Generation
{
    public class ProjectGenerator : IGenerator
    {
        public const string FileExtension = ".sln";

        /// <inheritdoc />
        public bool Generate(GenerationOptions boxedOptions)
        {
            var options = (ProjectGenerationOptions)boxedOptions;
            
        }
    }
}
