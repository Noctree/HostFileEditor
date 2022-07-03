using System;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args) {
            Console.WriteLine("Parse success? " + HostFileParser.ParseHostFile(HostFileParser.Platform.Windows, out var parsedFile));
            foreach (var entry in parsedFile)
                Console.WriteLine(entry.Serialize());
            Console.ReadKey();

            //Application app = new Application();
            //app.Run(new MainForm());
        }
    }
}
