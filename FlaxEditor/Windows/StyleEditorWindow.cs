using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Dialogs;
using FlaxEditor.GUI.Input;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Utilities;

namespace FlaxEditor.Windows
{
    /// <inheritdoc />
    public class StyleEditorWindow : EditorWindow
    {
        private const float BUTTONS_WIDTH = 60.0f;
        private const float PICKER_MARGIN = 6.0f;
        private const float PREVIEW_X = 50;

        private Style _oldStyle;
        private Style _newStyle;

        private bool _useDynamicEditing;
        private StyleValueEditor.ValueChangedEvent _onChanged;

        private Button _cCancel;
        private Button _cOK;
        private CustomEditorPresenter _valueEditor;

        private Panel _previewPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleEditorWindow"/> class.
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="valueChanged">The changed event.</param>
        /// <param name="useDynamicEditing">True if allow dynamic value editing (slider-like usage), otherwise will change event only on editing end.</param>
        public StyleEditorWindow(Editor editor, Style initialValue, StyleValueEditor.ValueChangedEvent valueChanged, bool useDynamicEditing) : base(editor, false, ScrollBars.None)
        {
            this.Title = "Style";
            _oldStyle = initialValue;
            _newStyle = _oldStyle.DeepClone();
            _useDynamicEditing = useDynamicEditing;
            _onChanged = valueChanged;

            var container = this.AddChild<UniformGridPanel>();
            container.DockStyle = DockStyle.Fill;
            container.SlotsVertically = 1;
            container.SlotsHorizontally = 2;

            var optionsPanel = container.AddChild<Panel>();
            optionsPanel.DockStyle = DockStyle.Fill;
            optionsPanel.IsScrollable = true;
            optionsPanel.ScrollBars = ScrollBars.Vertical;

            _valueEditor = new CustomEditorPresenter(null);
            _valueEditor.Panel.Parent = optionsPanel;
            _valueEditor.OverrideEditor = new GenericEditor();
            _valueEditor.Select(_newStyle);
            _valueEditor.Modified += OnEdited;

            _previewPanel = container.AddChild<Panel>();
            _previewPanel.DockStyle = DockStyle.Fill;

            var preview = CreatePreview(_newStyle);
            preview.Parent = _previewPanel;
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Cancel
            _cCancel = new Button(Width - BUTTONS_WIDTH - PICKER_MARGIN, Height - Button.DefaultHeight - PICKER_MARGIN, BUTTONS_WIDTH)
            {
                Text = "Cancel",
                Parent = this,
                AnchorStyle = AnchorStyle.BottomRight
            };
            _cCancel.Clicked += OnCancelClicked;

            // OK
            _cOK = new Button(_cCancel.Left - BUTTONS_WIDTH - PICKER_MARGIN, _cCancel.Y, BUTTONS_WIDTH)
            {
                Text = "Ok",
                Parent = this,
                AnchorStyle = AnchorStyle.BottomRight
            };
            _cOK.Clicked += OnOkClicked;
        }

        /// <summary>
        /// Creates a preview
        /// </summary>
        /// <param name="style">The style to use for the preview</param>
        /// <returns>The preview</returns>
        private ContainerControl CreatePreview(Style style)
        {
            var currentStyle = Style.Current;
            Style.Current = style;

            var preview = new ContainerControl
            {
                DockStyle = DockStyle.Fill,
                BackgroundColor = style.Background
            };

            var label = new Label
            {
                Text = "Example Label",
                Parent = preview,
                Location = new Vector2(PREVIEW_X, 50),
                TooltipText = "Example Tooltip"
            };

            var button = new Button(PREVIEW_X, 100)
            {
                Text = "Example Button",
                Parent = preview,
                TooltipText = "Example Tooltip"
            };

            var textBox = new TextBox(true, PREVIEW_X, 150)
            {
                Text = "Example TextBox",
                Parent = preview
            };

            var checkBox = new CheckBox(PREVIEW_X, 200)
            {
                Parent = preview
            };

            var panel = new Panel()
            {
                Parent = preview,
                X = PREVIEW_X,
                Y = 250,
                BackgroundColor = style.BackgroundSelected,
                Width = 250,
                Height = 30
            };

            var progressBar = new ProgressBar(20, 5, 150, 20)
            {
                Value = 42,
                Parent = panel
            };

            var comboBox = new ComboBox()
            {
                Items = new List<string>() { "Item 1", "Item 2", "Item 3" },
                X = PREVIEW_X,
                Y = 300,
                Parent = preview,
                SelectedIndex = 0,
                SelectedItem = "Item 1"
            };

            var slider = new SliderControl(30, PREVIEW_X, 350, min: 0, max: 100)
            {
                Parent = preview,
                Value = 31
            };

            Style.Current = currentStyle;
            return preview;
        }

        /// <summary>
        /// Gets called when the style has been edited
        /// </summary>
        private void OnEdited()
        {
            if (_previewPanel != null)
            {
                _previewPanel.DisposeChildren();
                var preview = CreatePreview(_newStyle);
                preview.Parent = _previewPanel;
            }
            if (_useDynamicEditing)
            {
                _onChanged?.Invoke(_newStyle, true);
            }
        }

        private void OnOkClicked()
        {
            _onChanged?.Invoke(_newStyle, false);
            Close();
        }

        private void OnCancelClicked()
        {
            // Restore old style
            if (_useDynamicEditing)
                _onChanged?.Invoke(_oldStyle, false);

            Close(ClosingReason.User);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (IsDisposing)
                return;

            if (_useDynamicEditing)
                _onChanged?.Invoke(_oldStyle, false);
        }
    }
}
