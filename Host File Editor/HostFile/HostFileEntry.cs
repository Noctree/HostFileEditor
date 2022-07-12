using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Host_File_Editor
{
    public class HostFileEntry : IHostFileEntry
    {
        private string source;
        private string destination;
        public bool Enabled { get; set; }
        public string Source {
            get => source;
            set {
                SourceValid = AddressVerifier.IsValidAddress(value);
                source = value;
            }
        }
        public bool SourceValid { get; private set; }
        public string Destination {
            get => destination;
            set {
                DestinationValid = AddressVerifier.IsValidIPAddress(value);
                destination = value;
            }
        }
        public bool DestinationValid { get; private set; }
        public IHostFileComment? Comment { get; set; }
        public HostFileEntry(string source, string destination, IHostFileComment? comment, bool enabled) {
            source = source.Trim();
            destination = destination.Trim();
            Source = source;
            Destination = destination;
            Comment = comment;
            Enabled = enabled;
        }

        public string Serialize() => string.Concat((Enabled ? string.Empty : "#"), destination, " ", source, " ", (Comment is null ? string.Empty : Comment.Serialize()));
    }
}
