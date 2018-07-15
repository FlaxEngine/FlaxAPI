// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
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

            // TODO: change the prefab source actor to be its instance
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
                using (new UndoMultiBlock(Undo, selection, "Break Prefab Link"))
                {
                    foreach (var e in selection)
                    {
                        // TODO: break link
                    }
                }
            }
            else
            {
                foreach (var e in selection)
                {
                    // TODO: break link
                }
            }
        }
    }
}
