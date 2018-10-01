// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    public static partial class Animation
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode" /> for the blending multiple animations in 1D.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class MultiBlend1D : SurfaceNode
        {
            private readonly Label _selctedAnimationLabel;
            private readonly ComboBox _selectedAnimation;
            private readonly AssetPicker _animationPicker;
            private readonly Label _animationSpeedLabel;
            private readonly FloatValueBox _animationSpeed;
            private readonly Label _animationXLabel;
            private readonly FloatValueBox _animationX;
            private bool _isUpdatingUI;

            /// <summary>
            /// The maximum animations amount to blend per node.
            /// </summary>
            public const int MaxAnimationsCount = 14;

            /// <inheritdoc />
            public MultiBlend1D(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
                var layoutOffsetY = FlaxEditor.Surface.Constants.LayoutOffsetY;

                _selctedAnimationLabel = new Label(300, 3 * layoutOffsetY, 120.0f, layoutOffsetY);
                _selctedAnimationLabel.HorizontalAlignment = TextAlignment.Near;
                _selctedAnimationLabel.Text = "Selected Animation:";
                _selctedAnimationLabel.Parent = this;

                _selectedAnimation = new ComboBox(_selctedAnimationLabel.X, 4 * layoutOffsetY, _selctedAnimationLabel.Width);
                _selectedAnimation.PopupShowing += OnSelectedAnimationPopupShowing;
                _selectedAnimation.SelectedIndexChanged += OnSelectedAnimationChanged;
                _selectedAnimation.Parent = this;

                var items = new List<string>(MaxAnimationsCount);
                while (items.Count < MaxAnimationsCount)
                    items.Add(string.Empty);
                _selectedAnimation.Items = items;

                _animationPicker = new AssetPicker(typeof(FlaxEngine.Animation), new Vector2(_selectedAnimation.Left, _selectedAnimation.Bottom + 4));
                _animationPicker.SelectedItemChanged += OnAnimationPickerItemChanged;
                _animationPicker.Parent = this;

                _animationSpeedLabel = new Label(_animationPicker.Left, _animationPicker.Bottom + 4, 40, TextBox.DefaultHeight);
                _animationSpeedLabel.HorizontalAlignment = TextAlignment.Near;
                _animationSpeedLabel.Text = "Speed:";
                _animationSpeedLabel.Parent = this;

                _animationSpeed = new FloatValueBox(1.0f, _animationSpeedLabel.Right + 4, _animationSpeedLabel.Y, _selectedAnimation.Right - _animationSpeedLabel.Right - 4);
                _animationSpeed.SlideSpeed = 0.01f;
                _animationSpeed.ValueChanged += OnAnimationSpeedValueChanged;
                _animationSpeed.Parent = this;

                _animationXLabel = new Label(_animationSpeedLabel.Left, _animationSpeedLabel.Bottom + 4, 40, TextBox.DefaultHeight);
                _animationXLabel.HorizontalAlignment = TextAlignment.Near;
                _animationXLabel.Text = "X:";
                _animationXLabel.Parent = this;

                _animationX = new FloatValueBox(0.0f, _animationXLabel.Right + 4, _animationXLabel.Y, _selectedAnimation.Right - _animationXLabel.Right - 4);
                _animationX.SlideSpeed = 0.01f;
                _animationX.ValueChanged += OnAnimationXChanged;
                _animationX.Parent = this;
            }

            private void OnSelectedAnimationPopupShowing(ComboBox comboBox)
            {
                var items = comboBox.Items;
                items.Clear();
                for (var i = 0; i < MaxAnimationsCount; i++)
                {
                    var animId = (Guid)Values[5 + i * 2];
                    var path = string.Empty;
                    if (FlaxEngine.Content.GetAssetInfo(animId, out _, out path))
                        path = Path.GetFileNameWithoutExtension(path);
                    items.Add(string.Format("[{0}] {1}", i, path));
                }
            }

            private void OnSelectedAnimationChanged(ComboBox comboBox)
            {
                UpdateUI();
            }

            private void OnAnimationPickerItemChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 5 + selectedIndex * 2;
                    SetValue(index, _animationPicker.SelectedID);
                }
            }

            private void OnAnimationSpeedValueChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 4 + selectedIndex * 2;
                    var data0 = (Vector4)Values[index];
                    data0.W = _animationSpeed.Value;
                    SetValue(index, data0);
                }
            }

            private void OnAnimationXChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 4 + selectedIndex * 2;
                    var data0 = (Vector4)Values[index];
                    data0.X = _animationX.Value;
                    SetValue(index, data0);
                }
            }

            private void UpdateUI()
            {
                if (_isUpdatingUI)
                    return;
                _isUpdatingUI = true;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                var isValid = selectedIndex != -1;
                if (isValid)
                {
                    var data0 = (Vector4)Values[4 + selectedIndex * 2];
                    var data1 = (Guid)Values[5 + selectedIndex * 2];

                    _animationPicker.SelectedID = data1;
                    _animationSpeed.Value = data0.W;
                    _animationX.Value = data0.X;

                    var path = string.Empty;
                    if (FlaxEngine.Content.GetAssetInfo(data1, out _, out path))
                        path = Path.GetFileNameWithoutExtension(path);
                    _selectedAnimation.Items[selectedIndex] = string.Format("[{0}] {1}", selectedIndex, path);
                }
                else
                {
                    _animationPicker.SelectedID = Guid.Empty;
                    _animationSpeed.Value = 1.0f;
                    _animationX.Value = 0.0f;
                }
                _animationPicker.Enabled = isValid;
                _animationSpeedLabel.Enabled = isValid;
                _animationSpeed.Enabled = isValid;
                _animationXLabel.Enabled = isValid;
                _animationX.Enabled = isValid;

                _isUpdatingUI = false;
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateUI();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                UpdateUI();
            }
        }
    }
}
