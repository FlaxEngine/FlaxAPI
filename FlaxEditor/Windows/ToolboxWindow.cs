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
        /// Initializes a new instance of the <see cref="ToolboxWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public ToolboxWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Toolbox";

            var tabs = new Tabs();
            tabs.DockStyle = DockStyle.Fill;
            tabs.TabsSize = new Vector2(48, 48);
            tabs.Parent = this;

            // TODO: fix showing tooltips on tab pages headers

            // TODO: spawn lights using toolbox

            // Spawn tab
            var spawnTab = tabs.AddTab(new Tab("Spawn", Editor.UI.GetIcon("Add48")));
            //spawnTab.LinkTooltip(GetSharedTooltip(), "Spawning objects"));
            var actorGroups = new Tabs();
            actorGroups.Orientation = Orientation.Vertical;
            actorGroups.DockStyle = DockStyle.Fill;
            actorGroups.TabsSize = new Vector2(120, 32);
            actorGroups.Parent = spawnTab;
            //
            //var groupRecentlyCreated = createGroupWithList(actorGroups, "Recently Created"));
            //
            /*var groupBasicModels = createGroupWithList(actorGroups, "Basic Models");
            groupBasicModels.AddItem(new DragTextItem("Cube", DragElement::GetDragData(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cube.flax"))));
            groupBasicModels.AddItem(new DragTextItem("Sphere", DragElement::GetDragData(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Sphere.flax"))));
            groupBasicModels.AddItem(new DragTextItem("Plane", DragElement::GetDragData(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Plane.flax"))));
            groupBasicModels.AddItem(new DragTextItem("Cylinder", DragElement::GetDragData(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cylinder.flax"))));
            groupBasicModels.AddItem(new DragTextItem("Cone", DragElement::GetDragData(StringUtils.CombinePaths(Globals.EditorFolder, "Primitives/Cone.flax"))));
            //
            var groupLights = createGroupWithList(actorGroups, "Lights");
            groupLights.AddItem(new DragTextItem("Directional Light", DragActorGetData(DirectionalLight::TypeID)));
            groupLights.AddItem(new DragTextItem("Point Light", DragActorGetData(PointLight::TypeID)));
            groupLights.AddItem(new DragTextItem("Spot Light", DragActorGetData(SpotLight::TypeID)));
            //
            var groupVisuals = createGroupWithList(actorGroups, "Visuals");
            groupVisuals.AddItem(new DragTextItem("Environment Probe", DragActorGetData(EnvironmentProbe::TypeID)));
            groupVisuals.AddItem(new DragTextItem("Skybox", DragActorGetData(Skybox::TypeID)));
            groupVisuals.AddItem(new DragTextItem("Sky", DragActorGetData(Sky::TypeID)));
            //
            var groupCSG = createGroupWithList(actorGroups, "CSG");
            groupCSG.AddItem(new DragTextItem("Box Brush", DragActorGetData(BoxBrush::TypeID)));
            //
            var groupPhysics = createGroupWithList(actorGroups, "Physics");
            groupPhysics.AddItem(new DragTextItem("Physical Object", DragActorGetData(PhysicalActor::TypeID)));*/

            // Paint tab
            var paintTab = tabs.AddTab(new Tab("Paint", Editor.UI.GetIcon("Paint48")));
            //paintTab.LinkTooltip(GetSharedTooltip(), "Vertex painint tool"));

            // Foliage tab
            var foliageTab = tabs.AddTab(new Tab("Foliage", Editor.UI.GetIcon("Foliage48")));
            //foliageTab.LinkTooltip(GetSharedTooltip(), "Foliage spawning tool"));

            // Carve tab
            var carveTab = tabs.AddTab(new Tab("Carve", Editor.UI.GetIcon("Mountain48")));
            //carveTab.LinkTooltip(GetSharedTooltip(), "Terrain carving tool"));
            
            // Prepare
            actorGroups.SelectTab(0);
            tabs.SelectTab(0);
        }


        /*class DragTextItem : public ItemsListItem
        {
        private string _dragText;

       public DragTextItem(string text, string dragText, Sprite icon = Sprite.Invalid)
            : base(text, icon)
            , _dragText(dragText)
        {
        }
        
        protected override void doDrag()
        {
            DoDragDrop(_dragText);
        }
        };*/

        /*ItemsList createGroupWithList(Tabs parentTabs, string title)
        {
            var tab = parentTabs.AddTab(new Tab(title));
            var panel = new Panel(ScrollBars.Vertical);
            panel.DockStyle = DockStyle.Fill;
            panel.Parent = tab;
            var list = new ItemsList();
            list.Position = Vector2.Zero;
            list.DockStyle = DockStyle.Top;
            list.Parent = panel;
            return list;
        }*/
    }
}
