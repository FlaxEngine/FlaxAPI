// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Describes GUI controls style (which fonts and colors use etc.)
    /// </summary>
    public class Style
    {
        /// <summary>
        /// Global GUI style used by all the controls.
        /// </summary>
        public static Style Current { get; set; }

        // Fonts
        public Font FontTitle;
        public Font FontLarge;
        public Font FontMedium;
        public Font FontSmall;

        // Background
        public Color Background;
        public Color LightBackground;
        public Color DragWindow;

        // Foreground
        public Color Foreground;
        public Color ForegroundDisabled;

        // General
        public Color BackgroundHighlighted;
        public Color BorderHighlighted;
        public Color BackgroundSelected;
        public Color BorderSelected;
        public Color BackgroundNormal;
        public Color BorderNormal;

        // Text Box
        public Color TextBoxBackground;
        public Color TextBoxBackgroundSelected;

        // Progress bar
        public Color ProgressNormal;

        // Icons
        public Sprite ArrowRight;
        public Sprite ArrowDown;
        public Sprite Search;
        public Sprite Settings;
        public Sprite Cross;
        public Sprite CheckBoxIntermediate;
        public Sprite CheckBoxTick;
        public Sprite StatusBarSizeGrip;
        public Sprite Translate16;
        public Sprite Rotate16;
        public Sprite Scale16;

        /// <summary>
        /// The shared tooltip control used by the controls if no custom tooltip is provided.
        /// </summary>
        public Tooltip SharedTooltip;

        /// <summary>
        /// Delegate function used to handle showing color picking dialog.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="colorChanged">The color changed event.</param>
        /// <param name="useDynamicEditing">True if allow dynamic value editing (slider-like usage), otherwise will fire color change event only on editing end.</param>
        public delegate void ShowPickColorDialogDelegate(Color initialValue, Action<Color> colorChanged, bool useDynamicEditing = true);

        /// <summary>
        /// Shows picking color dialog (see <see cref="ShowPickColorDialogDelegate"/>).
        /// </summary>
        public ShowPickColorDialogDelegate ShowPickColorDialog;
    }
}
