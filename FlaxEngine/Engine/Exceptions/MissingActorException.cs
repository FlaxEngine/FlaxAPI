// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace FlaxEngine
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
