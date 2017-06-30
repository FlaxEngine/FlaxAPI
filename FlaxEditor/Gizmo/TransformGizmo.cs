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
    }
}
