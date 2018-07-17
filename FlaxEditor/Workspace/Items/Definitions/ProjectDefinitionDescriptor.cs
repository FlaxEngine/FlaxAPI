using System;
using System.Collections.Generic;
using FlaxEditor.Workspace.Items.Configuration;

namespace FlaxEditor.Workspace.Items.Definitions
{
    public class ProjectDefinitionDescriptor
    {
        public static readonly Guid CSharpProjectConstant = Guid.Parse("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");

        /// <summary>
        /// Project build and platform configuration
        /// </summary>
        public readonly IReadOnlyCollection<ProjectConfigurationAndPlatform> Configurations;

        /// <summary>
        /// Name of the project
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Path of the project
        /// </summary>
        public readonly string Path;
        
        /// <summary>
        /// Unique Guid of the project
        /// </summary>
        public readonly Guid ProjectGuid;
        /// <summary>
        /// Identifier of the language used for the project. We want guid for C# here most likely
        /// </summary>
        public readonly Guid ProjectTypeGuid;

        /// <summary>
        /// Create new descriptor for the project
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="path"><see cref="Path"/></param>
        /// <param name="guid"><see cref="ProjectGuid"/></param>
        /// <param name="typeGuid"><see cref="ProjectTypeGuid"/></param>
        /// <param name="configurations"><see cref="Configurations"/></param>
        public ProjectDefinitionDescriptor(string name, string path, Guid guid, Guid typeGuid,
                                           IReadOnlyCollection<ProjectConfigurationAndPlatform> configurations)
        {
            Name = name;
            Path = path;
            ProjectGuid = guid;
            ProjectTypeGuid = typeGuid;
            Configurations = configurations;
        }

        /// <summary>
        /// Asign current Project definition to a solution folder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static ProjectDefinitionDescriptor ForSolutionFolder(string name, Guid guid)
        {
            return new ProjectDefinitionDescriptor(name, name, guid, SolutionFolderDefinition.TypeGuid, new List<ProjectConfigurationAndPlatform>());
        }
    }
}
