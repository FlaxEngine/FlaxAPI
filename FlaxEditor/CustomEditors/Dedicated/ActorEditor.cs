////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Actions;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Deciated custom editor for <see cref="Actor"/> objects.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
    [CustomEditor(typeof(Actor)), DefaultEditor]
    public class ActorEditor : GenericEditor
    {
        /// <summary>
        /// Drag and drop scripts area control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Control" />
        public class DragAreaControl : Control
        {
            private DragScripts _dragScripts;

            /// <summary>
            /// The parent scripts editor.
            /// </summary>
            public ScriptsEditor ScriptsEditor;

            /// <summary>
            /// Initializes a new instance of the <see cref="DragAreaControl"/> class.
            /// </summary>
            public DragAreaControl()
                : base(false, 0, 0, 120, 32)
            {
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = FlaxEngine.GUI.Style.Current;
                var size = Size;

                // Info
                Render2D.DrawText(style.FontSmall, "Drag scripts here", new Rectangle(2, 2, size.X - 4, size.Y - 4), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords);

                // Check if drag is over
                if (IsDragOver && _dragScripts != null && _dragScripts.HasValidDrag)
                {
                    var area = new Rectangle(Vector2.Zero, size);
                    Render2D.FillRectangle(area, Color.Orange * 0.5f, true);
                    Render2D.DrawRectangle(area, Color.Black);
                }
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

                if (_dragScripts == null)
                    _dragScripts = new DragScripts();
                if (_dragScripts.OnDragEnter(data, ValidateScript))
                    result = _dragScripts.Effect;

                return result;
            }

            /// <inheritdoc />
            public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
            {
                var result = base.OnDragMove(ref location, data);

                if (_dragScripts.HasValidDrag)
                    result = _dragScripts.Effect;

                return result;
            }

            /// <inheritdoc />
            public override void OnDragLeave()
            {
                _dragScripts.OnDragLeave();

                base.OnDragLeave();
            }

            /// <inheritdoc />
            public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
            {
                var result = base.OnDragDrop(ref location, data);

                if (_dragScripts.HasValidDrag)
                {
                    result = _dragScripts.Effect;

                    var actions = new List<IUndoAction>(4);

                    for (int i = 0; i < _dragScripts.Objects.Count; i++)
                    {
                        var item = _dragScripts.Objects[i];
                        var scriptName = item.ScriptName;
                        var scriptType = ScriptsBuilder.FindScript(scriptName);

                        var actors = ScriptsEditor.ParentEditor.Values;
                        for (int j = 0; j < actors.Count; j++)
                        {
                            var actor = (Actor)actors[j];
                            actions.Add(AddRemoveScript.Add(actor, scriptType));
                        }
                    }

                    var multiAction = new MultiUndoAction(actions);
                    multiAction.Do();
                    Editor.Instance.Undo.AddAction(multiAction);
                }

                _dragScripts.OnDragDrop();

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
            /// The scripts collection. Undo operations are recorder for scripts.
            /// </summary>
            private readonly List<Script> _scripts = new List<Script>();

            /// <inheritdoc />
            public override IEnumerable<object> UndoObjects => _scripts;

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                _scripts.Clear();

                // Area for drag&drop scripts
                var dragArea = layout.Custom<DragAreaControl>();
                dragArea.CustomControl.ScriptsEditor = this;

                // No support to show scripts for more than one actor selected
                if (Values.Count != 1)
                    return;

                // Scripts
                var scripts = (Script[])Values[0];
                _scripts.AddRange(scripts);
                var elementType = typeof(Script);
                for (int i = 0; i < scripts.Length; i++)
                {
                    var script = scripts[i];
                    var values = new ListValueContainer(elementType, i, Values);
                    var type = script.GetType();
                    var editor = CustomEditorsUtil.CreateEditor(type, false);

                    // Create group
                    var title = CustomEditorsUtil.GetPropertyNameUI(type.Name);
                    var group = layout.Group(title);

                    // Add settings button to the group
                    const float settingsButtonSize = 14;
                    var settingsButton = new Image(true, group.Panel.Width - settingsButtonSize, 0, settingsButtonSize, settingsButtonSize)
                    {
                        AnchorStyle = AnchorStyle.UpperRight,
                        IsScrollable = false,
                        Color = new Color(0.7f),
                        Margin = new Margin(1),
                        ImageSource = new SpriteImageSource(FlaxEngine.GUI.Style.Current.Settings),
                        Tag = script,
                        Parent = group.Panel
                    };
                    settingsButton.Clicked += SettingsButtonOnClicked;

                    group.Object(values, editor);
                }

                base.Initialize(layout);
            }

            private void SettingsButtonOnClicked(Image image, MouseButtons mouseButtons)
            {
                if (mouseButtons != MouseButtons.Left)
                    return;

                var script = (Script)image.Tag;

                var cm = new ContextMenu();
                cm.Tag = script;
                cm.OnButtonClicked += SettingsMenuOnButtonClicked;
                cm.AddButton(0, "Reset").Enabled = false;// TODO: finish this
                cm.AddSeparator();
                cm.AddButton(1, "Remove");
                cm.AddButton(2, "Move up").Enabled = script.OrderInParent > 0;
                cm.AddButton(3, "Move down").Enabled = script.OrderInParent < script.Actor.Scripts.Length - 1;
                // TODO: copy script
                // TODO: paste script values
                // TODO: paste script as new
                // TODO: copt script reference
                cm.AddSeparator();
                cm.AddButton(4, "Edit script");
                cm.AddButton(5, "Show in content window");
                cm.Show(image, image.Size);
            }

            private void SettingsMenuOnButtonClicked(int id, ContextMenu contextMenu)
            {
                var script = (Script)contextMenu.Tag;
                switch (id)
                {
                    // Reset
                    case 0:
                    {
                        throw new NotImplementedException("Reset script");
                        break;
                    }

                    // Remove
                    case 1:
                        var action = AddRemoveScript.Remove(script);
                        action.Do();
                        Editor.Instance.Undo.AddAction(action);
                        break;

                    // Move up
                    case 2:
                        script.OrderInParent -= 1;
                        break;

                    // Move down
                    case 3:
                        script.OrderInParent += 1;
                        break;

                    // Edit script
                    case 4:
                    {
                        var item = Editor.Instance.ContentDatabase.FindScriptWitScriptName(script);
                        if (item != null)
                            Editor.Instance.ContentEditing.Open(item);
                        break;
                    }

                    // Show in content window
                    case 5:
                    {
                        var item = Editor.Instance.ContentDatabase.FindScriptWitScriptName(script);
                        if (item != null)
                            Editor.Instance.Windows.ContentWin.Select(item);
                        break;
                    }
                }
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

        /// <inheritdoc />
        protected override List<ItemInfo> GetItemsForType(Type type)
        {
            var items = base.GetItemsForType(type);

            // Inject scripts editor
            var scriptsMember = type.GetProperty("Scripts");
            if (scriptsMember != null)
            {
                var item = new ItemInfo(scriptsMember);
                item.CustomEditor = new CustomEditorAttribute(typeof(ScriptsEditor));
                items.Add(item);
            }

            return items;
        }
    }
}
