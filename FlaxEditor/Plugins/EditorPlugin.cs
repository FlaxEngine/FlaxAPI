// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Base class for all plugins used in Editor.
    /// </summary>
    /// <seealso cref="FlaxEngine.Plugin" />
    public abstract class EditorPlugin : Plugin
    {
        /// <summary>
        /// Gets the type of the <see cref="GamePlugin"/> that is related to this plugin. Some plugins may be used only in editor while others want to gave a runtime representation. Use this property to link the related game plugin.
        /// </summary>
        public virtual Type GamePluginType => null;

        /// <summary>
        /// Gets the editor instance. Use it to extend the editor.
        /// </summary>
        public Editor Editor { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            Editor = Editor.Instance;
        }
    }
}
