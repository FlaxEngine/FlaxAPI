// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.Actions;
using FlaxEditor.Content;
using FlaxEditor.SceneGraph;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Prefabs management module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class PrefabsModule : EditorModule
    {
        internal PrefabsModule(Editor editor)
        : base(editor)
        {
        }

        /// <summary>
        /// Starts the creating prefab for the selected actor by showing the new item creation dialog in <see cref="ContentWindow"/>.
        /// </summary>
        /// <remarks>
        /// To create prefab manualy use <see cref="Editor.CreatePrefab"/> method.
        /// </remarks>
        public void CreatePrefab()
        {
            // Check selection
            var selection = Editor.SceneEditing.Selection;
            if (selection.Count == 1 && selection[0] is ActorNode actorNode && actorNode.CanCreatePrefab)
            {
                CreatePrefab(actorNode.Actor);
            }
        }

        /// <summary>
        /// Starts the creating prefab for the given actor by showing the new item creation dialog in <see cref="ContentWindow"/>. User can specify the new asset name.
        /// </summary>
        /// <remarks>
        /// To create prefab manualy use <see cref="Editor.CreatePrefab"/> method.
        /// </remarks>
        /// <param name="actor">The root prefab actor.</param>
        public void CreatePrefab(Actor actor)
        {
            // Skip in invalid states
            if (!Editor.StateMachine.CurrentState.CanEditContent)
                return;

            // Skip if cannot create assets in the given location
            if (!Editor.Windows.ContentWin.CurrentViewFolder.CanHaveAssets)
                return;

            var proxy = Editor.ContentDatabase.GetProxy<Prefab>();
            Editor.Windows.ContentWin.NewItem(proxy, actor, OnPrefabCreated);
        }

        private void OnPrefabCreated(ContentItem contetItem)
        {
            // Skip in invalid states
            if (!Editor.StateMachine.CurrentState.CanEditScene)
                return;

            // Record undo for prefab creating (backend links the target instance with the prefab)
            if (Editor.Undo.Enabled)
            {
                var selection = Editor.SceneEditing.Selection.Where(x => x is ActorNode).ToList().BuildNodesParents();
                if (selection.Count == 0)
                    return;
                
                if (selection.Count == 1)
                {
                    var action = BreakPrefabLinkAction.Linked(((ActorNode)selection[0]).Actor);
                    Undo.AddAction(action);
                }
                else
                {
                    var actions = new IUndoAction[selection.Count];
                    for (int i = 0; i < selection.Count; i++)
                    {
                        var action = BreakPrefabLinkAction.Linked(((ActorNode)selection[i]).Actor);
                        actions[i] = action;
                    }
                    Undo.AddAction(new MultiUndoAction(actions));
                }
            }
        }

        /// <summary>
        /// Breaks any prefab links for the selected objects. Supports undo/redo.
        /// </summary>
        public void BreakLinks()
        {
            // Skip in invalid states
            if (!Editor.StateMachine.CurrentState.CanEditScene)
                return;

            // Get valid objects (the top ones, C++ backend will process the child objects)
            var selection = Editor.SceneEditing.Selection.Where(x => x is ActorNode actorNode && actorNode.HasPrefabLink).ToList().BuildNodesParents();
            if (selection.Count == 0)
                return;

            // Perform action
            if (Editor.StateMachine.CurrentState.CanUseUndoRedo)
            {
                if (selection.Count == 1)
                {
                    var action = BreakPrefabLinkAction.Break(((ActorNode)selection[0]).Actor);
                    Undo.AddAction(action);
                    action.Do();
                }
                else
                {
                    var actions = new IUndoAction[selection.Count];
                    for (int i = 0; i < selection.Count; i++)
                    {
                        var action = BreakPrefabLinkAction.Break(((ActorNode)selection[i]).Actor);
                        actions[i] = action;
                        action.Do();
                    }
                    Undo.AddAction(new MultiUndoAction(actions));
                }
            }
            else
            {
                foreach (var e in selection)
                {
                    for (int i = 0; i < selection.Count; i++)
                    {
                        ((ActorNode)selection[i]).Actor.BreakPrefabLink();
                    }
                }
            }
        }

        /// <summary>
        /// Selects in Content Window the prefab asset used by the selected objects.
        /// </summary>
        public void SelectPrefab()
        {
            // Get valid objects (the top ones, C++ backend will process the child objects)
            var selection = Editor.SceneEditing.Selection.Where(x => x is ActorNode actorNode && actorNode.HasPrefabLink).ToList().BuildNodesParents();
            if (selection.Count == 0)
                return;

            var prefabId = ((ActorNode)selection[0]).Actor.PrefabID;
            var prefab = FlaxEngine.Content.LoadAsync<Prefab>(prefabId);
            Editor.Windows.ContentWin.Select(prefab);
        }
    }
}
