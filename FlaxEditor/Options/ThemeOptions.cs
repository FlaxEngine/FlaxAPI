using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Theme options data container object.
    /// </summary>
    [CustomEditor(typeof(Editor<ThemeOptions>))]
    public sealed class ThemeOptions
    {
        /// <summary>
        /// Currently selected style
        /// </summary>
        public string SelectedStyle = "Default";

        /// <summary>
        /// All available styles
        /// </summary>
        public Dictionary<string, Style> Styles { get; set; } = new Dictionary<string, Style>();
    }
}
