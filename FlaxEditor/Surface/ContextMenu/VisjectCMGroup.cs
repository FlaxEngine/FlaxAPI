////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// Drop panel for group of <see cref="VisjectCMItem"/>. It represents <see cref="GroupArchetype"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.DropPanel" />
    public sealed class VisjectCMGroup : DropPanel
    {
        /// <summary>
        /// The archetype
        /// </summary>
        public readonly GroupArchetype Archetype;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectCMGroup"/> class.
        /// </summary>
        /// <param name="archetype">The group archetype.</param>
        public VisjectCMGroup(GroupArchetype archetype) 
            : base(archetype.Name)
        {
            Archetype = archetype;
        }
    }
}
