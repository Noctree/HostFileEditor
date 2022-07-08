using System;
using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;

namespace Host_File_Editor
{
    public partial class EntryEditor : Dialog
    {
        public DialogResult Result { get; private set; }
        private HostFileEntry entry;
        public EntryEditor() {
            BuildUI();
        }

        private void OnAbort(object? sender, EventArgs e) {
            Result = DialogResult.Cancel;
            Close();
        }

        private void OnConfirm(object? sender, EventArgs e) {
            Result = DialogResult.Ok;
            SaveData();
            Close();
        }

        public new void ShowModal() => throw new NotSupportedException();
        public void ShowModal(HostFileEntry entry) {
            this.entry = entry;
            LoadData();
            base.ShowModal();
        }
        private void LoadData() {
            SourceInput.Text = entry.Source;
            DestinationInput.Text = entry.Destination;
            if (entry.Comment != null)
                CommentInput.Text = entry.Comment.Comment ?? String.Empty;
        }

        private void SaveData() {
            entry.Source = SourceInput.Text;
            entry.Destination = DestinationInput.Text;
            if (entry.Comment is null)
                entry.Comment = new HostFileComment(CommentInput.Text);
            else
                entry.Comment.Comment = CommentInput.Text;
        }
    }
}
