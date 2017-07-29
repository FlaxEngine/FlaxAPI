////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEngine;

namespace FlaxEditor.Windows
{
    public partial class ContentWindow
    {
        private void ShowContextMenuForItem(ContentItem item, ref Vector2 location)
        {
            MessageBox.Show("show cm for " + item?.ShortName);
        }
    }
}
