using System;
using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    public partial class PresetDownloader
    {
        private void BuildUI() {
            MinimumSize = new Eto.Drawing.Size(100, 50);
            SuspendLayout();

            layout = new TableLayout(1, 2);
            progressBar = new ProgressBar() { Indeterminate = true };
            progressDescription = new Label();

            layout.SuspendLayout();
            layout.Add(progressBar, 0, 0);
            layout.Add(progressDescription, 0, 1);
            layout.ResumeLayout();

            Content = layout;

            ResumeLayout();
        }

        TableLayout layout;
        ProgressBar progressBar;
        Label progressDescription;
    }
}
