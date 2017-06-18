////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// Initializes a new instance of the <see cref="NavigationBar"/> class.
        /// </summary>
        public NavigationBar()
            : base(ScrollBars.Horizontal)
        {
        }

        /// <inheritdoc />
        protected override void Arrage()
        {
            // Arrange buttons
            float x = 1;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                child.PerformLayout();
                child.X = x;
                x += child.Width + 2;
            }
        }
    }
}
