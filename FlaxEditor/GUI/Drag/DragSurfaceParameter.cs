// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Visject Surface parameter collection drag handler.
    /// </summary>
    public sealed class DragSurfaceParameters : DragSurfaceParameters<DragEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragSurfaceParameters"/> class.
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragSurfaceParameters(Func<string, bool> validateFunction)
        : base(validateFunction)
        {
        }
    }

    /// <summary>
    /// Helper class for handling <see cref="Surface.SurfaceParameter"/> drag and drop.
    /// </summary>
    /// <seealso cref="AssetItem" />
    public class DragSurfaceParameters<U> : DragHelper<string, U> where U : DragEventArgs
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="FlaxEditor.Surface.SurfaceParameter"/>.
        /// </summary>
        public const string DragPrefix = "SURFPARAM!?";

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragSurfaceParameters(Func<string, bool> validateFunction)
        : base(validateFunction)
        {
        }

        /// <inheritdoc/>
        public override DragData ToDragData(string item) => GetDragData(item);

        /// <inheritdoc/>
        public override DragData ToDragData(IEnumerable<string> items) => GetDragData(items);

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The data.</returns>
        public static DragData GetDragData(string parameterName)
        {
            return new DragDataText(DragPrefix + parameterName);
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="parameterNames">The parameter names.</param>
        /// <returns>The data.</returns>
        public static DragData GetDragData(IEnumerable<string> parameterNames)
        {
            // Not supported
            return null;
        }

        /// <summary>
        /// Tries to parse the drag data to extract parameters collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public override IEnumerable<string> FromDragData(DragData data)
        {
            if (data is DragDataText dataText)
            {
                if (dataText.Text.StartsWith(DragPrefix))
                {
                    // Remove prefix and parse spitted names
                    var parameterNames = dataText.Text.Remove(0, DragPrefix.Length).Split('\n');
                    return parameterNames;
                }
            }
            return new string[0];
        }
    }
}
