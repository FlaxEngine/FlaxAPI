////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Interface for Visject Surface parent objects.
    /// </summary>
    public interface IVisjectSurfaceOwner
    {
        /// <summary>
        /// On surface save command
        /// </summary>
        void OnSurfaceSave();

        /// <summary>
        /// On surface edited state gets changed
        /// </summary>
        void OnSurfaceEditedChanged();

        /// <summary>
        /// On surface graph edited
        /// </summary>
        void OnSurfaceGraphEdited();
    }
}
