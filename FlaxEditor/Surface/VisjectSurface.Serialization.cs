// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Meta10 // TypeID: 10, for surface
        {
            public Vector2 ViewCenterPosition;
            public float Scale;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Meta11 // TypeID: 11, for nodes
        {
            public Vector2 Position;
            public bool Selected;
        }

        /// <summary>
        /// Loads surface from the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool Load(byte[] bytes)
        {
            Enabled = false;

            // Clean data
            _connectionInstigator = null;
            _lastInstigatorUnderMouse = null;

            var failed = RootContext.Load();

            Enabled = true;

            if (failed)
            {
                // Error
                return true;
            }

            // Update the view from context meta
            ViewCenterPosition = _context.CachedSurfaceMeta.ViewCenterPosition;
            ViewScale = _context.CachedSurfaceMeta.Scale;

            // End
            Owner.OnSurfaceEditedChanged();

            return false;
        }

        /// <summary>
        /// Saves the surface graph to bytes.
        /// </summary>
        /// <remarks>
        /// The method calls the <see cref="ISurfaceContext.SurfaceData"/> setter to assign the result bytes. Sets null value if failed.
        /// </remarks>
        public void Save()
        {
            var hasFocus = IsFocused;
            var wasEdited = IsEdited;
            Enabled = false;

            // Update the current context meta
            _context.CachedSurfaceMeta.ViewCenterPosition = ViewCenterPosition;
            _context.CachedSurfaceMeta.Scale = ViewScale;

            // Save context (and every modified child context)
            bool failed = RootContext.Save();

            Enabled = true;
            if (hasFocus)
                Focus();

            if (failed)
            {
                // Error
                return;
            }

            // Clear flag
            if (wasEdited)
            {
                Owner.OnSurfaceEditedChanged();
            }
        }
    }
}
