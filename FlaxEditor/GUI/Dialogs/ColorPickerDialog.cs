// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Dialogs
{
    /// <summary>
    /// The base interface for the color picker dialogs.
    /// </summary>
    public interface IColorPickerDialog
    {
        /// <summary>
        /// Closes the picker (similar to value editing cancel).
        /// </summary>
        void ClosePicker();
    }

    /// <summary>
    /// Color picking dialog.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public class ColorPickerDialog : Dialog, IColorPickerDialog
    {
        private const float BUTTONS_WIDTH = 60.0f;
        private const float PICKER_MARGIN = 6.0f;
        private const float RGBA_MARGIN = 12.0f;
        private const float HSV_MARGIN = 0.0f;
        private const float CHANNELS_MARGIN = 4.0f;
        private const float CHANNEL_TEXT_WIDTH = 12.0f;

        private Color _oldColor;
        private Color _newColor;
        private bool _disableEvents;
        private bool _useDynamicEditing;
        private ColorValueBox.ColorPickerEvent _onChangedOk;

        private ColorSelectorWithSliders _cSelector;
        private IntValueBox _cRed;
        private IntValueBox _cGreen;
        private IntValueBox _cBlue;
        private IntValueBox _cAlpha;
        private IntValueBox _cHue;
        private IntValueBox _cSaturation;
        private IntValueBox _cValue;
        private TextBox _cHex;
        private Button _cCancel;
        private Button _cOK;

        /// <summary>
        /// Gets the selected color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color SelectedColor
        {
            get => _newColor;
            private set
            {
                if (_disableEvents || value == _newColor)
                    return;

                _disableEvents = true;
                _newColor = value;

                // Selector
                _cSelector.Color = _newColor;

                // RGBA
                _cRed.Value = (int)(_newColor.R * byte.MaxValue);
                _cGreen.Value = (int)(_newColor.G * byte.MaxValue);
                _cBlue.Value = (int)(_newColor.B * byte.MaxValue);
                _cAlpha.Value = (int)(_newColor.A * byte.MaxValue);

                // HSV
                var hsv = _newColor.ToHSV();
                _cHue.Value = (int)hsv.X;
                _cSaturation.Value = (int)(hsv.Y * 100);
                _cValue.Value = (int)(hsv.Z * 100);

                // Hex
                _cHex.Text = _newColor.ToHexString();

                // Dynamic value
                if (_useDynamicEditing)
                {
                    _onChangedOk?.Invoke(_newColor, true);
                }

                _disableEvents = false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerDialog"/> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="colorChanged">The color changed event.</param>
        /// <param name="useDynamicEditing">True if allow dynamic value editing (slider-like usage), otherwise will fire color change event only on editing end.</param>
        public ColorPickerDialog(Color initialValue, ColorValueBox.ColorPickerEvent colorChanged, bool useDynamicEditing)
        : base("Pick a color!")
        {
            _oldColor = initialValue;
            _useDynamicEditing = useDynamicEditing;
            _newColor = Color.Transparent;
            _onChangedOk = colorChanged;

            // Selector
            _cSelector = new ColorSelectorWithSliders(180, 18);
            _cSelector.Location = new Vector2(PICKER_MARGIN, PICKER_MARGIN);
            _cSelector.ColorChanged += x => SelectedColor = x;
            _cSelector.Parent = this;

            // Red
            _cRed = new IntValueBox(0, _cSelector.Right + PICKER_MARGIN + RGBA_MARGIN + CHANNEL_TEXT_WIDTH, PICKER_MARGIN, 100, 0, 255);
            _cRed.ValueChanged += OnRGBAChanged;
            _cRed.Parent = this;

            // Green
            _cGreen = new IntValueBox(0, _cRed.X, _cRed.Bottom + CHANNELS_MARGIN, _cRed.Width, 0, 255);
            _cGreen.ValueChanged += OnRGBAChanged;
            _cGreen.Parent = this;

            // Blue
            _cBlue = new IntValueBox(0, _cRed.X, _cGreen.Bottom + CHANNELS_MARGIN, _cRed.Width, 0, 255);
            _cBlue.ValueChanged += OnRGBAChanged;
            _cBlue.Parent = this;

            // Alpha
            _cAlpha = new IntValueBox(0, _cRed.X, _cBlue.Bottom + CHANNELS_MARGIN, _cRed.Width, 0, 255);
            _cAlpha.ValueChanged += OnRGBAChanged;
            _cAlpha.Parent = this;

            // Hue
            _cHue = new IntValueBox(0, PICKER_MARGIN + HSV_MARGIN + CHANNEL_TEXT_WIDTH, _cSelector.Bottom + PICKER_MARGIN, 100, 0, 360);
            _cHue.ValueChanged += OnHSVChanged;
            _cHue.Parent = this;

            // Saturation
            _cSaturation = new IntValueBox(0, _cHue.X, _cHue.Bottom + CHANNELS_MARGIN, _cHue.Width, 0, 100);
            _cSaturation.ValueChanged += OnHSVChanged;
            _cSaturation.Parent = this;

            // Value
            _cValue = new IntValueBox(0, _cHue.X, _cSaturation.Bottom + CHANNELS_MARGIN, _cHue.Width, 0, 100);
            _cValue.ValueChanged += OnHSVChanged;
            _cValue.Parent = this;

            // Set valid dialog size based on UI content
            Size = new Vector2(_cRed.Right + PICKER_MARGIN, 300);

            // Hex
            const float hexTextBoxWidth = 80;
            _cHex = new TextBox(false, Width - hexTextBoxWidth - PICKER_MARGIN, _cSelector.Bottom + PICKER_MARGIN, hexTextBoxWidth);
            _cHex.Parent = this;
            _cHex.EditEnd += OnHexChanged;

            // Cancel
            _cCancel = new Button(Width - BUTTONS_WIDTH - PICKER_MARGIN, Height - Button.DefaultHeight - PICKER_MARGIN, BUTTONS_WIDTH)
            {
                Text = "Cancel",
                Parent = this
            };
            _cCancel.Clicked += OnCancelClicked;

            // OK
            _cOK = new Button(_cCancel.Left - BUTTONS_WIDTH - PICKER_MARGIN, _cCancel.Y, BUTTONS_WIDTH)
            {
                Text = "Ok",
                Parent = this
            };
            _cOK.Clicked += OnOkClicked;

            // Set initial color
            SelectedColor = initialValue;
        }

        private void OnOkClicked()
        {
            _result = DialogResult.OK;
            _onChangedOk?.Invoke(_newColor, false);
            Close();
        }

        private void OnCancelClicked()
        {
            // Restore color
            if (_useDynamicEditing)
                _onChangedOk?.Invoke(_oldColor, false);

            Close(DialogResult.Cancel);
        }

        private void OnRGBAChanged()
        {
            if (_disableEvents)
                return;

            SelectedColor = new Color((byte)_cRed.Value, (byte)_cGreen.Value, (byte)_cBlue.Value, (byte)_cAlpha.Value);
        }

        private void OnHSVChanged()
        {
            if (_disableEvents)
                return;

            SelectedColor = Color.FromHSV(_cHue.Value, _cSaturation.Value / 100.0f, _cValue.Value / 100.0f, _cAlpha.Value / 255.0f);
        }

        private void OnHexChanged()
        {
            if (_disableEvents)
                return;

            Color color;
            if (Color.TryParseHex(_cHex.Text, out color))
                SelectedColor = color;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var textColor = Color.White;

            base.Draw();

            // RGBA
            var rgbaR = new Rectangle(_cRed.Left - CHANNEL_TEXT_WIDTH, _cRed.Y, 10000, _cRed.Height);
            Render2D.DrawText(style.FontMedium, "R", rgbaR, textColor, TextAlignment.Near, TextAlignment.Center);
            rgbaR.Location.Y = _cGreen.Y;
            Render2D.DrawText(style.FontMedium, "G", rgbaR, textColor, TextAlignment.Near, TextAlignment.Center);
            rgbaR.Location.Y = _cBlue.Y;
            Render2D.DrawText(style.FontMedium, "B", rgbaR, textColor, TextAlignment.Near, TextAlignment.Center);
            rgbaR.Location.Y = _cAlpha.Y;
            Render2D.DrawText(style.FontMedium, "A", rgbaR, textColor, TextAlignment.Near, TextAlignment.Center);

            // HSV left
            var hsvHl = new Rectangle(_cHue.Left - CHANNEL_TEXT_WIDTH, _cHue.Y, 10000, _cHue.Height);
            Render2D.DrawText(style.FontMedium, "H", hsvHl, textColor, TextAlignment.Near, TextAlignment.Center);
            hsvHl.Location.Y = _cSaturation.Y;
            Render2D.DrawText(style.FontMedium, "S", hsvHl, textColor, TextAlignment.Near, TextAlignment.Center);
            hsvHl.Location.Y = _cValue.Y;
            Render2D.DrawText(style.FontMedium, "V", hsvHl, textColor, TextAlignment.Near, TextAlignment.Center);

            // HSV right
            var hsvHr = new Rectangle(_cHue.Right + 2, _cHue.Y, 10000, _cHue.Height);
            Render2D.DrawText(style.FontMedium, "°", hsvHr, textColor, TextAlignment.Near, TextAlignment.Center);
            hsvHr.Location.Y = _cSaturation.Y;
            Render2D.DrawText(style.FontMedium, "%", hsvHr, textColor, TextAlignment.Near, TextAlignment.Center);
            hsvHr.Location.Y = _cValue.Y;
            Render2D.DrawText(style.FontMedium, "%", hsvHr, textColor, TextAlignment.Near, TextAlignment.Center);

            // Hex
            var hex = new Rectangle(_cHex.Left - 26, _cHex.Y, 10000, _cHex.Height);
            Render2D.DrawText(style.FontMedium, "Hex", hex, textColor, TextAlignment.Near, TextAlignment.Center);

            // Color difference
            var newRect = new Rectangle(_cOK.X, _cHex.Bottom + PICKER_MARGIN, _cCancel.Right - _cOK.Left, 0);
            newRect.Size.Y = _cValue.Bottom - newRect.Y;
            Render2D.FillRectangle(newRect, _newColor * _newColor.A);
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Auto cancel on lost focus
            ((WindowRootControl)Root).Window.LostFocus += OnCancelClicked;

            base.OnShow();
        }

        /// <inheritdoc />
        public void ClosePicker()
        {
            OnCancelClicked();
        }
    }
}
