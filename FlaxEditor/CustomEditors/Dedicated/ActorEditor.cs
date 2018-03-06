////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Actions;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
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
			        var script = scripts[i];
			        cm.ItemsPanel.AddChild(new ItemsListContextMenu.Item(script.ScriptName, script));
		        }
		        cm.ItemClicked += script => AddScript((ScriptItem)script.Tag);
				cm.SortChildren();
		        cm.Show(this, button.BottomLeft);
			}

	        /// <inheritdoc />
            public override void Draw()
            {
                var style = FlaxEngine.GUI.Style.Current;
                var size = Size;

                // Info
                Render2D.DrawText(style.FontSmall, "Drag scripts here", new Rectangle(2, 22, size.X - 4, size.Y - 4 - 20), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords);

                // Check if drag is over
                if (IsDragOver && _dragScriptItems != null && _dragScriptItems.HasValidDrag)
                {
                    var area = new Rectangle(Vector2.Zero, size);
                    Render2D.FillRectangle(area, Color.Orange * 0.5f, true);
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
                    _dragScriptItems = new DragScriptItems();
                if (_dragScriptItems.OnDragEnter(data, ValidateScript))
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

	        private void AddScript(ScriptItem items)
	        {
		        var list = new List<ScriptItem>(1) { items };
		        AddScripts(list);
	        }

	        private void AddScripts(List<ScriptItem> items)
	        {
		        var actions = new List<IUndoAction>(4);

		        for (int i = 0; i < items.Count; i++)
		        {
			        var item = items[i];
			        var scriptName = item.ScriptName;
			        var scriptType = ScriptsBuilder.FindScript(scriptName);

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
		        Editor.Instance.Undo.AddAction(multiAction);
			}
        }

		/// <summary>
		/// Small image control added per script group that allows to drag and drop a reference to it. Also used to reorder the scripts.
		/// </summary>
		/// <seealso cref="FlaxEngine.GUI.Image" />
		private class ScriptDragIcon : Image
		{
			private ScriptsEditor _editor;
			private bool _isMosueDown;
			private Vector2 _mosueDownPos;
			
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
				_mosueDownPos = Vector2.Minimum;

				base.OnMouseEnter(location);
			}

			/// <inheritdoc />
			public override void OnMouseLeave()
			{
				// Check if start drag drop
				if (_isMosueDown)
				{
					DoDrag();
					_isMosueDown = false;
				}

				base.OnMouseLeave();
			}

			/// <inheritdoc />
			public override void OnMouseMove(Vector2 location)
			{
				// Check if start drag drop
				if (_isMosueDown && Vector2.Distance(location, _mosueDownPos) > 10.0f)
				{
					DoDrag();
					_isMosueDown = false;
				}

				base.OnMouseMove(location);
			}

			/// <inheritdoc />
			public override bool OnMouseUp(Vector2 location, MouseButton buttons)
			{
				if (buttons == MouseButton.Left)
				{
					// Clear flag
					_isMosueDown = false;
				}

				return base.OnMouseUp(location, buttons);
			}

			/// <inheritdoc />
			public override bool OnMouseDown(Vector2 location, MouseButton buttons)
			{
				if (buttons == MouseButton.Left)
				{
					// Set flag
					_isMosueDown = true;
					_mosueDownPos = location;
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

	    private class ScriptArrangeBar : Control
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
				Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), color, true);
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
                        layout.Group("Missing script");
                        continue;
                    }
                    var values = new ListValueContainer(elementType, i, Values);
                    var type = script.GetType();
                    var editor = CustomEditorsUtil.CreateEditor(type, false);

                    // Create group
                    var title = CustomEditorsUtil.GetPropertyNameUI(type.Name);
                    var group = layout.Group(title);
					group.Panel.Open(false);
	                
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
	                scriptToggle.CheckChanged += ScriptToggleOnCheckChanged;

					// Add drag button to the group
	                const float dragIconSize = 14;
					var scriptDrag = new ScriptDragIcon(this, script, scriptToggle.Right, 0.5f, dragIconSize)
					{
						TooltipText = "Script reference",
						CanFocus = true,
						IsScrollable = false,
						Color = new Color(0.7f),
						Margin = new Margin(1),
						ImageSource = new SpriteImageSource(Editor.Instance.UI.DragBar12),
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
                        ImageSource = new SpriteImageSource(FlaxEngine.GUI.Style.Current.Settings),
                        Tag = script,
                        Parent = group.Panel
                    };
                    settingsButton.Clicked += SettingsButtonOnClicked;

	                group.Panel.HeaderMargin = new Margin(scriptDrag.Right, 15, 2, 2);
					group.Object(values, editor);

					// Scripts arrange bar
	                dragBar = layout.Custom<ScriptArrangeBar>();
	                dragBar.CustomControl.Init(i + 1, this);
				}

                base.Initialize(layout);
            }

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
		        Editor.Instance.Undo.AddAction(action);
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
		        Editor.Instance.Undo.AddAction(action);
			}

			private void OnClickMoveUp(ContextMenuButton button)
	        {
		        var script = (Script)button.ParentContextMenu.Tag;
				var action = ChangeScriptAction.ChangeOrder(script, script.OrderInParent - 1);
		        action.Do();
		        Editor.Instance.Undo.AddAction(action);
			}

			private void OnClickMoveDown(ContextMenuButton button)
	        {
		        var script = (Script)button.ParentContextMenu.Tag;
				var action = ChangeScriptAction.ChangeOrder(script, script.OrderInParent + 1);
		        action.Do();
		        Editor.Instance.Undo.AddAction(action);
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

        /// <inheritdoc />
        protected override void SpawnProperty(LayoutElementsContainer itemLayout, ValueContainer itemValues, ItemInfo item)
        {
            // Note: we cannot specify actor properties editor types directly because we want to keep editor classes in FlaxEditor assembly
            int order = item.Order?.Order ?? int.MinValue;
            switch (order)
            {
				// Override static flags editor
				case -80:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorStaticFlagsEditor));
					break;

                // Override layer editor
                case -69:
                    item.CustomEditor = new CustomEditorAttribute(typeof(ActorLayerEditor));
                    break;

                // Override tag editor
                case -68:
                    item.CustomEditor = new CustomEditorAttribute(typeof(ActorTagEditor));
                    break;

                // Override position/scale editor
                case -30:
                case -10:
                    item.CustomEditor = new CustomEditorAttribute(typeof(ActorTransformEditor.PositionScaleEditor));
                    break;

                // Override orientation editor
                case -20:
                    item.CustomEditor = new CustomEditorAttribute(typeof(ActorTransformEditor.OrientationEditor));
                    break;
            }

            base.SpawnProperty(itemLayout, itemValues, item);
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
