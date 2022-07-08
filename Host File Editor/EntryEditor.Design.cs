using System;
using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Host_File_Editor
{
    public partial class EntryEditor
    {
        private void BuildUI() {
            Title = "Entry Editor";
            SuspendLayout();
            Table = new TableLayout(2, 5);
            Table.Padding = new Padding(5);
            Table.Spacing = new Size(2, 2);

            Label space = new Label() { Text = " "};
            Label srcLabel = new Label() { Text = "Source: " };
            Label dstLabel = new Label() { Text = "Destination: " };
            Label commentLabel = new Label() { Text = "Comment: " };
            ConfirmButton = new Button() { Text = "OK" };
            CancelButton = new Button() { Text = "Cancel" };

            SourceInput = new TextBox();
            DestinationInput = new TextBox();
            CommentInput = new TextBox();

            Table.SuspendLayout();
            Table.Add(srcLabel, 0, 0);
            Table.Add(dstLabel, 0, 1);
            Table.Add(commentLabel, 0, 2);
            Table.Add(space, 0, 3);
            Table.Add(CancelButton, 0, 4);
            Table.Add(ConfirmButton, 1, 4);
            Table.Add(SourceInput, 1, 0);
            Table.Add(DestinationInput, 1, 1);
            Table.Add(CommentInput, 1, 2);
            Table.ResumeLayout();

            Content = Table;
            ResumeLayout();

            AbortButton = CancelButton;
            ConfirmButton.Click += OnConfirm;
            CancelButton.Click += OnAbort;
        }

        TableLayout Table;
        TextBox SourceInput;
        TextBox DestinationInput;
        TextBox CommentInput;
        Button ConfirmButton;
        Button CancelButton;
    }
}
