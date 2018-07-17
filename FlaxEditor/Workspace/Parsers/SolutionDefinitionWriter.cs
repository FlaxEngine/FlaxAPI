// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System.IO;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace.Generation;
using FlaxEditor.Workspace.Items;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Items.Definitions.Abstract;

namespace FlaxEditor.Workspace.Parsers
{
    /// <summary>
    /// Defines utilities for writing a solution to a string
    /// </summary>
    public static class SolutionDefinitionWriter
    {
        /// <summary>
        /// Converts <see cref="SolutionDefinition"/> to string
        /// </summary>
        /// <param name="solutionDefinition"></param>
        /// <returns></returns>
        public static string Write(SolutionDefinition solutionDefinition)
        {
            using (var writer = new StringWriter())
            {
                var writeVisitor = new WriteVisitor(writer);
                solutionDefinition.Visit(writeVisitor);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Defines a single time visitor that will help writer to create Solution file 
        /// </summary>
        private class WriteVisitor : DefinitionItemVisitor
        {
            /// <summary>
            /// Current String Writer instance
            /// </summary>
            private readonly StringWriter _myWriter;

            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="writer"></param>
            public WriteVisitor(StringWriter writer)
            {
                _myWriter = writer;
            }

            /// <summary>
            /// Visit single project definition imported withint this SLN and write it to writer
            /// </summary>
            /// <param name="item"></param>
            public override void VisitProject(ProjectDefinition item)
            {
                WriteIndent(item);
                //String Format is easier to read
                _myWriter.WriteLine("{4}(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
                    item.ProjectTypeGuid.ToUpperCurlyString(), item.Name, item.Path,
                    item.ProjectGuid.ToUpperCurlyString(), SolutionDefinitionConstants.ProjectPrefix);
                base.VisitProject(item);
                WriteIndent(item);
                _myWriter.WriteLine($"{SolutionDefinitionConstants.End}{SolutionDefinitionConstants.ProjectPrefix}");
            }

            /// <summary>
            /// Write section
            /// </summary>
            /// <param name="item"></param>
            public override void VisitSection(SectionDefinition item)
            {
                WriteIndent(item);
                _myWriter.WriteLine("{0}{1}({2}) = {3}", (object)item.Type,
                    (object)SolutionDefinitionConstants.Section, (object)item.SectionName,
                    (object)item.SectionValue);
                base.VisitSection(item);
                WriteIndent(item);
                _myWriter.WriteLine("{0}{1}{2}", SolutionDefinitionConstants.End, item.Type,
                    SolutionDefinitionConstants.Section);
            }

            /// <summary>
            /// Write global definition
            /// </summary>
            /// <param name="item"></param>
            public override void VisitGlobal(GlobalDefinition item)
            {
                WriteIndent(item);
                _myWriter.WriteLine(SolutionDefinitionConstants.GlobalPrefix);
                base.VisitGlobal(item);
                WriteIndent(item);
                _myWriter.WriteLine("{0}{1}", SolutionDefinitionConstants.End, SolutionDefinitionConstants.GlobalPrefix);
            }

            /// <summary>
            /// Write property definition
            /// </summary>
            /// <param name="item"></param>
            public override void VisitProperty(PropertyDefinition item)
            {
                WriteIndent(item);
                _myWriter.WriteLine("{0} = {1}", item.PropertyName, item.PropertyValue);
            }

            /// <summary>
            /// Write format version
            /// </summary>
            /// <param name="item"></param>
            public override void VisitFormatVersion(FormatVersionDefinition item)
            {
                WriteIndent(item);
                _myWriter.Write(SolutionDefinitionConstants.FormatVersionPrefix);
                _myWriter.WriteLine(item.FormatVersion.VsTechVersion);
                base.VisitFormatVersion(item);
            }

            /// <summary>
            /// Write Line definition
            /// </summary>
            /// <param name="item"></param>
            public override void VisitLine(LineDefinition item)
            {
                WriteIndent(item);
                _myWriter.WriteLine(item.Line);
                base.VisitLine(item);
            }

            /// <summary>
            /// Write an item with its given indent
            /// </summary>
            /// <param name="item">Item to flush</param>
            private void WriteIndent(DefinitionItem item)
            {
                for (var index = 0; index < item.Indent; ++index)
                {
                    _myWriter.Write('\t');
                }
            }
        }
    }
}
