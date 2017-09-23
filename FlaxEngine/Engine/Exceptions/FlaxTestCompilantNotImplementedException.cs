// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

#if UNIT_TEST_COMPILANT
using System;
using System.Runtime.Serialization;

namespace FlaxEngine
{
    /// <summary>
    /// Inner Flax exception for auto generated code to display proper message with Internal calls while UnitTest with method should be conducted.
    /// </summary>
    public class FlaxTestCompilantNotImplementedException : Exception
    {
        /// <summary>
        /// Default exception with text that current method is not supported with unit tests
        /// </summary>
        public FlaxTestCompilantNotImplementedException()
            : base("Unit tests, don't support methods calls. Only properties can be get or set.")
        {
        }

        /// <summary>
        /// Exception with custom message
        /// </summary>
        /// <param name="message">Custom message</param>
        public FlaxTestCompilantNotImplementedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Exception with custom message and Inner Exception rethrow
        /// </summary>
        /// <param name="message">Custom message</param>
        /// <param name="inner">Inner exception</param>
        public FlaxTestCompilantNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// FlaxTestCompilantNotImplementedException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FlaxTestCompilantNotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
#endif