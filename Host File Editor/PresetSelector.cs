using System;
using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    public partial class PresetSelector : Dialog
    {
        public string SelectedPresetName { get; private set; }
        public string SelectedPresetLink { get; private set; }
        public DialogResult Result { get; private set; }

        public PresetSelector() {
            BuildUI();
        }

        private void OnCancel(object? sender, EventArgs e) {
            Result = DialogResult.Cancel;
            Close();
        }

        private void OnConfirm(object? sender, EventArgs e) {
            Result = DialogResult.Ok;
            var preset = presets[presetSelect.SelectedIndex];
            SelectedPresetName = preset.Item1;
            SelectedPresetLink = preset.Item2;
            Close();
        }
    }
}
