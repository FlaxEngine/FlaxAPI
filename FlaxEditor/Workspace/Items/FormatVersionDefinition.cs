namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Format version definition of the current document
    /// </summary>
    public class FormatVersionDefinition : DefinitionItem
    {
        public readonly SolutionFormatVersion FormatVersion;

        /// <summary>
        /// Format version definition of the current document
        /// </summary>
        /// <param name="formatVersion">Version of the SLN file format</param>
        /// <param name="indent"><see cref="DefinitionItem.Indent"/></param>
        public FormatVersionDefinition(SolutionFormatVersion formatVersion, int indent)
        : base(indent)
        {
            FormatVersion = formatVersion;
        }

        /// <inheritdoc />
        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitFormatVersion(this);
        }
    }
}
