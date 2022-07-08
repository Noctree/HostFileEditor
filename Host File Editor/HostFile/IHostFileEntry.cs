using System;
using System.Collections.Generic;
using System.Text;

namespace Host_File_Editor
{
    public interface IHostFileEntry : IHostFileContent
    {
        public bool Enabled { get; set; }
        public string Source { get; set; }
        public bool SourceValid { get; }
        public string Destination { get; set; }
        public bool DestinationValid { get; }
        public IHostFileComment? Comment { get; set; }
    }
}
