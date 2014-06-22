using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace IMPORT_PLATFORM
{
    public class NewGame
    {
        private enum CurrentScreen { Logo = 0, Tips = 1 };
        private CurrentScreen screen = CurrentScreen.Logo;

        private FarioMain mainGame;

        private bool firstRun;

        private StringBuilder tips = new StringBuilder();

        private Player walkingPlayer0;
        private Player walkingPlayer1;

        public NewGame(FarioMain parentGame)
        {
            this.mainGame = parentGame;

            firstRun = true;

            InitializePlayers();
        }

        public void InitializePlayers()
        {
            walkingPlayer0 = new Player(mainGame, 3, mainGame.NumHelper);
            walkingPlayer0.WorldLocation = new Vector2(0, mainGame.Randomizer.Next(mainGame.Window.ClientBounds.Height));
            walkingPlayer0.HasCollisions = false;
            walkingPlayer0.HasGravity = false;
            walkingPlayer0.HasInput = false;
            walkingPlayer0.PlayAnimation("Walk");
            walkingPlayer0.Flipped = false;
            walkingPlayer0.Dead = false;
            walkingPlayer0.Enabled = true;

            walkingPlayer1 = new Player(mainGame, 0, mainGame.NumHelper);
            walkingPlayer1.WorldLocation = new Vector2(
                (2 * mainGame.Window.ClientBounds.Width) / 3,
                mainGame.Randomizer.Next(mainGame.Window.ClientBounds.Height));
            walkingPlayer1.HasCollisions = false;
            walkingPlayer1.HasGravity = false;
            walkingPlayer1.HasInput = false;
            walkingPlayer1.PlayAnimation("Walk");
            walkingPlayer1.Flipped = false;
            walkingPlayer1.Dead = false;
            walkingPlayer1.Enabled = true;
        }

        public void Update(GameTime gameTime)
        {           
            UpdatePlayers(gameTime);

            if (screen == CurrentScreen.Logo)
            {
                UpdateLogo(gameTime);
            }
            else
            {
                UpdateTips(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            walkingPlayer0.Draw(spriteBatch);
            walkingPlayer1.Draw(spriteBatch);

            if (screen == CurrentScreen.Logo)
            {
                spriteBatch.DrawString(
                    mainGame.LargeStateFont,
                    "Welcome",
                    Extensions.AllignThing(mainGame.LargeStateFont.MeasureString("Welcome"), TextLocation.TopMid, mainGame.Window.ClientBounds),
                    Color.White);
                Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.LargeGameLogo.Width, mainGame.LargeGameLogo.Height), TextLocation.Center, mainGame.Window.ClientBounds);
                spriteBatch.Draw(mainGame.LargeGameLogo, loc, Color.White);
            }
            else
            {
                DrawTips(spriteBatch);
            }
        }

        #region Helper Methods

        private void UpdateLogo(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
            {
                screen = CurrentScreen.Tips;
            }
        }

        private void UpdateTips(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
            {
                screen = CurrentScreen.Logo;
                Start();
            }
        }

        private void UpdatePlayers(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float moveAmount = walkingPlayer0.AcclerationX * elapsed * 10;

            walkingPlayer0.Speed = new Vector2(walkingPlayer0.Speed.X + moveAmount, walkingPlayer0.Speed.Y);
            walkingPlayer1.Speed = new Vector2(walkingPlayer1.Speed.X + moveAmount, walkingPlayer1.Speed.Y);

            walkingPlayer0.Update(gameTime);
            walkingPlayer1.Update(gameTime);

            if (walkingPlayer0.WorldLocation.X >= (mainGame.Window.ClientBounds.Width))
            {
                walkingPlayer0.WorldLocation = new Vector2(-walkingPlayer0.FrameWidth, mainGame.Randomizer.Next(mainGame.Window.ClientBounds.Height - walkingPlayer0.FrameHeight));
            }
            if (walkingPlayer1.WorldLocation.X >= (mainGame.Window.ClientBounds.Width))
            {
                walkingPlayer1.WorldLocation = new Vector2(-walkingPlayer1.FrameWidth, mainGame.Randomizer.Next(mainGame.Window.ClientBounds.Height - walkingPlayer1.FrameHeight));
            }
        }

        private void DrawTips(SpriteBatch spriteBatch)
        {
            tips.Clear();
            tips.AppendLine("A Few Quick Tips :");
            tips.AppendLine();
            tips.AppendLine("1) You Can Press \"Space\" To Jump");
            tips.AppendLine("2)You Can Press The Down Butten To Quickly Go Down After A Jump");
            tips.AppendLine("3) Make Sure To Explore The Whole Level To Collect All The Coins");
            tips.AppendLine("4) You Can Take 3 Hits Before You Lose A Life");
            tips.AppendLine("5) If You Touch Any Trap Or Enemy You Will Take Damage");
            tips.AppendLine("6) You Cant Kill Your Enemies So Dont Try");
            tips.AppendLine("7) If You Lose All Your Lives You Will Lose The Game");
            tips.AppendLine("8) You Can Pause The Game And Press \"S\" To Save The Game");
            tips.AppendLine("9) After You Save The Game You Can Press \"L\" To Load The Last Save");
            tips.AppendLine("10) You Can Change The Sound Options From The Pause Menue");
            tips.AppendLine("11) You Can View The High Scores From The Pause Menue");
            tips.AppendLine("12) When You Pause The Game You Can Press \"Q\" To Exit The Game");

            Vector2 textSize = mainGame.LargeStateFont.MeasureString(tips);
            Vector2 loc = Extensions.AllignThing(textSize, TextLocation.Center, mainGame.Window.ClientBounds);
            spriteBatch.DrawString(mainGame.LargeStateFont, tips.ToString(), loc, Color.White);
        }

        private void Start()
        {
            if (firstRun)
            {
                if (mainGame.StartingData.NewGame == true)
                {
                    MusicManager.Instance.StartLoop();
                    firstRun = false;
                    StartNewGame();
                }
                else
                {
                    MusicManager.Instance.StartLoop();
                    firstRun = false;
                    LoadGame(mainGame.StartingData);
                }
            }
            else
            {
                MusicManager.Instance.StartLoop();

                StartNewGame();
            }
        }

        #endregion

        #region Starting and loading the Game
        private void StartNewGame()
        {
            SaveGameData data = new SaveGameData
            {
                DeathsRemaining = 3,
                LivesRemaining = 3,
                PlayerScore = 0,
                ElapesedTime = new TimeSpan(),
                LevelName = mainGame.NewGameMap,
                NewGame = true,
                PlayerName = mainGame.StartingData.PlayerName,
                PlayerNumber = mainGame.StartingData.PlayerNumber
            };

            StartNewGame(data);
        }
        private void StartNewGame(SaveGameData data)
        {
            mainGame.PlayerName = data.PlayerName;
            mainGame.PlayerNumber = data.PlayerNumber;

            mainGame.GamePlayer.Reset();
            mainGame.InGameScreen.LevelMANAGER.LoadLevel(mainGame.NewGameMap);
            mainGame.StartingTime = new TimeSpan();

            mainGame.Timer.Reset();
            if (!mainGame.Timer.IsRunning)
                mainGame.Timer.Start();

            mainGame.InGameScreen.InGameState = InGameState.Playing;
            mainGame.SetState(GameState.InGame);
        }

        public void LoadGame(SaveGameData data)
        {
            mainGame.PlayerNumber = data.PlayerNumber;
            mainGame.PlayerName = data.PlayerName;
            mainGame.GamePlayer.Score = data.PlayerScore;
            mainGame.GamePlayer.LivesRemaining = data.LivesRemaining;
            mainGame.GamePlayer.DeathsRemaining = data.DeathsRemaining;
            mainGame.StartingTime = new TimeSpan(data.ElapesedTime.Hours, data.ElapesedTime.Minutes, data.ElapesedTime.Seconds);

            mainGame.Timer.Reset();
            if (!mainGame.Timer.IsRunning)
                mainGame.Timer.Start();

            mainGame.InGameScreen.LevelMANAGER.LoadLevel(data.LevelName);
            mainGame.SetState(GameState.InGame);
        }

        #endregion
    }
}
