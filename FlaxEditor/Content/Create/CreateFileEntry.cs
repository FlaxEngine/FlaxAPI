////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content.Create
{
    /// <summary>
    /// File create entry.
    /// </summary>
    public abstract class CreateFileEntry
    {
        /// <summary>
        /// The result file path.
        /// </summary>
        public readonly string ResultUrl;

        /// <summary>
        /// Gets a value indicating whether this entry has settings to modify.
        /// </summary>
        public virtual bool HasSettings => Settings != null;

        /// <summary>
        /// Gets or sets the settings object to modify.
        /// </summary>
        public virtual object Settings => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFileEntry"/> class.
        /// </summary>
        /// <param name="resultUrl">The result file url.</param>
        protected CreateFileEntry(string resultUrl)
        {
            ResultUrl = resultUrl;
        }

        /// <summary>
        /// Creates the result file.
        /// </summary>
        /// <returns>True if failed, otherwise false.</returns>
        public abstract bool Create();
    }
}
