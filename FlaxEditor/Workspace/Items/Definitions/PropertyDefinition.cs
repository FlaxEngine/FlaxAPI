using System;
using System.IO;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace.Items.Definitions.Abstract;

namespace FlaxEditor.Workspace.Items.Definitions
{
    /// <summary>
    /// <see cref="DefinitionItem"/> that is a single property that contains a Name and a Value
    /// </summary>
    public class PropertyDefinition : DefinitionItem
    {
        /// <summary>
        /// Name of this <see cref="ProjectDefinition"/>
        /// </summary>
        public readonly string PropertyName;

        /// <summary>
        /// Value of this <see cref="ProjectDefinition"/> 
        /// </summary>
        public readonly string PropertyValue;

        /// <summary>
        /// Create new not empty property by assigning name and value
        /// </summary>
        /// <param name="propertyName">Name of this property <see cref="PropertyName"/></param>
        /// <param name="propertyValue">Value of this property <see cref="PropertyValue"/></param>
        /// <param name="indent">Indent used by this <see cref="DefinitionItem"/></param>
        public PropertyDefinition(string propertyName, string propertyValue, int indent)
        : base(indent)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        /// <inheritdoc />
        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitProperty(this);
        }

        /// <summary>
        /// TODO Change name
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToProperty(Guid guid)
        {
            return guid.ToUpperCurlyString();
        }

        /// <summary>
        /// TODO change name to Flush
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="indent"></param>
        public void Dump(TextWriter writer, string indent)
        {
            writer.WriteLine("{0}{1} = {2}", indent, PropertyName, PropertyValue);
        }
    }
}
