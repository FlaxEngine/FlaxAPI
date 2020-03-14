// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

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

        /// <summary>
        /// The font title.
        /// </summary>
        public Font FontTitle;

        /// <summary>
        /// The font large.
        /// </summary>
        public Font FontLarge;

        /// <summary>
        /// The font medium.
        /// </summary>
        public Font FontMedium;

        /// <summary>
        /// The font small.
        /// </summary>
        public Font FontSmall;

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
        public SpriteHandle ArrowRight;

        /// <summary>
        /// The arrow down icon.
        /// </summary>
        public SpriteHandle ArrowDown;

        /// <summary>
        /// The search icon.
        /// </summary>
        public SpriteHandle Search;

        /// <summary>
        /// The settings icon.
        /// </summary>
        public SpriteHandle Settings;

        /// <summary>
        /// The cross icon.
        /// </summary>
        public SpriteHandle Cross;

        /// <summary>
        /// The CheckBox intermediate icon.
        /// </summary>
        public SpriteHandle CheckBoxIntermediate;

        /// <summary>
        /// The CheckBox tick icon.
        /// </summary>
        public SpriteHandle CheckBoxTick;

        /// <summary>
        /// The status bar size grip icon.
        /// </summary>
        public SpriteHandle StatusBarSizeGrip;

        /// <summary>
        /// The translate icon.
        /// </summary>
        public SpriteHandle Translate;

        /// <summary>
        /// The rotate icon.
        /// </summary>
        public SpriteHandle Rotate;

        /// <summary>
        /// The scale icon.
        /// </summary>
        public SpriteHandle Scale;

        /// <summary>
        /// The shared tooltip control used by the controls if no custom tooltip is provided.
        /// </summary>
        public Tooltip SharedTooltip;
    }
}
