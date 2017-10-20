////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            var tabs = new Tabs
            {
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(48, 48),
                Parent = this
            };

            InitSpawnTab(tabs);
            InitPaintTab(tabs);
            InitFoliageTab(tabs);
            InitCarveTab(tabs);

            tabs.SelectedTabIndex = 0;
        }

        private void InitSpawnTab(Tabs tabs)
        {
            var spawnTab = tabs.AddTab(new Tab(string.Empty, Editor.UI.GetIcon("Add48")));
            var actorGroups = new Tabs
            {
                Orientation = Orientation.Vertical,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = spawnTab
            };

            var groupBasicModels = createGroupWithList(actorGroups, "Basic Models");
            groupBasicModels.AddChild(CreateEditorAssetItem("Cube", "Primitives/Cube.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Sphere", "Primitives/Sphere.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Plane", "Primitives/Plane.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Cylinder", "Primitives/Cylinder.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Cone", "Primitives/Cone.flax"));

            var groupLights = createGroupWithList(actorGroups, "Lights");
            groupLights.AddChild(CreateActorItem("Directional Light", typeof(DirectionalLight)));
            groupLights.AddChild(CreateActorItem("Point Light", typeof(PointLight)));
            groupLights.AddChild(CreateActorItem("Spot Light", typeof(SpotLight)));

            var groupVisuals = createGroupWithList(actorGroups, "Visuals");
            groupVisuals.AddChild(CreateActorItem("Camera", typeof(Camera)));
            groupVisuals.AddChild(CreateActorItem("Environment Probe", typeof(EnvironmentProbe)));
            groupVisuals.AddChild(CreateActorItem("Skybox", typeof(Skybox)));
            groupVisuals.AddChild(CreateActorItem("Sky", typeof(Sky)));

            var groupPhysics = createGroupWithList(actorGroups, "Physics");
            groupPhysics.AddChild(CreateActorItem("Rigid Body", typeof(RigidBody)));
            groupPhysics.AddChild(CreateActorItem("Box Collider", typeof(BoxCollider)));
            groupPhysics.AddChild(CreateActorItem("Sphere Collider", typeof(SphereCollider)));
            groupPhysics.AddChild(CreateActorItem("Capsule Collider", typeof(CapsuleCollider)));

            var groupCsg = createGroupWithList(actorGroups, "CSG");
            groupCsg.AddChild(CreateActorItem("Box Brush", typeof(BoxBrush)));

            var groupVolumes = createGroupWithList(actorGroups, "Volumes");
            groupVolumes.AddChild(CreateActorItem("PostFx Volume", typeof(PostFxVolume)));

            actorGroups.SelectedTabIndex = 0;
        }

        private void InitPaintTab(Tabs tabs)
        {
            var paintTab = tabs.AddTab(new Tab(string.Empty, Editor.UI.GetIcon("Paint48")));
            //paintTab.LinkTooltip("Vertex painint tool"));
        }

        private void InitFoliageTab(Tabs tabs)
        {
            var foliageTab = tabs.AddTab(new Tab(string.Empty, Editor.UI.GetIcon("Foliage48")));
            //foliageTab.LinkTooltip("Foliage spawning tool"));
        }

        private void InitCarveTab(Tabs tabs)
        {
            var carveTab = tabs.AddTab(new Tab(string.Empty, Editor.UI.GetIcon("Mountain48")));
            //carveTab.LinkTooltip("Terrain carving tool"));
        }

        private Item CreateEditorAssetItem(string name, string path)
        {
            path = StringUtils.CombinePaths(Globals.EditorFolder, path);
            return new Item(name, GUI.Drag.DragItems.GetDragData(path));
        }

        private Item CreateActorItem(string name, Type type)
        {
            return new Item(name, GUI.Drag.DragActorType.GetDragData(type));
        }

        private class Item : TreeNode
        {
            private DragData _dragData;

            public Item(string text, DragData dragData = null)
                : this(text, dragData, Sprite.Invalid)
            {
            }

            public Item(string text, DragData dragData, Sprite icon)
                : base(false, icon, icon)
            {
                Text = text;
                _dragData = dragData;
                Height = 20;
            }

            /// <inheritdoc />
            protected override void DoDragDrop()
            {
                if (_dragData != null)
                    DoDragDrop(_dragData);
            }
        }

        ContainerControl createGroupWithList(Tabs parentTabs, string title)
        {
            var tab = parentTabs.AddTab(new Tab(title));
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = tab
            };
            var tree = new Tree(false)
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                RootNodesOffset = -8,
                Parent = panel
            };
            var root = new TreeNode(false)
            {
                Parent = tree
            };
            root.Expand();
            return root;
        }
    }
}
