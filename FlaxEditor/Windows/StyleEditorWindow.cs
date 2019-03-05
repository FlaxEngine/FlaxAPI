using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Dialogs;
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

        private Style _oldStyle;
        private Style _newStyle;

        private bool _useDynamicEditing;
        private StyleValueEditor.ValueChangedEvent _onChanged;

        private Button _cCancel;
        private Button _cOK;
        private CustomEditorPresenter _valueEditor;

        private Panel _previewPanel;

        private Control _test;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleEditorWindow"/> class.
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="valueChanged">The changed event.</param>
        /// <param name="useDynamicEditing">True if allow dynamic value editing (slider-like usage), otherwise will change event only on editing end.</param>
        public StyleEditorWindow(Editor editor, Style initialValue, StyleValueEditor.ValueChangedEvent valueChanged, bool useDynamicEditing) : base(editor, false, ScrollBars.Both)
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

            var panel1 = container.AddChild<Panel>();
            panel1.DockStyle = DockStyle.Fill;

            _valueEditor = new CustomEditorPresenter(null);
            _valueEditor.Panel.Parent = panel1;
            _valueEditor.ContainerControl.DockStyle = DockStyle.Fill;
            _valueEditor.Panel.DockStyle = DockStyle.Fill;
            _valueEditor.ContainerControl.IsScrollable = true;
            _valueEditor.Panel.IsScrollable = true;
            _valueEditor.OverrideEditor = new GenericEditor();
            _valueEditor.Select(_newStyle);
            _valueEditor.Modified += OnEdited;
            /*
                        foreach (var child in _valueEditor.Children)
                        {
                            child.Control.DockStyle = DockStyle.Fill;
                            child.Control.IsScrollable = true;
                        }*/

            _previewPanel = container.AddChild<Panel>();
            _previewPanel.DockStyle = DockStyle.Fill;

            var preview = CreatePreview(_newStyle);
            preview.Parent = _previewPanel;

            _test = _valueEditor.Panel;
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

            Debug.Log(this.Height);
            Debug.Log(_test.Height);
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
                Location = new Vector2(50, 50),
                TooltipText = "Example Tooltip"
            };

            var button = new Button(50, 100)
            {
                Text = "Example Button",
                Parent = preview,
                TooltipText = "Example Tooltip"
            };

            var textBox = new TextBox(true, 50, 150)
            {
                Text = "Example TextBox",
                Parent = preview
            };

            var checkBox = new CheckBox(50, 200)
            {
                Parent = preview
            };

            // TODO: Those 2 aren't optimal
            var progressBar = new ProgressBar(50, 250, 100)
            {
                Value = 42,
                Parent = preview
            };

            var dropDown = new Dropdown()
            {
                Items = new List<string>() { "Item 1", "Item 2", "Item 3" },
                X = 50,
                Y = 300,
                Parent = preview
            };

            // TODO: Add a slider

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
            // Restore color
            if (_useDynamicEditing)
                _onChanged?.Invoke(_oldStyle, false);

            Close(ClosingReason.User);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            if (IsDisposing)
                return;

            if (_useDynamicEditing)
                _onChanged?.Invoke(_oldStyle, false);
        }
    }
}
