// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Describes GUI style used by the surface.
    /// </summary>
    public class SurfaceStyle
    {
        /// <summary>
        /// Description with the colors used by the surface.
        /// </summary>
        public struct ColorsData
        {
            /// <summary>
            /// The connecting nodes color.
            /// </summary>
            public Color Connecting;

            /// <summary>
            /// The connecting nodes color (for valid connection).
            /// </summary>
            public Color ConnectingValid;

            /// <summary>
            /// The connecting nodes color (for invalid connection).
            /// </summary>
            public Color ConnectingInvalid;

            /// <summary>
            /// The impulse boxes color.
            /// </summary>
            public Color Impulse;

            /// <summary>
            /// The boolean boxes color.
            /// </summary>
            public Color Bool;

            /// <summary>
            /// The integer boxes color.
            /// </summary>
            public Color Integer;

            /// <summary>
            /// The floating point boxes color.
            /// </summary>
            public Color Float;

            /// <summary>
            /// The vector boxes color.
            /// </summary>
            public Color Vector;

            /// <summary>
            /// The string boxes color.
            /// </summary>
            public Color String;

            /// <summary>
            /// The object boxes color.
            /// </summary>
            public Color Object;

            /// <summary>
            /// The rotation boxes color.
            /// </summary>
            public Color Rotation;

            /// <summary>
            /// The transform boxes color.
            /// </summary>
            public Color Transform;

            /// <summary>
            /// The box boxes color.
            /// </summary>
            public Color Box;

            /// <summary>
            /// The impulse (secondary) boxes color.
            /// </summary>
            public Color ImpulseSecondary;

            /// <summary>
            /// The default boxes color.
            /// </summary>
            public Color Default;
        }

        /// <summary>
        /// Descriptions for icons used by the surface.
        /// </summary>
        public struct IconsData
        {
            /// <summary>
            /// Icon for boxes without connections.
            /// </summary>
            public Sprite BoxOpen;

            /// <summary>
            /// Icon for boxes with connections.
            /// </summary>
            public Sprite BoxClose;

            /// <summary>
            /// Icon for impulse boxes without connections.
            /// </summary>
            public Sprite ArrowOpen;

            /// <summary>
            /// Icon for impulse boxes with connections.
            /// </summary>
            public Sprite ArrowClose;
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
        /// The background image (tiled).
        /// </summary>
        public Texture Background;

        /// <summary>
        /// Gets the color for the connection.
        /// </summary>
        /// <param name="type">The connection type.</param>
        /// <param name="color">The color.</param>
        public void GetConnectionColor(ConnectionType type, out Color color)
        {
            switch (type)
            {
            case ConnectionType.Impulse:
                color = Colors.Impulse;
                break;
            case ConnectionType.Bool:
                color = Colors.Bool;
                break;
            case ConnectionType.Integer:
                color = Colors.Integer;
                break;
            case ConnectionType.Float:
                color = Colors.Float;
                break;
            case ConnectionType.Vector2:
            case ConnectionType.Vector3:
            case ConnectionType.Vector4:
            case ConnectionType.Vector:
                color = Colors.Vector;
                break;
            case ConnectionType.String:
                color = Colors.String;
                break;
            case ConnectionType.Object:
                color = Colors.Object;
                break;
            case ConnectionType.Rotation:
                color = Colors.Rotation;
                break;
            case ConnectionType.Transform:
                color = Colors.Transform;
                break;
            case ConnectionType.Box:
                color = Colors.Box;
                break;
            case ConnectionType.ImpulseSecondary:
                color = Colors.ImpulseSecondary;
                break;
            default:
                color = Colors.Default;
                break;
            }
        }

        /// <summary>
        ///  Function used to create style for the given surface type. Can be overriden to provide some customization via user plugin.
        /// </summary>
        public static Func<Editor, SurfaceStyle> CreateStyleHandler = CreateDefault;

        /// <summary>
        /// Creates the default style.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <returns>Created style.</returns>
        public static SurfaceStyle CreateDefault(Editor editor)
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
                    ImpulseSecondary = new Color(40, 130, 50),
                },
                Icons =
                {
                    BoxOpen = editor.Icons.VisjectBoxOpen,
                    BoxClose = editor.Icons.VisjectBoxClose,
                    ArrowOpen = editor.Icons.VisjectArrowOpen,
                    ArrowClose = editor.Icons.VisjectArrowClose,
                },
                Background = editor.UI.VisjectSurfaceBackground,
            };
        }
    }
}
