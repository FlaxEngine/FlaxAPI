////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Viewport.Widgets;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using FlaxEngine.Utilities;

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
                IsControlDown = window.GetKey(KeyCode.Control);
                IsShiftDown = window.GetKey(KeyCode.Shift);
                IsAltDown = window.GetKey(KeyCode.Alt);

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
        protected float _nearPlane = 0.2f;
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
        public float MovementSpeed
        {
            get => _movementSpeed;
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
        public Vector2 CamPitchAngles = new Vector2(-88, 88);

        /// <summary>
        /// Gets the view transform.
        /// </summary>
        public Transform ViewTransform => new Transform(ViewPosition, ViewOrientation);

        /// <summary>
        /// Gets or sets the view position.
        /// </summary>
        public Vector3 ViewPosition { get; protected set; }

        /// <summary>
        /// Gets or sets the view orientation.
        /// </summary>
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
        public virtual bool HasLoadedAssets => true;

        /// <summary>
        /// The 'View' widget button context menu.
        /// </summary>
        public ContextMenu ViewWidgetButtonMenu;

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
                camSpeedButton.TooltipText = "Camera speed scale";
                _speedWidget = camSpeedButton;
                for (int i = 0; i < EditorViewportCameraSpeedValues.Length; i++)
                {
                    var button = camSpeedCM.AddButton(i, EditorViewportCameraSpeedValues[i].ToString());
                    button.Tag = camSpeedButton;
                }
                camSpeedCM.Tag = this;
                camSpeedCM.OnButtonClicked += widgetCamSpeedClick;
                camSpeedCM.VisibleChanged += widgetCamSpeedShowHide;
                camSpeedButton.Parent = camSpeed;
                camSpeed.Parent = this;

                // View mode widget
                var viewMode = new ViewportWidgetsContainer(ViewportWidgetLocation.UpperLeft);
                ViewWidgetButtonMenu = new ContextMenu();
                var viewModeButton = new ViewportWidgetButton("View", Sprite.Invalid, ViewWidgetButtonMenu);
                viewModeButton.TooltipText = "View properties";
                viewModeButton.Parent = viewMode;
                viewMode.Parent = this;

                // Show FPS
                {
                    InitFpsCounter();
                    _showFpsButon = ViewWidgetButtonMenu.AddButton("Show FPS", () => ShowFpsCounter = !ShowFpsCounter);
                }

                // View Flags
                {
                    var viewFlags = ViewWidgetButtonMenu.AddChildMenu("View Flags").ContextMenu;
                    for (int i = 0; i < EditorViewportViewFlagsValues.Length; i++)
                    {
                        viewFlags.AddButton(i, EditorViewportViewFlagsValues[i].Name);
                    }
                    viewFlags.Tag = this;
                    viewFlags.OnButtonClicked += widgetViewFlagsClick;
                    viewFlags.VisibleChanged += widgetViewFlagsShowHide;
                }

                // Debug View
                {
                    var debugView = ViewWidgetButtonMenu.AddChildMenu("Debug View").ContextMenu;
                    for (int i = 0; i < EditorViewportViewModeValues.Length; i++)
                    {
                        debugView.AddButton(i, EditorViewportViewModeValues[i].Name);
                    }
                    debugView.Tag = this;
                    debugView.OnButtonClicked += widgetViewModeClick;
                    debugView.VisibleChanged += widgetViewModeShowHide;
                }
                
                ViewWidgetButtonMenu.AddSeparator();

                // Field of View
                {
                    var fov = ViewWidgetButtonMenu.AddButton("Field Of View", null);
                    var fovValue = new FloatValueBox(1, 75, 2, 50.0f, 35.0f, 160.0f);
                    fovValue.Parent = fov;
                    fovValue.ValueChanged += () => _fieldOfView = fovValue.Value;
                    ViewWidgetButtonMenu.VisibleChanged += control => fovValue.Value = _fieldOfView;
                }
                
                // Far Plane
                {
                    var farPlane = ViewWidgetButtonMenu.AddButton("Far Plane", null);
                    var farPlaneValue = new FloatValueBox(1000, 75, 2, 50.0f, 10.0f, 100000.0f);
                    farPlaneValue.Parent = farPlane;
                    farPlaneValue.ValueChanged += () => _farPlane = farPlaneValue.Value;
                    ViewWidgetButtonMenu.VisibleChanged += control => farPlaneValue.Value = _farPlane;
                }
            }

            // Link for task event
            task.Begin += x => CopyViewData(ref x.View);
        }

        #region FPS Counter

        private class FpsCounter : Control
        {
            private float frameCount;
            private float dt;
            private float fps;
            private float updateRate = 4.0f;

            public FpsCounter(float x, float y)
                : base(x, y, 64, 32)
            {
            }

            public override void Update(float deltaTime)
            {
                base.Update(deltaTime);

                frameCount++;
                dt += deltaTime;
                if (dt > 1.0 / updateRate)
                {
                    fps = frameCount / dt;
                    frameCount = 0;
                    dt -= 1.0f / updateRate;
                }
            }

            public override void Draw()
            {
                base.Draw();

                Color color = Color.Green;
                if (fps < 24.0f)
                    color = Color.Yellow;
                else if (fps < 15.0f)
                    color = Color.Red;
                string text = string.Format("FPS: {0}", (int)fps);
                Render2D.DrawText(Style.Current.FontMedium, text, new Rectangle(Vector2.Zero, Size), color);
            }
        }

        private FpsCounter _fpsCounter;
        private ContextMenuButton _showFpsButon;

        /// <summary>
        /// Gets or sets a value indicating whether show or hide FPS counter.
        /// </summary>
        public bool ShowFpsCounter
        {
            get => _fpsCounter.Visible;
            set
            {
                _fpsCounter.Visible = value;
                _showFpsButon.Icon = value ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }
        }

        private void InitFpsCounter()
        {
            _fpsCounter = new FpsCounter(10, ViewportWidgetsContainer.WidgetsHeight + 14);
            _fpsCounter.Visible = false;
            _fpsCounter.Parent = this;
        }

        #endregion

        /// <summary>
        /// Takes the screenshot of the current viewport.
        /// </summary>
        /// <param name="path">The output file path. Set null to use default value.</param>
        public void TakeScreenshot(string path = null)
        {
            Screenshot.Capture(Task, path);
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
                // Get input buttons and keys (skip if viewort has no focus or mouse is over a child control)
                _prevInput = _input;
                if (ContainsFocus && GetChildAt(_viewMousePos) == null)
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
            ViewportWidgetsContainer.ArrangeWidgets(this);
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

        private struct ViewFlagOptions
        {
            public ViewFlags Mode;
            public string Name;

            public ViewFlagOptions(ViewFlags mode, string name)
            {
                Mode = mode;
                Name = name;
            }
        }

        private ViewFlagOptions[] EditorViewportViewFlagsValues =
        {
            new ViewFlagOptions(ViewFlags.AntiAliasing, "Anti Aliasing"),
            new ViewFlagOptions(ViewFlags.Shadows, "Shadows"),
            new ViewFlagOptions(ViewFlags.DynamicActors, "Dynamic Actors"),
            new ViewFlagOptions(ViewFlags.EditorSprites, "Editor Sprites"),
            new ViewFlagOptions(ViewFlags.Reflections, "Reflectons"),
            new ViewFlagOptions(ViewFlags.SSR, "Screen Space Reflections"),
            new ViewFlagOptions(ViewFlags.AO, "Ambient Occlusion"),
            new ViewFlagOptions(ViewFlags.GI, "Global Illumination"),
            new ViewFlagOptions(ViewFlags.DirectionalLights, "Directional Lights"),
            new ViewFlagOptions(ViewFlags.PointLights, "Point Lights"),
            new ViewFlagOptions(ViewFlags.SpotLights, "Spot Lights"),
            new ViewFlagOptions(ViewFlags.SkyLights, "Sky Lights"),
            new ViewFlagOptions(ViewFlags.SpecularLight, "Specular Light"),
            new ViewFlagOptions(ViewFlags.CustomPostProcess, "Custom Post Process"),
            new ViewFlagOptions(ViewFlags.Bloom, "Bloom"),
            new ViewFlagOptions(ViewFlags.ToneMapping, "Tone Mapping"),
            new ViewFlagOptions(ViewFlags.EyeAdaptation, "Eye Adaptaion"),
            new ViewFlagOptions(ViewFlags.CameraArtifacts, "Camera Artifacts"),
            new ViewFlagOptions(ViewFlags.LensFlares, "Lens Flares"),
            new ViewFlagOptions(ViewFlags.CSG, "CSG Brushes"),
            new ViewFlagOptions(ViewFlags.DepthOfField, "Depth of Field"),
        };

        private void widgetViewFlagsClick(int id, ContextMenuBase cm)
        {
            var viewport = (EditorViewport)cm.Tag;
            viewport.Task.Flags ^= EditorViewportViewFlagsValues[id].Mode;
        }

        private void widgetViewFlagsShowHide(Control cm)
        {
            if (cm.Visible == false)
                return;

            var ccm = (ContextMenu)cm;
            var viewport = (EditorViewport)cm.Tag;
            for (int i = 0; i < EditorViewportViewFlagsValues.Length; i++)
            {
                ccm.GetButton(i).Icon = (viewport.Task.Flags & EditorViewportViewFlagsValues[i].Mode) != 0 ? Style.Current.CheckBoxTick : Sprite.Invalid;
            }
        }
    }
}
