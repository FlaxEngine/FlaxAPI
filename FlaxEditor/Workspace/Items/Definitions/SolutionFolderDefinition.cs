// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.Workspace.Items.Definitions.Abstract;

namespace FlaxEditor.Workspace.Items.Definitions
{
    /// <summary>
    /// Create definition for in solution folder
    /// </summary>
    public sealed class SolutionFolderDefinition : ProjectDefinition
    {
        /// <summary>
        /// Folder unique GUID
        /// </summary>
        public static readonly Guid TypeGuid = new Guid("{2150E333-8FDC-42A3-9474-1A3956D46DE8}");

        /// <summary>
        /// Create new solution folder definition
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="path"><see cref="Path"/></param>
        /// <param name="projectGuid"><see cref="ProjectDefinition.ProjectGuid"/></param>
        /// <param name="items"><see cref="CompositeDefinitionItem.ItemsList"/></param>
        /// <param name="indent"><inheritdoc cref="DefinitionItem.Indent"/></param>
        public SolutionFolderDefinition(string name, string path, Guid projectGuid,
                                        IList<DefinitionItem> items, int indent)
        : base(name, path, projectGuid, TypeGuid, items, indent)
        {
        }

        /// <summary>
        /// Stringify definition
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"SolutionFolderDefinition {base.ToString()}";
        }

        /// <summary>
        /// TODO Change name
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="indent"></param>
        public override void Dump(TextWriter writer, string indent)
        {
            DumpInternal(writer, indent, "SolutionFolder");
        }
    }
}
