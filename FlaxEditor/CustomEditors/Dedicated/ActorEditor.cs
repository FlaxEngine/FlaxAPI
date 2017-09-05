////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI.Drag;
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
        /// Custom editor for actor scripts collection.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.Editors.ArrayEditor" />
        public sealed class ScriptsEditor : ArrayEditor
        {
            /// <summary>
            /// Drag and drop scripts area control.
            /// </summary>
            /// <seealso cref="FlaxEngine.GUI.Control" />
            public class DragAreaControl : Control
            {
                private DragScripts _dragScripts;

                /// <summary>
                /// The linked actor.
                /// </summary>
                public Actor Actor;

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
                        Render2D.DrawRectangle(area.MakeExpanded(-1), Color.Black);
                    }
                }

                private bool ValidateScript(ScriptItem scriptItem)
                {
                    throw new NotImplementedException("validate script drag " + scriptItem.ScriptName);
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

                            throw new NotImplementedException("add script " + item.ScriptName);
                        }
                    }

                    return result;
                }
            }

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                // Area for drag&drop scripts
                var dragArea = layout.Custom<DragAreaControl>();
                //dragArea.CustomControl.Actor =  TODO: get actor!!!

                base.Initialize(layout);
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
