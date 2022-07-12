using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    public partial class PresetSelector
    {
        private static List<(string, string)> presets = new List<(string, string)>() {
            ("Dan Pollock's Host File", "https://someonewhocares.org/hosts/hosts"),
            ("Dan Pollock's Host File (Zero)", "https://someonewhocares.org/hosts/zero/hosts")
        };

        private void BuildUI() {
            Title = "Select Preset";
            MinimumSize = new Eto.Drawing.Size(150, 50);
            SuspendLayout();

            layout = new TableLayout(2, 2);

            Label l1 = new Label() { Text = "Presets: " };
            presetSelect = new DropDown();
            foreach (var preset in presets)
                presetSelect.Items.Add(preset.Item1);
            presetSelect.SelectedValueBinding.DataValueChanged += OnPresetSelected;

            ConfirmButton = new Button() { Text = "Append Preset" };
            CancelButton = new Button() { Text = "Cancel" };

            Content = layout;
            layout.SuspendLayout();
            layout.Add(l1, 0, 0);
            layout.Add(presetSelect, 1, 0);
            layout.Add(ConfirmButton, 1, 1);
            layout.Add(CancelButton, 0, 1);

            layout.ResumeLayout();

            ResumeLayout();

            CancelButton.Click += OnCancel;
            ConfirmButton.Click += OnConfirm;
        }
        private void BuildProgressUI() {
            MinimumSize = new Eto.Drawing.Size(100, 50);
            progressLayout = new TableLayout(1, 2);
            progressBar = new ProgressBar() { Indeterminate = true };
            progressDescription = new Label();

            progressLayout.SuspendLayout();
            progressLayout.Add(progressBar, 0, 0);
            progressLayout.Add(progressDescription, 0, 1);
            progressLayout.ResumeLayout();
        }

        TableLayout progressLayout;
        ProgressBar progressBar;
        Label progressDescription;

        TableLayout layout;
        DropDown presetSelect;
        Button ConfirmButton;
        Button CancelButton;
    }
}
