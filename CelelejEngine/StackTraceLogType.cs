// Celelej Game Engine scripting API

namespace CelelejEngine
{
    /// <summary>
    /// Stack trace logging options.
    /// </summary>
    public enum StackTraceLogType
    {
        /// <summary>
        /// No stack trace will be outputed to log.
        /// </summary>
        None,

        /// <summary>
        /// Only managed stack trace will be outputed.
        /// </summary>
        ScriptOnly,

        /// <summary>
        /// Native and managed stack trace will be logged.
        /// </summary>
        Full
    }
}
