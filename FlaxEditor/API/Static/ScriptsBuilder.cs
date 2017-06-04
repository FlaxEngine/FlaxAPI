////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Scripting
{
    /// <summary>
    /// Game scrips building service. Compiles user C# scripts into binary assemblies.
    /// </summary>
    public static partial class ScriptsBuilder
    {
        /// <summary>
        /// Action called on compilation end, bool param is true if success, otherwise false
        /// </summary>
        public static event Action<bool> OnCompilationEnd;

        /// <summary>
        /// Action called when compilation success
        /// </summary>
        public static event Action OnCompilationSuccess;

        /// <summary>
        /// Action called whe compilation fails
        /// </summary>
        public static event Action OnCompilationFailed;

        /// <summary>
        /// Checks if need to compile source code. If so calls compilation.
        /// </summary>
        public static void CheckForCompile()
        {
            if (IsSourceDirty)
                Compile();
        }

        // Called internally from C++
        internal static void Internal_OnCompilationEnd(bool success)
        {
            OnCompilationEnd?.Invoke(success);
            if (success)
                OnCompilationSuccess?.Invoke();
            else
                OnCompilationFailed?.Invoke();
        }
    }
}
