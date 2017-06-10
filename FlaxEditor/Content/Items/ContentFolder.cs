////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Represents workspace directory item.
    /// </summary>
    public class ContentFolder : ContentItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFolder"/> class.
        /// </summary>
        /// <param name="path">The path to the item.</param>
        public ContentFolder(string path)
            : base(path)
        {
        }

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Folder;
    }
}
