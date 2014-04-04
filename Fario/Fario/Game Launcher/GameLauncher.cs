using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace IMPORT_PLATFORM
{
    public partial class GameLauncher : Form
    {
        private List<SaveGameData> saves;

        SaveGameData saveData;
        ConfigurationData configData;
        SoundConfiguration sounds;
        TimeSpan SavedGameTime;

        int playerNumber = 0;

        public GameLauncher()
        {
            InitializeComponent();

            saveData = new SaveGameData();
            configData = new ConfigurationData();
            sounds = new SoundConfiguration();
            SavedGameTime = new TimeSpan();
        }

        private void StartGame()
        {
            using (FarioMain game = new FarioMain(saveData, configData))
            {
                game.Run();
                Application.Exit();
            }
        }

        private void LoadSaveGames()
        {
            saves = new List<SaveGameData>();
            listSaveGames.Items.Clear();

            string folderPath;
            folderPath = Application.StartupPath + "/Content/SavedGames";
            if (!Directory.Exists(folderPath))
                return;

            btnLoadGame.Enabled = false;
            foreach (string f in Directory.GetFiles(folderPath))
            {
                using (StreamReader reader = new StreamReader(f))
                {
                    try
                    {
                        SaveGameData data = new SaveGameData();
                        data.NewGame = false;
                        data.PlayerName = reader.ReadLine();
                        data.PlayerNumber = int.Parse(reader.ReadLine());
                        data.PlayerScore = int.Parse(reader.ReadLine());
                        data.LivesRemaining = int.Parse(reader.ReadLine());
                        data.DeathsRemaining = int.Parse(reader.ReadLine());
                        data.LevelName = reader.ReadLine();
                        int hours = int.Parse(reader.ReadLine());
                        int minuts = int.Parse(reader.ReadLine());
                        int seconds = int.Parse(reader.ReadLine());
                        data.ElapesedTime = new TimeSpan(hours, minuts, seconds);
                        listSaveGames.Items.Add(data.PlayerName);
                        saves.Add(data);
                        btnLoadGame.Enabled = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error Loading Saved Game Files...\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (saves.Count > 0)
            {
                UpdateSavedInfo(0);
                listSaveGames.SelectedIndex = 0;
            }
            else
            {
                lblDeaths.Text = "N/A";
                lblLevelNumber.Text = "N/A";
                lblLives.Text = "N/A";
                lblPlayerName.Text = "N/A";
                lblPlayerScore.Text = "N/A";
                pctrPlayer.Image = null;
            }
        }

        private void UpdateSavedInfo(int savedGameIndex)
        {
            lblPlayerName.Text = saves[savedGameIndex].PlayerName;
            lblPlayerScore.Text = saves[savedGameIndex].PlayerScore.ToString();
            lblLives.Text = saves[savedGameIndex].LivesRemaining.ToString();
            lblDeaths.Text = saves[savedGameIndex].DeathsRemaining.ToString();
            lblLevelNumber.Text = saves[savedGameIndex].LevelName;
            lblGameTime.Text = saves[savedGameIndex].ElapesedTime.Hours + ":" + saves[savedGameIndex].ElapesedTime.Minutes + ":" + saves[savedGameIndex].ElapesedTime.Seconds;
            switch (saves[savedGameIndex].PlayerNumber)
            {
                case 0:
                    pctrPlayer.Image = Fario.Resource1.alienGreen;
                    break;
                case 1:
                    pctrPlayer.Image = Fario.Resource1.alienBeige;
                    break;
                case 2:
                    pctrPlayer.Image = Fario.Resource1.alienBlue;
                    break;
                case 3:
                    pctrPlayer.Image = Fario.Resource1.alienPink;
                    break;
                case 4:
                    pctrPlayer.Image = Fario.Resource1.alienYellow;
                    break;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Please Enter a Name For Your Character", "Name Needed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            saveData.PlayerName = txtPlayerName.Text;
            saveData.PlayerNumber = playerNumber;
            saveData.PlayerScore = 0;
            saveData.LivesRemaining = 3;
            saveData.LevelName = "1";
            saveData.DeathsRemaining = 3;
            saveData.NewGame = true;

            sounds.MasterVolume = trackMaster.Value / 100.0f;
            sounds.MusicVolume = trackMusic.Value / 100.0f;
            sounds.EffectsVolume = trackEffects.Value / 100.0f;

            string res = (string)cboResolution.SelectedItem;
            string[] r = res.Split('x');
            configData.ResWidth = int.Parse(r[0]);
            configData.ResHeight = int.Parse(r[1]);
            configData.isVsync = chkVsync.Checked;
            configData.isFullScreen = chkFullScreen.Checked;
            configData.Sound = sounds;
            configData.StartUpPath = Application.StartupPath;

            Thread newThread = new Thread(StartGame);
            newThread.Start();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GameLauncher_Load(object sender, EventArgs e)
        {
            chkPlayer1.Checked = true;

            cboResolution.Items.Add("640x480");
            cboResolution.Items.Add("800x600");
            cboResolution.Items.Add("1024x768");
            cboResolution.Items.Add("1366x768");
            cboResolution.Items.Add("1280x1024");
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            switch (screenWidth)
            {
                case 640:
                    cboResolution.SelectedIndex = 0;
                    break;
                case 800:
                    cboResolution.SelectedIndex = 1;
                    break;
                case 1024:
                    cboResolution.SelectedIndex = 2;
                    break;
                case 1366:
                    cboResolution.SelectedIndex = 3;
                    break;
                case 1280:
                    cboResolution.SelectedIndex = 4;
                    break;
                default:
                    cboResolution.SelectedIndex = cboResolution.Items.Count - 1;
                    break;
            }

            LoadSaveGames();

            sounds.UseSounds = chkMaster.Checked;
            sounds.useMusic = chkMusic.Checked;
            sounds.useEffects = chkEffects.Checked;

            lblEffectsVolume.Text = trackEffects.Value.ToString();
            lblMasterVolume.Text = trackMaster.Value.ToString();
            lblMusicVolume.Text = trackMusic.Value.ToString();

            txtPlayerName.Focus();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            chkPlayer2.Checked = true;            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            chkPlayer1.Checked = true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            chkPlayer3.Checked = true;            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            chkPlayer4.Checked = true;            
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            chkPlayer5.Checked = true;            
        }

        private void chkPlayer1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayer1.Checked == true)
            {
                playerNumber = 4;
            }
        }

        private void chkPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayer2.Checked == true)
            {
                playerNumber = 3;
            }
        }

        private void chkPlayer3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayer3.Checked == true)
            {
                playerNumber = 0;
            }
        }

        private void chkPlayer4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayer4.Checked == true)
            {
                playerNumber = 2;
            }
        }

        private void chkPlayer5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayer5.Checked == true)
            {
                playerNumber = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void listSaveGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSavedInfo(listSaveGames.SelectedIndex);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (saves.Count > 0)
            {
                if (MessageBox.Show("Are You Sure You Want To Delete This Save Game ?\n" +
                "Once You Delete It You Wont Be Able To Load That Game Again...", "Confirmation"
                , MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    string path = Application.StartupPath + "/Content/SavedGames/" + saves[listSaveGames.SelectedIndex].PlayerName + ".Save";
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        LoadSaveGames();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Save Data Is Available To Delete...\nTo Save The Game Press \"L\" While The Game Is Paused.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            saveData = saves[listSaveGames.SelectedIndex];

            sounds.MasterVolume = trackMaster.Value / 100.0f;
            sounds.MusicVolume = trackMusic.Value / 100.0f;
            sounds.EffectsVolume = trackEffects.Value / 100.0f;

            string res = (string)cboResolution.SelectedItem;
            string[] r = res.Split('x');
            configData.ResWidth = int.Parse(r[0]);
            configData.ResHeight = int.Parse(r[1]);
            configData.isVsync = chkVsync.Checked;
            configData.isFullScreen = chkFullScreen.Checked;
            configData.Sound = sounds;
            configData.StartUpPath = Application.StartupPath;

            Thread newThread = new Thread(StartGame);
            newThread.Start();
            this.Hide();
        }

        private void trackMaster_Scroll(object sender, EventArgs e)
        {
            lblMasterVolume.Text = trackMaster.Value.ToString();
        }

        private void trackMusic_Scroll(object sender, EventArgs e)
        {
            lblMusicVolume.Text = trackMusic.Value.ToString();
        }

        private void trackEffects_Scroll(object sender, EventArgs e)
        {
            lblEffectsVolume.Text = trackEffects.Value.ToString();
        }

        private void chkMusic_CheckedChanged(object sender, EventArgs e)
        {
            sounds.useMusic = chkMusic.Checked;
        }

        private void chkEffects_CheckedChanged(object sender, EventArgs e)
        {
            sounds.useEffects = chkEffects.Checked;
        }

        private void chkMaster_CheckedChanged(object sender, EventArgs e)
        {
            sounds.UseSounds = chkMaster.Checked;
        }
    }
}