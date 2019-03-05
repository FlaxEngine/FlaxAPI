// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Options
{
    /// <summary>
    /// Editor options data container object.
    /// </summary>
    public sealed class EditorOptions
    {
        /// <summary>
        /// The general options.
        /// </summary>
        public GeneralOptions General = new GeneralOptions();

        /// <summary>
        /// The interface options.
        /// </summary>
        public InterfaceOptions Interface = new InterfaceOptions();

        /// <summary>
        /// The visual options.
        /// </summary>
        public VisualOptions Visual = new VisualOptions();

        /// <summary>
        /// The source code options.
        /// </summary>
        public SourceCodeOptions SourceCode = new SourceCodeOptions();

        /// <summary>
        /// The theme options.
        /// </summary>
        public ThemeOptions Theme = new ThemeOptions();
    }
}
