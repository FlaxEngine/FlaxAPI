// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Options
{
    /// <summary>
    /// Editor options data container object.
    /// </summary>
    public sealed class EditorOptions
    {
        /// <summary>
        /// The general options
        /// </summary>
        public GeneralOptions General = new GeneralOptions();

        /// <summary>
        /// The visual options.
        /// </summary>
        public VisualOptions Visual = new VisualOptions();
    }
}
