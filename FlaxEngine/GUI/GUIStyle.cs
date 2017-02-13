////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Describes GUI controls style (which fonts and colors use etc.)
    /// </summary>
    public class GUIStyle
    {
        /// <summary>
        /// Global GUI style used by all the control
        /// </summary>
        public static GUIStyle Current { get; set; } = CreateDefault();

        /*
                // Fonts
                public FontProperty PrimaryFont;
                public Font* FontTitle1;
                public Font* FontLarge;
                public Font* FontMedium;
                public Font* FontSmall;
                */
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
        /*
        // Icons
        public SpriteHandle ArrowRight;
        public SpriteHandle ArrowDown;
        public SpriteHandle Search;
        public SpriteHandle Settings;
        public SpriteHandle Cross;
        public SpriteHandle CheckBoxIntermediate;
        public SpriteHandle CheckBoxTick;
        public SpriteHandle StatusBarSizeGrip;
        public SpriteHandle Translate16;
        public SpriteHandle Rotate16;
        public SpriteHandle Scale16;
        */

        /// <summary>
        /// Creates default GUI style
        /// </summary>
        /// <returns>GUI style</returns>
        public static GUIStyle CreateDefault()
        {
            GUIStyle style = new GUIStyle();

            // Metro Style colors
            style.Background = Color.FromBgra(0x1C1C1C);
            style.LightBackground = Color.FromBgra(0x2D2D30);
            style.Foreground = Color.FromBgra(0xFFFFFF);
            style.ForegroundDisabled = new Color(0.6f);
            style.BackgroundHighlighted = Color.FromBgra(0x54545C);
            style.BorderHighlighted = Color.FromBgra(0x6A6A75);
            style.BackgroundSelected = Color.FromBgra(0x007ACC);
            style.BorderSelected = Color.FromBgra(0x1C97EA);
            style.BackgroundNormal = Color.FromBgra(0x3F3F46);
            style.BorderNormal = Color.FromBgra(0x54545C);
            style.TextBoxBackground = Color.FromBgra(0x333337);
            style.TextBoxBackgroundSelected = Color.FromBgra(0x3F3F46);
            style.DragWindow = style.BackgroundSelected * 0.7f;
            style.ProgressNormal = Color.FromBgra(0x0ad328);

            return style;
        }
    }
}
