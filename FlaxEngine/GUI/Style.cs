////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.Assertions;

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

        /*
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
        */

        /// <summary>
        /// Creates default GUI style
        /// </summary>
        /// <returns>GUI style</returns>
        public static Style CreateDefault()
        {
            Style style = new Style();

            // Font
            string primaryFontNameInternal = "Editor/Segoe Media Center Regular";
            var primaryFont = Content.LoadInternal<FontAsset>(primaryFontNameInternal);
            if (primaryFont)
            {
                // Create fonts
                style.FontTitle = primaryFont.CreateFont(18);
                style.FontLarge = primaryFont.CreateFont(14);
                style.FontMedium = primaryFont.CreateFont(9);
                style.FontSmall = primaryFont.CreateFont(9);

                Assert.IsNotNull(style.FontTitle, "Missing Title font.");
                Assert.IsNotNull(style.FontLarge, "Missing Large font.");
                Assert.IsNotNull(style.FontMedium, "Missing Medium font.");
                Assert.IsNotNull(style.FontSmall, "Missing Small font.");
            }
            else
            {
                Debug.LogError("Cannot load primary GUI Style font " + primaryFontNameInternal);
            }

            // Metro Style colors
            style.Background = Color.FromBgra(0xFF1C1C1C);
            style.LightBackground = Color.FromBgra(0xFF2D2D30);
            style.Foreground = Color.FromBgra(0xFFFFFFFF);
            style.ForegroundDisabled = new Color(0.6f);
            style.BackgroundHighlighted = Color.FromBgra(0xFF54545C);
            style.BorderHighlighted = Color.FromBgra(0xFF6A6A75);
            style.BackgroundSelected = Color.FromBgra(0xFF007ACC);
            style.BorderSelected = Color.FromBgra(0xFF1C97EA);
            style.BackgroundNormal = Color.FromBgra(0xFF3F3F46);
            style.BorderNormal = Color.FromBgra(0xFF54545C);
            style.TextBoxBackground = Color.FromBgra(0xFF333337);
            style.TextBoxBackgroundSelected = Color.FromBgra(0xFF3F3F46);
            style.DragWindow = style.BackgroundSelected * 0.7f;
            style.ProgressNormal = Color.FromBgra(0xFF0ad328);

            return style;
        }
    }
}
