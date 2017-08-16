////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.SceneGraph;
using FlaxEngine.Json;
using Newtonsoft.Json;

namespace FlaxEditor
{
    internal class SceneTreeNodeConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guid id = Guid.Empty;
            if (value is SceneGraphNode obj)
                id = obj.ID;

            writer.WriteValue(id.ToString("N"));
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var id = Guid.Parse((string)reader.Value);
                return SceneGraphFactory.FindNode(id);
            }

            return null;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(SceneGraphNode).IsAssignableFrom(objectType);
        }
    }

    /// <summary>
    /// Base class for <see cref="IUndoAction"/> implementations. Stores undo data serialized and preserves references to the game objects.
    /// </summary>
    /// <typeparam name="TData">The type of the data. Must have <see cref="SerializableAttribute"/>.</typeparam>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public abstract class UndoActionBase<TData> : IUndoAction where TData : struct
    {
        private string _data;

        /// <summary>
        /// Gets or sets the serialized undo data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TData Data
        {
            get => JsonConvert.DeserializeObject<TData>(_data, InternalJsonSerializer.Settings);
            protected set => _data = JsonConvert.SerializeObject(value, Formatting.None, InternalJsonSerializer.Settings);
        }

        /// <inheritdoc />
        public abstract string ActionString { get; }

        /// <inheritdoc />
        public abstract void Do();

        /// <inheritdoc />
        public abstract void Undo();
    }
}
