// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            public StateMachine(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
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
            public StateMachineEntry(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
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
            public override void DrawConnections(ref Vector2 mousePosition)
            {
                var targetState = FirstState;
                if (targetState != null)
                {
                    // Draw the connection
                    var center = Size * 0.5f;
                    var startPos = PointToParent(ref center);
                    targetState.GetConnectionEndPoint(ref startPos, out var endPos);
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

            /// <summary>
            /// The transitions rectangle (in surface-space).
            /// </summary>
            public Rectangle TransitionsRectangle;

            /// <inheritdoc />
            public StateMachineState(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                TransitionsRectangle = Rectangle.Empty;
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
                    Render2D.DrawSprite(surface.Style.Icons.ArrowClose, arrowRect, color);
                    Render2D.PopTransform();

                    endPos -= dir * 4.0f;
                }
                Render2D.DrawLine(startPos, endPos, color, 2.2f);
            }

            /// <summary>
            /// Gets the connection end point for the given input position. Puts the end point near the edge of the node bounds.
            /// </summary>
            /// <param name="startPos">The start position (in surface space).</param>
            /// <param name="endPos">The end position (in surface space).</param>
            public void GetConnectionEndPoint(ref Vector2 startPos, out Vector2 endPos)
            {
                var bounds = new Rectangle(Vector2.Zero, Size);
                bounds.Expand(4.0f);
                var upperLeft = bounds.UpperLeft;
                var bottomRight = bounds.BottomRight;
                bounds = Rectangle.FromPoints(PointToParent(ref upperLeft), PointToParent(ref bottomRight));
                CollisionsHelper.ClosestPointRectanglePoint(ref bounds, ref startPos, out endPos);
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

                // Register for surface mouse events to handle transition arrows interactions
                Surface.CustomMouseUp += OnSurfaceMouseUp;
                Surface.CustomMouseDoubleClick += OnSurfaceMouseDoubleClick;
            }

            private void OnSurfaceMouseUp(ref Vector2 mouse, MouseButton buttons, ref bool handled)
            {
                if (handled)
                    return;

                // Check click over the connection
                var mousePosition = Surface.SurfaceRoot.PointFromParent(ref mouse);
                if (!TransitionsRectangle.Contains(ref mousePosition))
                    return;
                for (int i = 0; i < Transitions.Count; i++)
                {
                    var t = Transitions[i];
                    if (t.Bounds.Contains(ref mousePosition))
                    {
                        CollisionsHelper.ClosestPointPointLine(ref mousePosition, ref t.StartPos, ref t.EndPos, out var point);
                        if (Vector2.DistanceSquared(ref mousePosition, ref point) < 25.0f)
                        {
                            OnTransitionClicked(t, ref mouse, ref mousePosition, buttons);
                            handled = true;
                            return;
                        }
                    }
                }
            }

            private void OnSurfaceMouseDoubleClick(ref Vector2 mouse, MouseButton buttons, ref bool handled)
            {
                if (handled)
                    return;

                // Check double click over the connection
                var mousePosition = Surface.SurfaceRoot.PointFromParent(ref mouse);
                if (!TransitionsRectangle.Contains(ref mousePosition))
                    return;
                for (int i = 0; i < Transitions.Count; i++)
                {
                    var t = Transitions[i];
                    if (t.Bounds.Contains(ref mousePosition))
                    {
                        CollisionsHelper.ClosestPointPointLine(ref mousePosition, ref t.StartPos, ref t.EndPos, out var point);
                        if (Vector2.DistanceSquared(ref mousePosition, ref point) < 25.0f)
                        {
                            t.EditRule();
                            handled = true;
                            return;
                        }
                    }
                }
            }

            private void OnTransitionClicked(StateMachineTransition transition, ref Vector2 mouse, ref Vector2 mousePosition, MouseButton buttons)
            {
                switch (buttons)
                {
                case MouseButton.Left:
                    transition.Edit();
                    break;
                case MouseButton.Right:
                    var contextMenu = new FlaxEngine.GUI.ContextMenu();
                    contextMenu.AddButton("Edit").Clicked += transition.Edit;
                    contextMenu.AddSeparator();
                    contextMenu.AddButton("Delete").Clicked += transition.Delete;
                    contextMenu.AddButton("Select source state").Clicked += transition.SelectSourceState;
                    contextMenu.AddButton("Select destination state").Clicked += transition.SelectDestinationState;
                    contextMenu.Show(Surface, mouse);
                    break;
                case MouseButton.Middle:
                    transition.Delete();
                    break;
                }
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

                var bytes = StateData;
                if (bytes == null || bytes.Length == 0)
                {
                    // Empty state
                    return;
                }

                try
                {
                    StateMachineTransition.Data data;
                    using (var stream = new MemoryStream(bytes))
                    using (var reader = new BinaryReader(stream))
                    {
                        int version = reader.ReadInt32();
                        if (version != 1)
                        {
                            Editor.LogError("Invalid state machine state data version.");
                            return;
                        }

                        int tCount = reader.ReadInt32();
                        Transitions.Capacity = Mathf.Max(Transitions.Capacity, tCount);

                        for (int i = 0; i < tCount; i++)
                        {
                            data.Destination = reader.ReadUInt32();
                            data.Flags = (StateMachineTransition.Data.FlagTypes)reader.ReadInt32();
                            data.Order = reader.ReadInt32();
                            data.BlendDuration = reader.ReadSingle();
                            data.BlendMode = (AlphaBlendMode)reader.ReadInt32();
                            data.Unused0 = reader.ReadInt32();
                            data.Unused1 = reader.ReadInt32();
                            data.Unused2 = reader.ReadInt32();

                            int ruleSize = reader.ReadInt32();
                            byte[] rule = null;
                            if (ruleSize != 0)
                                rule = reader.ReadBytes(ruleSize);

                            var destination = Context.FindNode(data.Destination) as StateMachineState;
                            if (destination == null)
                            {
                                Editor.LogWarning("Missing state machine state destination node.");
                                continue;
                            }

                            var t = new StateMachineTransition(this, destination, ref data, rule);
                            Transitions.Add(t);
                        }
                    }
                }
                finally
                {
                    UpdateTransitionsOrder();
                    UpdateTransitions();
                    UpdateTransitionsColors();
                }
            }

            /// <summary>
            /// Saves the state data to the node value (writes transitions and related information).
            /// </summary>
            public void SaveData()
            {
                try
                {
                    _isSavingData = true;

                    if (Transitions.Count == 0)
                    {
                        StateData = Enumerable.Empty<byte>() as byte[];
                    }
                    else
                    {
                        StateMachineTransition.Data data;
                        using (var stream = new MemoryStream(512))
                        using (var writer = new BinaryWriter(stream))
                        {
                            writer.Write(1);
                            writer.Write(Transitions.Count);
                            for (int i = 0; i < Transitions.Count; i++)
                            {
                                var t = Transitions[i];
                                t.GetData(out data);
                                var rule = t.RuleGraph;

                                writer.Write(data.Destination);
                                writer.Write((int)data.Flags);
                                writer.Write(data.Order);
                                writer.Write(data.BlendDuration);
                                writer.Write((int)data.BlendMode);
                                writer.Write(data.Unused0);
                                writer.Write(data.Unused1);
                                writer.Write(data.Unused2);

                                if (rule == null || rule.Length == 0)
                                {
                                    writer.Write(0);
                                }
                                else
                                {
                                    writer.Write(rule.Length);
                                    writer.Write(rule);
                                }
                            }

                            StateData = stream.ToArray();
                        }
                    }
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
                TransitionsRectangle = Rectangle.Empty;
            }

            /// <summary>
            /// Opens the state editing UI.
            /// </summary>
            public void Edit()
            {
                Surface.OpenContext(this);
            }

            private bool IsSoloAndEnabled(StateMachineTransition t)
            {
                return t.Solo && t.Enabled;
            }

            /// <summary>
            /// Updates the transitions order in the list vy using the <see cref="StateMachineTransition.Order"/> property.
            /// </summary>
            public void UpdateTransitionsOrder()
            {
                Transitions.Sort((a, b) => b.Order - a.Order);
            }

            /// <summary>
            /// Updates the transitions colors (for disabled/enabled/solo transitions matching).
            /// </summary>
            public void UpdateTransitionsColors()
            {
                if (Transitions.Count == 0)
                    return;

                bool anySolo = Transitions.Any(IsSoloAndEnabled);
                if (anySolo)
                {
                    var firstSolo = Transitions.First(IsSoloAndEnabled);
                    for (int i = 0; i < Transitions.Count; i++)
                    {
                        var t = Transitions[i];
                        t.LineColor = t == firstSolo ? Color.White : Color.Gray;
                    }
                }
                else
                {
                    for (int i = 0; i < Transitions.Count; i++)
                    {
                        var t = Transitions[i];
                        t.LineColor = t.Enabled ? Color.White : Color.Gray;
                    }
                }
            }

            /// <summary>
            /// Updates the transitions rectangles.
            /// </summary>
            public void UpdateTransitions()
            {
                for (int i = 0; i < Transitions.Count; i++)
                {
                    var t = Transitions[i];
                    var sourceState = this;
                    var targetState = t.DestinationState;
                    var isBothDirection = targetState.Transitions.Any(x => x.DestinationState == this);

                    Vector2 startPos, endPos;
                    if (isBothDirection)
                    {
                        bool diff = string.Compare(sourceState.Title, targetState.Title, StringComparison.Ordinal) > 0;
                        var s1 = diff ? sourceState : targetState;
                        var s2 = diff ? targetState : sourceState;

                        // Two aligned arrows in the opposite direction
                        var center = s1.Size * 0.5f;
                        startPos = s1.PointToParent(ref center);
                        s2.GetConnectionEndPoint(ref startPos, out endPos);
                        s1.GetConnectionEndPoint(ref endPos, out startPos);

                        // Offset a little to not overlap
                        var offset = diff ? -6.0f : 6.0f;
                        var dir = startPos - endPos;
                        dir.Normalize();
                        Vector2.Perpendicular(ref dir, out var nrm);
                        nrm *= offset;
                        startPos += nrm;
                        endPos += nrm;

                        // Swap fo the other arrow
                        if (!diff)
                        {
                            var tmp = startPos;
                            startPos = endPos;
                            endPos = tmp;
                        }
                    }
                    else
                    {
                        // Single connection over the closest path
                        var center = Size * 0.5f;
                        startPos = PointToParent(ref center);
                        targetState.GetConnectionEndPoint(ref startPos, out endPos);
                        sourceState.GetConnectionEndPoint(ref endPos, out startPos);
                    }

                    t.StartPos = startPos;
                    t.EndPos = endPos;
                    Rectangle.FromPoints(ref startPos, ref endPos, out t.Bounds);
                    t.Bounds.Expand(10.0f);
                }

                if (Transitions.Count > 0)
                {
                    TransitionsRectangle = Transitions[0].Bounds;
                    for (int i = 1; i < Transitions.Count; i++)
                    {
                        Rectangle.Union(ref TransitionsRectangle, ref Transitions[i].Bounds, out TransitionsRectangle);
                    }
                }
                else
                {
                    TransitionsRectangle = Rectangle.Empty;
                }
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
            public override void Update(float deltaTime)
            {
                base.Update(deltaTime);

                // TODO: maybe update only on actual transitions change?
                UpdateTransitions();
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
            public override void DrawConnections(ref Vector2 mousePosition)
            {
                for (int i = 0; i < Transitions.Count; i++)
                {
                    var t = Transitions[i];
                    var isMouseOver = t.Bounds.Contains(ref mousePosition);
                    if (isMouseOver)
                    {
                        CollisionsHelper.ClosestPointPointLine(ref mousePosition, ref t.StartPos, ref t.EndPos, out var point);
                        isMouseOver = Vector2.DistanceSquared(ref mousePosition, ref point) < 25.0f;
                    }
                    var color = isMouseOver ? Color.Wheat : t.LineColor;
                    DrawConnection(Surface, ref t.StartPos, ref t.EndPos, ref color);
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
                var state = (StateMachineState)other;

                // Create a new transition
                var data = new StateMachineTransition.Data
                {
                    Flags = StateMachineTransition.Data.FlagTypes.Enabled,
                    Order = 0,
                    BlendDuration = 0.1f,
                    BlendMode = AlphaBlendMode.HermiteCubic,
                };
                var transition = new StateMachineTransition(this, state, ref data);
                Transitions.Add(transition);

                UpdateTransitionsOrder();
                UpdateTransitions();
                UpdateTransitionsColors();

                SaveData();
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
            /// The packed data container for the transition data storage. Helps with serialization and versioning the data.
            /// </summary>
            /// <remarks>
            /// It does not store GC objects references to make it more lightweight. Transition rule bytes data is stores in a separate way.
            /// </remarks>
            [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 32)]
            public struct Data
            {
                /// <summary>
                /// The transition flag types.
                /// </summary>
                [Flags]
                public enum FlagTypes
                {
                    /// <summary>
                    /// The none.
                    /// </summary>
                    None = 0,

                    /// <summary>
                    /// The enabled flag.
                    /// </summary>
                    Enabled = 1,

                    /// <summary>
                    /// The solo flag.
                    /// </summary>
                    Solo = 2,

                    /// <summary>
                    /// The use default rule flag.
                    /// </summary>
                    UseDefaultRule = 4,
                }

                /// <summary>
                /// The destination state node ID.
                /// </summary>
                public uint Destination;

                /// <summary>
                /// The flags.
                /// </summary>
                public FlagTypes Flags;

                /// <summary>
                /// The order.
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
                /// The unused data 0.
                /// </summary>
                public int Unused0;

                /// <summary>
                /// The unused data 1.
                /// </summary>
                public int Unused1;

                /// <summary>
                /// The unused data 2.
                /// </summary>
                public int Unused2;

                /// <summary>
                /// Determines whether the data has a given flag set.
                /// </summary>
                /// <param name="flag">The flag.</param>
                /// <returns><c>true</c> if the specified flag is set; otherwise, <c>false</c>.</returns>
                public bool HasFlag(FlagTypes flag)
                {
                    return (Flags & flag) == flag;
                }

                /// <summary>
                /// Sets the flag to the given value.
                /// </summary>
                /// <param name="flag">The flag.</param>
                /// <param name="value">If set to <c>true</c> the flag will be set, otherwise it will be cleared.</param>
                public void SetFlag(FlagTypes flag, bool value)
                {
                    if (value)
                        Flags |= flag;
                    else
                        Flags &= ~flag;
                }
            }

            private Data _data;
            private byte[] _ruleGraph;

            /// <summary>
            /// The transition start state.
            /// </summary>
            [HideInEditor]
            public readonly StateMachineState SourceState;

            /// <summary>
            /// The transition end state.
            /// </summary>
            [HideInEditor]
            public readonly StateMachineState DestinationState;

            /// <summary>
            /// If checked, the transition can be triggered, otherwise it will be ignored.
            /// </summary>
            [EditorOrder(10), DefaultValue(true), Tooltip("If checked, the transition can be triggered, otherwise it will be ignored.")]
            public bool Enabled
            {
                get => _data.HasFlag(Data.FlagTypes.Enabled);
                set
                {
                    _data.SetFlag(Data.FlagTypes.Enabled, value);
                    SourceState.UpdateTransitionsColors();
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// If checked, animation graph will ignore other transitions from the source state and use only this transition.
            /// </summary>
            [EditorOrder(20), DefaultValue(false), Tooltip("If checked, animation graph will ignore other transitions from the source state and use only this transition.")]
            public bool Solo
            {
                get => _data.HasFlag(Data.FlagTypes.Solo);
                set
                {
                    _data.SetFlag(Data.FlagTypes.Solo, value);
                    SourceState.UpdateTransitionsColors();
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// If checked, animation graph will perform automatic transition based on the state animation pose (single shot animation play).
            /// </summary>
            [EditorOrder(30), DefaultValue(false), Tooltip("If checked, animation graph will perform automatic transition based on the state animation pose (single shot animation play).")]
            public bool UseDefaultRule
            {
                get => _data.HasFlag(Data.FlagTypes.UseDefaultRule);
                set
                {
                    _data.SetFlag(Data.FlagTypes.UseDefaultRule, value);
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// The transition order (higher first).
            /// </summary>
            [EditorOrder(40), DefaultValue(0), Tooltip("The transition order. Transitions with the higher order are handled before the ones with the lower order.")]
            public int Order
            {
                get => _data.Order;
                set
                {
                    _data.Order = value;
                    SourceState.UpdateTransitionsOrder();
                    SourceState.UpdateTransitionsColors();
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// The blend duration (in seconds).
            /// </summary>
            [EditorOrder(50), DefaultValue(0.1f), Limit(0, 20.0f, 0.1f), Tooltip("Transition blend duration (in seconds).")]
            public float BlendDuration
            {
                get => _data.BlendDuration;
                set
                {
                    _data.BlendDuration = value;
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// The blend mode.
            /// </summary>
            [EditorOrder(60), DefaultValue(AlphaBlendMode.HermiteCubic), Tooltip("Transition blending mode for blend alpha.")]
            public AlphaBlendMode BlendMode
            {
                get => _data.BlendMode;
                set
                {
                    _data.BlendMode = value;
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// The rule graph data.
            /// </summary>
            [HideInEditor]
            public byte[] RuleGraph
            {
                get => _ruleGraph;
                set
                {
                    _ruleGraph = value ?? Enumerable.Empty<byte>() as byte[];
                    SourceState.SaveData();
                }
            }

            /// <summary>
            /// The start position (cached).
            /// </summary>
            [HideInEditor]
            public Vector2 StartPos;

            /// <summary>
            /// The end position (cached).
            /// </summary>
            [HideInEditor]
            public Vector2 EndPos;

            /// <summary>
            /// The bounds of the transition connection line (cached).
            /// </summary>
            [HideInEditor]
            public Rectangle Bounds;

            /// <summary>
            /// The color of the transition connection line (cached).
            /// </summary>
            [HideInEditor]
            public Color LineColor;

            /// <summary>
            /// Initializes a new instance of the <see cref="StateMachineTransition"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="destination">The destination.</param>
            /// <param name="data">The transition data container.</param>
            /// <param name="ruleGraph">The transition rule graph. Can be null.</param>
            public StateMachineTransition(StateMachineState source, StateMachineState destination, ref Data data, byte[] ruleGraph = null)
            {
                SourceState = source;
                DestinationState = destination;
                _data = data;
                _data.Destination = destination.ID;
                _ruleGraph = ruleGraph ?? Enumerable.Empty<byte>() as byte[];
            }

            /// <summary>
            /// Gets the transition data.
            /// </summary>
            /// <param name="data">The data.</param>
            public void GetData(out Data data)
            {
                data = _data;
            }

            /// <inheritdoc />
            public string SurfaceName => string.Format("{0} to {1}", SourceState.StateTitle, DestinationState.StateTitle);

            /// <inheritdoc />
            [HideInEditor]
            public byte[] SurfaceData
            {
                get => RuleGraph;
                set => RuleGraph = value;
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

                    // TODO: add default rule nodes for easier usage
                }
            }

            /// <summary>
            /// Removes the transition.
            /// </summary>
            public void Delete()
            {
                SourceState.Transitions.Remove(this);

                SourceState.UpdateTransitionsOrder();
                SourceState.UpdateTransitions();
                SourceState.UpdateTransitionsColors();

                SourceState.SaveData();
            }

            /// <summary>
            /// Selects the source state node of the transition
            /// </summary>
            public void SelectSourceState()
            {
                SourceState.Surface.Select(SourceState);
            }

            /// <summary>
            /// Selects the destination state node of the transition
            /// </summary>
            public void SelectDestinationState()
            {
                DestinationState.Surface.Select(DestinationState);
            }

            /// <summary>
            /// Opens the transition editor popup.
            /// </summary>
            public void Edit()
            {
                var surface = SourceState.Surface;
                var center = Bounds.Center + new Vector2(3.0f);
                var editor = new TransitionEditor(this);
                editor.Show(surface, surface.SurfaceRoot.PointToParent(ref center));
            }

            /// <summary>
            /// Opens the transition rule editing UI.
            /// </summary>
            public void EditRule()
            {
                SourceState.Surface.OpenContext(this);
            }
        }
    }
}
