////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.Utilities;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Text Box control which can gather text input from the user
    /// </summary>
    public class TextBox : Control
    {
        /// <summary>
        /// Default height of the text box
        /// </summary>
        protected static int DefaultHeight = 18;

        /// <summary>
        /// Left and right margin for text inside the text box bounds rectangle
        /// </summary>
        protected static int DefaultMargin = 4;

        // TODO: support password protected text box

        protected string _text;

        // State
        private string _onStartEditValue;
        private bool _isEditing;
        private Vector2 _cachedSize;
        private Vector2 _viewOffset;

        // Options
        private bool _isMultiline, _isReadOnly;
        private int _maxLength;

        // Selecting
        private bool _isSelecting, _isCaretOnLeft;
        private int _selectionLeft, _selectionRight;
        private Rectangle _selectionRect;
        private Rectangle _caretRect;
        private float _caretTime;
        private float _selectionTime;

        #region Events

        /// <summary>
        /// Event fired when text gets changed
        /// </summary>
        public Action TextChanged;

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline text box control.
        /// </summary>
        public bool IsMultiline
        {
            get { return _isMultiline; }
            /*set
            {
                if (_isMultiline != value)
                {
                    _isMultiline = value;
                }
            }*/
        }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type into the text box control.
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                if (_maxLength <= 0 || _maxLength > 1000000)
                    throw new ArgumentOutOfRangeException(nameof(MaxLength));

                if (_maxLength != value)
                {
                    _maxLength = value;

                    // Cut too long text
                    if (_text.Length > _maxLength)
                    {
                        Text = _text.Substring(0, _maxLength);
                    }
                }
            }
        }

        /*
        /// <summary>
        /// Determines if the control is in password protect mode.
        /// </summary>
        public virtual bool PasswordProtect
        {
            get { return false; }
        }
        */

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only. 
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            // TOOD: finish change to read only mode, should we do sth else than just change a flag?
            /*set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                }
            }*/
        }

        /// <summary>
        /// Gets or sets text property
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                // Ensure to use only single line
                if (_isMultiline == false && value.Length > 0)
                {
                    // Extract only the first line
                    value = value.GetLines()[0];
                }

                if (_text != value)
                {
                    _text = value;

                    Deselect();
                    OnTextChanged();
                }
            }
        }

        /// <summary>
        /// Gets length of the text
        /// </summary>
        public int TextLength
        {
            get { return _text.Length; }
        }

        /// <summary>
        /// Gets the currently selected text in the control.
        /// </summary>
        public string SelectedText
        {
            get
            {
                int selectedChars = _selectionRight - _selectionLeft;
                Debug.Assert(selectedChars >= 0);
                return _text.Substring(_selectionLeft, selectedChars);
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// </summary>
        public int SelectionLength
        {
            get { return _selectionLeft != -1 ? _selectionRight - _selectionLeft : 0; }
        }

        /// <summary>
        /// Gets text font
        /// </summary>
        protected virtual Font Font
        {
            get { return Style.Current.FontMedium; }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        /// <param name="x">Position X coordinate</param>
        /// <param name="y">Position Y coordinate</param>
        /// <param name="width">Width</param>
        public TextBox(bool isMultiline, float x, float y, float width = 120)
            : base(true, x, y, width, DefaultHeight)
        {
            _isMultiline = isMultiline;
            _maxLength = 32000;
            _selectionLeft = -1;
            _selectionRight = -1;
        }

        /// <summary>
        /// Clears all text from the text box control. 
        /// </summary>
        public void Clear()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Clear selection range
        /// </summary>
        public void ClearSelection()
        {
            endSelecting();
            _selectionLeft = _selectionLeft = -1;
            calSelectionRect();
        }

        /// <summary>
        /// Copies the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Copy()
        {
            throw new NotImplementedException("Copy textbox text");
            // TODO: we need to support Clipboard via Flax API
        }

        /// <summary>
        /// Moves the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Cut()
        {
            throw new NotImplementedException("Cut textbox text");
            // TODO: we need to support Clipboard via Flax API
        }

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            throw new NotImplementedException("Paste textbox text");
            // TODO: we need to support Clipboard via Flax API
        }

        /// <summary>
        /// Ensures that the caret is visible in the TextBox window, by scrolling the TextBox control surface if necessary.
        /// </summary>
        public void ScrollToCaret()
        {
            throw new NotImplementedException("ScrollToCaret");
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            _isCaretOnLeft = true;
            if (TextLength > 0)
            {
                _selectionLeft = 0;
                _selectionRight = TextLength;
            }
            else
            {
                _selectionLeft = _selectionRight = -1;
            }
            _viewOffset = Vector2.Zero;
            calSelectionRect();
        }

        /// <summary>
        /// Sets selection to empty value
        /// </summary>
        public void Deselect()
        {
            _isCaretOnLeft = true;
            _selectionLeft = _selectionRight = -1;
            _viewOffset = Vector2.Zero;
            calSelectionRect();
        }

        #region Logic

        private int charIndexAtPoint(Vector2 location)
        {
            // Take into account text rectangle left margin
            var textRect = GetTextRectView();
            location.X -= textRect.Left;

            // Early out
            if (location.X < 0 || _text.Length == 0)
            {
                return 0;
            }

            // Cache data
            var font = Font;
            Debug.Assert(font, "Missing font.");

            // Early out
            // TODO: improve searching and remove that eraly out
            /*float fullWidth = font.MeasureText(_text).X;
            if (location.X > fullWidth)
            {
                return _text.Length;
            }

            // Find char at point
            // TODO: maybe call font to calculate all chanracters widths in array and then use binary search???
            for (int i = 0; i < _text.Length; i++)
            {
                if (location.X <= font.MeasureText(_text.Left(i + 1)).X)
                    return i;
            }*/

            return 0;
        }

        private void calSelectionRect()
        {
            // Check if need to perform that calculation
            /*if (_selectionLeft != -1)
            {
                // Cache data
                var textArea = GetTextRect();
                int selectedChars = _selectionRight - _selectionLeft;
                var font = Font;
                Debug.Assert(font, "Missing font.");

                // Calcuate selection rectangle
                // TODO: cache text trails and reuse it during MeasureText and HitTestTextPosition as well as mouse events?
                float beforeSelectionWidth = _selectionLeft == 0 ? 0 : font.HitTestTextPosition(_text, _selectionLeft).X;
                float selectionWidth = selectedChars == 0 ? 0 : font.MeasureText(_text.Substring(_selectionLeft, selectedChars)).X;
                _selectionRect = new Rectangle(beforeSelectionWidth, 0, selectionWidth, textArea.GetHeight()); // TODO: update this code for multiline case
                _selectionRect += textArea.Location;

                // Calculate caret rectangle
                const float caretWidth = 1.4f;
                float caretPosX = _isCaretOnLeft ? _selectionRect.Left : _selectionRect.Right;
                float caretPosY = _selectionRect.Y;
                _caretRect = new Rectangle(
                    caretPosX - (caretWidth * 0.5f),
                    caretPosY,
                    caretWidth,
                    _selectionRect.Height);

                // Update view offset (caret needs to be in a view)
                // TODO: update this code for multiline case
                Vector2 caretInView = new Vector2(caretPosX, caretPosY) - _viewOffset;
                Vector2 clampedCaretInView = Vector2.Clamp(caretInView, textArea.UpperLeft, textArea.BottomRight);
                _viewOffset += caretInView - clampedCaretInView;
            }
            else*/
            {
                // Clear values
                _caretRect = _selectionRect = new Rectangle(-1, 0, 0, 0);
                _viewOffset = Vector2.Zero;
            }

            // Reset caret visibility and cache textbox size
            _caretTime = 0;
            _cachedSize = Size;
        }

        private void insertText(string text)
        {
            if (_selectionLeft == -1)
            {
                _text = text;
                _selectionLeft = 0;
                _selectionRight = 1;
            }
            else
            {
                int left = Mathf.Min(_selectionRight, _selectionLeft);
                int right = Mathf.Max(_selectionRight, _selectionLeft);
                int selectedChars = right - left;
                if (selectedChars > 0)
                    _text = _text.Remove(_selectionLeft, selectedChars);
                _text = _text.Insert(_selectionLeft, text);
                _selectionLeft += text.Length;
                _selectionRight = _selectionLeft;
            }

            // Update selection
            calSelectionRect();
        }

        private void rollSelection()
        {
            int selectedChars = Mathf.Abs(_selectionRight - _selectionLeft);
            if (selectedChars > 0)
            {
                if (_isCaretOnLeft)
                    _selectionRight = _selectionLeft;
                else
                    _selectionLeft = _selectionRight;
            }
        }

        private void endSelecting()
        {   
            // Check if user was selecting
            if (_isSelecting)
            {
                // Clear flag
                _isSelecting = false;

                // Stop tracking mouse
                //GetParentWindow()->GetWin()->EndTrackingMouse();
            }
        }

        #endregion

        #region Internal Events

        protected virtual void OnEditBegin()
        {
            if (_isEditing)
                return;

            _isEditing = true;
            _onStartEditValue = _text;
        }

        protected virtual void OnEditEnd()
        {
            if (!_isEditing)
                return;

            _isEditing = false;
            if (_onStartEditValue != _text)
            {
                _onStartEditValue = _text;
                OnTextChanged();
            }
            _onStartEditValue = string.Empty;

            ClearSelection();
        }

        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke();
        }

        #endregion

        #region Appearance

        protected virtual Rectangle GetTextClipRect()
        {
            return new Rectangle(1, 1, _width - 2, _height - 2);
        }

        protected virtual Rectangle GetTextRect()
        {
            return new Rectangle(DefaultMargin, 1, _width - 2 * DefaultMargin, _height - 2);
        }

        protected Rectangle GetTextRectView()
        {
            return GetTextRect() - _viewOffset;
        }

        #endregion

        #region Control

        public override void Update(float dt)
        {
            // Update
            _caretTime += dt;
            _selectionTime += dt;

            // Ensure to keep selection rectangle valid during scalling
            /*if (!Vector2.NearEqual(Size, _cachedSize))
            {
                calSelectionRect();
            }*/

            // TODO: animate viewOffset??

            base.Update(dt);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;

            // Background
            Color backColor = style.TextBoxBackground;
            if (IsMouseOver)
                backColor = style.TextBoxBackgroundSelected;
            if (_backgroundColor.A > 0)
                backColor = _backgroundColor;
            Render2D.FillRectangle(new Rectangle(0, 0, _width, _height), backColor);
            if (IsFocused)
                Render2D.DrawRectangle(new Rectangle(0, 0, _width, _height), style.BackgroundSelected);

            // Apply view offset and clip mask
            var trans = Render2D.Transform;
            Render2D.Transform = trans - _viewOffset;
            Render2D.PushClip(GetTextClipRect());

            // Selection background
            if (_selectionRect.Width > 1)
            {
                float alpha = Mathf.Min(1.0f, Mathf.Cos(_selectionTime * 6.0f) * 0.5f + 1.3f);
                alpha = alpha * alpha;
                if (!IsFocused)
                    alpha = 0.1f;
                Render2D.FillRectangle(_selectionRect, style.BackgroundSelected * alpha, true);
            }

            // Draw text (use clipping)
            Render2D.DrawText(style.FontMedium, _text, GetTextRect(), Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, 1.0f);

            // Carret
            if (_caretRect.Width > 1 && IsFocused)
            {
                float alpha = Mathf.Saturate(Mathf.Cos(_caretTime * Mathf.TwoPi) * 0.5f + 0.7f);
                alpha = alpha * alpha * alpha * alpha * alpha * alpha;
                Render2D.FillRectangle(_caretRect, style.Foreground * alpha, true);
            }

            // Restore rendering state
            Render2D.PopClip();
            Render2D.Transform = trans;
        }

        /// <inheritdoc />
        public override void OnGotFocus()
        {
            base.OnGotFocus();
            OnEditBegin();
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            base.OnLostFocus();
            OnEditEnd();
        }

        public override bool HasMouseCapture
        {
            get { return _isSelecting; }
        }

        public override void OnLostMouseCapture()
        {
            OnEditEnd();
        }

        public override bool OnMouseDoubleClick(MouseButtons buttons, Vector2 location)
        {
            SelectAll();
            return base.OnMouseDoubleClick(buttons, location);
        }

        public override void OnMouseMove(Vector2 location)
        {
            /*// Check if user is selecting
            if (_isSelecting)
            {
                // Find char index at current mosue location
                int currentIndex = charIndexAtPoint(location);

                // Switch state
                if (currentIndex < _selectionLeft)
                {
                    _selectionLeft = currentIndex;
                    _isCaretOnLeft = true;
                }
                else if (currentIndex > _selectionRight)
                {
                    _selectionRight = currentIndex;
                    _isCaretOnLeft = false;
                }
                else if (_isCaretOnLeft)
                {
                    _selectionLeft = currentIndex;
                }
                else
                {
                    _selectionRight = currentIndex;
                }

                // Ensure to have left selection indexx smaller than right index
                if (_selectionRight < _selectionLeft)
                {
                    int tmp = _selectionLeft;
                    _selectionLeft = _selectionRight;
                    _selectionRight = tmp;
                }

                // Update
                calSelectionRect();
            }*/
        }

        public override bool OnMouseDown(MouseButtons buttons, Vector2 location)
        {
            // Check button
            /*if (buttons & MouseButtons::Left)
            {
                // Check if has any text
                if (_text.HasChars())
                {
                    // Calculate char index under the mouse location
                    _selectionLeft = _selectionRight = charIndexAtPoint(location);

                    // Update
                    calSelectionRect();
                }

                // Start selecting
                _isSelecting = true;

                // Start tracking mouse
                GetParentWindow()->GetWin()->StartTrackingMouse(true);
            }*/

            // Base
            base.OnMouseDown(buttons, location);
            return true;
        }

        public override bool OnMouseUp(MouseButtons buttons, Vector2 location)
        {
            endSelecting();
            return base.OnMouseUp(buttons, location);
        }

        #endregion
    }
}
