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
            // Check if has no model or is inactive
            if (!_isActive
                /*|| _modelTranslateAxis == null || !_modelTranslateAxis.IsLoaded
                || _modelScaleAxis == ńull || !_modelScaleAxis.IsLoaded
                || view.Pass != RenderPass.GBufferFill*/
            )
                return;

            // Temorary data
            Matrix m1, m2, m3;

            // Switch mode
            const float gizmoModelsScale2RealGizmoSize = 0.075f;
            switch (_activeMode)
            {
                case Mode.Translate:
                {
                    if (!_modelTranslateAxis || !_modelTranslateAxis.IsLoaded)
                        break;

                    // Cache data
                    Matrix.Scaling(gizmoModelsScale2RealGizmoSize, out m3);
                    Matrix.Multiply(ref m3, ref _gizmoWorld, out m1);
                    var mesh = _modelTranslateAxis.LODs[0].Meshes[0];

                    // X axis
                    collector.AddDrawCall(mesh, _activeAxis == Axis.X ? _materialAxisFocus : _materialAxisX, ref m1);

                    // Y axis
                    Matrix.RotationZ(Mathf.PiOverTwo, out m2);
                    Matrix.Multiply(ref m2, ref m1, out m3);
                    collector.AddDrawCall(mesh, _activeAxis == Axis.Y ? _materialAxisFocus : _materialAxisY, ref m3);

                    // Z axis
                    Matrix.RotationY(-Mathf.PiOverTwo, out m2);
                    Matrix.Multiply(ref m2, ref m1, out m3);
                    collector.AddDrawCall(mesh, _activeAxis == Axis.Z ? _materialAxisFocus : _materialAxisZ, ref m3);

                    break;
                }

                /*case Mode.Rotate:
                {
                // TODO: render rotation gizmo
                break;
                }


                case Mode.Scale:
                {
                // Cache data
                Matrix.Scaling(gizmoModelsScale2RealGizmoSize, out m3);
                Matrix.Multiply(ref m3, ref _gizmoWorld, out m1);
                var model = _modelScaleAxis->GetModel();
                var mesh = model->LODs[0].Meshes[0];

                // X axis
                Matrix.RotationY(-Mathf.PiOverTwo, out m2);
                Matrix.Multiply(ref m2, ref m1, out m3);
                view.Cache->AddDrawCall(StaticFlags.None, mesh, _activeAxis == X ? _materialAxisFocus->GetMaterialBase() : _materialAxisX->GetMaterialBase(), m3);

                // Y axis
                Matrix.RotationX(Mathf.PiOverTwo, out m2);
                Matrix.Multiply(ref m2, ref m1, out m3);
                view.Cache->AddDrawCall(StaticFlags.None, mesh, _activeAxis == Y ? _materialAxisFocus->GetMaterialBase() : _materialAxisY->GetMaterialBase(), m3);

                // Z axis
                Matrix.RotationX(PI, out m2);
                Matrix.Multiply(ref m2, ref m1, out m3);
                view.Cache->AddDrawCall(StaticFlags.None, mesh, _activeAxis == Z ? _materialAxisFocus->GetMaterialBase() : _materialAxisZ->GetMaterialBase(), m3);

                break;
                }
                */
            }

            // Draw selected sub-object
            /*var subObject = GetSelectedSubObject();
            if (subObject)
            {
                subObject->DrawSelected(context, view);
            }*/
        }
    }
}
