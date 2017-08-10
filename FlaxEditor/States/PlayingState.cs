// Flax Engine scripting API

using System;
using FlaxEditor.Utilities;
using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is simulating game.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class PlayingState : EditorState
    {
        private DuplicateScenes _duplicateSceness;

        /// <inheritdoc />
        public override bool CanEditScene => true;

        /// <inheritdoc />
        public override bool CanEnterPlayMode => true;

        /// <summary>
        /// Gets or sets a value indicating whether game logic is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if game logic is paused; otherwise, <c>false</c>.
        /// </value>
        public bool IsPaused
        {
            get
            {
                // TODO: finish this
                throw new NotImplementedException();
                //auto root = SceneManager::Instance()->Root;
                //return root ? !root->IsGameLogicRunning : true;
            }
            private set
            {
                if (!IsActive)
                    throw new InvalidOperationException();

                // TODO: finish this
                throw new NotImplementedException();
                //auto scene = SceneManager::Instance();
                //scene->Lock();
                //ASSERT(scene->Root != nullptr);
                //scene->Root->IsGameLogicRunning = !pauseGame;
                //scene->Unlock();
            }
        }

        internal PlayingState(Editor editor)
            : base(editor)
        {
        }

        /// <inheritdoc />
        public override void OnEnter()
        {
            // TODO: finish this
            throw new NotImplementedException();

            /*// Clear selection
            // TODO: cache selection as Undo/Redo action but without Add to the UR context
            CEditor->GetMainGizmo()->Deselect();

            // Duplicate editor scene for simulation
            _duplicateScenes.GatherSceneData();

            // TODO: deserialize that scene data? and unlink edited scene??


            // TODO: after finishing csg data serialization remove that build call
            CSG::Builder::Instance()->Build();




            // Fire events
            for (int32 i = 0; i < CWindowsModule->Windows.Count(); i++)
            {
                CWindowsModule->Windows[i]->OnPlayBegin();
            }
            IsPaused = false;
            CSimulationModule->onPlayModeEnter();*/
        }

        /// <inheritdoc />
        public override void OnExit(State nextState)
        {
            // TODO: finish this
            throw new NotImplementedException();

            /*// Clear selection
            CEditor->GetMainGizmo()->Deselect();

            // Restore editor scene
            _duplicateScenes.RestoreSceneData();

            // Fire events
            for (int32 i = 0; i < CWindowsModule->Windows.Count(); i++)
            {
                CWindowsModule->Windows[i]->OnPlayEnd();
            }
            CSimulationModule->onPlayModeExit();*/
        }
    }
}
