////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Describies GUI style used by the surface.
    /// </summary>
    public class SurfaceStyle
    {
        /// <summary>
        /// Description with the colors used by the surface.
        /// </summary>
        public struct ColorsData
        {
            // Connecting nodes
            public Color Connecting;
            public Color ConnectingValid;
            public Color ConnectingInvalid;

            // Boxes
            public Color Impulse;
            public Color Bool;
            public Color Integer;
            public Color Float;
            public Color Vector;
            public Color String;
            public Color Object;
            public Color Rotation;
            public Color Transform;
            public Color Box;
            public Color Default;
        }

        /// <summary>
        /// Descriptions for icons used by the surface.
        /// </summary>
        public struct IconsData
        {
            public Sprite BoxOpen;
            public Sprite BoxClose;
            public Sprite ArowOpen;
            public Sprite ArowClose;
        }

        /// <summary>
        /// The colors.
        /// </summary>
        public ColorsData Colors;

        /// <summary>
        /// The icons.
        /// </summary>
        public IconsData Icons;

        /// <summary>
        /// Gets the color for the connection.
        /// </summary>
        /// <param name="type">The connection type.</param>
        /// <param name="color">The color.</param>
        public void GetConnectionColor(ConnectionType type, out Color color)
        {
            switch (type)
            {
                case ConnectionType.Impulse: color = Colors.Impulse; break;
                case ConnectionType.Bool: color = Colors.Box; break;
                case ConnectionType.Integer: color = Colors.Integer; break;
                case ConnectionType.Float: color = Colors.Float; break;
                case ConnectionType.Vector2:
                case ConnectionType.Vector3:
                case ConnectionType.Vector4:
                case ConnectionType.Vector: color = Colors.Vector; break;
                case ConnectionType.String: color = Colors.String; break;
                case ConnectionType.Object: color = Colors.Object; break;
                case ConnectionType.Rotation: color = Colors.Rotation; break;
                case ConnectionType.Transform: color = Colors.Transform; break;
                case ConnectionType.Box: color = Colors.Box; break;
                default: color = Colors.Default; break;
            }
        }

        /// <summary>
        ///  Function used to create style for the given surface type. Can be overriden to provide some customization via user plugin.
        /// </summary>
        public static Func<Editor, SurfaceType, SurfaceStyle> CreateStyleHandler = CreateDefault;

        /// <summary>
        /// Creates the default style.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="surfaceType">Type of the surface.</param>
        /// <returns>Created style.</returns>
        public static SurfaceStyle CreateDefault(Editor editor, SurfaceType surfaceType)
        {
            switch (surfaceType)
            {
                case SurfaceType.Material: return CreateDefaultMaterial(editor);
                default: throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Creates the material surface style.
        /// </summary>
        /// <param name="editor">The editor instance.</param>
        /// <returns>Created style descriptor.</returns>
        public static SurfaceStyle CreateDefaultMaterial(Editor editor)
        {
            return new SurfaceStyle
            {
                Colors =
                {
                    // Connecting nodes
                    Connecting = Color.White,
                    ConnectingValid = new Color(11, 252, 11),
                    ConnectingInvalid = new Color(252, 12, 11),

                    // Boxes
                    Impulse = new Color(252, 255, 255),
                    Bool = new Color(237, 28, 36),
                    Integer = new Color(181, 230, 29),
                    Float = new Color(146, 208, 80),
                    Vector = new Color(255, 251, 1),
                    String = new Color(163, 73, 164),
                    Object = new Color(0, 162, 232),
                    Rotation = new Color(153, 217, 234),
                    Transform = new Color(255, 127, 39),
                    Box = new Color(34, 117, 76),
                    Default = new Color(200, 200, 200),
                },
                Icons =
                {
                    BoxOpen = editor.UI.GetIcon("VisjectBoxOpen"),
                    BoxClose = editor.UI.GetIcon("VisjectBoxClose"),
                    ArowOpen = editor.UI.GetIcon("VisjectArrowOpen"),
                    ArowClose = editor.UI.GetIcon("VisjectArrowClose"),
                }
            };
        }
    }
}
