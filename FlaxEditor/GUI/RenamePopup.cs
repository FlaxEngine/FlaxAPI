////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Popup menu useful for renaming objects via UI. Displays text box for renaming.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
    public class RenamePopup : ContextMenuBase
    {
        private string _startValue;
        private TextBox _inputField;
        
        /// <summary>
        /// Occurs when renaming is done.
        /// </summary>
        public event Action<RenamePopup> Renamed;

        /// <summary>
        /// Occurs when popup is closeing (after renaming done or not).
        /// </summary>
        public event Action<RenamePopup> Closed;

        /// <summary>
        /// Gets or sets the initial value.
        /// </summary>
        /// <value>
        /// The initial value.
        /// </value>
        public string InitialValue
        {
            get => _startValue;
            set => _startValue = value;
        }

        /// <summary>
        /// Gets or sets the input field text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get => _inputField.Text;
            set => _inputField.Text = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenamePopup"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        public RenamePopup(string value, Vector2 size, bool isMultiline)
        {
            if (!isMultiline)
                size.Y = TextBox.DefaultHeight;
            Size = size;

            _startValue = value;

            _inputField = new TextBox(isMultiline, 0, 0, size.Y);
            _inputField.DockStyle = DockStyle.Fill;
            _inputField.Text = _startValue;
            _inputField.Parent = this;
        }

        /// <summary>
        /// Shows the rename popup.
        /// </summary>
        /// <param name="control">The target control.</param>
        /// <param name="area">The target control area to cover.</param>
        /// <param name="value">The initial value.</param>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        /// <returns>Created popup.</returns>
        public static RenamePopup Show(Control control, Rectangle area, string value, bool isMultiline)
        {
            var rename = new RenamePopup(value, area.Size, isMultiline);
            rename.Show(control, area.Location + new Vector2(0, (area.Height - rename.Height) * 0.5f));
            return rename;
        }

        private void OnTextChanged()
        {
            var text = Text;
            if (text.Length > 0 && text != _startValue)
            {
                Renamed?.Invoke(this);
                Renamed = null;
            }

            Hide();
        }

        /// <inheritdoc />
        public override bool OnKeyPressed(InputChord key)
        {
            if (key.InvokeFirstCommand(
                new InputChord.KeyCommand(KeyCode.Return, OnTextChanged),
                new InputChord.KeyCommand(KeyCode.Escape, Hide))) return true;

            // Base
            return base.OnKeyPressed(key);
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            _inputField.Focus();
            _inputField.SelectAll();

            base.OnShow();
        }

        /// <inheritdoc />
        protected override void OnHide()
        {
            Closed?.Invoke(this);
            Closed = null;

            base.OnHide();

            // Remove itself
            Dispose();
        }
    }
}
