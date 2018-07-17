using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FlaxEditor.Workspace.Generation;
using FlaxEngine;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace.Items.Configuration;
using FlaxEditor.Workspace.Items.Definitions.Abstract;
using FlaxEditor.Workspace.Parsers;
using FlaxEditor.Workspace.Utils;

namespace FlaxEditor.Workspace.Items.Definitions
{
    public class SolutionDefinition : CompositeDefinitionItem
    {
        private SolutionDefinition(string location, IList<DefinitionItem> items, ICollection<string> errors)
        : base(items, 0)
        {
            Location = location;
            Errors = errors;
        }


        public string Name => Path.GetFileNameWithoutExtension(Location);


        public string Location { get; }


        public string Directory => Path.GetDirectoryName(Location);


        public ICollection<string> Errors { get; }

        public static SolutionDefinition Read(string location)
        {
            var definitionReader = new SolutionDefinitionReader(location);
            definitionReader.Read();
            return new SolutionDefinition(location, definitionReader.Items, definitionReader.Errors);
        }

        public static SolutionDefinition Create(string location, IList<DefinitionItem> items)
        {
            return new SolutionDefinition(location, items, new List<string>());
        }

        public string Write()
        {
            return SolutionDefinitionWriter.Write(this);
        }

        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitSolution(this);
        }

        /// <summary>
        /// Get all projects definitions from current solution definiton
        /// </summary>
        /// <returns>Returns list of all ProjectDefinitions</returns>
        public ICollection<ProjectDefinition> GetProjects()
        {
            return Items.OfType<ProjectDefinition>().ToList();
        }

        /// <summary>
        /// Gets all global sections from the root of solution
        /// </summary>
        /// <returns></returns>
        public ICollection<SectionDefinition> GetGlobalSections()
        {
            var sectionDefinitionList = new List<SectionDefinition>();
            foreach (var globalDefinition in Items.OfType<GlobalDefinition>())
            {
                sectionDefinitionList.AddRange(globalDefinition.Items.OfType<SectionDefinition>());
            }

            foreach (var projectDefinition in Items.OfType<ProjectDefinition>())
            {
                sectionDefinitionList.AddRange(projectDefinition.Items.OfType<SectionDefinition>()
                                                                .Where(x => x.Type == SectionDefinitionType.Global));
            }

            return sectionDefinitionList;
        }

        /// <summary>
        /// Get global section with given name
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public ICollection<SectionDefinition> GetGlobalSections(string sectionName)
        {
            return GetGlobalSections().Where(section => section.SectionName == sectionName).ToList();
        }

        /// <summary>
        /// Gets all properties defined in the solution root
        /// </summary>
        /// <returns></returns>
        public ICollection<PropertyDefinition> GetProperties()
        {
            return Items.OfType<PropertyDefinition>().ToList();
        }

        /// <summary>
        /// Get format version definition for current solution
        /// </summary>
        /// <returns></returns>
        public FormatVersionDefinition GetFormatVersion()
        {
            return Items.OfType<FormatVersionDefinition>().FirstOrDefault();
        }

        /// <summary>
        /// Adds new project to the solution under given folder (or root)
        /// </summary>
        /// <param name="descriptor">Descriptor form which new project will be created</param>
        /// <param name="parent">Parent folder</param>
        /// <returns>Returns injected project definition</returns>
        public ProjectDefinition AddProject(ProjectDefinitionDescriptor descriptor,
                                            ProjectDefinition parent = null)
        {
            var projectDefinition = AddProjectDefinitionInternal(descriptor, parent, (path, items, indent) =>
            {
                if (descriptor.ProjectTypeGuid == SolutionFolderDefinition.TypeGuid)
                {
                    return (ProjectDefinition)new SolutionFolderDefinition(descriptor.Name, path,
                                                                           descriptor.ProjectGuid, items, indent);
                }

                return (ProjectDefinition)new RegularProjectDefinition(descriptor.Name, path, descriptor.ProjectGuid,
                                                                       descriptor.ProjectTypeGuid, items, indent, new EnvironmentVariablesExpander());
            });
            if (descriptor.Configurations.Count == 0)
            {
                return projectDefinition;
            }

            var globalSection = GetOrCreateGlobalSection("ProjectConfigurationPlatforms", "postSolution");
            foreach (var projectConfigurations in SolutionDefinitionConfigurator.BuildNewProjectConfigurationsList(this,
                                                                                                                   projectDefinition, descriptor.Configurations))
            {
                globalSection.AddProperty(projectConfigurations.Item1, projectConfigurations.Item2);
            }

            return projectDefinition;
        }

        /// <summary>
        /// Injects new project defintion at the end of all current project definitions
        /// </summary>
        /// <param name="descriptor">Project description with all global project related data</param>
        /// <param name="parent">Parent folder of the project</param>
        /// <param name="projectFactory">Elements to create new project from</param>
        /// <returns>Returns new project definition injected into Solution</returns>
        private ProjectDefinition AddProjectDefinitionInternal(ProjectDefinitionDescriptor descriptor,
                                                               ProjectDefinition parent,
                                                               Func<string, List<DefinitionItem>, int, ProjectDefinition> projectFactory)
        {
            var lastProjectDefinition = GetProjects().LastOrDefault();

            var definitionItemList = new List<DefinitionItem>();
            var num = lastProjectDefinition?.Indent ?? 0;
            var fileSystemPath = descriptor.Path;
            var str = FlaxEngine.Utils.MakeRelativePath(fileSystemPath, Path.GetDirectoryName(Location));
            var projectToInject = projectFactory(str, definitionItemList, num);

            if (lastProjectDefinition == null)
            {
                var globalDefinition = GetOrCreateGlobalDefinition();
                InsertBefore(projectToInject, globalDefinition);
            }
            else
            {
                InsertAfter(projectToInject, lastProjectDefinition);
            }

            if (parent != null)
            {
                GetOrCreateNestedProjectsSection().AddProperty(PropertyDefinition.ToProperty(descriptor.ProjectGuid),
                                                               PropertyDefinition.ToProperty(parent.ProjectGuid));
            }

            return projectToInject;
        }

        /// <summary>
        /// Removes project with provided definition
        /// </summary>
        /// <param name="projectDefinition">Project to remove</param>
        public void RemoveProject(ProjectDefinition projectDefinition)
        {
            Remove(projectDefinition);
            //Handle removing from nested project section
            var nestedSection = GetGlobalSections("NestedProjects").FirstOrDefault();
            if (nestedSection != null)
            {
                var projectGuid = PropertyDefinition.ToProperty(projectDefinition.ProjectGuid);
                foreach (var propertyDefinition in nestedSection.GetProperties().Where(p =>
                {
                    if (p.PropertyName != projectGuid)
                    {
                        return p.PropertyValue == projectGuid;
                    }

                    return true;
                }).ToList())
                {
                    nestedSection.RemoveProperty(propertyDefinition);
                }
            }
            
            //Handle globals
            var solutionConfig = GetGlobalSections("SolutionConfigurationPlatforms").FirstOrDefault();
            var projectConfig = GetGlobalSections("ProjectConfigurationPlatforms").FirstOrDefault();
            if (solutionConfig == null || projectConfig == null)
            {
                return;
            }

            //Handle given project in found region
            foreach (var propertyDefinition in projectConfig.Items.OfType<PropertyDefinition>()
                                                                 .Where(propertyDefinition => propertyDefinition.PropertyName.ToUpper(CultureInfo.InvariantCulture)
                                                                                                                .StartsWith(projectDefinition.ProjectGuid.ToUpperCurlyString())).ToList())
            {
                projectConfig.RemoveProperty(propertyDefinition);
            }
        }

        /// <summary>
        /// Move project to provided folder with given definition
        /// </summary>
        /// <param name="projectDefinition">Project to move</param>
        /// <param name="parentProjectDefinition">Parent folder to move into</param>
        public void MoveProject(ProjectDefinition projectDefinition,
                                ProjectDefinition parentProjectDefinition)
        {
            var nestedProjectsSection = GetOrCreateNestedProjectsSection();
            var projectGuid = PropertyDefinition.ToProperty(projectDefinition.ProjectGuid);
            foreach (var propertyDefinition in nestedProjectsSection.GetProperties()
                                                                    .Where(p => p.PropertyName == projectGuid).ToList())
            {
                nestedProjectsSection.RemoveProperty(propertyDefinition);
            }

            if (parentProjectDefinition == null)
            {
                return;
            }

            var property = PropertyDefinition.ToProperty(parentProjectDefinition.ProjectGuid);
            nestedProjectsSection.AddProperty(projectGuid, property);
        }

        /// <summary>
        /// Gets first global section of the file or creates new one if one desn't exists
        /// </summary>
        /// <param name="sectionName">Name of the section (see example)</param>
        /// <param name="sectionValue">Value of the section (see example)</param>
        /// <example>
        /// GlobalSection({sectionName}) = {sectionValue}
        /// GlobalSection(SolutionConfigurationPlatforms) = preSolution
        /// </example>
        /// <returns>Returns the global definition</returns>
        public SectionDefinition GetOrCreateGlobalSection(string sectionName, string sectionValue)
        {
            return GetGlobalSections(sectionName).FirstOrDefault() ??
                   GetOrCreateGlobalDefinition().AddSection(sectionName, sectionValue);
        }

        /// <summary>
        /// Create global section for nested project in solution or gets existing one
        /// </summary>
        /// <returns></returns>
        private SectionDefinition GetOrCreateNestedProjectsSection()
        {
            return GetOrCreateGlobalSection("NestedProjects", "preSolution");
        }

        /// <summary>
        /// Gets first global section of the file or creates new one if one desn't exists
        /// </summary>
        /// <returns>Returns the global definition</returns>
        private GlobalDefinition GetOrCreateGlobalDefinition()
        {
            var list = Items.OfType<GlobalDefinition>().ToList();
            if (list.Any())
            {
                return list.First();
            }

            var globalDefinition = new GlobalDefinition(new List<DefinitionItem>(), Indent);
            Add(globalDefinition);
            return globalDefinition;
        }

        //TODO
        public void Dump(TextWriter writer)
        {
            if (Errors.Any())
            {
                foreach (var error in Errors)
                {
                    writer.WriteLine(error);
                }
            }
            else
            {
                var formatVersionDefinition = GetFormatVersion();
                if (formatVersionDefinition == null)
                {
                    throw new NullReferenceException(nameof(formatVersionDefinition) + " cannot be null");
                }
                var original = formatVersionDefinition.FormatVersion.VsTechVersion;
                if (original.EndsWith("00"))
                {
                    original = original.RemoveEnd("0");
                }

                writer.WriteLine("FileVersion: {0}", original);
                writer.WriteLine("Properties:");
                foreach (var property in GetProperties())
                {
                    property.Dump(writer, "  ");
                }

                foreach (var project in GetProjects())
                {
                    project.Dump(writer, "");
                }

                writer.WriteLine("Global:");
                foreach (var globalSection in GetGlobalSections())
                {
                    globalSection.Dump(writer, "  ");
                }
            }
        }
    }
}
