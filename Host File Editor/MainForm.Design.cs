using System;
using System.Collections.Generic;
using System.Text;
using Eto.Forms;
using Eto.Drawing;

namespace Host_File_Editor
{
    internal partial class MainForm
    {
        private Size MinButtonSize = new Size(100, 30);
        private Size RoutingToggleSize = new Size(30, 30);
        private void BuildUI() {
            SuspendLayout();
            Table = new TableLayout(1, 3);
            Table.Size = new Size(100, 300);
            Table.Padding = new Padding(3);
            Table.Spacing = new Size(2, 2);

            Label space = new Label();
            space.Text = new string(' ', 25);
            Label space2 = new Label();
            space2.Text = new string(' ', 25);

            SaveButton = new Button((sender, args) => SaveFile());
            SaveButton.Text = "Save";
            SaveButton.MinimumSize = MinButtonSize;

            PresetButton = new Button((sender, args) => PickPreset());
            PresetButton.Text = "Presets";
            PresetButton.MinimumSize = MinButtonSize;


            AddEntryButton = new Button((sender, args) => AddEntry());
            AddEntryButton.Text = "Add";
            AddEntryButton.MinimumSize = MinButtonSize;

            DeleteEntryButton = new Button((sender, args) => DeleteEntry());
            DeleteEntryButton.Text = "Delete";
            DeleteEntryButton.MinimumSize = MinButtonSize;

            RoutingTableView = new GridView();

            var fileButtonContainer = new TableLayout(3, 1);
            fileButtonContainer.Padding = new Padding(3);
            fileButtonContainer.Spacing = new Size(2, 2);

            var entryButtonContainer = new TableLayout(3, 1);
            entryButtonContainer.Padding = new Padding(3);
            entryButtonContainer.Spacing = new Size(2, 2);

            fileButtonContainer.SuspendLayout();
            fileButtonContainer.Add(SaveButton, 0, 0, false, false);
            fileButtonContainer.Add(space, 1, 0, true, false);
            fileButtonContainer.Add(PresetButton, 2, 0, false, false);
            fileButtonContainer.ResumeLayout();

            entryButtonContainer.SuspendLayout();
            entryButtonContainer.Add(AddEntryButton, 0, 0, false, false);
            entryButtonContainer.Add(space2, 1, 0, true, false);
            entryButtonContainer.Add(DeleteEntryButton, 2, 0, false, false);
            entryButtonContainer.ResumeLayout();

            Table.SuspendLayout();
            Table.Add(fileButtonContainer, 0, 0);
            Table.Add(entryButtonContainer, 0, 1);
            Table.Add(RoutingTableView, 0, 2, true, true);

            Content = Table;

            Table.ResumeLayout();
            ResumeLayout();
            MinimumSize = new Size(3*2 + 2*2 + 110 + MinButtonSize.Width * 2, 500);
            SetupRoutingTableView();
        }

        void SetupRoutingTableView() {
            RoutingTableView.AllowColumnReordering = false;
            RoutingTableView.AllowEmptySelection = true;
            RoutingTableView.AllowMultipleSelection = true;
            RoutingTableView.ShowHeader = true;
            RoutingTableView.Columns.Add(new GridColumn() {
                HeaderText = "",
                Expand = false,
                Resizable = false,
                Sortable = false,
                Width = RoutingToggleSize.Width,
            });
            RoutingTableView.Columns.Add(new GridColumn() {
                HeaderText = "Source",
                Expand = false,
                Resizable = true,
                Sortable = false,
                AutoSize = true
            });
            RoutingTableView.Columns.Add(new GridColumn() {
                HeaderText = "Destination",
                Expand = false,
                Resizable = true,
                Sortable = false,
                AutoSize = true
            });
            RoutingTableView.Columns.Add(new GridColumn() {
                HeaderText = "Description",
                Expand = true,
                Resizable = true,
                Sortable = false,
                AutoSize = true
            });
        }

        TableLayout Table;

        Button SaveButton;
        Button PresetButton;
        Button AddEntryButton;
        Button DeleteEntryButton;

        GridView RoutingTableView;
    }
}
