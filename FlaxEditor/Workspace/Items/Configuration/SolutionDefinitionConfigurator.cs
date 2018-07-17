// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Items.Definitions.Abstract;
using FlaxEditor.Workspace.Utils;

namespace FlaxEditor.Workspace.Items.Configuration
{
    public class SolutionDefinitionConfigurator
    {
        public static IEnumerable<Tuple<string, string>> BuildNewProjectConfigurationsList(
            SolutionDefinition solutionDefinition, ProjectDefinition projectDefinition,
            IReadOnlyCollection<ProjectConfigurationAndPlatform> knownConfigurationAndPlatforms)
        {
            var pairList = new List<Tuple<string, string>>();
            foreach (var solutionConfiguration in solutionDefinition.GetSolutionConfigurations())
            {
                var projectConfiguration =
                FindProjectConfiguration(solutionConfiguration, knownConfigurationAndPlatforms);
                var upper = projectDefinition.ProjectGuid.ToString("B").ToUpper(CultureInfo.InvariantCulture);
                var separatedValue1 = solutionConfiguration.ToSeparatedValue();
                var separatedValue2 = projectConfiguration.ToSeparatedValue();
                var str = upper + "." + separatedValue1;
                var second = separatedValue2;
                pairList.Add(new Tuple<string, string>(str + ".ActiveCfg", second));
                if (projectConfiguration.ShouldBuild)
                {
                    pairList.Add(new Tuple<string, string>(str + ".Build.0", second));
                }
            }

            return pairList;
        }

        public static ProjectConfigurationAndPlatform FindProjectConfiguration(
            SolutionConfigurationAndPlatform configurationAndPlatform,
            IReadOnlyCollection<ProjectConfigurationAndPlatform> knownConfigurationAndPlatforms)
        {
            if (knownConfigurationAndPlatforms.Count == 0)
            {
                return new ProjectConfigurationAndPlatform(configurationAndPlatform, true, false);
            }

            return knownConfigurationAndPlatforms.FirstOrDefault(x =>
                   {
                       if (x.ConfigurationForSolution == configurationAndPlatform.Configuration)
                       {
                           return x.PlatformForSolution == configurationAndPlatform.Platform;
                       }

                       return false;
                   }) ?? knownConfigurationAndPlatforms.
                   FirstOrDefault(x => x.ConfigurationForSolution == configurationAndPlatform.Configuration) 
                   ?? knownConfigurationAndPlatforms.First();
        }
    }
}
