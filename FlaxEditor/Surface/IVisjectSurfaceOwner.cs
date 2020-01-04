// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Interface for Visject Surface parent objects.
    /// </summary>
    public interface IVisjectSurfaceOwner : ISurfaceContext
    {
        /// <summary>
        /// On surface edited state gets changed
        /// </summary>
        void OnSurfaceEditedChanged();

        /// <summary>
        /// On surface graph edited
        /// </summary>
        void OnSurfaceGraphEdited();

        /// <summary>
        /// Called when surface wants to close the tool window (due to user interaction or sth else).
        /// </summary>
        void OnSurfaceClose();
    }
}
