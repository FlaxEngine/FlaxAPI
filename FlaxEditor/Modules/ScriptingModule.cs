// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Support for editing, compilation and scripts debugging.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ScriptingModule : EditorModule
    {
        internal ScriptingModule(Editor editor)
            : base(editor)
        {
        }
    }
}
