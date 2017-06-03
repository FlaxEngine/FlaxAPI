// Flax Engine scripting API

using System;
using System.Collections.Generic;

namespace FlaxEngine.Utilities
{
    /// <summary>
    /// State machine logic pattern
    /// </summary>
    public abstract class StateMachine
    {
        protected State currentState;
        protected readonly List<State> states = new List<State>();

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <value>
        /// The current state.
        /// </value>
        public State CurrentState => currentState;

        /// <summary>
        /// Gets state of given type.
        /// </summary>
        /// <typeparam name="TStateType">The type of the state.</typeparam>
        public State GetState<TStateType>()
        {
            return states.Find(x => x.GetType().IsAssignableFrom(typeof(TStateType)));
        }

        /// <summary>
        /// Goes to the state.
        /// </summary>
        /// <typeparam name="TStateType">The type of the state.</typeparam>
        /// <exception cref="InvalidOperationException">Cannot find state of given type.</exception>
        public void GoToState<TStateType>()
        {
            var state = states.Find(x => x.GetType().IsAssignableFrom(typeof(TStateType)));
            if(state == null)
                throw new InvalidOperationException($"Cannot find state {typeof(TStateType)}.");
            GoToState(state);
        }

        /// <summary>
        /// Goes to the state.
        /// </summary>
        /// <param name="state">The target state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        public virtual void GoToState(State state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            // Prevent from entering the same state
            if (state == currentState)
                return;

            // Check if cannot leave current state
            if (currentState != null && !currentState.CanExit(state))
                return;

            // Check if cannot enter new state
            if (!state.CanEnter())
                return;

            // Change state
            SwitchState(state);
        }

        /// <summary>
        /// Adds the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public virtual void AddState(State state)
        {
            if(state.owner == this)
                throw new InvalidOperationException("Cannot add already registered state.");

            states.Add(state);
            state.owner = this;
        }

        /// <summary>
        /// Removes the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public virtual void RemoveState(State state)
        {
            if (state.owner == null)
                throw new InvalidOperationException("Cannot remove unregistered state.");

            states.Remove(state);
            state.owner = null;
        }

        /// <summary>
        /// Switches the state.
        /// </summary>
        /// <param name="nextState">Then next state.</param>
        protected virtual void SwitchState(State nextState)
        {
            currentState?.OnExit();

            currentState = nextState;

            currentState?.OnEnter();
        }
    }
}
