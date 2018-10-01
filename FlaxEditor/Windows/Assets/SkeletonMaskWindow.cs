// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.GUI;
using FlaxEditor.Viewport.Cameras;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="SkeletonMask"/> asset.
    /// </summary>
    /// <seealso cref="SkeletonMask" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class SkeletonMaskWindow : AssetEditorWindowBase<SkeletonMask>
    {
        /// <summary>
        /// The asset properties proxy object.
        /// </summary>
        [CustomEditor(typeof(ProxyEditor))]
        private sealed class PropertiesProxy
        {
            private SkeletonMaskWindow Window;
            private SkeletonMask Asset;

            [EditorDisplay("Skeleton"), Tooltip("The skinned model asset used for the skeleton mask reference.")]
            public SkinnedModel Skeleton
            {
                get => Window._preview.SkinnedModel;
                set
                {
                    if (value != Window._preview.SkinnedModel)
                    {
                        // Change skeleton, invalidate mask and request UI update
                        Window._preview.SkinnedModel = value;
                        Window._preview.BonesMask = null;
                        Window._propertiesPresenter.BuildLayoutOnUpdate();
                    }
                }
            }

            [HideInEditor]
            public bool[] Mask
            {
                get => Window._preview.BonesMask;
                set => Window._preview.BonesMask = value;
            }

            public void OnLoad(SkeletonMaskWindow window)
            {
                // Link
                Window = window;
                Asset = window.Asset;

                // Get data from the asset
                Skeleton = Asset.Skeleton;
                Mask = Asset.GetMask(Mask);
            }

            public void OnClean()
            {
                // Unlink
                Window = null;
                Asset = null;
            }

            private class ProxyEditor : GenericEditor
            {
                private bool _waitForSkeletonLoaded;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var proxy = (PropertiesProxy)Values[0];

                    if (proxy.Asset == null || !proxy.Asset.IsLoaded)
                    {
                        layout.Label("Loading...");
                        return;
                    }

                    base.Initialize(layout);

                    // Check reference skeleton
                    var skeleton = proxy.Skeleton;
                    if (skeleton == null)
                        return;
                    if (!skeleton.IsLoaded)
                    {
                        // We need to have skeleton loaded for a nodes references
                        _waitForSkeletonLoaded = true;
                        return;
                    }

                    // Init mask if missing or validate it
                    var bones = skeleton.Skeleton;
                    if (bones == null || bones.Length == 0)
                        return;
                    var mask = proxy.Mask;
                    if (mask == null || mask.Length != bones.Length)
                    {
                        mask = proxy.Mask = new bool[bones.Length];
                        for (int i = 0; i < bones.Length; i++)
                            mask[i] = true;
                    }

                    // Skeleton Mask
                    var group = layout.Group("Mask");
                    var tree = group.Tree();

                    for (int i = 0; i < bones.Length; i++)
                    {
                        if (bones[i].ParentIndex == -1)
                        {
                            BuildSkeletonNodeTree(mask, bones, tree, i);
                        }
                    }
                }

                /// <inheritdoc />
                public override void Refresh()
                {
                    if (_waitForSkeletonLoaded)
                    {
                        _waitForSkeletonLoaded = false;
                        RebuildLayout();
                        return;
                    }

                    base.Refresh();
                }

                private void BuildSkeletonNodeTree(bool[] mask, SkeletonBone[] skeleton, ITreeElement layout, int boneIndex)
                {
                    var node = layout.Node(skeleton[boneIndex].Name);
                    node.TreeNode.ClipChildren = false;
                    node.TreeNode.TextMargin = new Margin(20.0f, 2.0f, 2.0f, 2.0f);
                    node.TreeNode.Expand(true);
                    var checkbox = new CheckBox(0, 0, mask[boneIndex])
                    {
                        Height = 16.0f,
                        IsScrollable = false,
                        Tag = boneIndex,
                        Parent = node.TreeNode
                    };
                    checkbox.StateChanged += OnCheckChanged;

                    for (int i = 0; i < skeleton.Length; i++)
                    {
                        if (skeleton[i].ParentIndex == boneIndex)
                        {
                            BuildSkeletonNodeTree(mask, skeleton, node, i);
                        }
                    }
                }

                private void OnCheckChanged(CheckBox checkBox)
                {
                    var proxy = (PropertiesProxy)Values[0];
                    int boneIndex = (int)checkBox.Tag;
                    proxy.Mask[boneIndex] = checkBox.Checked;
                    proxy.Window.MarkAsEdited();
                }
            }
        }

        private readonly SplitPanel _split;
        private readonly AnimatedModelPreview _preview;
        private readonly CustomEditorPresenter _propertiesPresenter;
        private readonly PropertiesProxy _properties;
        private readonly ToolStripButton _saveButton;

        /// <inheritdoc />
        public SkeletonMaskWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(editor.Icons.Save32, Save).LinkTooltip("Save asset to the file");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/animation/skeleton-mask.html")).LinkTooltip("See documentation to learn more");

            // Split Panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Model preview
            _preview = new AnimatedModelPreview(true)
            {
                ViewportCamera = new FPSCamera(),
                ShowBones = true,
                Parent = _split.Panel1
            };

            // Model properties
            _propertiesPresenter = new CustomEditorPresenter(null);
            _propertiesPresenter.Panel.Parent = _split.Panel2;
            _properties = new PropertiesProxy();
            _propertiesPresenter.Select(_properties);
            _propertiesPresenter.Modified += MarkAsEdited;
        }

        /// <inheritdoc />
        public override void Save()
        {
            if (!IsEdited)
                return;

            // Wait until model asset file be fully loaded
            if (_asset.WaitForLoaded())
            {
                // Error
                return;
            }

            // Call asset saving
            if (_asset.Save(_properties.Skeleton, _properties.Mask))
            {
                // Error
                Editor.LogError("Failed to save asset " + _item);
                return;
            }

            // Update
            ClearEditedFlag();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.SkinnedModel = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.SkinnedModel = null;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _properties.OnLoad(this);
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("Split", _split.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("Split"), out value1))
                _split.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _split.SplitterValue = 0.7f;
        }
    }
}
