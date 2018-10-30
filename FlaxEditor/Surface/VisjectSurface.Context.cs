// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        private VisjectSurfaceContext _root;
        private VisjectSurfaceContext _context;
        private readonly Dictionary<ISurfaceContext, VisjectSurfaceContext> _contextCache = new Dictionary<ISurfaceContext, VisjectSurfaceContext>();

        /// <summary>
        /// The surface context stack.
        /// </summary>
        public readonly Stack<VisjectSurfaceContext> ContextStack = new Stack<VisjectSurfaceContext>(8);

        /// <summary>
        /// Gets the root surface context.
        /// </summary>
        public VisjectSurfaceContext RootContext => _root;

        /// <summary>
        /// Gets the active surface context.
        /// </summary>
        public VisjectSurfaceContext Context => _context;

        /// <summary>
        /// Occurs when context gets changed.
        /// </summary>
        public event Action<VisjectSurfaceContext> ContextChanged;

        /// <summary>
        /// Creates the Visject surface context for the given surface data source context.
        /// </summary>
        /// <param name="parent">The parent context.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected virtual VisjectSurfaceContext CreateContext(VisjectSurfaceContext parent, ISurfaceContext context)
        {
            return new VisjectSurfaceContext(this, parent, context);
        }

        /// <summary>
        /// Opens the child context of the current context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void OpenContext(ISurfaceContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (_context != null && _context.Context == context)
                return;

            // Get or create context
            VisjectSurfaceContext surfaceContext;
            if (!_contextCache.TryGetValue(context, out surfaceContext))
            {
                surfaceContext = CreateContext(_context, context);
                surfaceContext.Modified += (_) => MarkAsEdited();
                _context?.Children.Add(surfaceContext);
                _contextCache.Add(context, surfaceContext);
                context.OnContextCreated(surfaceContext);
            }
            if (_root == null)
                _root = surfaceContext;
            else if (ContextStack.Contains(surfaceContext))
                throw new ArgumentException("Context has been already added to the stack.");

            // Load context
            if (_root != surfaceContext)
            {
                if (surfaceContext.Load())
                    throw new Exception("Failed to load graph.");
            }

            // Change stack
            ContextStack.Push(surfaceContext);

            // Update
            OnContextChanged();
        }

        /// <summary>
        /// Closes the last opened context (the current one).
        /// </summary>
        public void CloseContext()
        {
            if (ContextStack.Count < 2)
                throw new ArgumentException("No context to close.");

            // Change stack
            ContextStack.Pop();

            // Update
            OnContextChanged();
        }

        /// <summary>
        /// Changes the current opened context to the given one. Used as a navigation method.
        /// </summary>
        /// <param name="context">The target context.</param>
        public void ChangeContext(ISurfaceContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (_context == null)
            {
                OpenContext(context);
                return;
            }
            if (_context.Context == context)
                return;

            // Check if already in a path
            VisjectSurfaceContext surfaceContext;
            if (_contextCache.TryGetValue(context, out surfaceContext) && ContextStack.Contains(surfaceContext))
            {
                // Change stack
                do
                {
                    ContextStack.Pop();
                } while (ContextStack.Peek() != surfaceContext);
            }
            else
            {
                // TODO: implement this case (need to find first parent of the context that is in path)
                throw new NotSupportedException("TODO: support changing context to one not in the active path");
            }

            // Update
            OnContextChanged();
        }

        /// <summary>
        /// Called when context gets changed. Updates current context and UI. Updates the current context based on the first element in teh stack.
        /// </summary>
        protected virtual void OnContextChanged()
        {
            var context = ContextStack.Count > 0 ? ContextStack.Peek() : null;
            _context = context;

            // Update root control linkage
            if (_rootControl != null)
            {
                _rootControl.Parent = null;
            }
            if (context != null)
            {
                _rootControl = _context.RootControl;
                _rootControl.Parent = this;
            }
            else
            {
                _rootControl = null;
            }

            ContextChanged?.Invoke(_context);
        }
    }
}
