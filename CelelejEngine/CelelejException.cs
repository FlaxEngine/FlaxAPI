using System;
using System.Runtime.Serialization;

namespace CelelejEngine
{
    [Serializable]
    public class CelelejException : SystemException
    {
        private string CelelejStackTrace;

        public CelelejException()
            : base("A Celelej Runtime error occurred!")
        {
            HResult = -2147467261;
        }

        public CelelejException(string message)
            : base(message)
        {
            HResult = -2147467261;
        }

        public CelelejException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = -2147467261;
        }

        protected CelelejException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
