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
        [EditorOrder(10)]
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
        [EditorOrder(20)]
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
        [EditorOrder(30)]
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
        [EditorOrder(40)]
        public Font FontSmall
        {
            get => _fontSmall.GetFont();
            set => _fontSmall = new FontReference(value);
        }

        /// <summary>
        /// The background color.
        /// </summary>
        [EditorOrder(60)]
        public Color Background;

        /// <summary>
        /// The light background color.
        /// </summary>
        [EditorOrder(70)]
        public Color LightBackground;

        /// <summary>
        /// The drag window color.
        /// </summary>
        [EditorOrder(80)]
        public Color DragWindow;

        /// <summary>
        /// The foreground color (text).
        /// </summary>
        [EditorOrder(90)]
        public Color Foreground;

        /// <summary>
        /// The foreground disabled (text).
        /// </summary>
        [EditorOrder(100)]
        public Color ForegroundDisabled;

        /// <summary>
        /// The background highlighted color.
        /// </summary>
        [EditorOrder(110)]
        public Color BackgroundHighlighted;

        /// <summary>
        /// The border highlighted color.
        /// </summary>
        [EditorOrder(120)]
        public Color BorderHighlighted;

        /// <summary>
        /// The background selected color.
        /// </summary>
        [EditorOrder(130)]
        public Color BackgroundSelected;

        /// <summary>
        /// The border selected color.
        /// </summary>
        [EditorOrder(140)]
        public Color BorderSelected;

        /// <summary>
        /// The background normal color.
        /// </summary>
        [EditorOrder(150)]
        public Color BackgroundNormal;

        /// <summary>
        /// The border normal color.
        /// </summary>
        [EditorOrder(160)]
        public Color BorderNormal;

        /// <summary>
        /// The text box background color.
        /// </summary>
        [EditorOrder(170)]
        public Color TextBoxBackground;

        /// <summary>
        /// The text box background selected color.
        /// </summary>
        [EditorOrder(180)]
        public Color TextBoxBackgroundSelected;

        /// <summary>
        /// The progress normal color.
        /// </summary>
        [EditorOrder(190)]
        public Color ProgressNormal;

        /// <summary>
        /// The arrow right icon.
        /// </summary>
        [EditorOrder(210)]
        public Sprite ArrowRight;

        /// <summary>
        /// The arrow down icon.
        /// </summary>
        [EditorOrder(220)]
        public Sprite ArrowDown;

        /// <summary>
        /// The search icon.
        /// </summary>
        [EditorOrder(230)]
        public Sprite Search;

        /// <summary>
        /// The settings icon.
        /// </summary>
        [EditorOrder(240)]
        public Sprite Settings;

        /// <summary>
        /// The cross icon.
        /// </summary>
        [EditorOrder(250)]
        public Sprite Cross;

        /// <summary>
        /// The CheckBox intermediate icon.
        /// </summary>
        [EditorOrder(260)]
        public Sprite CheckBoxIntermediate;

        /// <summary>
        /// The CheckBox tick icon.
        /// </summary>
        [EditorOrder(270)]
        public Sprite CheckBoxTick;

        /// <summary>
        /// The status bar size grip icon.
        /// </summary>
        [EditorOrder(280)]
        public Sprite StatusBarSizeGrip;

        /// <summary>
        /// The translate icon.
        /// </summary>
        [EditorOrder(290)]
        public Sprite Translate;

        /// <summary>
        /// The rotate icon.
        /// </summary>
        [EditorOrder(300)]
        public Sprite Rotate;

        /// <summary>
        /// The scale icon.
        /// </summary>
        [EditorOrder(310)]
        public Sprite Scale;

        /// <summary>
        /// The shared tooltip control used by the controls if no custom tooltip is provided.
        /// </summary>
        [EditorOrder(330)]
        public Tooltip SharedTooltip;
    }
}
