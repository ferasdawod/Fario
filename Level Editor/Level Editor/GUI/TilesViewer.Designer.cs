namespace Level_Editor
{
    partial class TilesViewer
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
            this.components = new System.ComponentModel.Container();
            this.imgListTiles = new System.Windows.Forms.ImageList(this.components);
            this.listTiles = new System.Windows.Forms.ListView();
            this.listTemp = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listCoins = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listTraps = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listEnemies = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgListTiles
            // 
            this.imgListTiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListTiles.ImageSize = new System.Drawing.Size(48, 48);
            this.imgListTiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listTiles
            // 
            this.listTiles.BackColor = System.Drawing.SystemColors.Control;
            this.listTiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTiles.HideSelection = false;
            this.listTiles.LargeImageList = this.imgListTiles;
            this.listTiles.Location = new System.Drawing.Point(3, 16);
            this.listTiles.MultiSelect = false;
            this.listTiles.Name = "listTiles";
            this.listTiles.Size = new System.Drawing.Size(460, 276);
            this.listTiles.TabIndex = 3;
            this.listTiles.TileSize = new System.Drawing.Size(10, 10);
            this.listTiles.UseCompatibleStateImageBehavior = false;
            this.listTiles.SelectedIndexChanged += new System.EventHandler(this.listTiles_SelectedIndexChanged);
            // 
            // listTemp
            // 
            this.listTemp.BackColor = System.Drawing.SystemColors.Control;
            this.listTemp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTemp.HideSelection = false;
            this.listTemp.LargeImageList = this.imgListTiles;
            this.listTemp.Location = new System.Drawing.Point(3, 16);
            this.listTemp.MultiSelect = false;
            this.listTemp.Name = "listTemp";
            this.listTemp.Size = new System.Drawing.Size(686, 85);
            this.listTemp.TabIndex = 5;
            this.listTemp.TileSize = new System.Drawing.Size(10, 10);
            this.listTemp.UseCompatibleStateImageBehavior = false;
            this.listTemp.SelectedIndexChanged += new System.EventHandler(this.listTemp_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listTemp);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(692, 104);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recent Tiles";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listTiles);
            this.groupBox2.Location = new System.Drawing.Point(4, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(466, 295);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "All Tiles";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(4, 413);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Top Most";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(618, 410);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 20);
            this.button1.TabIndex = 9;
            this.button1.Text = "Clear Recent";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listCoins
            // 
            this.listCoins.BackColor = System.Drawing.SystemColors.Control;
            this.listCoins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listCoins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listCoins.HideSelection = false;
            this.listCoins.LargeImageList = this.imgListTiles;
            this.listCoins.Location = new System.Drawing.Point(3, 3);
            this.listCoins.Margin = new System.Windows.Forms.Padding(0);
            this.listCoins.MultiSelect = false;
            this.listCoins.Name = "listCoins";
            this.listCoins.Size = new System.Drawing.Size(206, 257);
            this.listCoins.TabIndex = 6;
            this.listCoins.TileSize = new System.Drawing.Size(10, 10);
            this.listCoins.UseCompatibleStateImageBehavior = false;
            this.listCoins.SelectedIndexChanged += new System.EventHandler(this.listCoins_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(476, 118);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(220, 289);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.listCoins);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(212, 263);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Coins";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.listTraps);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(211, 267);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Traps";
            // 
            // listTraps
            // 
            this.listTraps.BackColor = System.Drawing.SystemColors.Control;
            this.listTraps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listTraps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTraps.HideSelection = false;
            this.listTraps.LargeImageList = this.imgListTiles;
            this.listTraps.Location = new System.Drawing.Point(0, 0);
            this.listTraps.Margin = new System.Windows.Forms.Padding(0);
            this.listTraps.MultiSelect = false;
            this.listTraps.Name = "listTraps";
            this.listTraps.Size = new System.Drawing.Size(211, 267);
            this.listTraps.TabIndex = 8;
            this.listTraps.TileSize = new System.Drawing.Size(10, 10);
            this.listTraps.UseCompatibleStateImageBehavior = false;
            this.listTraps.SelectedIndexChanged += new System.EventHandler(this.listTraps_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.listEnemies);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(211, 267);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Enemies";
            // 
            // listEnemies
            // 
            this.listEnemies.BackColor = System.Drawing.SystemColors.Control;
            this.listEnemies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listEnemies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEnemies.HideSelection = false;
            this.listEnemies.LargeImageList = this.imgListTiles;
            this.listEnemies.Location = new System.Drawing.Point(3, 3);
            this.listEnemies.Margin = new System.Windows.Forms.Padding(0);
            this.listEnemies.MultiSelect = false;
            this.listEnemies.Name = "listEnemies";
            this.listEnemies.Size = new System.Drawing.Size(205, 261);
            this.listEnemies.TabIndex = 7;
            this.listEnemies.TileSize = new System.Drawing.Size(10, 10);
            this.listEnemies.UseCompatibleStateImageBehavior = false;
            this.listEnemies.SelectedIndexChanged += new System.EventHandler(this.listEnemies_SelectedIndexChanged);
            // 
            // TilesViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 432);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TilesViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TileSet Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TilesViewer_FormClosing);
            this.Load += new System.EventHandler(this.TilesViewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imgListTiles;
        private System.Windows.Forms.ListView listTiles;
        private System.Windows.Forms.ListView listTemp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listCoins;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listEnemies;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listTraps;
    }
}