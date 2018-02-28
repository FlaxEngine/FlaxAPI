////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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

        private ContextMenuButton _showGridButton;
        private readonly ViewportWidgetButton _gizmoModeTranslate;
        private readonly ViewportWidgetButton _gizmoModeRotate;
        private readonly ViewportWidgetButton _gizmoModeScale;

	    private ViewportWidgetButton _translateSnappng;
	    private ViewportWidgetButton _rotateSnapping;
	    private ViewportWidgetButton _scaleSnapping;
		
		private readonly DragAssets _dragAssets = new DragAssets();
        private readonly DragActorType _dragActorType = new DragActorType();
        private readonly ViewportDebugDrawData _debugDrawData = new ViewportDebugDrawData(32);

        /// <summary>
        /// The transform gizmo.
        /// </summary>
        public readonly TransformGizmo TransformGizmo;

        /// <summary>
        /// The grid gizmo.
        /// </summary>
        public readonly GridGizmo Grid;

        /// <summary>
        /// The selection outline postFx.
        /// </summary>
        public SelectionOutline SelectionOutline;

        /// <summary>
        /// The editor primitives postFx.
        /// </summary>
        public EditorPrimitives EditorPrimitives;

        internal ViewportDebugDrawData DebugDrawData => _debugDrawData;

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
            Task.Draw += RenderTaskOnDraw;

            // Create post effects
            SelectionOutline = FlaxEngine.Object.New<SelectionOutline>();
            Task.CustomPostFx.Add(SelectionOutline);
            EditorPrimitives = FlaxEngine.Object.New<EditorPrimitives>();
            Task.CustomPostFx.Add(EditorPrimitives);

            // Add transformation gizmo
            TransformGizmo = new TransformGizmo(this);
            TransformGizmo.OnApplyTransformation += ApplyTransform;
            TransformGizmo.OnModeChanged += OnGizmoModeChanged;
            Gizmos.Active = TransformGizmo;

            // Add grid
            Grid = new GridGizmo(this);
            Grid.EnabledChanged += gizmo => _showGridButton.Icon = gizmo.Enabled ? Style.Current.CheckBoxTick : Sprite.Invalid;

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
            _scaleSnapping = new ViewportWidgetButton(TransformGizmo.ScaleSnapValue.ToString(), Sprite.Invalid, scaleSnappingCM);
	        _scaleSnapping.TooltipText = "Scale snapping values";
            for (int i = 0; i < EditorViewportScaleSnapValues.Length; i++)
            {
	            var v = EditorViewportScaleSnapValues[i];
				var button = scaleSnappingCM.AddButton(v.ToString());
	            button.Tag = v;
            }
            scaleSnappingCM.ButtonClicked += widgetScaleSnapClick;
            scaleSnappingCM.VisibleChanged += widgetScaleSnapShowHide;
	        _scaleSnapping.Parent = scaleSnappingWidget;
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
	        _rotateSnapping = new ViewportWidgetButton(TransformGizmo.RotationSnapValue.ToString(), Sprite.Invalid, rotateSnappingCM);
	        _rotateSnapping.TooltipText = "Rotation snapping values";
            for (int i = 0; i < EditorViewportRotateSnapValues.Length; i++)
            {
				var v = EditorViewportRotateSnapValues[i];
	            var button = rotateSnappingCM.AddButton(v.ToString());
	            button.Tag = v;
            }
            rotateSnappingCM.ButtonClicked += widgetRotateSnapClick;
            rotateSnappingCM.VisibleChanged += widgetRotateSnapShowHide;
	        _rotateSnapping.Parent = rotateSnappingWidget;
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
            _translateSnappng = new ViewportWidgetButton(TransformGizmo.TranslationSnapValue.ToString(), Sprite.Invalid, translateSnappingCM);
	        _translateSnappng.TooltipText = "Position snapping values";
            for (int i = 0; i < EditorViewportTranslateSnapValues.Length; i++)
            {
	            var v = EditorViewportTranslateSnapValues[i];
	            var button = translateSnappingCM.AddButton(v.ToString());
	            button.Tag = v;
			}
            translateSnappingCM.ButtonClicked += widgetTranslateSnapClick;
            translateSnappingCM.VisibleChanged += widgetTranslateSnapShowHide;
	        _translateSnappng.Parent = translateSnappingWidget;
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

            // Create Camera Here widget
            _showGridButton = ViewWidgetButtonMenu.AddButton("Show grid", () => Grid.Enabled = !Grid.Enabled);
            _showGridButton.Icon = Style.Current.CheckBoxTick;
            _showGridButton.IndexInParent = 1;
            ViewWidgetButtonMenu.AddSeparator();
            ViewWidgetButtonMenu.AddButton("Create camera here", CreateCameraAtView);
        }

        private void CreateCameraAtView()
        {
            if (!SceneManager.IsAnySceneLoaded)
                return;

            // Create actor
            var actor = Camera.New();
            actor.StaticFlags = StaticFlags.None;
            actor.Name = "Camera";
            actor.Transform = ViewTransform;
            actor.FieldOfView = _fieldOfView;

            // Spawn
            Editor.Instance.SceneEditing.Spawn(actor);
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

        private void widgetScaleSnapClick(ContextMenuButton button)
        {
	        var v = (float)button.Tag;
	        TransformGizmo.ScaleSnapValue = v;
	        _scaleSnapping.Text = v.ToString();
		}

        private void widgetScaleSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu)control;
			foreach (var e in ccm.Items)
			{
				if (e is ContextMenuButton b)
				{
					var v = (float)b.Tag;
					b.Icon = Mathf.Abs(TransformGizmo.ScaleSnapValue - v) < 0.001f
						? Style.Current.CheckBoxTick
						: Sprite.Invalid;
				}
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

        private void widgetRotateSnapClick(ContextMenuButton button)
        {
	        var v = (float)button.Tag;
	        TransformGizmo.RotationSnapValue = v;
	        _rotateSnapping.Text = v.ToString();
		}

        private void widgetRotateSnapShowHide(Control control)
        {
            if (control.Visible == false)
                return;

            var ccm = (ContextMenu)control;
			foreach (var e in ccm.Items)
			{
				if (e is ContextMenuButton b)
				{
					var v = (float)b.Tag;
					b.Icon = Mathf.Abs(TransformGizmo.RotationSnapValue - v) < 0.001f
						? Style.Current.CheckBoxTick
						: Sprite.Invalid;
				}
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

        private void widgetTranslateSnapClick(ContextMenuButton button)
        {
	        var v = (float)button.Tag;
			TransformGizmo.TranslationSnapValue = v;
	        _translateSnappng.Text = v.ToString();
        }

	    private void widgetTranslateSnapShowHide(Control control)
	    {
		    if (control.Visible == false)
			    return;

		    var ccm = (ContextMenu)control;
		    foreach (var e in ccm.Items)
		    {
			    if (e is ContextMenuButton b)
			    {
				    var v = (float)b.Tag;
				    b.Icon = Mathf.Abs(TransformGizmo.TranslationSnapValue - v) < 0.001f
					    ? Style.Current.CheckBoxTick
					    : Sprite.Invalid;
			    }
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
			    if (isPlayMode && obj.CanTransform == false)
				    continue;
			    var trans = obj.Transform;

			    // Apply rotation
			    if (applyRotation)
			    {
				    Vector3 pivotOffset = trans.Translation - gizmoPosition;
				    if (useObjCenter || pivotOffset.IsZero)
				    {
					    //trans.Orientation *= rotationDelta;
					    trans.Orientation *= Quaternion.Invert(trans.Orientation) * rotationDelta * trans.Orientation;
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

                bool addRemove = ParentWindow.GetKey(Keys.Control);
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
        public override bool OnKeyDown(Keys key)
        {
            if (key == Keys.Delete)
            {
                _editor.SceneEditing.Delete();
                return true;
            }
            if (key == Keys.Alpha1)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Translate;
                return true;
            }
            if (key == Keys.Alpha2)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Rotate;
                return true;
            }
            if (key == Keys.Alpha3)
            {
                TransformGizmo.ActiveMode = TransformGizmo.Mode.Scale;
                return true;
            }
            if (key == Keys.F)
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
            if (_dragActorType.OnDragEnter(data, ValidateDragActorType))
                result = _dragActorType.Effect;

            return result;
        }

        private bool ValidateDragItem(ContentItem contentItem)
        {
            switch (contentItem.ItemDomain)
            {
                case ContentDomain.Material:
                case ContentDomain.Model:
                case ContentDomain.Audio:
                case ContentDomain.Prefab: return SceneManager.IsAnySceneLoaded;
                case ContentDomain.Scene: return true;
                default: return false;
            }
        }

        private bool ValidateDragActorType(Type actorType)
        {
            return SceneManager.IsAnySceneLoaded;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (_dragAssets.HasValidDrag)
                return _dragAssets.Effect;
            if (_dragActorType.HasValidDrag)
                return _dragActorType.Effect;

            return DragDropEffect.None;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            _dragAssets.OnDragLeave();
            _dragActorType.OnDragLeave();

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            // Check if drag sth
            Vector3 hitLocation = ViewPosition;
            SceneGraphNode hit = null;
            if (_dragAssets.HasValidDrag || _dragActorType.HasValidDrag)
            {
                // Get mouse ray and try to hit any object
                var ray = ConvertMouseToRay(ref location);
                float closest = float.MaxValue;
                hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest);
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
            }

            // Drag assets
            if (_dragAssets.HasValidDrag)
            {
                result = _dragAssets.Effect;

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
                            else if (hit is BoxBrushNode.SideLinkNode brushSurfaceNode)
                            {
                                var material = FlaxEngine.Content.LoadAsync<MaterialBase>(item.ID);
                                using (new UndoBlock(Undo, brushSurfaceNode.Brush, "Change material"))
                                    brushSurfaceNode.Surface.Material = material;
                            }

                            break;
                        }
                        case ContentDomain.Model:
                        {
                            // Create actor
                            var model = FlaxEngine.Content.LoadAsync<Model>(item.ID);
                            var actor = ModelActor.New();
                            actor.Name = item.ShortName;
                            actor.Model = model;

                            // Place it
                            var box = actor.Box;
                            actor.Position = hitLocation - (box.Size.Length * 0.5f) * ViewDirection;

                            // Spawn
                            Editor.Instance.SceneEditing.Spawn(actor);

                            break;
                        }
	                    case ContentDomain.Audio:
	                    {
		                    var clip = FlaxEngine.Content.LoadAsync<AudioClip>(item.ID);
		                    var actor = AudioSource.New();
		                    actor.Name = item.ShortName;
		                    actor.Clip = clip;
		                    actor.Position = hitLocation;
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
            // Drag actor type
            else if (_dragActorType.HasValidDrag)
            {
                result = _dragActorType.Effect;

                // Process items
                for (int i = 0; i < _dragActorType.Objects.Count; i++)
                {
                    var item = _dragActorType.Objects[i];

                    // Create actor
                    var actor = FlaxEngine.Object.New(item) as Actor;
                    if (actor == null)
                    {
                        Editor.LogWarning("Failed to spawn actor of type " + item.FullName);
                        continue;
                    }
                    actor.Name = item.Name;

                    // Place it
                    var box = actor.Box;
                    actor.Position = hitLocation - (box.Size.Length * 0.5f) * ViewDirection;

                    // Spawn
                    Editor.Instance.SceneEditing.Spawn(actor);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _debugDrawData.Dispose();
            FlaxEngine.Object.Destroy(ref SelectionOutline);
            FlaxEngine.Object.Destroy(ref EditorPrimitives);

            base.OnDestroy();
        }

        private RenderTask _savedTask;
        private RenderTarget _savedBackBuffer;

        internal void SaveProjectIcon()
        {
            TakeScreenshot(StringUtils.CombinePaths(Globals.ProjectCacheFolder, "icon.png"));

            _savedTask = _task;
            _savedBackBuffer = _backBuffer;

            _task = null;
            _backBuffer = null;
        }

        internal void SaveProjectIconEnd()
        {
            if (_savedTask)
            {
                _savedTask.Dispose();
                _savedTask = null;
            }
            FlaxEngine.Object.Destroy(ref _savedBackBuffer);
        }
    }
}
