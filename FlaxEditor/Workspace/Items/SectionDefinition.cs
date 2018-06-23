using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlaxEditor.Workspace.Items
{
    public partial class SectionDefinition : CompositeDefinitionItem
    {
        public readonly string SectionName;

        public readonly string SectionValue;

        public readonly SectionDefinitionType Type;

        public SectionDefinition(string sectionName, string sectionValue,
            SectionDefinitionType type, IList<DefinitionItem> items, int indent)
            : base(items, indent)
        {
            Type = type;
            SectionName = sectionName;
            SectionValue = sectionValue;
        }


        public IEnumerable<PropertyDefinition> GetProperties()
        {
            return Items.OfType<PropertyDefinition>();
        }


        public IEnumerable<PropertyDefinition> GetProperties(string propertyName)
        {
            return GetProperties().Where(property => property.PropertyName == propertyName);
        }


        public IEnumerable<string> GetPropertyValues(string propertyName)
        {
            return GetProperties().Where(property => property.PropertyName == propertyName)
                .Select(property => property.PropertyValue);
        }

        public override void Visit(DefinitionItemVisitor visitor)
        {
            visitor.VisitSection(this);
        }


        public PropertyDefinition AddProperty(string propertyName, string propertyValue)
        {
            var propertyDefinition1 = GetProperties(propertyName).FirstOrDefault(p => p.PropertyValue == propertyValue);
            if (propertyDefinition1 != null)
            {
                return propertyDefinition1;
            }

            var propertyDefinition2 = new PropertyDefinition(propertyName, propertyValue, Indent + 1);
            Add(propertyDefinition2);
            return propertyDefinition2;
        }


        public PropertyDefinition AddPropertyAfter(string propertyName, string propertyValue,
             DefinitionItem anchor)
        {
            var propertyDefinition1 = GetProperties(propertyName).FirstOrDefault(p => p.PropertyValue == propertyValue);
            if (propertyDefinition1 != null)
            {
                return propertyDefinition1;
            }

            var propertyDefinition2 = new PropertyDefinition(propertyName, propertyValue, Indent + 1);
            InsertAfter(propertyDefinition2, anchor);
            return propertyDefinition2;
        }


        public PropertyDefinition UpdateProperty(PropertyDefinition original, string propertyName,
             string propertyValue)
        {
            var propertyDefinition = new PropertyDefinition(propertyName, propertyValue, Indent + 1);
            InsertAfter(propertyDefinition, original);
            RemoveProperty(original);
            return propertyDefinition;
        }

        public void RemoveProperty(PropertyDefinition propertyDefinition)
        {
            Remove(propertyDefinition);
        }

        public void Dump(TextWriter writer, string indent)
        {
            writer.WriteLine("{0}{1}Section:", indent, Type);
            var str = indent + "  ";
            writer.WriteLine("{0}Name: {1}", str, SectionName);
            writer.WriteLine("{0}Stage: {1}", str, SectionValue);
            writer.WriteLine("{0}{1}SectionProperties:", str, Type);
            var indent1 = str + "  ";
            foreach (var property in GetProperties())
            {
                property.Dump(writer, indent1);
            }
        }
    }
}
