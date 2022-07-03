using System;
using System.Collections.Generic;
using System.Text;

namespace Host_File_Editor
{
    public class HostFileComment : IHostFileEntry
    {
        private static StringBuilder sb = new StringBuilder();
        public string PrettyComment { get; }
        public string Comment { get; }
        public HostFileComment(string comment) {
            Comment = comment;
            if (Comment.StartsWith('#'))
                Comment = Comment.Remove(0, 1);
            PrettyComment = Comment.Trim();
        }

        public string Serialize() {
            sb.Clear();
            sb.Append('#');
            foreach (var ch in Comment) {
                sb.Append(ch);
                if (ch == '\n')
                    sb.Append('#');
            }
            if (sb.Length >= 2 && sb[sb.Length - 1] == '#' && sb[sb.Length - 2] == '\n')
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
