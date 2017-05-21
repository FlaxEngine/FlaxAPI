// Flax Engine scripting API

using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state engine is building static lighting for the scene. Editing scene is blocked.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class BuildingLightingState : EditorState
    {
        private bool _wasBuildFinished;

        /// <inheritdoc />
        public override bool CanEditContent => false;

        /// <inheritdoc />
        public override bool CanEnter()
        {
            return StateMachine.IsEditMode;
        }

        /// <inheritdoc />
        public override bool CanExit(State nextState)
        {
            return _wasBuildFinished;
        }

        /// <inheritdoc />
        public override void OnEnter()
        {
            // Clear flag
            _wasBuildFinished = false;

            // Bind event
            //ShadowsOfMordor::Builder::Instance()->OnBuildFinished.Bind<BuildingLightingState, &BuildingLightingState::onBuildFinished>(this);

            // Start building
            //CEditor->MarkAsEdited();
            //ShadowsOfMordor::Builder::Instance()->Build();
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Unbind event
            //ShadowsOfMordor::Builder::Instance()->OnBuildFinished.Unbind<BuildingLightingState, &BuildingLightingState::onBuildFinished>(this);

            // Notify user
            //CWindowsModule->FlashMainWindow();
        }

        private void onBuildFinished(bool failed)
        {
            //ASSERT(IsActive() && !_wasBuildFinished);
            //_wasBuildFinished = true;
            //GetParent()->GoTo(EditorStates::EditingScene);
        }
    }
}
