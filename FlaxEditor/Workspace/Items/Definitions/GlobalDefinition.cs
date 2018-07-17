using System.Collections.Generic;
using FlaxEditor.Workspace.Items.Configuration;
using FlaxEditor.Workspace.Items.Definitions.Abstract;

namespace FlaxEditor.Workspace.Items.Definitions
{
    /// <summary>
    /// Global definition of the *.sln file, this will only contain GlobalSection elements
    /// </summary>
    /// <seealso cref="SectionDefinition"/>
    public class GlobalDefinition : CompositeDefinitionItem
    {
        /// <summary>
        /// Constructor for root GlobalSection holder
        /// </summary>
        /// <param name="items">Items to create with</param>
        /// <param name="indent"><see cref="DefinitionItem.Indent"/></param>
        /// <seealso cref="SectionDefinition"/>
        public GlobalDefinition(IList<DefinitionItem> items, int indent)
        : base(items, indent)
        {
        }

        /// <inheritdoc />
        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitGlobal(this);
        }

        /// <summary>
        /// Add new element to Global list
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="sectionValue">Value of the section</param>
        /// <returns></returns>
        public SectionDefinition AddSection(string sectionName, string sectionValue)
        {
            var sectionDefinition = new SectionDefinition(sectionName, sectionValue, SectionDefinitionType.Global, new List<DefinitionItem>(), Indent + 1);
            Add(sectionDefinition);
            return sectionDefinition;
        }
    }
}
