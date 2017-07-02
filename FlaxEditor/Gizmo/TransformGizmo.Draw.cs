////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    public partial class TransformGizmo
    {
        private Model _modelTranslateAxis;
        private Model _modelScaleAxis;
        private MaterialInstance _materialAxisX;
        private MaterialInstance _materialAxisY;
        private MaterialInstance _materialAxisZ;
        private MaterialInstance _materialAxisFocus;

        private void InitDrawing()
        {
            // Load content (but async - don't wait and don't block execution)
            _modelTranslateAxis = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Gizmo/TranslateAxis");
            _modelScaleAxis = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Gizmo/ScaleAxis");
            _materialAxisX = FlaxEngine.Content.LoadAsyncInternal<MaterialInstance>("Editor/Gizmo/MaterialAxisX");
            _materialAxisY = FlaxEngine.Content.LoadAsyncInternal<MaterialInstance>("Editor/Gizmo/MaterialAxisY");
            _materialAxisZ = FlaxEngine.Content.LoadAsyncInternal<MaterialInstance>("Editor/Gizmo/MaterialAxisZ");
            _materialAxisFocus = FlaxEngine.Content.LoadAsyncInternal<MaterialInstance>("Editor/Gizmo/MaterialAxisFocus");

            // Ensure that evey asset was loaded
            if (_modelTranslateAxis == null ||
                _modelScaleAxis == null ||
                _materialAxisX == null ||
                _materialAxisY == null ||
                _materialAxisZ == null ||
                _materialAxisFocus == null
            )
            {
                // Error
                Application.Fatal("Failed to load Transform Gizmo resources.");
            }
        }

        /// <inheritdoc />
        public override void Draw(DrawCallsCollector collector)
        {
        }
    }
}
