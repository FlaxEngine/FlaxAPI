// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI;
using FlaxEditor.Tools.Terrain;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// A helper utility window with bunch of tools used during scene editing.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public class ToolboxWindow : EditorWindow
    {
        /// <summary>
        /// Gets the tabs control used by this window. Can be used to add custom toolbox modes.
        /// </summary>
        public Tabs TabsControl { get; private set; }

        /// <summary>
        /// The items spawning tab.
        /// </summary>
        public Tab Spawn;

        /// <summary>
        /// The terrain carving tab.
        /// </summary>
        public CarveTab Carve;

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
            TabsControl = new Tabs
            {
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(48, 48),
                Parent = this
            };

            InitSpawnTab(TabsControl);
            InitPaintTab(TabsControl);
            InitFoliageTab(TabsControl);
            InitCarveTab(TabsControl);

            TabsControl.SelectedTabIndex = 0;
        }

        private void InitSpawnTab(Tabs tabs)
        {
            Spawn = tabs.AddTab(new Tab(string.Empty, Editor.Icons.Add48));
            Spawn.Selected += (tab) => Editor.Windows.EditWin.Viewport.SetActiveMode<TransformGizmoMode>();
            var actorGroups = new Tabs
            {
                Orientation = Orientation.Vertical,
                UseScroll = true,
                DockStyle = DockStyle.Fill,
                TabsSize = new Vector2(120, 32),
                Parent = Spawn
            };

            var groupBasicModels = CreateGroupWithList(actorGroups, "Basic Models");
            groupBasicModels.AddChild(CreateEditorAssetItem("Cube", "Primitives/Cube.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Sphere", "Primitives/Sphere.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Plane", "Primitives/Plane.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Cylinder", "Primitives/Cylinder.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Cone", "Primitives/Cone.flax"));
            groupBasicModels.AddChild(CreateEditorAssetItem("Capsule", "Primitives/Capsule.flax"));

            var groupLights = CreateGroupWithList(actorGroups, "Lights");
            groupLights.AddChild(CreateActorItem("Directional Light", typeof(DirectionalLight)));
            groupLights.AddChild(CreateActorItem("Point Light", typeof(PointLight)));
            groupLights.AddChild(CreateActorItem("Spot Light", typeof(SpotLight)));
            groupLights.AddChild(CreateActorItem("Sky Light", typeof(SkyLight)));

            var groupVisuals = CreateGroupWithList(actorGroups, "Visuals");
            groupVisuals.AddChild(CreateActorItem("Camera", typeof(Camera)));
            groupVisuals.AddChild(CreateActorItem("Environment Probe", typeof(EnvironmentProbe)));
            groupVisuals.AddChild(CreateActorItem("Skybox", typeof(Skybox)));
            groupVisuals.AddChild(CreateActorItem("Sky", typeof(Sky)));
            groupVisuals.AddChild(CreateActorItem("Exponential Height Fog", typeof(ExponentialHeightFog)));
            groupVisuals.AddChild(CreateActorItem("PostFx Volume", typeof(PostFxVolume)));
            groupVisuals.AddChild(CreateActorItem("Decal", typeof(Decal)));

            var groupPhysics = CreateGroupWithList(actorGroups, "Physics");
            groupPhysics.AddChild(CreateActorItem("Rigid Body", typeof(RigidBody)));
            groupPhysics.AddChild(CreateActorItem("Character Controller", typeof(CharacterController)));
            groupPhysics.AddChild(CreateActorItem("Box Collider", typeof(BoxCollider)));
            groupPhysics.AddChild(CreateActorItem("Sphere Collider", typeof(SphereCollider)));
            groupPhysics.AddChild(CreateActorItem("Capsule Collider", typeof(CapsuleCollider)));
            groupPhysics.AddChild(CreateActorItem("Mesh Collider", typeof(MeshCollider)));
            groupPhysics.AddChild(CreateActorItem("Fixed Joint", typeof(FixedJoint)));
            groupPhysics.AddChild(CreateActorItem("Distance Joint", typeof(DistanceJoint)));
            groupPhysics.AddChild(CreateActorItem("Slider Joint", typeof(SliderJoint)));
            groupPhysics.AddChild(CreateActorItem("Spherical Joint", typeof(SphericalJoint)));
            groupPhysics.AddChild(CreateActorItem("Hinge Joint", typeof(HingeJoint)));
            groupPhysics.AddChild(CreateActorItem("D6 Joint", typeof(D6Joint)));

            var groupOther = CreateGroupWithList(actorGroups, "Other");
            groupOther.AddChild(CreateActorItem("Animated Model", typeof(AnimatedModel)));
            groupOther.AddChild(CreateActorItem("Bone Socket", typeof(BoneSocket)));
            groupOther.AddChild(CreateActorItem("CSG Box Brush", typeof(BoxBrush)));
            groupOther.AddChild(CreateActorItem("Audio Source", typeof(AudioSource)));
            groupOther.AddChild(CreateActorItem("Audio Listener", typeof(AudioListener)));
            groupOther.AddChild(CreateActorItem("Empty Actor", typeof(EmptyActor)));

            var groupGui = CreateGroupWithList(actorGroups, "GUI");
            groupGui.AddChild(CreateActorItem("UI Control", typeof(UIControl)));
            groupGui.AddChild(CreateActorItem("UI Canvas", typeof(UICanvas)));
            groupGui.AddChild(CreateActorItem("Text Render", typeof(TextRender)));

            actorGroups.SelectedTabIndex = 0;
        }

        private void InitPaintTab(Tabs tabs)
        {
            var paintTab = tabs.AddTab(new Tab(string.Empty, Editor.Icons.Paint48));

            var info = paintTab.AddChild<Label>();
            info.Text = "Vertex painting coming soon...";
            info.DockStyle = DockStyle.Fill;
        }

        private void InitFoliageTab(Tabs tabs)
        {
            var foliageTab = tabs.AddTab(new Tab(string.Empty, Editor.Icons.Foliage48));

            var info = foliageTab.AddChild<Label>();
            info.Text = "Foliage spawning coming soon...";
            info.DockStyle = DockStyle.Fill;
        }

        private void InitCarveTab(Tabs tabs)
        {
            Carve = tabs.AddTab(new CarveTab(Editor.Icons.Mountain48, Editor));
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
                TextMargin = new Margin(-5.0f, 2.0f, 2.0f, 2.0f);
            }

            /// <inheritdoc />
            protected override void DoDragDrop()
            {
                if (_dragData != null)
                    DoDragDrop(_dragData);
            }
        }

        private ContainerControl CreateGroupWithList(Tabs parentTabs, string title)
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
                Parent = panel
            };
            return tree;
        }
    }
}
