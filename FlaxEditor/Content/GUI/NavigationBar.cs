// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;
using FlaxEditor.Windows;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// A <see cref="ContentWindow"/> navigation bar control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Panel" />
    public class NavigationBar : Panel
    {
        /// <summary>
        /// The default buttons margin.
        /// </summary>
        public const float DefaultButtonsMargin = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBar"/> class.
        /// </summary>
        public NavigationBar()
        : base(ScrollBars.Horizontal)
        {
        }

        /// <inheritdoc />
        protected override void Arrange()
        {
            // Arrange buttons
            float x = DefaultButtonsMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                child.X = x;
                x += child.Width + DefaultButtonsMargin;
            }
        }
    }
}
