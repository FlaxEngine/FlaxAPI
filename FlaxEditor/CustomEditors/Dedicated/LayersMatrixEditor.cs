// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content.Settings;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Custom editor for layers matrix editor. Used to define layer-based collision detection for <see cref="PhysicsSettings.LayerMasks"/>
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
    public sealed class LayersMatrixEditor : CustomEditor
    {
        private int _layersCount;
        private List<CheckBox> _checkBoxs = new List<CheckBox>();

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            string[] layerNames = LayersAndTagsSettings.GetCurrentLayers();
            _checkBoxs.Clear();
            int layersCount = Math.Max(4, layerNames.Length);
            _layersCount = layersCount;

            float labelsWidth = 100.0f;
            float labelsHeight = 18;

            var panel = layout.Space(100).Spacer;
            var gridPanel = new GridPanel(0)
            {
                Parent = panel
            };

            var upperLeftCell = new Label
            {
                Parent = gridPanel
            };

            var upperRightCellPanel = new Panel(ScrollBars.None)
            {
                Rotation = 90,
                Pivot = new Vector2(1.0f, 0.0f),
                Parent = gridPanel
            };
            var upperRightCell = new VerticalPanel
            {
                Offset = new Vector2(labelsWidth, 0),
                Spacing = 0,
                TopMargin = 0,
                BottomMargin = 0,
                DockStyle = DockStyle.Fill,
                Parent = upperRightCellPanel
            };

            var bottomLeftCell = new VerticalPanel
            {
                Spacing = 0,
                TopMargin = 0,
                BottomMargin = 0,
                Parent = gridPanel
            };

            var grid = new UniformGridPanel(0)
            {
                SlotsHorizontally = layersCount,
                SlotsVertically = layersCount,
                Parent = gridPanel
            };

            // Set layer names
            int layerIndex = 0;
            for (; layerIndex < layerNames.Length; layerIndex++)
            {
                upperRightCell.AddChild(new Label
                {
                    Height = labelsHeight,
                    Text = layerNames[layerIndex],
                    HorizontalAlignment = TextAlignment.Far,
                });
                bottomLeftCell.AddChild(new Label
                {
                    Height = labelsHeight,
                    Text = layerNames[layerIndex],
                    HorizontalAlignment = TextAlignment.Far,
                });
            }
            for (; layerIndex < layersCount; layerIndex++)
            {
                string name = "Layer " + layerIndex;
                upperRightCell.AddChild(new Label
                {
                    Height = labelsHeight,
                    Text = name,
                    HorizontalAlignment = TextAlignment.Far,
                });
                bottomLeftCell.AddChild(new Label
                {
                    Height = labelsHeight,
                    Text = name,
                    HorizontalAlignment = TextAlignment.Far,
                });
            }

            // Arrange
            panel.Height = gridPanel.Height = gridPanel.Width = labelsWidth + layersCount * 18;
            gridPanel.RowFill[0] = gridPanel.ColumnFill[0] = labelsWidth / gridPanel.Width;
            gridPanel.RowFill[1] = gridPanel.ColumnFill[1] = 1 - gridPanel.ColumnFill[0];

            // Create matrix
            for (int row = 0; row < layersCount; row++)
            {
                int column = 0;
                for (; column < layersCount - row; column++)
                {
                    var box = new CheckBox(0, 0, true)
                    {
                        Tag = new Vector2(_layersCount - column - 1, row),
                        Parent = grid
                    };
                    box.Checked = GetBit(column, row);
                    box.StateChanged += OnCheckBoxChanged;
                    _checkBoxs.Add(box);
                }
                for (; column < layersCount; column++)
                {
                    grid.AddChild(new Label());
                }
            }
        }

        private void OnCheckBoxChanged(CheckBox box)
        {
            int column = (int)((Vector2)box.Tag).X;
            int row = (int)((Vector2)box.Tag).Y;
            SetBit(column, row, box.Checked);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            // Sync check boxes
            for (int i = 0; i < _checkBoxs.Count; i++)
            {
                var box = _checkBoxs[i];
                int column = (int)((Vector2)box.Tag).X;
                int row = (int)((Vector2)box.Tag).Y;
                box.Checked = GetBit(column, row);
            }
        }

        private bool GetBit(int column, int row)
        {
            var values = (int[])Values[0];
            var mask = 1 << column;
            return (values[row] & mask) != 0;
        }

        private void SetBit(int column, int row, bool flag)
        {
            var values = (int[])((int[])Values[0]).Clone();

            values[row] = SetMaskBit(values[row], 1 << column, flag);
            values[column] = SetMaskBit(values[column], 1 << row, flag);

            SetValue(values);
        }

        private static int SetMaskBit(int value, int mask, bool flag)
        {
            return (value & ~mask) | (flag ? mask : 0);
        }
    }
}
