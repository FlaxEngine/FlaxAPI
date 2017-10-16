////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Sprite Atlas window allows to view and edit <see cref="SpriteAtlas"/> asset.
    /// </summary>
    /// <seealso cref="SpriteAtlas" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class SpriteAtlasWindow : AssetEditorWindowBase<SpriteAtlas>
    {
        // TODO: allow to select and move sprites
        // TODO: restore changes on win close without a changes

        /// <summary>
        /// Atlas view control. Shows sprites.
        /// </summary>
        /// <seealso cref="FlaxEditor.Viewport.Previews.SpriteAtlasPreview" />
        private sealed class AtlasView : SpriteAtlasPreview
        {
            public AtlasView(bool useWidgets)
                : base(useWidgets)
            {
            }

            protected override void DrawTexture(ref Rectangle rect)
            {
                base.DrawTexture(ref rect);

                if (Asset && Asset.IsLoaded)
                {
                    Rectangle area;
                    var count = Asset.SpritesCount;
                    var style = Style.Current;

                    // Draw all splites
                    for (int i = 0; i < count; i++)
                    {
                        Asset.GetSpriteArea(i, out area);

                        Vector2 position = area.Location * rect.Size + rect.Location;
                        Vector2 size = area.Size * rect.Size;

                        Render2D.DrawRectangle(new Rectangle(position, size), style.BackgroundSelected);
                    }
                }
            }
        }

        /// <summary>
        /// The texture properties proxy object.
        /// </summary>
        [CustomEditor(typeof(ProxyEditor))]
        private sealed class PropertiesProxy
        {
            private SpriteAtlasWindow _window;

            public class SpriteEntry
            {
                [HideInEditor]
                public Sprite Sprite;

                public SpriteEntry(SpriteAtlas atlas, int index)
                {
                    Sprite = atlas.GetSprite(index);
                }

                [EditorOrder(0)]
                public string Name
                {
                    get => Sprite.Name;
                    set => Sprite.Name = value;
                }

                [EditorOrder(1), Limit(-4096, 4096)]
                public Vector2 Location
                {
                    get => Sprite.Location;
                    set => Sprite.Location = value;
                }
                
                [EditorOrder(3), Limit(0, 4096)]
                public Vector2 Size
                {
                    get => Sprite.Size;
                    set => Sprite.Size = value;
                }
            }

            [EditorOrder(0), EditorDisplay("Sprites")]
            [CustomEditor(typeof(SpritesColelctionEditor))]
            public SpriteEntry[] Sprites;
            
            [EditorOrder(1000), EditorDisplay("Import Settings", "__inline__")]
            public TextureImportSettings ImportSettings { get; set; } = new TextureImportSettings();

            public sealed class ProxyEditor : GenericEditor
            {
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var window = ((PropertiesProxy)Values[0])._window;

                    base.Initialize(layout);

                    layout.Space(10);
                    var reimportButton = layout.Button("Reimport");
                    reimportButton.Button.Clicked += () => ((PropertiesProxy)Values[0]).Reimport();
                }
            }

            public sealed class SpritesColelctionEditor : CustomEditor
            {
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                public override void Initialize(LayoutElementsContainer layout)
                {
                    var sprites = (SpriteEntry[])Values[0];

                    if (sprites != null)
                    {
                        var elementType = typeof(SpriteEntry);
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            var group = layout.Group(sprites[i].Name);
                            group.Object(new ListValueContainer(elementType, i, Values));
                        }
                    }
                }
            }

            /// <summary>
            /// Updates the sprites colelction.
            /// </summary>
            public void UpdateSprites()
            {
                var atlas = _window.Asset;
                Sprites = new SpriteEntry[atlas.SpritesCount];
                for (int i = 0; i < Sprites.Length; i++)
                {
                    Sprites[i] = new SpriteEntry(atlas, i);
                }
            }

            /// <summary>
            /// Gathers parameters from the specified texture.
            /// </summary>
            /// <param name="win">The texture window.</param>
            public void OnLoad(SpriteAtlasWindow win)
            {
                // Link
                _window = win;
                UpdateSprites();

                // Try to restore target asset texture import options (usefull for fast reimport)
                TextureImportSettings.InternalOptions options;
                if (TextureImportEntry.Internal_GetTextureImportOptions(win.Item.Path, out options))
                {
                    // Restore settings
                    ImportSettings.FromInternal(ref options);
                }

                // Prepare restore data
                PeekState();
            }

            /// <summary>
            /// Records the current state to restore it on DiscardChanges.
            /// </summary>
            public void PeekState()
            {
            }

            /// <summary>
            /// Reimports asset.
            /// </summary>
            public void Reimport()
            {
                Editor.Instance.ContentImporting.Reimport((BinaryAssetItem)_window.Item, ImportSettings);
            }

            /// <summary>
            /// On discard changes
            /// </summary>
            public void DiscardChanges()
            {
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                _window = null;
                Sprites = null;
            }
        }

        private readonly AtlasView _preview;
        private readonly CustomEditorPresenter _propertiesEditor;

        private readonly PropertiesProxy _properties;
        private bool _isWaitingForLoad;

        /// <inheritdoc />
        public SpriteAtlasWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Toolstrip
            _toolstrip.AddButton(1, Editor.UI.GetIcon("Save32")).LinkTooltip("Save");
            _toolstrip.AddButton(2, Editor.UI.GetIcon("Import32")).LinkTooltip("Reimport");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(3, Editor.UI.GetIcon("AddDoc32")).LinkTooltip("Add a new sprite");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(5, Editor.UI.GetIcon("PageScale32")).LinkTooltip("Center view");

            // Split Panel
            var splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Sprite atlas preview
            _preview = new AtlasView(true)
            {
                Parent = splitPanel.Panel1
            };

            // Sprite atlas properties editor
            _propertiesEditor = new CustomEditorPresenter(null);
            _propertiesEditor.Panel.Parent = splitPanel.Panel2;
            _properties = new PropertiesProxy();
            _propertiesEditor.Select(_properties);
            _propertiesEditor.Modified += MarkAsEdited;
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the orginal asset
            if (!IsEdited)
                return;

            // Save asset
            if (Asset.Save())
            {
                // Error
                Editor.Log("Cannot save sprite atlas.");
                return;
            }

            // Update
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void OnToolstripButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                    Save();
                    break;
                case 2:
                    Editor.ContentImporting.Reimport((BinaryAssetItem)Item);
                    break;
                case 3:
                {
                    var sprite = Asset.AddSprite();
                    MarkAsEdited();
                    _properties.UpdateSprites();
                    _propertiesEditor.BuildLayout();
                    break;
                }
                case 5:
                    _preview.CenterView();
                    break;
                default:
                    base.OnToolstripButtonClicked(id);
                    break;
            }
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            if (_toolstrip != null)
            {
                _toolstrip.GetButton(1).Enabled = IsEdited;
            }

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Asset = null;
            _isWaitingForLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Asset = _asset;
            _isWaitingForLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override void OnItemReimported(ContentItem item)
        {
            // Invalidate data
            _isWaitingForLoad = true;
        }

        /// <inheritdoc />
        protected override void OnClose()
        {
            // Discard unsaved changes
            _properties.DiscardChanges();

            base.OnClose();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if need to load
            if (_isWaitingForLoad && _asset.IsLoaded)
            {
                // Clear flag
                _isWaitingForLoad = false;

                // Init properties and parameters proxy
                _properties.OnLoad(this);
                _propertiesEditor.BuildLayout();

                // Setup
                ClearEditedFlag();
            }
        }
    }
}
