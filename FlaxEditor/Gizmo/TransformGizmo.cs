////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// The most import gizmo tool used to move, rotate, scale and select objects in viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public partial class TransformGizmo : GizmoBase
    {
        /// <summary>
        /// Applies scale to the selected objects pool.
        /// </summary>
        /// <param name="selection">The selected objects pool.</param>
        /// <param name="translationDelta">The translation delta.</param>
        /// <param name="rotationDelta">The rotation delta.</param>
        /// <param name="scaleDelta">The scale delta.</param>
        public delegate void ApplyTransformationDelegate(List<ISceneTreeNode> selection, ref Vector3 translationDelta, ref Matrix rotationDelta, ref Vector3 scaleDelta);

        private bool _isTransforming;
        private bool _isCloning;
        private bool _isActive;
        private readonly List<Transform> _startTransforms = new List<Transform>();
        private readonly List<ISceneTreeNode> _selection = new List<ISceneTreeNode>();
        private readonly List<ISceneTreeNode> _selectionParents = new List<ISceneTreeNode>();

        private Matrix _screenScaleMatrix;
        private float _screenScale;

        private Vector3 _position;
        private Vector3 _accMoveDelta;
        private Matrix _rotationMatrix;

        private Vector3 _localForward = Vector3.ForwardLH;
        private Vector3 _localUp = Vector3.Up;
        private Vector3 _localRight = Vector3.Right;

        private Matrix _objectOrientedWorld = Matrix.Identity;
        private Matrix _axisAlignedWorld = Matrix.Identity;

        private Matrix _gizmoWorld = Matrix.Identity;

        private Vector3 _translationDelta;
        private Matrix _rotationDelta = Matrix.Identity;
        private Vector3 _scaleDelta;

        private Vector3 _tDelta;
        private Vector3 _lastIntersectionPosition;
        private Vector3 _intersectPosition;

        private Vector3 _translationScaleSnapDelta;
        private float _rotationSnapDelta;

        /// <summary>
        /// The event to apply objects transformation.
        /// </summary>
        public ApplyTransformationDelegate OnApplyTransformation;

        /// <summary>
        /// Gets the gizmo position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position => _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformGizmo"/> class.
        /// </summary>
        /// <param name="owner">The gizmos owner.</param>
        public TransformGizmo(IGizmoOwner owner)
            : base(owner)
        {
            InitDrawing();
        }

        private void StartTransforming()
        {
            // Check if can start new action
            int count = _selectionParents.Count;
            if (count <= 0 || _isTransforming)
            {
                // Back
                return;
            }

            // Start
            _isTransforming = true;

            // Cache 'before' state
            _startTransforms.Clear();
            if (_startTransforms.Capacity < count)
                _startTransforms.Capacity = Mathf.NextPowerOfTwo(count);
            for (int i = 0; i < count; i++)
                _startTransforms.Add(_selectionParents[i].Transform);
        }

        private void EndTransforming()
        {
            // Check if wasn't working at all
            if (!_isTransforming)
            {
                // Back
                return;
            }

            // End action
            /*var ur = _parent->GetUndoRedo();
            if (!ur->IsDisabled())
            {
                ur->AddAction(new TransformActors(this, _startTransforms));
            }*/
            _startTransforms.Clear();
            _isTransforming = false;
            _isCloning = false;
            /*_parent->OnTransfomingEnd();
            _parent->OnTransformObject();*/
        }

        private void UpdateGizmoPosition()
        {
            switch (_activePivotType)
            {
                case PivotType.ObjectCenter:
                    if (_selection.Count > 0)
                        _position = _selection[0].Position;
                    break;
                case PivotType.SelectionCenter:
                    _position = GetSelectionCenter();
                    break;
                case PivotType.WorldOrigin:
                    _position = Vector3.Zero;
                    break;
            }
            _position += _translationDelta;
        }

        private void UpdateMatricies()
        {
            // Check there is no need to perform update
            if (_selection.Count == 0)
                return;

            // Set positions of the gizmo
            UpdateGizmoPosition();

            // Scale Gizmo to fit on-screen
            Vector3 vLength = Owner.ViewPosition - _position;
            _screenScale = vLength.Length / GizmoScaleFactor;
            Matrix.Scaling(_screenScale, out _screenScaleMatrix);

            // TODO: use quaternion instead of matrix?
            Matrix rotation;
            var orientation = _selection[0].Orientation;
            Matrix.RotationQuaternion(ref orientation, out rotation);
            _localForward = rotation.Forward;
            _localUp = rotation.Up;

            // Vector Rotation (Local/World)
            _localForward.Normalize();
            Vector3.Cross(ref _localForward, ref _localUp, out _localRight);
            Vector3.Cross(ref _localRight, ref _localForward, out _localUp);
            _localRight.Normalize();
            _localUp.Normalize();

            // Create both world matrices
            _objectOrientedWorld = _screenScaleMatrix * Matrix.CreateWorld(_position, _localForward, _localUp);
            _axisAlignedWorld = _screenScaleMatrix * Matrix.CreateWorld(_position, Vector3.ForwardRH, Vector3.Up);

            // Assign world
            if (_activeTransformSpace == TransformSpace.World || _activeMode == Mode.Rotate || _activeMode == Mode.Scale)
            {
                _gizmoWorld = _axisAlignedWorld;

                // Align lines, boxes etc. with the grid-lines
                _rotationMatrix = Matrix.Identity;
            }
            else
            {
                _gizmoWorld = _objectOrientedWorld;

                // Align lines, boxes etc. with the selected object
                _rotationMatrix.Forward = _localForward;
                _rotationMatrix.Up = _localUp;
                _rotationMatrix.Right = _localRight;
            }
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            bool isLeftBtnDown = Owner.IsLeftMouseButtonDown;

            // Only when is active
            if (_isActive)
            {
                // Backup position
                _lastIntersectionPosition = _intersectPosition;
                _intersectPosition = Vector3.Zero;

                // Check if user is holding left mouse button and any axis is selected
                if (isLeftBtnDown && _activeAxis != Axis.None)
                {
                    switch (_activeMode)
                    {
                        case Mode.Scale:
                        case Mode.Translate:
                        {
                            bool isScalling = _activeMode == Mode.Scale;

                            Vector3 delta = Vector3.Zero;
                            Ray ray = Owner.MouseRay;

                            Matrix invRotationMatrix;
                            Matrix.Invert(ref _rotationMatrix, out invRotationMatrix);
                            ray.Position = Vector3.Transform(ray.Position, invRotationMatrix);
                            Vector3.TransformNormal(ref ray.Direction, ref invRotationMatrix, out ray.Direction);

                            switch (_activeAxis)
                            {
                                case Axis.XY:
                                case Axis.X:
                                {
                                    Plane plane = new Plane(Vector3.BackwardLH, Vector3.Transform(_position, invRotationMatrix).Z);

                                    float intersection;
                                    if (ray.Intersects(ref plane, out intersection))
                                    {
                                        _intersectPosition = (ray.Position + (ray.Direction * intersection));
                                        if (_lastIntersectionPosition != Vector3.Zero)
                                        {
                                            _tDelta = _intersectPosition - _lastIntersectionPosition;
                                        }
                                        delta = _activeAxis == Axis.X
                                            ? new Vector3(_tDelta.X, 0, 0)
                                            : new Vector3(_tDelta.X, _tDelta.Y, 0);
                                    }

                                    break;
                                }


                                case Axis.Z:
                                case Axis.YZ:
                                case Axis.Y:
                                {
                                    Plane plane = new Plane(Vector3.Left, Vector3.Transform(_position, invRotationMatrix).X);

                                    float intersection;
                                    if (ray.Intersects(ref plane, out intersection))
                                    {
                                        _intersectPosition = (ray.Position + (ray.Direction * intersection));
                                        if (_lastIntersectionPosition != Vector3.Zero)
                                        {
                                            _tDelta = _intersectPosition - _lastIntersectionPosition;
                                        }
                                        switch (_activeAxis)
                                        {
                                            case Axis.Y:
                                                delta = new Vector3(0, _tDelta.Y, 0);
                                                break;
                                            case Axis.Z:
                                                delta = new Vector3(0, 0, _tDelta.Z);
                                                break;
                                            default:
                                                delta = new Vector3(0, _tDelta.Y, _tDelta.Z);
                                                break;
                                        }
                                    }

                                    break;
                                }

                                case Axis.ZX:
                                {
                                    Plane plane = new Plane(Vector3.Down, Vector3.Transform(_position, invRotationMatrix).Y);

                                    float intersection;
                                    if (ray.Intersects(ref plane, out intersection))
                                    {
                                        _intersectPosition = (ray.Position + (ray.Direction * intersection));
                                        if (_lastIntersectionPosition != Vector3.Zero)
                                        {
                                            _tDelta = _intersectPosition - _lastIntersectionPosition;
                                        }
                                        delta = new Vector3(_tDelta.X, 0, _tDelta.Z);
                                    }

                                    break;
                                }

                                case Axis.Center:
                                {
                                    Vector3 gizmoToView = _position - Owner.ViewPosition;
                                    Plane plane = new Plane(-Vector3.Normalize(gizmoToView), gizmoToView.Length);

                                    float intersection;
                                    if (ray.Intersects(ref plane, out intersection))
                                    {
                                        _intersectPosition = (ray.Position + (ray.Direction * intersection));
                                        if (_lastIntersectionPosition != Vector3.Zero)
                                        {
                                            _tDelta = _intersectPosition - _lastIntersectionPosition;
                                        }
                                    }

                                    delta = _tDelta;

                                    break;
                                }
                            }

                            if ((isScalling ? ScaleSnapEnabled : TranslationSnapEnable) || Owner.UseSnapping)
                            {
                                float snapValue = isScalling ? ScaleSnapValue : TranslationSnapValue;
                                if (_precisionModeEnabled)
                                {
                                    delta *= PrecisionModeScale;
                                    snapValue *= PrecisionModeScale;
                                }

                                _translationScaleSnapDelta += delta;

                                delta = new Vector3(
                                    (float)(int)(_translationScaleSnapDelta.X / snapValue) * snapValue,
                                    (float)(int)(_translationScaleSnapDelta.Y / snapValue) * snapValue,
                                    (float)(int)(_translationScaleSnapDelta.Z / snapValue) * snapValue);

                                _translationScaleSnapDelta -= delta;
                            }
                            else if (_precisionModeEnabled)
                            {
                                delta *= PrecisionModeScale;
                            }

                            if (_activeMode == Mode.Translate)
                            {
                                // Transform (local or world)
                                delta = Vector3.Transform(delta, _rotationMatrix);
                                _translationDelta = delta;
                            }
                            else if (_activeMode == Mode.Scale)
                            {
                                // Apply Scale
                                _scaleDelta += delta * ScaleFactor;
                            }
                        }
                            break;

                        case Mode.Rotate:
                        {
                            float delta = Owner.MouseDelta.X * dt;
                                
                            if (RotationSnapEnabled || Owner.UseSnapping)
                            {
                                float snapValue = RotationSnapValue * Mathf.DegreesToRadians;
                                if (_precisionModeEnabled)
                                {
                                    delta *= PrecisionModeScale;
                                    snapValue *= PrecisionModeScale;
                                }

                                _rotationSnapDelta += delta;

                                float snapped = Mathf.Round(_rotationSnapDelta / snapValue) * snapValue;
                                _rotationSnapDelta -= snapped;

                                delta = snapped;
                            }
                            else if (_precisionModeEnabled)
                            {
                                delta *= PrecisionModeScale;
                            }

                            switch (_activeAxis)
                            {
                                case Axis.X:
                                case Axis.Y:
                                case Axis.Z:
                                {
                                    Vector3 dir;
                                    if (_activeAxis == Axis.X)
                                        dir = _rotationMatrix.Right;
                                    else if (_activeAxis == Axis.Y)
                                        dir = _rotationMatrix.Up;
                                    else
                                        dir = _rotationMatrix.Forward;
                                    Matrix.CreateFromAxisAngle(ref dir, delta, out _rotationDelta);
                                    break;
                                }

                                default:
                                    _rotationDelta = Matrix.Identity;
                                    break;
                            }

                            break;
                        }

                    }
                }
                else
                {
                    // If nothing selected, try to select any axis
                    if (!isLeftBtnDown && !Owner.IsRightMouseButtonDown)
                    {
                        SelectAxis();
                    }
                }

                // Set positions of the gizmo
                UpdateGizmoPosition();

                // Trigger Translation, Rotation & Scale events
                if (isLeftBtnDown)
                {
                    bool anyValid = false;

                    // Translation
                    Vector3 translationDelta;
                    if (_translationDelta.LengthSquared > 0.000001f)
                    {
                        anyValid = true;
                        translationDelta = _translationDelta;
                        _translationDelta = Vector3.Zero;

                        // Prevent from moving objects too far away, like to diffrent galaxy or sth
                        var prevMoveDelta = _accMoveDelta;
                        _accMoveDelta += _translationDelta;
                        if (_accMoveDelta.Length > Owner.ViewFarPlane * 0.7f)
                        {
                            _accMoveDelta = prevMoveDelta;
                        }
                    }
                    else
                    {
                        translationDelta = Vector3.Zero;
                    }

                    // Rotation
                    Matrix rotationDelta;
                    if (!_rotationDelta.IsIdentity)
                    {
                        anyValid = true;
                        rotationDelta = _rotationDelta;
                        _rotationDelta = Matrix.Identity;
                    }
                    else
                    {
                        rotationDelta = Matrix.Identity;
                    }

                    // Scale
                    Vector3 scaleDelta;
                    if (_scaleDelta.LengthSquared > 0.000001f)
                    {
                        anyValid = true;
                        scaleDelta = _scaleDelta;
                        _scaleDelta = Vector3.Zero;
                    }
                    else
                    {
                        scaleDelta = ActiveAxis == Axis.Center ? Vector3.One : Vector3.Zero;
                    }

                    // Apply transformation (but to the parents, not whole selection pool)
                    if (anyValid)
                    {
                        StartTransforming();

                        OnApplyTransformation(_selectionParents, ref translationDelta, ref rotationDelta, ref scaleDelta);
                    }
                }
                else
                {
                    // Clear cache
                    _accMoveDelta = Vector3.Zero;
                    EndTransforming();
                }
            }

            // Check if has no objects selected
            if (_selection.Count == 0)
            {
                // Deactivate
                _isActive = false;
                _activeAxis = Axis.None;
                return;
            }

            // Helps solve visual lag (1-frame-lag) after selecting a new entity
            if (!_isActive)
            {
                UpdateGizmoPosition();
            }

            // Activate
            _isActive = true;

            // Update
            UpdateMatricies();
            
            // TODO: draw gizmo planes
            /*if (_activeMode == Mode.Translate)
            {
                //DebugDraw.Instance()->DrawBox(XAxisBox * _gizmoWorld, _activeAxis == X ? Color.Yellow : Color.Magenta, 0, false);
                //DebugDraw.Instance()->DrawBox(YAxisBox * _gizmoWorld, _activeAxis == Y ? Color.Yellow : Color.Magenta, 0, false);
                //DebugDraw.Instance()->DrawBox(ZAxisBox * _gizmoWorld, _activeAxis == Z ? Color.Yellow : Color.Magenta, 0, false);

                DebugDraw.Instance()->DrawBox(OrientedBoundingBox(XYBox) * _gizmoWorld, _activeAxis == XY ? Color.Yellow : Color.Gray, 0, false);
                DebugDraw.Instance()->DrawBox(OrientedBoundingBox(XZBox) * _gizmoWorld, _activeAxis == ZX ? Color.Yellow : Color.Gray, 0, false);
                DebugDraw.Instance()->DrawBox(OrientedBoundingBox(YZBox) * _gizmoWorld, _activeAxis == YZ ? Color.Yellow : Color.Gray, 0, false);
            }
            else if (_activeMode == Mode.Scale)
            {
                DebugDraw.Instance()->DrawBox(OrientedBoundingBox(CenterBox) * _gizmoWorld, _activeAxis == Center ? Color.Yellow : Color.Gray, 0, false);

                //DebugDraw.Instance()->DrawSphere(getScaleXSphere(), _activeAxis == X ? Color.Yellow : Color.Magenta, 0, false);
                //DebugDraw.Instance()->DrawSphere(getScaleYSphere(), _activeAxis == Y ? Color.Yellow : Color.Magenta, 0, false);
                //DebugDraw.Instance()->DrawSphere(getScaleZSphere(), _activeAxis == Z ? Color.Yellow : Color.Magenta, 0, false);
            }
            else
            {
                //DebugDraw.Instance()->DrawSphere(getRotateXSphere(), _activeAxis == X ? Color.Yellow : Color.Magenta, 0, false);

                DebugDraw.Instance()->DrawSphere(getRotateXSphere(), _activeAxis == X ? Color.Yellow : Color.Magenta, 0, false);
                DebugDraw.Instance()->DrawSphere(getRotateYSphere(), _activeAxis == Y ? Color.Yellow : Color.Magenta, 0, false);
                DebugDraw.Instance()->DrawSphere(getRotateZSphere(), _activeAxis == Z ? Color.Yellow : Color.Magenta, 0, false);
            }*/
        }
    }
}
