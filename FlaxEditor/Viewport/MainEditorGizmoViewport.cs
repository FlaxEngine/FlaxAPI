////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Gizmo;
using FlaxEditor.SceneGraph;
using FlaxEditor.Viewport.Widgets;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Main edior gizmo viewport used by the <see cref="EditGameWindow"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorGizmoViewport" />
    public class MainEditorGizmoViewport : EditorGizmoViewport
    {
        private readonly Editor _editor;

        private readonly ViewportWidgetButton _gizmoModeTranslate;
        private readonly ViewportWidgetButton _gizmoModeRotate;
        private readonly ViewportWidgetButton _gizmoModeScale;

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
            Task.OnEnd += task =>
            {
                IntPtr[] selectedActors = null;
                var selectedParents = TransformGizmo.SelectedParents;
                if (selectedParents.Count > 0)
                {
                    var actors = new List<IntPtr>(selectedParents.Count);
                    for (int i = 0; i < selectedParents.Count; i++)
                    {
                        if (selectedParents[i] is ActorNode actor)
                            actors.Add(FlaxEngine.Object.GetUnmanagedPtr(actor.Actor));
                    }
                    if (actors.Count > 0)
                        selectedActors = actors.ToArray();
                }
                DebugDraw.Draw(task, selectedActors);
            };

            TransformGizmo = new TransformGizmo(this);
            TransformGizmo.OnApplyTransformation += ApplyTransform;
            TransformGizmo.OnModeChanged += OnGizmoModeChanged;
            Gizmos.Active = TransformGizmo;

            editor.SceneEditing.OnSelectionChanged += OnSelectionChanged;

            // Transform space widget
            var transformSpaceWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var transformSpaceToggle = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("World16"), null, true)
            {
                Checked = TransformGizmo.ActiveTransformSpace == TransformGizmo.TransformSpace.World,
                Parent = transformSpaceWidget
            };
            transformSpaceToggle.OnToggle += onTransformSpaceToggle;
            transformSpaceWidget.Parent = this;

            // Scale snapping widget
            var scaleSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableScaleSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("ScaleStep16"), null, true)
            {
                Checked = TransformGizmo.ScaleSnapEnabled,
                Parent = scaleSnappingWidget
            };
            enableScaleSnapping.OnToggle += onScaleSnappingToggle;
            var scaleSnappingCM = new ContextMenu();
            var scaleSnappng = new ViewportWidgetButton(TransformGizmo.ScaleSnapValue.ToString(), Sprite.Invalid,
                scaleSnappingCM);
            for (int i = 0; i < EditorViewportScaleSnapValues.Length; i++)
            {
                var button = scaleSnappingCM.AddButton(i, EditorViewportScaleSnapValues[i].ToString());
                button.Tag = scaleSnappng;
            }
            scaleSnappingCM.OnButtonClicked += widgetScaleSnapClick;
            scaleSnappingCM.OnVisibleChanged += widgetScaleSnapShowHide;
            scaleSnappng.Parent = scaleSnappingWidget;
            scaleSnappingWidget.Parent = this;

            // Rotation snapping widget
            var rotateSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableRotateSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("RotateStep16"), null, true)
            {
                Checked = TransformGizmo.RotationSnapEnabled,
                Parent = rotateSnappingWidget
            };
            enableRotateSnapping.OnToggle += onRotateSnappingToggle;
            var rotateSnappingCM = new ContextMenu();
            var rotateSnappng = new ViewportWidgetButton(TransformGizmo.RotationSnapValue.ToString(), Sprite.Invalid, rotateSnappingCM);
            for (int i = 0; i < EditorViewportRotateSnapValues.Length; i++)
            {
                var button = rotateSnappingCM.AddButton(i, EditorViewportRotateSnapValues[i].ToString());
                button.Tag = rotateSnappng;
            }
            rotateSnappingCM.OnButtonClicked += widgetRotateSnapClick;
            rotateSnappingCM.OnVisibleChanged += widgetRotateSnapShowHide;
            rotateSnappng.Parent = rotateSnappingWidget;
            rotateSnappingWidget.Parent = this;

            // Translation snapping widget
            var translateSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableTranslateSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Grid16"), null, true)
            {
                Checked = TransformGizmo.TranslationSnapEnable,
                Parent = translateSnappingWidget
            };
            enableTranslateSnapping.OnToggle += onTranslateSnappingToggle;
            var translateSnappingCM = new ContextMenu();
            var translateSnappng = new ViewportWidgetButton(TransformGizmo.TranslationSnapValue.ToString(), Sprite.Invalid, translateSnappingCM);
            for (int i = 0; i < EditorViewportTranslateSnapValues.Length; i++)
            {
                var button = translateSnappingCM.AddButton(i, EditorViewportTranslateSnapValues[i].ToString());
                button.Tag = translateSnappng;
            }
            translateSnappingCM.OnButtonClicked += widgetTranslateSnapClick;
            translateSnappingCM.OnVisibleChanged += widgetTranslateSnapShowHide;
            translateSnappng.Parent = translateSnappingWidget;
            translateSnappingWidget.Parent = this;

            // Gizmo mode widget
            var gizmoMode = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            _gizmoModeTranslate = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Translate16"), null, true)
            {
                Tag = TransformGizmo.Mode.Translate,
                Checked = true,
                Parent = gizmoMode
            };
            _gizmoModeTranslate.OnToggle += onGizmoModeToggle;
            _gizmoModeRotate = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Rotate16"), null, true)
            {
                Tag = TransformGizmo.Mode.Rotate,
                Parent = gizmoMode
            };
            _gizmoModeRotate.OnToggle += onGizmoModeToggle;
            _gizmoModeScale = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Scale16"), null, true)
            {
                Tag = TransformGizmo.Mode.Scale,
                Parent = gizmoMode
            };
            _gizmoModeScale.OnToggle += onGizmoModeToggle;
            gizmoMode.Parent = this;
        }

        private void onGizmoModeToggle(ViewportWidgetButton button)
        {
            TransformGizmo.ActiveMode = (TransformGizmo.Mode) (int) button.Tag;
        }

        private void onTranslateSnappingToggle(ViewportWidgetButton button)
        {
            TransformGizmo.TranslationSnapEnable = !TransformGizmo.TranslationSnapEnable;
        }

        private void onRotateSnappingToggle(ViewportWidgetButton button)
        {
            TransformGizmo.RotationSnapEnabled = !TransformGizmo.RotationSnapEnabled;
        }

        private void onScaleSnappingToggle(ViewportWidgetButton button)
        {
            TransformGizmo.ScaleSnapEnabled = !TransformGizmo.ScaleSnapEnabled;
        }

        private void onTransformSpaceToggle(ViewportWidgetButton button)
        {
            TransformGizmo.ToggleTransformSpace();
        }

        private void OnGizmoModeChanged()
        {
            // Update all viewport widgets status
            var mode = TransformGizmo.ActiveMode;
            _gizmoModeTranslate.Checked = mode == TransformGizmo.Mode.Translate;
            _gizmoModeRotate.Checked = mode == TransformGizmo.Mode.Rotate;
            _gizmoModeScale.Checked = mode == TransformGizmo.Mode.Scale;
        }

        private static readonly float[] EditorViewportScaleSnapValues =
        {
            0.1f,
            0.25f,
            0.5f,
            1.0f,
            2.0f,
            4.0f,
            6.0f,
            8.0f,
        };

        private void widgetScaleSnapClick(int id, ContextMenu contextMenu)
        {
            var button = (ViewportWidgetButton) contextMenu.GetButton(id).Tag;
            TransformGizmo.ScaleSnapValue = EditorViewportScaleSnapValues[id];
            button.Text = EditorViewportScaleSnapValues[id].ToString();
        }

        private void widgetScaleSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu) control;
            for (int i = 0; i < EditorViewportScaleSnapValues.Length; i++)
            {
                ccm.GetButton(i).Icon = Mathf.Abs(TransformGizmo.ScaleSnapValue - EditorViewportScaleSnapValues[i]) < 0.001f
                    ? Style.Current.CheckBoxTick
                    : Sprite.Invalid;
            }
        }

        private static readonly float[] EditorViewportRotateSnapValues =
        {
            1.0f,
            5.0f,
            10.0f,
            15.0f,
            30.0f,
            45.0f,
            60.0f,
            90.0f,
        };

        private void widgetRotateSnapClick(int id, ContextMenu contextMenu)
        {
            var button = (ViewportWidgetButton) contextMenu.GetButton(id).Tag;
            TransformGizmo.RotationSnapValue = EditorViewportRotateSnapValues[id];
            button.Text = EditorViewportRotateSnapValues[id].ToString();
        }

        private void widgetRotateSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu) control;
            for (int i = 0; i < EditorViewportRotateSnapValues.Length; i++)
            {
                ccm.GetButton(i).Icon = Mathf.Abs(TransformGizmo.RotationSnapValue - EditorViewportRotateSnapValues[i]) < 0.001f
                    ? Style.Current.CheckBoxTick
                    : Sprite.Invalid;
            }
        }

        private static readonly float[] EditorViewportTranslateSnapValues =
        {
            0.1f,
            0.5f,
            1.0f,
            5.0f,
            10.0f,
            100.0f,
            1000.0f,
        };

        private void widgetTranslateSnapClick(int id, ContextMenu contextMenu)
        {
            var button = (ViewportWidgetButton) contextMenu.GetButton(id).Tag;
            TransformGizmo.TranslationSnapValue = EditorViewportTranslateSnapValues[id];
            button.Text = EditorViewportTranslateSnapValues[id].ToString();
        }

        private void widgetTranslateSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu) control;
            for (int i = 0; i < EditorViewportTranslateSnapValues.Length; i++)
            {
                ccm.GetButton(i).Icon = Mathf.Abs(TransformGizmo.TranslationSnapValue - EditorViewportTranslateSnapValues[i]) < 0.001f
                    ? Style.Current.CheckBoxTick
                    : Sprite.Invalid;
            }
        }

        private void OnSelectionChanged()
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
                if (!rotationDelta.IsIdentity)
                {
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
                }

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

            // Update selection
            var sceneEditing = Editor.Instance.SceneEditing;
            if (hit != null)
            {
                bool addRemove = ParentWindow.GetKey(KeyCode.CONTROL);
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

            base.OnLeftMouseButtonUp();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            if (key == KeyCode.DELETE)
            {
                _editor.SceneEditing.Delete();
                return true;
            }
            if (key == KeyCode.Alpha1)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Translate;
                return true;
            }
            if (key == KeyCode.Alpha2)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate;
                return true;
            }
            if (key == KeyCode.Alpha3)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Scale;
                return true;
            }
            if (key == KeyCode.F)
            {
                _editor.Windows.EditWin.ShowSelectedActors();
                return true;
            }
            if (ParentWindow.GetKey(KeyCode.CONTROL))
            {
                switch (key)
                {
                    case KeyCode.X:
                        _editor.SceneEditing.Cut();
                        return true;
                    case KeyCode.C:
                        _editor.SceneEditing.Copy();
                        return true;
                    case KeyCode.V:
                        _editor.SceneEditing.Paste();
                        return true;
                    case KeyCode.D:
                        _editor.SceneEditing.Duplicate();
                        return true;
                    case KeyCode.F:
                        _editor.Windows.SceneWin.Search();
                        return true;
                    case KeyCode.A:
                        _editor.SceneEditing.SelectAllScenes();
                        return true;
                }
            }

            return base.OnKeyDown(key);
        }
    }
}
