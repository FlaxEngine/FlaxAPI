namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Definition for single line of the item. This is raw format of the .sln file and should not be used
    /// </summary>
    public class LineDefinition : DefinitionItem
    {
        /// <summary>
        /// Text of the current line
        /// </summary>
        public readonly string Line;

        /// <summary>
        /// Default constructor that would take line and its indent
        /// </summary>
        /// <param name="line">Text of the line to create abstraction for</param>
        /// <param name="indent"><see cref="DefinitionItem.Indent"/></param>
        public LineDefinition(string line, int indent)
        : base(indent)
        {
            Line = line;
        }

        /// <inheritdoc />
        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitLine(this);
        }
    }
}
