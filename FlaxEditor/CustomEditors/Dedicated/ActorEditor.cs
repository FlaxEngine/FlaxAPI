////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;

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

                    for (int i = 0; i < _dragScripts.Objects.Count; i++)
                    {
                        var item = _dragScripts.Objects[i];
                        var scriptName = item.ScriptName;
                        var scriptType = ScriptsBuilder.FindScript(scriptName);

                        var actors = ScriptsEditor.ParentEditor.Values;
                        for (int j = 0; j < actors.Count; j++)
                        {
                            var actor = (Actor)actors[j];

                            var script = (Script)FlaxEngine.Object.New(scriptType);
                            actor.AddScript(script);
                        }
                    }

                    ScriptsEditor.ParentEditor.RebuildLayout();
                }

                _dragScripts.OnDragDrop();

                return result;
            }
        }

        /// <summary>
        /// Custom editor for actor scripts collection.
        /// </summary>
        /// <seealso cref="CustomEditor" />
        public sealed class ScriptsEditor : CustomEditor
        {
            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                // Area for drag&drop scripts
                var dragArea = layout.Custom<DragAreaControl>();
                dragArea.CustomControl.ScriptsEditor = this;

                // No support to show scripts for more than one actor selected
                if (Values.Count > 1)
                    return;
                
                // Scripts
                var scripts = (Script[])Values[0];
                var elementType = typeof(Script);
                for (int i = 0; i < scripts.Length; i++)
                {
                    var values = new ListValueContainer(elementType, i, Values);
                    var type = scripts[i].GetType();
                    var editor = CustomEditorsUtil.CreateEditor(type, false);
                    var title = CustomEditorsUtil.GetPropertyNameUI(type.Name);
                    var group = layout.Group(title);
                    group.Object(values, editor);
                }
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
