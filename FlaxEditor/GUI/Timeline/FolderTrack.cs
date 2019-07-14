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
        /// <param name="mute">The mute flag.</param>
        public FolderTrack(Timeline.TrackArchetype archetype, bool mute)
        {
            Name = archetype.Name;
            Icon = archetype.Icon;
            Mute = mute;

            const float buttonSize = 14;
            var colorPickerButton = new Image(Width - buttonSize - 2.0f, 0, buttonSize, buttonSize)
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

            var muteButton = new CheckBox(colorPickerButton.Left - buttonSize - 2.0f, 0, !Mute, buttonSize)
            {
                TooltipText = "Mute track",
                AutoFocus = true,
                AnchorStyle = AnchorStyle.CenterRight,
                IsScrollable = false,
                Parent = this
            };
            muteButton.StateChanged += OnMuteButtonStateChanged;
        }

        private void OnMuteButtonStateChanged(CheckBox checkBox)
        {
            Mute = !checkBox.Checked;
            Timeline.MarkAsEdited();
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
