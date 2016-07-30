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
            this.labelBotStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.groupBox1.Size = new System.Drawing.Size(776, 398);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
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
            this.consoleView.Size = new System.Drawing.Size(763, 378);
            this.consoleView.TabIndex = 0;
            this.consoleView.UseCompatibleStateImageBehavior = false;
            this.consoleView.View = System.Windows.Forms.View.Details;
            // 
            // footerStrip
            // 
            this.footerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelBotStatus});
            this.footerStrip.Location = new System.Drawing.Point(0, 443);
            this.footerStrip.Name = "footerStrip";
            this.footerStrip.Size = new System.Drawing.Size(800, 22);
            this.footerStrip.TabIndex = 1;
            this.footerStrip.Text = "footerStrip";
            // 
            // labelBotStatus
            // 
            this.labelBotStatus.Name = "labelBotStatus";
            this.labelBotStatus.Size = new System.Drawing.Size(146, 17);
            this.labelBotStatus.Text = "Bot Status: NOT RUNNING";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHeader});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuHeader
            // 
            this.menuHeader.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsMenuItem,
            this.startMenuItem,
            this.stopMenuItem});
            this.menuHeader.Name = "menuHeader";
            this.menuHeader.Size = new System.Drawing.Size(50, 20);
            this.menuHeader.Text = "Menu";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsMenuItem.Text = "Settings";
            // 
            // startMenuItem
            // 
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startMenuItem.Text = "Start NecroBot";
            this.startMenuItem.Click += new System.EventHandler(this.startMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stopMenuItem.Text = "Stop NecroBot";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Log";
            this.columnHeader1.Width = 954;
            // 
            // FormNecroMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 465);
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
        private System.Windows.Forms.ToolStripStatusLabel labelBotStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuHeader;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}