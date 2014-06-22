using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Tile_Engine;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using Application = System.Windows.Forms.Application;

namespace IMPORT_PLATFORM
{
    public class Paused
    {
        private enum CurrentScreen { States = 0, Options = 1, HighScores = 2 }

        #region Declerations

        private Player WalkingPlayer0;
        private Player WalkingPlayer1;

        private ContentManager Content;

        private SpriteFont gameFont;

        private FarioMain mainGame;

        private NumbersHelper numHelper;

        private float distance;
        private Vector2 stringLocation;

        private Texture2D title;
        private Vector2 titleLocation;

        private StringBuilder states;

        private CurrentScreen activeScreen = CurrentScreen.States;

        #endregion

        #region Initialization

        public Paused(FarioMain parentGame)
        {
            mainGame = parentGame;
            Content = mainGame.Content;
            gameFont = mainGame.StateFont;
            numHelper = mainGame.NumHelper;

            WalkingPlayer0 = new Player(mainGame, 0, numHelper);
            WalkingPlayer0.WorldLocation = new Vector2((mainGame.Window.ClientBounds.Width / 4) - WalkingPlayer0.FrameWidth, mainGame.Window.ClientBounds.Height * (2 / 4.0f));
            WalkingPlayer0.HasCollisions = false;
            WalkingPlayer0.HasGravity = false;
            WalkingPlayer0.HasInput = false;
            WalkingPlayer0.PlayAnimation("Walk");
            WalkingPlayer0.Flipped = true;

            WalkingPlayer1 = new Player(mainGame, 3, numHelper);
            WalkingPlayer1.WorldLocation = new Vector2(mainGame.Window.ClientBounds.Width * (3 / 4.0f), mainGame.Window.ClientBounds.Height * (2 / 4.0f));
            WalkingPlayer1.HasCollisions = false;
            WalkingPlayer1.HasGravity = false;
            WalkingPlayer1.HasInput = false;
            WalkingPlayer1.PlayAnimation("Walk");

            distance = Math.Abs(WalkingPlayer0.WorldLocation.X - WalkingPlayer1.WorldLocation.X);
            stringLocation = new Vector2(WalkingPlayer0.WorldRectangle.Right + (distance / 4), WalkingPlayer0.WorldRectangle.Top - WalkingPlayer0.WorldRectangle.Height);

            title = mainGame.SmallGameLogo;
            titleLocation = Extensions.AllignThing(new Vector2(title.Width, title.Height), TextLocation.TopMid, mainGame.Window.ClientBounds);

            states = new StringBuilder();
        }

        #endregion

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            CheckInput();

            if (Input.KeyPressed(Keys.Escape))
                mainGame.SetState(GameState.InGame);

            if (Input.KeyPressed(Keys.S))
                SaveGame();

            if (Input.KeyPressed(Keys.L))
                mainGame.LoadLastSave();

            if (Input.KeyPressed(Keys.Q))
                mainGame.QuitGame();

            if (Input.KeyPressed(Keys.M))
            {
                mainGame.configData.Sound.UseSounds = !mainGame.configData.Sound.UseSounds;
                if (!mainGame.configData.Sound.UseSounds)
                {
                    MusicManager.Instance.StopLoop();
                }
                else
                {
                    MusicManager.Instance.Stopped = false;
                }
            }

            if (Input.KeyPressed(Keys.N))
            {
                mainGame.configData.Sound.useMusic = !mainGame.configData.Sound.useMusic;
                if (!mainGame.configData.Sound.useMusic)
                {
                    MusicManager.Instance.StopLoop();
                }
                else
                {
                    MusicManager.Instance.Stopped = false;
                }
            }

            if (Input.KeyPressed(Keys.B))
                mainGame.configData.Sound.useEffects = !mainGame.configData.Sound.useEffects;


            WalkingPlayer0.Update(gameTime);
            WalkingPlayer1.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPermaStuff(spriteBatch);
            string line = "Current Screen : " + activeScreen.ToString();
            spriteBatch.DrawString(mainGame.StateFont, line, Vector2.Zero, Color.White);
            line = "Press Left And Right To Switch Screens";
            Vector2 loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(line), TextLocation.BotMid, mainGame.Window.ClientBounds);
            spriteBatch.DrawString(mainGame.StateFont, line, loc, Color.White);
            DrawPlayers(spriteBatch);
            switch (activeScreen)
            {
                case CurrentScreen.States:
                    DrawStates(spriteBatch);
                    break;
                case CurrentScreen.Options:
                    DrawOptions(spriteBatch);
                    break;
                case CurrentScreen.HighScores:
                    DrawHighScores(spriteBatch);
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void CheckInput()
        {
            if (Input.KeyPressed(Keys.Left))
            {
                activeScreen--;
                if ((int)activeScreen < 0)
                {
                    activeScreen = CurrentScreen.HighScores;
                }
            }
            if (Input.KeyPressed(Keys.Right))
            {
                activeScreen++;
                if ((int)activeScreen > 2)
                {
                    activeScreen = CurrentScreen.States;
                }
            }
            if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
            {
                activeScreen++;
                if ((int)activeScreen > 2)
                {
                    activeScreen = CurrentScreen.States;
                }
            }
        }

        private void DrawPermaStuff(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(title, titleLocation, Color.White);
        }

        private void DrawOptions(SpriteBatch spriteBatch)
        {
            states.Clear();
            states.AppendLine("Game Options :");
            states.AppendLine();

            states.AppendLine("Press \"S\" To Save The Game");
            states.AppendLine("Press \"L\" To Load The Last Saved Game");
            states.AppendLine();

            states.AppendLine("Press \"M\" To Toggle Master Sound On/Off");
            states.AppendLine("     Master Sound : " + mainGame.Configurations.Sound.UseSounds);

            states.AppendLine("Press \"N\" To Toggle Music Sound On/Off");
            states.AppendLine("     Music Sound : " + mainGame.Configurations.Sound.useMusic);

            states.AppendLine("Press \"B\" To Toggle Effects Sound On/Off");
            states.AppendLine("     Effects Sound : " + mainGame.Configurations.Sound.useEffects);
            states.AppendLine();
            states.AppendLine();

            states.AppendLine("Press \"Q\" To Exit The Game");

            Vector2 loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(states), TextLocation.Center, mainGame.Window.ClientBounds);
            loc.Y += loc.Y / 2;

            spriteBatch.DrawString(mainGame.StateFont, states, loc, Color.White);
        }

        private void DrawPlayers(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(WalkingPlayer0.Animations["Walk"].Texture, WalkingPlayer0.WorldLocation, WalkingPlayer0.Animations["Walk"].FrameRectangle, Color.White);
            spriteBatch.Draw(WalkingPlayer1.Animations["Walk"].Texture, WalkingPlayer1.WorldLocation, WalkingPlayer1.Animations["Walk"].FrameRectangle, Color.White);
        }

        private void DrawStates(SpriteBatch spritebatch)
        {
            states.Clear();
            states.AppendLine("--- The Game Is Paused ---");
            states.AppendLine();
            states.AppendLine("> Player Name : " + mainGame.PlayerName);
            states.AppendLine("> Player Score : " + mainGame.GamePlayer.Score);
            states.AppendLine("> Lives Remaining : " + mainGame.GamePlayer.LivesRemaining);
            states.AppendLine("> Deaths Remaining : " + mainGame.GamePlayer.DeathsRemaining);
            states.AppendLine("> Level Number : " + mainGame.InGameScreen.LevelName);

            Vector2 loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(states), TextLocation.Center, mainGame.Window.ClientBounds);

            spritebatch.DrawString(mainGame.StateFont, states, loc, Color.White);
        }

        private void DrawHighScores(SpriteBatch spriteBatch)
        {
            if (mainGame.HighScoreData.Count != 0)
            {
                states.Clear();
                states.AppendLine();
                states.AppendLine("Player Name    Player Score    Level Name    Game Time");
                states.AppendLine();
                foreach (HighScoreData data in mainGame.HighScoreData)
                {
                    string line = "- " + data.Name;
                    string wholeLine;
                    line = line.PadRight(40 - line.Length, '.');
                    wholeLine = line;

                    line = data.Score.ToString();
                    line = line.PadRight(32 - line.Length, '.');
                    wholeLine += line;

                    line = data.LevelName;
                    line = line.PadRight(25 - line.Length, '.');
                    wholeLine += line;

                    line = data.TotalTime.Minutes.ToString() + ":" + data.TotalTime.Seconds.ToString();
                    wholeLine += line;
                    states.AppendLine(wholeLine);
                }

                Vector2 loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(states), TextLocation.Center, mainGame.Window.ClientBounds);

                spriteBatch.DrawString(mainGame.StateFont, states.ToString(), loc, Color.White);
            }
            else
            {
                string line = "No HighScore Data is Available Yet...";

                Vector2 loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(line), TextLocation.Center, mainGame.Window.ClientBounds);
                spriteBatch.DrawString(mainGame.StateFont, line, loc, Color.White);
            }
        }

        private void SaveGame()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(mainGame.PlayerName);
            builder.AppendLine(mainGame.PlayerNumber.ToString());
            builder.AppendLine(mainGame.InGameScreen.LevelMANAGER.StartingScore.ToString());
            builder.AppendLine(mainGame.GamePlayer.LivesRemaining.ToString());
            builder.AppendLine(mainGame.GamePlayer.DeathsRemaining.ToString());
            builder.AppendLine(mainGame.InGameScreen.LevelMANAGER.CurrentLevel);
            builder.AppendLine(mainGame.TotalTime.Hours.ToString());
            builder.AppendLine(mainGame.TotalTime.Minutes.ToString());
            builder.AppendLine(mainGame.TotalTime.Seconds.ToString());

            FarioMain.LastSave = new SaveGameData()
            {
                PlayerScore = mainGame.InGameScreen.LevelMANAGER.StartingScore,
                PlayerNumber = mainGame.PlayerNumber,
                DeathsRemaining = mainGame.GamePlayer.DeathsRemaining,
                LivesRemaining = mainGame.GamePlayer.LivesRemaining,
                ElapesedTime = new TimeSpan(mainGame.TotalTime.Hours, mainGame.TotalTime.Minutes, mainGame.TotalTime.Seconds),
                LevelName = mainGame.InGameScreen.LevelMANAGER.CurrentLevel,
                NewGame = false,
                PlayerName = mainGame.PlayerName
            };

            string folderPath = Application.StartupPath + "/Content/SavedGames";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string path = folderPath + "/" + mainGame.PlayerName + ".Save";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(builder);
            }
        }

        #endregion
    }
}