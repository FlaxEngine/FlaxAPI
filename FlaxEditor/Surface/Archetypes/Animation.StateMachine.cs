// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    public static partial class Animation
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode" /> for the state machine output node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        /// <seealso cref="FlaxEditor.Surface.ISurfaceContext" />
        public class StateMachine : SurfaceNode, ISurfaceContext
        {
            private IntValueBox _maxTransitionsPerUpdate;
            private CheckBox _reinitializeOnBecomingRelevant;
            private CheckBox _skipFirstUpdateTransition;

            /// <summary>
            /// Flag for editor UI updating. Used to skip value change events to prevent looping data flow.
            /// </summary>
            protected bool _isUpdatingUI;

            /// <summary>
            /// Gets or sets the node title text.
            /// </summary>
            public string StateMachineTitle
            {
                get => (string)Values[0];
                set
                {
                    if (!string.Equals(value, (string)Values[0], StringComparison.Ordinal))
                    {
                        SetValue(0, value);
                    }
                }
            }

            /// <summary>
            /// Gets or sets the maximum amount of active transitions per update.
            /// </summary>
            public int MaxTransitionsPerUpdate
            {
                get => (int)Values[2];
                set => SetValue(2, value);
            }

            /// <summary>
            /// Gets or sets a value indicating whether reinitialize state machine on becoming relevant (used for blending, etc.).
            /// </summary>
            public bool ReinitializeOnBecomingRelevant
            {
                get => (bool)Values[3];
                set => SetValue(3, value);
            }

            /// <summary>
            /// Gets or sets a value indicating whether skip any triggered transitions durig first animation state machine update.
            /// </summary>
            public bool SkipFirstUpdateTransition
            {
                get => (bool)Values[4];
                set => SetValue(4, value);
            }

            /// <inheritdoc />
            public StateMachine(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
                var marginX = FlaxEditor.Surface.Constants.NodeMarginX;
                var uiStartPosY = FlaxEditor.Surface.Constants.NodeMarginY + FlaxEditor.Surface.Constants.NodeHeaderSize;

                var editButton = new Button(marginX, uiStartPosY, 246, 20);
                editButton.Text = "Edit";
                editButton.Parent = this;
                editButton.Clicked += Edit;

                var maxTransitionsPerUpdateLabel = new Label(marginX, editButton.Bottom + 4, 153, TextBox.DefaultHeight);
                maxTransitionsPerUpdateLabel.HorizontalAlignment = TextAlignment.Near;
                maxTransitionsPerUpdateLabel.Text = "Max Transitions Per Update:";
                maxTransitionsPerUpdateLabel.Parent = this;

                _maxTransitionsPerUpdate = new IntValueBox(3, maxTransitionsPerUpdateLabel.Right + 4, maxTransitionsPerUpdateLabel.Y, 40, 1, 32, 0.1f);
                _maxTransitionsPerUpdate.ValueChanged += () => MaxTransitionsPerUpdate = _maxTransitionsPerUpdate.Value;
                _maxTransitionsPerUpdate.Parent = this;

                var reinitializeOnBecomingRelevantLabel = new Label(marginX, maxTransitionsPerUpdateLabel.Bottom + 4, 185, TextBox.DefaultHeight);
                reinitializeOnBecomingRelevantLabel.HorizontalAlignment = TextAlignment.Near;
                reinitializeOnBecomingRelevantLabel.Text = "Reinitialize On Becoming Relevant:";
                reinitializeOnBecomingRelevantLabel.Parent = this;

                _reinitializeOnBecomingRelevant = new CheckBox(reinitializeOnBecomingRelevantLabel.Right + 4, reinitializeOnBecomingRelevantLabel.Y, true, TextBox.DefaultHeight);
                _reinitializeOnBecomingRelevant.StateChanged += (checkbox) => ReinitializeOnBecomingRelevant = checkbox.Checked;
                _reinitializeOnBecomingRelevant.Parent = this;

                var skipFirstUpdateTransitionLabel = new Label(marginX, reinitializeOnBecomingRelevantLabel.Bottom + 4, 152, TextBox.DefaultHeight);
                skipFirstUpdateTransitionLabel.HorizontalAlignment = TextAlignment.Near;
                skipFirstUpdateTransitionLabel.Text = "Skip First Update Transition:";
                skipFirstUpdateTransitionLabel.Parent = this;

                _skipFirstUpdateTransition = new CheckBox(skipFirstUpdateTransitionLabel.Right + 4, skipFirstUpdateTransitionLabel.Y, true, TextBox.DefaultHeight);
                _skipFirstUpdateTransition.StateChanged += (checkbox) => SkipFirstUpdateTransition = checkbox.Checked;
                _skipFirstUpdateTransition.Parent = this;
            }

            /// <summary>
            /// Opens the state machine editing UI.
            /// </summary>
            public void Edit()
            {
                Surface.OpenContext(this);
            }

            /// <summary>
            /// Starts the state machine renaming by showing a rename popup to the user.
            /// </summary>
            public void StartRenaming()
            {
                Surface.Select(this);
                var dialog = RenamePopup.Show(this, _headerRect, Title, false);
                dialog.Renamed += OnRenamed;
            }

            private void OnRenamed(RenamePopup renamePopup)
            {
                StateMachineTitle = renamePopup.Text;
            }

            /// <summary>
            /// Updates the editor UI.
            /// </summary>
            protected void UpdateUI()
            {
                if (_isUpdatingUI)
                    return;
                _isUpdatingUI = true;

                _maxTransitionsPerUpdate.Value = MaxTransitionsPerUpdate;
                _reinitializeOnBecomingRelevant.Checked = ReinitializeOnBecomingRelevant;
                _skipFirstUpdateTransition.Checked = SkipFirstUpdateTransition;
                Title = StateMachineTitle;

                _isUpdatingUI = false;
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateUI();
            }

            /// <inheritdoc />
            public override void OnSpawned()
            {
                base.OnSpawned();

                StartRenaming();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                UpdateUI();
            }

            /// <inheritdoc />
            public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseDoubleClick(location, buttons))
                    return true;

                if (_headerRect.Contains(ref location))
                {
                    StartRenaming();
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                if (IsDisposing)
                    return;

                // Remove from cache
                Surface.RemoveContext(this);

                base.Dispose();
            }

            /// <inheritdoc />
            public string SurfaceName => StateMachineTitle;

            /// <inheritdoc />
            public byte[] SurfaceData
            {
                get => (byte[])Values[1];
                set => SetValue(1, value);
            }

            /// <inheritdoc />
            public void OnContextCreated(VisjectSurfaceContext context)
            {
                context.Loaded += OnSurfaceLoaded;
            }

            private void OnSurfaceLoaded(VisjectSurfaceContext context)
            {
                // Ensure that loaded surface has entry node for state machine
                var entryNode = context.FindNode(9, 19);
                if (entryNode == null)
                {
                    entryNode = context.SpawnNode(9, 19, new Vector2(100.0f));
                }
            }
        }

        /// <summary>
        /// Customized <see cref="SurfaceNode" /> for the state machine entry node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        /// <seealso cref="FlaxEditor.Surface.IConnectionInstigator" />
        public class StateMachineEntry : SurfaceNode, IConnectionInstigator
        {
            private bool _isMouseDown;
            private Rectangle _textRect;
            private Rectangle _dragAreaRect;

            /// <summary>
            /// Gets or sets the first state for the state machine pointed by the entry node.
            /// </summary>
            public StateMachineState FirstState
            {
                get => Surface.FindNode((int)Values[0]) as StateMachineState;
                set
                {
                    if (FirstState != value)
                    {
                        var id = value != null ? (int)value.ID : -1;
                        SetValue(0, id);
                    }
                }
            }

            /// <inheritdoc />
            public StateMachineEntry(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
            }

            private void StartCreatingTransition()
            {
                Surface.ConnectingStart(this);
            }

            /// <inheritdoc />
            protected override void UpdateRectangles()
            {
                base.UpdateRectangles();

                _textRect = new Rectangle(Vector2.Zero, Size);

                var style = Style.Current;
                var titleSize = style.FontLarge.MeasureText(Title);
                var width = Mathf.Max(100, titleSize.X + 50);
                Resize(width, 0);
                titleSize.X += 8.0f;
                _dragAreaRect = new Rectangle((Size - titleSize) * 0.5f, titleSize);
            }

            /// <inheritdoc />
            public override void Draw()
            {
                var style = Style.Current;

                // Paint Background
                BackgroundColor = _isSelected ? Color.OrangeRed : style.BackgroundNormal;
                if (IsMouseOver)
                    BackgroundColor *= 1.2f;
                Render2D.FillRectangle(_textRect, BackgroundColor);

                // Push clipping mask
                if (ClipChildren)
                {
                    GetDesireClientArea(out var clientArea);
                    Render2D.PushClip(ref clientArea);
                }

                DrawChildren();

                // Pop clipping mask
                if (ClipChildren)
                {
                    Render2D.PopClip();
                }

                // Name
                Render2D.DrawText(style.FontLarge, Title, _textRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);
            }

            /// <inheritdoc />
            public override bool CanSelect(ref Vector2 location)
            {
                return _dragAreaRect.MakeOffseted(Location).Contains(ref location);
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && !_dragAreaRect.Contains(ref location))
                {
                    _isMouseDown = true;
                    Cursor = CursorType.Hand;
                    Focus();
                    return true;
                }

                if (base.OnMouseDown(location, buttons))
                    return true;

                return false;
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    _isMouseDown = false;
                    Cursor = CursorType.Default;
                    Surface.ConnectingEnd(this);
                }

                if (base.OnMouseUp(location, buttons))
                    return true;

                return false;
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                Surface.ConnectingOver(this);
                base.OnMouseMove(location);
            }

            /// <inheritdoc />
            public override void OnMouseLeave()
            {
                base.OnMouseLeave();

                if (_isMouseDown)
                {
                    _isMouseDown = false;
                    Cursor = CursorType.Default;

                    StartCreatingTransition();
                }
            }

            /// <inheritdoc />
            public override void DrawConnections()
            {
                var targetState = FirstState;
                if (targetState != null)
                {
                    // Draw the connection
                    var startPos = PointToParent(Size * 0.5f);
                    var endPos = targetState.PointToParent(targetState.Size * 0.5f);
                    var color = Color.White;
                    StateMachineState.DrawConnection(Surface, ref startPos, ref endPos, ref color);
                }
            }

            /// <inheritdoc />
            public override void RemoveConnections()
            {
                base.RemoveConnections();

                FirstState = null;
            }

            /// <inheritdoc />
            public Vector2 ConnectionOrigin => Center;

            /// <inheritdoc />
            public bool AreConnected(IConnectionInstigator other)
            {
                return other is StateMachineState state && (int)state.ID == (int)Values[0];
            }

            /// <inheritdoc />
            public bool CanConnectWith(IConnectionInstigator other)
            {
                return other is StateMachineState;
            }

            /// <inheritdoc />
            public void DrawConnectingLine(ref Vector2 startPos, ref Vector2 endPos, ref Color color)
            {
                StateMachineState.DrawConnection(Surface, ref startPos, ref endPos, ref color);
            }

            /// <inheritdoc />
            public void Connect(IConnectionInstigator other)
            {
                var state = (StateMachineState)other;

                FirstState = state;
            }
        }

        /// <summary>
        /// Customized <see cref="SurfaceNode" /> for the state machine state node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        /// <seealso cref="FlaxEditor.Surface.IConnectionInstigator" />
        /// <seealso cref="FlaxEditor.Surface.ISurfaceContext" />
        public class StateMachineState : SurfaceNode, ISurfaceContext, IConnectionInstigator
        {
            private bool _isSavingData;
            private bool _isMouseDown;
            private Rectangle _textRect;
            private Rectangle _dragAreaRect;
            private Rectangle _renameButtonRect;

            /// <summary>
            /// The transitions list from this state to the others.
            /// </summary>
            public readonly List<StateMachineTransition> Transitions = new List<StateMachineTransition>();

            /// <summary>
            /// Gets or sets the node title text.
            /// </summary>
            public string StateTitle
            {
                get => (string)Values[0];
                set
                {
                    if (!string.Equals(value, (string)Values[0], StringComparison.Ordinal))
                    {
                        SetValue(0, value);
                    }
                }
            }

            /// <summary>
            /// Gets or sets the state data (transitions list with rules graph and other options).
            /// </summary>
            public byte[] StateData
            {
                get => (byte[])Values[2];
                set => SetValue(2, value);
            }

            /// <inheritdoc />
            public StateMachineState(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
            }

            /// <summary>
            /// Draws the connection between two state machine nodes.
            /// </summary>
            /// <param name="surface">The surface.</param>
            /// <param name="startPos">The start position.</param>
            /// <param name="endPos">The end position.</param>
            /// <param name="color">The line color.</param>
            public static void DrawConnection(VisjectSurface surface, ref Vector2 startPos, ref Vector2 endPos, ref Color color)
            {
                var sub = endPos - startPos;
                var length = sub.Length;
                if (length > Mathf.Epsilon)
                {
                    var dir = sub / length;
                    var arrowRect = new Rectangle(0, 0, 16.0f, 16.0f);
                    float rotation = Vector2.Dot(dir, Vector2.UnitY);
                    if (endPos.X < startPos.X)
                        rotation = 2 - rotation;
                    // TODO: make it look better (fix the math)
                    var arrowTransform = Matrix3x3.Translation2D(new Vector2(-16.0f, -8.0f)) * Matrix3x3.RotationZ(rotation * Mathf.PiOverTwo) * Matrix3x3.Translation2D(endPos);

                    Render2D.PushTransform(ref arrowTransform);
                    Render2D.DrawSprite(surface.Style.Icons.ArrowClose, arrowRect);
                    Render2D.PopTransform();

                    endPos -= dir * 4.0f;
                }
                Render2D.DrawLine(startPos, endPos, color, 2.2f);
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                // Check for external state data changes (eg. via undo)
                if (!_isSavingData && index == 2)
                {
                    // Synchronize data
                    LoadData();
                }
                else if (index == 0)
                {
                    // Update node title UI on change
                    UpdateTitle();
                }
            }

            private void UpdateTitle()
            {
                Title = StateTitle;
                var style = Style.Current;
                var titleSize = style.FontLarge.MeasureText(Title);
                var width = Mathf.Max(100, titleSize.X + 50);
                Resize(width, 0);
                titleSize.X += 8.0f;
                _dragAreaRect = new Rectangle((Size - titleSize) * 0.5f, titleSize);
            }

            /// <inheritdoc />
            protected override void UpdateRectangles()
            {
                base.UpdateRectangles();

                const float buttonMargin = FlaxEditor.Surface.Constants.NodeCloseButtonMargin;
                const float buttonSize = FlaxEditor.Surface.Constants.NodeCloseButtonSize;
                _renameButtonRect = new Rectangle(_closeButtonRect.Left - buttonSize - buttonMargin, buttonMargin, buttonSize, buttonSize);
                _textRect = new Rectangle(Vector2.Zero, Size);
                _dragAreaRect = _headerRect;
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateTitle();
                LoadData();
            }

            /// <inheritdoc />
            public override void OnSpawned()
            {
                base.OnSpawned();

                StartRenaming();
            }

            /// <summary>
            /// Loads the state data from the node value (reads transitions and related information).
            /// </summary>
            public void LoadData()
            {
                ClearData();

                var data = StateData;
                if (data == null || data.Length == 0)
                {
                    // Empty state
                    return;
                }

                // TODO: load data from bytes and update UI
            }

            /// <summary>
            /// Saves the state data to the node value (writes transitions and related information).
            /// </summary>
            public void SaveData()
            {
                try
                {
                    _isSavingData = true;

                    // TODO: save data to bytes and set node value
                }
                finally
                {
                    _isSavingData = false;
                }
            }

            /// <summary>
            /// Clears the state data (removes transitions and related information).
            /// </summary>
            public void ClearData()
            {
                Transitions.Clear();
            }

            /// <summary>
            /// Opens the state editing UI.
            /// </summary>
            public void Edit()
            {
                Surface.OpenContext(this);
            }

            /// <summary>
            /// Starts the state renaming by showing a rename popup to the user.
            /// </summary>
            public void StartRenaming()
            {
                Surface.Select(this);
                var dialog = RenamePopup.Show(this, _textRect, Title, false);
                dialog.Renamed += OnRenamed;
            }

            private void OnRenamed(RenamePopup renamePopup)
            {
                StateTitle = renamePopup.Text;
            }

            private void StartCreatingTransition()
            {
                Surface.ConnectingStart(this);
            }

            /// <inheritdoc />
            public override void Draw()
            {
                var style = Style.Current;

                // Paint Background
                BackgroundColor = _isSelected ? Color.OrangeRed : style.BackgroundNormal;
                if (IsMouseOver)
                    BackgroundColor *= 1.2f;
                Render2D.FillRectangle(_textRect, BackgroundColor);

                // Push clipping mask
                if (ClipChildren)
                {
                    GetDesireClientArea(out var clientArea);
                    Render2D.PushClip(ref clientArea);
                }

                DrawChildren();

                // Pop clipping mask
                if (ClipChildren)
                {
                    Render2D.PopClip();
                }

                // Name
                Render2D.DrawText(style.FontLarge, Title, _textRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);

                // Close button
                float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));

                // Rename button
                alpha = _renameButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Settings, _renameButtonRect, new Color(alpha));
            }

            /// <inheritdoc />
            public override bool CanSelect(ref Vector2 location)
            {
                return _dragAreaRect.MakeOffseted(Location).Contains(ref location);
            }

            /// <inheritdoc />
            public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
            {
                if (base.OnMouseDoubleClick(location, buttons))
                    return true;

                if (_renameButtonRect.Contains(ref location) || _closeButtonRect.Contains(ref location))
                    return true;

                Edit();
                return true;
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && !_dragAreaRect.Contains(ref location))
                {
                    _isMouseDown = true;
                    Cursor = CursorType.Hand;
                    Focus();
                    return true;
                }

                if (base.OnMouseDown(location, buttons))
                    return true;

                return false;
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    _isMouseDown = false;
                    Cursor = CursorType.Default;
                    Surface.ConnectingEnd(this);
                }

                if (base.OnMouseUp(location, buttons))
                    return true;

                // Rename
                if (_renameButtonRect.Contains(ref location))
                {
                    StartRenaming();
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                Surface.ConnectingOver(this);
                base.OnMouseMove(location);
            }

            /// <inheritdoc />
            public override void OnMouseLeave()
            {
                base.OnMouseLeave();

                if (_isMouseDown)
                {
                    _isMouseDown = false;
                    Cursor = CursorType.Default;

                    StartCreatingTransition();
                }
            }

            /// <inheritdoc />
            public override void RemoveConnections()
            {
                base.RemoveConnections();

                Transitions.Clear();
                SaveData();
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                if (IsDisposing)
                    return;

                ClearData();

                base.Dispose();
            }

            /// <inheritdoc />
            public string SurfaceName => StateTitle;

            /// <inheritdoc />
            public byte[] SurfaceData
            {
                get => (byte[])Values[1];
                set => SetValue(1, value);
            }

            /// <inheritdoc />
            public void OnContextCreated(VisjectSurfaceContext context)
            {
                context.Loaded += OnSurfaceLoaded;
            }

            private void OnSurfaceLoaded(VisjectSurfaceContext context)
            {
                // Ensure that loaded surface has output node for state
                var entryNode = context.FindNode(9, 21);
                if (entryNode == null)
                {
                    entryNode = context.SpawnNode(9, 21, new Vector2(100.0f));
                }
            }

            /// <inheritdoc />
            public override void DrawConnections()
            {
                var color = Color.White;
                for (int i = 0; i < Transitions.Count; i++)
                {
                    var targetState = Transitions[i].DestinationState;
                    var startPos = PointToParent(Size * 0.5f);
                    var endPos = targetState.PointToParent(targetState.Size * 0.5f);
                    DrawConnection(Surface, ref startPos, ref endPos, ref color);
                }
            }

            /// <inheritdoc />
            public Vector2 ConnectionOrigin => Center;

            /// <inheritdoc />
            public bool AreConnected(IConnectionInstigator other)
            {
                if (other is StateMachineState otherState)
                    return Transitions.Any(x => x.DestinationState == otherState);
                return false;
            }

            /// <inheritdoc />
            public bool CanConnectWith(IConnectionInstigator other)
            {
                if (other is StateMachineState otherState)
                {
                    // Can connect not connected states
                    return Transitions.All(x => x.DestinationState != otherState);
                }
                return false;
            }

            /// <inheritdoc />
            public void DrawConnectingLine(ref Vector2 startPos, ref Vector2 endPos, ref Color color)
            {
                DrawConnection(Surface, ref startPos, ref endPos, ref color);
            }

            /// <inheritdoc />
            public void Connect(IConnectionInstigator other)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// State machine transition data container object.
        /// </summary>
        /// <seealso cref="StateMachineState"/>
        /// <seealso cref="ISurfaceContext"/>
        public class StateMachineTransition : ISurfaceContext
        {
            /// <summary>
            /// The transition start state.
            /// </summary>
            public StateMachineState SourceState;

            /// <summary>
            /// The transition end state.
            /// </summary>
            public StateMachineState DestinationState;

            /// <summary>
            /// If checked, the transition can be triggered, otherwise it will be ignored.
            /// </summary>
            public bool Enabled;

            /// <summary>
            /// If checked, animation graph will ignore other transitions from the source state and use only this transition.
            /// </summary>
            public bool Solo;

            /// <summary>
            /// If checked, animation graph will perform automatic transition based on the state animation pose (single shot animation play).
            /// </summary>
            public bool UseDefaultRule;

            /// <summary>
            /// The transition order (higher first).
            /// </summary>
            public int Order;

            /// <summary>
            /// The blend duration (in seconds).
            /// </summary>
            public float BlendDuration;

            /// <summary>
            /// The blend mode.
            /// </summary>
            public AlphaBlendMode BlendMode;

            /// <summary>
            /// The rule graph data.
            /// </summary>
            public byte[] RuleGraph;

            /// <inheritdoc />
            public string SurfaceName => string.Format("{0} to {1}", SourceState.StateTitle, DestinationState.StateTitle);

            /// <inheritdoc />
            public byte[] SurfaceData
            {
                get => RuleGraph;
                set
                {
                    RuleGraph = value;
                    SourceState.SaveData();
                }
            }

            /// <inheritdoc />
            public void OnContextCreated(VisjectSurfaceContext context)
            {
                context.Loaded += OnSurfaceLoaded;
            }

            private void OnSurfaceLoaded(VisjectSurfaceContext context)
            {
                // Ensure that loaded surface has rule output node
                var ruleOutputNode = context.FindNode(9, 22);
                if (ruleOutputNode == null)
                {
                    ruleOutputNode = context.SpawnNode(9, 22, new Vector2(100.0f));
                }
            }
        }
    }
}
