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
        private bool closed = false;
        public PresetDownloader() {
            BuildUI();
        }

        public new void ShowModal() => throw new NotSupportedException();
        public void DownloadPreset(string presetName, Uri presetLocation) {
            this.presetName = presetName;
            this.presetLocation = presetLocation;
            closed = false;
            Task.Run(async () => { await Task.Delay(10); Download(); });
            base.ShowModal();
        }

        private void Download() {
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
            progressDescription.Text = "Finished";
            Thread.Sleep(250);
        }
    }
}
