using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Host_File_Editor
{
    internal static class HostFileParser
    {
        private const string WindowsHostFileLocationConst = "drivers/etc/hosts";
        private const string LinuxHostFileLocation = "/etc/hosts";
        private static string WindowsHostFileLocation => Path.Combine(Environment.SystemDirectory, WindowsHostFileLocationConst);
        public enum Platform { Windows, Linux }

        public static List<IHostFileContent> LoadPlatformHostFile(Platform platform) {
            if (platform == Platform.Windows)
                return LoadWindowsHostFile();
            else if (platform == Platform.Linux)
                return LoadLinuxHostFile();
            else
                throw new PlatformNotSupportedException();
        }

        private static List<IHostFileContent> LoadWindowsHostFile() {
            using var stream = File.Open(WindowsHostFileLocation, FileMode.Open, FileAccess.Read);
            return LoadHostFile(stream);
        }
        private static List<IHostFileContent> LoadLinuxHostFile() {
            using var stream = File.Open(LinuxHostFileLocation, FileMode.OpenOrCreate, FileAccess.Read);
            return LoadHostFile(stream);
        }

        public static List<IHostFileContent> LoadHostFile(Stream stream) {
            List<IHostFileContent> output = new List<IHostFileContent>();
            var sb = new StringBuilder();
            using var reader = new StreamReader(stream);
            int whitespaceCounter = 0;
            while (!reader.EndOfStream) {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line)) {
                    ++whitespaceCounter;
                    continue;
                } else if (whitespaceCounter > 0) {
                    if (sb.Length > 0) {
                        output.Add(new HostFileComment(sb.ToString()));
                        sb.Clear();
                    }
                    output.Add(new HostFileWhitespace(whitespaceCounter));
                    whitespaceCounter = 0;
                }
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
                    output.Add(new HostFileEntry(split[1], split[0], comment, enabled));
                }
            }
            if (sb.Length > 0)
                output.Add(new HostFileComment(sb.ToString()));
            return output;
        }

        public static void SavePlatformHostFile(Platform platform, IList<IHostFileContent> content) {
            if (platform == Platform.Windows) {
                using var stream = File.Open(WindowsHostFileLocation, FileMode.Truncate, FileAccess.Write);
                SaveHostFile(stream, content);
            }
            else if (platform == Platform.Linux) {
                using var stream = File.Open(LinuxHostFileLocation, FileMode.Truncate, FileAccess.Write);
                SaveHostFile(stream, content);
            }
            else
                throw new PlatformNotSupportedException();
        }

        public static void SaveHostFile(Stream stream, IList<IHostFileContent> content) {
            using var writer = new StreamWriter(stream);
            foreach (var entry in content) {
                if (entry is HostFileWhitespace space)
                    space.WhitespaceCount -= 1;
                writer.WriteLine(entry.Serialize());
            }
        }
    }
}
