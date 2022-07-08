using System;
using System.Collections.Generic;
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
            presetSelect.DataStore = presets as IEnumerable<object>;

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

        TableLayout layout;
        DropDown presetSelect;
        Button ConfirmButton;
        Button CancelButton;
    }
}
