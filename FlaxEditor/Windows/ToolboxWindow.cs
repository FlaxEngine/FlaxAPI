////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Tabs;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// A helper utility window with bunch of tools used during scene editing.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class ToolboxWindow : EditorWindow
    {
        /// <summary>
        /// Customized tabs control for the<see cref="ToolboxWindow"/>.
        /// </summary>
        public sealed class ToolboxTabs : Tabs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToolboxTabs"/> class.
            /// </summary>
            public ToolboxTabs()
            {
                DockStyle = DockStyle.Fill;
            }

            /// <inheritdoc />
            public override Vector2 TabsSize => new Vector2(48, 48);

            /// <inheritdoc />
            public override void Draw()
            {
                // Skip default Tabs rendering (draw only child controls)
                DrawChildren();

                // Cache data
                var style = Style.Current;
                var tabSize = TabsSize;
                var tabRect = new Rectangle(Vector2.Zero, tabSize);

                // Header
                Render2D.FillRectangle(new Rectangle(0, 0, Width, tabSize.Y), style.LightBackground);

                // Draw all tabs
                for (int i = 0; i < _tabs.Count; i++)
                {
                    // Check if is selected
                    bool isSelected = i == _selectedIndex;

                    // Draw bar
                    if (isSelected)
                        Render2D.FillRectangle(tabRect, style.BackgroundSelected);
                    else if (IsMouseOver && tabRect.MakeExpanded(-1).Contains(_mosuePosition))
                        Render2D.FillRectangle(tabRect, style.BackgroundHighlighted);

                    // Draw icon
                    Render2D.DrawSprite(_tabs[i].Icon, tabRect.MakeExpanded(-8));

                    // Move
                    tabRect.Offset(tabSize.X, 0);
                }
            }

            /// <inheritdoc />
            protected override void OnSelectedTabChanged()
            {
                base.OnSelectedTabChanged();

                // TODO: hide gizmo or show paint/carve brush or highlight selected model vertex colors or sth else . just change editing mode
            }
        }

        private ToolboxTabs _tabs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public ToolboxWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Toolbox";

            _tabs = new ToolboxTabs();
            _tabs.Parent = this;

            // TODO: fix showing tooltips on tab pages headers

            // Spawn tab
            var spawnTab = _tabs.AddTab(new Tab("Spawn", Editor.UI.GetIcon("Add48")));
            //spawnTab.LinkTooltip(GetSharedTooltip(), "Spawning objects"));
            /*var actorGroups = new CTabsMenu(120);
            //
            //var groupRecentlyCreated = createGroupWithList(actorGroups, "Recently Created"));
            //
            var groupBasicModels = createGroupWithList(actorGroups, "Basic Models"));
            groupBasicModels.AddItem(new DragTextItem("Cube"), DragElement::GetDragData(Globals::Paths::EditorFolder / "Primitives/Cube" + ASSET_FILES_EXTENSION_WITH_DOT)));
            groupBasicModels.AddItem(new DragTextItem("Sphere"), DragElement::GetDragData(Globals::Paths::EditorFolder / "Primitives/Sphere" + ASSET_FILES_EXTENSION_WITH_DOT)));
            groupBasicModels.AddItem(new DragTextItem("Plane"), DragElement::GetDragData(Globals::Paths::EditorFolder / "Primitives/Plane" + ASSET_FILES_EXTENSION_WITH_DOT)));
            groupBasicModels.AddItem(new DragTextItem("Cylinder"), DragElement::GetDragData(Globals::Paths::EditorFolder / "Primitives/Cylinder" + ASSET_FILES_EXTENSION_WITH_DOT)));
            groupBasicModels.AddItem(new DragTextItem("Cone"), DragElement::GetDragData(Globals::Paths::EditorFolder / "Primitives/Cone" + ASSET_FILES_EXTENSION_WITH_DOT)));
            //
            var groupLights = createGroupWithList(actorGroups, "Lights");
            groupLights.AddItem(new DragTextItem("Directional Light", DragActorGetData(DirectionalLight::TypeID)));
            groupLights.AddItem(new DragTextItem("Point Light", DragActorGetData(PointLight::TypeID)));
            groupLights.AddItem(new DragTextItem("Spot Light", DragActorGetData(SpotLight::TypeID)));
            //
            var groupVisuals = createGroupWithList(actorGroups, "Visuals"));
            groupVisuals.AddItem(new DragTextItem("Environment Probe", DragActorGetData(EnvironmentProbe::TypeID)));
            groupVisuals.AddItem(new DragTextItem("Skybox", DragActorGetData(Skybox::TypeID)));
            groupVisuals.AddItem(new DragTextItem("Sky", DragActorGetData(Sky::TypeID)));
            //
            var groupCSG = createGroupWithList(actorGroups, "CSG"));
            groupCSG.AddItem(new DragTextItem("Box Brush", DragActorGetData(BoxBrush::TypeID)));
            //
            var groupPhysics = createGroupWithList(actorGroups, "Physics"));
            groupPhysics.AddItem(new DragTextItem("Physical Object", DragActorGetData(PhysicalActor::TypeID)));
            //
            actorGroups.SetDockStyle(DockStyle::Fill);
            actorGroups.SetParent(spawnTab);
            actorGroups.SelectTab(0);*/

            // Paint tab
            var paintTab = _tabs.AddTab(new Tab("Paint", Editor.UI.GetIcon("Paint48")));
            //paintTab.LinkTooltip(GetSharedTooltip(), "Vertex painint tool"));

            // Foliage tab
            var foliageTab = _tabs.AddTab(new Tab("Foliage", Editor.UI.GetIcon("Foliage48")));
            //foliageTab.LinkTooltip(GetSharedTooltip(), "Foliage spawning tool"));

            // Carve tab
            var carveTab = _tabs.AddTab(new Tab("Carve", Editor.UI.GetIcon("Mountain48")));
            //carveTab.LinkTooltip(GetSharedTooltip(), "Terrain carving tool"));
        }
    }
}
