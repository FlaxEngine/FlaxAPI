using System;
using System.Runtime.Serialization;

namespace CelelejEngine
{
    [Serializable]
    public class CelelejException : SystemException
    {
        public CelelejException()
            : base("A Celelej Runtime error occurred!")
        {
        }

        public CelelejException(string message)
            : base(message)
        {
        }

        public CelelejException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CelelejException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
