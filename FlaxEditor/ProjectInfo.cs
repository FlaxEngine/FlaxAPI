////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Project metadata loaded from the project root file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProjectInfo
    {
        /// <summary>
        /// The project name (used by the launcher).
        /// </summary>
        public string Name;

        /// <summary>
        /// The default scene asset identifier to open on project startup.
        /// </summary>
        public Guid DefaultSceneId;

		/// <summary>
		/// The default scene spawn point (position and view direction).
		/// </summary>
		public Ray DefaultSceneSpawn;
	}
}
