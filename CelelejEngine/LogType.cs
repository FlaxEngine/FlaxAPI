namespace CelelejEngine
{
    /// <summary>
    /// The type of the log message in Debug.logger.Log or delegate registered with Application.RegisterLogCallback.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// LogType used for Errors.
        /// </summary>
        Error,

        /// <summary>
        /// LogType used for Asserts. (These could also indicate an error inside Celelej itself.)
        /// </summary>
        Assert,

        /// <summary>
        /// LogType used for Warnings.
        /// </summary>
        Warning,

        /// <summary>
        /// LogType used for regular log messages.
        /// </summary>
        Log,

        /// <summary>
        /// LogType used for Exceptions.
        /// </summary>
        Exception
    }
}
