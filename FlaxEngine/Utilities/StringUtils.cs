// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Text.RegularExpressions;

namespace FlaxEngine
{
    /// <summary>
    /// String utilities class.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Parse hexadecimals digit to value.
        /// </summary>
        /// <param name="c">The hex character.</param>
        /// <returns>Value.</returns>
        public static int HexDigit(char c)
        {
            int result;

            if (c >= '0' && c <= '9')
            {
                result = c - '0';
            }
            else if (c >= 'a' && c <= 'f')
            {
                result = c + 10 - 'a';
            }
            else if (c >= 'A' && c <= 'F')
            {
                result = c + 10 - 'A';
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Removes extension from the file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Path without extension.</returns>
        public static string GetPathWithoutExtension(string path)
        {
            int num = path.LastIndexOf('.');
            if (num != -1)
            {
                return path.Substring(0, num);
            }
            return path;
        }

        /// <summary>
        /// Normalizes the path to the standard Flax format (all separators are '/' except for drive 'C:\').
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The normalized path.</returns>
        public static string NormalizePath(string path)
        {
            var chars = path.ToCharArray();

            // Convert all '\' to '/'
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\\')
                    chars[i] = '/';
            }

            // Fix case 'C:/' to 'C:\'
            if (chars.Length > 2 && !char.IsDigit(chars[0]) && chars[1] == ':')
            {
                chars[2] = '\\';
            }

            return new string(chars);
        }

        /// <summary>
        /// Normalizes the file extension to common format: no leading dot and all lowercase.
        /// For example: '.TxT' will return 'txt'.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The normalized extension.</returns>
        public static string NormalizeExtension(string extension)
        {
            if (extension[0] == '.')
                extension = extension.Remove(0, 1);
            return extension.ToLower();
        }

        /// <summary>
        /// Combines the paths.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The combined path</returns>
        public static string CombinePaths(string left, string right)
        {
            int cnt = left.Length;
            if (cnt > 1 && left[cnt - 1] != '/' && left[cnt - 1] != '\\'
                && (right.Length == 0 || (right[0] != '/' && right[0] != '\\')))
            {
                left += '/';
            }
            return left + right;
        }

        /// <summary>
        /// Determines whether the specified path is relative or is absolute.
        /// </summary>
        /// <param name="path">The input path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is relative; otherwise, <c>false</c> if is relative.
        /// </returns>
        public static bool IsRelative(string path)
        {
            bool isRooted = (path.Length >= 2 && char.IsLetterOrDigit(path[0]) && path[1] == ':') ||
                            path.StartsWith("\\\\") ||
                            path.StartsWith("/") ||
                            path.StartsWith("\\") ||
                            path.StartsWith("/");
            return !isRooted;
        }

        /// <summary>
        /// Converts path relative to the engine startup folder into absolute path.
        /// </summary>
        /// <param name="path">Path relative to the engine directory.</param>
        /// <returns>Absolute path</returns>
        public static string ConvertRelativePathToAbsolute(string path)
        {
            return ConvertRelativePathToAbsolute(Globals.StartupPath, path);
        }

        /// <summary>
        /// Converts path relative to basePath into absolute path.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="path">Path relative to basePath.</param>
        /// <returns>Absolute path</returns>
        public static string ConvertRelativePathToAbsolute(string basePath, string path)
        {
            string fullyPathed;
            if (IsRelative(path))
            {
                fullyPathed = CombinePaths(basePath, path);
            }
            else
            {
                fullyPathed = path;
            }

            NormalizePath(fullyPathed);
            return fullyPathed;
        }

        /// <summary>
        /// Reverses the specified input string.
        /// </summary>
        /// <param name="s">The string to reverse.</param>
        /// <returns>The reversed string.</returns>
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static readonly Regex IncNameRegex1 = new Regex("^(\\d+)");
        private static readonly Regex IncNameRegex2 = new Regex("^\\)(\\d+)\\(");

        /// <summary>
        /// Tries to parse number in the name brackets at the end of the value and then increment it to create a new name.
        /// Supports numbers at the end without brackets.
        /// </summary>
        /// <param name="name">The input name.</param>
        /// <param name="isValid">Custom function to validate the created name.</param>
        /// <returns>The new name.</returns>
        public static string IncrementNameNumber(string name, Func<string, bool> isValid)
        {
            // Validate input name
            if (isValid == null || isValid(name))
                return name;

            // Temporary data
            int index;
            int MaxChecks = 10000;
            string result;
            string reversed = name.Reverse();

            // Find '<name><num>' case
            var match = IncNameRegex1.Match(reversed);
            if (match.Success && match.Groups.Count == 2)
            {
                // Get result
                string num = match.Groups[0].Value.Reverse();

                // Parse value
                if (int.TryParse(num, out index))
                {
                    // Get prefix
                    string prefix = name.Substring(0, name.Length - num.Length);

                    // Generate name
                    do
                    {
                        result = string.Format("{0}{1}", prefix, ++index);

                        if (MaxChecks-- < 0)
                            return name + Guid.NewGuid();
                    } while (!isValid(result));

                    if (result.Length > 0)
                        return result;
                }
            }

            // Find '<name> (<num>)' case
            match = IncNameRegex2.Match(reversed);
            if (match.Success && match.Groups.Count == 2)
            {
                // Get result
                string num = match.Groups[0].Value;
                num = num.Substring(1, num.Length - 2).Reverse();

                // Parse value
                if (int.TryParse(num, out index))
                {
                    // Get prefix
                    string prefix = name.Substring(0, name.Length - num.Length - 2);

                    // Generate name
                    do
                    {
                        result = string.Format("{0}({1})", prefix, ++index);

                        if (MaxChecks-- < 0)
                            return name + Guid.NewGuid();
                    } while (!isValid(result));

                    if (result.Length > 0)
                        return result;
                }
            }

            // Generate name
            index = 0;
            do
            {
                result = string.Format("{0} {1}", name, index++);

                if (MaxChecks-- < 0)
                    return name + Guid.NewGuid();
            } while (!isValid(result));

            return result;
        }
    }
}
