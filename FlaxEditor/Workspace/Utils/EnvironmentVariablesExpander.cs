// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;

namespace FlaxEditor.Workspace.Utils
{
    /// <summary>
    ///     Utility class that expands all environment variables in a requested string
    /// </summary>
    public sealed class EnvironmentVariablesExpander
    {
        private readonly IDictionary _myEnvironmentVariables = Environment.GetEnvironmentVariables();

        /// <summary>
        ///     Expand all environment variables in a requested string
        /// </summary>
        /// <param name="value">Value to expand</param>
        /// <returns>Returns fully qualified enviromental variable</returns>
        public string Expand(string value)
        {
            var str = value;
            foreach (var key in _myEnvironmentVariables.Keys)
            {
                var environmentVariable = _myEnvironmentVariables[key];
                str = str.Replace($"%{key}%", environmentVariable.ToString());
            }

            return str;
        }
    }
}
