namespace PoGo.NecroBot.UI
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
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label15;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.walkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bicycleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecycled = new System.Windows.Forms.Label();
            this.lblTransferred = new System.Windows.Forms.Label();
            this.lblStardust = new System.Windows.Forms.Label();
            this.lblPph = new System.Windows.Forms.Label();
            this.lblXph = new System.Windows.Forms.Label();
            this.lblRuntime = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblEta = new System.Windows.Forms.Label();
            this.lblXp = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lblLevel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtfLog = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            label11 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = System.Windows.Forms.DockStyle.Fill;
            label11.Location = new System.Drawing.Point(3, 131);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(119, 20);
            label11.TabIndex = 20;
            label11.Text = "Stardust:";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = System.Windows.Forms.DockStyle.Fill;
            label9.Location = new System.Drawing.Point(3, 111);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(119, 20);
            label9.TabIndex = 18;
            label9.Text = "Pokémon/hour:";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = System.Windows.Forms.DockStyle.Fill;
            label7.Location = new System.Drawing.Point(3, 91);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(119, 20);
            label7.TabIndex = 16;
            label7.Text = "XP/hour:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.Location = new System.Drawing.Point(3, 71);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(119, 20);
            label5.TabIndex = 14;
            label5.Text = "Run time:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = System.Windows.Forms.DockStyle.Fill;
            label13.Location = new System.Drawing.Point(3, 151);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(119, 20);
            label13.TabIndex = 23;
            label13.Text = "Transferred:";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Dock = System.Windows.Forms.DockStyle.Fill;
            label15.Location = new System.Drawing.Point(3, 171);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(119, 20);
            label15.TabIndex = 25;
            label15.Text = "Recycled:";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(944, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(944, 419);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.webBrowser);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.splitContainer2.Size = new System.Drawing.Size(250, 419);
            this.splitContainer2.SplitterDistance = 199;
            this.splitContainer2.TabIndex = 0;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(0, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(250, 174);
            this.webBrowser.TabIndex = 2;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.toolStripButton2,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(250, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Checked = true;
            this.toolStripButton1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(125, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.walkToolStripMenuItem,
            this.bicycleToolStripMenuItem,
            this.driveToolStripMenuItem,
            this.flyToolStripMenuItem});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripDropDownButton1.Text = "Go";
            // 
            // walkToolStripMenuItem
            // 
            this.walkToolStripMenuItem.Checked = true;
            this.walkToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.walkToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("walkToolStripMenuItem.Image")));
            this.walkToolStripMenuItem.Name = "walkToolStripMenuItem";
            this.walkToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.walkToolStripMenuItem.Text = "Run";
            // 
            // bicycleToolStripMenuItem
            // 
            this.bicycleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("bicycleToolStripMenuItem.Image")));
            this.bicycleToolStripMenuItem.Name = "bicycleToolStripMenuItem";
            this.bicycleToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.bicycleToolStripMenuItem.Text = "Cycle";
            // 
            // driveToolStripMenuItem
            // 
            this.driveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("driveToolStripMenuItem.Image")));
            this.driveToolStripMenuItem.Name = "driveToolStripMenuItem";
            this.driveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.driveToolStripMenuItem.Text = "Drive";
            // 
            // flyToolStripMenuItem
            // 
            this.flyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("flyToolStripMenuItem.Image")));
            this.flyToolStripMenuItem.Name = "flyToolStripMenuItem";
            this.flyToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.flyToolStripMenuItem.Text = "Fly";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblRecycled, 1, 7);
            this.tableLayoutPanel1.Controls.Add(label15, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblTransferred, 1, 6);
            this.tableLayoutPanel1.Controls.Add(label13, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblStardust, 1, 5);
            this.tableLayoutPanel1.Controls.Add(label11, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblPph, 1, 4);
            this.tableLayoutPanel1.Controls.Add(label9, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblXph, 1, 3);
            this.tableLayoutPanel1.Controls.Add(label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblRuntime, 1, 2);
            this.tableLayoutPanel1.Controls.Add(label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblUser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 204);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lblRecycled
            // 
            this.lblRecycled.AutoSize = true;
            this.lblRecycled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecycled.Location = new System.Drawing.Point(128, 171);
            this.lblRecycled.Name = "lblRecycled";
            this.lblRecycled.Size = new System.Drawing.Size(119, 20);
            this.lblRecycled.TabIndex = 26;
            this.lblRecycled.Text = "0";
            this.lblRecycled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTransferred
            // 
            this.lblTransferred.AutoSize = true;
            this.lblTransferred.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransferred.Location = new System.Drawing.Point(128, 151);
            this.lblTransferred.Name = "lblTransferred";
            this.lblTransferred.Size = new System.Drawing.Size(119, 20);
            this.lblTransferred.TabIndex = 24;
            this.lblTransferred.Text = "0";
            this.lblTransferred.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStardust
            // 
            this.lblStardust.AutoSize = true;
            this.lblStardust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStardust.Location = new System.Drawing.Point(128, 131);
            this.lblStardust.Name = "lblStardust";
            this.lblStardust.Size = new System.Drawing.Size(119, 20);
            this.lblStardust.TabIndex = 21;
            this.lblStardust.Text = "0";
            this.lblStardust.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPph
            // 
            this.lblPph.AutoSize = true;
            this.lblPph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPph.Location = new System.Drawing.Point(128, 111);
            this.lblPph.Name = "lblPph";
            this.lblPph.Size = new System.Drawing.Size(119, 20);
            this.lblPph.TabIndex = 19;
            this.lblPph.Text = "Calculating...";
            this.lblPph.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblXph
            // 
            this.lblXph.AutoSize = true;
            this.lblXph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblXph.Location = new System.Drawing.Point(128, 91);
            this.lblXph.Name = "lblXph";
            this.lblXph.Size = new System.Drawing.Size(119, 20);
            this.lblXph.TabIndex = 17;
            this.lblXph.Text = "Calculating...";
            this.lblXph.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRuntime
            // 
            this.lblRuntime.AutoSize = true;
            this.lblRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRuntime.Location = new System.Drawing.Point(128, 71);
            this.lblRuntime.Name = "lblRuntime";
            this.lblRuntime.Size = new System.Drawing.Size(119, 20);
            this.lblRuntime.TabIndex = 15;
            this.lblRuntime.Text = "00.00:00:00";
            this.lblRuntime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblUser, 2);
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(3, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(244, 21);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "Loading...";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.lblEta, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblXp, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.progress, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblLevel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 21);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(250, 50);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // lblEta
            // 
            this.lblEta.AutoSize = true;
            this.lblEta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEta.Location = new System.Drawing.Point(190, 29);
            this.lblEta.Name = "lblEta";
            this.lblEta.Size = new System.Drawing.Size(57, 21);
            this.lblEta.TabIndex = 15;
            this.lblEta.Text = "ETA";
            this.lblEta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblXp
            // 
            this.lblXp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblXp.Location = new System.Drawing.Point(65, 29);
            this.lblXp.Name = "lblXp";
            this.lblXp.Size = new System.Drawing.Size(119, 21);
            this.lblXp.TabIndex = 14;
            this.lblXp.Text = "XP";
            this.lblXp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progress
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.progress, 3);
            this.progress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progress.Location = new System.Drawing.Point(3, 3);
            this.progress.Maximum = 1000;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(244, 23);
            this.progress.TabIndex = 12;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLevel.Location = new System.Drawing.Point(3, 29);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(56, 21);
            this.lblLevel.TabIndex = 13;
            this.lblLevel.Text = "Level";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(690, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtfLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(682, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Activity";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtfLog
            // 
            this.rtfLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfLog.Location = new System.Drawing.Point(3, 3);
            this.rtfLog.Margin = new System.Windows.Forms.Padding(0);
            this.rtfLog.Name = "rtfLog";
            this.rtfLog.Size = new System.Drawing.Size(676, 387);
            this.rtfLog.TabIndex = 4;
            this.rtfLog.Text = "";
            this.rtfLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtfLog_LinkClicked);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(511, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Inventory";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(511, 393);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Pokemon";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 441);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "NecroBot - Pokémon GO Bot based on Ferox\'s RocketAPI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox rtfLog;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem walkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bicycleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem driveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flyToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblEta;
        private System.Windows.Forms.Label lblXp;
        private System.Windows.Forms.Label lblRuntime;
        private System.Windows.Forms.Label lblXph;
        private System.Windows.Forms.Label lblPph;
        private System.Windows.Forms.Label lblStardust;
        private System.Windows.Forms.Label lblTransferred;
        private System.Windows.Forms.Label lblRecycled;
    }
}

