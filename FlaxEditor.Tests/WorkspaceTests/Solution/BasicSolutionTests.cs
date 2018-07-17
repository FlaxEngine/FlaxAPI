using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace;
using FlaxEditor.Workspace.Generation;
using FlaxEditor.Workspace.Items;
using FlaxEditor.Workspace.Items.Configuration;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Utils;
using FlaxEngine;
using NUnit.Framework;

namespace FlaxEditor.Tests.WorkspaceTests
{
    [TestFixture]
    public class BasicSolutionTests
    {
        public static string ApiDirectory;
        public static string FlaxSln;
        public static SolutionDefinition BaseSolutionDefinition;

        [SetUp]
        public void GetSlnData()
        {
            ApiDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "..\\..\\..";
            FlaxSln = ApiDirectory + "\\FlaxAPI.sln";
            if (!File.Exists(FlaxSln))
            {
                Assert.Fail("FlaxAPI.sln do not exists in specified directory: " + FlaxSln);
            }
            BaseSolutionDefinition = SolutionDefinition.Read(FlaxSln);
        }

        [Test]
        public void CompareDataStructures()
        {
            var raw = File.ReadAllText(FlaxSln);
            var result = BaseSolutionDefinition.Write();
            Assert.AreEqual(raw, result);
        }

        [Test]
        public void GetGlobalSeciton()
        {
            var globalDefinitionItems = ((GlobalDefinition)BaseSolutionDefinition.Items.First(item => item.GetType() == typeof(GlobalDefinition))).Items.ToList();
            var globalSectionsTest = BaseSolutionDefinition.GetGlobalSections();
            for (int i = 0; i < globalDefinitionItems.Count(); i++)
            {
                var item = globalDefinitionItems[i];
                var testItem = globalSectionsTest.ToList()[i];
                Assert.AreEqual(item, testItem);
            }
        }

        [Test]
        public void GetGlobalSectionWithName()
        {
            var globalDefinitionItem = (GlobalDefinition)BaseSolutionDefinition.Items.First(item => item.GetType() == typeof(GlobalDefinition));
            var projectConfigurationPlatforms = (SectionDefinition)globalDefinitionItem.Items.First(item => item is SectionDefinition && ((SectionDefinition)item).SectionName == "ProjectConfigurationPlatforms");

            var projectConfigurationPlatformsTest = BaseSolutionDefinition.GetGlobalSections("ProjectConfigurationPlatforms").First();
            Assert.AreEqual(projectConfigurationPlatforms, projectConfigurationPlatformsTest);
        }

        [Test]
        public void GetSolutionConfigurations()
        {
            
            var solution = BaseSolutionDefinition.GetSolutionConfigurations();
            var property = BaseSolutionDefinition.GetGlobalSections("SolutionConfigurationPlatforms").First().AddProperty("Test", "Test CPU");
            var solution2 = BaseSolutionDefinition.GetSolutionConfigurations();

            Assert.AreEqual(solution.Count, solution2.Count - 1);
            Assert.AreEqual(property, solution2.First(platform => platform.Platform == "Test"));
        }

        [Test]
        public void AddNewProject()
        {
            var projectGuid = Guid.NewGuid();
            var projectName = "__TestProjectUsedInUnitTesting";
            var result = BaseSolutionDefinition.AddProject(
                new ProjectDefinitionDescriptor(projectName, Path.Combine(ApiDirectory, projectName),
                                                projectGuid, ProjectDefinitionDescriptor.CSharpProjectConstant,
                                                ProjectConfigurationAndPlatform.CreateDefaultConfigurationAndPlatform()));

            Assert.AreEqual(projectName, result.Name);
            Assert.AreEqual(projectGuid, result.ProjectGuid);
            Assert.AreEqual(ProjectDefinitionDescriptor.CSharpProjectConstant, result.ProjectTypeGuid);
            Assert.AreEqual(BaseSolutionDefinition, result.SolutionDefinition);
            Assert.AreEqual(0, result.Indent);
            Assert.AreEqual(0, result.Items.Count());

            var projectConfigurationPlatforms = BaseSolutionDefinition.GetGlobalSections("ProjectConfigurationPlatforms").First();
            var configs = projectConfigurationPlatforms.Items.Select(item => (PropertyDefinition)item).Where(item => item != null && item.PropertyName.Contains(projectGuid.ToString().ToUpperInvariant())).ToList();

            Assert.AreEqual($"{projectGuid.ToUpperCurlyString()}.Debug|Any CPU.ActiveCfg", configs[0].PropertyName);
            Assert.AreEqual($"{projectGuid.ToUpperCurlyString()}.Debug|Any CPU.Build.0", configs[1].PropertyName);
            Assert.AreEqual($"{projectGuid.ToUpperCurlyString()}.Release|Any CPU.ActiveCfg", configs[2].PropertyName);
            Assert.AreEqual($"{projectGuid.ToUpperCurlyString()}.Release|Any CPU.Build.0", configs[3].PropertyName);
        }

        [Test]
        public void AddNewProjectSolutionDefinitions()
        {
            var projectGuid = Guid.NewGuid();
            var projectName = "__TestProjectUsedInUnitTesting";
            var result = BaseSolutionDefinition.AddProject(
                new ProjectDefinitionDescriptor(projectName, Path.Combine(ApiDirectory, projectName),
                                                projectGuid, ProjectDefinitionDescriptor.CSharpProjectConstant,
                                                ProjectConfigurationAndPlatform.CreateSolutionConfigurationAndPlatform(BaseSolutionDefinition)));

            var sections = BaseSolutionDefinition.GetProjectConfigurations();
            var solution = BaseSolutionDefinition.GetSolutionConfigurations();
        }
    }
}
