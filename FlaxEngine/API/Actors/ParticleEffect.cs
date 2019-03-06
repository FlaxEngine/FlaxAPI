// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class ParticleEffect
    {
        private object[] _parameters;

        /// <summary>
        /// Helper value used to keep parameters collection in sync with actual backend data.
        /// </summary>
        internal int _parametersHash;

        /// <summary>
        /// Occurs when particle effect parameters collection gets changed.
        /// It's called on <see cref="ParticleEffect"/> particle system asset changed or when particle system asset gets reloaded, etc.
        /// </summary>
        public event Action<ParticleEffect> ParametersChanged;

        internal void Internal_OnSystemChanged()
        {
            // Clear cached data
            _parametersHash++;
            _parameters = null;

            ParametersChanged?.Invoke(this);
        }
    }
}
