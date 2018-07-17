using FlaxEditor.Workspace.Items.Definitions.Abstract;

namespace FlaxEditor.Workspace.Items.Definitions
{
    /// <summary>
    /// Definition for single line of the item. This is raw for raw format of the file and should be used with caution
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
