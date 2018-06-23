using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlaxEngine.Assertions;

namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Create abstractior for Project definition
    /// </summary>
    public abstract class ProjectDefinition : CompositeDefinitionItem
    {
        /// <summary>
        /// Name of Project displayed in Solution Explorer
        /// </summary>
        /// <example>FlaxEngine</example>
        public readonly string Name;

        /// <summary>
        /// Relative path to the *.sln file to *.csproj for the reference
        /// </summary>
        /// <example>"FlaxEngine\FlaxEngine.csproj"</example>
        public readonly string Path;

        /// <summary>
        /// Unique GUID of the project 
        /// </summary>
        public readonly Guid ProjectGuid;
        /// <summary>
        /// Project type GUID
        /// </summary>
        /// <remarks>This value ussuably will be FAE04EC0-301F-11D3-BF4B-00C04F79EFBC as this is C#. Other project types might have diffrent guid</remarks>
        public readonly Guid ProjectTypeGuid;

        /// <summary>
        /// Default constructor containg all required fields by project definition
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="path"><see cref="Path"/></param>
        /// <param name="projectGuid"><see cref="ProjectGuid"/></param>
        /// <param name="projectTypeGuid"><see cref="ProjectTypeGuid"/></param>
        /// <param name="items"><see cref="CompositeDefinitionItem.ItemsList"/></param>
        /// <param name="indent"><inheritdoc cref="DefinitionItem.Indent"/></param>
        protected ProjectDefinition(string name, string path, Guid projectGuid,
                                    Guid projectTypeGuid, IList<DefinitionItem> items, int indent)
        : base(items, indent)
        {
            Name = name;
            Path = path;
            ProjectGuid = projectGuid;
            ProjectTypeGuid = projectTypeGuid;
        }

        /// <summary>
        /// Parent Solution definition for cross reference
        /// </summary>
        public SolutionDefinition SolutionDefinition
        {
            get
            {
                if(Parent == null)
                {
                    throw new AssertionException();
                }
                return (SolutionDefinition)Parent;
            }
        }


        /// <inheritdoc />
        public sealed override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitProject(this);
        }


        /// <summary>
        /// Get all elements from internal list that are projects
        /// </summary>
        /// <returns>Returns cutout section of the <see cref="ProjectDefinition"/></returns>
        public IEnumerable<SectionDefinition> GetProjectSections()
        {
            return Items.OfType<SectionDefinition>().Where(x => x.Type == SectionDefinitionType.Project);
        }

        /// <summary>
        /// Get all elements from internal list that matches <see cref="sectionName"/>
        /// </summary>
        /// <param name="sectionName">Name of the section to find</param>
        /// <returns>Returns cutout section of the <see cref="ProjectDefinition"/></returns>
        public IEnumerable<SectionDefinition> GetProjectSections(string sectionName)
        {
            return GetProjectSections().Where(section => section.SectionName == sectionName);
        }

        /// <summary>
        /// Get Projects fisrt project that matches <see cref="sectionName"/> and <see cref="sectionValue"/> and return is
        /// <para>If element was not found, create new one with given parameters</para>
        /// </summary>
        /// <param name="sectionName">Name of project to find or create</param>
        /// <param name="sectionValue">Project Section value TODO add better comment?</param>
        /// <returns>Returns definiton of found or created section</returns>
        public SectionDefinition GetOrCreateProjectSection(string sectionName, string sectionValue)
        {
            var firstProjectSection = GetProjectSections().FirstOrDefault(section =>
            {
                if (section.SectionName == sectionName)
                {
                    return section.SectionValue == sectionValue;
                }

                return false;
            });
            if (firstProjectSection != null)
            {
                return firstProjectSection;
            }

            var newProjectSection = new SectionDefinition(sectionName, sectionValue, SectionDefinitionType.Project,
                                                           new List<DefinitionItem>(), Indent);
            Add(newProjectSection);
            return newProjectSection;
        }

        /// <summary>
        /// Removes project section from internal list
        /// </summary>
        /// <param name="sectionDefinition"></param>
        public void RemoveProjectSection(SectionDefinition sectionDefinition)
        {
            Remove(sectionDefinition);
        }

        /// <summary>
        /// Converts Project current to string (see <see cref="Dump"/> for writing to .sln file)
        /// </summary>
        public override string ToString()
        {
            return $"[Name={(object)Name}, Guid={(object)ProjectGuid}, Path={(object)Path}, TypeGuid={(object)ProjectTypeGuid}]";
        }

        /// <summary>
        /// TODO Chagne name 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="indent"></param>
        public abstract void Dump(TextWriter writer, string indent);

        /// <summary>
        /// TODO change name
        /// Writes given <see cref="DefinitionItem"/> to the given TextWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="indent"></param>
        /// <param name="name"></param>
        protected void DumpInternal(TextWriter writer, string indent, string name)
        {
            writer.WriteLine("{0}{1}:", indent, name);
            var indent1 = indent + "  ";
            writer.WriteLine("{0}Name: {1}", indent1, Name);
            writer.WriteLine("{0}Path: {1}", indent1, Path);
            writer.WriteLine("{0}ProjectGuid: {1}", indent1, ProjectGuid);
            writer.WriteLine("{0}ProjectTypeGuid: {1}", indent1, ProjectTypeGuid);
            foreach (var projectSection in GetProjectSections())
            {
                projectSection.Dump(writer, indent1);
            }
        }
    }
}
