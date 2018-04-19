// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace FlaxEngine
{
    [Serializable]
    public class UnassignedReferenceException : SystemException
    {
        public UnassignedReferenceException()
            : base("Unassigned reference exception.")
        {
        }

        public UnassignedReferenceException(string message)
            : base(message)
        {
        }

        public UnassignedReferenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnassignedReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
