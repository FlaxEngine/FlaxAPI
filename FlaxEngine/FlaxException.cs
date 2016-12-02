// Flax Engine scripting API

using System;
using System.Runtime.Serialization;

namespace FlaxEngine
{
    [Serializable]
    public class FlaxException : SystemException
    {
        public FlaxException()
            : base("A Flax Runtime error occurred!")
        {
        }

        public FlaxException(string message)
            : base(message)
        {
        }

        public FlaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FlaxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
