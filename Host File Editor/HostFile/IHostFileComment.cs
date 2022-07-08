using System;
using System.Collections.Generic;
using System.Text;

namespace Host_File_Editor
{
    public interface IHostFileComment : IHostFileContent
    {
        public string? Comment { get; set; }
    }
}
