// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Dedicated custom editor for <see cref="Actor"/> objects.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
    [CustomEditor(typeof(Actor)), DefaultEditor]
    public class ActorEditor : GenericEditor
    {
        private Guid _linkedPrefabId;

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

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            // Check for prefab link
            if (Values.IsSingleObject && Values[0] is Actor actor && actor.HasPrefabLink)
            {
                // TODO: consider editing more than one instance of the same prefab asset at once

                var prefab = FlaxEngine.Content.LoadAsync<Prefab>(actor.PrefabID);
                // TODO: don't stall here?
                if (prefab && !prefab.WaitForLoaded())
                {
                    var prefabObjectId = actor.PrefabObjectID;
                    var prefabInstance = Prefab.Internal_GetDefaultInstance(prefab.unmanagedPtr, ref prefabObjectId);
                    if (prefabInstance != null)
                    {
                        // Use default prefab instance as a reference for the editor
                        Values.SetReferenceValue(prefabInstance);

                        // Add some UI
                        var panel = layout.CustomContainer<UniformGridPanel>();
                        panel.CustomControl.Height = 20.0f;
                        panel.CustomControl.SlotsVertically = 1;
                        panel.CustomControl.SlotsHorizontally = 2;

                        // Selecting actor prefab asset
                        var selectPrefab = panel.Button("Select Prefab");
                        selectPrefab.Button.Clicked += () => Editor.Instance.Windows.ContentWin.Select(prefab);

                        // Viewing changes applied to this actor
                        var viewChanges = panel.Button("View Changes");
                        viewChanges.Button.Clicked += () => ViewChanges(viewChanges.Button, new Vector2(0.0f, 20.0f));

                        // Link event to update editor on prefab apply
                        _linkedPrefabId = prefab.ID;
                        Editor.Instance.Prefabs.PrefabApplied += OnPrefabApplied;
                    }
                }
            }

            base.Initialize(layout);
        }

        /// <inheritdoc />
        protected override void Deinitialize()
        {
            base.Deinitialize();

            if (_linkedPrefabId != Guid.Empty)
            {
                _linkedPrefabId = Guid.Empty;
                Editor.Instance.Prefabs.PrefabApplied -= OnPrefabApplied;
            }
        }

        private void OnPrefabApplied(Prefab prefab, Actor instance)
        {
            if (prefab.ID == _linkedPrefabId)
            {
                // This works fine but in PrefabWindow when using live update it crashes on using color picker/float slider because UI is being rebuild
                //Presenter.BuildLayoutOnUpdate();

                // Better way is to just update the reference value using the new default instance of the prefab, created after changes apply
                if (prefab && !prefab.WaitForLoaded())
                {
                    var actor = (Actor)Values[0];
                    var prefabObjectId = actor.PrefabObjectID;
                    var prefabInstance = Prefab.Internal_GetDefaultInstance(prefab.unmanagedPtr, ref prefabObjectId);
                    if (prefabInstance != null)
                    {
                        Values.SetReferenceValue(prefabInstance);
                        RefreshReferenceValue();
                    }
                }
            }
        }

        private TreeNode CreateDiffNode(CustomEditor editor)
        {
            var node = new TreeNode(false);

            node.Tag = editor;

            if (editor.ParentEditor?.Values?.Type?.IsArray ?? false)
            {
                node.Text = "Element " + editor.ParentEditor.ChildrenEditors.IndexOf(editor);
            }
            else if (editor.Values.Info != null)
            {
                node.Text = CustomEditorsUtil.GetPropertyNameUI(editor.Values.Info.Name);
            }
            else if (editor.Values[0] != null)
            {
                node.Text = editor.Values[0].ToString();
            }

            if (editor.Values[0] is Actor)
            {
                node.TextColor = FlaxEngine.GUI.Style.Current.ProgressNormal;
            }
            if (editor.Values[0] is Script script)
            {
                node.TextColor = script.HasPrefabLink ? FlaxEngine.GUI.Style.Current.ProgressNormal : FlaxEngine.GUI.Style.Current.BackgroundSelected;
                node.Text = CustomEditorsUtil.GetPropertyNameUI(script.GetType().Name);
            }

            node.Expand(true);

            return node;
        }

        private TreeNode ProcessDiff(CustomEditor editor)
        {
            // Special case for new Script added to actor
            if (editor.Values[0] is Script script && !script.HasPrefabLink)
            {
                return CreateDiffNode(editor);
            }

            // TODO: show scripts removed from the actor
            // TODO: proper reverting removed scripts from actor with undo

            // Skip if no change detected
            if (!editor.Values.IsReferenceValueModified)
                return null;

            TreeNode result = null;

            if (editor.ChildrenEditors.Count == 0)
                result = CreateDiffNode(editor);

            for (int i = 0; i < editor.ChildrenEditors.Count; i++)
            {
                var child = ProcessDiff(editor.ChildrenEditors[i]);
                if (child != null)
                {
                    if (result == null)
                        result = CreateDiffNode(editor);

                    result.AddChild(child);
                }
            }

            return result;
        }

        private void ViewChanges(Control target, Vector2 targetLocation)
        {
            // Build a tree out of modified properties
            var rootNode = ProcessDiff(this);

            // Skip if no changes detected
            if (rootNode == null || rootNode.ChildrenCount == 0)
            {
                var cm1 = new ContextMenu();
                cm1.AddButton("No changes detected");
                cm1.Show(target, targetLocation);
                return;
            }

            // Create context menu
            var cm = new PrefabDiffContextMenu();
            cm.Tree.AddChild(rootNode);
            cm.Tree.RightClick += OnDiffNodeRightClick;
            cm.Tree.Tag = cm;
            cm.RevertAll += OnDiffRevertAll;
            cm.ApplyAll += OnDiffApplyAll;
            cm.Show(target, targetLocation);
        }

        private void OnDiffNodeRightClick(TreeNode node, Vector2 location)
        {
            var diffMenu = (PrefabDiffContextMenu)node.ParentTree.Tag;

            var menu = new ContextMenu();
            menu.AddButton("Revert", () => OnDiffRevert((CustomEditor)node.Tag));
            menu.AddSeparator();
            menu.AddButton("Revert All", OnDiffRevertAll);
            menu.AddButton("Apply All", OnDiffApplyAll);

            diffMenu.ShowChild(menu, node.PointToParent(diffMenu, new Vector2(location.X, node.HeaderHeight)));
        }

        private void OnDiffRevertAll()
        {
            OnDiffRevert(this);
        }

        private void OnDiffApplyAll()
        {
            Editor.Instance.Prefabs.ApplyAll((Actor)Values[0]);
        }

        private void OnDiffRevert(CustomEditor editor)
        {
            editor.RevertToReferenceValue();
        }
    }
}
