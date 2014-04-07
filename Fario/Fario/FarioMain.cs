using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Tile_Engine;

using Platformer.Debugging;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using Application = System.Windows.Forms.Application;

namespace IMPORT_PLATFORM
{
    public enum GameState { NewGame = 0, InGame = 1, Paused = 2, GameOver = 3, FinishedGame = 4 }

    public struct SaveGameData
    {
        public string LevelName;
        public string PlayerName;
        public int PlayerNumber;
        public int PlayerScore;
        public int LivesRemaining;
        public int DeathsRemaining;
        public bool NewGame;
        public TimeSpan ElapesedTime;
    }

    public struct ConfigurationData
    {
        public int ResWidth;
        public int ResHeight;
        public bool isFullScreen;
        public bool isVsync;
        public SoundConfiguration Sound;
        public string StartUpPath;
    }

    public struct SoundConfiguration
    {
        public float MasterVolume;
        public float MusicVolume;
        public float EffectsVolume;
        public bool UseSounds;
        public bool useMusic;
        public bool useEffects;
    }

    public struct HighScoreData
    {
        public string Name;
        public int Score;
        public string LevelName;
        public TimeSpan TotalTime;
    };

    public class FarioMain : Microsoft.Xna.Framework.Game
    {

        #region Declerationgs

        //Graphics And fonts declerations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont stateFont;
        public SpriteFont LargeStateFont { get; set; }
        SpriteFont debugFont;
        SpriteFont particalFont;

        //the randome number generator used across the game
        private Random rnd;

        //player declerations
        private Player player;
        private int playerNumber;
        private string playerName;

        private SaveGameData startingData;
        public ConfigurationData configData;

        public static SaveGameData LastSave { get; set; }

        //TODO remove the numhelper
        NumbersHelper helper;

        //game state manegment stuff
        FarioMain instance;
        GameState activeState = GameState.NewGame;

        //states
        InGame inGame;
        Paused paused;
        NewGame newGame;
        GameOver gameOver;
        GameFinished gameFinished;

        CloudManager cloudManager;

        //the background image
        Texture2D grassBackground;
        Texture2D pinkBackground;
        Texture2D snowBackground;
        Texture2D grayBackground;
        Texture2D orangeBackground;
        Texture2D brownBackground;
        Dictionary<MapType, Texture2D> backgrounds;

        //Logos For The Paused Screen
        public Texture2D LargeGameLogo { get; private set; }
        public Texture2D SmallGameLogo { get; private set; }

        //Logos For The GameOver screen
        public Texture2D GameOverImageLarge { get; private set; }
        public Texture2D GameOverImageSmall { get; private set; }

        //Logos For The Game Finished Screen
        public Texture2D GameFinishedLarge { get; private set; }
        public Texture2D GameFinishedSmall { get; private set; }

        //a timer used to check the elapesed time
        public Stopwatch Timer { get; set; }
        public TimeSpan StartingTime { get; set; }

        //starting a new game map name
        private const string newGameMap = "1";

        public System.Collections.Generic.List<HighScoreData> HighScoreData { get; private set; }

        #endregion

        #region Properties

        public InGame InGameScreen
        {
            get { return inGame; }
        }

        public Paused PausedScreen
        {
            get { return paused; }
        }

        public GameOver GameOverScreen
        {
            get { return gameOver; }
        }

        public NewGame NewGameScreen
        {
            get { return newGame; }
        }

        public Player GamePlayer
        {
            get { return player; }
        }

        public int PlayerNumber
        {
            get { return playerNumber; }
            set { playerNumber = value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public string NewGameMap
        {
            get { return newGameMap; }
        }

        public SpriteFont StateFont
        {
            get { return stateFont; }
        }

        public SpriteFont ParticalFont
        {
            get { return particalFont; }
        }

        public SpriteFont DebugFont
        {
            get { return debugFont; }
        }

        public NumbersHelper NumHelper
        {
            get { return helper; }
        }

        public Random Randomizer
        {
            get { return rnd; }
        }

        public TimeSpan TotalTime
        {
            get { return StartingTime.Add(Timer.Elapsed); }
        }

        public SaveGameData StartingData
        {
            get { return startingData; }
        }

        public SoundConfiguration SoundSettings
        {
            get { return configData.Sound; }
            set { configData.Sound = value; }
        }

        public ConfigurationData Configurations
        {
            get { return configData; }
        }

        #endregion

        #region Iniyialization

        public FarioMain(SaveGameData saveData, ConfigurationData configData)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.startingData = saveData;
            this.configData = configData;
        }

        protected override void Initialize()
        {
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            Window.Title = "Fario";
            Window.AllowUserResizing = false;

            helper = new NumbersHelper();
            rnd = new Random();
            instance = this;

            graphics.PreferredBackBufferWidth = configData.ResWidth;
            graphics.PreferredBackBufferHeight = configData.ResHeight;
            graphics.IsFullScreen = configData.isFullScreen;
            graphics.SynchronizeWithVerticalRetrace = configData.isVsync;
            graphics.ApplyChanges();

            this.IsFixedTimeStep = false;

#if DEBUG
            DebugSystem.Initialize(instance, @"Fonts/debugFont");
            DebugSystem.Instance.FpsCounter.Visible = true;
            DebugSystem.Instance.TimeRuler.Visible = true;
            DebugSystem.Instance.TimeRuler.ShowLog = true;
            DebugSystem.Instance.DebugCommandUI.Enabled = false;
#endif

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Loading Fonts

            stateFont = Content.Load<SpriteFont>(@"Fonts\StateFont");
            stateFont.LineSpacing = 30;

            LargeStateFont = Content.Load<SpriteFont>(@"Fonts\StateFontLarge");
            LargeStateFont.LineSpacing = 50;

            particalFont = Content.Load<SpriteFont>(@"Fonts\particalFont");

            debugFont = Content.Load<SpriteFont>(@"Fonts\debugFont");

            #endregion

            #region Camera
            Camera.Position = Vector2.Zero;
            Camera.ViewPortWidth = graphics.PreferredBackBufferWidth;
            Camera.ViewPortHeight = graphics.PreferredBackBufferHeight;
            #endregion

            #region Backgrounds Textures Loading

            grassBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundGrass");
            pinkBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundPink");
            snowBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundSnow");
            grayBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundGray");
            orangeBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundOrange");
            brownBackground = Content.Load<Texture2D>(@"Textures\Backgrounds\BackgroundBrown");

            backgrounds = new Dictionary<MapType, Texture2D>();
            backgrounds.Add(MapType.Grass, grassBackground);
            backgrounds.Add(MapType.Candy, pinkBackground);
            backgrounds.Add(MapType.Chocolat, orangeBackground);
            backgrounds.Add(MapType.Castle, grayBackground);
            backgrounds.Add(MapType.StoneBrown, grayBackground);
            backgrounds.Add(MapType.Vanilla, grayBackground);
            backgrounds.Add(MapType.StoneWhite, grayBackground);
            backgrounds.Add(MapType.StonePurpel, grayBackground);
            backgrounds.Add(MapType.Ice, snowBackground);

            #endregion

            this.LargeGameLogo = Content.Load<Texture2D>(@"Textures/Title");
            this.SmallGameLogo = Content.Load<Texture2D>(@"Textures/TitleSmall");

            this.GameOverImageLarge = Content.Load<Texture2D>(@"Textures/GameOver");
            this.GameOverImageSmall = Content.Load<Texture2D>(@"Textures/GameOverSmall");

            this.GameFinishedLarge = Content.Load<Texture2D>(@"Textures/FinishedLogoLarge");
            this.GameFinishedSmall = Content.Load<Texture2D>(@"Textures/FinishedLogoSmall");


            player = new Player(instance, startingData.PlayerNumber, helper);
            player.WorldLocation = Vector2.Zero;

            newGame = new NewGame(instance);
            inGame = new InGame(instance);
            paused = new Paused(instance);
            gameOver = new GameOver(instance);
            gameFinished = new GameFinished(instance);

            cloudManager = new CloudManager(instance);


            Timer = new Stopwatch();
            Timer.Reset();

            MusicManager.Instance.LoadContent(instance);

            UpdateHighScoreList();

            LastSave = new SaveGameData()
            {
                LevelName = "-1"
            };

            SetState(GameState.NewGame);
        }

        protected override void UnloadContent()
        {
            Content.Dispose();
        }

        #endregion

        #region Update And Draw

        protected override void Update(GameTime gameTime)
        {

#if DEBUG
            DebugSystem.Instance.TimeRuler.StartFrame();
            DebugSystem.Instance.TimeRuler.BeginMark("Update", Color.Blue);

            DebugInput();
#endif

            switch (activeState)
            {
                case GameState.NewGame:
                    newGame.Update(gameTime);
                    break;
                case GameState.InGame:
                    inGame.Update(gameTime);
                    break;
                case GameState.Paused:
                    paused.Update(gameTime);
                    break;
                case GameState.GameOver:
                    gameOver.Update(gameTime);
                    break;
                case GameState.FinishedGame:
                    gameFinished.Update(gameTime);
                    break;
            }
            cloudManager.Update(gameTime);

            MusicManager.Instance.Update();            
            Input.Update();

            base.Update(gameTime);

#if DEBUG
            DebugSystem.Instance.TimeRuler.EndMark("Update");
#endif

        }

        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            DebugSystem.Instance.TimeRuler.BeginMark("Draw", Color.Red);
#endif
            
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            spriteBatch.Draw(backgrounds[TileMap.MapType], new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            cloudManager.Draw(spriteBatch);

            switch (activeState)
            {
                case GameState.NewGame:
                    newGame.Draw(spriteBatch);
                    break;
                case GameState.InGame:
                    inGame.Draw(spriteBatch);
                    break;
                case GameState.Paused:
                    paused.Draw(spriteBatch);
                    break;
                case GameState.GameOver:
                    gameOver.Draw(spriteBatch);
                    break;
                case GameState.FinishedGame:
                    gameFinished.Draw(spriteBatch);
                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);

#if DEBUG
            DebugSystem.Instance.TimeRuler.EndMark("Draw");
#endif

        }

        #endregion

        #region HelperMethods

        private void DebugInput()
        {
            if (Input.KeyPressed(Keys.F1))
            {
                TileMap.EditorMode = !TileMap.EditorMode;
            }
            if (Input.KeyPressed(Keys.F2))
            {
                activeState = GameState.NewGame;
            }
            if (Input.KeyPressed(Keys.F3))
            {
                activeState = GameState.InGame;
            }
            if (Input.KeyPressed(Keys.F4))
            {
                activeState = GameState.GameOver;
            }
            if (Input.KeyPressed(Keys.F5))
            {
                activeState = GameState.FinishedGame;
            }
        }

        public void SetState(GameState state)
        {
            this.activeState = state;
        }


        public void RestartLevel()
        {
            player.Speed = Vector2.Zero;
            player.Score = inGame.LevelMANAGER.StartingScore;
            inGame.LevelMANAGER.ReloadLevel();
            inGame.InGameState = InGameState.Playing;
        }
        
        public void LoadLastSave()
        {
            if (LastSave.LevelName != "-1")
            {
                playerName = LastSave.PlayerName;
                playerNumber = LastSave.PlayerNumber;
                player.Score = LastSave.PlayerScore;
                player.LivesRemaining = LastSave.LivesRemaining;
                player.DeathsRemaining = LastSave.DeathsRemaining;
                StartingTime = LastSave.ElapesedTime;

                player.Dead = false;

                inGame.LevelMANAGER.LoadLevel(LastSave.LevelName);

                Timer.Reset();
                if (!Timer.IsRunning)
                    Timer.Start();

                if (MediaPlayer.State != MediaState.Playing)
                {
                    MusicManager.Instance.StartLoop();
                }

                SetState(GameState.InGame);
            }
        }

        public void SaveHighScore()
        {
            string folderPath = Application.StartupPath + "/Content/Settings";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = folderPath + "/HighScores.Data";
            string data;
            if (!File.Exists(filePath))
            {
                data = "";
            }
            else
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    data = reader.ReadToEnd();
                }
            }
            StringBuilder builder = new StringBuilder(data);
            builder.Append(playerName);
            builder.Append("," + player.Score.ToString());
            builder.Append("," + inGame.LevelMANAGER.CurrentLevel);
            builder.Append("," + TotalTime.Hours.ToString());
            builder.Append("," + TotalTime.Minutes.ToString());
            builder.AppendLine("," + TotalTime.Seconds.ToString());

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(builder.ToString());
            }

            UpdateHighScoreList();
        }

        private void UpdateHighScoreList()
        {
            string filePath = Application.StartupPath + "/Content/Settings/HighScores.Data";
            if (!File.Exists(filePath))
            {
                HighScoreData = new List<HighScoreData>();
            }
            else
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    HighScoreData = new List<HighScoreData>();
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] info = line.Split(',');
                        HighScoreData data = new HighScoreData()
                        {
                            Name = info[0],
                            Score = int.Parse(info[1]),
                            LevelName = info[2],
                            TotalTime = new TimeSpan(int.Parse(info[3]), int.Parse(info[4]), int.Parse(info[5]))
                        };
                        HighScoreData.Add(data);
                    }
                }

                if (HighScoreData.Count > 21)
                {
                    while (HighScoreData.Count > 21)
                    {
                        HighScoreData.RemoveAt(0);
                    }
                }
            }
        }

        #endregion
    }
}