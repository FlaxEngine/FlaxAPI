////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Viewport.Widgets;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Editor viewports base class.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.RenderOutputControl" />
    public class EditorViewport : RenderOutputControl
    {
        // how much frames we want to keep in the buffer to calculate the avg. delta currently hardcoded
        public const int FpsCameraFilteringFrames = 3;

        // Movement
        protected ViewportWidgetButton _speedWidget;
        protected float _movementSpeed;
        protected float _mouseAccelerationScale;
        protected bool _useMouseFiltering;
        protected bool _useMouseAcceleration;

        // Mouse
        protected bool _isMouseRightDown;
        protected bool _isMouseLeftDown;
        protected int _deltaFilteringStep;
        protected Vector2 _viewMousePos;
        protected Vector2 _absMousePos;
        protected Vector2 _mouseDeltaRight;
        protected Vector2 _mouseDeltaLeft;
        protected Vector2 _startPosRight;
        protected Vector2 _startPosLeft;
        protected Vector2 _mouseDeltaRightLast;
        protected Vector2[] _deltaFilteringBuffer = new Vector2[FpsCameraFilteringFrames];

        // Camera
        protected float _fieldOfView = 60.0f;
        protected float _nearPlane = 0.1f;
        protected float _farPlane = 10000.0f;

        /// <summary>
        /// Speed of the mouse.
        /// </summary>
        public float MouseSpeed;

        /// <summary>
        /// Gets or sets the camera movement speed.
        /// </summary>
        /// <value>
        /// The movement speed.
        /// </value>
        public float MovementSpeed
        {
            get { return _movementSpeed; }
            set
            {
                for (int i = 0; i < EditorViewportCameraSpeedValues.Length; i++)
                {
                    if (Math.Abs(value - EditorViewportCameraSpeedValues[i]) < 0.001f)
                    {
                        _movementSpeed = EditorViewportCameraSpeedValues[i];
                        if (_speedWidget != null)
                            _speedWidget.Text = _movementSpeed.ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Camera's pitch angle clamp range (in degrees).
        /// </summary>
        public Vector2 CamPitchAngles = new Vector2(-80, 80);

        /// <summary>
        /// Gets the view transform.
        /// </summary>
        /// <value>
        /// The view transform.
        /// </value>
        public Transform ViewTransform => new Transform(ViewPosition, ViewOrientation);
        
        /// <summary>
        /// Gets or sets the view position.
        /// </summary>
        /// <value>
        /// The view position.
        /// </value>
        public Vector3 ViewPosition { get; protected set; }

        /// <summary>
        /// Gets or sets the view orientation.
        /// </summary>
        /// <value>
        /// The view orientation.
        /// </value>
        public Quaternion ViewOrientation { get; protected set; } = Quaternion.Identity;

        /// <summary>
        /// Gets or sets the view direction vector.
        /// </summary>
        public Vector3 ViewDirection
        {
            get { return Vector3.ForwardLH * ViewOrientation; }
            protected set
            {
                Vector3 right = Vector3.Cross(value, Vector3.Up);
                Vector3 up = Vector3.Cross(right, value);
                ViewOrientation = Quaternion.LookRotation(value, up);
            }
        }

        /// <summary>
        /// Gets or sets the absolute mouse position (normalized, not in pixels).
        /// </summary>
        /// <value>
        /// The absolute mouse position.
        /// </value>
        protected Vector2 AbsMousePosition
        {
            get => _absMousePos;
            set => _absMousePos = new Vector2(value.X, Mathf.Clamp(value.Y, CamPitchAngles.X, CamPitchAngles.Y));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewport"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="useWidgets">Enable/disable viewport widgets.</param>
        public EditorViewport(SceneRenderTask task, bool useWidgets)
            : base(task)
        {
            _movementSpeed = 1;
            _mouseAccelerationScale = 0.2f;
            _useMouseFiltering = true;
            _useMouseAcceleration = true;
            MouseSpeed = 1;

            DockStyle = DockStyle.Fill;

            if (useWidgets)
            {
                // Camera speed widget
                var camSpeed = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperRight);
                var camSpeedCM = new ContextMenu();
                var camSpeedButton = new ViewportWidgetButton("1", Editor.Instance.UI.GetIcon("ArrowRightBorder16"), camSpeedCM);
                _speedWidget = camSpeedButton;
                for (int i = 0; i < EditorViewportCameraSpeedValues.Length; i++)
                {
                    var button = camSpeedCM.AddButton(i, EditorViewportCameraSpeedValues[i].ToString());
                    button.Tag = camSpeedButton;
                }
                camSpeedCM.Tag = this;
                camSpeedCM.OnButtonClicked += widgetCamSpeedClick;
                camSpeedCM.OnVisibleChanged += widgetCamSpeedShowHide;
                camSpeedButton.Parent = camSpeed;
                camSpeed.Parent = this;

                // View mode widget
                var viewMode = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperLeft);
                var viewModeCM = new ContextMenu();
                for (int i = 0; i < EditorViewportViewModeValues.Length; i++)
                {
                    viewModeCM.AddButton(i, EditorViewportViewModeValues[i].Name);
                }
                viewModeCM.Tag = this;
                viewModeCM.OnButtonClicked += widgetViewModeClick;
                viewModeCM.OnVisibleChanged += widgetViewModeShowHide;
                var viewModeButton = new ViewportWidgetButton("View", Sprite.Invalid, viewModeCM);
                viewModeButton.Parent = viewMode;
                viewMode.Parent = this;

                // TODO: provide widget for chaging near and far plane
            }

            // Link for task event
            task.OnBegin += x => CopyViewData(ref x.View);
        }

        /// <summary>
        /// Takes the screenshot of the current viewport.
        /// </summary>
        /// <param name="path">The output file path. Set null to use default value.</param>
        public void TakeScreenshot(string path = null)
        {
            //CaptureScreenshot.Capture(Task, path);
            throw new NotImplementedException();// TODO: taking screenshots
        }
        
        /// <summary>
        /// Copies the render view data to <see cref="RenderView"/> structure.
        /// </summary>
        /// <param name="view">The view.</param>
        public void CopyViewData(ref RenderView view)
        {
            // Create projection matrix
            float aspect = Width / Height;
            Matrix.PerspectiveFovLH(_fieldOfView * Mathf.DegreesToRadians, aspect, _nearPlane, _farPlane, out view.Projection);

            // Create view matrix
            Vector3 position = ViewPosition;
            Vector3 direction = ViewDirection;
            Vector3 target = position + direction;
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Up, direction));
            Vector3 up = Vector3.Normalize(Vector3.Cross(direction, right));
            Matrix.LookAtLH(ref position, ref target, ref up, out view.View);

            // Copy data
            view.Position = ViewPosition;
            view.Direction = ViewDirection;
            view.Near = _nearPlane;
            view.Far = _farPlane;
        }

        protected virtual void UpdateMouse(float dt, ref Vector3 move)
        {
        }

        protected virtual void OnLeftMouseButtonUp()
        {
        }

        protected virtual void OnRightMouseButtonUp()
        {
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Get parent window
            var win = ParentWindow;

            // Get current mouse position in the view
            _viewMousePos = PointFromWindow(win.MousePosition);

            // Update
            Vector2 size = Size;
            if (_isMouseRightDown)
            {
                // Get input movement
                Vector3 move = Vector3.Zero;
                if (win.GetKey(KeyCode.W))
                {
                    move += Vector3.ForwardLH;
                }
                if (win.GetKey(KeyCode.S))
                {
                    move += Vector3.BackwardLH;
                }
                if (win.GetKey(KeyCode.D))
                {
                    move += Vector3.Right;
                }
                if (win.GetKey(KeyCode.A))
                {
                    move += Vector3.Left;
                }
                if (win.GetKey(KeyCode.E))
                {
                    move += Vector3.Up;
                }
                if (win.GetKey(KeyCode.Q))
                {
                    move += Vector3.Down;
                }
                move.Normalize();// normalize direction
                move *= _movementSpeed;

                if (win.GetKey(KeyCode.SHIFT))
                    move *= 2.0f;
                if (win.GetKey(KeyCode.CONTROL))
                    move *= 0.5f;

                // Calculate smooth mouse delta not dependant on viewport size
                Vector2 offset = _viewMousePos - _startPosRight;
                offset.X = offset.X > 0 ? Mathf.Floor(offset.X) : Mathf.Ceil(offset.X);
                offset.Y = offset.Y > 0 ? Mathf.Floor(offset.Y) : Mathf.Ceil(offset.Y);
                _mouseDeltaRight = offset / size;
                _mouseDeltaRight.Y *= size.Y / size.X;

                Vector2 delta = Vector2.Zero;
                if (_useMouseFiltering)// mouse filtering
                {
                    // update delta filtering buffer
                    _deltaFilteringBuffer[_deltaFilteringStep] = _mouseDeltaRight;
                    _deltaFilteringStep++;

                    // if the step is too far, zeroe
                    if (_deltaFilteringStep == FpsCameraFilteringFrames)
                        _deltaFilteringStep = 0;

                    // calculate filtered delta(avg)
                    for (int i = 0; i < FpsCameraFilteringFrames; i++)
                        delta += _deltaFilteringBuffer[i];

                    delta /= FpsCameraFilteringFrames;
                }
                else
                    delta = _mouseDeltaRight;

                if (_useMouseAcceleration)// mouse acceleration
                {
                    // accelerate the delta
                    var currentDelta = delta;
                    delta = delta + _mouseDeltaRightLast * _mouseAccelerationScale;
                    _mouseDeltaRightLast = currentDelta;
                }

                // Accumulate position
                AbsMousePosition += delta * (200.0f * MouseSpeed);

                // Update
                var dt = Time.UnscaledDeltaTime;
                move *= dt * 60.0f;
                UpdateMouse(dt, ref move);

                // Move mouse back to the root position
                Vector2 center = PointToWindow(_startPosRight);
                win.MousePosition = center;
            }
            else
            {
                _mouseDeltaRight = Vector2.Zero;
            }
            if (_isMouseLeftDown)
            {
                // Calculate smooth mouse delta not dependant on viewport size
                Vector2 offset = _viewMousePos - _startPosLeft;
                offset.X = offset.X > 0 ? Mathf.Floor(offset.X) : Mathf.Ceil(offset.X);
                offset.Y = offset.Y > 0 ? Mathf.Floor(offset.Y) : Mathf.Ceil(offset.Y);
                _mouseDeltaLeft = offset / size;
                _startPosLeft = _viewMousePos;
            }
            else
            {
                _mouseDeltaLeft = Vector2.Zero;
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Focus
            Focus();

            // Base
            if (base.OnMouseDown(location, buttons))
                return true;

            // Update buttons
            if (buttons == MouseButtons.Right)
            {
                // Set flag
                _isMouseRightDown = true;

                // Get start mouse position
                var win = ParentWindow;
                _startPosRight = PointFromWindow(win.MousePosition);

                // Request buffers resize
                RequestResize();

                // Event handled
                return true;
            }
            if (buttons == MouseButtons.Left)
            {
                // Set flag
                _isMouseLeftDown = true;

                // Get start mouse position
                var win = ParentWindow;
                _startPosLeft = PointFromWindow(win.MousePosition);

                // Event handled
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Update flags
            if (buttons == MouseButtons.Right && _isMouseRightDown)
            {
                _isMouseRightDown = false;
                OnRightMouseButtonUp();
            }
            if (buttons == MouseButtons.Left && _isMouseLeftDown)
            {
                _isMouseLeftDown = false;
                OnLeftMouseButtonUp();
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flags
            _isMouseLeftDown = false;
            _isMouseRightDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            // Arrange viewport widgets
            const float margin = ViewportWidgetsContainer.WidgetsMargin;
            float left = margin;
            float right = Width - margin;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ViewportWidgetsContainer widget && widget.Visible)
                {
                    float x;
                    switch (widget.WidgetLocation)
                    {
                        case ViewportWidgetLocation.UpperLeft:
                            x = left;
                            left += widget.Width + margin;
                            break;
                        case ViewportWidgetLocation.UpperRight:
                            x = right - widget.Width;
                            right = x - margin;
                            break;
                        default:
                            x = 0;
                            break;
                    }
                    widget.Location = new Vector2(x, margin);
                }
            }
        }

        float[] EditorViewportCameraSpeedValues =
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

        struct ViewModeOptions
        {
            public ViewMode Mode;
            public string Name;

            public ViewModeOptions(ViewMode mode, string name)
            {
                Mode = mode;
                Name = name;
            }
        }

        private ViewModeOptions[] EditorViewportViewModeValues =
        {
            new ViewModeOptions(ViewMode.Default, "Default"),
            new ViewModeOptions(ViewMode.Fast, "No PostFx"),
            new ViewModeOptions(ViewMode.LightsBuffer, "Light Buffer"),
            new ViewModeOptions(ViewMode.Reflections, "Reflections Buffer"),
            new ViewModeOptions(ViewMode.Depth, "Depth Buffer"),
            new ViewModeOptions(ViewMode.Diffuse, "Diffuse"),
            new ViewModeOptions(ViewMode.Metalness, "Metalness"),
            new ViewModeOptions(ViewMode.Roughness, "Roughness"),
            new ViewModeOptions(ViewMode.Specular, "Specular"),
            new ViewModeOptions(ViewMode.SpecularColor, "Specular Color"),
            new ViewModeOptions(ViewMode.ShadingModel, "Shading Model"),
            new ViewModeOptions(ViewMode.Emissive, "Emissive Light"),
            new ViewModeOptions(ViewMode.Normals, "Normals"),
            new ViewModeOptions(ViewMode.AmbientOcclusion, "Ambient Occlusion"),
        };

        void widgetCamSpeedClick(int id, ContextMenuBase cm)
        {
            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            var button = (ViewportWidgetButton)ccm.GetButton(id).Tag;
            viewport.MovementSpeed = EditorViewportCameraSpeedValues[id];
            button.Text = EditorViewportCameraSpeedValues[id].ToString();
        }

        void widgetCamSpeedShowHide(Control cm)
        {
            if (cm.Visible == false)
                return;

            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            for (int i = 0; i < EditorViewportCameraSpeedValues.Length; i++)
            {
                ccm.GetButton(i).Icon = Math.Abs(viewport.MovementSpeed - EditorViewportCameraSpeedValues[i]) < 0.001f ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }
        }

        void widgetViewModeClick(int id, ContextMenuBase cm)
        {
            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            viewport.Task.Mode = EditorViewportViewModeValues[id].Mode;
        }

        void widgetViewModeShowHide(Control cm)
        {
            if (cm.Visible == false)
                return;

            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            for (int i = 0; i < EditorViewportViewModeValues.Length; i++)
            {
                ccm.GetButton(i).Icon = viewport.Task.Mode == EditorViewportViewModeValues[i].Mode ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }
        }
    }
}
