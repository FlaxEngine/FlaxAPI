// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor.States
{
    /// <summary>
    /// Operation cannot be performed in the current editor state.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidStateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStateException"/> class.
        /// </summary>
        /// <param name="msg">The message.</param>
        public InvalidStateException(string msg)
        : base(msg)
        {
        }
    }
}
