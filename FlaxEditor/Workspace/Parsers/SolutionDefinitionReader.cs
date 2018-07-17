// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FlaxEditor.Utilities;
using FlaxEditor.Workspace.Items;
using FlaxEditor.Workspace.Items.Configuration;
using FlaxEditor.Workspace.Items.Definitions;
using FlaxEditor.Workspace.Items.Definitions.Abstract;
using FlaxEditor.Workspace.Utils;
using FlaxEngine;

namespace FlaxEditor.Workspace.Generation
{
    public class SolutionDefinitionReader
    {
        private const string ReVisualStudioVersionProperty = "VisualStudioVersion";
        private const string ReMinimumVisualStudioVersionProperty = "MinimumVisualStudioVersion";

        private static readonly Regex OurHeaderMatcher = ConstructRegex(SolutionDefinitionConstants.FormatVersionPrefix,
                                                                        "(?<formatVersion>\\d+\\.\\d+)");

        private static readonly Regex OurProjectMatcher = ConstructRegex(SolutionDefinitionConstants.ProjectPrefix,
                                                                         "\\(\\\"(?<package>\\{.*?\\})\\\".*?\\\"(?<name>.*?)\\\".*?\\\"(?<project>.*?)\\\".*?\\\"(?<guid>.*?)\\\"");

        private static readonly Regex OurGlobalMatcher = ConstructRegex(SolutionDefinitionConstants.GlobalPrefix, null);

        private static readonly Regex OurSectionMatcher = ConstructRegex(
            string.Format("(?<type>{0}|{1}){2}", SolutionDefinitionConstants.ProjectPrefix,
                          SolutionDefinitionConstants.GlobalPrefix, SolutionDefinitionConstants.Section),
            "\\((?<name>[^\\s]*)\\)\\s*=\\s*(?<value>.*)");

        private static readonly Regex OurPropertyMatcher =
        ConstructRegex(string.Empty, "^(?<name>[^=]+?)\\s*=\\s*(?<value>.*)$");

        private readonly string _myLocation;
        private CurrentLine _myLineForRollback;
        private int _myLineNumber;


        public SolutionDefinitionReader(string location)
        {
            _myLocation = location;
            Errors = new List<string>();
            Items = new List<DefinitionItem>();
        }

        public List<string> Errors { get; }

        public List<DefinitionItem> Items { get; }

        private void CaptureError(string errorMessage)
        {
            Errors.Add($"Line:{_myLineNumber} ({errorMessage})");
        }

        public void Read()
        {
            if (!_myLocation.IsNullOrEmpty())
            {
                if (!File.Exists(_myLocation))
                {
                    Errors.Add($"Solution file '{_myLocation}' does not exists");
                    return;
                }
            }
            try
            {
                using (var stream = File.OpenRead(_myLocation))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        ReadSolutionFile(reader, Items);
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.LogWarning($"Failed to parse solution file '{_myLocation}'");
                Debug.LogException(ex);
            }
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
                    var match1 = OurProjectMatcher.Match(currentLine);
                    if (match1.Success)
                    {
                        var definitionItemList = new List<DefinitionItem>();
                        ParseSectionOwner(reader, definitionItemList, SolutionDefinitionConstants.ProjectPrefix,
                                          SolutionDefinitionConstants.GlobalPrefix);
                        var name = match1.Groups["name"].Value;
                        var path = match1.Groups["project"].Value;
                        var guid1 = Guid.Parse(match1.Groups["guid"].Value);
                        var guid2 = Guid.Parse(match1.Groups["package"].Value);
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
                    else if (OurGlobalMatcher.Match(currentLine).Success)
                    {
                        var definitionItemList = new List<DefinitionItem>();
                        ParseSectionOwner(reader, definitionItemList, SolutionDefinitionConstants.GlobalPrefix, null);
                        items.Add(new GlobalDefinition(definitionItemList, currentLine.Indent));
                    }
                    else
                    {
                        var match2 = OurPropertyMatcher.Match(currentLine);
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
                                CaptureError($"Unexpected property '{propertyName}'");
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
                                CaptureError($"Unexpected line '{currentLine}'");
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

        private void ParseSectionOwner(StreamReader reader, ICollection<DefinitionItem> items, string owner,
                                       string nextOwner = null)
        {
            var str = string.Empty;
            CurrentLine line;
            while (true)
            {
                line = FetchNextLine(reader, items);
                if (!line.IsNull)
                {
                    if (!line.Line.Equals(SolutionDefinitionConstants.End + owner, StringComparison.OrdinalIgnoreCase))
                    {
                        var match = OurSectionMatcher.Match(line);
                        if (match.Success)
                        {
                            var items1 = new List<DefinitionItem>();
                            var name = match.Groups["type"].Value;
                            var sectionName = match.Groups["name"].Value;
                            var sectionValue = match.Groups["value"].Value;
                            str = $"{SolutionDefinitionConstants.End}{name}{SolutionDefinitionConstants.Section}";
                            ParseSection(reader, items1, name);
                            var type = (SectionDefinitionType)Enum.Parse(typeof(SectionDefinitionType), name, true);
                            items.Add(new SectionDefinition(sectionName, sectionValue, type, items1, line.Indent));
                        }
                        else if (!line.Line.StartsWith(owner, StringComparison.OrdinalIgnoreCase) &&
                                 (nextOwner == null ||
                                  !line.Line.StartsWith(nextOwner, StringComparison.OrdinalIgnoreCase)))
                        {
                            CaptureError("ProjectSection or GlobalSection is expected");
                            items.Add(new LineDefinition(line, line.Indent));
                        }
                        else
                        {
                            goto label_7;
                        }
                    }
                    else
                    {
                        goto label_9;
                    }
                }
                else
                {
                    break;
                }
            }

            CaptureError(str == string.Empty
                         ? string.Format("{0}{2} or {1}{2} is expected", SolutionDefinitionConstants.ProjectPrefix,
                                         SolutionDefinitionConstants.GlobalPrefix, SolutionDefinitionConstants.Section)
                         : $"{str} is expected");
            return;
            label_9:
            return;
            label_7:
            RollbackFetch(line);
        }

        private void ParseSection(StreamReader reader, List<DefinitionItem> items, string name)
        {
            var str1 = name + SolutionDefinitionConstants.Section;
            var str2 = SolutionDefinitionConstants.End + str1;
            var str3 = SolutionDefinitionConstants.End + name;
            CurrentLine line;
            while (true)
            {
                line = FetchNextLine(reader, items);
                if (!line.IsNull)
                {
                    if (!line.Line.Equals(str2, StringComparison.OrdinalIgnoreCase))
                    {
                        var match = OurPropertyMatcher.Match(line);
                        if (match.Success)
                        {
                            var propertyName = match.Groups[nameof(name)].Value;
                            var propertyValue = match.Groups["value"].Value;
                            items.Add(new PropertyDefinition(propertyName, propertyValue, line.Indent));
                        }
                        else if (!line.Line.StartsWith(str1, StringComparison.OrdinalIgnoreCase) &&
                                 !line.Line.StartsWith(str3, StringComparison.OrdinalIgnoreCase))
                        {
                            CaptureError("Property is expected");
                            items.Add(new LineDefinition(line, line.Indent));
                        }
                        else
                        {
                            goto label_7;
                        }
                    }
                    else
                    {
                        goto label_9;
                    }
                }
                else
                {
                    break;
                }
            }

            CaptureError($"{str2} is expected");
            return;
            label_9:
            return;
            label_7:
            RollbackFetch(line);
        }

        private void ReadHeaderLine(StreamReader reader, ICollection<DefinitionItem> items)
        {
            var line = FetchNextLine(reader, items);
            if (line.IsNull)
            {
                return;
            }

            var match = OurHeaderMatcher.Match(line.Line);
            if (match.Success)
            {
                var definitionItems = items;
                var versionDefinition =
                new FormatVersionDefinition(SolutionFormatVersion.Get(match.Groups["formatVersion"].Value),
                                            line.Indent);
                versionDefinition.Indent = line.Indent;
                definitionItems.Add(versionDefinition);
            }
            else
            {
                CaptureError("Solution file header is expected");
                RollbackFetch(line);
            }
        }

        private CurrentLine FetchNextLine(StreamReader reader, ICollection<DefinitionItem> items)
        {
            var lineForRollback = _myLineForRollback;
            if (!lineForRollback.IsNull)
            {
                _myLineForRollback = CurrentLine.Null;
                return lineForRollback;
            }

            int indent;
            string str1;
            while (true)
            {
                ++_myLineNumber;
                var str2 = reader.ReadLine();
                if (str2 != null)
                {
                    var str3 = str2.TrimStart();
                    indent = str2.Length - str3.Length;
                    str1 = str3.TrimEnd();
                    if (str1.IsEmpty())
                    {
                        items.Add(new LineDefinition(str1, indent));
                    }
                    else if (str1.StartsWith("#"))
                    {
                        items.Add(new LineDefinition(str1, indent));
                    }
                    else
                    {
                        goto label_8;
                    }
                }
                else
                {
                    break;
                }
            }

            return CurrentLine.Null;
            label_8:
            return new CurrentLine(str1, indent);
        }

        private void RollbackFetch(CurrentLine line)
        {
            _myLineForRollback = line;
        }

        private static Regex ConstructRegex(string prefix, string pattern = null)
        {
            if (pattern == null)
            {
                return new Regex(prefix, RegexOptions.Compiled);
            }

            return new Regex(prefix + pattern, RegexOptions.Compiled);
        }

        private struct CurrentLine
        {
            public static readonly CurrentLine Null = new CurrentLine(null, 0);
            public readonly string Line;
            public readonly int Indent;

            public bool IsNull => Line == null;

            public CurrentLine(string line, int indent)
            {
                Line = line;
                Indent = indent;
            }

            public static implicit operator string(CurrentLine currentLine)
            {
                return currentLine.Line;
            }

            public override string ToString()
            {
                return Line ?? "<NULL>";
            }
        }
    }
}
