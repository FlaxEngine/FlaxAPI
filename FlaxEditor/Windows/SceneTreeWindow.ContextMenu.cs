// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public partial class SceneTreeWindow
    {
        /// <summary>
        /// Creates the context menu for the current objects selection and the current Editor state.
        /// </summary>
        /// <returns>The context menu.</returns>
        private ContextMenu CreateContextMenu()
        {
            // Preapre

            bool hasSthSelected = Editor.SceneEditing.HasSthSelected;
            bool isSingleActorSelected = Editor.SceneEditing.SelectionCount == 1 && Editor.SceneEditing.Selection[0] is ActorNode;
            bool canEditScene = Editor.StateMachine.CurrentState.CanEditScene && SceneManager.IsAnySceneLoaded;

            // Create popup

            var contextMenu = new ContextMenu();
            contextMenu.MinimumWidth = 120;

            // Basic editing actions

            var b = contextMenu.AddButton("Rename", Rename);
            b.Enabled = isSingleActorSelected;

            b = contextMenu.AddButton("Duplicate", Editor.SceneEditing.Duplicate);
            b.Enabled = hasSthSelected;

            b = contextMenu.AddButton("Delete", Editor.SceneEditing.Delete);
            b.Enabled = hasSthSelected;

            contextMenu.AddSeparator();
            b = contextMenu.AddButton("Copy", Editor.SceneEditing.Copy);

            b.Enabled = hasSthSelected;
            contextMenu.AddButton("Paste", Editor.SceneEditing.Paste);

            b = contextMenu.AddButton("Cut", Editor.SceneEditing.Cut);
            b.Enabled = hasSthSelected;
            
            // Spawning actors

            contextMenu.AddSeparator();
            var spawnMenu = contextMenu.AddChildMenu("New");
            var newActorCm = spawnMenu.ContextMenu;
            for (int i = 0; i < _spawnActorsGroups.Length; i++)
            {
                var group = _spawnActorsGroups[i];

                if (group.Types.Length == 1)
                {
                    var type = group.Types[0].Value;
                    newActorCm.AddButton(group.Types[0].Key, () => Spawn(type));
                }
                else
                {
                    var groupCm = newActorCm.AddChildMenu(group.Name).ContextMenu;
                    for (int j = 0; j < group.Types.Length; j++)
                    {
                        var type = group.Types[j].Value;
                        groupCm.AddButton(group.Types[j].Key, () => Spawn(type));
                    }
                }
            }

            return contextMenu;
        }

        /// <summary>
        /// Shows the context menu on a given location (in the given control coordinates).
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="location">The location (within a given control).</param>
        private void ShowContextMenu(Control parent, ref Vector2 location)
        {
            var contextMenu = CreateContextMenu();

            contextMenu.Show(parent, location);
        }
    }
}
