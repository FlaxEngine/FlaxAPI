// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FlaxEditor.Workspace.Generation
{
    public partial class SolutionDefinitionReader
	{
        private const string ReVisualStudioVersionProperty = "VisualStudioVersion";
        private const string ReMinimumVisualStudioVersionProperty = "MinimumVisualStudioVersion";

        private static readonly Regex ourHeaderMatcher = ConstructRegex(SolutionDefinitionConstants.FormatVersionPrefix,
            "(?<formatVersion>\\d+\\.\\d+)");

        private static readonly Regex ourProjectMatcher = ConstructRegex(SolutionDefinitionConstants.ProjectPrefix,
            "\\(\\\"(?<package>\\{.*?\\})\\\".*?\\\"(?<name>.*?)\\\".*?\\\"(?<project>.*?)\\\".*?\\\"(?<guid>.*?)\\\"");

        private static readonly Regex ourGlobalMatcher = ConstructRegex(SolutionDefinitionConstants.GlobalPrefix, null);

        private static readonly Regex ourSectionMatcher = ConstructRegex(
            string.Format("(?<type>{0}|{1}){2}", SolutionDefinitionConstants.ProjectPrefix,
                SolutionDefinitionConstants.GlobalPrefix, SolutionDefinitionConstants.Section),
            "\\((?<name>[^\\s]*)\\)\\s*=\\s*(?<value>.*)");

        private static readonly Regex ourPropertyMatcher =
            ConstructRegex(string.Empty, "^(?<name>[^=]+?)\\s*=\\s*(?<value>.*)$");


        private static Regex ConstructRegex(string prefix, string pattern = null)
        {
            if (pattern == null)
            {
                return new Regex(prefix, RegexOptions.Compiled);
            }

            return new Regex(prefix + pattern, RegexOptions.Compiled);
        }

        private void ReadSolutionFile(StreamReader reader, ICollection<DefinitionItem> items)
        {
            ReadHeaderLine(reader, items);
            var environmentVariablesExpander = new EnvironmentVariablesExpander();
            while (true)
            {
                var currentLine = FetchNextLine(reader, items);
                if (!currentLine.IsNull)
                {
                    var match1 = ourProjectMatcher.Match(currentLine);
                    if (match1.Success)
                    {
                        var definitionItemList = new List<DefinitionItem>();
                        ParseSectionOwner(reader, definitionItemList, SolutionDefinitionConstants.ProjectPrefix,
                            SolutionDefinitionConstants.GlobalPrefix);
                        var name = match1.Groups["name"].Value;
                        var path = match1.Groups["project"].Value;
                        var guid1 = GuidExtensions.TryParseGuid(match1.Groups["guid"].Value);
                        var guid2 = GuidExtensions.TryParseGuid(match1.Groups["package"].Value);
                        if (guid2 == SolutionFolderDefinition.TypeGuid)
                        {
                            items.Add(new SolutionFolderDefinition(name, path, guid1, definitionItemList,
                                currentLine.Indent));
                        }
                        else
                        {
                            items.Add(new RegularProjectDefinition(name, path, guid1, guid2, definitionItemList,
                                currentLine.Indent, environmentVariablesExpander));
                        }
                    }
                    else if (ourGlobalMatcher.Match(currentLine).Success)
                    {
                        var definitionItemList = new List<DefinitionItem>();
                        ParseSectionOwner(reader, definitionItemList, SolutionDefinitionConstants.GlobalPrefix, null);
                        items.Add(new GlobalDefinition(definitionItemList, currentLine.Indent));
                    }
                    else
                    {
                        var match2 = ourPropertyMatcher.Match(currentLine);
                        if (match2.Success)
                        {
                            var propertyName = match2.Groups["name"].Value;
                            if (propertyName == "MinimumVisualStudioVersion" || propertyName == "VisualStudioVersion")
                            {
                                items.Add(new PropertyDefinition(propertyName, match2.Groups["value"].Value,
                                    currentLine.Indent));
                            }
                            else
                            {
                                CaptureError(string.Format("Unexpected property '{0}'", propertyName));
                                items.Add(new LineDefinition(currentLine, currentLine.Indent));
                            }
                        }
                        else
                        {
                            var line = currentLine.Line;
                            if (line.StartsWith("MinimumVisualStudioVersion"))
                            {
                                items.Add(new PropertyDefinition("MinimumVisualStudioVersion",
                                    line.RemoveStart("MinimumVisualStudioVersion").Trim(), currentLine.Indent));
                            }
                            else if (line.StartsWith("VisualStudioVersion"))
                            {
                                items.Add(new PropertyDefinition("VisualStudioVersion",
                                    line.RemoveStart("VisualStudioVersion").Trim(), currentLine.Indent));
                            }
                            else
                            {
                                CaptureError(string.Format("Unexpected line '{0}'", currentLine));
                                items.Add(new LineDefinition(currentLine, currentLine.Indent));
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
