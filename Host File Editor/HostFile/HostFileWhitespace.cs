using System;
using System.Collections.Generic;
using System.Text;

namespace Host_File_Editor
{
    public class HostFileWhitespace : IHostFileContent
    {
        public int WhitespaceCount { get; set; }
        public HostFileWhitespace(int count) {
            WhitespaceCount = count;
        }
        public string Serialize() => WhitespaceCount <= 0? string.Empty : new string('\n', WhitespaceCount);
    }
}
