// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Single tab control used by <see cref="Tabs"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Tab : ContainerControl
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public Sprite Icon;
        
        /// <summary>
        /// Occurs when tab gets selected.
        /// </summary>
        public event Action<Tab> Selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="icon">The icon.</param>
        public Tab(Sprite icon)
        : this(string.Empty, icon)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Tab(string text)
        : this(text, Sprite.Invalid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="icon">The icon.</param>
        public Tab(string text, Sprite icon)
        {
            Text = text;
            Icon = icon;
        }

        /// <summary>
        /// Called when tab gets selected.
        /// </summary>
        public virtual void OnSelected()
        {
            Selected?.Invoke(this);
        }
    }
}
