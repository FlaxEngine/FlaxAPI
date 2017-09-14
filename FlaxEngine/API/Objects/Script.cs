////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
	public abstract partial class Script
    {
        /// <summary>
        /// Gets the world space transformation of the actors owning this script.
        /// </summary>
        public Transform Transform => Actor.Transform;
        
        /// <summary>
        /// Gets the local space transformation of the actors owning this script.
        /// </summary>
        public Transform LocalTransform => Actor.LocalTransform;
    }
}
