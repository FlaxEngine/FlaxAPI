// Flax Engine scripting API

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Base class for all Editor modules.
    /// </summary>
    public abstract class EditorModule
    {
        /// <summary>
        /// Gets the initialization order. Lower first ordering.
        /// </summary>
        /// <value>
        /// The initialization order.
        /// </value>
        public int InitOrder => 0;

        /// <summary>
        /// Called when Editor is startup up. Performs module initialization.
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// Called when Editor is ready and winn start work.
        /// </summary>
        protected virtual void OnEndInit()
        {
        }
        
        /// <summary>
        /// Called when every Editor update tick.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Called when Editor is closing. Performs module cleanup.
        /// </summary>
        protected virtual void OnExit()
        {
        }
    }
}
