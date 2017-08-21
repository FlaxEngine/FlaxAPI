////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.GUI.Dialogs;
using FlaxEngine;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// About this product dialog window.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    internal sealed class AboutDialog : Dialog
    {
        /// <inheritdoc />
        public AboutDialog()
            : base("About Flax")
        {
            Size = new Vector2(600, 400);
        }
    }
}
