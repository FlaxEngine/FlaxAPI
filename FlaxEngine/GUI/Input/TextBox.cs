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
    public partial class TextBox : Control
    {
        private static readonly char[] Separators = { ' ', '.', ',', '\t', '\r', '\n' };

        /// <summary>
        /// Default height of the text box
        /// </summary>
        private static int DefaultHeight = 18;

        /// <summary>
        /// Left and right margin for text inside the text box bounds rectangle
        /// </summary>
        private static int DefaultMargin = 4;

        // TODO: support password protected text box

        private string _text;

        // State
        private string _onStartEditValue;
        private bool _isEditing;
        private Vector2 _viewOffset;

        // Options
        private bool _isMultiline, _isReadOnly;
        private int _maxLength;
        private TextLayoutOptions _layout;

        // Selecting
        private bool _isSelecting;
        private int _selectionStart, _selectionEnd;
        private float _animateTime;

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
                    
            Deselect();
            update _layout settings
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
                if(IsReadOnly)
                    throw new AccessViolationException("Text Box is readonly.");

                // Clamp length
                if (value.Length > MaxLength)
                    value = value.Substring(0, MaxLength);

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
                int length = SelectionLength;
                return length > 0 ? _text.Substring(SelectionLeft, length) : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// </summary>
        public int SelectionLength
        {
            get { return Mathf.Abs(_selectionEnd - _selectionStart); }
        }

        /// <summary>
        /// Returns true if any text is selected, otherwise false
        /// </summary>
        public bool HasSelection
        {
            get { return SelectionLength > 0; }
        }

        /// <summary>
        /// Index of the character on left edge of the selection
        /// </summary>
        private int SelectionLeft
        {
            get { return Mathf.Min(_selectionStart, _selectionEnd); }
        }

        /// <summary>
        /// Index of the character on right edge of the selection
        /// </summary>
        private int SelectionRight
        {
            get { return Mathf.Max(_selectionStart, _selectionEnd); }
        }

        /// <summary>
        /// Gets current caret position (index of the character)
        /// </summary>
        private int CaretPosition
        {
            get { return _selectionEnd; }
        }

        /// <summary>
        /// Calculates caret rectangle
        /// </summary>
        private Rectangle CaretBounds
        {
            get
            {
                const float caretWidth = 1.2f;

                Vector2 caretPos = Font.GetCharPosition(_text, CaretPosition, _layout);

                return new Rectangle(
                    caretPos.X - (caretWidth * 0.5f),
                    caretPos.Y,
                    caretWidth,
                    Font.Height);
            }
        }

        /// <summary>
        /// Gets text font
        /// </summary>
        private Font Font
        {
            get { return Style.Current.FontMedium; }
        }

        /// <summary>
        /// Gets rectangle with area for text
        /// </summary>
        protected virtual Rectangle TextRectangle
        {
            get { return new Rectangle(DefaultMargin, 1, Width - 2 * DefaultMargin, Height - 2); }
        }

        /// <summary>
        /// Gets rectangle used to clip text
        /// </summary>
        protected virtual Rectangle TextClipRectangle
        {
            get { return new Rectangle(1, 1, Width - 2, Height - 2); }
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
            _selectionStart = _selectionEnd = -1;

            _layout = TextLayoutOptions.Default;
            _layout.VerticalAlignment = IsMultiline ? TextAlignment.Near : TextAlignment.Center;
            _layout.TextWrapping = TextWrapping.NoWrap;

            UpdateTextRect();
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
            if (_isSelecting)
                OnSelectingEnd();
            setSelection(-1);
        }

        /// <summary>
        /// Copies the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Copy()
        {
            throw new NotImplementedException("Copy textbox text");
            // TODO: we need to support Clipboard via Flax API

            /*// Check if sth is selected
            if (SelectionLength > 0)
            {
                // Get selection
                String selectedText = GetSelection();

                // Copy
                Application::ClipboardSetData(selectedText);
            }*/
        }

        /// <summary>
        /// Moves the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Cut()
        {
            throw new NotImplementedException("Cut textbox text");
            // TODO: we need to support Clipboard via Flax API

            // Check if sth is selected
            /*if (SelectionLength > 0)
            {
                // Get selection
                String selectedText = GetSelection();

                // Copy
                Application::ClipboardSetData(selectedText);

                // Delete selected text
                _text.Remove(Math::Max(0, _selectionLeft), SelectionLength);
                _selectionRight = _selectionLeft;
                _isCaretOnLeft = false;

                // Fire event
                //OnValueChanged.TryCall(this);

                // Update
                calSelectionRect();
            }*/
        }

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            throw new NotImplementedException("Paste textbox text");
            // TODO: we need to support Clipboard via Flax API

            // Get clipboard data
            /*String clipboardText = Application::ClipboardGetData();

            // Check clipboad text length
            if (clipboardText.HasChars())
            {
                // Check if sth is selected
                int32 left = Math::Max(0, _selectionLeft);
                int32 selectedChars = _selectionRight - left;
                if (selectedChars > 0)
                {
                    // Delete selected text
                    _text.Remove(left, selectedChars);
                    _selectionRight = _selectionLeft;
                    _isCaretOnLeft = false;
                }

                // Insert text
                insertText(clipboardText);
            }*/
        }

        /// <summary>
        /// Duplicates the current selection in the text box.
        /// </summary>
        public void Duplicate()
        {
            throw new NotImplementedException("Duplicate textbox text");
            // TODO: we need to support Clipboard via Flax API

            // Check if sth is selected
            /*if (GetSelectionLength() > 0)
            {
                // Get selection text
                String selection = GetSelection();

                // Duplicate selected text
                int32 right = _selectionRight;
                _text.Insert(_selectionRight, selection);

                // Selected inserted text
                _selectionLeft = right;
                _selectionRight = right + selection.Length();
                calSelectionRect();
            }*/
        }

        /// <summary>
        /// Ensures that the caret is visible in the TextBox window, by scrolling the TextBox control surface if necessary.
        /// </summary>
        public void ScrollToCaret()
        {
            //throw new NotImplementedException("ScrollToCaret");

            /*// Update view offset (caret needs to be in a view)
            // TODO: update this code for multiline case
            Vector2 caretInView = new Vector2(caretPosX, caretPosY) - _viewOffset;
            Vector2 clampedCaretInView = Vector2.Clamp(caretInView, textArea.UpperLeft, textArea.BottomRight);
            _viewOffset += caretInView - clampedCaretInView;*/

            // TODO: update view offset
            _viewOffset = Vector2.Zero;
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            if (TextLength > 0)
            {
                setSelection(0, TextLength);
            }
        }

        /// <summary>
        /// Sets selection to empty value
        /// </summary>
        public void Deselect()
        {
            setSelection(-1);
        }

        #region Logic

        private void UpdateTextRect()
        {
            _layout.Bounds = TextRectangle;
        }

        private int CharIndexAtPoint(ref Vector2 location)
        {
            Debug.Assert(Font, "Missing font.");

            // Perform test using Font API
            return Font.HitTestText(_text, location, _layout);
        }

        private void Insert(char c)
        {
            Insert(c.ToString());
        }

        private void Insert(string str)
        {
            if (IsReadOnly)
                throw new AccessViolationException("Text Box is readonly.");

            int selectionLength = SelectionLength;
            int charactersLeft = MaxLength - _text.Length + selectionLength;
            Debug.Assert(charactersLeft >= 0);
            if (charactersLeft == 0)
                return;
            if (charactersLeft < str.Length)
                str = str.Substring(0, charactersLeft);

            if (TextLength == 0)
            {
                _text = str;
                setSelection(TextLength);
            }
            else
            {
                if (HasSelection)
                    _text = _text.Remove(SelectionLeft, selectionLength);

                _text = _text.Insert(SelectionLeft, str);
                setSelection(SelectionLeft + 1);
            }
        }

        private void MoveRight(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                setSelection(SelectionRight);
            }
            else if(SelectionRight < TextLength)
            {
                int position;
                if (ctrl)
                    position = FindtNextWordBegin();
                else
                    position = _selectionEnd + 1;

                if (shift)
                {
                    setSelection(_selectionStart, position);
                }
                else
                {
                    setSelection(position);
                }
            }
        }

        private void MoveLeft(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                setSelection(SelectionLeft);
            }
            else if(SelectionLeft > 0)
            {
                int position;
                if (ctrl)
                    position = FindtPrevWordBegin();
                else
                    position = _selectionEnd - 1;

                if (shift)
                {
                    setSelection(_selectionStart, position);
                }
                else
                {
                    setSelection(position);
                }
            }
        }

        private void MoveDown(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                setSelection(SelectionRight);
            }
            else
            {
                int position = FindLineDownChar(CaretPosition);

                if (shift)
                {
                    setSelection(_selectionStart, position);
                }
                else
                {
                    setSelection(position);
                }
            }
        }

        private void MoveUp(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                setSelection(SelectionLeft);
            }
            else
            {
                int position = FindLineUpChar(CaretPosition);

                if (shift)
                {
                    setSelection(_selectionStart, position);
                }
                else
                {
                    setSelection(position);
                }
            }
        }

        private void setSelection(int caret)
        {
            setSelection(caret, caret);
        }

        private void setSelection(int start, int end)
        {
            // Update parameters
            int textLength = _text.Length;
            _selectionStart = Mathf.Clamp(start, -1, textLength);
            _selectionEnd = Mathf.Clamp(end, -1, textLength);

            // Update view on caret modified
            ScrollToCaret();

            // Reset caret and selection animation
            _animateTime = 0.0f;
        }

        private int FindtNextWordBegin()
        {
            int textLength = TextLength;
            int caretPos = CaretPosition;

            if (caretPos + 1 >= textLength)
                return textLength;

            int spaceLoc = Text.IndexOfAny(Separators, caretPos + 1);

            if (spaceLoc == -1)
                spaceLoc = textLength;
            else
                spaceLoc++;

            return spaceLoc;
        }

        private int FindtPrevWordBegin()
        {
            int caretPos = CaretPosition;

            if (caretPos - 2 < 0)
                return 0;

            int spaceLoc = _text.LastIndexOfAny(Separators, caretPos - 2);

            if (spaceLoc == -1)
                spaceLoc = 0;
            else
                spaceLoc++;

            return spaceLoc;
        }

        private int FindLineDownChar(int index)
        {
            if (!IsMultiline)
                return 0;

            Vector2 location = Font.GetCharPosition(_text, index, _layout);
            location.Y += Font.Height;

            return Font.HitTestText(_text, location, _layout);
        }

        private int FindLineUpChar(int index)
        {
            if (!IsMultiline)
                return _text.Length;

            Vector2 location = Font.GetCharPosition(_text, index, _layout);
            location.Y -= Font.Height;

            return Font.HitTestText(_text, location, _layout);
        }

        /// <summary>
        /// Action called when user starts text selecting
        /// </summary>
        protected virtual void OnSelectingBegin()
        {
            // Set flag
            _isSelecting = true;

            // Start tracking mouse
            //GetParentWindow()->GetWin()->StartTrackingMouse(false);
        }

        /// <summary>
        /// Action called when user ends text selecting
        /// </summary>
        protected virtual void OnSelectingEnd()
        {
            // Clear flag
            _isSelecting = false;

            // Stop tracking mouse
            //GetParentWindow()->GetWin()->EndTrackingMouse();
        }

        #endregion

        #region Internal Events

        /// <summary>
        /// Action called when user starts text editing
        /// </summary>
        protected virtual void OnEditBegin()
        {
            if (_isEditing)
                return;

            _isEditing = true;
            _onStartEditValue = _text;

            // Reset caret visibility
            _animateTime = 0;
        }

        /// <summary>
        /// Action called when user ends text editing
        /// </summary>
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

        /// <summary>
        /// Set text value
        /// </summary>
        /// <param name="value">Value to assign</param>
        protected virtual void SetTextInternal(ref string value)
        {
            
        }

        /// <summary>
        /// Action called when text gets modified
        /// </summary>
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke();
        }

        #endregion

        #region Control

        /// <inheritdoc />
        public override void Update(float dt)
        {
            // Update
            _animateTime += dt;

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
            // Cache data
            var style = Style.Current;
            var rect = new Rectangle(0, 0, Width, Height);
            var font = Font;
            Debug.Assert(font, "Missing font.");

            // Background
            Color backColor = style.TextBoxBackground;
            if (IsMouseOver)
                backColor = style.TextBoxBackgroundSelected;
            if (_backgroundColor.A > 0)
                backColor = _backgroundColor;
            Render2D.FillRectangle(rect, backColor);
            if (IsFocused)
                Render2D.DrawRectangle(rect, style.BackgroundSelected);

            // Apply view offset and clip mask
            var trans = Render2D.Transform;
            Render2D.PushClip(TextClipRectangle);
            Render2D.Transform = trans - _viewOffset;

            // Check if sth is selected to draw selection
            if (HasSelection)
            {
                // TODO: maybe we could use ProcessText Font API to render selection faster?

                Vector2 leftEdge = Font.GetCharPosition(_text, SelectionLeft, _layout);
                Vector2 rightEdge = Font.GetCharPosition(_text, SelectionRight, _layout);
                float fontHeight = font.Height;

                // Draw selection background
                float alpha = Mathf.Min(1.0f, Mathf.Cos(_animateTime * 6.0f) * 0.5f + 1.3f);
                alpha = alpha * alpha;
                if (!IsFocused)
                    alpha = 0.1f;
                Color selectionColor = style.BackgroundSelected * alpha;
                //
                int selectedLinesCount = 1 + Mathf.FloorToInt((rightEdge.Y - leftEdge.Y) / fontHeight);
                if (selectedLinesCount == 1)
                {
                    // Selected is part of single line
                    Rectangle r1 = new Rectangle(leftEdge.X, leftEdge.Y, rightEdge.X - leftEdge.X, fontHeight);
                    Render2D.FillRectangle(r1, selectionColor, true);
                }
                else
                {
                    float leftMargin = _layout.Bounds.Location.X;

                    // Selected is more than one line
                    Rectangle r1 = new Rectangle(leftEdge.X, leftEdge.Y, _layout.Bounds.Width - leftEdge.X + leftMargin, fontHeight);
                    Render2D.FillRectangle(r1, selectionColor, true);
                    //
                    for (int i = 3; i <= selectedLinesCount; i++)
                    {
                        leftEdge.Y += fontHeight;
                        Rectangle r = new Rectangle(leftMargin, leftEdge.Y, _layout.Bounds.Width, fontHeight);
                        Render2D.FillRectangle(r, selectionColor, true);
                    }
                    //
                    Rectangle r2 = new Rectangle(leftMargin, rightEdge.Y, rightEdge.X - leftMargin, fontHeight);
                    Render2D.FillRectangle(r2, selectionColor, true);
                }
            }

            // Text
            Render2D.DrawText(font, _text, _layout.Bounds, Enabled ? style.Foreground : style.ForegroundDisabled, _layout.HorizontalAlignment, _layout.VerticalAlignment, _layout.TextWrapping);

            // Caret
            if (IsFocused && CaretPosition > -1)
            {
                float alpha = Mathf.Saturate(Mathf.Cos(_animateTime * Mathf.TwoPi) * 0.5f + 0.7f);
                alpha = alpha * alpha * alpha * alpha * alpha * alpha;
                Render2D.FillRectangle(CaretBounds, style.Foreground * alpha, true);
            }

            // Restore rendering state
            Render2D.Transform = trans;
            Render2D.PopClip();
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

        /// <inheritdoc />
        public override bool HasMouseCapture
        {
            get { return _isSelecting; }
        }

        /// <inheritdoc />
        public override void OnLostMouseCapture()
        {
            OnEditEnd();
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(MouseButtons buttons, Vector2 location)
        {
            SelectAll();
            return base.OnMouseDoubleClick(buttons, location);
        }
        
        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if user is selecting
            if (_isSelecting)
            {
                // Find char index at current mosue location
                int currentIndex = CharIndexAtPoint(ref location);

                // Modify selection end
                setSelection(_selectionStart, currentIndex);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(MouseButtons buttons, Vector2 location)
        {
            if (buttons == MouseButtons.Left && _text.Length > 0)
            {
                OnSelectingBegin();

                // Calculate char index under the mouse location
                setSelection(CharIndexAtPoint(ref location));
            }

            // Base
            base.OnMouseDown(buttons, location);
            return true;
        }
        
        /// <inheritdoc />
        public override bool OnMouseUp(MouseButtons buttons, Vector2 location)
        {
            if (_isSelecting)
                OnSelectingEnd();
            return base.OnMouseUp(buttons, location);
        }

        /// <inheritdoc />
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            UpdateTextRect();
        }

        #endregion
    }
}
