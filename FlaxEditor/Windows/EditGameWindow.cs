////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.States;
using FlaxEditor.Viewport;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Main editor window used to modify scene objects. Provides Gizmos and camera viewport navigation.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.SceneEditorWindow" />
    public sealed class EditGameWindow : SceneEditorWindow
    {
        /// <summary>
        /// Camera preview output control.
        /// </summary>
        public class CameraPreview : RenderOutputControl
        {
            private bool _isPinned;
            private Button _pinButton;

            /// <summary>
            /// Gets or sets a value indicating whether this preview is pinned.
            /// </summary>
            public bool IsPinned
            {
                get => _isPinned;
                set
                {
                    if (_isPinned != value)
                    {
                        _isPinned = value;
                        UpdatePinButton();
                    }
                }
            }

            /// <summary>
            /// Gets or sets the camera.
            /// </summary>
            public Camera Camera
            {
                get => Task.Camera;
                set => Task.Camera = value;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CameraPreview"/> class.
            /// </summary>
            public CameraPreview()
                : base(RenderTask.Create<SceneRenderTask>())
            {
                const float PinSize = 12.0f;
                const float PinMargin = 2.0f;

                _pinButton = new Button(Width - PinSize - PinMargin, PinMargin)
                {
                    Size = new Vector2(PinSize),
                    AnchorStyle = AnchorStyle.UpperRight,
                    Parent = this
                };
                _pinButton.Clicked += () => IsPinned = !IsPinned;
                UpdatePinButton();
            }

            private void UpdatePinButton()
            {
                if (_isPinned)
                {
                    _pinButton.Text = "-";
                    _pinButton.TooltipText = "Unpin preview";
                }
                else
                {
                    _pinButton.Text = "+";
                    _pinButton.TooltipText = "Pin preview";
                }
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                // Draw frame
                Render2D.DrawRectangle(new Rectangle(Vector2.Zero, Size), Color.Black);
            }

            /// <inheritdoc />
            public override void OnDestroy()
            {
                IsPinned = false;
                Camera = null;

                base.OnDestroy();
            }
        }

        private readonly List<CameraPreview> _previews = new List<CameraPreview>();

        /// <summary>
        /// The viewport control.
        /// </summary>
        public readonly MainEditorGizmoViewport Viewport;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditGameWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public EditGameWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Editor";

            // Create viewport
            Viewport = new MainEditorGizmoViewport(editor);
            Viewport.Parent = this;
            Viewport.Task.Flags = ViewFlags.DefaultEditor;

            Editor.Scene.ActorRemoved += SceneOnActorRemoved;
            AddCommandsToController();
        }

        private void SceneOnActorRemoved(ActorNode actorNode)
        {
            if (actorNode is CameraNode cameraNode)
            {
                // Remove previews using this camera
                HideCameraPreview((Camera)cameraNode.Actor);
            }
        }

        /// <summary>
        /// Moves the viewport to visualize the actor.
        /// </summary>
        /// <param name="actor">The actor to preview.</param>
        public void ShowActor(Actor actor)
        {
            BoundingSphere sphere;
            GetActorSphere(actor, out sphere);
            ShowSphere(ref sphere);
        }

        /// <summary>
        /// Moves the viewport to visualize selected actors.
        /// </summary>
        public void ShowSelectedActors()
        {
            var selection = Viewport.TransformGizmo.SelectedParents;
            if (selection.Count == 0)
                return;

            BoundingSphere mergesSphere = BoundingSphere.Empty;
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i] is ActorNode actor)
                {
                    BoundingSphere sphere;
                    GetActorSphere(actor.Actor, out sphere);
                    BoundingSphere.Merge(ref mergesSphere, ref sphere, out mergesSphere);
                }
            }
            ShowSphere(ref mergesSphere);
        }

        private void GetActorSphere(Actor actor, out BoundingSphere sphere)
        {
            BoundingBox box = actor.BoxWithChildren;
            BoundingSphere.FromBox(ref box, out sphere);
            sphere.Radius = Math.Max(sphere.Radius, 15.0f);
        }

        private void ShowSphere(ref BoundingSphere sphere)
        {
            // Calculate view transform
            Quaternion orientation = new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f);
            Vector3 position = sphere.Center - Vector3.ForwardLH * orientation * (sphere.Radius * 2.5f);

            // Move vieport
            Viewport.TargetPoint = sphere.Center;
            Viewport.MoveViewport(position, orientation);
        }

        /// <summary>
        /// Updates the camera previews.
        /// </summary>
        private void UpdateCameraPreview()
        {
            // Disable rendering preview during GI baking
            if (Editor.StateMachine.CurrentState is BuildingLightingState)
            {
                HideAllCameraPreviews();
                return;
            }

            var selection = Editor.SceneEditing.Selection;

            // Hide unpined previews for which camera being previews is not selected
            for (int i = 0; i < _previews.Count; i++)
            {
                if (_previews[i].IsPinned)
                    continue;
                var camera = _previews[i].Camera;
                var cameraNode = Editor.Scene.GetActorNode(camera);
                if (cameraNode == null || !selection.Contains(cameraNode))
                {
                    // Hide it
                    HideCameraPreview(_previews[i--]);
                }
            }

            // Find any selected cameras and create pewviews for them
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i] is CameraNode cameraNode)
                {
                    // Check limit for cameras
                    if (_previews.Count >= 8)
                        break;

                    var camera = (Camera)cameraNode.Actor;
                    var preview = _previews.FirstOrDefault(x => x.Camera == camera);
                    if (preview == null)
                    {
                        // Show it
                        preview = new CameraPreview
                        {
                            Camera = camera,
                            Parent = this
                        };
                        _previews.Add(preview);
                    }
                }
            }

            // Update previews locations
            int count = _previews.Count;
            if (count > 0)
            {
                // Update view dimensions and check if we can show it
                const float aspectRatio = 16.0f / 9.0f;
                const float minHeight = 20;
                const float minWidth = minHeight * aspectRatio;
                const float maxHeight = 150;
                const float maxWidth = maxHeight * aspectRatio;
                const float viewSpaceMaxPercentage = 0.7f;
                const float margin = 10;
                Vector2 totalSize = Size * viewSpaceMaxPercentage - margin;
                Vector2 singleSize = totalSize / count - count * margin;
                float sizeX = Mathf.Clamp(singleSize.X, minWidth, maxWidth);
                float sizeY = sizeX / aspectRatio;
                singleSize = new Vector2(sizeX, sizeY);
                int countPerX = Mathf.FloorToInt(totalSize.X / singleSize.X);
                int countPerY = Mathf.FloorToInt(totalSize.Y / singleSize.Y);
                int index = 0;
                for (int y = 1; y <= countPerY; y++)
                {
                    for (int x = 1; x <= countPerX; x++)
                    {
                        if (index == count)
                            break;

                        var pos = Size - (singleSize + margin) * new Vector2(x, y);
                        _previews[index++].Bounds = new Rectangle(pos, singleSize);
                    }

                    if (index == count)
                        break;
                }
            }
        }

        /// <summary>
        /// Hides the camera preview that uses given camera.
        /// </summary>
        /// <param name="camera">The camera to hide.</param>
        private void HideCameraPreview(Camera camera)
        {
            var preview = _previews.FirstOrDefault(x => x.Camera == camera);
            if (preview != null)
                HideCameraPreview(preview);
        }

        /// <summary>
        /// Hides the camera preview.
        /// </summary>
        /// <param name="preview">The preview to hide.</param>
        private void HideCameraPreview(CameraPreview preview)
        {
            _previews.Remove(preview);
            preview.Dispose();
        }

        /// <summary>
        /// Hides all the camera previews.
        /// </summary>
        private void HideAllCameraPreviews()
        {
            while (_previews.Count > 0)
                HideCameraPreview(_previews[0]);
        }

        /// <inheritdoc />
        public override void OnSceneLoaded(Scene scene, Guid sceneId)
        {
            if (SceneManager.LoadedScenesCount == 1)
            {


                // TODO: load cached viewport for that scene
            }
        }

        /// <inheritdoc />
        public override void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            for (int i = 0; i < _previews.Count; i++)
            {
                if (_previews[i].Camera.Scene == scene)
                    HideCameraPreview(_previews[i--]);
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            UpdateCameraPreview();

            base.Update(deltaTime);
        }

        protected void AddCommandsToController()
        {
            CommandsController.Add(new InputCommand(() => { Viewport.TakeScreenshot(); }, new InputChord(KeyCode.F12)));
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            HideAllCameraPreviews();

            base.OnDestroy();
        }
    }
}
