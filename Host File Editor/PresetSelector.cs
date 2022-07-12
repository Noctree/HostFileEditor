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
    public partial class PresetSelector : Dialog
    {
        public string SelectedPresetName { get; private set; }
        public string SelectedPresetLink { get; private set; }
        public IEnumerable<IHostFileContent> HostFileContent { get; private set; }
        public DialogResult Result { get; private set; }

        public PresetSelector() {
            BuildUI();
            BuildProgressUI();
        }

        private void OnPresetSelected(object? sender, EventArgs e) {
            int index = presetSelect.SelectedIndex;
            SelectedPresetName = presets[index].Item1;
            SelectedPresetLink = presets[index].Item2;
        }

        private void OnCancel(object? sender, EventArgs e) {
            Result = DialogResult.Cancel;
            Close();
        }

        private void OnConfirm(object? sender, EventArgs e) {
            Result = DialogResult.Ok;
            Download();
            Close();
        }

        private void Download() {
            Content = progressLayout;
            string presetName = SelectedPresetName;
            string presetLocation = SelectedPresetLink;
            progressBar.Value = 0;
            Title = "Preset downloader - " + presetName;
            progressDescription.Text = $"Downloading preset from {presetLocation}...";
            var request = WebRequest.Create(new Uri(presetLocation));
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
            Content = layout;
        }
    }
}
