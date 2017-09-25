////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Gizmo;
using FlaxEditor.GUI.Drag;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
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

        private readonly DragAssets _dragAssets = new DragAssets();
        private readonly ViewportDebugDrawData _debugDrawData = new ViewportDebugDrawData(32);

        /// <summary>
        /// The transform gizmo.
        /// </summary>
        public readonly TransformGizmo TransformGizmo;

        /// <summary>
        /// The selection outline postFx.
        /// </summary>
        public SelectionOutline SelectionOutline;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainEditorGizmoViewport"/> class.
        /// </summary>
        /// <param name="editor">Editor instance.</param>
        public MainEditorGizmoViewport(Editor editor)
            : base(RenderTask.Create<SceneRenderTask>(), editor.Undo)
        {
            _editor = editor;

            // Prepare rendering task
            Task.ActorsSource = ActorsSources.ScenesAndCustomActors;
            Task.Flags = ViewFlags.DefaultEditor;
            Task.Begin += RenderTaskOnBegin;
            Task.End += RenderTaskOnEnd;
            Task.Draw += RenderTaskOnDraw;

            // Create selection outline postFx
            SelectionOutline = FlaxEngine.Object.New<SelectionOutline>();
            Task.CustomPostFx.Add(SelectionOutline);

            // Add transformation gizmo
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
                TooltipText = "Gizmo transform space (world or local)",
                Parent = transformSpaceWidget
            };
            transformSpaceToggle.OnToggle += onTransformSpaceToggle;
            transformSpaceWidget.Parent = this;

            // Scale snapping widget
            var scaleSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableScaleSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("ScaleStep16"), null, true)
            {
                Checked = TransformGizmo.ScaleSnapEnabled,
                TooltipText = "Enable scale snapping",
                Parent = scaleSnappingWidget
            };
            enableScaleSnapping.OnToggle += onScaleSnappingToggle;
            var scaleSnappingCM = new ContextMenu();
            var scaleSnappng = new ViewportWidgetButton(TransformGizmo.ScaleSnapValue.ToString(), Sprite.Invalid, scaleSnappingCM);
            scaleSnappng.TooltipText = "Scale snapping values";
            for (int i = 0; i < EditorViewportScaleSnapValues.Length; i++)
            {
                var button = scaleSnappingCM.AddButton(i, EditorViewportScaleSnapValues[i].ToString());
                button.Tag = scaleSnappng;
            }
            scaleSnappingCM.OnButtonClicked += widgetScaleSnapClick;
            scaleSnappingCM.VisibleChanged += widgetScaleSnapShowHide;
            scaleSnappng.Parent = scaleSnappingWidget;
            scaleSnappingWidget.Parent = this;

            // Rotation snapping widget
            var rotateSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableRotateSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("RotateStep16"), null, true)
            {
                Checked = TransformGizmo.RotationSnapEnabled,
                TooltipText = "Enable rotation snapping",
                Parent = rotateSnappingWidget
            };
            enableRotateSnapping.OnToggle += onRotateSnappingToggle;
            var rotateSnappingCM = new ContextMenu();
            var rotateSnappng = new ViewportWidgetButton(TransformGizmo.RotationSnapValue.ToString(), Sprite.Invalid, rotateSnappingCM);
            rotateSnappng.TooltipText = "Rotation snapping values";
            for (int i = 0; i < EditorViewportRotateSnapValues.Length; i++)
            {
                var button = rotateSnappingCM.AddButton(i, EditorViewportRotateSnapValues[i].ToString());
                button.Tag = rotateSnappng;
            }
            rotateSnappingCM.OnButtonClicked += widgetRotateSnapClick;
            rotateSnappingCM.VisibleChanged += widgetRotateSnapShowHide;
            rotateSnappng.Parent = rotateSnappingWidget;
            rotateSnappingWidget.Parent = this;

            // Translation snapping widget
            var translateSnappingWidget = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            var enableTranslateSnapping = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Grid16"), null, true)
            {
                Checked = TransformGizmo.TranslationSnapEnable,
                TooltipText = "Enable position snapping",
                Parent = translateSnappingWidget
            };
            enableTranslateSnapping.OnToggle += onTranslateSnappingToggle;
            var translateSnappingCM = new ContextMenu();
            var translateSnappng = new ViewportWidgetButton(TransformGizmo.TranslationSnapValue.ToString(), Sprite.Invalid, translateSnappingCM);
            translateSnappng.TooltipText = "Position snapping values";
            for (int i = 0; i < EditorViewportTranslateSnapValues.Length; i++)
            {
                var button = translateSnappingCM.AddButton(i, EditorViewportTranslateSnapValues[i].ToString());
                button.Tag = translateSnappng;
            }
            translateSnappingCM.OnButtonClicked += widgetTranslateSnapClick;
            translateSnappingCM.VisibleChanged += widgetTranslateSnapShowHide;
            translateSnappng.Parent = translateSnappingWidget;
            translateSnappingWidget.Parent = this;

            // Gizmo mode widget
            var gizmoMode = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
            _gizmoModeTranslate = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Translate16"), null, true)
            {
                Tag = TransformGizmo.Mode.Translate,
                TooltipText = "Translate gizmo mode",
                Checked = true,
                Parent = gizmoMode
            };
            _gizmoModeTranslate.OnToggle += onGizmoModeToggle;
            _gizmoModeRotate = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Rotate16"), null, true)
            {
                Tag = TransformGizmo.Mode.Rotate,
                TooltipText = "Rotate gizmo mode",
                Parent = gizmoMode
            };
            _gizmoModeRotate.OnToggle += onGizmoModeToggle;
            _gizmoModeScale = new ViewportWidgetButton(string.Empty, editor.UI.GetIcon("Scale16"), null, true)
            {
                Tag = TransformGizmo.Mode.Scale,
                TooltipText = "Scale gizmo mode",
                Parent = gizmoMode
            };
            _gizmoModeScale.OnToggle += onGizmoModeToggle;
            gizmoMode.Parent = this;
        }

        private void RenderTaskOnBegin(SceneRenderTask task)
        {
            _debugDrawData.Clear();

            // Collect selected objects debug shapes and visuals
            var selectedParents = TransformGizmo.SelectedParents;
            if (selectedParents.Count > 0)
            {
                for (int i = 0; i < selectedParents.Count; i++)
                {
                    if (selectedParents[i].IsActiveInHierarchy)
                        selectedParents[i].OnDebugDraw(_debugDrawData);
                }
            }
        }

        private void RenderTaskOnEnd(SceneRenderTask task)
        {
            // Draw selected objects debug shapes and visuals
            DebugDraw.Draw(task, _debugDrawData.ActorsPtrs);
        }

        private void RenderTaskOnDraw(DrawCallsCollector collector)
        {
            _debugDrawData.OnDraw(collector);
        }

        private void onGizmoModeToggle(ViewportWidgetButton button)
        {
            TransformGizmo.ActiveMode = (TransformGizmo.Mode)(int)button.Tag;
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
            var button = (ViewportWidgetButton)contextMenu.GetButton(id).Tag;
            TransformGizmo.ScaleSnapValue = EditorViewportScaleSnapValues[id];
            button.Text = EditorViewportScaleSnapValues[id].ToString();
        }

        private void widgetScaleSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu)control;
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
            var button = (ViewportWidgetButton)contextMenu.GetButton(id).Tag;
            TransformGizmo.RotationSnapValue = EditorViewportRotateSnapValues[id];
            button.Text = EditorViewportRotateSnapValues[id].ToString();
        }

        private void widgetRotateSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu)control;
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
            var button = (ViewportWidgetButton)contextMenu.GetButton(id).Tag;
            TransformGizmo.TranslationSnapValue = EditorViewportTranslateSnapValues[id];
            button.Text = EditorViewportTranslateSnapValues[id].ToString();
        }

        private void widgetTranslateSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu)control;
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

        /// <summary>
        /// Applies the transform to the collection of scene graph nodes.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="translationDelta">The translation delta.</param>
        /// <param name="rotationDelta">The rotation delta.</param>
        /// <param name="scaleDelta">The scale delta.</param>
        public void ApplyTransform(List<SceneGraphNode> selection, ref Vector3 translationDelta, ref Quaternion rotationDelta, ref Vector3 scaleDelta)
        {
            bool applyRotation = !rotationDelta.IsIdentity;
            bool useObjCenter = TransformGizmo.ActivePivot == TransformGizmo.PivotType.ObjectCenter;
            Vector3 gizmoPosition = TransformGizmo.Position;

            // Transform selected objects
            bool isPlayMode = Editor.Instance.StateMachine.IsPlayMode;
            for (int i = 0; i < selection.Count; i++)
            {
                var obj = selection[i];
                
                // Block transforming static objects in play mode
                if(isPlayMode && obj.CanTransform == false)
                    continue;
                var trans = obj.Transform;

                // Apply rotation
                if (applyRotation)
                {
                    Vector3 pivotOffset = trans.Translation - gizmoPosition;
                    if (useObjCenter || pivotOffset.IsZero)
                    {
                        trans.Orientation *= rotationDelta;
                    }
                    else
                    {
                        Matrix.RotationQuaternion(ref trans.Orientation, out var transWorld);
                        Matrix.RotationQuaternion(ref rotationDelta, out var deltaWorld);
                        Matrix world = transWorld * Matrix.Translation(pivotOffset) * deltaWorld * Matrix.Translation(-pivotOffset);
                        trans.SetRotation(ref world);
                        trans.Translation += world.TranslationVector;
                    }
                }

                // Apply scale
                const float scaleLimit = 99_999_999.0f;
                trans.Scale = Vector3.Clamp(trans.Scale + scaleDelta, new Vector3(-scaleLimit), new Vector3(scaleLimit));

                // Apply translation
                trans.Translation += translationDelta;

                obj.Transform = trans;
            }
        }

        /// <inheritdoc />
        protected override void OnLeftMouseButtonUp()
        {
            // Skip if was controlling mouse or mouse is not over the area
            if (_prevInput.IsControllingMouse || !Bounds.Contains(ref _viewMousePos))
                return;

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
            var hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest);

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

                bool addRemove = ParentWindow.GetKey(KeyCode.Control);
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

            // Keep focus
            Focus();

            base.OnLeftMouseButtonUp();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            if (key == KeyCode.Delete)
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

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            var result = base.OnDragEnter(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (_dragAssets.OnDragEnter(data, ValidateDragItem))
                result = _dragAssets.Effect;

            return result;
        }

        private bool ValidateDragItem(ContentItem contentItem)
        {
            switch (contentItem.ItemDomain)
            {
                case ContentDomain.Material:
                case ContentDomain.Model:
                case ContentDomain.Prefab:
                case ContentDomain.Scene: return true;
                default: return false;
            }
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);
            if (result != DragDropEffect.None)
                return result;
            
            return _dragAssets.Effect;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            _dragAssets.OnDragLeave();

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (_dragAssets.HasValidDrag)
            {
                result = _dragAssets.Effect;

                // Get mouse ray and try to hit any object
                var ray = ConvertMouseToRay(ref location);
                float closest = float.MaxValue;
                var hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest);
                Vector3 hitLocation;
                if (hit != null)
                {
                    // Use hit location
                    hitLocation = ray.Position + ray.Direction * closest;
                }
                else
                {
                    // Use area in front of the viewport
                    hitLocation = ViewPosition + ViewDirection * 10;
                }

                // Process items
                for (int i = 0; i < _dragAssets.Objects.Count; i++)
                {
                    var item = _dragAssets.Objects[i];

                    switch (item.ItemDomain)
                    {
                        case ContentDomain.Material:
                        {
                            if (hit is ModelActorNode.EntryNode meshNode)
                            {
                                var material = FlaxEngine.Content.LoadAsync<MaterialBase>(item.ID);
                                using (new UndoBlock(Undo, meshNode.ModelActor, "Change material"))
                                    meshNode.Entry.Material = material;
                            }

                            break;
                        }
                        case ContentDomain.Model:
                        {
                            // Create actor
                            var model = FlaxEngine.Content.LoadAsync<Model>(item.ID);
                            var actor = ModelActor.New();
                            actor.StaticFlags = StaticFlags.FullyStatic;
                            actor.Name = item.ShortName;
                            actor.Model = model;

                            // Place it
                            var box = actor.Box;
                            actor.Position = hitLocation - (box.Size.Length * 0.5f) * ViewDirection;

                            // Spawn
                            Editor.Instance.SceneEditing.Spawn(actor);
                            
                            break;
                        }
                        case ContentDomain.Prefab:
                        {
                            throw new NotImplementedException("Spawning prefabs");
                        }
                        case ContentDomain.Scene:
                        {
                            Editor.Instance.Scene.OpenScene(item.ID, true);
                            break;
                        }
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _debugDrawData.Dispose();
            FlaxEngine.Object.Destroy(ref SelectionOutline);

            base.OnDestroy();
        }
    }
}
