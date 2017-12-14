////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="CollisionData"/> asset.
    /// </summary>
    /// <seealso cref="CollisionData" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class CollisionDataWindow : AssetEditorWindowBase<CollisionData>
    {
        // TODO: Functionalities
        // show convex shape wires
        // show source model
        // allow to change options (type and source model and params) and cook a new collision data
        
        /// <summary>
        /// The asset properties proxy object.
        /// </summary>
        [CustomEditor(typeof(Editor))]
        private sealed class PropertiesProxy
        {
            private CollisionDataWindow Window;
            private CollisionData Asset;

            [EditorOrder(0), EditorDisplay("General"), Tooltip("Type of the collision data to use")]
            public CollisionDataType Type;

            [EditorOrder(10), EditorDisplay("General"), Tooltip("Source model asset to use for collision data generation")]
            public Model Model;

            [EditorOrder(20), Limit(0, 5), EditorDisplay("General", "Model LOD Index"), Tooltip("Source model LOD index to use for collision data generation (will be clamped to the actual model LODs collection size)")]
            public int ModelLodIndex;

            public class Editor : GenericEditor
            {
                private ButtonElement _cookButton;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    base.Initialize(layout);

                    layout.Space(10);
                    _cookButton = layout.Button("Cook");
                    _cookButton.Button.Clicked += OnCookButtonClicked;
                }

                /// <inheritdoc />
                public override void Refresh()
                {
                    if (_cookButton != null && Values.Count == 1)
                    {
                        var p = (PropertiesProxy)Values[0];
                        _cookButton.Button.Enabled = p.Type != CollisionDataType.None && p.Model != null;
                    }

                    base.Refresh();
                }

                private void OnCookButtonClicked()
                {
                    ((PropertiesProxy)Values[0]).Cook();
                }
            }

            public void Cook()
            {
                FlaxEditor.Editor.CookMeshCollision(Window.Asset.Path, Type, Model, ModelLodIndex);
                Window._preview.Model = Model;
            }

            public void OnLoad(CollisionDataWindow window)
            {
                // Link
                Window = window;
                Asset = window.Asset;

                // Setup cooking parameters
                Type = Asset.Type;
                if (Type == CollisionDataType.None)
                    Type = CollisionDataType.ConvexMesh;
                Model = Asset.Model;
                ModelLodIndex = Asset.ModelLodIndex;
            }

            public void OnClean()
            {
                // Unlink
                Window = null;
                Asset = null;
                Model = null;
            }
        }

        private readonly ModelPreview _preview;
        private readonly CustomEditorPresenter _propertiesPresenter;
        private readonly PropertiesProxy _properties;
        private MeshCollider _meshCollider;
        
        /// <inheritdoc />
        public CollisionDataWindow(Editor editor, AssetItem item)
            : base(editor, item)
        {
            // Split Panel
            var splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Model preview
            _preview = new ModelPreview(true)
            {
                Parent = splitPanel.Panel1
            };
            _preview.Task.Flags |= ViewFlags.PhysicsDebug;

            // Asset properties
            _propertiesPresenter = new CustomEditorPresenter(null);
            _propertiesPresenter.Panel.Parent = splitPanel.Panel2;
            _properties = new PropertiesProxy();
            _propertiesPresenter.Select(_properties);
            
            // Mesh collider actor (used to show the convex/triangle mesh wires)
            _meshCollider = MeshCollider.New();
            _preview.Task.CustomActors.Add(_meshCollider);
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Model = null;
            _meshCollider.CollisionData = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Model = null;
            _meshCollider.CollisionData = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        protected override void OnAssetLoaded()
        {
            _properties.OnLoad(this);
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();
            _preview.Model = _asset.Model;

            base.OnAssetLoaded();
        }

        /// <inheritdoc />
        public override void OnItemReimported(ContentItem item)
        {
            // Refresh the properties (will get new data in OnAssetLoaded)
            _properties.OnClean();
            _propertiesPresenter.BuildLayout();
            ClearEditedFlag();

            base.OnItemReimported(item);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            FlaxEngine.Object.Destroy(ref _meshCollider);
        }
    }
}
