using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.ComponentModel;

namespace PoGo.NecroBot.CLI
{
    partial class Program
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
            this.PlayerInfoBox = new System.Windows.Forms.TextBox();
            this.DebugTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PlayerInfoBox
            // 
            this.PlayerInfoBox.Location = new System.Drawing.Point(0, 5);
            this.PlayerInfoBox.Name = "PlayerInfoBox";
            this.PlayerInfoBox.ReadOnly = true;
            this.PlayerInfoBox.Size = new System.Drawing.Size(590, 20);
            this.PlayerInfoBox.TabIndex = 1;
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.DebugTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DebugTextBox.Location = new System.Drawing.Point(0, 31);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(590, 219);
            this.DebugTextBox.TabIndex = 2;
            this.DebugTextBox.Text = "";
            // 
            // Program
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 262);
            this.Controls.Add(this.DebugTextBox);
            this.Controls.Add(this.PlayerInfoBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Program";
            this.Text = "Necrobot for Pokemon Go";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        public void AppendDebugMessage(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.DebugTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AppendDebugMessage);
                this.Invoke(d, new object[] { message, level, color });
            }
            else
            {

                switch (level)
                {
                    case LogLevel.Error:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Red;
                        break;
                    case LogLevel.Warning:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (ATTENTION) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Orange;
                        break;
                    case LogLevel.Info:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (INFO) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkCyan;
                        break;
                    case LogLevel.Pokestop:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (POKESTOP) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Cyan;
                        break;
                    case LogLevel.Farming:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (FARMING) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Magenta;
                        break;
                    case LogLevel.Recycling:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (RECYCLING) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkMagenta;
                        break;
                    case LogLevel.Caught:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (PKMN) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Green;
                        break;
                    case LogLevel.Transfer:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (TRANSFERED) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.DarkGreen;
                        break;
                    case LogLevel.Evolve:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (EVOLVED) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Yellow;
                        break;
                    case LogLevel.Berry:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (BERRY) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Orange;
                        break;
                    case LogLevel.Egg:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (EGG) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Orange;
                        break;
                    case LogLevel.Debug:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (DEBUG) {message}");
                        this.DebugTextBox.Find(message);
                        this.DebugTextBox.SelectionColor = System.Drawing.Color.Gray;
                        break;
                    default:
                        this.DebugTextBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                        this.DebugTextBox.Find(message);
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
                SetLighTextCallback d = new SetLighTextCallback(UpdatePlayerDetails);
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