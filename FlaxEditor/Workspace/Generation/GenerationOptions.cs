// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Workspace.Generation
{
	public class GenerationOptions
	{
        /// <summary>
        /// The path to where given template file is located
        /// </summary>
        string TemplateTargetPath { get; set; }

        /// <summary>
        /// The path to where given file is located
        /// </summary>
        string TargetPath { get; set; }

		/// <summary>
		/// Generated file name (excluding extension)
		/// </summary>
		string FileName { get; set; }

		/// <summary>
		/// True if generation should regenerate whole solution file by force, otherwise generate it only if missing.
		/// </summary>
		bool ForceGenerateSolution { get; set; }
	}

	public class ProjectGenerationOptions : GenerationOptions
	{
		/// <summary>
		/// The defines used for the compilation.
		/// </summary>
		string[] Defines { get; set; }
	}
}
