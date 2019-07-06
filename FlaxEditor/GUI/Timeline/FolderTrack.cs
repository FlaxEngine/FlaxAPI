// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI.Input;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline track that represents a folder used to group and organize tracks.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Track" />
    public class FolderTrack : Track
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderTrack"/> class.
        /// </summary>
        /// <param name="archetype">The archetype.</param>
        public FolderTrack(Timeline.TrackArchetype archetype)
        {
            Name = archetype.Name;
            Icon = archetype.Icon;

            const float settingsButtonSize = 14;
            var colorPickerButton = new Image(Width - settingsButtonSize - 2.0f, 0, settingsButtonSize, settingsButtonSize)
            {
                TooltipText = "Change folder color",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Color = new Color(0.7f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(Style.Current.Settings),
                Parent = this
            };
            colorPickerButton.Clicked += OnColorPickerButtonClicked;
        }

        private void OnColorPickerButtonClicked(Image image, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                ColorValueBox.ShowPickColorDialog?.Invoke(IconColor, OnColorChanged);
            }
        }

        private void OnColorChanged(Color color, bool sliding)
        {
            IconColor = color;
            Timeline.MarkAsEdited();
        }

        /// <inheritdoc />
        protected override bool CanAddChildTrack(Track track)
        {
            return true;
        }
    }
}
