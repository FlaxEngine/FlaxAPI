// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Helper module for engine long-operations progress reporting in editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ProgressReportingModule : EditorModule
    {
        internal ProgressReportingModule(Editor editor)
            : base(editor)
        {
        }
    }
}
