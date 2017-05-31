////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
	public sealed partial class Scene
	{
	    /// <summary>
	    /// Saves this scene to the asset.
	    /// </summary>
	    /// <returns>True if action fails, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    public bool SaveScene(Scene scene)
	    {
	        return SceneManager.SaveScene(this);
        }

	    /// <summary>
	    /// Saves this scene to the asset. Done in the background.
	    /// </summary>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    public void SaveSceneAsync(Scene scene)
	    {
	        SceneManager.SaveSceneAsync(this);
	    }

	    /// <summary>
        /// Unloads this scene.
        /// </summary>
        /// <returns>True if action fails, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public bool UnloadScene(Scene scene)
	    {
	        return SceneManager.UnloadScene(this);
        }

        /// <summary>
        /// Unloads this scene. Done in the background.
        /// </summary>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        public void UnloadSceneAsync(Scene scene)
	    {
	        SceneManager.UnloadSceneAsync(this);
	    }

    }
}
