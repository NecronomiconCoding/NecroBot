namespace PoGo.NecroBot.CLI.Forms
{
    partial class FormNecroMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNecroMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consoleView = new System.Windows.Forms.ListView();
            this.footerStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelBotInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.footerStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.consoleView);
            this.groupBox1.Location = new System.Drawing.Point(12, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(876, 425);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console";
            // 
            // consoleView
            // 
            this.consoleView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.consoleView.BackColor = System.Drawing.Color.Black;
            this.consoleView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.consoleView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleView.LabelWrap = false;
            this.consoleView.Location = new System.Drawing.Point(7, 14);
            this.consoleView.Name = "consoleView";
            this.consoleView.Size = new System.Drawing.Size(863, 405);
            this.consoleView.TabIndex = 0;
            this.consoleView.UseCompatibleStateImageBehavior = false;
            this.consoleView.View = System.Windows.Forms.View.Details;
            // 
            // footerStrip
            // 
            this.footerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelBotInfo});
            this.footerStrip.Location = new System.Drawing.Point(0, 470);
            this.footerStrip.Name = "footerStrip";
            this.footerStrip.Size = new System.Drawing.Size(900, 22);
            this.footerStrip.TabIndex = 1;
            this.footerStrip.Text = "footerStrip";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHeader});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(900, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuHeader
            // 
            this.menuHeader.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsMenuItem,
            this.startMenuItem,
            this.exitToolStripMenuItem});
            this.menuHeader.Name = "menuHeader";
            this.menuHeader.Size = new System.Drawing.Size(50, 20);
            this.menuHeader.Text = "Menu";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // startMenuItem
            // 
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startMenuItem.Text = "Start NecroBot";
            this.startMenuItem.Click += new System.EventHandler(this.startMenuItem_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Log";
            this.columnHeader1.Width = 954;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // labelBotInfo
            // 
            this.labelBotInfo.ForeColor = System.Drawing.Color.DarkRed;
            this.labelBotInfo.Name = "labelBotInfo";
            this.labelBotInfo.Size = new System.Drawing.Size(12, 17);
            this.labelBotInfo.Text = "-";
            // 
            // FormNecroMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 492);
            this.Controls.Add(this.footerStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormNecroMain";
            this.Text = "NecroBot";
            this.Load += new System.EventHandler(this.FormNecroMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.footerStrip.ResumeLayout(false);
            this.footerStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView consoleView;
        private System.Windows.Forms.StatusStrip footerStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuHeader;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel labelBotInfo;
    }
}