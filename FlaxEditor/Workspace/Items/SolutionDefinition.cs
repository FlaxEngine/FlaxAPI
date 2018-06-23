using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FlaxEditor.Workspace.Generation;

namespace FlaxEditor.Workspace.Items
{
    public class SolutionDefinition : CompositeDefinitionItem
    {
        private SolutionDefinition(FileSystemPath location, IList<DefinitionItem> items, ICollection<string> errors)
        : base(items, 0)
        {
            Location = location;
            Errors = errors;
        }

        
        public string Name => Location.NameWithoutExtension;

        
        public FileSystemPath Location { get; }

        
        public FileSystemPath Directory => Location.Directory;

        
        public ICollection<string> Errors { get; }

        public static SolutionDefinition Read(FileSystemPath location)
        {
            var definitionReader = new SolutionDefinitionReader(location);
            definitionReader.Read();
            return new SolutionDefinition(location, definitionReader.Items, definitionReader.Errors);
        }

        public static SolutionDefinition Create(FileSystemPath location, IList<DefinitionItem> items)
        {
            return new SolutionDefinition(location, items, EmptyList<string>.Instance);
        }

        public string Write()
        {
            return SolutionDefinitionWriter.Write(this);
        }

        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitSolution(this);
        }

        
        public ICollection<ProjectDefinition> GetProjects()
        {
            return Items.OfType<ProjectDefinition>().ToList();
        }

        
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

        
        public ICollection<SectionDefinition> GetGlobalSections( string sectionName)
        {
            return GetGlobalSections().Where(section => section.SectionName == sectionName).ToList();
        }

        
        public ICollection<PropertyDefinition> GetProperties()
        {
            return Items.OfType<PropertyDefinition>().ToList();
        }

        
        public FormatVersionDefinition GetFormatVersion()
        {
            return Items.OfType<FormatVersionDefinition>().FirstOrDefault();
        }

        
        public ProjectDefinition AddProject( ProjectDefinitionDescriptor descriptor,
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
            if (descriptor.Configurations.IsEmpty())
            {
                return projectDefinition;
            }

            var globalSection = GetOrCreateGlobalSection("ProjectConfigurationPlatforms", "postSolution");
            foreach (var projectConfigurations in SolutionDefinitionConfigurator.BuildNewProjectConfigurationsList(this,
                                                                                                                   projectDefinition, descriptor.Configurations))
            {
                globalSection.AddProperty(projectConfigurations.First, projectConfigurations.Second);
            }

            return projectDefinition;
        }

        
        private ProjectDefinition AddProjectDefinitionInternal( ProjectDefinitionDescriptor descriptor,
                                                                ProjectDefinition parent,
                                                                Func<string, List<DefinitionItem>, int, ProjectDefinition> factory)
        {
            var projectDefinition1 = GetProjects().LastOrDefault();
            var definitionItemList = new List<DefinitionItem>();
            var num = projectDefinition1 != null ? projectDefinition1.Indent : 0;
            var fileSystemPath = FileSystemPath.TryParse(descriptor.Path, FileSystemPathInternStrategy.INTERN);
            var str = fileSystemPath.IsEmpty || !fileSystemPath.IsAbsolute
                      ? descriptor.Path
                      : fileSystemPath.MakeRelativeTo(Location.Directory).FullPath;
            var projectDefinition2 = factory(str, definitionItemList, num);
            if (projectDefinition1 == null)
            {
                var globalDefinition = GetOrCreateGlobalDefinition();
                InsertBefore(projectDefinition2, globalDefinition);
            }
            else
            {
                InsertAfter(projectDefinition2, projectDefinition1);
            }

            if (parent != null)
            {
                GetOrCreateNestedProjectsSection().AddProperty(PropertyDefinition.ToProperty(descriptor.ProjectGuid),
                                                               PropertyDefinition.ToProperty(parent.ProjectGuid));
            }

            return projectDefinition2;
        }

        public void RemoveProject( ProjectDefinition projectDefinition)
        {
            Remove(projectDefinition);
            var sectionDefinition1 = GetGlobalSections("NestedProjects").FirstOrDefault();
            if (sectionDefinition1 != null)
            {
                var projectGuid = PropertyDefinition.ToProperty(projectDefinition.ProjectGuid);
                foreach (var propertyDefinition in sectionDefinition1.GetProperties().Where(p =>
                {
                    if (!(p.PropertyName == projectGuid))
                    {
                        return p.PropertyValue == projectGuid;
                    }

                    return true;
                }).ToList())
                {
                    sectionDefinition1.RemoveProperty(propertyDefinition);
                }
            }

            var sectionDefinition2 = GetGlobalSections("SolutionConfigurationPlatforms").FirstOrDefault();
            var sectionDefinition3 = GetGlobalSections("ProjectConfigurationPlatforms").FirstOrDefault();
            if (sectionDefinition2 == null || sectionDefinition3 == null)
            {
                return;
            }

            foreach (var propertyDefinition in sectionDefinition3.Items.OfType<PropertyDefinition>()
                                                                 .Where(propertyDefinition => propertyDefinition.PropertyName.ToUpper(CultureInfo.InvariantCulture)
                                                                                                                .StartsWith(projectDefinition.ProjectGuid.ToUpperCurlyString())).ToList())
            {
                sectionDefinition3.RemoveProperty(propertyDefinition);
            }
        }

        public void MoveProject( ProjectDefinition projectDefinition,
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

        public SectionDefinition GetOrCreateGlobalSection(string sectionName, string sectionValue)
        {
            return GetGlobalSections(sectionName).FirstOrDefault() ??
                   GetOrCreateGlobalDefinition().AddSection(sectionName, sectionValue);
        }

        private SectionDefinition GetOrCreateNestedProjectsSection()
        {
            return GetOrCreateGlobalSection("NestedProjects", "preSolution");
        }

        
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
                var original = GetFormatVersion().NotNull().FormatVersion.VsTechVersion;
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
