using System;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    internal class Program
    {
        [STAThread]
        static void Main() {
            Application app = new Application(new Eto.GtkSharp.Platform());
            app.Run(new MainForm());
        }
    }
}
