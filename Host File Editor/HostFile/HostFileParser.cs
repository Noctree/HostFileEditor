using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Host_File_Editor
{
    internal static class HostFileParser
    {
        private const string WindowsHostFileLocation = "drivers/etc/hosts";
        public enum Platform { Windows, Linux }

        public static bool ParseHostFile(Platform platform, out List<IHostFileEntry> result) {
            result = new List<IHostFileEntry>();
            if (platform == Platform.Windows)
                return ParseWindowsHostFile(result);
            else if (platform == Platform.Linux)
                return ParseLinuxHostFile();
            else
                throw new NotSupportedException();
        }

        private static bool ParseWindowsHostFile(List<IHostFileEntry> output) {
            var sb = new StringBuilder();
            string path = Path.Combine(Environment.SystemDirectory, WindowsHostFileLocation);
            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream) {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                bool isComment = line.StartsWith('#');
                var split = line.Split(' ', 3);
                if (isComment)
                    split[0] = split[0].Substring(1);
                if (line.StartsWith('#') && !AddressVerifier.IsValidAddress(split[0])) {
                    line = line.Substring(1);
                    sb.AppendLine(line);
                }
                else {
                    if (sb.Length > 0) {
                        output.Add(new HostFileComment(sb.ToString()));
                        sb.Clear();
                    }
                    bool enabled = !line.StartsWith('#');
                    HostFileComment? comment = null;
                    if (split.Length == 3)
                        comment = new HostFileComment(split[2]);
                    if (!HostFileEntry.TryCreate(split[1], split[0], comment, enabled, out var entry) || entry is null)
                        return false;
                    output.Add(entry);
                }
            }
            if (sb.Length > 0)
                output.Add(new HostFileComment(sb.ToString()));
            return true;
        }

        private static bool ParseLinuxHostFile() {
            return false;
        }
    }
}
