namespace PokemonGo.RocketAPI.Logic
{
    partial class liveView
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
            this.grpMyItems = new System.Windows.Forms.GroupBox();
            this.labelBagSpace = new System.Windows.Forms.Label();
            this.dataMyItems = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelPokemonSpace = new System.Windows.Forms.Label();
            this.dataMyPokemons = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpStats = new System.Windows.Forms.GroupBox();
            this.textTotalPokemons = new System.Windows.Forms.TextBox();
            this.textTotalPokestop = new System.Windows.Forms.TextBox();
            this.textPokecoins = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textStardust = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelExpHr = new System.Windows.Forms.Label();
            this.labelExp = new System.Windows.Forms.Label();
            this.textLevel = new System.Windows.Forms.TextBox();
            this.progressLevel = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.grpMyPokemons = new System.Windows.Forms.GroupBox();
            this.grpMap = new System.Windows.Forms.GroupBox();
            this.textCurrentLatLng = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMyItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyPokemons)).BeginInit();
            this.grpStats.SuspendLayout();
            this.grpMyPokemons.SuspendLayout();
            this.grpMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMyItems
            // 
            this.grpMyItems.Controls.Add(this.labelBagSpace);
            this.grpMyItems.Controls.Add(this.dataMyItems);
            this.grpMyItems.Location = new System.Drawing.Point(12, 178);
            this.grpMyItems.Name = "grpMyItems";
            this.grpMyItems.Size = new System.Drawing.Size(305, 255);
            this.grpMyItems.TabIndex = 8;
            this.grpMyItems.TabStop = false;
            this.grpMyItems.Text = "Inventory";
            // 
            // labelBagSpace
            // 
            this.labelBagSpace.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelBagSpace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBagSpace.Location = new System.Drawing.Point(9, 229);
            this.labelBagSpace.Name = "labelBagSpace";
            this.labelBagSpace.Size = new System.Drawing.Size(285, 23);
            this.labelBagSpace.TabIndex = 10;
            this.labelBagSpace.Text = "label2";
            this.labelBagSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataMyItems
            // 
            this.dataMyItems.AllowUserToAddRows = false;
            this.dataMyItems.AllowUserToDeleteRows = false;
            this.dataMyItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dataMyItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMyItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column8});
            this.dataMyItems.Location = new System.Drawing.Point(6, 19);
            this.dataMyItems.Name = "dataMyItems";
            this.dataMyItems.ReadOnly = true;
            this.dataMyItems.RowHeadersWidth = 10;
            this.dataMyItems.RowTemplate.Height = 40;
            this.dataMyItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataMyItems.Size = new System.Drawing.Size(288, 206);
            this.dataMyItems.TabIndex = 9;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 30;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Item";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Count";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 75;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "ID";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Visible = false;
            // 
            // labelPokemonSpace
            // 
            this.labelPokemonSpace.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelPokemonSpace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPokemonSpace.Location = new System.Drawing.Point(12, 237);
            this.labelPokemonSpace.Name = "labelPokemonSpace";
            this.labelPokemonSpace.Size = new System.Drawing.Size(285, 19);
            this.labelPokemonSpace.TabIndex = 11;
            this.labelPokemonSpace.Text = "label2";
            this.labelPokemonSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataMyPokemons
            // 
            this.dataMyPokemons.AllowUserToAddRows = false;
            this.dataMyPokemons.AllowUserToDeleteRows = false;
            this.dataMyPokemons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dataMyPokemons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMyPokemons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column7});
            this.dataMyPokemons.Location = new System.Drawing.Point(6, 19);
            this.dataMyPokemons.Name = "dataMyPokemons";
            this.dataMyPokemons.ReadOnly = true;
            this.dataMyPokemons.RowHeadersWidth = 10;
            this.dataMyPokemons.RowTemplate.Height = 40;
            this.dataMyPokemons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataMyPokemons.Size = new System.Drawing.Size(291, 210);
            this.dataMyPokemons.TabIndex = 8;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 30;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Pokemon";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "CP";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 75;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "ID";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Visible = false;
            // 
            // grpStats
            // 
            this.grpStats.Controls.Add(this.textTotalPokemons);
            this.grpStats.Controls.Add(this.textTotalPokestop);
            this.grpStats.Controls.Add(this.textPokecoins);
            this.grpStats.Controls.Add(this.label4);
            this.grpStats.Controls.Add(this.textStardust);
            this.grpStats.Controls.Add(this.label2);
            this.grpStats.Controls.Add(this.labelExpHr);
            this.grpStats.Controls.Add(this.labelExp);
            this.grpStats.Controls.Add(this.textLevel);
            this.grpStats.Controls.Add(this.progressLevel);
            this.grpStats.Controls.Add(this.label3);
            this.grpStats.Location = new System.Drawing.Point(12, 12);
            this.grpStats.Name = "grpStats";
            this.grpStats.Size = new System.Drawing.Size(305, 160);
            this.grpStats.TabIndex = 9;
            this.grpStats.TabStop = false;
            this.grpStats.Text = "Player stats";
            // 
            // textTotalPokemons
            // 
            this.textTotalPokemons.Location = new System.Drawing.Point(232, 38);
            this.textTotalPokemons.Name = "textTotalPokemons";
            this.textTotalPokemons.Size = new System.Drawing.Size(65, 20);
            this.textTotalPokemons.TabIndex = 18;
            // 
            // textTotalPokestop
            // 
            this.textTotalPokestop.Location = new System.Drawing.Point(232, 12);
            this.textTotalPokestop.Name = "textTotalPokestop";
            this.textTotalPokestop.Size = new System.Drawing.Size(65, 20);
            this.textTotalPokestop.TabIndex = 17;
            // 
            // textPokecoins
            // 
            this.textPokecoins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textPokecoins.Location = new System.Drawing.Point(69, 60);
            this.textPokecoins.Name = "textPokecoins";
            this.textPokecoins.ReadOnly = true;
            this.textPokecoins.Size = new System.Drawing.Size(97, 20);
            this.textPokecoins.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Pokecoins";
            // 
            // textStardust
            // 
            this.textStardust.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textStardust.Location = new System.Drawing.Point(69, 38);
            this.textStardust.Name = "textStardust";
            this.textStardust.ReadOnly = true;
            this.textStardust.Size = new System.Drawing.Size(97, 20);
            this.textStardust.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Stardust";
            // 
            // labelExpHr
            // 
            this.labelExpHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExpHr.Location = new System.Drawing.Point(12, 131);
            this.labelExpHr.Name = "labelExpHr";
            this.labelExpHr.Size = new System.Drawing.Size(282, 23);
            this.labelExpHr.TabIndex = 12;
            this.labelExpHr.Text = "XP/HR";
            this.labelExpHr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelExp
            // 
            this.labelExp.Location = new System.Drawing.Point(12, 111);
            this.labelExp.Name = "labelExp";
            this.labelExp.Size = new System.Drawing.Size(285, 19);
            this.labelExp.TabIndex = 11;
            this.labelExp.Text = "EXP/LEVELEXP";
            this.labelExp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textLevel
            // 
            this.textLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLevel.Location = new System.Drawing.Point(69, 16);
            this.textLevel.Name = "textLevel";
            this.textLevel.ReadOnly = true;
            this.textLevel.Size = new System.Drawing.Size(97, 20);
            this.textLevel.TabIndex = 10;
            // 
            // progressLevel
            // 
            this.progressLevel.Location = new System.Drawing.Point(12, 85);
            this.progressLevel.Name = "progressLevel";
            this.progressLevel.Size = new System.Drawing.Size(285, 23);
            this.progressLevel.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressLevel.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Level";
            // 
            // gMap
            // 
            this.gMap.Bearing = 0F;
            this.gMap.CanDragMap = true;
            this.gMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(6, 38);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 18;
            this.gMap.MinZoom = 2;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(526, 647);
            this.gMap.TabIndex = 10;
            this.gMap.Zoom = 0D;
            // 
            // grpMyPokemons
            // 
            this.grpMyPokemons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grpMyPokemons.Controls.Add(this.labelPokemonSpace);
            this.grpMyPokemons.Controls.Add(this.dataMyPokemons);
            this.grpMyPokemons.Location = new System.Drawing.Point(12, 439);
            this.grpMyPokemons.Name = "grpMyPokemons";
            this.grpMyPokemons.Size = new System.Drawing.Size(305, 264);
            this.grpMyPokemons.TabIndex = 11;
            this.grpMyPokemons.TabStop = false;
            this.grpMyPokemons.Text = "Pokémons";
            // 
            // grpMap
            // 
            this.grpMap.Controls.Add(this.textCurrentLatLng);
            this.grpMap.Controls.Add(this.label1);
            this.grpMap.Controls.Add(this.gMap);
            this.grpMap.Location = new System.Drawing.Point(323, 12);
            this.grpMap.Name = "grpMap";
            this.grpMap.Size = new System.Drawing.Size(538, 691);
            this.grpMap.TabIndex = 12;
            this.grpMap.TabStop = false;
            this.grpMap.Text = "Live Map";
            // 
            // textCurrentLatLng
            // 
            this.textCurrentLatLng.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCurrentLatLng.Location = new System.Drawing.Point(86, 12);
            this.textCurrentLatLng.Name = "textCurrentLatLng";
            this.textCurrentLatLng.ReadOnly = true;
            this.textCurrentLatLng.Size = new System.Drawing.Size(446, 20);
            this.textCurrentLatLng.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Current lat/lng";
            // 
            // liveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 729);
            this.Controls.Add(this.grpMap);
            this.Controls.Add(this.grpMyPokemons);
            this.Controls.Add(this.grpStats);
            this.Controls.Add(this.grpMyItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "liveView";
            this.Text = "liveView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.liveView_FormClosing);
            this.grpMyItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataMyItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyPokemons)).EndInit();
            this.grpStats.ResumeLayout(false);
            this.grpStats.PerformLayout();
            this.grpMyPokemons.ResumeLayout(false);
            this.grpMap.ResumeLayout(false);
            this.grpMap.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpMyItems;
        private System.Windows.Forms.DataGridView dataMyPokemons;
        private System.Windows.Forms.DataGridView dataMyItems;
        private System.Windows.Forms.GroupBox grpStats;
        private System.Windows.Forms.ProgressBar progressLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textLevel;
        private System.Windows.Forms.Label labelExp;
        private System.Windows.Forms.Label labelPokemonSpace;
        private System.Windows.Forms.Label labelBagSpace;
        private System.Windows.Forms.Label labelExpHr;
        private System.Windows.Forms.TextBox textPokecoins;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textStardust;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.GroupBox grpMyPokemons;
        private System.Windows.Forms.GroupBox grpMap;
        private System.Windows.Forms.TextBox textCurrentLatLng;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textTotalPokestop;
        private System.Windows.Forms.TextBox textTotalPokemons;
    }
}