using System;
using System.Collections.Generic;

namespace FlaxEditor.Workspace.Items
{
    public class ProjectDefinitionDescriptor
    {
        public readonly IReadOnlyCollection<ProjectConfigurationAndPlatform> Configurations;

        public readonly string Name;

        public readonly string Path;

        public readonly Guid ProjectGuid;
        public readonly Guid ProjectTypeGuid;

        public ProjectDefinitionDescriptor(string name, string path, Guid guid, Guid typeGuid,
                                           IReadOnlyCollection<ProjectConfigurationAndPlatform> configurations)
        {
            Name = name;
            Path = path;
            ProjectGuid = guid;
            ProjectTypeGuid = typeGuid;
            Configurations = configurations;
        }

        public static ProjectDefinitionDescriptor ForSolutionFolder(string name, Guid guid)
        {
            return new ProjectDefinitionDescriptor(name, name, guid, SolutionFolderDefinition.TypeGuid,
                                                   EmptyList<ProjectConfigurationAndPlatform>.Instance);
        }
    }
}
