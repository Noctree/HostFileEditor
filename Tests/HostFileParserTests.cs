using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Host_File_Editor;

namespace Tests
{
    public class HostFileParserTests
    {
        [SetUp]
        public void SetUp() {

        }

        [Test]
        public void BasicWindowsHostFile() {
            using var file = File.OpenRead("WindowsHosts.txt");
            using var reader = new StreamReader(file);
            var content = HostFileParser.LoadHostFileV2(reader);
            foreach (var entry in content)
                Debug.WriteLine(entry.Serialize());
        }
    }
}
