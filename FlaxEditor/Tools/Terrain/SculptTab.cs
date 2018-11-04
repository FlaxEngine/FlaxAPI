// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Carve tab related to terrain carving. Allows to modify terrain height and visibility using brush.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    internal class SculptTab : Tab
    {
        /// <summary>
        /// The object for sculp mode settings adjusting via Custom Editor.
        /// </summary>
        public sealed class ProxyObject
        {
            // === Tool

            // Mode
            // Strength
            // Target Value (<pick>)
            // 
        }

        /// <summary>
        /// The parent carve tab.
        /// </summary>
        public readonly CarveTab CarveTab;

        /// <summary>
        /// The related sculp terrain gizmo.
        /// </summary>
        public readonly SculptTerrainGizmoMode Gizmo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SculptTab"/> class.
        /// </summary>
        /// <param name="tab">The parent tab.</param>
        /// <param name="gizmo">The related gizmo.</param>
        public SculptTab(CarveTab tab, SculptTerrainGizmoMode gizmo)
        : base("Sculpt")
        {
            CarveTab = tab;
            Gizmo = gizmo;

            // Main panel
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // TODO: use editor undo for changing brush options

            // TODO: implement some UI
        }
    }
}
