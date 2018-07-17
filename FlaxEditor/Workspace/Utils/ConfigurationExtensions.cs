// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace.Items.Configuration;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Items.Definitions.Abstract;
using FlaxEngine;

namespace FlaxEditor.Workspace.Utils
{
    public static class ConfigurationExtensions
    {
        public const char ConfigurationPlatformSeparatorChar = '|';
        public const string ConfigurationPlatformSeparator = "|";
        public const string SolutionConfigurationPlatformsSection = "SolutionConfigurationPlatforms";
        public const string SolutionConfigurationPlatformsSectionValue = "preSolution";
        public const string ProjectConfigurationPlatformsSection = "ProjectConfigurationPlatforms";
        public const string ProjectConfigurationPlatformsSectionValue = "postSolution";
        public const string ActiveCfgSuffix = "ActiveCfg";
        public const string Build0Suffix = "Build.0";
        public const string Deploy0Suffix = "Deploy.0";

        public static ICollection<SolutionConfigurationAndPlatform> GetSolutionConfigurations(
            this SolutionDefinition solutionDefinition)
        {
            return solutionDefinition.GetGlobalSections("SolutionConfigurationPlatforms")
                                     .SelectMany(x => x.Items.OfType<PropertyDefinition>()).Select(x =>
                                     {
                                         var strArray = x.PropertyName.Split('|');
                                         if (strArray.Length != 2)
                                         {
                                             return (SolutionConfigurationAndPlatform)null;
                                         }

                                         return new SolutionConfigurationAndPlatform(strArray[0], strArray[1]);
                                     }).ToList();
        }

        public static IDictionary<ProjectDefinition, IDictionary<SolutionConfigurationAndPlatform, ProjectConfigurationAndPlatform>>
        GetProjectConfigurations(this SolutionDefinition solutionDefinition)
        {
            var globalSections = solutionDefinition.GetGlobalSections("ProjectConfigurationPlatforms");
            var propertyDefinitionsDictionary = new Dictionary<string, PropertyDefinition>(StringComparer.InvariantCultureIgnoreCase);
            foreach (CompositeDefinitionItem compositeDefinitionItem in globalSections)
            {
                foreach (var propertyDefinition in compositeDefinitionItem.Items.OfType<PropertyDefinition>())
                {
                    propertyDefinitionsDictionary[propertyDefinition.PropertyName] = propertyDefinition;
                }
            }

            var projects = solutionDefinition.GetProjects();
            var solutionConfigurations = solutionDefinition.GetSolutionConfigurations();
            var dictionary2 =
            new Dictionary<ProjectDefinition,
                IDictionary<SolutionConfigurationAndPlatform, ProjectConfigurationAndPlatform>>();
            foreach (var index in solutionConfigurations)
            {
                foreach (var projectDefinition in projects)
                {
                    var separatedValue = index.ToSeparatedValue();
                    var configurationString1 = projectDefinition.ToActiveConfigurationString(separatedValue);
                    var valueSafe = propertyDefinitionsDictionary.GetValueSafe(configurationString1);
                    if (valueSafe != null)
                    {
                        var newProjectConfig = new Dictionary<SolutionConfigurationAndPlatform, ProjectConfigurationAndPlatform>();
                        if (!dictionary2.ContainsKey(projectDefinition))
                        {
                            dictionary2[projectDefinition] = newProjectConfig;
                        }
                        else
                        {
                            newProjectConfig = (Dictionary<SolutionConfigurationAndPlatform, ProjectConfigurationAndPlatform>)dictionary2[projectDefinition];
                        }
                        var configurationString2 = projectDefinition.ToBuildConfigurationString(separatedValue);
                        var configurationString3 = projectDefinition.ToDeployConfigurationString(separatedValue);
                        var strArray = valueSafe.PropertyValue.Split('|');
                        if (strArray.Length == 2)
                        {
                            var configurationAndPlatform = new ProjectConfigurationAndPlatform(strArray[0], strArray[1],
                                                                                               propertyDefinitionsDictionary.ContainsKey(configurationString2),
                                                                                               propertyDefinitionsDictionary.ContainsKey(configurationString3));
                            newProjectConfig[index] = configurationAndPlatform;
                        }
                    }
                }
            }

            return dictionary2;
        }

        public static void UpdateSolutionConfigurations(this SolutionDefinition solutionDefinition,
                                                        ICollection<SolutionConfigurationAndPlatform> actualSolutionConfigurations)
        {
            var sortedSet = new SortedSet<Tuple<string, string>>(actualSolutionConfigurations.Select(x => x.ToSeparatedValue()).Select(x => new Tuple<string, string>(x, x)));
            SyncSectionProperties(solutionDefinition, "SolutionConfigurationPlatforms", "preSolution", sortedSet,
                                  x => x.SubstringBefore("|"));
        }

        public static void UpdateProjectConfigurations(this SolutionDefinition solutionDefinition,
                                                       IDictionary<SolutionConfigurationAndPlatform,
                                                           IDictionary<ProjectDefinition, ProjectConfigurationAndPlatform>> actualProjectConfigurations)
        {
            var pairSet = new HashSet<Tuple<string, string>>();
            foreach (var projectConfiguration in actualProjectConfigurations)
            {
                foreach (var keyValuePair in projectConfiguration.Value)
                {
                    var separatedValue1 = projectConfiguration.Key.ToSeparatedValue();
                    var separatedValue2 = keyValuePair.Value.ToSeparatedValue();
                    var key = keyValuePair.Key;
                    pairSet.Add(new Tuple<string, string>(key.ToActiveConfigurationString(separatedValue1), separatedValue2));
                    if (keyValuePair.Value.ShouldBuild)
                    {
                        pairSet.Add(new Tuple<string, string>(key.ToBuildConfigurationString(separatedValue1), separatedValue2));
                    }

                    if (keyValuePair.Value.ShouldDeploy)
                    {
                        pairSet.Add(new Tuple<string, string>(key.ToDeployConfigurationString(separatedValue1), separatedValue2));
                    }
                }
            }

            SyncSectionProperties(solutionDefinition, "ProjectConfigurationPlatforms", "postSolution", pairSet,
                                  x => x.SubstringBefore("."));
        }

        private static void SyncSectionProperties(SolutionDefinition solutionDefinition, string sectionName,
                                                  string sectionValue, ISet<Tuple<string, string>> allProperties, Func<string, string> prefixFunc)
        {
            var globalSections = solutionDefinition.GetGlobalSections(sectionName);
            var pairSet = new HashSet<Tuple<string, string>>(allProperties);
            foreach (var sectionDefinition in globalSections)
            {
                foreach (var propertyDefinition in sectionDefinition.Items.OfType<PropertyDefinition>().ToList())
                {
                    var pair = new Tuple<string, string>(propertyDefinition.PropertyName, propertyDefinition.PropertyValue);
                    pairSet.Remove(pair);
                    if (!allProperties.Contains(pair))
                    {
                        sectionDefinition.RemoveProperty(propertyDefinition);
                    }
                }
            }

            var globalSection = solutionDefinition.GetOrCreateGlobalSection(sectionName, sectionValue);
            var dictionary = new Dictionary<string, PropertyDefinition>();
            foreach (var property in globalSection.GetProperties())
            {
                var index = prefixFunc(property.PropertyName);
                dictionary[index] = property;
            }

            foreach (var pair in pairSet)
            {
                var key = prefixFunc(pair.Item1);
                var valueSafe = dictionary.GetValueSafe(key, null);
                var propertyDefinition = valueSafe != null
                                         ? globalSection.AddPropertyAfter(pair.Item1, pair.Item2, valueSafe)
                                         : globalSection.AddProperty(pair.Item1, pair.Item2);
                dictionary[key] = propertyDefinition;
            }
        }

        public static string ToActiveConfigurationString(this ProjectDefinition projectDefinition,
                                                         string solutionConfiguration)
        {
            return ToConfigurationString(projectDefinition, solutionConfiguration, "ActiveCfg");
        }

        public static string ToBuildConfigurationString(this ProjectDefinition projectDefinition,
                                                        string solutionConfiguration)
        {
            return ToConfigurationString(projectDefinition, solutionConfiguration, "Build.0");
        }

        public static string ToDeployConfigurationString(this ProjectDefinition projectDefinition,
                                                         string solutionConfiguration)
        {
            return ToConfigurationString(projectDefinition, solutionConfiguration, "Deploy.0");
        }

        private static string ToConfigurationString(ProjectDefinition projectDefinition, string solutionConfiguration,
                                                    string suffix)
        {
            return $"{projectDefinition.ProjectGuid.ToUpperCurlyString()}.{solutionConfiguration}.{suffix}";
        }

        public static string ToSeparatedValue(this SolutionConfigurationAndPlatform value)
        {
            return value.Configuration + '|' + value.Platform;
        }

        public static string ToSeparatedValue(this ProjectConfigurationAndPlatform value)
        {
            return value.ConfigurationForSolution + '|' + value.PlatformForSolution;
        }

        public static SolutionConfigurationAndPlatform ToSolutuonConfiguration(
            this ProjectConfigurationAndPlatform value)
        {
            return new SolutionConfigurationAndPlatform(value.ConfigurationForSolution, value.PlatformForSolution);
        }
    }
}
