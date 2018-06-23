namespace FlaxEditor.Workspace.Items
{
    public abstract class DefinitionItemVisitor
    {
        public virtual void VisitSolution(SolutionDefinition item)
        {
            VisitCompositeItem(item);
        }

        public virtual void VisitProject(ProjectDefinition item)
        {
            VisitCompositeItem(item);
        }

        public virtual void VisitGlobal(GlobalDefinition item)
        {
            VisitCompositeItem(item);
        }

        public virtual void VisitSection(SectionDefinition item)
        {
            VisitCompositeItem(item);
        }

        public virtual void VisitProperty(PropertyDefinition item)
        {
            VisitItem(item);
        }

        public virtual void VisitLine(LineDefinition item)
        {
            VisitItem(item);
        }

        public virtual void VisitFormatVersion(FormatVersionDefinition item)
        {
            VisitItem(item);
        }

        public virtual void VisitCompositeItem(CompositeDefinitionItem item)
        {
            VisitItem(item);
            foreach (var definitionItem in item.Items)
            {
                definitionItem.Visit(this);
            }
        }

        public virtual void VisitItem(DefinitionItem item)
        {
        }
    }
}
