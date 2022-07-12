using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Host_File_Editor
{
    public static class HostFileParser
    {
        private static readonly char[] separators = { ' ', '\t' };
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
            using var reader = new StreamReader(stream);
            return LoadHostFileV2(reader);
        }
        private static List<IHostFileContent> LoadLinuxHostFile() {
            using var stream = File.Open(LinuxHostFileLocation, FileMode.OpenOrCreate, FileAccess.Read);
            using var reader = new StreamReader(stream);
            return LoadHostFileV2(reader);
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
                var split = line.Split(separators, 3);
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
        
        public static List<IHostFileContent> LoadHostFileV2(StreamReader reader) {
            List<IHostFileContent> output = new List<IHostFileContent>();
            var sb = new StringBuilder();
            int whitespaceCounter = 0;

            while (!reader.EndOfStream) {
                string line = reader.ReadLine() ?? string.Empty;

                // Is line empty?
                if (string.IsNullOrEmpty(line)) {
                    whitespaceCounter++;
                    continue;
                }

                // Handle Whitespace
                if (whitespaceCounter > 0) {
                    if (sb.Length > 0) {
                        output.Add(new HostFileComment(sb.ToString()));
                        sb.Clear();
                    }
                    output.Add(new HostFileWhitespace(whitespaceCounter));
                    whitespaceCounter = 0;
                }
                
                int commentStart = line.IndexOf('#');

                // Is Host File Entry?
                //Line begins with destination address,
                //which must be an IPv4/IPv6 address, so the line must start with either a digit, or ':'
                if (StringHelper.IsDigitStrict(line[0]) || line[0] == ':') {
                    if (sb.Length > 0) {
                        output.Add(new HostFileComment(sb.ToString()));
                        sb.Clear();
                    }
                    string destination = StringHelper.TakeWord(line, 0);
                    string source = StringHelper.TakeWord(line, 1);
                    bool hasComment = commentStart != -1;
                    HostFileComment? comment = null;
                    if (hasComment) {
                        comment = new HostFileComment(line.Substring(commentStart + 1));
                    }
                    var entry = new HostFileEntry(source, destination, comment, true);
                    output.Add(entry);
                    continue;
                }

                //Verify that line is then indeed a comment
                if (commentStart == -1)
                    throw new FormatException("Host file contains a line that is not a comment, but starts with illegal characters");

                // Is Disabled Entry?
                //Disabled entries are just commented out
                string ip = StringHelper.TakeWord(line, 0).Substring(1);
                bool isDisabledEntry = AddressVerifier.IsValidAddress(ip);
                if (isDisabledEntry) {
                    if (sb.Length > 0) {
                        output.Add(new HostFileComment(sb.ToString()));
                        sb.Clear();
                    }
                    string destination = ip;
                    string source = StringHelper.TakeWord(line, 1);
                    int DeCommentStart = line.IndexOf('#', commentStart + 1);
                    bool hasComment = DeCommentStart != -1;
                    HostFileComment? comment = null;
                    if (hasComment) {
                        comment = new HostFileComment(line.Substring(DeCommentStart + 1));
                    }
                    var entry = new HostFileEntry(source, destination, comment, false);
                    output.Add(entry);
                }
                // Just a comment
                else {
                    sb.AppendLine(line);
                }
            }

            //If file ends with a comment, append it
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
