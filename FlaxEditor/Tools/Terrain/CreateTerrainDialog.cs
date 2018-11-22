// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.ComponentModel;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.Dialogs;
using FlaxEngine;
using FlaxEngine.GUI;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
#pragma warning disable 649
#pragma warning disable 414

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain creator dialog. Allows user to specify initial terrain properties perform proper setup.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public sealed class CreateTerrainDialog : Dialog
    {
        private enum ChunkSizes
        {
            _31 = 31,
            _63 = 63,
            _127 = 127,
            _255 = 255,
        }

        private class Options
        {
            [EditorOrder(100), EditorDisplay("Layout", "Number Of Patches"), DefaultValue(typeof(Int2), "4,4"), Limit(0, 512), Tooltip("Amount of terrain patches in each direction (X and Z). Each terrain patch contains a grid of 16 chunks. Patches can be later added or removed from terrain using a terrain editor tool.")]
            public Int2 NumberOfPatches = new Int2(4, 4);

            [EditorOrder(110), EditorDisplay("Layout"), DefaultValue(ChunkSizes._127), Tooltip("The size of the chunk (amount of quads per edge for the highest LOD). Must be power of two minus one (eg. 63).")]
            public ChunkSizes ChunkSize = ChunkSizes._127;

            [EditorOrder(120), EditorDisplay("Layout", "LOD Count"), DefaultValue(6), Limit(1, FlaxEngine.Terrain.MaxLODs), Tooltip("The maximum Level Of Details count. The actual amount of LODs may be lower due to provided chunk size (each LOD has 4 times less quads).")]
            public int LODCount = 6;

            [EditorOrder(130), EditorDisplay("Layout"), DefaultValue(null), Tooltip("The default material used for terrain rendering (chunks can override this). It must have Domain set to terrain.")]
            public MaterialBase Material;

            [EditorOrder(200), EditorDisplay("Collision"), DefaultValue(null), AssetReference(typeof(PhysicalMaterial), true), Tooltip("Terrain default physical material used to define the collider physical properties.")]
            public JsonAsset PhysicalMaterial;

            [EditorOrder(210), EditorDisplay("Collision", "Collision LOD"), DefaultValue(-1), Limit(-1, 100, 0.1f), Tooltip("Terrain geometry LOD index used for collision.")]
            public int CollisionLOD = -1;

            [EditorOrder(300), EditorDisplay("Import Data"), DefaultValue(null), Tooltip("Custom heightmap texture to import. Used as a source for height field values (from channel Red).")]
            public Texture Heightmap;

            [EditorOrder(310), EditorDisplay("Import Data"), DefaultValue(20000.0f), Tooltip("Custom heightmap texture values scale. Applied to adjust the normalized heightmap values into the world units.")]
            public float HeightmapScale = 20000.0f;

            [EditorOrder(320), EditorDisplay("Import Data"), DefaultValue(null), Tooltip("Custom terrain splat map used as a source of the terrain layers weights. Each channel from RGBA is used as an independent layer weight for terrain layers compositing.")]
            public Texture Splatmap1;

            [EditorOrder(330), EditorDisplay("Import Data"), DefaultValue(null), Tooltip("Custom terrain splat map used as a source of the terrain layers weights. Each channel from RGBA is used as an independent layer weight for terrain layers compositing.")]
            public Texture Splatmap2;

            [EditorOrder(400), EditorDisplay("Transform", "Position"), DefaultValue(typeof(Vector3), "0,0,0"), Tooltip("Position of the terrain")]
            public Vector3 Position = Vector3.Zero;

            [EditorOrder(410), EditorDisplay("Transform", "Rotation"), DefaultValue(typeof(Quaternion), "0,0,0,1"), Tooltip("Orientation of the terrain")]
            public Quaternion Orientation = Quaternion.Identity;

            [EditorOrder(420), EditorDisplay("Transform", "Scale"), DefaultValue(typeof(Vector3), "1,1,1"), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("Scale of the terrain")]
            public Vector3 Scale = Vector3.One;
        }

        private readonly Options _options = new Options();
        private bool _isDone;
        private bool _isWorking;
        private FlaxEngine.Terrain _terrain;
        private CustomEditorPresenter _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTerrainDialog"/> class.
        /// </summary>
        public CreateTerrainDialog()
        : base("Create terrain")
        {
            const float TotalWidth = 450;
            const float EditorHeight = 600;
            Width = TotalWidth;

            // Header and help description
            var headerLabel = new Label(0, 0, TotalWidth, 40)
            {
                Text = "New Terrain",
                DockStyle = DockStyle.Top,
                Parent = this,
                Font = new FontReference(Style.Current.FontTitle)
            };
            var infoLabel = new Label(10, headerLabel.Bottom + 5, TotalWidth - 20, 70)
            {
                Text = "Specify options for new terrain.\nIt will be added to the first opened scene.\nMany of the following settings can be adjusted later.\nYou can also create terrain at runtime from code.",
                HorizontalAlignment = TextAlignment.Near,
                Margin = new Margin(7),
                DockStyle = DockStyle.Top,
                Parent = this
            };

            // Buttons
            const float ButtonsWidth = 60;
            const float ButtonsMargin = 8;
            var importButton = new Button(TotalWidth - ButtonsMargin - ButtonsWidth, infoLabel.Bottom - 30, ButtonsWidth)
            {
                Text = "Create",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            importButton.Clicked += OnCreate;
            var cancelButton = new Button(importButton.Left - ButtonsMargin - ButtonsWidth, importButton.Y, ButtonsWidth)
            {
                Text = "Cancel",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            cancelButton.Clicked += OnCancel;

            // Settings editor
            var settingsEditor = new CustomEditorPresenter(null);
            settingsEditor.Panel.Y = infoLabel.Bottom;
            settingsEditor.Panel.Size = new Vector2(TotalWidth, EditorHeight);
            settingsEditor.Panel.DockStyle = DockStyle.Fill;
            settingsEditor.Panel.Parent = this;
            _editor = settingsEditor;

            Size = new Vector2(TotalWidth, settingsEditor.Panel.Bottom);

            settingsEditor.Select(_options);
        }

        private void OnCreate()
        {
            if (_isWorking)
                return;

            var scene = SceneManager.GetScene(0);
            if (scene == null)
                throw new InvalidOperationException("No scene found to add terrain to it!");

            // Create terrain object and setup some options
            var terrain = FlaxEngine.Terrain.New();
            terrain.Setup(_options.LODCount, (int)_options.ChunkSize);
            terrain.Transform = new Transform(_options.Position, _options.Orientation, _options.Scale);
            terrain.Material = _options.Material;
            terrain.PhysicalMaterial = _options.PhysicalMaterial;
            terrain.CollisionLOD = _options.CollisionLOD;

            // Add to scene (even if generation fails user gets a terrain in the scene)
            terrain.Parent = scene;
            Editor.Instance.Scene.MarkSceneEdited(scene);

            // Show loading label
            var label = new Label
            {
                DockStyle = DockStyle.Fill,
                Text = "Generating terrain...",
                BackgroundColor = Color.Black * 0.6f,
                Parent = this,
            };

            // Lock UI
            _editor.Panel.Enabled = false;
            _isWorking = true;
            _isDone = false;

            // Start async work
            _terrain = terrain;
            var thread = new System.Threading.Thread(Generate);
            thread.Name = "Terrain Generator";
            thread.Start();
        }

        private void Generate()
        {
            _isWorking = true;
            _isDone = false;

            // Call tool to generate the terrain patches from the input data
            if (TerrainTools.GenerateTerrain(_terrain, ref _options.NumberOfPatches, _options.Heightmap, _options.HeightmapScale, _options.Splatmap1, _options.Splatmap2))
            {
                Editor.LogError("Failed to generate terrain. See log for more info.");
            }

            _isWorking = false;
            _isDone = true;
        }

        private void OnCancel()
        {
            if (_isWorking)
                return;

            Close(DialogResult.Cancel);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            if (_isDone)
            {
                _terrain = null;
                _isDone = false;
                Close(DialogResult.OK);
                return;
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        protected override bool CanCloseWindow(ClosingReason reason)
        {
            if (_isWorking && reason == ClosingReason.User)
                return false;

            return base.CanCloseWindow(reason);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (_isWorking)
                return true;

            switch (key)
            {
            case Keys.Escape:
                OnCancel();
                return true;
            case Keys.Return:
                OnCreate();
                return true;
            }

            return base.OnKeyDown(key);
        }
    }
}
