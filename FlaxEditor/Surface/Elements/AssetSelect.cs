// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEngine;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Assets picking control.
    /// </summary>
    /// <seealso cref="AssetPicker" />
    /// <seealso cref="ISurfaceNodeElement" />
    public class AssetSelect : AssetPicker, ISurfaceNodeElement
    {
        /// <inheritdoc />
        public SurfaceNode ParentNode { get; }

        /// <inheritdoc />
        public NodeElementArchetype Archetype { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelect"/> class.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="archetype">The archetype.</param>
        public AssetSelect(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(Utilities.Utils.GetType((ContentDomain)archetype.BoxID), archetype.ActualPosition)
        {
            SelectedID = (Guid)parentNode.Values[archetype.ValueIndex];

            ParentNode = parentNode;
            Archetype = archetype;
        }

        /// <inheritdoc />
        protected override void OnSelectedItemChanged()
        {
            if (ParentNode != null)
            {
                ParentNode.SetValue(Archetype.ValueIndex, SelectedID);
            }

            base.OnSelectedItemChanged();
        }
    }
}
