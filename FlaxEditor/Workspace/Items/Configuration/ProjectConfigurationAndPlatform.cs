using System;
using System.Collections.Generic;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Utils;

namespace FlaxEditor.Workspace.Items.Configuration
{
    /// <summary>Store configuration and platform for a project</summary>
    /// <remarks>
    ///     Configuration and Platform can be written in a different way in project files and solution files ('Any CPU' vs
    ///     'AnyCPU')
    ///     that's why we store configuration and platform in PROJECT style and provide and API to get both styles
    /// </remarks>
    public class ProjectConfigurationAndPlatform : IProjectConfigurationAndPlatform,
                                                   IEquatable<ProjectConfigurationAndPlatform>
    {
        public static ProjectConfigurationAndPlatform[] CreateDefaultConfigurationAndPlatform()
        {
            return new ProjectConfigurationAndPlatform[]
            {
                new ProjectConfigurationAndPlatform("Debug", "AnyCPU", true, false),
                new ProjectConfigurationAndPlatform("Release", "AnyCPU", true, false),
            };
        }

        public static ProjectConfigurationAndPlatform[] CreateSolutionConfigurationAndPlatform(SolutionDefinition solutionDefinition)
        {
            var list = new List<ProjectConfigurationAndPlatform>();
            var definitions = solutionDefinition.GetSolutionConfigurations();
            foreach (var definition in definitions)
            {
                list.Add(new ProjectConfigurationAndPlatform(definition, true, false));
            }
            return list.ToArray();
        }

        public readonly string Configuration;

        public readonly string Platform;

        public ProjectConfigurationAndPlatform(string configuration, string platform, bool shouldBuild,
                                               bool shouldDeploy)
        {
            Configuration = EnsureConfigurationInProjectStyle(configuration);
            Platform = EnsurePlatformInProjectStyle(platform);
            ShouldBuild = shouldBuild;
            ShouldDeploy = shouldDeploy;
        }

        public ProjectConfigurationAndPlatform(SolutionConfigurationAndPlatform solutionConfigurationAndPlatform,
                                               bool shouldBuild, bool shouldDeploy)
        : this(solutionConfigurationAndPlatform.Configuration, solutionConfigurationAndPlatform.Platform,
               shouldBuild, shouldDeploy)
        {
        }

        /// <summary>Configuration in 'solution file' style</summary>
        public string ConfigurationForSolution => ConvertConfigurationToSolutionStyle(Configuration);

        /// <summary>Platform in 'solutuion file' style</summary>
        public string PlatformForSolution => ConvertPlatformToSolutionStyle(Platform);

        public bool ShouldBuild { get; }

        public bool ShouldDeploy { get; }

        public bool Equals(ProjectConfigurationAndPlatform other)
        {
            if (other == null)
            {
                return false;
            }

            if (this == other)
            {
                return true;
            }

            if (string.Equals(Configuration, other.Configuration) && string.Equals(Platform, other.Platform) &&
                ShouldBuild == other.ShouldBuild)
            {
                return ShouldDeploy == other.ShouldDeploy;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this == obj)
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ProjectConfigurationAndPlatform)obj);
        }

        public override int GetHashCode()
        {
            return (((((Configuration.GetHashCode() * 397) ^ Platform.GetHashCode()) * 397) ^
                     ShouldBuild.GetHashCode()) * 397) ^ ShouldDeploy.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Configuration: {0}, Platform: {1}, ShouldBuild: {2}, ShouldDeploy: {3}",
                                 (object)Configuration, (object)Platform, (object)ShouldBuild, (object)ShouldDeploy);
        }

        public static string ConvertConfigurationToSolutionStyle(string configuration)
        {
            return configuration;
        }

        public static string ConvertPlatformToSolutionStyle(string platform)
        {
            if (platform.Equals("AnyCPU", StringComparison.OrdinalIgnoreCase))
            {
                return "Any CPU";
            }

            return platform;
        }

        public static string EnsureConfigurationInProjectStyle(string configuration)
        {
            return configuration;
        }

        public static string EnsurePlatformInProjectStyle(string platform)
        {
            if (platform.Equals("Any CPU", StringComparison.OrdinalIgnoreCase))
            {
                return "AnyCPU";
            }

            return platform;
        }
    }
}
