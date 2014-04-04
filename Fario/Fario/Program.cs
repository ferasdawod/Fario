using System;
using System.Windows.Forms;

namespace IMPORT_PLATFORM
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            SaveGameData data = new SaveGameData();
            data.PlayerName = "Feras";
            data.PlayerNumber = 4;
            data.PlayerScore = 0;
            data.LivesRemaining = 3;
            data.LevelName = "1";
            data.DeathsRemaining = 3;
            data.NewGame = true;

            ConfigurationData da = new ConfigurationData();
            da.ResWidth = 1366;
            da.ResHeight = 758;
            da.isVsync = false;
            da.isFullScreen = false;
            da.Sound = new SoundConfiguration()
            {
                UseSounds = false,
                useMusic = false,
                useEffects = false,
                EffectsVolume = 1.0f,
                MasterVolume = 1.0f,
                MusicVolume = 1.0f
            };
            da.StartUpPath = Application.StartupPath;

            using (FarioMain game = new FarioMain(data, da))
            {
                game.Run();
            }
#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameLauncher launcher = new GameLauncher();
            Application.Run(launcher);
#endif
        }
    }
#endif
}

