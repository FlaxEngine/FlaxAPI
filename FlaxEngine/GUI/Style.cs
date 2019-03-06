// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Describes GUI controls style (which fonts and colors use etc.). Defines the default values used by the GUI control.s
    /// </summary>
    public class Style
    {
        /// <summary>
        /// Global GUI style used by all the controls.
        /// </summary>
        public static Style Current { get; set; }

        [Serialize]
        private FontReference _fontTitle;

        /// <summary>
        /// The font title.
        /// </summary>
        [NoSerialize]
        public Font FontTitle
        {
            get => _fontTitle.GetFont();
            set => _fontTitle = new FontReference(value);
        }

        [Serialize]
        private FontReference _fontLarge;

        /// <summary>
        /// The font large.
        /// </summary>
        [NoSerialize]
        public Font FontLarge
        {
            get => _fontLarge.GetFont();
            set => _fontLarge = new FontReference(value);
        }

        [Serialize]
        private FontReference _fontMedium;

        /// <summary>
        /// The font medium.
        /// </summary>
        [NoSerialize]
        public Font FontMedium
        {
            get => _fontMedium.GetFont();
            set => _fontMedium = new FontReference(value);
        }

        [Serialize]
        private FontReference _fontSmall;

        /// <summary>
        /// The font small.
        /// </summary>
        [NoSerialize]
        public Font FontSmall
        {
            get => _fontSmall.GetFont();
            set => _fontSmall = new FontReference(value);
        }

        /// <summary>
        /// The background color.
        /// </summary>
        public Color Background;

        /// <summary>
        /// The light background color.
        /// </summary>
        public Color LightBackground;

        /// <summary>
        /// The drag window color.
        /// </summary>
        public Color DragWindow;

        /// <summary>
        /// The foreground color (text).
        /// </summary>
        public Color Foreground;

        /// <summary>
        /// The foreground disabled (text).
        /// </summary>
        public Color ForegroundDisabled;

        /// <summary>
        /// The background highlighted color.
        /// </summary>
        public Color BackgroundHighlighted;

        /// <summary>
        /// The border highlighted color.
        /// </summary>
        public Color BorderHighlighted;

        /// <summary>
        /// The background selected color.
        /// </summary>
        public Color BackgroundSelected;

        /// <summary>
        /// The border selected color.
        /// </summary>
        public Color BorderSelected;

        /// <summary>
        /// The background normal color.
        /// </summary>
        public Color BackgroundNormal;

        /// <summary>
        /// The border normal color.
        /// </summary>
        public Color BorderNormal;

        /// <summary>
        /// The text box background color.
        /// </summary>
        public Color TextBoxBackground;

        /// <summary>
        /// The text box background selected color.
        /// </summary>
        public Color TextBoxBackgroundSelected;

        /// <summary>
        /// The progress normal color.
        /// </summary>
        public Color ProgressNormal;

        /// <summary>
        /// The arrow right icon.
        /// </summary>
        public Sprite ArrowRight;

        /// <summary>
        /// The arrow down icon.
        /// </summary>
        public Sprite ArrowDown;

        /// <summary>
        /// The search icon.
        /// </summary>
        public Sprite Search;

        /// <summary>
        /// The settings icon.
        /// </summary>
        public Sprite Settings;

        /// <summary>
        /// The cross icon.
        /// </summary>
        public Sprite Cross;

        /// <summary>
        /// The CheckBox intermediate icon.
        /// </summary>
        public Sprite CheckBoxIntermediate;

        /// <summary>
        /// The CheckBox tick icon.
        /// </summary>
        public Sprite CheckBoxTick;

        /// <summary>
        /// The status bar size grip icon.
        /// </summary>
        public Sprite StatusBarSizeGrip;

        /// <summary>
        /// The translate icon.
        /// </summary>
        public Sprite Translate;

        /// <summary>
        /// The rotate icon.
        /// </summary>
        public Sprite Rotate;

        /// <summary>
        /// The scale icon.
        /// </summary>
        public Sprite Scale;

        /// <summary>
        /// The shared tooltip control used by the controls if no custom tooltip is provided.
        /// </summary>
        public Tooltip SharedTooltip;
    }
}
