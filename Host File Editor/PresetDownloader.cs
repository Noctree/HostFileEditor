using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    public partial class PresetDownloader : Dialog
    {
        public IList<IHostFileContent> HostFileContent { get; private set; }
        private string presetName;
        private Uri presetLocation;
        public PresetDownloader() {
            BuildUI();
        }

        public new void ShowModal() => throw new NotSupportedException();
        public void ShowModal(string presetName, Uri presetLocation) {
            this.presetName = presetName;
            this.presetLocation = presetLocation;
            base.ShowModal();
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            progressBar.Value = 0;
            Title = "Preset downloader - " + presetName;
            progressDescription.Text = $"Downloading preset from {presetLocation}...";
            var request = WebRequest.Create(presetLocation);
            var awaiter = request.GetResponseAsync();
            while (!awaiter.IsCompleted) {
                Application.Instance.RunIteration();
                Thread.Sleep(32);
            }
            progressDescription.Text = "Parsing preset...";
            var task = Task.Run(() => HostFileContent = HostFileParser.LoadHostFile(awaiter.Result.GetResponseStream()));
            while (!task.IsCompleted) {
                Application.Instance.RunIteration();
                Thread.Sleep(32);
            }
            Close();
        }
    }
}
