// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.Dialogs;
using FlaxEngine;
using FlaxEngine.GUI;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
#pragma warning disable 649

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain creator dialog. Allows user to specify initial terrain properties perform proper setup.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public sealed class CreateTerrainDialog : Dialog
    {
        private class Options
        {
            [EditorOrder(10), EditorDisplay("Transform", "Position"), DefaultValue(typeof(Vector3), "0,0,0"), Tooltip("Position of the terrain")]
            public Vector3 Position = Vector3.Zero;

            [EditorOrder(20), EditorDisplay("Transform", "Rotation"), DefaultValue(typeof(Quaternion), "0,0,0,1"), Tooltip("Orientation of the terrain")]
            public Quaternion Orientation = Quaternion.Identity;

            [EditorOrder(30), EditorDisplay("Transform", "Scale"), DefaultValue(typeof(Vector3), "1,1,1"), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("Scale of the terrain")]
            public Vector3 Scale = Vector3.One;

            [EditorOrder(100), EditorDisplay("Layout", "Number Of Patches"), DefaultValue(typeof(Int2), "4,4"), Limit(0, 512), Tooltip("Amount of terrain patches in each direction (X and Z). Each terrain patch contains a grid of 16 chunks. Patches can be later added or removed from terrain using a terrain editor tool.")]
            public Int2 NumberOfPatches = new Int2(4, 4);

            // TODO: implement rest of the options

            // ChunkSize
            // LODCount
            // material

            // physical material
            // collision lod
            // HolesThreshold

            // heightmap
            // splatmap
        }

        private readonly Options _options = new Options();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTerrainDialog"/> class.
        /// </summary>
        public CreateTerrainDialog()
        : base("Create terrain")
        {
            const float TotalWidth = 450;
            const float EditorHeight = 450;
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

            Size = new Vector2(TotalWidth, settingsEditor.Panel.Bottom);

            settingsEditor.Select(_options);
        }

        private void OnCreate()
        {
            Close(DialogResult.OK);

            // TODO: create terrain using the given options...
        }

        private void OnCancel()
        {
            Close(DialogResult.Cancel);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
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
