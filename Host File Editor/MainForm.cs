using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    internal partial class MainForm : Form
    {
        private HostFileParser.Platform platform;
        private Version Version = new Version(0, 1);

        private List<IHostFileContent> entries;
        private ExtendedObservableCollection<IHostFileContent> tableEntries;

        private EntryEditor editor = new EntryEditor();
        private PresetSelector presetSelector = new PresetSelector();
        private PresetDownloader presetDownloader = new PresetDownloader();

        public MainForm() {
            BuildUI();
            Title = $"Host File Editor v{Version}";
        }

        protected override void OnShown(EventArgs e) {
            LoadHostFile();
            base.OnShown(e);
        }

        private void LoadHostFile() {
            if (EtoEnvironment.Platform.IsWindows)
                platform = HostFileParser.Platform.Windows;
            else if (EtoEnvironment.Platform.IsLinux)
                platform = HostFileParser.Platform.Linux;
            else {
                MessageBox.Show("Your platform is not supported");
                Close();
                return;
            }
            try {
                entries = HostFileParser.LoadPlatformHostFile(platform);
            } catch (Exception) {
                MessageBox.Show($"Couldn't read the hostfile, make sure the program is running as {(platform == HostFileParser.Platform.Windows ? "Administrator" : "Root")}", "Error", MessageBoxType.Error);
                Close();
                return;
            }
            tableEntries = new ExtendedObservableCollection<IHostFileContent>(entries.Where(entry => entry is IHostFileEntry));
            RoutingTableView.DataStore = tableEntries;
        }

        private void EditEntry(object? sender, GridCellMouseEventArgs e) {
            var item = e.Item;
            if (item is HostFileEntry entry) {
                editor.ShowModal(entry);
                RoutingTableView.Invalidate();
            }
        }

        private void SaveFile() {
            try {
                HostFileParser.SavePlatformHostFile(platform, entries);
            } catch (Exception) {
                MessageBox.Show($"Couldn't save the hostfile, make sure the program is running as {(platform == HostFileParser.Platform.Windows ? "Administrator" : "Root")}", "Error", MessageBoxType.Error);
                return;
            }
            MessageBox.Show("Hostfile saved successfully!", "Success");
        }

        private void PickPreset() {
            presetSelector.ShowModal();
            if (presetSelector.Result == DialogResult.Ok) {
                Application.Instance.RunIteration();
                presetDownloader.ShowModal(presetSelector.SelectedPresetName, new Uri(presetSelector.SelectedPresetLink));
                entries.AddRange(presetDownloader.HostFileContent);
                tableEntries.AddRange(presetDownloader.HostFileContent.Where(entry => entry is IHostFileEntry));
            }
        }

        private void AddEntry() {
            HostFileEntry entry = new HostFileEntry("0.0.0.0", "0.0.0.0", null, true);
            editor.ShowModal(entry);
            if (editor.Result == DialogResult.Ok) {
                entries.Add(entry);
                tableEntries.Add(entry);
            }
        }

        private void DeleteEntry() {
            if (MessageBox.Show("Delete selected entries?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxType.Warning) == DialogResult.Ok) {
                foreach (var index in RoutingTableView.SelectedItems) {
                    entries.Remove((IHostFileContent)index);
                    tableEntries.Remove((IHostFileContent)index);
                }
            }
        }
    }
}
