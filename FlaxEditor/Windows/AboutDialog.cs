////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.GUI.Dialogs;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// About this product dialog window.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    internal sealed class AboutDialog : Dialog
    {
        /// <inheritdoc />
        public AboutDialog()
            : base("About Flax")
        {
            Size = new Vector2(400, 260);

            // Icon
            var icon = new Image(false, 4, 4, 80, 80)
            {
                ImageSource = new SpriteImageSource(Editor.Instance.UI.GetIcon("Logo64")),
                Parent = this
            };

            // Name
            var nameLabel = new Label(false, icon.Right + 10, icon.Top, 100, 34)
            {
                Text = "Flax Engine",
                Font = Style.Current.FontTitle,
                HorizontalAlignment = TextAlignment.Near,
                VerticalAlignment = TextAlignment.Center,
                Parent = this
            };

            // Version
            var versionLabel = new Label(false, nameLabel.Left, nameLabel.Bottom + 4, nameLabel.Width, 50)
            {
                Text = string.Format("Version: {0}\nCopyright (c) 2012-2017 Wojciech Figat.\nAll rights reserved.", Globals.Version),
                HorizontalAlignment = TextAlignment.Near,
                VerticalAlignment = TextAlignment.Near,
                Parent = this
            };

            // Authors
            var authors = new List<string>(new[]
            {
                "Wojciech Figat",
                "Tomasz Juszczak",
                "Damian Korczowski",
            });
            authors.Sort();
            var authorsLabel = new Label(false, 4, icon.Bottom + 20, Width - 8, 50)
            {
                Text = "People who made it:\n" + string.Join(", ", authors),
                HorizontalAlignment = TextAlignment.Near,
                VerticalAlignment = TextAlignment.Near,
                Parent = this
            };
            
            // 3rdParty software and other licenses
            var thirdPartyPanel = new Panel(ScrollBars.Vertical)
            {
                Bounds = new Rectangle(0, authorsLabel.Bottom + 4, Width, Height - authorsLabel.Bottom - 24),
                Parent = this
            };
            var thirdPartyEntries = new[]
            {
                "Used third party software:",
                "",
                "Mono Project - www.mono-project.com",
                "FreeType Project - www.freetype.org",
                "Assimp - www.assimp.sourceforge.net",
                "DirectXMesh - Copyright (c) Microsoft Corporation. All rights reserved.",
                "DirectXTex - Copyright (c) Microsoft Corporation. All rights reserved.",
                "UVAtlas - Copyright (c) Microsoft Corporation. All rights reserved.",
                "fmt - www.fmtlib.net",
                "pugixml - www.pugixml.org",
                "rapidjson - www.rapidjson.org",
                "Editor icons - www.icons8.com, www.iconfinder.com",
            };
            float y = 0;
            for (var i = 0; i < thirdPartyEntries.Length; i++)
            {
                var entry = thirdPartyEntries[i];
                var entryLabel = new Label(false, 0, y, Width, 14)
                {
                    Text = entry,
                    HorizontalAlignment = TextAlignment.Near,
                    VerticalAlignment = TextAlignment.Center,
                    Parent = thirdPartyPanel
                };
                y += entryLabel.Height + 2;
            }

            // Footer
            var footerLabel = new Label(false, 4, thirdPartyPanel.Bottom, Width - 8, Height - thirdPartyPanel.Bottom)
            {
                HorizontalAlignment = TextAlignment.Far,
                Text = "Made with <3 in Poland",
                Parent = this
            };
        }
    }
}
