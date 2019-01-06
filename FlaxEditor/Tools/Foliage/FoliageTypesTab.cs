// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Foliage types editor tab. Allows to add, remove or modify foliage instance types defined for the current foliage object.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    public class FoliageTypesTab : Tab
    {
        /// <summary>
        /// The object for foliage type settings adjusting via Custom Editor.
        /// </summary>
        [CustomEditor(typeof(ProxyObjectEditor))]
        private sealed class ProxyObject
        {
            /// <summary>
            /// The tab.
            /// </summary>
            [HideInEditor]
            public readonly FoliageTypesTab Tab;

            /// <summary>
            /// The foliage actor.
            /// </summary>
            [HideInEditor]
            public FlaxEngine.Foliage Foliage;

            /// <summary>
            /// The selected foliage instance type index.
            /// </summary>
            [HideInEditor]
            public int SelectedFoliageTypeIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyObject"/> class.
            /// </summary>
            /// <param name="tab">The tab.</param>
            public ProxyObject(FoliageTypesTab tab)
            {
                Tab = tab;
                SelectedFoliageTypeIndex = -1;
            }

            private MaterialBase[] _materials;
            private IntPtr[] _materialsPtr;
            private FoliageTools.InstanceTypeOptions _options;

            public void SyncOptions()
            {
                FoliageTools.GetFoliageTypeOptions(Foliage, SelectedFoliageTypeIndex, out _options);
            }

            public void SetOptions()
            {
                FoliageTools.SetFoliageTypeOptions(Foliage, SelectedFoliageTypeIndex, ref _options);
            }

            //

            [EditorOrder(10), EditorDisplay("Model"), Tooltip("Model to draw by all the foliage instances of this type. It must be unique within the foliage actor and cannot be null.")]
            public Model Model
            {
                get => FoliageTools.GetFoliageTypeModel(Foliage, SelectedFoliageTypeIndex);
                set
                {
                    FoliageTools.SetFoliageTypeModel(Foliage, SelectedFoliageTypeIndex, value);
                    Tab.UpdateFoliageTypesList();

                    // TODO: support undo for editing foliage type properties
                }
            }

            [EditorOrder(20), EditorDisplay("Model"), MemberCollection(ReadOnly = true), Tooltip("Model materials override collection. Can be used to change a specific material of the mesh to the custom one without editing the asset.")]
            public MaterialBase[] Materials
            {
                get
                {
                    if (Model.WaitForLoaded())
                        throw new Exception("Failed to load foliage model.");

                    var size = Model.MaterialSlotsCount;
                    if (_materials == null || _materials.Length != size)
                    {
                        _materials = new MaterialBase[size];
                        _materialsPtr = new IntPtr[size];
                    }

                    FoliageTools.GetFoliageTypeMaterials(Foliage, SelectedFoliageTypeIndex, _materials);

                    return _materials;
                }
                set
                {
                    if (Model.WaitForLoaded())
                        throw new Exception("Failed to load foliage model.");

                    var size = Model.MaterialSlotsCount;
                    if (value == null || value.Length != size)
                        throw new ArgumentException("value", "Inalid materials buffer size for foliage type overrides.");

                    _materials = value;

                    if (_materialsPtr == null || _materialsPtr.Length != size)
                    {
                        _materialsPtr = new IntPtr[size];
                    }

                    for (int i = 0; i < size; i++)
                    {
                        var v = value[i];
                        _materialsPtr[i] = v ? v.unmanagedPtr : IntPtr.Zero;
                    }

                    FoliageTools.SetFoliageTypeMaterials(Foliage, SelectedFoliageTypeIndex, _materialsPtr);
                }
            }

            //

            [EditorOrder(100), EditorDisplay("Instance Options"), Limit(0.0f), Tooltip("The per-instance cull distance.")]
            public float CullDistance
            {
                get => _options.CullDistance;
                set
                {
                    _options.CullDistance = value;
                    SetOptions();
                }
            }

            [EditorOrder(110), EditorDisplay("Instance Options"), Limit(0.0f), Tooltip("The per-instance cull distance randomization range (randomized per instance and added to master CullDistance value).")]
            public float CullDistanceRandomRange
            {
                get => _options.CullDistanceRandomRange;
                set
                {
                    _options.CullDistanceRandomRange = value;
                    SetOptions();
                }
            }

            [EditorOrder(120), EditorDisplay("Instance Options"), Limit(0.0f, 10000.0f, 0.01f), Tooltip("Per foliage type scale factor in lightmap charts. Higher value increases the quality but reduces baking performance.")]
            public float ScaleInLightmap
            {
                get => _options.ScaleInLightmap;
                set
                {
                    _options.ScaleInLightmap = value;
                    SetOptions();
                }
            }

            [EditorOrder(130), EditorDisplay("Instance Options"), Tooltip("The shadows casting mode.")]
            public ShadowsCastingMode ShadowsMode
            {
                get => _options.ShadowsMode;
                set
                {
                    _options.ShadowsMode = value;
                    SetOptions();
                }
            }

            [EditorOrder(140), EditorDisplay("Instance Options"), Tooltip("Determines whenever this meshes can receive decals.")]
            public bool ReceiveDecals
            {
                get => _options.ReceiveDecals != 0;
                set
                {
                    _options.ReceiveDecals = (byte)(value ? 1 : 0);
                    SetOptions();
                }
            }

            [EditorOrder(150), EditorDisplay("Instance Options"), Tooltip("Flag used to determinate whenever use global foliage density scaling for instances of this foliage type.")]
            public bool UseDensityScaling
            {
                get => _options.UseDensityScaling != 0;
                set
                {
                    _options.UseDensityScaling = (byte)(value ? 1 : 0);
                    SetOptions();
                }
            }

            //

            [EditorOrder(200), EditorDisplay("Painting"), Limit(0.0f), Tooltip("The foliage instances density defined in instances count per 1000x1000 units area.")]
            public float Density
            {
                get => _options.PaintDensity;
                set
                {
                    _options.PaintDensity = value;
                    SetOptions();
                }
            }

            [EditorOrder(210), EditorDisplay("Painting"), Limit(0.0f), Tooltip("The minimum radius between foliage instances.")]
            public float Radius
            {
                get => _options.PaintRadius;
                set
                {
                    _options.PaintRadius = value;
                    SetOptions();
                }
            }

            [EditorOrder(215), EditorDisplay("Painting"), Limit(0.0f, 360.0f), Tooltip("The minimum and maximum ground slope angle to paint foliage on it (in degrees).")]
            public Vector2 PaintGroundSlopeAngleRange
            {
                get => new Vector2(_options.PaintGroundSlopeAngleMin, _options.PaintGroundSlopeAngleMax);
                set
                {
                    _options.PaintGroundSlopeAngleMin = value.X;
                    _options.PaintGroundSlopeAngleMax = value.Y;
                    SetOptions();
                }
            }

            [EditorOrder(220), EditorDisplay("Painting"), Tooltip("The scaling mode.")]
            public FoliageTools.ScalingModes Scaling
            {
                get => _options.PaintScaling;
                set
                {
                    _options.PaintScaling = value;
                    SetOptions();
                }
            }

            [EditorOrder(230), EditorDisplay("Painting"), Limit(0.0f), CustomEditor(typeof(ActorTransformEditor.PositionScaleEditor)), Tooltip("The scale minimum values per axis.")]
            public Vector3 ScaleMin
            {
                get => _options.PaintScaleMin;
                set
                {
                    _options.PaintScaleMin = value;
                    SetOptions();
                }
            }

            [EditorOrder(240), EditorDisplay("Painting"), Limit(0.0f), CustomEditor(typeof(ActorTransformEditor.PositionScaleEditor)), Tooltip("The scale maximum values per axis.")]
            public Vector3 ScaleMax
            {
                get => _options.PaintScaleMax;
                set
                {
                    _options.PaintScaleMax = value;
                    SetOptions();
                }
            }

            //

            [EditorOrder(300), EditorDisplay("Placement", "Offset Y"), Tooltip("The per-instance random offset range on axis Y (min-max).")]
            public Vector2 OffsetY
            {
                get => _options.PlacementOffsetY;
                set
                {
                    _options.PlacementOffsetY = value;
                    SetOptions();
                }
            }

            [EditorOrder(310), EditorDisplay("Placement"), Limit(0.0f), Tooltip("The random pitch angle range (uniform in both ways around normal vector).")]
            public float RandomPitchAngle
            {
                get => _options.PlacementRandomPitchAngle;
                set
                {
                    _options.PlacementRandomPitchAngle = value;
                    SetOptions();
                }
            }

            [EditorOrder(320), EditorDisplay("Placement"), Limit(0.0f), Tooltip("The random roll angle range (uniform in both ways around normal vector).")]
            public float RandomRollAngle
            {
                get => _options.PlacementRandomRollAngle;
                set
                {
                    _options.PlacementRandomRollAngle = value;
                    SetOptions();
                }
            }

            [EditorOrder(330), EditorDisplay("Placement"), Tooltip("If checked, instances will be aligned to normal of the placed surface.")]
            public bool AlignToNormal
            {
                get => _options.PlacementAlignToNormal != 0;
                set
                {
                    _options.PlacementAlignToNormal = (byte)(value ? 1 : 0);
                    SetOptions();
                }
            }

            [EditorOrder(340), EditorDisplay("Placement"), Tooltip("If checked, instances will use randomized yaw when placed. Random yaw uses will rotation range over the Y axis.")]
            public bool RandomYaw
            {
                get => _options.PlacementRandomYaw != 0;
                set
                {
                    _options.PlacementRandomYaw = (byte)(value ? 1 : 0);
                    SetOptions();
                }
            }
        }

        /// <summary>
        /// The custom editor for <see cref="ProxyObject"/>.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
        private sealed class ProxyObjectEditor : GenericEditor
        {
            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                base.Initialize(layout);

                var space = layout.Space(22);
                var removeButton = new Button(2, 2.0f, 80.0f, 18.0f)
                {
                    Text = "Remove",
                    TooltipText = "Removes the selected foliage type and all foliage instances using this type",
                    Parent = space.Spacer
                };
                removeButton.Clicked += OnRemoveButtonClicked;
            }

            /// <inheritdoc />
            public override void Refresh()
            {
                // Sync selected foliage options once before update to prevent too many data copies when fetching data from UI properties accessors
                var proxyObject = (ProxyObject)Values[0];
                proxyObject.SyncOptions();

                base.Refresh();
            }

            private void OnRemoveButtonClicked()
            {
                var proxyObject = (ProxyObject)Values[0];
                proxyObject.Tab.RemoveFoliageType(proxyObject.SelectedFoliageTypeIndex);
            }
        }

        private readonly ProxyObject _proxy;
        private readonly VerticalPanel _items;
        private readonly Button _addFoliageTypeButton;
        private readonly CustomEditorPresenter _presenter;
        private int _foliageTypesCount;

        /// <summary>
        /// The parent foliage tab.
        /// </summary>
        public readonly FoliageTab Tab;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoliageTypesTab"/> class.
        /// </summary>
        /// <param name="tab">The parent tab.</param>
        public FoliageTypesTab(FoliageTab tab)
        : base("Foliage Types")
        {
            Tab = tab;
            Tab.SelectedFoliageChanged += OnSelectedFoliageChanged;
            Tab.SelectedFoliageTypeIndexChanged += OnSelectedFoliageTypeIndexChanged;
            Tab.SelectedFoliageTypesChanged += UpdateFoliageTypesList;
            _proxy = new ProxyObject(this);

            // Main panel
            var splitPanel = new SplitPanel(Orientation.Vertical, ScrollBars.Vertical, ScrollBars.Vertical)
            {
                SplitterValue = 0.2f,
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // Foliage types list
            _items = new VerticalPanel
            {
                Y = 4,
                Height = 4,
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = splitPanel.Panel1
            };

            // Foliage add button
            _addFoliageTypeButton = new Button
            {
                Text = "Add Foliage Type",
                TooltipText = "Add new model to use it as a new foliage type for instancing and spawning in the level",
                Parent = splitPanel.Panel1
            };
            _addFoliageTypeButton.Clicked += OnAddFoliageTypeButtonClicked;

            // Options editor
            // TODO: use editor undo for changing foliage type options
            var editor = new CustomEditorPresenter(null, "No foliage type selected");
            editor.Panel.Parent = splitPanel.Panel2;
            editor.Modified += OnModified;
            _presenter = editor;
        }

        private void OnModified()
        {
            Editor.Instance.Scene.MarkSceneEdited(_proxy.Foliage?.Scene);
        }

        private void OnSelectedFoliageChanged()
        {
            _proxy.SelectedFoliageTypeIndex = -1;
            _proxy.Foliage = Tab.SelectedFoliage;

            _presenter.Deselect();

            UpdateFoliageTypesList();
        }

        private void OnSelectedFoliageTypeIndexChanged(int previousIndex, int currentIndex)
        {
            if (previousIndex != -1)
            {
                _items.Children[previousIndex].BackgroundColor = Color.Transparent;
            }

            _proxy.SelectedFoliageTypeIndex = currentIndex;

            if (currentIndex != -1)
            {
                _items.Children[currentIndex].BackgroundColor = Style.Current.BackgroundSelected;

                _presenter.Select(_proxy);
                _presenter.BuildLayoutOnUpdate();
            }
            else
            {
                _presenter.Deselect();
            }
        }

        private void RemoveFoliageType(int index)
        {
            // Deselect if selected
            if (Tab.SelectedFoliageTypeIndex == index)
                Tab.SelectedFoliageTypeIndex = -1;

            var foliage = Tab.SelectedFoliage;
            var action = new Undo.EditFoliageAction(foliage);

            FoliageTools.RemoveFoliageType(foliage, index);

            action.RecordEnd();
            Tab.Editor.Undo.AddAction(action);

            Tab.OnSelectedFoliageTypesChanged();
        }

        private void OnAddFoliageTypeButtonClicked()
        {
            // Show model picker
            AssetSearchPopup.Show(_addFoliageTypeButton, new Vector2(_addFoliageTypeButton.Width * 0.5f, _addFoliageTypeButton.Height), IsItemValidForFoliageModel, OnItemSelectedForFoliageModel);
        }

        private void OnItemSelectedForFoliageModel(AssetItem item)
        {
            var foliage = Tab.SelectedFoliage;
            var model = FlaxEngine.Content.LoadAsync<Model>(item.ID);
            var action = new Undo.EditFoliageAction(foliage);

            FoliageTools.AddFoliageType(foliage, model);
            Editor.Instance.Scene.MarkSceneEdited(foliage.Scene);

            action.RecordEnd();
            Tab.Editor.Undo.AddAction(action);

            Tab.OnSelectedFoliageTypesChanged();

            Tab.SelectedFoliageTypeIndex = FoliageTools.GetFoliageTypesCount(foliage) - 1;
        }

        private bool IsItemValidForFoliageModel(AssetItem item)
        {
            return item is BinaryAssetItem binaryItem && binaryItem.Type == typeof(Model);
        }

        private void UpdateFoliageTypesList()
        {
            var foliage = Tab.SelectedFoliage;

            // Cleanup previous items
            _items.DisposeChildren();

            // Add new ones
            float y = 0;
            if (foliage != null)
            {
                int typesCount = FoliageTools.GetFoliageTypesCount(foliage);
                _foliageTypesCount = typesCount;
                for (int i = 0; i < typesCount; i++)
                {
                    var model = FoliageTools.GetFoliageTypeModel(foliage, i);
                    var asset = Tab.Editor.ContentDatabase.FindAsset(model.ID);
                    var itemView = new AssetSearchPopup.AssetItemView(asset)
                    {
                        TooltipText = asset.NamePath,
                        Tag = i,
                        Parent = _items,
                    };
                    itemView.Clicked += OnFoliageTypeListItemClicked;

                    y += itemView.Height + _items.Spacing;
                }
                y += _items.Margin.Height;
            }
            else
            {
                _foliageTypesCount = 0;
            }
            _items.Height = y;

            var selectedFoliageTypeIndex = Tab.SelectedFoliageTypeIndex;
            if (selectedFoliageTypeIndex != -1)
            {
                _items.Children[selectedFoliageTypeIndex].BackgroundColor = Style.Current.BackgroundSelected;
            }

            ArrangeAddFoliageButton();
        }

        private void OnFoliageTypeListItemClicked(ItemsListContextMenu.Item item)
        {
            Tab.SelectedFoliageTypeIndex = (int)item.Tag;
        }

        private void ArrangeAddFoliageButton()
        {
            _addFoliageTypeButton.Location = new Vector2((_addFoliageTypeButton.Parent.Width - _addFoliageTypeButton.Width) * 0.5f, _items.Bottom + 4);
        }

        internal void CheckFoliageTypesCount()
        {
            var foliage = Tab.SelectedFoliage;
            var count = foliage ? FoliageTools.GetFoliageTypesCount(foliage) : 0;
            if (foliage != null && count != _foliageTypesCount)
            {
                if (Tab.SelectedFoliageTypeIndex >= count)
                    Tab.SelectedFoliageTypeIndex = -1;
                Tab.OnSelectedFoliageTypesChanged();
            }
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            ArrangeAddFoliageButton();
        }
    }
}
