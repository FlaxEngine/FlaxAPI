// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine.Assertions;
using FlaxEngine.Utilities;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Text Box control which can gather text input from the user
    /// </summary>
    public partial class TextBox : Control
    {
        private static readonly char[] Separators =
        {
            ' ',
            '.',
            ',',
            '\t',
            '\r',
            '\n'
        };

        /// <summary>
        /// Default height of the text box
        /// </summary>
        public static float DefaultHeight = 18;

        /// <summary>
        /// Left and right margin for text inside the text box bounds rectangle
        /// </summary>
        public static float DefaultMargin = 4;

        // TODO: support password protected text box

        private string _text = string.Empty;

        // State
        private string _onStartEditValue;
        private bool _isEditing;
        private Vector2 _viewOffset, _targetViewOffset;

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
        public event Action TextChanged;

        /// <summary>
        /// Event fired when text gets changed after editing (user accepted entered value).
        /// </summary>
        public event Action EditEnd;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline text box control.
        /// </summary>
        [EditorOrder(40), Tooltip("If checked, the textbox will support multiline text input.")]
        public bool IsMultiline
        {
            get => _isMultiline;
            set
            {
                if (_isMultiline != value)
                {
                    _isMultiline = value;

                    _layout.VerticalAlignment = IsMultiline ? TextAlignment.Near : TextAlignment.Center;

                    Deselect();

                    if (!_isMultiline)
                    {
                        var lines = _text.Split('\n');
                        _text = lines[0];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type into the text box control.
        /// </summary>
        [EditorOrder(50), Tooltip("The maximum number of characters the user can type into the text box control.")]
        public int MaxLength
        {
            get => _maxLength;
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
        [EditorOrder(60), Tooltip("If checked, text in the text box is read-only.")]
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;

                    // TODO: change sth more except the flag?
                }
            }
        }

        /// <summary>
        /// Gets or sets text property.
        /// </summary>
        [EditorOrder(0), MultilineText, Tooltip("The entered text.")]
        public string Text
        {
            get => _text;
            set
            {
                // Skip set if user is editing value
                if (_isEditing)
                    return;

                SetText(value);
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void SetText(string value)
        {
            // Prevent from null problems
            if (value == null)
                value = string.Empty;

            // Filter text
            if (value.IndexOf('\r') != -1)
                value = value.Replace("\r", "");

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
                Deselect();
                ResetViewOffset();

                _text = value;

                OnTextChanged();
            }
        }

        /// <summary>
        /// Gets length of the text
        /// </summary>
        public int TextLength => _text.Length;

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
        public int SelectionLength => Mathf.Abs(_selectionEnd - _selectionStart);

        /// <summary>
        /// Returns true if any text is selected, otherwise false
        /// </summary>
        public bool HasSelection => SelectionLength > 0;

        /// <summary>
        /// Gets or sets the watermark text to show grayed when textbox is empty.
        /// </summary>
        [EditorOrder(20), Tooltip("The watermark text to show grayed when textbox is empty.")]
        public string WatermarkText { get; set; }

        #endregion

        /// <summary>
        /// Index of the character on left edge of the selection
        /// </summary>
        protected int SelectionLeft => Mathf.Min(_selectionStart, _selectionEnd);

        /// <summary>
        /// Index of the character on right edge of the selection
        /// </summary>
        protected int SelectionRight => Mathf.Max(_selectionStart, _selectionEnd);

        /// <summary>
        /// Gets current caret position (index of the character)
        /// </summary>
        protected int CaretPosition => _selectionEnd;

        /// <summary>
        /// Calculates caret rectangle
        /// </summary>
        protected Rectangle CaretBounds
        {
            get
            {
                const float caretWidth = 1.2f;

                var font = Font?.GetFont();
                if (font == null)
                    return new Rectangle(0, 0, caretWidth, Height);

                Vector2 caretPos = font.GetCharPosition(_text, CaretPosition, _layout);

                return new Rectangle(
                    caretPos.X - (caretWidth * 0.5f),
                    caretPos.Y,
                    caretWidth,
                    font.Height);
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000)]
        public FontReference Font { get; set; }

        /// <summary>
        /// Gets or sets the custom material used to render the text. It must has domain set to GUI and have a public texture parameter named Font used to sample font atlas texture with font characters data.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("Custom material used to render the text. It must has domain set to GUI and have a public texture parameter named Font used to sample font atlas texture with font characters data.")]
        public MaterialBase TextMaterial { get; set; }

        /// <summary>
        /// Gets or sets textbox background color when the control is selected (has focus).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The textbox background color when the control is selected (has focus).")]
        public Color BackgroundSelectedColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the text.")]
        public Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the watermark text.")]
        public Color WatermarkTextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the selection (Transparent if not used).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the selection (Transparent if not used).")]
        public Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the caret (Transparent if not used).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the caret (Transparent if not used).")]
        public Color CaretColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border (Transparent if not used).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the border (Transparent if not used).")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border when control is focused (Transparent if not used).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the border when control is focused (Transparent if not used)")]
        public Color BorderSelectedColor { get; set; }

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
        /// Gets the current view offset.
        /// </summary>
        protected Vector2 ViewOffset => _viewOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        public TextBox()
        : this(false, 0, 0)
        {
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        /// <param name="x">Position X coordinate</param>
        /// <param name="y">Position Y coordinate</param>
        /// <param name="width">Width</param>
        public TextBox(bool isMultiline, float x, float y, float width = 120)
        : base(x, y, width, DefaultHeight)
        {
            _isMultiline = isMultiline;
            _maxLength = 32000;
            _selectionStart = _selectionEnd = -1;

            _layout = TextLayoutOptions.Default;
            _layout.VerticalAlignment = IsMultiline ? TextAlignment.Near : TextAlignment.Center;
            _layout.TextWrapping = TextWrapping.NoWrap;
            _layout.Bounds = new Rectangle(DefaultMargin, 1, Width - 2 * DefaultMargin, Height - 2);

            var style = Style.Current;
            Font = new FontReference(style.FontMedium);
            TextColor = style.Foreground;
            WatermarkTextColor = new Color(0.7f);
            CaretColor = style.Foreground;
            SelectionColor = style.BackgroundSelected;
            BorderColor = Color.Transparent;
            BorderSelectedColor = style.BackgroundSelected;
            BackgroundColor = style.TextBoxBackground;
            BackgroundSelectedColor = style.TextBoxBackgroundSelected;
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
            OnSelectingEnd();
            SetSelection(-1);
        }

        /// <summary>
        /// Resets the view offset (text scroll view).
        /// </summary>
        public void ResetViewOffset()
        {
            _viewOffset = _targetViewOffset = Vector2.Zero;
        }

        /// <summary>
        /// Copies the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Copy()
        {
            var selectedText = SelectedText;
            if (selectedText.Length > 0)
            {
                // Copy selected text
                Application.ClipboardText = selectedText;
            }
        }

        /// <summary>
        /// Moves the current selection in the text box to the Clipboard. 
        /// </summary>
        public void Cut()
        {
            var selectedText = SelectedText;
            if (selectedText.Length > 0)
            {
                // Copy selected text
                Application.ClipboardText = selectedText;

                if (IsReadOnly)
                    return;

                // Remove selection
                int left = SelectionLeft;
                _text = _text.Remove(left, SelectionLength);
                SetSelection(left);
                OnTextChanged();
            }
        }

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            if (IsReadOnly)
                return;

            // Get clipboard data
            var clipboardText = Application.ClipboardText;
            if (string.IsNullOrEmpty(clipboardText))
                return;

            var right = SelectionRight;
            Insert(clipboardText);
            SetSelection(Mathf.Max(right, 0) + clipboardText.Length);
        }

        /// <summary>
        /// Duplicates the current selection in the text box.
        /// </summary>
        public void Duplicate()
        {
            if (IsReadOnly)
                return;

            var selectedText = SelectedText;
            if (selectedText.Length > 0)
            {
                var right = SelectionRight;
                SetSelection(right);
                Insert(selectedText);
                SetSelection(right, right + selectedText.Length);
            }
        }

        /// <summary>
        /// Ensures that the caret is visible in the TextBox window, by scrolling the TextBox control surface if necessary.
        /// </summary>
        public void ScrollToCaret()
        {
            // If it's empty
            if (_text.Length == 0)
            {
                _targetViewOffset = Vector2.Zero;
                return;
            }

            Rectangle caretBounds = CaretBounds;
            Rectangle textArea = TextRectangle;

            // Update view offset (caret needs to be in a view)
            Vector2 caretInView = caretBounds.Location - _targetViewOffset;
            Vector2 clampedCaretInView = Vector2.Clamp(caretInView, textArea.UpperLeft, textArea.BottomRight);
            _targetViewOffset += caretInView - clampedCaretInView;
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            if (TextLength > 0)
            {
                SetSelection(0, TextLength);
            }
        }

        /// <summary>
        /// Sets selection to empty value
        /// </summary>
        public void Deselect()
        {
            SetSelection(-1);
        }

        #region Logic

        private int CharIndexAtPoint(ref Vector2 location)
        {
            Assert.IsNotNull(Font, "Missing font.");

            // Perform test using Font API
            var font = Font.GetFont();
            return font.HitTestText(_text, location + _viewOffset, _layout);
        }

        private void Insert(char c)
        {
            Insert(c.ToString());
        }

        private void Insert(string str)
        {
            if (IsReadOnly)
                return;

            // Filter text
            if (str.IndexOf('\r') != -1)
                str = str.Replace("\r", "");

            int selectionLength = SelectionLength;
            int charactersLeft = MaxLength - _text.Length + selectionLength;
            Assert.IsTrue(charactersLeft >= 0);
            if (charactersLeft == 0)
                return;
            if (charactersLeft < str.Length)
                str = str.Substring(0, charactersLeft);

            if (TextLength == 0)
            {
                _text = str;
                SetSelection(TextLength);
            }
            else
            {
                var left = SelectionLeft >= 0 ? SelectionLeft : 0;
                if (HasSelection)
                    _text = _text.Remove(left, selectionLength);

                _text = _text.Insert(left, str);

                SetSelection(left + 1);
            }

            OnTextChanged();
        }

        private void MoveRight(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                SetSelection(SelectionRight);
            }
            else if (SelectionRight < TextLength)
            {
                int position;
                if (ctrl)
                    position = FindtNextWordBegin();
                else
                    position = _selectionEnd + 1;

                if (shift)
                {
                    SetSelection(_selectionStart, position);
                }
                else
                {
                    SetSelection(position);
                }
            }
        }

        private void MoveLeft(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                SetSelection(SelectionLeft);
            }
            else if (SelectionLeft > 0)
            {
                int position;
                if (ctrl)
                    position = FindtPrevWordBegin();
                else
                    position = _selectionEnd - 1;

                if (shift)
                {
                    SetSelection(_selectionStart, position);
                }
                else
                {
                    SetSelection(position);
                }
            }
        }

        private void MoveDown(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                SetSelection(SelectionRight);
            }
            else
            {
                int position = FindLineDownChar(CaretPosition);

                if (shift)
                {
                    SetSelection(_selectionStart, position);
                }
                else
                {
                    SetSelection(position);
                }
            }
        }

        private void MoveUp(bool shift, bool ctrl)
        {
            if (HasSelection && !shift)
            {
                SetSelection(SelectionLeft);
            }
            else
            {
                int position = FindLineUpChar(CaretPosition);

                if (shift)
                {
                    SetSelection(_selectionStart, position);
                }
                else
                {
                    SetSelection(position);
                }
            }
        }

        private void SetSelection(int caret)
        {
            SetSelection(caret, caret);
        }

        private void SetSelection(int start, int end)
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

            var font = Font.GetFont();
            Vector2 location = font.GetCharPosition(_text, index, _layout);
            location.Y += font.Height;

            return font.HitTestText(_text, location, _layout);
        }

        private int FindLineUpChar(int index)
        {
            if (!IsMultiline)
                return _text.Length;

            var font = Font.GetFont();
            Vector2 location = font.GetCharPosition(_text, index, _layout);
            location.Y -= font.Height;

            return font.HitTestText(_text, location, _layout);
        }

        /// <summary>
        /// Action called when user starts text selecting
        /// </summary>
        protected virtual void OnSelectingBegin()
        {
            if (!_isSelecting)
            {
                // Set flag
                _isSelecting = true;

                // Start tracking mouse
                StartMouseCapture();
            }
        }

        /// <summary>
        /// Action called when user ends text selecting
        /// </summary>
        protected virtual void OnSelectingEnd()
        {
            if (_isSelecting)
            {
                // Clear flag
                _isSelecting = false;

                // Stop tracking mouse
                EndMouseCapture();
            }
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
        /// Action called when user ends text editing.
        /// </summary>
        protected virtual void OnEditEnd()
        {
            if (!_isEditing)
                return;

            _isEditing = false;
            if (_onStartEditValue != _text)
            {
                _onStartEditValue = _text;
                EditEnd?.Invoke();
            }
            _onStartEditValue = string.Empty;

            ClearSelection();
            ResetViewOffset();
        }

        /// <summary>
        /// Action called when text gets modified.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke();
        }

        #endregion

        #region Control

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            bool isDeltaSlow = deltaTime > (1 / 20.0f);

            _animateTime += deltaTime;

            // Animate view offset
            _viewOffset = isDeltaSlow ? _targetViewOffset : Vector2.Lerp(_viewOffset, _targetViewOffset, deltaTime * 20.0f);

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var rect = new Rectangle(Vector2.Zero, Size);
            bool enabled = EnabledInHierarchy;
            var font = Font.GetFont();
            Assert.IsNotNull(font, "Missing font.");

            // Background
            Color backColor = BackgroundColor;
            if (IsMouseOver)
                backColor = BackgroundSelectedColor;
            Render2D.FillRectangle(rect, backColor);
            Render2D.DrawRectangle(rect, IsFocused ? BorderSelectedColor : BorderColor);

            // Apply view offset and clip mask
            Render2D.PushClip(TextClipRectangle);
            bool useViewOffset = !_viewOffset.IsZero;
            if (useViewOffset)
                Render2D.PushTransform(Matrix3x3.Translation2D(-_viewOffset));

            // Check if sth is selected to draw selection
            if (HasSelection)
            {
                // TODO: maybe we could use ProcessText Font API to render selection faster?

                Vector2 leftEdge = font.GetCharPosition(_text, SelectionLeft, _layout);
                Vector2 rightEdge = font.GetCharPosition(_text, SelectionRight, _layout);
                float fontHeight = font.Height;

                // Draw selection background
                float alpha = Mathf.Min(1.0f, Mathf.Cos(_animateTime * 6.0f) * 0.5f + 1.3f);
                alpha = alpha * alpha;
                if (!IsFocused)
                    alpha = 0.1f;
                Color selectionColor = SelectionColor * alpha;
                //
                int selectedLinesCount = 1 + Mathf.FloorToInt((rightEdge.Y - leftEdge.Y) / fontHeight);
                if (selectedLinesCount == 1)
                {
                    // Selected is part of single line
                    Rectangle r1 = new Rectangle(leftEdge.X, leftEdge.Y, rightEdge.X - leftEdge.X, fontHeight);
                    Render2D.FillRectangle(r1, selectionColor);
                }
                else
                {
                    float leftMargin = _layout.Bounds.Location.X;

                    // Selected is more than one line
                    Rectangle r1 = new Rectangle(leftEdge.X, leftEdge.Y, 1000000000, fontHeight);
                    Render2D.FillRectangle(r1, selectionColor);
                    //
                    for (int i = 3; i <= selectedLinesCount; i++)
                    {
                        leftEdge.Y += fontHeight;
                        Rectangle r = new Rectangle(leftMargin, leftEdge.Y, 1000000000, fontHeight);
                        Render2D.FillRectangle(r, selectionColor);
                    }
                    //
                    Rectangle r2 = new Rectangle(leftMargin, rightEdge.Y, rightEdge.X - leftMargin, fontHeight);
                    Render2D.FillRectangle(r2, selectionColor);
                }
            }

            // Text or watermark
            if (_text.Length > 0)
            {
                var color = TextColor;
                if (!enabled)
                    color *= 0.6f;
                Render2D.DrawText(font, TextMaterial, _text, _layout.Bounds, color, _layout.HorizontalAlignment, _layout.VerticalAlignment, _layout.TextWrapping);
            }
            else if (!string.IsNullOrEmpty(WatermarkText) && !IsFocused)
            {
                Render2D.DrawText(font, TextMaterial, WatermarkText, _layout.Bounds, WatermarkTextColor, _layout.HorizontalAlignment, _layout.VerticalAlignment, _layout.TextWrapping);
            }

            // Caret
            if (IsFocused && CaretPosition > -1)
            {
                float alpha = Mathf.Saturate(Mathf.Cos(_animateTime * Mathf.TwoPi) * 0.5f + 0.7f);
                alpha = alpha * alpha * alpha * alpha * alpha * alpha;
                Render2D.FillRectangle(CaretBounds, CaretColor * alpha);
            }

            // Restore rendering state
            if (useViewOffset)
                Render2D.PopTransform();
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
        public override void OnEndMouseCapture()
        {
            // Clear flag
            _isSelecting = false;
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            SelectAll();
            return base.OnMouseDoubleClick(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if user is selecting
            if (_isSelecting)
            {
                // Find char index at current mouse location
                int currentIndex = CharIndexAtPoint(ref location);

                // Modify selection end
                SetSelection(_selectionStart, currentIndex);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left && _text.Length > 0)
            {
                OnSelectingBegin();

                // Calculate char index under the mouse location
                var hitPos = CharIndexAtPoint(ref location);

                // Select range with shift
                if (_selectionStart != -1 && RootWindow.GetKey(Keys.Shift) && SelectionLength == 0)
                {
                    if (hitPos < _selectionStart)
                        SetSelection(hitPos, _selectionStart);
                    else
                        SetSelection(_selectionStart, hitPos);
                }
                else
                {
                    SetSelection(hitPos);
                }
            }

            // Base
            base.OnMouseDown(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                OnSelectingEnd();
            }

            // Base
            base.OnMouseUp(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Base
            if (base.OnMouseWheel(location, delta))
                return true;

            // Multiline scroll
            if (IsMultiline && _text.Length != 0)
            {
                var font = Font.GetFont();
                Vector2 endLocation = font.GetCharPosition(_text, _text.Length, _layout);
                _targetViewOffset = Vector2.Clamp(_targetViewOffset - new Vector2(0, delta * 10.0f), Vector2.Zero, new Vector2(_targetViewOffset.X, endLocation.Y));
                return true;
            }

            // No event handled
            return false;
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(ref Vector2 size)
        {
            base.SetSizeInternal(ref size);

            _layout.Bounds = TextRectangle;
        }

        #endregion
    }
}
