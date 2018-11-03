// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Actions;
using FlaxEditor.Content;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Drag and drop scripts area control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class DragAreaControl : ContainerControl
    {
        private DragScriptItems _dragScriptItems;

        /// <summary>
        /// The parent scripts editor.
        /// </summary>
        public ScriptsEditor ScriptsEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAreaControl"/> class.
        /// </summary>
        public DragAreaControl()
        : base(0, 0, 120, 40)
        {
            CanFocus = false;

            // Add script button
            float addScriptButtonWidth = 60.0f;
            var addScriptButton = new Button((Width - addScriptButtonWidth) / 2, 1, addScriptButtonWidth, 18)
            {
                TooltipText = "Add new scripts to the actor",
                AnchorStyle = AnchorStyle.UpperCenter,
                Text = "Add script",
                Parent = this,
            };
            addScriptButton.ButtonClicked += AddScriptButtonOnClicked;
        }

        private void AddScriptButtonOnClicked(Button button)
        {
            var scripts = Editor.Instance.CodeEditing.GetScripts();
            if (scripts.Count == 0)
            {
                // No scripts
                var cm1 = new ContextMenu();
                cm1.AddButton("No scripts in project");
                cm1.Show(this, button.BottomLeft);
                return;
            }

            // Show context menu with list of scripts to add
            var cm = new ItemsListContextMenu(180);
            for (int i = 0; i < scripts.Count; i++)
            {
                var scriptType = scripts[i];
                cm.ItemsPanel.AddChild(new ItemsListContextMenu.Item(scriptType.Name, scriptType)
                {
                    TooltipText = scriptType.FullName,
                });
            }

            cm.ItemClicked += script => AddScript((Type)script.Tag);
            cm.SortChildren();
            cm.Show(this, button.BottomLeft);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var size = Size;

            // Info
            Render2D.DrawText(style.FontSmall, "Drag scripts here", new Rectangle(2, 22, size.X - 4, size.Y - 4 - 20), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);

            // Check if drag is over
            if (IsDragOver && _dragScriptItems != null && _dragScriptItems.HasValidDrag)
            {
                var area = new Rectangle(Vector2.Zero, size);
                Render2D.FillRectangle(area, Color.Orange * 0.5f);
                Render2D.DrawRectangle(area, Color.Black);
            }

            base.Draw();
        }

        private bool ValidateScript(ScriptItem scriptItem)
        {
            var scriptName = scriptItem.ScriptName;
            var scriptType = ScriptsBuilder.FindScript(scriptName);
            return scriptType != null;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            var result = base.OnDragEnter(ref location, data);

            if (_dragScriptItems == null)
                _dragScriptItems = new DragScriptItems(ValidateScript);
            if (_dragScriptItems.OnDragEnter(data))
                result = _dragScriptItems.Effect;

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);

            if (_dragScriptItems.HasValidDrag)
                result = _dragScriptItems.Effect;

            return result;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            _dragScriptItems.OnDragLeave();

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);

            if (_dragScriptItems.HasValidDrag)
            {
                result = _dragScriptItems.Effect;
                AddScripts(_dragScriptItems.Objects);
            }

            _dragScriptItems.OnDragDrop();

            return result;
        }

        private void AddScript(Type items)
        {
            var list = new List<Type>(1) { items };
            AddScripts(list);
        }

        private void AddScripts(List<ScriptItem> items)
        {
            var list = new List<Type>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var scriptName = item.ScriptName;
                var scriptType = ScriptsBuilder.FindScript(scriptName);
                if (scriptType == null)
                {
                    Editor.LogWarning("Invalid script type " + scriptName);
                }
                else
                {
                    list.Add(scriptType);
                }
            }
            AddScripts(list);
        }

        private void AddScripts(List<Type> items)
        {
            var actions = new List<IUndoAction>(4);

            for (int i = 0; i < items.Count; i++)
            {
                var scriptType = items[i];
                var actors = ScriptsEditor.ParentEditor.Values;
                for (int j = 0; j < actors.Count; j++)
                {
                    var actor = (Actor)actors[j];
                    actions.Add(AddRemoveScript.Add(actor, scriptType));
                }
            }

            if (actions.Count == 0)
            {
                Editor.LogWarning("Failed to spawn scripts");
                return;
            }

            var multiAction = new MultiUndoAction(actions);
            multiAction.Do();
            ScriptsEditor.Presenter?.Undo.AddAction(multiAction);
        }
    }

    /// <summary>
    /// Small image control added per script group that allows to drag and drop a reference to it. Also used to reorder the scripts.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Image" />
    internal class ScriptDragIcon : Image
    {
        private ScriptsEditor _editor;
        private bool _isMouseDown;
        private Vector2 _mouseDownPos;

        /// <summary>
        /// Gets the target script.
        /// </summary>
        public Script Script => (Script)Tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptDragIcon"/> class.
        /// </summary>
        /// <param name="editor">The script editor.</param>
        /// <param name="script">The target script.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="size">The size (both width and height).</param>
        public ScriptDragIcon(ScriptsEditor editor, Script script, float x, float y, float size)
        : base(x, y, size, size)
        {
            Tag = script;
            _editor = editor;
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            _mouseDownPos = Vector2.Minimum;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Check if start drag drop
            if (_isMouseDown)
            {
                DoDrag();
                _isMouseDown = false;
            }

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if start drag drop
            if (_isMouseDown && Vector2.Distance(location, _mouseDownPos) > 10.0f)
            {
                DoDrag();
                _isMouseDown = false;
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Clear flag
                _isMouseDown = false;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Set flag
                _isMouseDown = true;
                _mouseDownPos = location;
            }

            return base.OnMouseDown(location, buttons);
        }

        private void DoDrag()
        {
            var script = Script;
            _editor.OnScriptDragChange(true, script);
            DoDragDrop(DragScripts.GetDragData(script));
            _editor.OnScriptDragChange(false, script);
        }
    }

    internal class ScriptArrangeBar : Control
    {
        private ScriptsEditor _editor;
        private int _index;
        private Script _script;
        private DragDropEffect _dragEffect;

        public ScriptArrangeBar()
        : base(0, 0, 120, 6)
        {
            CanFocus = false;
            Visible = false;
        }

        public void Init(int index, ScriptsEditor editor)
        {
            _editor = editor;
            _index = index;
            _editor.ScriptDragChange += OnScriptDragChange;
        }

        private void OnScriptDragChange(bool start, Script script)
        {
            _script = start ? script : null;
            Visible = start;
            OnDragLeave();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var color = FlaxEngine.GUI.Style.Current.BackgroundSelected * (IsDragOver ? 0.9f : 0.1f);
            Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), color);
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            _dragEffect = DragDropEffect.None;

            var result = base.OnDragEnter(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (data is DragDataText textData && DragScripts.IsValidData(textData))
                return _dragEffect = DragDropEffect.Move;

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            return _dragEffect;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            _dragEffect = DragDropEffect.None;

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (_dragEffect != DragDropEffect.None)
            {
                result = _dragEffect;
                _dragEffect = DragDropEffect.None;

                _editor.ReorderScript(_script, _index);
            }

            return result;
        }
    }

    /// <summary>
    /// Custom editor for actor scripts collection.
    /// </summary>
    /// <seealso cref="CustomEditor" />
    public sealed class ScriptsEditor : SyncPointEditor
    {
        /// <summary>
        /// Delegate for script drag start and event events.
        /// </summary>
        /// <param name="start">Set to true if drag started, otherwise false.</param>
        /// <param name="script">The target script to reorder.</param>
        public delegate void ScriptDragDelegate(bool start, Script script);

        /// <summary>
        /// Occurs when script drag changes (starts or ends).
        /// </summary>
        public event ScriptDragDelegate ScriptDragChange;

        /// <summary>
        /// The scripts collection. Undo operations are recorder for scripts.
        /// </summary>
        private readonly List<Script> _scripts = new List<Script>();

        /// <inheritdoc />
        public override IEnumerable<object> UndoObjects => _scripts;

        private void AddMissingScript(int index, LayoutElementsContainer layout)
        {
            var group = layout.Group("Missing script");

            // Add settings button to the group
            const float settingsButtonSize = 14;
            var settingsButton = new Image(group.Panel.Width - settingsButtonSize, 0, settingsButtonSize, settingsButtonSize)
            {
                TooltipText = "Settings",
                CanFocus = true,
                AnchorStyle = AnchorStyle.UpperRight,
                IsScrollable = false,
                Color = new Color(0.7f),
                Margin = new Margin(1),
                Brush = new SpriteBrush(FlaxEngine.GUI.Style.Current.Settings),
                Tag = index,
                Parent = group.Panel
            };
            settingsButton.Clicked += MissingSettingsButtonOnClicked;
        }

        private void MissingSettingsButtonOnClicked(Image image, MouseButton mouseButton)
        {
            if (mouseButton != MouseButton.Left)
                return;

            var index = (int)image.Tag;

            var cm = new ContextMenu();
            cm.Tag = index;
            cm.AddButton("Remove", OnClickMissingRemove);
            cm.Show(image, image.Size);
        }

        private void OnClickMissingRemove(ContextMenuButton button)
        {
            var index = (int)button.ParentContextMenu.Tag;
            // TODO: support undo
            var actors = ParentEditor.Values;
            for (int i = 0; i < actors.Count; i++)
            {
                var actor = (Actor)actors[i];
                actor.DeleteScript(index);
                Editor.Instance.Scene.MarkSceneEdited(actor.Scene);
            }
        }

        /// <summary>
        /// Values container for the collection of the scripts. Helps with prefab linkage and reference value usage (uses Prefab Instance ID rather than index in array).
        /// </summary>
        public sealed class ScriptsContainer : ListValueContainer
        {
            private readonly Guid _prefabObjectId;

            /// <summary>
            /// Gets the prefab object identifier used by the container scripts. Empty if there is no valid linkage to the prefab object.
            /// </summary>
            public Guid PrefabObjectId => _prefabObjectId;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptsContainer"/> class.
            /// </summary>
            /// <param name="elementType">Type of the collection elements (script type).</param>
            /// <param name="index">The script index in the actor scripts collection.</param>
            /// <param name="values">The collection values (scripts array).</param>
            public ScriptsContainer(Type elementType, int index, ValueContainer values)
            : base(elementType, index)
            {
                Capacity = values.Count;
                for (int i = 0; i < values.Count; i++)
                {
                    var v = (IList)values[i];
                    Add(v[index]);
                }

                if (values.HasReferenceValue && Count > 0 && this[0] is Script script && script.HasPrefabLink)
                {
                    _prefabObjectId = script.PrefabObjectID;
                    RefreshReferenceValue(values.ReferenceValue);
                }
            }

            /// <inheritdoc />
            public override void RefreshReferenceValue(object instanceValue)
            {
                // Clear
                _referenceValue = null;
                _hasReferenceValue = false;

                if (instanceValue is IList v)
                {
                    // Get the reference value if script with the given link id exists in the reference values collection
                    for (int i = 0; i < v.Count; i++)
                    {
                        if (v[i] is Script script && script.PrefabObjectID == _prefabObjectId)
                        {
                            _referenceValue = script;
                            _hasReferenceValue = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _scripts.Clear();

            // Area for drag&drop scripts
            var dragArea = layout.CustomContainer<DragAreaControl>();
            dragArea.CustomControl.ScriptsEditor = this;

            // No support to show scripts for more than one actor selected
            // TODO: support showing scripts from objects that has the same scripts layout
            if (Values.Count != 1)
                return;

            // Scripts arrange bar
            var dragBar = layout.Custom<ScriptArrangeBar>();
            dragBar.CustomControl.Init(0, this);

            // Scripts
            var scripts = (Script[])Values[0];
            _scripts.AddRange(scripts);
            var elementType = typeof(Script);
            for (int i = 0; i < scripts.Length; i++)
            {
                var script = scripts[i];
                if (script == null)
                {
                    AddMissingScript(i, layout);
                    continue;
                }

                var values = new ScriptsContainer(elementType, i, Values);
                var type = script.GetType();
                var editor = CustomEditorsUtil.CreateEditor(type, false);

                // Create group
                var title = CustomEditorsUtil.GetPropertyNameUI(type.Name);
                var group = layout.Group(title);
                group.Panel.Open(false);

                // Customize
                var typeAttributes = type.GetCustomAttributes(true);
                var tooltip = (TooltipAttribute)typeAttributes.FirstOrDefault(x => x is TooltipAttribute);
                if (tooltip != null)
                    group.Panel.TooltipText = tooltip.Text;
                if (script.HasPrefabLink)
                    group.Panel.HeaderTextColor = FlaxEngine.GUI.Style.Current.ProgressNormal;

                // Add toggle button to the group
                var scriptToggle = new CheckBox(2, 0, script.Enabled)
                {
                    TooltipText = "If checked, script will be enabled",
                    IsScrollable = false,
                    Size = new Vector2(14, 14),
                    BoxSize = 12.0f,
                    Tag = script,
                    Parent = group.Panel
                };
                scriptToggle.StateChanged += ScriptToggleOnCheckChanged;

                // Add drag button to the group
                const float dragIconSize = 14;
                var scriptDrag = new ScriptDragIcon(this, script, scriptToggle.Right, 0.5f, dragIconSize)
                {
                    TooltipText = "Script reference",
                    CanFocus = true,
                    IsScrollable = false,
                    Color = new Color(0.7f),
                    Margin = new Margin(1),
                    Brush = new SpriteBrush(Editor.Instance.Icons.DragBar12),
                    Tag = script,
                    Parent = group.Panel
                };

                // Add settings button to the group
                const float settingsButtonSize = 14;
                var settingsButton = new Image(group.Panel.Width - settingsButtonSize, 0, settingsButtonSize, settingsButtonSize)
                {
                    TooltipText = "Settings",
                    CanFocus = true,
                    AnchorStyle = AnchorStyle.UpperRight,
                    IsScrollable = false,
                    Color = new Color(0.7f),
                    Margin = new Margin(1),
                    Brush = new SpriteBrush(FlaxEngine.GUI.Style.Current.Settings),
                    Tag = script,
                    Parent = group.Panel
                };
                settingsButton.Clicked += SettingsButtonOnClicked;

                group.Panel.HeaderTextMargin = new Margin(scriptDrag.Right, 15, 2, 2);
                group.Object(values, editor);

                // Scripts arrange bar
                dragBar = layout.Custom<ScriptArrangeBar>();
                dragBar.CustomControl.Init(i + 1, this);
            }

            base.Initialize(layout);
        }

        /// <summary>
        /// Called when script drag changes.
        /// </summary>
        /// <param name="start">if set to <c>true</c> drag just started, otherwise ended.</param>
        /// <param name="script">The target script.</param>
        public void OnScriptDragChange(bool start, Script script)
        {
            ScriptDragChange.Invoke(start, script);
        }

        /// <summary>
        /// Changes the script order (with undo).
        /// </summary>
        /// <param name="script">The script to reorder.</param>
        /// <param name="targetIndex">The target index to move script.</param>
        public void ReorderScript(Script script, int targetIndex)
        {
            // Skip if no change
            if (script.OrderInParent == targetIndex)
                return;

            var action = ChangeScriptAction.ChangeOrder(script, targetIndex);
            action.Do();
            Presenter?.Undo.AddAction(action);
        }

        private void ScriptToggleOnCheckChanged(CheckBox box)
        {
            var script = (Script)box.Tag;
            script.Enabled = box.Checked;
        }

        private void SettingsButtonOnClicked(Image image, MouseButton mouseButton)
        {
            if (mouseButton != MouseButton.Left)
                return;

            var script = (Script)image.Tag;

            var cm = new ContextMenu();
            cm.Tag = script;
            //cm.AddButton("Reset").Enabled = false;// TODO: finish this
            //cm.AddSeparator();
            cm.AddButton("Remove", OnClickRemove);
            cm.AddButton("Move up", OnClickMoveUp).Enabled = script.OrderInParent > 0;
            cm.AddButton("Move down", OnClickMoveDown).Enabled = script.OrderInParent < script.Actor.Scripts.Length - 1;
            // TODO: copy script
            // TODO: paste script values
            // TODO: paste script as new
            // TODO: copy script reference
            cm.AddSeparator();
            cm.AddButton("Copy name", OnClickCopyName);
            cm.AddButton("Edit script", OnClickEditScript);
            cm.AddButton("Show in content window", OnClickShowScript);
            cm.Show(image, image.Size);
        }

        private void OnClickRemove(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            var action = AddRemoveScript.Remove(script);
            action.Do();
            Presenter.Undo?.AddAction(action);
        }

        private void OnClickMoveUp(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            var action = ChangeScriptAction.ChangeOrder(script, script.OrderInParent - 1);
            action.Do();
            Presenter.Undo?.AddAction(action);
        }

        private void OnClickMoveDown(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            var action = ChangeScriptAction.ChangeOrder(script, script.OrderInParent + 1);
            action.Do();
            Presenter.Undo?.AddAction(action);
        }

        private void OnClickCopyName(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            Application.ClipboardText = script.GetType().FullName;
        }

        private void OnClickEditScript(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            var item = Editor.Instance.ContentDatabase.FindScriptWitScriptName(script);
            if (item != null)
                Editor.Instance.ContentEditing.Open(item);
        }

        private void OnClickShowScript(ContextMenuButton button)
        {
            var script = (Script)button.ParentContextMenu.Tag;
            var item = Editor.Instance.ContentDatabase.FindScriptWitScriptName(script);
            if (item != null)
                Editor.Instance.Windows.ContentWin.Select(item);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (Values.Count == 1)
            {
                var scripts = ((Actor)ParentEditor.Values[0]).Scripts;
                if (!Utils.ArraysEqual(scripts, _scripts))
                {
                    ParentEditor.RebuildLayout();
                    return;
                }
            }

            base.Refresh();
        }
    }
}
