////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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
    public class EditGameWindow : SceneEditorWindow
    {
        // TODO: finish selected camera preview

        /// <summary>
        /// Camera preview output control
        /// </summary>
        /*public class CameraPreview : RenderOutputControl
        {
            public CameraPreview()
                : base(SceneRenderTask.Create())
            {
            }

            /// <summary>
            /// Update viewport for camera
            /// </summary>
            /// <param name="camera">Camera to use</param>
            public void UpdateForCamera(Camera camera)
            {
                //((CameraRenderTask*)Task)->Camera = camera;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                // Draw frame
                Render2D.DrawRectangle(new Rectangle(Vector2.Zero, Size), Color.Black);
            }
        }
        */

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
            Viewport = new MainEditorGizmoViewport();
            Viewport.Parent = this;
            Viewport.Task.Flags = ViewFlags.DefaultEditor;
        }

        /// <summary>
        /// Moves the viewport to visualize the actor.
        /// </summary>
        /// <param name="actor">The actor to preview.</param>
        public void ShowActor(Actor actor)
        {
            // Calculate target transform to see whole object
            BoundingBox box = actor.BoxWithChildren;
            if (box.Size.LengthSquared < 100)
                box.Size = new Vector3(10);
            BoundingSphere sphere;
            BoundingSphere.FromBox(ref box, out sphere);
            sphere.Radius = Math.Max(sphere.Radius, 10.0f);
            Quaternion orientation = new Quaternion(0.424461186f, -0.0940724313f, 0.0443938486f, 0.899451137f);
            Vector3 position = sphere.Center - Vector3.ForwardLH * orientation * (sphere.Radius * 2.5f);

            // Move vieport
            Viewport.MoveViewport(position, orientation);
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
        public override void Update(float deltaTime)
        {
            if (ParentWindow.GetKeyDown(KeyCode.F12))
            {
                Viewport.TakeScreenshot();
            }

            base.Update(deltaTime);
        }
    }
}
