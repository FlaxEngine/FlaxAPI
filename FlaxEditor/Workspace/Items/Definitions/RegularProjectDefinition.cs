// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.Workspace.Items;
using FlaxEditor.Workspace.Items.Definitions.Abstract;
using FlaxEditor.Workspace.Utils;
using FlaxEngine;

namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Regural project definition
    /// </summary>
    public sealed class RegularProjectDefinition : ProjectDefinition
    {
        private readonly EnvironmentVariablesExpander _myEnvironmentVariablesExpander;

        private string _myLocation;

        /// <inheritdoc />
        public RegularProjectDefinition(string name, string path, Guid projectGuid,
                                        Guid projectTypeGuid, IList<DefinitionItem> items, int indent,
                                        EnvironmentVariablesExpander environmentVariablesExpander)
        : base(name, path, projectGuid, projectTypeGuid, items, indent)
        {
            _myEnvironmentVariablesExpander = environmentVariablesExpander;
        }

        /// <summary>
        /// Location of the project definition
        /// </summary>
        public string Location
        {
            get
            {
                var location = _myLocation;
                if ((object)location != null)
                {
                    return location;
                }

                return _myLocation = CalculateLocation();
            }
        }

        /// <summary>
        /// Get absolute location of the <see cref="Location" />
        /// </summary>
        /// <returns></returns>
        private string CalculateLocation()
        {
            var directory = SolutionDefinition.Directory;
            try
            {
                var str = _myEnvironmentVariablesExpander.Expand(Path);
                var uri = new Uri(str, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                {
                    return System.IO.Path.Combine(directory, str);
                }

                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    return "";
                }

                return uri.Scheme == Uri.UriSchemeFile
                       ? System.IO.Path.Combine(directory, uri.LocalPath)
                       : str;
            }
            catch (ArgumentException ex)
            {
                Debug.LogException(ex);
            }
            catch (UriFormatException ex)
            {
                Debug.LogException(ex);
            }

            return String.Empty;
        }

        /// <summary>
        /// Stringify ProjectDefinition
        /// </summary>
        public override string ToString()
        {
            return $"ProjectDefinition {base.ToString()}";
        }

        /// <summary>
        /// TODO Change name
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="indent"></param>
        public override void Dump(TextWriter writer, string indent)
        {
            DumpInternal(writer, indent, "Project");
        }
    }
}
