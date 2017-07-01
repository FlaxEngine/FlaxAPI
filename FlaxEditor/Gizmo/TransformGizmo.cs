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
        /// Initializes a new instance of the <see cref="TransformGizmo"/> class.
        /// </summary>
        /// <param name="owner">The gizmos owner.</param>
        public TransformGizmo(IGizmoOwner owner)
            : base(owner)
        {
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

            /*// End action
            auto ur = _parent->GetUndoRedo();
            if (!ur->IsDisabled())
            {
                ur->AddAction(new TransformActors(this, _startTransforms));
            }
            _startTransforms.Clear();
            _isTransforming = false;
            _isCloning = false;
            _parent->OnTransfomingEnd();
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
    }
}
