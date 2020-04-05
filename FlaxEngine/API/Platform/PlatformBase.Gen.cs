// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Network connection types for device.
    /// </summary>
    [Tooltip("Network connection types for device.")]
    public enum NetworkConnectionType
    {
        /// <summary>
        /// No connection.
        /// </summary>
        [Tooltip("No connection.")]
        None,

        /// <summary>
        /// The unknown connection type.
        /// </summary>
        [Tooltip("The unknown connection type.")]
        Unknown,

        /// <summary>
        /// The airplane mode.
        /// </summary>
        [Tooltip("The airplane mode.")]
        AirplaneMode,

        /// <summary>
        /// The cell connection.
        /// </summary>
        [Tooltip("The cell connection.")]
        Cell,

        /// <summary>
        /// The WiFi connection.
        /// </summary>
        [Tooltip("The WiFi connection.")]
        WiFi,

        /// <summary>
        /// The Bluetooth connection.
        /// </summary>
        [Tooltip("The Bluetooth connection.")]
        Bluetooth,

        /// <summary>
        /// The Ethernet cable connection (LAN).
        /// </summary>
        [Tooltip("The Ethernet cable connection (LAN).")]
        Ethernet,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Runtime platform service.
    /// </summary>
    [Tooltip("Runtime platform service.")]
    public static unsafe partial class Platform
    {
        /// <summary>
        /// Returns the current runtime platform type. It's compile-time constant.
        /// </summary>
        [Tooltip("Returns the current runtime platform type. It's compile-time constant.")]
        public static PlatformType PlatformType
        {
            get { return Internal_GetPlatformType(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PlatformType Internal_GetPlatformType();

        /// <summary>
        /// Returns true if is running 64 bit application (otherwise 32 bit). It's compile-time constant.
        /// </summary>
        [Tooltip("Returns true if is running 64 bit application (otherwise 32 bit). It's compile-time constant.")]
        public static bool Is64BitApp
        {
            get { return Internal_Is64BitApp(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Is64BitApp();

        /// <summary>
        /// Returns true if running on 64-bit computer
        /// </summary>
        [Tooltip("Returns true if running on 64-bit computer")]
        public static bool Is64BitPlatform
        {
            get { return Internal_Is64BitPlatform(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Is64BitPlatform();

        /// <summary>
        /// Returns true if the game is running in the Flax Editor; false if run from any deployment target. Use this property to perform Editor-related actions.
        /// </summary>
        [Tooltip("Returns true if the game is running in the Flax Editor; false if run from any deployment target. Use this property to perform Editor-related actions.")]
        public static bool IsEditor
        {
            get { return Internal_IsEditor(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsEditor();

        /// <summary>
        /// Gets the CPU information.
        /// </summary>
        [Tooltip("The CPU information.")]
        public static CPUInfo CPUInfo
        {
            get { Internal_GetCPUInfo(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCPUInfo(out CPUInfo resultAsRef);

        /// <summary>
        /// Gets the CPU cache line size.
        /// </summary>
        [Tooltip("The CPU cache line size.")]
        public static int CacheLineSize
        {
            get { return Internal_GetCacheLineSize(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetCacheLineSize();

        /// <summary>
        /// Gets the current memory stats.
        /// </summary>
        [Tooltip("The current memory stats.")]
        public static MemoryStats MemoryStats
        {
            get { Internal_GetMemoryStats(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMemoryStats(out MemoryStats resultAsRef);

        /// <summary>
        /// Gets the process current memory stats.
        /// </summary>
        [Tooltip("The process current memory stats.")]
        public static ProcessMemoryStats ProcessMemoryStats
        {
            get { Internal_GetProcessMemoryStats(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetProcessMemoryStats(out ProcessMemoryStats resultAsRef);

        /// <summary>
        /// Gets the current process unique identifier.
        /// </summary>
        [Tooltip("The current process unique identifier.")]
        public static ulong CurrentProcessId
        {
            get { return Internal_GetCurrentProcessId(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetCurrentProcessId();

        /// <summary>
        /// Gets the current thread unique identifier.
        /// </summary>
        [Tooltip("The current thread unique identifier.")]
        public static ulong CurrentThreadID
        {
            get { return Internal_GetCurrentThreadID(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetCurrentThreadID();

        /// <summary>
        /// Gets the current time in seconds.
        /// </summary>
        [Tooltip("The current time in seconds.")]
        public static double TimeSeconds
        {
            get { return Internal_GetTimeSeconds(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern double Internal_GetTimeSeconds();

        /// <summary>
        /// Gets the current time as CPU cycles counter.
        /// </summary>
        [Tooltip("The current time as CPU cycles counter.")]
        public static ulong TimeCycles
        {
            get { return Internal_GetTimeCycles(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetTimeCycles();

        /// <summary>
        /// Gets the system clock frequency.
        /// </summary>
        [Tooltip("The system clock frequency.")]
        public static ulong ClockFrequency
        {
            get { return Internal_GetClockFrequency(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetClockFrequency();

        /// <summary>
        /// Gets the screen DPI setting.
        /// </summary>
        [Tooltip("The screen DPI setting.")]
        public static int Dpi
        {
            get { return Internal_GetDpi(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetDpi();

        /// <summary>
        /// Gets the screen DPI setting scale factor (1 is default).
        /// </summary>
        [Tooltip("The screen DPI setting scale factor (1 is default).")]
        public static float DpiScale
        {
            get { return Internal_GetDpiScale(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDpiScale();

        /// <summary>
        /// Gets the current network connection type.
        /// </summary>
        [Tooltip("The current network connection type.")]
        public static NetworkConnectionType NetworkConnectionType
        {
            get { return Internal_GetNetworkConnectionType(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern NetworkConnectionType Internal_GetNetworkConnectionType();

        /// <summary>
        /// Gets the current  locale culture (eg. "pl-PL" or "en-US").
        /// </summary>
        [Tooltip("The current  locale culture (eg. \"pl-PL\" or \"en-US\").")]
        public static string UserLocaleName
        {
            get { return Internal_GetUserLocaleName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetUserLocaleName();

        /// <summary>
        /// Gets the computer machine name.
        /// </summary>
        [Tooltip("The computer machine name.")]
        public static string ComputerName
        {
            get { return Internal_GetComputerName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetComputerName();

        /// <summary>
        /// Gets the user name.
        /// </summary>
        [Tooltip("The user name.")]
        public static string UserName
        {
            get { return Internal_GetUserName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetUserName();

        /// <summary>
        /// Returns true if app has user focus.
        /// </summary>
        [Tooltip("Returns true if app has user focus.")]
        public static bool HasFocus
        {
            get { return Internal_GetHasFocus(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetHasFocus();

        /// <summary>
        /// Gets size of the primary desktop.
        /// </summary>
        [Tooltip("Gets size of the primary desktop.")]
        public static Vector2 DesktopSize
        {
            get { Internal_GetDesktopSize(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDesktopSize(out Vector2 resultAsRef);

        /// <summary>
        /// Gets virtual bounds of the desktop made of all the monitors outputs attached.
        /// </summary>
        [Tooltip("Gets virtual bounds of the desktop made of all the monitors outputs attached.")]
        public static Rectangle VirtualDesktopBounds
        {
            get { Internal_GetVirtualDesktopBounds(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVirtualDesktopBounds(out Rectangle resultAsRef);

        /// <summary>
        /// Gets virtual size of the desktop made of all the monitors outputs attached.
        /// </summary>
        [Tooltip("Gets virtual size of the desktop made of all the monitors outputs attached.")]
        public static Vector2 VirtualDesktopSize
        {
            get { Internal_GetVirtualDesktopSize(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVirtualDesktopSize(out Vector2 resultAsRef);

        /// <summary>
        /// Gets full path of the main engine directory.
        /// </summary>
        [Tooltip("Gets full path of the main engine directory.")]
        public static string MainDirectory
        {
            get { return Internal_GetMainDirectory(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetMainDirectory();

        /// <summary>
        /// Gets full path of the main engine executable file.
        /// </summary>
        [Tooltip("Gets full path of the main engine executable file.")]
        public static string ExecutableFilePath
        {
            get { return Internal_GetExecutableFilePath(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetExecutableFilePath();

        /// <summary>
        /// Gets the (almost) unique ID of the current user device.
        /// </summary>
        [Tooltip("The (almost) unique ID of the current user device.")]
        public static Guid UniqueDeviceId
        {
            get { Internal_GetUniqueDeviceId(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetUniqueDeviceId(out Guid resultAsRef);

        /// <summary>
        /// Gets the current working directory of the process.
        /// </summary>
        [Tooltip("The current working directory of the process.")]
        public static string WorkingDirectory
        {
            get { return Internal_GetWorkingDirectory(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetWorkingDirectory();

        /// <summary>
        /// Shows the fatal error message to the user.
        /// </summary>
        /// <param name="msg">The message content.</param>
        public static void Fatal(string msg)
        {
            Internal_Fatal(msg);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Fatal(string msg);

        /// <summary>
        /// Shows the error message to the user.
        /// </summary>
        /// <param name="msg">The message content.</param>
        public static void Error(string msg)
        {
            Internal_Error(msg);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Error(string msg);

        /// <summary>
        /// Shows the warning message to the user.
        /// </summary>
        /// <param name="msg">The message content.</param>
        public static void Warning(string msg)
        {
            Internal_Warning(msg);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Warning(string msg);

        /// <summary>
        /// Shows the information message to the user.
        /// </summary>
        /// <param name="msg">The message content.</param>
        public static void Info(string msg)
        {
            Internal_Info(msg);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Info(string msg);

        /// <summary>
        /// Returns a value indicating whether can open a given URL in a web browser.
        /// </summary>
        /// <param name="url">The URI to assign to web browser.</param>
        /// <returns>True if can open URL, otherwise false.</returns>
        public static bool CanOpenUrl(string url)
        {
            return Internal_CanOpenUrl(url);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CanOpenUrl(string url);

        /// <summary>
        /// Launches a web browser and opens a given URL.
        /// </summary>
        /// <param name="url">The URI to assign to web browser.</param>
        public static void OpenUrl(string url)
        {
            Internal_OpenUrl(url);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OpenUrl(string url);

        /// <summary>
        /// Gets the origin position and size of the monitor at the given screen-space location.
        /// </summary>
        /// <param name="screenPos">The screen position (in pixels).</param>
        /// <returns>The monitor bounds.</returns>
        public static Rectangle GetMonitorBounds(Vector2 screenPos)
        {
            Internal_GetMonitorBounds(ref screenPos, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMonitorBounds(ref Vector2 screenPos, out Rectangle resultAsRef);

        /// <summary>
        /// Starts a new process.
        /// </summary>
        /// <param name="filename">The path to the file.</param>
        /// <param name="args">Custom arguments for command line</param>
        /// <param name="workingDir">The custom name of the working directory</param>
        /// <param name="hiddenWindow">True if start process with hidden window</param>
        /// <param name="waitForEnd">True if wait for process competition</param>
        /// <returns>Retrieves the termination status of the specified process. Valid only if processed ended.</returns>
        public static int StartProcess(string filename, string args, string workingDir, bool hiddenWindow = false, bool waitForEnd = false)
        {
            return Internal_StartProcess(filename, args, workingDir, hiddenWindow, waitForEnd);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_StartProcess(string filename, string args, string workingDir, bool hiddenWindow, bool waitForEnd);

        /// <summary>
        /// Starts a new process. Waits for it's end and captures its output.
        /// </summary>
        /// <param name="cmdLine">Command line to execute</param>
        /// <param name="workingDir">The custom path of the working directory.</param>
        /// <param name="hiddenWindow">True if start process with hidden window.</param>
        /// <returns>Retrieves the termination status of the specified process. Valid only if processed ended.</returns>
        public static int RunProcess(string cmdLine, string workingDir, bool hiddenWindow = true)
        {
            return Internal_RunProcess(cmdLine, workingDir, hiddenWindow);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_RunProcess(string cmdLine, string workingDir, bool hiddenWindow);

        /// <summary>
        /// Starts a new process. Waits for it's end and captures its output.
        /// </summary>
        /// <param name="cmdLine">Command line to execute</param>
        /// <param name="workingDir">The custom path of the working directory.</param>
        /// <param name="environment">The process environment variables. If null the current process environment is used.</param>
        /// <param name="hiddenWindow">True if start process with hidden window.</param>
        /// <returns>Retrieves the termination status of the specified process. Valid only if processed ended.</returns>
        public static int RunProcess(string cmdLine, string workingDir, System.Collections.Generic.Dictionary<string, string> environment, bool hiddenWindow = true)
        {
            return Internal_RunProcess1(cmdLine, workingDir, environment, hiddenWindow);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_RunProcess1(string cmdLine, string workingDir, System.Collections.Generic.Dictionary<string, string> environment, bool hiddenWindow);

        /// <summary>
        /// Creates the window.
        /// </summary>
        /// <param name="settings">The window settings.</param>
        /// <returns>The created native window object or null if failed.</returns>
        public static Window CreateWindow(ref CreateWindowSettings settings)
        {
            return Internal_CreateWindow(ref settings);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Window Internal_CreateWindow(ref CreateWindowSettings settings);
    }
}
