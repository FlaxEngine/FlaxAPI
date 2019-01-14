// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Foliage painting tab. Allows to add or remove foliage instances defined for the current foliage object.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    public class PaintTab : Tab
    {
        /// <summary>
        /// The object for foliage painting settings adjusting via Custom Editor.
        /// </summary>
        [CustomEditor(typeof(ProxyObjectEditor))]
        private sealed class ProxyObject
        {
            private readonly PaintFoliageGizmoMode _mode;
            private readonly PaintTab _tab;

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
            /// <param name="mode">The mode.</param>
            public ProxyObject(PaintTab tab, PaintFoliageGizmoMode mode)
            {
                _mode = mode;
                _tab = tab;
                SelectedFoliageTypeIndex = -1;
            }

            private FlaxEngine.Foliage.TypeOptions _options;

            public void SyncOptions()
            {
                Foliage.GetFoliageTypeOptions(SelectedFoliageTypeIndex, out _options);
            }

            public void SetOptions()
            {
                Foliage.SetFoliageTypeOptions(SelectedFoliageTypeIndex, ref _options);
            }

            //

            [EditorOrder(100), EditorDisplay("Brush", EditorDisplayAttribute.InlineStyle)]
            public Brush Brush
            {
                get => _mode.CurrentBrush;
                set { }
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
            public FlaxEngine.Foliage.ScalingModes Scaling
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

            [EditorOrder(330), EditorDisplay("Placement", "Align To Normal"), Tooltip("If checked, instances will be aligned to normal of the placed surface.")]
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
            public override void Refresh()
            {
                // Sync selected foliage options once before update to prevent too many data copies when fetching data from UI properties accessors
                var proxyObject = (ProxyObject)Values[0];
                proxyObject.SyncOptions();

                base.Refresh();
            }
        }

        private readonly ProxyObject _proxy;
        private readonly VerticalPanel _items;
        private readonly CustomEditorPresenter _presenter;

        /// <summary>
        /// The parent foliage tab.
        /// </summary>
        public readonly FoliageTab Tab;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintTab"/> class.
        /// </summary>
        /// <param name="tab">The parent tab.</param>
        /// <param name="mode">The gizmo mode.</param>
        public PaintTab(FoliageTab tab, PaintFoliageGizmoMode mode)
        : base("Paint")
        {
            Tab = tab;
            Tab.SelectedFoliageChanged += OnSelectedFoliageChanged;
            Tab.SelectedFoliageTypeIndexChanged += OnSelectedFoliageTypeIndexChanged;
            Tab.SelectedFoliageTypesChanged += UpdateFoliageTypesList;
            _proxy = new ProxyObject(this, mode);

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
                ((ContainerControl)_items.Children[previousIndex]).Children[1].BackgroundColor = Color.Transparent;
            }

            _proxy.SelectedFoliageTypeIndex = currentIndex;

            if (currentIndex != -1)
            {
                ((ContainerControl)_items.Children[currentIndex]).Children[1].BackgroundColor = Style.Current.BackgroundSelected;

                _presenter.Select(_proxy);
                _presenter.BuildLayoutOnUpdate();
            }
            else
            {
                _presenter.Deselect();
            }
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
                int typesCount = foliage.FoliageTypesCount;
                for (int i = 0; i < typesCount; i++)
                {
                    var model = foliage.GetFoliageTypeModel(i);
                    var asset = Tab.Editor.ContentDatabase.FindAsset(model.ID);

                    var itemPanel = new ContainerControl();

                    var itemCheck = new CheckBox
                    {
                        DockStyle = DockStyle.Left,
                        Width = 18,
                        TooltipText = "If checked, enables painting with this foliage type.",
                        Tag = i,
                        Parent = itemPanel,
                    };

                    // Try restore painting with the given model ID
                    bool itemChecked;
                    if (!Tab.FoliageTypeModelIdsToPaint.TryGetValue(model.ID, out itemChecked))
                    {
                        // Enable by default
                        itemChecked = true;
                        Tab.FoliageTypeModelIdsToPaint[model.ID] = itemChecked;
                    }
                    itemCheck.Checked = itemChecked;
                    itemCheck.StateChanged += OnItemCheckStateChanged;

                    var itemView = new AssetSearchPopup.AssetItemView(asset)
                    {
                        DockStyle = DockStyle.Fill,
                        TooltipText = asset.NamePath,
                        Tag = i,
                        Parent = itemPanel,
                    };
                    itemView.Clicked += OnFoliageTypeListItemClicked;

                    itemPanel.Height = itemView.Height;
                    itemPanel.Parent = _items;

                    itemPanel.UnlockChildrenRecursive();
                    itemPanel.PerformLayout();

                    y += itemPanel.Height + _items.Spacing;
                }
                y += _items.Margin.Height;
            }
            _items.Height = y;

            var selectedFoliageTypeIndex = Tab.SelectedFoliageTypeIndex;
            if (selectedFoliageTypeIndex != -1)
            {
                ((ContainerControl)_items.Children[selectedFoliageTypeIndex]).Children[1].BackgroundColor = Style.Current.BackgroundSelected;
            }
        }

        private void OnItemCheckStateChanged(CheckBox item)
        {
            var index = (int)item.Tag;
            var foliage = Tab.SelectedFoliage;
            var foliageType = foliage.GetFoliageTypeModel(index);
            Tab.FoliageTypeModelIdsToPaint[foliageType.ID] = item.Checked;
        }

        private void OnFoliageTypeListItemClicked(ItemsListContextMenu.Item item)
        {
            Tab.SelectedFoliageTypeIndex = (int)item.Tag;
        }
    }
}
