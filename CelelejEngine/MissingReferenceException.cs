// Celelej Game Engine scripting API

using System;
using System.Runtime.Serialization;

namespace CelelejEngine
{
    [Serializable]
    public class MissingReferenceException : SystemException
    {
        public MissingReferenceException()
            : base("Missing reference exception.")
        {
        }

        public MissingReferenceException(string message)
            : base(message)
        {
        }

        public MissingReferenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MissingReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
