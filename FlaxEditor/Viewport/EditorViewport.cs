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
        // TODO: maybe cache view/projection matricies to reuse them

        protected struct Input
        {
            public bool IsPanning;
            public bool IsRotating;
            public bool IsMoving;
            public bool IsZooming;
            public bool IsOrbiting;

            public bool IsControlDown;
            public bool IsShiftDown;
            public bool IsAltDown;
            public bool IsMouseRightDown;
            public bool IsMouseMiddleDown;
            public bool IsMouseLeftDown;

            public int MouseWheelDelta;

            public bool IsControllingMouse => IsMouseMiddleDown || IsMouseRightDown || (IsAltDown && IsMouseLeftDown);
            
            public void Gather(FlaxEngine.Window window)
            {
                IsControlDown = window.GetKey(KeyCode.CONTROL);
                IsShiftDown = window.GetKey(KeyCode.SHIFT);
                IsAltDown = window.GetKey(KeyCode.ALT);

                IsMouseRightDown = window.GetMouseButton(MouseButtons.Right);
                IsMouseMiddleDown = window.GetMouseButton(MouseButtons.Middle);
                IsMouseLeftDown = window.GetMouseButton(MouseButtons.Left);
            }

            public void Clear()
            {
                IsControlDown = false;
                IsShiftDown = false;
                IsAltDown = false;

                IsMouseRightDown = false;
                IsMouseMiddleDown = false;
                IsMouseLeftDown = false;
            }
        }

        // how much frames we want to keep in the buffer to calculate the avg. delta currently hardcoded
        public const int FpsCameraFilteringFrames = 3;

        // Movement
        protected ViewportWidgetButton _speedWidget;
        protected float _movementSpeed;
        protected float _mouseAccelerationScale;
        protected bool _useMouseFiltering;
        protected bool _useMouseAcceleration;

        // Input
        private bool _isControllingMouse;
        protected Input _prevInput;
        protected Input _input;
        protected int _deltaFilteringStep;
        protected Vector2 _viewMousePos;
        protected Vector2 _mouseDeltaRight;
        protected Vector2 _mouseDeltaLeft;
        protected Vector2 _startPosRight;
        protected Vector2 _startPosLeft;
        protected Vector2 _mouseDeltaRightLast;
        protected Vector2[] _deltaFilteringBuffer = new Vector2[FpsCameraFilteringFrames];

        // Camera
        protected float _yaw;
        protected float _pitch;
        protected float _fieldOfView = 60.0f;
        protected float _nearPlane = 0.1f;
        protected float _farPlane = 10000.0f;
        
        /// <summary>
        /// Speed of the mouse.
        /// </summary>
        public float MouseSpeed = 1;

        /// <summary>
        /// Speed of the mouse wheel zooming.
        /// </summary>
        public float MouseWheelZoomSpeedFactor = 1;

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
        public Vector2 CamPitchAngles = new Vector2(-86, 86);

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
        public Quaternion ViewOrientation
        {
            get => Quaternion.RotationYawPitchRoll(_yaw * Mathf.DegreesToRadians, _pitch * Mathf.DegreesToRadians, 0);
            protected set => EulerAngles = value.EulerAngles;
        }

        /// <summary>
        /// Gets or sets the view direction vector.
        /// </summary>
        public Vector3 ViewDirection
        {
            get => Vector3.ForwardLH * ViewOrientation;
            protected set
            {
                Vector3 right = Vector3.Cross(value, Vector3.Up);
                Vector3 up = Vector3.Cross(right, value);
                ViewOrientation = Quaternion.LookRotation(value, up);
            }
        }

        /// <summary>
        /// Gets or sets the yaw angle (in degrees).
        /// </summary>
        protected float Yaw
        {
            get => _yaw;
            set => _yaw = value;
        }

        /// <summary>
        /// Gets or sets the pitch angle (in degrees).
        /// </summary>
        protected float Pitch
        {
            get => _pitch;
            set => _pitch = Mathf.Clamp(value, CamPitchAngles.X, CamPitchAngles.Y);
        }

        /// <summary>
        /// Gets or sets the absolute mouse position (normalized, not in pixels). Yaw is X, Pitch is Y.
        /// </summary>
        /// <value>
        /// The absolute mouse position.
        /// </value>
        protected Vector2 YawPitch
        {
            get => new Vector2(_yaw, _pitch);
            set
            {
                Yaw = value.X;
                Pitch = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the euler angles (pitch, yaw, roll).
        /// </summary>
        /// <value>
        /// The euler angles.
        /// </value>
        protected Vector3 EulerAngles
        {
            get => new Vector3(_pitch, _yaw, 0);
            set
            {
                Pitch = value.X;
                Yaw = value.Y;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this viewport has loaded dependant assets.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this viewport has loaded assets; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasLoadedAssets => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewport"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="useWidgets">Enable/disable viewport widgets.</param>
        public EditorViewport(SceneRenderTask task, bool useWidgets)
            : base(task)
        {
            _movementSpeed = 1;
            _mouseAccelerationScale = 0.1f;
            _useMouseFiltering = false;
            _useMouseAcceleration = false;
            
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
            // Ceate matricies
            CreateProjectionMatrix(out view.Projection);
            CreateViewMatrix(out view.View);

            // Copy data
            view.Position = ViewPosition;
            view.Direction = ViewDirection;
            view.Near = _nearPlane;
            view.Far = _farPlane;
        }

        /// <summary>
        /// Creates the projection matrix.
        /// </summary>
        /// <param name="result">The result.</param>
        protected virtual void CreateProjectionMatrix(out Matrix result)
        {
            // Create projection matrix
            float aspect = Width / Height;
            Matrix.PerspectiveFovLH(_fieldOfView * Mathf.DegreesToRadians, aspect, _nearPlane, _farPlane, out result);
        }

        /// <summary>
        /// Creates the view matrix.
        /// </summary>
        /// <param name="result">The result.</param>
        protected virtual void CreateViewMatrix(out Matrix result)
        {
            // Create view matrix
            Vector3 position = ViewPosition;
            Vector3 direction = ViewDirection;
            Vector3 target = position + direction;
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Up, direction));
            Vector3 up = Vector3.Normalize(Vector3.Cross(direction, right));
            Matrix.LookAtLH(ref position, ref target, ref up, out result);
        }

        /// <summary>
        /// Gets the mouse ray.
        /// </summary>
        /// <value>
        /// The mouse ray.
        /// </value>
        public Ray MouseRay
        {
            get
            {
                if (IsMouseOver)
                    return ConvertMouseToRay(ref _viewMousePos);
                return new Ray(Vector3.Maximum, Vector3.Up);
            }
        }

        /// <summary>
        /// Converts the mouse position to the ray (in world space of the viewport).
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>The result ray.</returns>
        public Ray ConvertMouseToRay(ref Vector2 mousePosition)
        {
            // Prepare
            var viewport = new FlaxEngine.Viewport(0, 0, Width, Height);
            Matrix v, p, ivp;
            CreateProjectionMatrix(out p);
            CreateViewMatrix(out v);
            Matrix.Multiply(ref v, ref p, out ivp);
            ivp.Invert();

            // Create near and far points
            Vector3 nearPoint = new Vector3(mousePosition, 0.0f);
            Vector3 farPoint = new Vector3(mousePosition, 1.0f);
            viewport.Unproject(ref nearPoint, ref ivp, out nearPoint);
            viewport.Unproject(ref farPoint, ref ivp, out farPoint);

            // Create direction vector
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Called when mouse control begins.
        /// </summary>
        /// <param name="win">The parent window.</param>
        protected virtual void OnControlMouseBegin(FlaxEngine.Window win)
        {
            win.StartTrackingMouse(false);
            win.Cursor = CursorType.Hidden;

            _viewMousePos = Center;
            win.MousePosition = PointToWindow(_viewMousePos);
        }

        /// <summary>
        /// Called when mouse control ends.
        /// </summary>
        /// <param name="win">The parent window.</param>
        protected virtual void OnControlMouseEnd(FlaxEngine.Window win)
        {
            win.Cursor = CursorType.Default;
            win.EndTrackingMouse();
        }

        /// <summary>
        /// Called when left mouse button goes down (on press).
        /// </summary>
        protected virtual void OnLeftMouseButtonDown()
        {
            _startPosLeft = _viewMousePos;
        }

        /// <summary>
        /// Called when left mouse button goes up (on release).
        /// </summary>
        protected virtual void OnLeftMouseButtonUp()
        {
        }

        /// <summary>
        /// Called when right mouse button goes down (on press).
        /// </summary>
        protected virtual void OnRightMouseButtonDown()
        {
            _startPosRight = _viewMousePos;
        }

        /// <summary>
        /// Called when right mouse button goes up (on release).
        /// </summary>
        protected virtual void OnRightMouseButtonUp()
        {
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="dt">The delta time (in seconds).</param>
        /// <param name="moveDelta">The move delta (scaled).</param>
        /// <param name="mouseDelta">The mouse delta (scaled).</param>
        protected virtual void UpdateView(float dt, ref Vector3 moveDelta, ref Vector2 mouseDelta)
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

            // Update input
            {
                // Get input buttons and keys
                _prevInput = _input;
                if(ContainsFocus)
                    _input.Gather(win.NativeWindow);
                else
                    _input.Clear();

                // Track controlling mouse state change
                bool wasControllingMouse = _prevInput.IsControllingMouse;
                _isControllingMouse = _input.IsControllingMouse;
                if (wasControllingMouse != _isControllingMouse)
                {
                    if (_isControllingMouse)
                        OnControlMouseBegin(win.NativeWindow);
                    else
                        OnControlMouseEnd(win.NativeWindow);
                }

                // Track mouse buttons state change
                if (!_prevInput.IsMouseLeftDown && _input.IsMouseLeftDown)
                    OnLeftMouseButtonDown();
                else if (_prevInput.IsMouseLeftDown && !_input.IsMouseLeftDown)
                    OnLeftMouseButtonUp();
                //
                if (!_prevInput.IsMouseRightDown && _input.IsMouseRightDown)
                    OnRightMouseButtonDown();
                else if (_prevInput.IsMouseRightDown && !_input.IsMouseRightDown)
                    OnRightMouseButtonUp();
            }

            // Check if update mouse
            Vector2 size = Size;
            if (_isControllingMouse)
            {
                // Gather input
                {
                    bool isAltDown = _input.IsAltDown;
                    bool lbDown = _input.IsMouseLeftDown;
                    bool mbDown = _input.IsMouseMiddleDown;
                    bool rbDown = _input.IsMouseRightDown;
                    bool wheelInUse = Math.Abs(_input.MouseWheelDelta) > Mathf.Epsilon;

                    _input.IsPanning = !isAltDown && mbDown && !rbDown;
                    _input.IsRotating = !isAltDown && !mbDown && rbDown;
                    _input.IsMoving = !isAltDown && mbDown && rbDown;
                    //_input.IsZooming = (isAltDown && !lbDown && !mbDown && rbDown) || wheelInUse;
                    _input.IsZooming = wheelInUse;
                    _input.IsOrbiting = isAltDown && lbDown && !mbDown && !rbDown;
                }

                // Get input movement
                Vector3 moveDelta = Vector3.Zero;
                if (win.GetKey(KeyCode.W))
                {
                    moveDelta += Vector3.ForwardLH;
                }
                if (win.GetKey(KeyCode.S))
                {
                    moveDelta += Vector3.BackwardLH;
                }
                if (win.GetKey(KeyCode.D))
                {
                    moveDelta += Vector3.Right;
                }
                if (win.GetKey(KeyCode.A))
                {
                    moveDelta += Vector3.Left;
                }
                if (win.GetKey(KeyCode.E))
                {
                    moveDelta += Vector3.Up;
                }
                if (win.GetKey(KeyCode.Q))
                {
                    moveDelta += Vector3.Down;
                }
                moveDelta.Normalize();// normalize direction
                moveDelta *= _movementSpeed;

                // Speed up or speed down
                if (_input.IsShiftDown)
                    moveDelta *= 4.0f;
                if (_input.IsControlDown)
                    moveDelta *= 0.3f;

                // Calculate smooth mouse delta not dependant on viewport size
                Vector2 offset = _viewMousePos - _startPosRight;
                offset.X = offset.X > 0 ? Mathf.Floor(offset.X) : Mathf.Ceil(offset.X);
                offset.Y = offset.Y > 0 ? Mathf.Floor(offset.Y) : Mathf.Ceil(offset.Y);
                _mouseDeltaRight = offset / size;
                _mouseDeltaRight.Y *= size.Y / size.X;

                Vector2 mouseDelta = Vector2.Zero;
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
                        mouseDelta += _deltaFilteringBuffer[i];

                    mouseDelta /= FpsCameraFilteringFrames;
                }
                else
                    mouseDelta = _mouseDeltaRight;

                if (_useMouseAcceleration)// mouse acceleration
                {
                    // accelerate the delta
                    var currentDelta = mouseDelta;
                    mouseDelta = mouseDelta + _mouseDeltaRightLast * _mouseAccelerationScale;
                    _mouseDeltaRightLast = currentDelta;
                }

                // Get clamped delta time (more stable during lags)
                var dt = Math.Min(Time.UnscaledDeltaTime, 1.0f);

                // Update
                moveDelta *= dt * 60.0f;
                mouseDelta *= 200.0f * MouseSpeed;
                UpdateView(dt, ref moveDelta, ref mouseDelta);

                // Move mouse back to the root position
                Vector2 center = PointToWindow(_startPosRight);
                win.MousePosition = center;
            }
            else
            {
                _mouseDeltaRight = _mouseDeltaRightLast = Vector2.Zero;
            }
            if (_input.IsMouseLeftDown)
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

            _input.MouseWheelDelta = 0;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            Focus();

            base.OnMouseDown(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, int delta)
        {
            _input.MouseWheelDelta += delta;

            return base.OnMouseWheel(location, delta);
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

        private float[] EditorViewportCameraSpeedValues =
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

        private struct ViewModeOptions
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

        private void widgetCamSpeedClick(int id, ContextMenuBase cm)
        {
            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            var button = (ViewportWidgetButton)ccm.GetButton(id).Tag;
            viewport.MovementSpeed = EditorViewportCameraSpeedValues[id];
            button.Text = EditorViewportCameraSpeedValues[id].ToString();
        }

        private void widgetCamSpeedShowHide(Control cm)
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

        private void widgetViewModeClick(int id, ContextMenuBase cm)
        {
            var viewport = (EditorViewport)cm.Tag;
            viewport.Task.Mode = EditorViewportViewModeValues[id].Mode;
        }

        private void widgetViewModeShowHide(Control cm)
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
