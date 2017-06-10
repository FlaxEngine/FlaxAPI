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
    /// Content item that contains C# script file with source code.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentItem" />
    public class ScriptItem : ContentItem
    {
        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Script;
    }
}
