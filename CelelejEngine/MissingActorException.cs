using System;
using System.Runtime.Serialization;

namespace CelelejEngine
{
    [Serializable]
    public class MissingActorException : SystemException
    {
        public MissingActorException()
            : base("Missing Actor exception.")
        {
        }

        public MissingActorException(string message)
            : base(message)
        {
        }

        public MissingActorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MissingActorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
