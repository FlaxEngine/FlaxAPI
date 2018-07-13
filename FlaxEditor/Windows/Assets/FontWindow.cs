// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Editor window to view/modify <see cref="FontAsset"/> asset.
    /// </summary>
    /// <seealso cref="FontAsset" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class FontAssetWindow : AssetEditorWindowBase<FontAsset>
    {
        private TextBox _inputText;
        private Label _textPreview;

        /// <inheritdoc />
        public FontAssetWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            var panel = new SplitPanel(Orientation.Vertical, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.2f,
                Parent = this
            };

            _inputText = new TextBox(true, 0, 0)
            {
                DockStyle = DockStyle.Fill,
                Parent = panel.Panel1
            };
            _inputText.TextChanged += OnTextChanged;

            _textPreview = new Label
            {
                DockStyle = DockStyle.Fill,
                Margin = new Margin(4),
                Wrapping = TextWrapping.WrapWords,
                HorizontalAlignment = TextAlignment.Near,
                VerticalAlignment = TextAlignment.Near,
                Parent = panel.Panel2
            };
        }

        private void OnTextChanged()
        {
            _textPreview.Text = _inputText.Text;
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _textPreview.Font = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            Asset.WaitForLoaded();
            _textPreview.Font = new FontReference(Asset.CreateFont(30));
            _inputText.Text = string.Format("This is a sample text using font {0}.", Asset.FamilyName);

            base.OnAssetLinked();
        }
    }
}
