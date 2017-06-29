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
        private readonly List<Actor> _selection = new List<Actor>();
        private readonly List<Actor> _selectionParents = new List<Actor>();
        
        private Matrix _screenScaleMatrix;
        private float _screenScale;

        private Vector3 _position;
        private Vector3 _accMoveDelta;
        private Matrix _rotationMatrix;

        private Vector3 _localForward;
        private Vector3 _localUp;
        private Vector3 _localRight;

        private Matrix _objectOrientedWorld;
        private Matrix _axisAlignedWorld;

        private Matrix _gizmoWorld;

        private Vector3 _translationDelta;
        private Matrix _rotationDelta;
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
    }
}
