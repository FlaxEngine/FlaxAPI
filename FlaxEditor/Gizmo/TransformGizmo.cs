// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// The most import gizmo tool used to move, rotate, scale and select objects in viewport.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public partial class TransformGizmo : GizmoBase
    {
        private readonly List<SceneGraphNode> _selection = new List<SceneGraphNode>();
        private readonly List<SceneGraphNode> _selectionParents = new List<SceneGraphNode>();
        private readonly List<Transform> _startTransforms = new List<Transform>();
        private Vector3 _accMoveDelta;
        private Matrix _axisAlignedWorld = Matrix.Identity;

        private Matrix _gizmoWorld = Matrix.Identity;
        private Vector3 _intersectPosition;
        private bool _isActive;
        private bool _isDuplicating;

        private bool _isTransforming;
        private Vector3 _lastIntersectionPosition;

        private Vector3 _localForward = Vector3.Forward;
        private Vector3 _localRight = Vector3.Right;
        private Vector3 _localUp = Vector3.Up;

        private Matrix _objectOrientedWorld = Matrix.Identity;

        private Quaternion _rotationDelta = Quaternion.Identity;
        private Matrix _rotationMatrix;
        private float _rotationSnapDelta;
        private Vector3 _scaleDelta;
        private float _screenScale;

        private Matrix _screenScaleMatrix;

        private Vector3 _tDelta;

        private Vector3 _translationDelta;

        private Vector3 _translationScaleSnapDelta;

        /// <summary>
        /// The event to apply objects transformation.
        /// </summary>
        public ApplyTransformationDelegate OnApplyTransformation;

        /// <summary>
        /// The event to duplicate selected objects.
        /// </summary>
        public Action Duplicate;

        /// <summary>
        /// Gets the gizmo position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Gets the last transformation delta.
        /// </summary>
        public Transform LastDelta { get; private set; }

        /// <summary>
        /// Applies scale to the selected objects pool.
        /// </summary>
        /// <param name="selection">The selected objects pool.</param>
        /// <param name="translationDelta">The translation delta.</param>
        /// <param name="rotationDelta">The rotation delta.</param>
        /// <param name="scaleDelta">The scale delta.</param>
        public delegate void ApplyTransformationDelegate(List<SceneGraphNode> selection, ref Vector3 translationDelta, ref Quaternion rotationDelta, ref Vector3 scaleDelta);

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformGizmo" /> class.
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
                return;

            // Check if duplicate objects
            if (Owner.UseDuplicate && !_isDuplicating)
            {
                _isDuplicating = true;
                Duplicate();
                return;
            }

            // Start
            _isTransforming = true;

            // Cache 'before' state
            _startTransforms.Clear();
            if (_startTransforms.Capacity < count)
                _startTransforms.Capacity = Mathf.NextPowerOfTwo(count);
            for (var i = 0; i < count; i++)
                _startTransforms.Add(_selectionParents[i].Transform);
        }

        private void EndTransforming()
        {
            // Check if wasn't working at all
            if (!_isTransforming)
                return;

            // End action
            Owner.Undo.AddAction(new TransformObjectsAction(SelectedParents, _startTransforms));
            _startTransforms.Clear();
            _isTransforming = false;
            _isDuplicating = false;
        }

        private void UpdateGizmoPosition()
        {
            switch (_activePivotType)
            {
            case PivotType.ObjectCenter:
                if (_selection.Count > 0)
                    Position = _selection[0].Transform.Translation;
                break;
            case PivotType.SelectionCenter:
                Position = GetSelectionCenter();
                break;
            case PivotType.WorldOrigin:
                Position = Vector3.Zero;
                break;
            }
            Position += _translationDelta;
        }

        private void UpdateMatricies()
        {
            // Check there is no need to perform update
            if (_selection.Count == 0)
                return;

            // Set positions of the gizmo
            UpdateGizmoPosition();

            // Scale Gizmo to fit on-screen
            Vector3 vLength = Owner.ViewPosition - Position;
            _screenScale = vLength.Length / GizmoScaleFactor;
            Matrix.Scaling(_screenScale, out _screenScaleMatrix);

            Matrix rotation;
            Quaternion orientation = _selection[0].Transform.Orientation;
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
            _objectOrientedWorld = _screenScaleMatrix * Matrix.CreateWorld(Position, _localForward, _localUp);
            _axisAlignedWorld = _screenScaleMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            // Assign world
            if (_activeTransformSpace == TransformSpace.World)
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

        private void UpdateTranslateScale()
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
                var plane = new Plane(Vector3.Backward, Vector3.Transform(Position, invRotationMatrix).Z);

                float intersection;
                if (ray.Intersects(ref plane, out intersection))
                {
                    _intersectPosition = ray.Position + ray.Direction * intersection;
                    if (_lastIntersectionPosition != Vector3.Zero)
                        _tDelta = _intersectPosition - _lastIntersectionPosition;
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
                var plane = new Plane(Vector3.Left, Vector3.Transform(Position, invRotationMatrix).X);

                float intersection;
                if (ray.Intersects(ref plane, out intersection))
                {
                    _intersectPosition = ray.Position + ray.Direction * intersection;
                    if (_lastIntersectionPosition != Vector3.Zero)
                        _tDelta = _intersectPosition - _lastIntersectionPosition;
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
                var plane = new Plane(Vector3.Down, Vector3.Transform(Position, invRotationMatrix).Y);

                float intersection;
                if (ray.Intersects(ref plane, out intersection))
                {
                    _intersectPosition = ray.Position + ray.Direction * intersection;
                    if (_lastIntersectionPosition != Vector3.Zero)
                        _tDelta = _intersectPosition - _lastIntersectionPosition;
                    delta = new Vector3(_tDelta.X, 0, _tDelta.Z);
                }

                break;
            }

            case Axis.Center:
            {
                Vector3 gizmoToView = Position - Owner.ViewPosition;
                var plane = new Plane(-Vector3.Normalize(gizmoToView), gizmoToView.Length);

                float intersection;
                if (ray.Intersects(ref plane, out intersection))
                {
                    _intersectPosition = ray.Position + ray.Direction * intersection;
                    if (_lastIntersectionPosition != Vector3.Zero)
                        _tDelta = _intersectPosition - _lastIntersectionPosition;
                }

                delta = _tDelta;

                break;
            }
            }

            if (isScalling)
                delta *= 0.01f;

            if (Owner.IsAltKeyDown)
                delta *= 0.5f;

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
                    (int)(_translationScaleSnapDelta.X / snapValue) * snapValue,
                    (int)(_translationScaleSnapDelta.Y / snapValue) * snapValue,
                    (int)(_translationScaleSnapDelta.Z / snapValue) * snapValue);

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
                _scaleDelta = delta;
            }
        }

        private void UpdateRotate(float dt)
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

                Vector3 viewDir = Owner.ViewPosition - Position;
                Vector3.Dot(ref viewDir, ref dir, out float dot);
                if (dot < 0.0f)
                    delta *= -1;

                Quaternion.RotationAxis(ref dir, delta, out _rotationDelta);
                break;
            }

            default:
                _rotationDelta = Quaternion.Identity;
                break;
            }
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            LastDelta = Transform.Identity;

            if (!IsActive)
                return;

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
                        UpdateTranslateScale();
                        break;

                    case Mode.Rotate:
                        UpdateRotate(dt);
                        break;
                    }
                }
                else
                {
                    // If nothing selected, try to select any axis
                    if (!isLeftBtnDown && !Owner.IsRightMouseButtonDown)
                        SelectAxis();
                }

                // Set positions of the gizmo
                UpdateGizmoPosition();

                // Trigger Translation, Rotation & Scale events
                if (isLeftBtnDown)
                {
                    var anyValid = false;

                    // Translation
                    Vector3 translationDelta = Vector3.Zero;
                    if (_translationDelta.LengthSquared > 0.000001f)
                    {
                        anyValid = true;
                        translationDelta = _translationDelta;
                        _translationDelta = Vector3.Zero;

                        // Prevent from moving objects too far away, like to different galaxy or sth
                        Vector3 prevMoveDelta = _accMoveDelta;
                        _accMoveDelta += _translationDelta;
                        if (_accMoveDelta.Length > Owner.ViewFarPlane * 0.7f)
                            _accMoveDelta = prevMoveDelta;
                    }

                    // Rotation
                    Quaternion rotationDelta = Quaternion.Identity;
                    if (!_rotationDelta.IsIdentity)
                    {
                        anyValid = true;
                        rotationDelta = _rotationDelta;
                        _rotationDelta = Quaternion.Identity;
                    }

                    // Scale
                    Vector3 scaleDelta = Vector3.Zero;
                    if (_scaleDelta.LengthSquared > 0.000001f)
                    {
                        anyValid = true;
                        scaleDelta = _scaleDelta;
                        _scaleDelta = Vector3.Zero;

                        if (ActiveAxis == Axis.Center)
                            scaleDelta = new Vector3(scaleDelta.AvgValue);
                    }

                    // Apply transformation (but to the parents, not whole selection pool)
                    if (anyValid)
                    {
                        StartTransforming();

                        LastDelta = new Transform(translationDelta, rotationDelta, scaleDelta);
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
                UpdateGizmoPosition();

            // Activate
            _isActive = true;

            // Update
            UpdateMatricies();
        }

        /// <inheritdoc />
        public override void Pick()
        {
            // Ensure player is not moving objects
            if (ActiveAxis != Axis.None)
                return;

            // Get mouse ray and try to hit any object
            var ray = Owner.MouseRay;
            float closest = float.MaxValue;
            bool selectColliders = (Owner.RenderTask.Flags & ViewFlags.PhysicsDebug) == ViewFlags.PhysicsDebug;
            SceneGraphNode.RayCastData.FlagTypes rayCastFlags = SceneGraphNode.RayCastData.FlagTypes.None;
            if (!selectColliders)
                rayCastFlags |= SceneGraphNode.RayCastData.FlagTypes.SkipColliders;
            var hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest, rayCastFlags);

            // Update selection
            var sceneEditing = Editor.Instance.SceneEditing;
            if (hit != null)
            {
                // For child actor nodes (mesh, link or sth) we need to select it's owning actor node first or any other child node (but not a child actor)
                if (hit is ActorChildNode actorChildNode)
                {
                    var parentNode = actorChildNode.ParentNode;
                    bool canChildBeSelected = sceneEditing.Selection.Contains(parentNode);
                    if (!canChildBeSelected)
                    {
                        for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                        {
                            if (sceneEditing.Selection.Contains(parentNode.ChildNodes[i]))
                            {
                                canChildBeSelected = true;
                                break;
                            }
                        }
                    }

                    if (!canChildBeSelected)
                    {
                        // Select parent
                        hit = parentNode;
                    }
                }

                bool addRemove = Owner.IsControlDown;
                bool isSelected = sceneEditing.Selection.Contains(hit);

                if (addRemove)
                {
                    if (isSelected)
                        sceneEditing.Deselect(hit);
                    else
                        sceneEditing.Select(hit, true);
                }
                else
                {
                    sceneEditing.Select(hit);
                }
            }
            else
            {
                sceneEditing.Deselect();
            }
        }
    }
}
