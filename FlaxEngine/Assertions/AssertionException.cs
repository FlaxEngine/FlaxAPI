using System;

namespace FlaxEngine.Assertions
{
    /// <summary>
    /// An exception that is thrown on a failure. Assertions.Assert._raiseExceptions needs to be set to true.
    /// </summary>
    public class AssertionException : Exception
    {
        private string _userMessage;

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (_userMessage != null)
                    message = string.Concat(message, '\n', _userMessage);
                return message;
            }
        }

        public AssertionException(string message, string userMessage)
            : base(message)
        {
            _userMessage = userMessage;
        }
    }
}
