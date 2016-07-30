using PoGo.NecroBot.Logic.Logging;
using System;

namespace PoGo.NecroBot.GUI
{
    partial class NecroGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NecroGUI));
            this.PlayerInfoBox = new System.Windows.Forms.TextBox();
            this.DebugTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PlayerInfoBox
            // 
            this.PlayerInfoBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PlayerInfoBox.Location = new System.Drawing.Point(0, 0);
            this.PlayerInfoBox.Name = "PlayerInfoBox";
            this.PlayerInfoBox.ReadOnly = true;
            this.PlayerInfoBox.Size = new System.Drawing.Size(590, 20);
            this.PlayerInfoBox.TabIndex = 1;
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.DebugTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DebugTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DebugTextBox.Location = new System.Drawing.Point(0, 20);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(590, 242);
            this.DebugTextBox.TabIndex = 2;
            this.DebugTextBox.Text = "";
            // 
            // NecroGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 262);
            this.Controls.Add(this.DebugTextBox);
            this.Controls.Add(this.PlayerInfoBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NecroGUI";
            this.Text = "Necrobot for Pokemon Go";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        public void AppendDebugMessage(string message, ConsoleColor color = ConsoleColor.Black)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (!message.EndsWith("\n"))
            {
                message = message + "\n";
            }
            if (this.DebugTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AppendDebugMessage);
                this.Invoke(d, new object[] { message, color });
            }
            else
            {
                this.DebugTextBox.AppendText(message);
                this.DebugTextBox.Find(message);

                switch (color)
                {
                    case ConsoleColor.Red:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Red;
                        break;
                    case ConsoleColor.DarkYellow:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Orange;
                        break;
                    case ConsoleColor.DarkCyan:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkCyan;
                        break;
                    case ConsoleColor.Cyan:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Cyan;
                        break;
                    case ConsoleColor.Magenta:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Magenta;
                        break;
                    case ConsoleColor.DarkMagenta:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkMagenta;
                        break;
                    case ConsoleColor.Green:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Green;
                        break;
                    case ConsoleColor.DarkGreen:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkGreen;
                        break;
                    case ConsoleColor.Yellow:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Yellow;
                        break;
                    case ConsoleColor.Gray:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Gray;
                        break;
                    default:
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.White;
                        break;
                }
                this.DebugTextBox.Select(this.DebugTextBox.Text.Length, 0);
            }
        }

        public void UpdatePlayerDetails(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.PlayerInfoBox.InvokeRequired)
            {
                SetLightTextCallback d = new SetLightTextCallback(UpdatePlayerDetails);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.PlayerInfoBox.Text = text;
            }
        }
        private System.Windows.Forms.TextBox PlayerInfoBox;
        private System.Windows.Forms.RichTextBox DebugTextBox;
    }
}