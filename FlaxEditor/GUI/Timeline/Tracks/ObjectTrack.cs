// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.Tracks
{
    /// <summary>
    /// The timeline track for animating managed objects.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public abstract class ObjectTrack : Track
    {
        /// <summary>
        /// The add button.
        /// </summary>
        protected Button _addButton;

        /// <summary>
        /// Gets the object instance (may be null if reference is invalid or data is missing).
        /// </summary>
        public abstract FlaxEngine.Object Object { get; }

        /// <inheritdoc />
        protected ObjectTrack(ref TrackCreateOptions options)
        : base(ref options)
        {
            // Add button
            const float buttonSize = 14;
            _addButton = new Button(_muteCheckbox.Left - buttonSize - 2.0f, 0, buttonSize, buttonSize)
            {
                Text = "+",
                TooltipText = "Add sub-tracks",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Parent = this
            };
            _addButton.Clicked += OnAddButtonClicked;
        }

        private void OnAddButtonClicked()
        {
            var menu = new ContextMenu.ContextMenu();
            OnShowAddContextMenu(menu);
            menu.Show(_addButton.Parent, _addButton.BottomLeft);
        }

        /// <summary>
        /// Called when showing the context menu for add button (for sub-tracks adding).
        /// </summary>
        /// <param name="menu">The menu.</param>
        protected virtual void OnShowAddContextMenu(ContextMenu.ContextMenu menu)
        {
        }
    }
}
