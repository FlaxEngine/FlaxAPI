////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Gizmo;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Main edior gizmo viewport used by the <see cref="EditGameWindow"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorGizmoViewport" />
    public class MainEditorGizmoViewport : EditorGizmoViewport
    {
        private Editor _editor;

        /// <summary>
        /// The transform gizmo.
        /// </summary>
        public readonly TransformGizmo TransformGizmo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainEditorGizmoViewport"/> class.
        /// </summary>
        /// <param name="editor">Editor instance.</param>
        public MainEditorGizmoViewport(Editor editor)
            : base(RenderTask.Create<SceneRenderTask>())
        {
            _editor = editor;

            Task.Flags = ViewFlags.DefaultEditor;
            TransformGizmo = new TransformGizmo(this);
            TransformGizmo.OnApplyTransformation += ApplyTransform;
            Gizmos.Active = TransformGizmo;

            editor.SceneEditing.OnSelectionChanged += OnOnSelectionChanged;
        }

        private void OnOnSelectionChanged()
        {
            var selection = _editor.SceneEditing.Selection;
            Gizmos.ForEach(x => x.OnSelectionChanged(selection));
        }

        public void ApplyTransform(List<ISceneTreeNode> selection, ref Vector3 translationDelta, ref Matrix rotationDelta, ref Vector3 scaleDelta)
        {
            // TODO: lock properties editor here

            bool useObjCenter = TransformGizmo.ActivePivot == TransformGizmo.PivotType.ObjectCenter;
            bool uniformScale = TransformGizmo.ActiveAxis == TransformGizmo.Axis.Center;
            Vector3 gizmoPosition = TransformGizmo.Position;

            // Transform selected objects
            for (int i = 0; i < selection.Count; i++)
            {
                var obj = selection[i];
                var trans = obj.Transform;

                // Apply translation
                trans.Translation += translationDelta;

                // Apply scale
                if (uniformScale)
                    trans.Scale *= scaleDelta;
                else
                    trans.Scale += scaleDelta;
                const float scaleLimit = 99_999_999.0f;
                trans.Scale = Vector3.Clamp(trans.Scale, new Vector3(-scaleLimit), new Vector3(scaleLimit));

                // Apply rotation
                Matrix localRot = Matrix.Identity;
                Vector3 rotationCenter = useObjCenter ? trans.Translation : gizmoPosition;
                localRot.Forward = trans.Forward;
                localRot.Up = trans.Up;
                localRot.Right = Vector3.Normalize(Vector3.Cross(trans.Forward, trans.Up));
                localRot.TranslationVector = trans.Translation - rotationCenter;
                Matrix newRot = localRot * rotationDelta;
                trans.SetRotation(ref newRot);
                if (newRot.TranslationVector.LengthSquared > 0.0001f)
                    trans.Translation = newRot.TranslationVector + rotationCenter;

                obj.Transform = trans;
            }

            // Fire event
            // TODO: mark scene as edited (all parent scenes of the selected objects)
        }

        /// <inheritdoc />
        protected override void OnLeftMouseButtonUp()
        {
            if (TransformGizmo.IsActive)
            {
                // Ensure player is not moving objects
                if (TransformGizmo.ActiveAxis != TransformGizmo.Axis.None)
                    return;
            }
            else
            {
                // For now just pick objects in transform gizmo mode
                return;
            }

            // Get mouse ray and try to hit any object
            var ray = MouseRay;
            float closest = float.MaxValue;
            var hit = Editor.Instance.Windows.SceneWin.Root.RayCast(ref ray, ref closest);

            if (hit != null)
            {
                bool addRemove = ParentWindow.GetKey(KeyCode.CONTROL);
                

            }

            base.OnLeftMouseButtonUp();
        }
    }
}
