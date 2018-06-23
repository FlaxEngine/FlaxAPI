using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Formatter for resolving SLN version of the file
    /// </summary>
    public class SolutionFormatVersion
    {
        public const string VS_DEFAULT_TECH_VERSION = "12.00";
        public const string VS_2011_TECH_VERSION = "12.00";
        public const string VS_2010_TECH_VERSION = "11.00";
        public const string VS_2008_TECH_VERSION = "10.00";
        public const string VS_2005_TECH_VERSION = "9.00";

        /// <summary>
        /// List of known version that we currently support
        /// </summary>
        private static readonly List<SolutionFormatVersion> ourKnownVersions = new List<SolutionFormatVersion>
        {
            new SolutionFormatVersion("12.00", new Version(4, 5)),
            new SolutionFormatVersion("11.00", new Version(4, 0)),
            new SolutionFormatVersion("10.00", new Version(3, 5)),
            new SolutionFormatVersion("9.00", new Version(2, 0))
        };

        /// <summary>
        /// Predefined item of Unknown type
        /// </summary>
        public static readonly SolutionFormatVersion Unknown = new SolutionFormatVersion("UNKNOWN", null);
        /// <summary>
        /// Predefined item of latest default version used
        /// </summary>
        public static readonly SolutionFormatVersion Default = new SolutionFormatVersion(VS_DEFAULT_TECH_VERSION, null);

        private SolutionFormatVersion(string vsTechVersion, Version platformVersion)
        {
            VsTechVersion = vsTechVersion;
            MaxPlatformVersion = platformVersion;
        }

        /// <summary>
        /// Visual studio version of the SLN file format
        /// </summary>
        public string VsTechVersion { get; }

        /// <summary>
        /// TODO ???
        /// </summary>
        public Version MaxPlatformVersion { get; }

        /// <summary>
        /// Get version formatter from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SolutionFormatVersion Get(string value)
        {
            var solutionFormatVersion =
            ourKnownVersions.SingleOrDefault(version => version.VsTechVersion.Equals(value));
            if (solutionFormatVersion != null)
            {
                return solutionFormatVersion;
            }
            
            Debug.LogWarning($"Solution format version {value} is unknown.");
            return new SolutionFormatVersion(value, null);
        }
    }
}
