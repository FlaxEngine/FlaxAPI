////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// Single item used for <see cref="VisjectCM"/>. Represents type of the <see cref="NodeArchetype"/> that can be spawned.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public sealed class VisjectCMItem : Control
    {
        public VisjectCMItem(VisjectCMGroup group, NodeArchetype archetype)
            : base(true, 0, 0, 120, 12)
        {
            
        }
}
}
