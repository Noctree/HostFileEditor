using System;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    internal class Program
    {
        [STAThread]
        static void Main() {
            var parsedFile = HostFileParser.LoadPlatformHostFile(HostFileParser.Platform.Windows);
            foreach (var entry in parsedFile)
                Console.WriteLine(entry.Serialize());
            Console.ReadKey();

            Application app = new Application(new Eto.GtkSharp.Platform());
            app.Run(new MainForm());
        }
    }
}
