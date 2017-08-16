////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor.Gizmo
{
    public partial class TransformGizmo
    {
        /// <summary>
        /// Gets the array of selected parent objects (as actors).
        /// </summary>
        /// <value>
        /// The selected parents.
        /// </value>
        public List<SceneGraphNode> SelectedParents => _selectionParents;

        /// <summary>
        /// Gets the selection center point (in world space).
        /// </summary>
        /// <returns>Center point or <see cref="Vector3.Zero"/> if no object selectd.</returns>
        public Vector3 GetSelectionCenter()
        {
            int count = _selectionParents.Count;

            // Check if there is no objects selected at all
            if (count == 0)
                return Vector3.Zero;

            // Get center point
            Vector3 center = Vector3.Zero;
            for (int i = 0; i < count; i++)
                center += _selectionParents[i].Transform.Translation;

            // Return arithmetic average or whatever it means
            return center / count;
        }

        /// <inheritdoc />
        public override void OnSelectionChanged(List<SceneGraphNode> newSelection)
        {
            // End current action
            EndTransforming();

            // Prepare collections
            _selection.Clear();
            _selectionParents.Clear();
            int count = newSelection.Count;
            if (_selection.Capacity < count)
            {
                _selection.Capacity = Mathf.NextPowerOfTwo(count);
                _selectionParents.Capacity = Mathf.NextPowerOfTwo(count);
            }

            // Cache selected objects
            _selection.AddRange(newSelection);

            // Build selected objects parents list.
            // Note: because selection may contain objects and their children we have to split them and get only parents.
            // Later during transformation we apply translation/scale/rotation only on them (children inherit transformations)
            SceneGraphTools.BuildNodesParents(_selection, _selectionParents);
        }

        private void SelectAxis()
        {
            // Get mouse ray
            Ray ray = Owner.MouseRay;

            // Find gizmo collisions with mouse
            float closestintersection = float.MaxValue;
            float intersection;
            _activeAxis = Axis.None;
            switch (_activeMode)
            {
                case Mode.Translate:
                {
                    // Transform ray into local space of the bounding boxes
                    Matrix invGizmoWorld;
                    Matrix.Invert(ref _gizmoWorld, out invGizmoWorld);
                    Vector3.TransformNormal(ref ray.Direction, ref invGizmoWorld, out ray.Direction);
                    Vector3.Transform(ref ray.Position, ref invGizmoWorld, out ray.Position);

                    // Axis boxes collision
                    if (XAxisBox.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.X;
                        closestintersection = intersection;
                    }
                    if (YAxisBox.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Y;
                        closestintersection = intersection;
                    }
                    if (ZAxisBox.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Z;
                        closestintersection = intersection;
                    }

                    // Quad planes collision
                    if (closestintersection >= float.MaxValue)
                        closestintersection = float.MinValue;
                    if (XYBox.Intersects(ref ray, out intersection) && intersection > closestintersection)
                    {
                        _activeAxis = Axis.XY;
                        closestintersection = intersection;
                    }
                    if (XZBox.Intersects(ref ray, out intersection) && intersection > closestintersection)
                    {
                        _activeAxis = Axis.ZX;
                        closestintersection = intersection;
                    }
                    if (YZBox.Intersects(ref ray, out intersection) && intersection > closestintersection)
                    {
                        _activeAxis = Axis.YZ;
                        closestintersection = intersection;
                    }

                    break;
                }

                case Mode.Rotate:
                {
                    // Spheres collision
                    if (RotateXSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.X;
                        closestintersection = intersection;
                    }
                    if (RotateYSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Y;
                        closestintersection = intersection;
                    }
                    if (RotateZSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Z;
                        closestintersection = intersection;
                    }

                    break;
                }

                case Mode.Scale:
                {
                    // Spheres collision
                    if (ScaleXSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.X;
                        closestintersection = intersection;
                    }
                    if (ScaleYSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Y;
                        closestintersection = intersection;
                    }
                    if (ScaleZSphere.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Z;
                        closestintersection = intersection;
                    }

                    // Center
                    if (CenterBox.Intersects(ref ray, out intersection) && intersection < closestintersection)
                    {
                        _activeAxis = Axis.Center;
                        closestintersection = intersection;
                    }

                    break;
                }
            }
        }
    }
}
