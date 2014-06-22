using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework;
using System.IO;
using Tile_Engine;
using System.Xml;

namespace Level_Editor
{
    public partial class MapEditor : Form
    {

        public Game1 game;
        public TilesViewer viewer = new TilesViewer();
        public static int proMin = 0;
        public static int proMax = 1;
        public static int proValue = 0;
        public static bool canExit = true;
        
        OpenFileDialog open = new OpenFileDialog();

        public bool MakeGround
        {
            get { return !chkPass.Checked; }
        }

        public MapEditor()
        {
            InitializeComponent();

            open.Filter = "Fario Map Files (*.FarioMap)|*.FarioMap";
            open.InitialDirectory = Application.StartupPath;
            open.Multiselect = false;
            open.Title = "Load Map";
            open.ValidateNames = true;
            open.AutoUpgradeEnabled = true;

            string[] mapTypes = Enum.GetNames(typeof(MapType));
            for (int i = 0; i < Enum.GetNames(typeof(MapType)).Length; i++)
            {
                cmboMapTypes.Items.Add(mapTypes[i]);
            }
            cmboMapTypes.SelectedIndex = 0;

            FixScrollBarScales();
            FillComboBoxes();

            TileMap.EditorMode = true;
            checkBox1.Checked = true;

            toolStripProgressBar1.Minimum = 0;
            txtNewCode.Enabled = false;
            cboCodeValues.SelectedIndex = 0;

            toolStripComboBox1.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.Exit();
            Application.Exit();
        }

        private void FixScrollBarScales()
        {
            Camera.ViewPortWidth = pctSurface.Width;
            Camera.ViewPortHeight = pctSurface.Height;
            Camera.Move(Vector2.Zero);
            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = (Camera.WorldRectangle.Height - Camera.ViewPortHeight) + TileMap.TileHeight;

            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = (Camera.WorldRectangle.Width - Camera.ViewPortWidth) + TileMap.TileWidth;
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {
            lblMapWidth.Text = TileMap.MapWidth.ToString();
            lblMapHeight.Text = TileMap.MapHeight.ToString();
        }

        private void FillComboBoxes()
        {
            cboCodeValues.Items.Clear();
            cboCodeValues.Items.Add("Start");
            cboCodeValues.Items.Add("Clear");
            cboCodeValues.Items.Add("Move");
            cboCodeValues.Items.Add("Custom");
        }

        private void MapEditor_Resize(object sender, EventArgs e)
        {
            FixScrollBarScales();
        }

        private void cboCodeValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNewCode.Enabled = false;
            switch (
                cboCodeValues.Items[cboCodeValues.SelectedIndex].ToString())
            {
                case "Start":
                    txtNewCode.Text = "START";
                    break;
                case "Clear":
                    txtNewCode.Text = "";
                    break;
                case "Move" :
                    txtNewCode.Text = "To ";
                    txtNewCode.Enabled = true;
                    break;
                case "Custom":
                    txtNewCode.Text = "";
                    txtNewCode.Enabled = true;
                    break;
            }
        }

        private void MapEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
                "Are You Sure You Want To Exit ?\nAll Proggress Made Will Be Lost If You Exit Now",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)
                ==
                System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                game.Exit();
            }
        }

        private void radioPassable_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPassable.Checked)
            {
                game.EditingCode = false;
            }
            else
            {
                game.EditingCode = true;
            }
        }

        private void radioCode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPassable.Checked)
            {
                game.EditingCode = false;
            }
            else
            {
                game.EditingCode = true;
            }
        }

        private void txtNewCode_TextChanged(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.CurrentCodeValue = txtNewCode.Text;
            }
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.DrawLayer = 0;
        }

        private void interactiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.DrawLayer = 1;
        }

        private void foregroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.DrawLayer = 2;
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            if (hScrollBar1.Maximum < 0)
            {
                FixScrollBarScales();
            }
            game.Tick();
            if (game.HoverCodeValue != lblCurrentCode.Text)
            {
                lblCurrentCode.Text = game.HoverCodeValue;
            }
            lblLocX.Text = game.TileLocation.X.ToString();
            lblLocY.Text = game.TileLocation.Y.ToString();
            if (pctSurface.Capture == true)
            {
                game.shouldSet = true;
            }
            if (pctSurface.Capture == false)
            {
                game.shouldSet = false;
            }
            toolStripProgressBar1.Minimum = proMin;
            toolStripProgressBar1.Maximum = proMax;
            toolStripProgressBar1.Value = (int)MathHelper.Clamp(proValue, proMin, proMax);
            if (proValue >= proMax)
                toolStripProgressBar1.Value = 0;

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.DrawLayer = toolStripComboBox1.SelectedIndex;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                TileMap.EditorMode = true;
            }
            if (!checkBox1.Checked)
            {
                TileMap.EditorMode = false;
            }
        }

        private void btnNewMap_Click(object sender, EventArgs e)
        {
            if (txtMapHeight.Text == "" || txtMapWidth.Text == "")
            {
                MessageBox.Show("Please Enter Values For The Map Height And Width", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TileMap.MapWidth = int.Parse(txtMapWidth.Text);
            TileMap.MapHeight = int.Parse(txtMapHeight.Text);
            TileMap.ResetMapSquares();
            TileMap.ClearMap();
            game.specialTiles.Clear();
            FixScrollBarScales();
            MessageBox.Show("New Map Created Sucssesfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblMapHeight.Text = TileMap.MapHeight.ToString();
            lblMapWidth.Text = TileMap.MapWidth.ToString();
            txtMapName.Text = "";
        }

        private void btnViewTiles_Click(object sender, EventArgs e)
        {
            game.shouldDraw = false;
            game.shouldUpdate = false;
            if (viewer.IsDisposed)
            {
                viewer = new TilesViewer();
                viewer.Show(); 
                viewer.BringToFront();
            }
            else
            {
                viewer.Show();
                viewer.BringToFront();
            }
            game.shouldDraw = true;
            game.shouldUpdate = true;
        }

        private void btnClearMap_Click(object sender, EventArgs e)
        {
            TileMap.ClearMap();
            game.specialTiles.Clear();
            MessageBox.Show("Map Cleared And All Tiles Has Benn Reset !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            game.mapChanged = false;
        }

        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            game.shouldUpdate = false;
            game.shouldDraw = false;
            string mapName = txtMapName.Text;
            if (mapName == "")
            {
                MessageBox.Show("The Map Name Cannot Be Empty, Please Type A Map Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TileMap.SaveMap(Application.StartupPath, mapName);
            MessageBox.Show("Map Saved To Game Directory", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            game.mapChanged = false;
            game.shouldUpdate = true;
            game.shouldDraw = true; ;
        }

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            game.shouldUpdate = false;
            game.shouldDraw = false;
            DialogResult shouldLoad;
            bool load = true;
            if (game.mapChanged)
            {
                shouldLoad = MessageBox.Show("The Map Has Been Changed, Are You Sure You Want To Discard The Changes ?", "Confirm Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (shouldLoad == DialogResult.No) { load = false; }
            }
            if (load)
            {
                try
                {
                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //TileMap.LoadMap(Application.StartupPath, mapName);
                        TileMap.LoadMap(open.FileName);
                        FixScrollBarScales();
                        game.specialTiles.Clear();
                        MessageBox.Show("Map Has Been Sucssesfully Loaded !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        game.mapChanged = false;
                        lblMapType.Text = TileMap.MapType.ToString();

                        lblMapHeight.Text = TileMap.MapHeight.ToString();
                        lblMapWidth.Text = TileMap.MapWidth.ToString();
                        string name = open.SafeFileName;
                        name = name.Replace(".FarioMap", "");
                        txtMapName.Text = name;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Unable To Load Map File");
                    MessageBox.Show("Error Loading Map File\nCheck That The File Exists And Try Again\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.Message);
                }
            }
            game.shouldUpdate = true;
            game.shouldDraw = true; ;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMapWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMapHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmboMapTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            TileMap.MapType = (MapType)cmboMapTypes.SelectedIndex;
            lblMapType.Text = TileMap.MapType.ToString();
        }

        private void MapEditor_MouseLeave(object sender, EventArgs e)
        {
            //toolStripComboBox1.Enabled = false;
        }

        private void MapEditor_MouseEnter(object sender, EventArgs e)
        {
            //toolStripComboBox1.Enabled = true;
        }

        private void groupBoxRightClick_MouseHover(object sender, EventArgs e)
        {
            //toolStripComboBox1.Enabled = true;
        }

        private void toolStrip2_MouseEnter(object sender, EventArgs e)
        {
            //toolStripComboBox1.Enabled = true;
        }

        private void toolStrip1_MouseEnter(object sender, EventArgs e)
        {
            //toolStripComboBox1.Enabled = true;
        }

        private void pctSurface_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (pctSurface.Capture == true) toolStripComboBox1.Enabled = false;
            if (pctSurface.Capture == false) toolStripComboBox1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (frmResize resize = new frmResize())
            {
                resize.ShowDialog();
                if (resize.ClickedOk)
                {
                    TileMap.MapWidth = resize.MapWidth;
                    TileMap.MapHeight = resize.MapHeight;
                    MessageBox.Show("Map Resized", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblMapHeight.Text = resize.MapHeight.ToString();
                    lblMapWidth.Text = resize.MapWidth.ToString();
                }
            }
        }
    }
}

