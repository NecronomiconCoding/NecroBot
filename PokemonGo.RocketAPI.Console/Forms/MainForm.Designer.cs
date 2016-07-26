namespace PokemonGo.RocketAPI.Console.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xylosTabControl1 = new PokemonGo.RocketAPI.Console.XylosTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DeviceListView = new PokemonGo.RocketAPI.Console.UI.ListViewDB();
            this.Col_Device = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Model = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Connected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ImportLocBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.SaveLocBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.YCoordTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.XCoordTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.SaveLocationTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.SavedLocationsListView = new PokemonGo.RocketAPI.Console.UI.ListViewDB();
            this.COL_LOC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.COL_X_COORDS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.COL_Y_COORDS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.COL_BY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.CatchBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.CatchTextBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.DontCatchListBox = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.EvolveBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.EvolveTextBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.EvolveListBox = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.KeepBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.KeepTextBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.KeepListBox = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NotifErrorCheckBox = new PokemonGo.RocketAPI.Console.XylosCheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.NotifCatchCPHigherRBtn = new PokemonGo.RocketAPI.Console.XylosRadioButton();
            this.NotifCatchEverythingRBtn = new PokemonGo.RocketAPI.Console.XylosRadioButton();
            this.CPHigherTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.NotifTransferedCheckBox = new PokemonGo.RocketAPI.Console.XylosCheckBox();
            this.NotifPokeStopCheckBox = new PokemonGo.RocketAPI.Console.XylosCheckBox();
            this.NotifEvolvedCheckBox = new PokemonGo.RocketAPI.Console.XylosCheckBox();
            this.NotifRecycledCheckBox = new PokemonGo.RocketAPI.Console.XylosCheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AuthLoginBtn = new PokemonGo.RocketAPI.Console.XylosButton();
            this.AuthPasswordTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.AuthTypeComboBox = new PokemonGo.RocketAPI.Console.XylosCombobox();
            this.AuthUserNameTxtBox = new PokemonGo.RocketAPI.Console.XylosTextBox();
            this.xylosTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xylosTabControl1
            // 
            this.xylosTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.xylosTabControl1.Controls.Add(this.tabPage1);
            this.xylosTabControl1.Controls.Add(this.tabPage2);
            this.xylosTabControl1.Controls.Add(this.tabPage3);
            this.xylosTabControl1.Controls.Add(this.tabPage4);
            this.xylosTabControl1.Controls.Add(this.tabPage5);
            this.xylosTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xylosTabControl1.FirstHeaderBorder = false;
            this.xylosTabControl1.ItemSize = new System.Drawing.Size(40, 180);
            this.xylosTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xylosTabControl1.Multiline = true;
            this.xylosTabControl1.Name = "xylosTabControl1";
            this.xylosTabControl1.SelectedIndex = 0;
            this.xylosTabControl1.Size = new System.Drawing.Size(757, 433);
            this.xylosTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.xylosTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.DeviceListView);
            this.tabPage1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.tabPage1.Location = new System.Drawing.Point(184, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(569, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Devices";
            // 
            // DeviceListView
            // 
            this.DeviceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_Device,
            this.Col_Version,
            this.Col_Model,
            this.Col_IP,
            this.Col_Connected});
            this.DeviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceListView.Location = new System.Drawing.Point(3, 3);
            this.DeviceListView.Name = "DeviceListView";
            this.DeviceListView.Size = new System.Drawing.Size(563, 419);
            this.DeviceListView.TabIndex = 0;
            this.DeviceListView.UseCompatibleStateImageBehavior = false;
            this.DeviceListView.View = System.Windows.Forms.View.Details;
            // 
            // Col_Device
            // 
            this.Col_Device.Text = "Device";
            this.Col_Device.Width = 119;
            // 
            // Col_Version
            // 
            this.Col_Version.Text = "Version";
            this.Col_Version.Width = 84;
            // 
            // Col_Model
            // 
            this.Col_Model.Text = "Model";
            this.Col_Model.Width = 146;
            // 
            // Col_IP
            // 
            this.Col_IP.Text = "IP";
            this.Col_IP.Width = 108;
            // 
            // Col_Connected
            // 
            this.Col_Connected.Text = "Connected";
            this.Col_Connected.Width = 77;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.tabPage2.Location = new System.Drawing.Point(184, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(569, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Poke Map";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ImportLocBtn);
            this.groupBox4.Controls.Add(this.SaveLocBtn);
            this.groupBox4.Controls.Add(this.YCoordTxtBox);
            this.groupBox4.Controls.Add(this.XCoordTxtBox);
            this.groupBox4.Controls.Add(this.SaveLocationTxtBox);
            this.groupBox4.Controls.Add(this.SavedLocationsListView);
            this.groupBox4.Location = new System.Drawing.Point(6, 268);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(555, 149);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Saved Locations";
            // 
            // ImportLocBtn
            // 
            this.ImportLocBtn.EnabledCalc = true;
            this.ImportLocBtn.Location = new System.Drawing.Point(466, 15);
            this.ImportLocBtn.Name = "ImportLocBtn";
            this.ImportLocBtn.Size = new System.Drawing.Size(83, 29);
            this.ImportLocBtn.TabIndex = 5;
            this.ImportLocBtn.Text = "Import";
            // 
            // SaveLocBtn
            // 
            this.SaveLocBtn.EnabledCalc = true;
            this.SaveLocBtn.Location = new System.Drawing.Point(383, 15);
            this.SaveLocBtn.Name = "SaveLocBtn";
            this.SaveLocBtn.Size = new System.Drawing.Size(77, 29);
            this.SaveLocBtn.TabIndex = 4;
            this.SaveLocBtn.Text = "Save";
            // 
            // YCoordTxtBox
            // 
            this.YCoordTxtBox.EnabledCalc = true;
            this.YCoordTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.YCoordTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.YCoordTxtBox.Location = new System.Drawing.Point(288, 15);
            this.YCoordTxtBox.MaxLength = 32767;
            this.YCoordTxtBox.MultiLine = false;
            this.YCoordTxtBox.Name = "YCoordTxtBox";
            this.YCoordTxtBox.ReadOnly = false;
            this.YCoordTxtBox.Size = new System.Drawing.Size(89, 29);
            this.YCoordTxtBox.TabIndex = 3;
            this.YCoordTxtBox.Text = "Y Coords";
            this.YCoordTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.YCoordTxtBox.UseSystemPasswordChar = false;
            // 
            // XCoordTxtBox
            // 
            this.XCoordTxtBox.EnabledCalc = true;
            this.XCoordTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.XCoordTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.XCoordTxtBox.Location = new System.Drawing.Point(193, 15);
            this.XCoordTxtBox.MaxLength = 32767;
            this.XCoordTxtBox.MultiLine = false;
            this.XCoordTxtBox.Name = "XCoordTxtBox";
            this.XCoordTxtBox.ReadOnly = false;
            this.XCoordTxtBox.Size = new System.Drawing.Size(89, 29);
            this.XCoordTxtBox.TabIndex = 2;
            this.XCoordTxtBox.Text = "X Coords";
            this.XCoordTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.XCoordTxtBox.UseSystemPasswordChar = false;
            // 
            // SaveLocationTxtBox
            // 
            this.SaveLocationTxtBox.EnabledCalc = true;
            this.SaveLocationTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SaveLocationTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.SaveLocationTxtBox.Location = new System.Drawing.Point(6, 15);
            this.SaveLocationTxtBox.MaxLength = 32767;
            this.SaveLocationTxtBox.MultiLine = false;
            this.SaveLocationTxtBox.Name = "SaveLocationTxtBox";
            this.SaveLocationTxtBox.ReadOnly = false;
            this.SaveLocationTxtBox.Size = new System.Drawing.Size(181, 29);
            this.SaveLocationTxtBox.TabIndex = 1;
            this.SaveLocationTxtBox.Text = "Location";
            this.SaveLocationTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.SaveLocationTxtBox.UseSystemPasswordChar = false;
            // 
            // SavedLocationsListView
            // 
            this.SavedLocationsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.COL_LOC,
            this.COL_X_COORDS,
            this.COL_Y_COORDS,
            this.COL_BY});
            this.SavedLocationsListView.Location = new System.Drawing.Point(6, 50);
            this.SavedLocationsListView.Name = "SavedLocationsListView";
            this.SavedLocationsListView.Size = new System.Drawing.Size(543, 93);
            this.SavedLocationsListView.TabIndex = 0;
            this.SavedLocationsListView.UseCompatibleStateImageBehavior = false;
            this.SavedLocationsListView.View = System.Windows.Forms.View.Details;
            // 
            // COL_LOC
            // 
            this.COL_LOC.Text = "Location";
            this.COL_LOC.Width = 174;
            // 
            // COL_X_COORDS
            // 
            this.COL_X_COORDS.Text = "X Coords";
            this.COL_X_COORDS.Width = 109;
            // 
            // COL_Y_COORDS
            // 
            this.COL_Y_COORDS.Text = "Y Coords";
            this.COL_Y_COORDS.Width = 110;
            // 
            // COL_BY
            // 
            this.COL_BY.Text = "Author";
            this.COL_BY.Width = 145;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.tabPage3.Location = new System.Drawing.Point(184, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(569, 425);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Filters";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.CatchBtn);
            this.groupBox7.Controls.Add(this.CatchTextBox);
            this.groupBox7.Controls.Add(this.DontCatchListBox);
            this.groupBox7.Location = new System.Drawing.Point(6, 8);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(176, 411);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Not To Catch";
            // 
            // CatchBtn
            // 
            this.CatchBtn.EnabledCalc = true;
            this.CatchBtn.Location = new System.Drawing.Point(6, 57);
            this.CatchBtn.Name = "CatchBtn";
            this.CatchBtn.Size = new System.Drawing.Size(164, 28);
            this.CatchBtn.TabIndex = 2;
            this.CatchBtn.Text = "Submit";
            // 
            // CatchTextBox
            // 
            this.CatchTextBox.EnabledCalc = true;
            this.CatchTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CatchTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.CatchTextBox.Location = new System.Drawing.Point(6, 22);
            this.CatchTextBox.MaxLength = 32767;
            this.CatchTextBox.MultiLine = false;
            this.CatchTextBox.Name = "CatchTextBox";
            this.CatchTextBox.ReadOnly = false;
            this.CatchTextBox.Size = new System.Drawing.Size(164, 29);
            this.CatchTextBox.TabIndex = 1;
            this.CatchTextBox.Text = "Pkmn Name";
            this.CatchTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CatchTextBox.UseSystemPasswordChar = false;
            // 
            // DontCatchListBox
            // 
            this.DontCatchListBox.FormattingEnabled = true;
            this.DontCatchListBox.ItemHeight = 15;
            this.DontCatchListBox.Location = new System.Drawing.Point(0, 91);
            this.DontCatchListBox.Name = "DontCatchListBox";
            this.DontCatchListBox.Size = new System.Drawing.Size(176, 319);
            this.DontCatchListBox.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.EvolveBtn);
            this.groupBox6.Controls.Add(this.EvolveTextBox);
            this.groupBox6.Controls.Add(this.EvolveListBox);
            this.groupBox6.Location = new System.Drawing.Point(196, 8);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(176, 411);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "To Evolve";
            // 
            // EvolveBtn
            // 
            this.EvolveBtn.EnabledCalc = true;
            this.EvolveBtn.Location = new System.Drawing.Point(6, 57);
            this.EvolveBtn.Name = "EvolveBtn";
            this.EvolveBtn.Size = new System.Drawing.Size(164, 28);
            this.EvolveBtn.TabIndex = 2;
            this.EvolveBtn.Text = "Submit";
            // 
            // EvolveTextBox
            // 
            this.EvolveTextBox.EnabledCalc = true;
            this.EvolveTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.EvolveTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.EvolveTextBox.Location = new System.Drawing.Point(6, 22);
            this.EvolveTextBox.MaxLength = 32767;
            this.EvolveTextBox.MultiLine = false;
            this.EvolveTextBox.Name = "EvolveTextBox";
            this.EvolveTextBox.ReadOnly = false;
            this.EvolveTextBox.Size = new System.Drawing.Size(164, 29);
            this.EvolveTextBox.TabIndex = 1;
            this.EvolveTextBox.Text = "Pkmn Name";
            this.EvolveTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.EvolveTextBox.UseSystemPasswordChar = false;
            // 
            // EvolveListBox
            // 
            this.EvolveListBox.FormattingEnabled = true;
            this.EvolveListBox.ItemHeight = 15;
            this.EvolveListBox.Location = new System.Drawing.Point(0, 91);
            this.EvolveListBox.Name = "EvolveListBox";
            this.EvolveListBox.Size = new System.Drawing.Size(176, 319);
            this.EvolveListBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.KeepBtn);
            this.groupBox5.Controls.Add(this.KeepTextBox);
            this.groupBox5.Controls.Add(this.KeepListBox);
            this.groupBox5.Location = new System.Drawing.Point(385, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(176, 411);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "To Keep";
            // 
            // KeepBtn
            // 
            this.KeepBtn.EnabledCalc = true;
            this.KeepBtn.Location = new System.Drawing.Point(6, 57);
            this.KeepBtn.Name = "KeepBtn";
            this.KeepBtn.Size = new System.Drawing.Size(164, 28);
            this.KeepBtn.TabIndex = 2;
            this.KeepBtn.Text = "Submit";
            // 
            // KeepTextBox
            // 
            this.KeepTextBox.EnabledCalc = true;
            this.KeepTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeepTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.KeepTextBox.Location = new System.Drawing.Point(6, 22);
            this.KeepTextBox.MaxLength = 32767;
            this.KeepTextBox.MultiLine = false;
            this.KeepTextBox.Name = "KeepTextBox";
            this.KeepTextBox.ReadOnly = false;
            this.KeepTextBox.Size = new System.Drawing.Size(164, 29);
            this.KeepTextBox.TabIndex = 1;
            this.KeepTextBox.Text = "Pkmn Name";
            this.KeepTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.KeepTextBox.UseSystemPasswordChar = false;
            // 
            // KeepListBox
            // 
            this.KeepListBox.FormattingEnabled = true;
            this.KeepListBox.ItemHeight = 15;
            this.KeepListBox.Location = new System.Drawing.Point(0, 91);
            this.KeepListBox.Name = "KeepListBox";
            this.KeepListBox.Size = new System.Drawing.Size(176, 319);
            this.KeepListBox.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.tabPage4.Location = new System.Drawing.Point(184, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(569, 425);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Notifications";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.NotifErrorCheckBox);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.NotifTransferedCheckBox);
            this.groupBox2.Controls.Add(this.NotifPokeStopCheckBox);
            this.groupBox2.Controls.Add(this.NotifEvolvedCheckBox);
            this.groupBox2.Controls.Add(this.NotifRecycledCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(143, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(281, 170);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notifications When";
            // 
            // NotifErrorCheckBox
            // 
            this.NotifErrorCheckBox.Checked = false;
            this.NotifErrorCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifErrorCheckBox.EnabledCalc = true;
            this.NotifErrorCheckBox.Location = new System.Drawing.Point(6, 118);
            this.NotifErrorCheckBox.Name = "NotifErrorCheckBox";
            this.NotifErrorCheckBox.Size = new System.Drawing.Size(75, 18);
            this.NotifErrorCheckBox.TabIndex = 13;
            this.NotifErrorCheckBox.Text = "Error";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.NotifCatchCPHigherRBtn);
            this.groupBox3.Controls.Add(this.NotifCatchEverythingRBtn);
            this.groupBox3.Controls.Add(this.CPHigherTxtBox);
            this.groupBox3.Location = new System.Drawing.Point(132, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(143, 137);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Catch";
            // 
            // NotifCatchCPHigherRBtn
            // 
            this.NotifCatchCPHigherRBtn.Checked = false;
            this.NotifCatchCPHigherRBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifCatchCPHigherRBtn.EnabledCalc = true;
            this.NotifCatchCPHigherRBtn.Location = new System.Drawing.Point(6, 78);
            this.NotifCatchCPHigherRBtn.Name = "NotifCatchCPHigherRBtn";
            this.NotifCatchCPHigherRBtn.Size = new System.Drawing.Size(131, 18);
            this.NotifCatchCPHigherRBtn.TabIndex = 13;
            this.NotifCatchCPHigherRBtn.Text = "CP Higher Than";
            // 
            // NotifCatchEverythingRBtn
            // 
            this.NotifCatchEverythingRBtn.Checked = false;
            this.NotifCatchEverythingRBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifCatchEverythingRBtn.EnabledCalc = true;
            this.NotifCatchEverythingRBtn.Location = new System.Drawing.Point(6, 22);
            this.NotifCatchEverythingRBtn.Name = "NotifCatchEverythingRBtn";
            this.NotifCatchEverythingRBtn.Size = new System.Drawing.Size(95, 18);
            this.NotifCatchEverythingRBtn.TabIndex = 12;
            this.NotifCatchEverythingRBtn.Text = "Everything";
            // 
            // CPHigherTxtBox
            // 
            this.CPHigherTxtBox.EnabledCalc = true;
            this.CPHigherTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CPHigherTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.CPHigherTxtBox.Location = new System.Drawing.Point(6, 102);
            this.CPHigherTxtBox.MaxLength = 32767;
            this.CPHigherTxtBox.MultiLine = false;
            this.CPHigherTxtBox.Name = "CPHigherTxtBox";
            this.CPHigherTxtBox.ReadOnly = false;
            this.CPHigherTxtBox.Size = new System.Drawing.Size(131, 29);
            this.CPHigherTxtBox.TabIndex = 11;
            this.CPHigherTxtBox.Text = "300";
            this.CPHigherTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CPHigherTxtBox.UseSystemPasswordChar = false;
            // 
            // NotifTransferedCheckBox
            // 
            this.NotifTransferedCheckBox.Checked = false;
            this.NotifTransferedCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifTransferedCheckBox.EnabledCalc = true;
            this.NotifTransferedCheckBox.Location = new System.Drawing.Point(6, 94);
            this.NotifTransferedCheckBox.Name = "NotifTransferedCheckBox";
            this.NotifTransferedCheckBox.Size = new System.Drawing.Size(90, 18);
            this.NotifTransferedCheckBox.TabIndex = 9;
            this.NotifTransferedCheckBox.Text = "Transfered";
            // 
            // NotifPokeStopCheckBox
            // 
            this.NotifPokeStopCheckBox.Checked = false;
            this.NotifPokeStopCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifPokeStopCheckBox.EnabledCalc = true;
            this.NotifPokeStopCheckBox.Location = new System.Drawing.Point(6, 70);
            this.NotifPokeStopCheckBox.Name = "NotifPokeStopCheckBox";
            this.NotifPokeStopCheckBox.Size = new System.Drawing.Size(90, 18);
            this.NotifPokeStopCheckBox.TabIndex = 8;
            this.NotifPokeStopCheckBox.Text = "Pokestop";
            // 
            // NotifEvolvedCheckBox
            // 
            this.NotifEvolvedCheckBox.Checked = false;
            this.NotifEvolvedCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifEvolvedCheckBox.EnabledCalc = true;
            this.NotifEvolvedCheckBox.Location = new System.Drawing.Point(6, 46);
            this.NotifEvolvedCheckBox.Name = "NotifEvolvedCheckBox";
            this.NotifEvolvedCheckBox.Size = new System.Drawing.Size(75, 18);
            this.NotifEvolvedCheckBox.TabIndex = 7;
            this.NotifEvolvedCheckBox.Text = "Evolved";
            // 
            // NotifRecycledCheckBox
            // 
            this.NotifRecycledCheckBox.Checked = false;
            this.NotifRecycledCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifRecycledCheckBox.EnabledCalc = true;
            this.NotifRecycledCheckBox.Location = new System.Drawing.Point(6, 22);
            this.NotifRecycledCheckBox.Name = "NotifRecycledCheckBox";
            this.NotifRecycledCheckBox.Size = new System.Drawing.Size(75, 18);
            this.NotifRecycledCheckBox.TabIndex = 6;
            this.NotifRecycledCheckBox.Text = "Recycled";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPage5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.tabPage5.Location = new System.Drawing.Point(184, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(569, 425);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Credentials";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AuthLoginBtn);
            this.groupBox1.Controls.Add(this.AuthPasswordTxtBox);
            this.groupBox1.Controls.Add(this.AuthTypeComboBox);
            this.groupBox1.Controls.Add(this.AuthUserNameTxtBox);
            this.groupBox1.Location = new System.Drawing.Point(150, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 170);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credentials";
            // 
            // AuthLoginBtn
            // 
            this.AuthLoginBtn.EnabledCalc = true;
            this.AuthLoginBtn.Location = new System.Drawing.Point(8, 124);
            this.AuthLoginBtn.Name = "AuthLoginBtn";
            this.AuthLoginBtn.Size = new System.Drawing.Size(254, 35);
            this.AuthLoginBtn.TabIndex = 0;
            this.AuthLoginBtn.Text = "Login";
            this.AuthLoginBtn.Click += new PokemonGo.RocketAPI.Console.XylosButton.ClickEventHandler(this.AuthLoginBtn_Click);
            // 
            // AuthPasswordTxtBox
            // 
            this.AuthPasswordTxtBox.EnabledCalc = true;
            this.AuthPasswordTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AuthPasswordTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.AuthPasswordTxtBox.Location = new System.Drawing.Point(8, 89);
            this.AuthPasswordTxtBox.MaxLength = 32767;
            this.AuthPasswordTxtBox.MultiLine = false;
            this.AuthPasswordTxtBox.Name = "AuthPasswordTxtBox";
            this.AuthPasswordTxtBox.ReadOnly = false;
            this.AuthPasswordTxtBox.Size = new System.Drawing.Size(254, 29);
            this.AuthPasswordTxtBox.TabIndex = 3;
            this.AuthPasswordTxtBox.Text = "Password";
            this.AuthPasswordTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AuthPasswordTxtBox.UseSystemPasswordChar = false;
            // 
            // AuthTypeComboBox
            // 
            this.AuthTypeComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AuthTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.AuthTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthTypeComboBox.EnabledCalc = true;
            this.AuthTypeComboBox.FormattingEnabled = true;
            this.AuthTypeComboBox.ItemHeight = 20;
            this.AuthTypeComboBox.Items.AddRange(new object[] {
            "Google",
            "Ptc"});
            this.AuthTypeComboBox.Location = new System.Drawing.Point(8, 22);
            this.AuthTypeComboBox.Name = "AuthTypeComboBox";
            this.AuthTypeComboBox.Size = new System.Drawing.Size(254, 26);
            this.AuthTypeComboBox.TabIndex = 1;
            this.AuthTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.AuthTypeComboBox_SelectionChangeCommitted);
            // 
            // AuthUserNameTxtBox
            // 
            this.AuthUserNameTxtBox.EnabledCalc = true;
            this.AuthUserNameTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AuthUserNameTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.AuthUserNameTxtBox.Location = new System.Drawing.Point(8, 54);
            this.AuthUserNameTxtBox.MaxLength = 32767;
            this.AuthUserNameTxtBox.MultiLine = false;
            this.AuthUserNameTxtBox.Name = "AuthUserNameTxtBox";
            this.AuthUserNameTxtBox.ReadOnly = false;
            this.AuthUserNameTxtBox.Size = new System.Drawing.Size(254, 29);
            this.AuthUserNameTxtBox.TabIndex = 2;
            this.AuthUserNameTxtBox.Text = "Email/Username";
            this.AuthUserNameTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AuthUserNameTxtBox.UseSystemPasswordChar = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 433);
            this.Controls.Add(this.xylosTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "PokeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.xylosTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox7;
        private XylosButton CatchBtn;
        private XylosTextBox CatchTextBox;
        private System.Windows.Forms.ListBox DontCatchListBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private XylosButton EvolveBtn;
        private XylosTextBox EvolveTextBox;
        private System.Windows.Forms.ListBox EvolveListBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private XylosButton KeepBtn;
        private XylosTextBox KeepTextBox;
        private System.Windows.Forms.ListBox KeepListBox;
        private System.Windows.Forms.ColumnHeader Col_IP;
        private System.Windows.Forms.ColumnHeader Col_Model;
        private System.Windows.Forms.ColumnHeader Col_Version;
        private System.Windows.Forms.ColumnHeader Col_Device;
        private UI.ListViewDB DeviceListView;
        private System.Windows.Forms.ColumnHeader Col_Connected;
        private System.Windows.Forms.TabPage tabPage1;
        private XylosTabControl xylosTabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private XylosButton ImportLocBtn;
        private XylosButton SaveLocBtn;
        private XylosTextBox YCoordTxtBox;
        private XylosTextBox XCoordTxtBox;
        private XylosTextBox SaveLocationTxtBox;
        private UI.ListViewDB SavedLocationsListView;
        private System.Windows.Forms.ColumnHeader COL_LOC;
        private System.Windows.Forms.ColumnHeader COL_X_COORDS;
        private System.Windows.Forms.ColumnHeader COL_Y_COORDS;
        private System.Windows.Forms.ColumnHeader COL_BY;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox2;
        private XylosCheckBox NotifErrorCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private XylosRadioButton NotifCatchCPHigherRBtn;
        private XylosRadioButton NotifCatchEverythingRBtn;
        private XylosTextBox CPHigherTxtBox;
        private XylosCheckBox NotifTransferedCheckBox;
        private XylosCheckBox NotifPokeStopCheckBox;
        private XylosCheckBox NotifEvolvedCheckBox;
        private XylosCheckBox NotifRecycledCheckBox;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox1;
        private XylosButton AuthLoginBtn;
        private XylosTextBox AuthPasswordTxtBox;
        private XylosCombobox AuthTypeComboBox;
        private XylosTextBox AuthUserNameTxtBox;
    }
}