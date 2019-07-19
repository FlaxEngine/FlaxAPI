// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public abstract partial class Joint
    {
        /// <summary>
        /// Occurs when a joint gets broken during simulation.
        /// </summary>
        public event Action JointBreak;

        internal void OnJointBreak()
        {
            JointBreak?.Invoke();
        }
    }
}
