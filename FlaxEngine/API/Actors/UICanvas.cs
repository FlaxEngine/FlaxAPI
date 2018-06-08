// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Globalization;
using System.IO;
using System.Text;
using FlaxEngine.GUI;
using Newtonsoft.Json;

namespace FlaxEngine
{
    /// <summary>
    /// The canvas rendering modes.
    /// </summary>
    public enum CanvasRenderMode
    {
        /// <summary>
        /// The screen space rendering mode that places UI elements on the screen rendered on top of the scene. If the screen is resized or changes resolution, the Canvas will automatically change size to match this.
        /// </summary>
        ScreenSpace = 0,

        /// <summary>
        /// The camera space rendering mode that places Canvas in a given distance in front of a specified Camera. The UI elements are rendered by this camera, which means that the Camera settings affect the appearance of the UI. If the Camera is set to Perspective, the UI elements will be rendered with perspective, and the amount of perspective distortion can be controlled by the Camera Field of View. If the screen is resized, changes resolution, or the camera frustum changes, the Canvas will automatically change size to match as well.
        /// </summary>
        CameraSpace = 1,

        /// <summary>
        /// The world space rendering mode that places Canvas as any other object in the scene. The size of the Canvas can be set manually using its Rect Transform, and UI elements will render in front of or behind other objects in the scene based on 3D placement. This is useful for UIs that are meant to be a part of the world. This is also known as a "diegetic interface".
        /// </summary>
        WorldSpace = 2,
    }

    public sealed partial class UICanvas
    {
        private CanvasRenderMode _renderMode;
        private readonly CanvasRootControl _guiRoot;
        private bool _isLoading;

        /// <summary>
        /// Gets or sets the canvas rendering mode.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Canvas"), Tooltip("Canvas rendering mode.")]
        public CanvasRenderMode RenderMode
        {
            get => _renderMode;
            set
            {
                if (_renderMode != value)
                {
                    _renderMode = value;

                    Cleanup();
                    Setup();
                }
            }
        }

        /// <summary>
        /// Gets the canvas GUI root control.
        /// </summary>
        public CanvasRootControl GUI => _guiRoot;

        private UICanvas()
        {
            _guiRoot = new CanvasRootControl(this);
        }

        private void Setup()
        {
            if (_isLoading)
                return;

            _guiRoot.IsLayoutLocked = true;

            switch (_renderMode)
            {
            case CanvasRenderMode.ScreenSpace:
            {
                // Link to the game UI and fill the area
                _guiRoot.DockStyle = DockStyle.Fill;
                _guiRoot.Parent = RootControl.GameRoot;
                break;
            }
            }

            _guiRoot.IsLayoutLocked = false;
            _guiRoot.PerformLayout();
        }

        private void Cleanup()
        {
            _guiRoot.Parent = null;
        }

        internal string Serialize()
        {
            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("RenderMode");
                jsonWriter.WriteValue(_renderMode);

                jsonWriter.WriteEndObject();
            }

            return sw.ToString();
        }

        internal void Deserialize(string json)
        {
            _isLoading = true;
            Json.JsonSerializer.Deserialize(this, json);
            _isLoading = false;

            Setup();
        }

        internal void EndPlay()
        {
            Cleanup();
        }
    }
}
