////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlaxEditor
{
    /// <summary>
    /// Base class for <see cref="IUndoAction"/> implementations. Stores undo data serialized and preserves references to the game objects.
    /// </summary>
    /// <typeparam name="TData">The type of the data. Must have <see cref="SerializableAttribute"/>.</typeparam>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public abstract class UndoActionBase<TData> : IUndoAction
    {
        //private byte[] _data;
        private TData _data;

        /// <summary>
        /// Gets or sets the serialized undo data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        protected TData Data
        {
            get
            {
                /*using (var ms = new MemoryStream(_data))
                {
                    var formatter = Formatter;
                    return (TData)formatter.Deserialize(ms);
                }*/
                return _data;
            }
            set
            {
                /*using (var ms = new MemoryStream())
                {
                    var formatter = Formatter;
                    formatter.Serialize(ms, value);
                    _data = ms.ToArray();
                }*/
                _data = value;
            }
        }

        /// <summary>
        /// Gets the formatter used to data serialization.
        /// </summary>
        /// <value>
        /// The formatter.
        /// </value>
        protected IFormatter Formatter
        {
            get
            {
                return new BinaryFormatter();
            }
        }
        
        /// <inheritdoc />
        public abstract string ActionString { get; }

        /// <inheritdoc />
        public abstract void Do();

        /// <inheritdoc />
        public abstract void Undo();
    }
}
