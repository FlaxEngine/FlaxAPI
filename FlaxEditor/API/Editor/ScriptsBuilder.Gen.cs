// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Game scrips building service. Compiles user C# scripts into binary assemblies. Exposes many events used to track scripts compilation and reloading.
    /// </summary>
    [Tooltip("Game scrips building service. Compiles user C# scripts into binary assemblies. Exposes many events used to track scripts compilation and reloading.")]
    public static unsafe partial class ScriptsBuilder
    {
        /// <summary>
        /// Gets amount of source code compile actions since Editor startup.
        /// </summary>
        [Tooltip("Gets amount of source code compile actions since Editor startup.")]
        public static int CompilationsCount
        {
            get { return Internal_GetCompilationsCount(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetCompilationsCount();

        /// <summary>
        /// Gets the solution file path.
        /// </summary>
        [Tooltip("The solution file path.")]
        public static string SolutionPath
        {
            get { return Internal_GetSolutionPath(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetSolutionPath();

        /// <summary>
        /// Gets the solution filename (e.g. MyGame.sln).
        /// </summary>
        [Tooltip("The solution filename (e.g. MyGame.sln).")]
        public static string SolutionFileName
        {
            get { return Internal_GetSolutionFileName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetSolutionFileName();

        /// <summary>
        /// Gets the main project filename (e.g. MyGame.csproj).
        /// </summary>
        [Tooltip("The main project filename (e.g. MyGame.csproj).")]
        public static string MainProjectFileName
        {
            get { return Internal_GetMainProjectFileName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetMainProjectFileName();

        /// <summary>
        /// Gets the main editor project filename (e.g. MyGame.Editor..csproj).
        /// </summary>
        [Tooltip("The main editor project filename (e.g. MyGame.Editor..csproj).")]
        public static string MainEditorProjectFileName
        {
            get { return Internal_GetMainEditorProjectFileName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetMainEditorProjectFileName();

        /// <summary>
        /// Checks if last scripting building failed due to errors.
        /// </summary>
        [Tooltip("Checks if last scripting building failed due to errors.")]
        public static bool LastCompilationFailed
        {
            get { return Internal_LastCompilationFailed(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_LastCompilationFailed();

        /// <summary>
        /// Returns true if source code has been edited.
        /// </summary>
        [Tooltip("Returns true if source code has been edited.")]
        public static bool IsSourceDirty
        {
            get { return Internal_IsSourceDirty(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsSourceDirty();

        /// <summary>
        /// Returns true if scripts are being now compiled/reloaded.
        /// </summary>
        [Tooltip("Returns true if scripts are being now compiled/reloaded.")]
        public static bool IsCompiling
        {
            get { return Internal_IsCompiling(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsCompiling();

        /// <summary>
        /// Returns true if source code has been compiled and assemblies are ready to load.
        /// </summary>
        [Tooltip("Returns true if source code has been compiled and assemblies are ready to load.")]
        public static bool IsReady
        {
            get { return Internal_IsReady(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsReady();

        /// <summary>
        /// Generates Visual Studio solution and project files for project as a plugin.
        /// </summary>
        /// <param name="assemblyName">Assembly name for the plugin.</param>
        /// <returns>True if cannot generate project files, otherwise false.</returns>
        public static bool GeneratePluginProject(string assemblyName)
        {
            return Internal_GeneratePluginProject(assemblyName);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GeneratePluginProject(string assemblyName);

        /// <summary>
        /// Generates Visual Studio solution and project files.
        /// </summary>
        /// <param name="forceGenerateSolution">True if generate solution file by force event if there is no need to.</param>
        /// <param name="forceGenerateProject">True if generate project file by force event if there is no need to.</param>
        /// <returns>True if cannot generate project files, otherwise false.</returns>
        public static bool GenerateProject(bool forceGenerateSolution, bool forceGenerateProject)
        {
            return Internal_GenerateProject(forceGenerateSolution, forceGenerateProject);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GenerateProject(bool forceGenerateSolution, bool forceGenerateProject);

        /// <summary>
        /// Indicates that scripting directory has been modified so scripts need to be rebuild.
        /// </summary>
        public static void MarkWorkspaceDirty()
        {
            Internal_MarkWorkspaceDirty();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_MarkWorkspaceDirty();

        /// <summary>
        /// Checks if need to compile source code. If so calls compilation.
        /// </summary>
        public static void CheckForCompile()
        {
            Internal_CheckForCompile();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CheckForCompile();

        /// <summary>
        /// Requests project source code compilation.
        /// </summary>
        public static void Compile()
        {
            Internal_Compile();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Compile();

        /// <summary>
        /// Compiles the specified solution project.
        /// </summary>
        /// <remarks>
        /// It does not fire any CompileBegin, CompileEnd or other events except compilation warnings and errors.
        /// Also does not fires any scripting assemblies reload or any other actions.
        /// Scripts compilation is performed on a separate process.
        /// </remarks>
        /// <param name="solutionPath">The solution path (normalized, full path).</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool Compile(string solutionPath, BuildMode configuration)
        {
            return Internal_Compile1(solutionPath, configuration);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Compile1(string solutionPath, BuildMode configuration);

        /// <summary>
        /// Tries to find a script type with the given name.
        /// </summary>
        /// <param name="scriptName">The script full name.</param>
        /// <returns>Found script type or null if missing or invalid name.</returns>
        public static System.Type FindScript(string scriptName)
        {
            return Internal_FindScript(scriptName);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern System.Type Internal_FindScript(string scriptName);

        public static void GetExistingEditors(int* result, int count)
        {
            Internal_GetExistingEditors(result, count);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetExistingEditors(int* result, int count);

        public enum BuildMode
        {
            Debug = 0,

            Release = 1,
        }
    }
}
