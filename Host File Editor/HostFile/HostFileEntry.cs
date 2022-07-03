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
                if (!AddressVerifier.IsValidAddress(value))
                    throw new FormatException("Not a valid IPv4/IPv6 address or a domain name");
                source = value;
            }
        }
        public string Destination {
            get => destination;
            set {
                if (!AddressVerifier.IsValidAddress(value))
                    throw new FormatException("Not a valid IPv4/IPv6 address or a domain name");
                destination = value;
            }
        }
        public HostFileComment? Comment { get; set; }
        private HostFileEntry(string source, string destination, HostFileComment? comment, bool enabled) {
            Source = source;
            Destination = destination;
            Comment = comment;
            Enabled = enabled;
        }

        public static bool TryCreate(string source, string destination, HostFileComment? comment, bool enabled, out HostFileEntry? entry) {
            entry = null;
            source = source.Trim();
            destination = destination.Trim();
            if (!AddressVerifier.IsValidAddress(source) || !AddressVerifier.IsValidAddress(destination))
                return false;
            entry = new HostFileEntry(source, destination, comment, enabled);
            return true;
        }

        public string Serialize() => string.Concat((Enabled ? string.Empty : "#"), destination, " ", source, " ", (Comment is null ? string.Empty : Comment.Serialize()));
    }
}
