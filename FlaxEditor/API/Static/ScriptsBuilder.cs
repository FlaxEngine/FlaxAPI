////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Scripting
{
    /// <summary>
    /// Game scrips building service. Compiles user C# scripts into binary assemblies.
    /// Exposes many events used to track scripts copilation and reloading.
    /// </summary>
    public static partial class ScriptsBuilder
    {
        // TODO: expose api to inject custom defines to compilation and more customizations

        /// <summary>
        /// On compilation end
        /// </summary>
        /// <param name="success">False if compilation has failed, otherwise true.</param>
        public delegate void CompilationEndDelegate(bool success);

        /// <summary>
        /// Occurs when compilation ends.
        /// </summary>
        public static event CompilationEndDelegate CompilationEnd;

        /// <summary>
        /// Occurs when compilation success.
        /// </summary>
        public static event Action CompilationSuccess;

        /// <summary>
        /// Occurs when compilation failed.
        /// </summary>
        public static event Action CompilationFailed;

        /// <summary>
        /// Occurs when compilation begins.
        /// </summary>
        public static event Action CompilationBegin;

        /// <summary>
        /// Occurs when compilation just started.
        /// </summary>
        public static event Action CompilationStarted;

        /// <summary>
        /// Occurs when user scripts reload is called (just before actual reload).
        /// </summary>
        public static event Action ScriptsReloadCalled;

        /// <summary>
        /// Occurs when user scripts reload starts.
        /// Usef objects should be removed at this point to reduce leaks and issues.
        /// </summary>
        public static event Action ScriptsReloadBegin;

        /// <summary>
        /// Occurs when user scripts reload ends.
        /// </summary>
        public static event Action ScriptsReloadEnd;

        /// <summary>
        /// Checks if need to compile source code. If so calls compilation.
        /// </summary>
        public static void CheckForCompile()
        {
            if (IsSourceDirty)
                Compile();
        }

        internal enum EventType
        {
            CompileBegin = 0,
            CompileStarted = 1,
            CompileEndGood = 2,
            CompileEndFailed = 3,
            ReloadCalled = 4,
            ReloadBegin = 5,
            ReloadEnd = 6,
        }

        internal static void Internal_OnEvent(EventType type)
        {
            switch (type)
            {
                case EventType.CompileBegin:
                    CompilationBegin?.Invoke();
                    break;
                case EventType.CompileStarted:
                    CompilationStarted?.Invoke();
                    break;
                case EventType.CompileEndGood:
                    CompilationEnd?.Invoke(true);
                    CompilationSuccess?.Invoke();
                    break;
                case EventType.CompileEndFailed:
                    CompilationEnd?.Invoke(false);
                    CompilationFailed?.Invoke();
                    break;
                case EventType.ReloadCalled:
                    ScriptsReloadCalled?.Invoke();
                    break;
                case EventType.ReloadBegin:
                    ScriptsReloadBegin?.Invoke();
                    break;
                case EventType.ReloadEnd:
                    ScriptsReloadEnd?.Invoke();
                    break;
            }
        }
    }
}
