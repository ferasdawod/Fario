using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace IMPORT_PLATFORM
{
    public class GameFinished
    {
        private enum ActiveScreen { Intro = 0, HighScrores = 1, Options = 2 }

        #region Declerations

        private FarioMain mainGame;

        private StringBuilder stringBuilder = new StringBuilder();

        private ActiveScreen activeScreen = ActiveScreen.Intro;

        #endregion
        public GameFinished(FarioMain parentGame)
        {
            this.mainGame = parentGame;
        }

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            ScrollScreenInput();
            if (Input.KeyPressed(Keys.Q) || Input.KeyPressed(Keys.Escape))
            {
                mainGame.Exit();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (activeScreen)
            {
                case ActiveScreen.Intro:
                    DrawIntro(spriteBatch);
                    break;
                case ActiveScreen.HighScrores:
                    DrawHighScores(spriteBatch);
                    break;
                case ActiveScreen.Options:
                    DrawOptions(spriteBatch);
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void ScrollScreenInput()
        {
            if (Input.KeyPressed(Keys.Left))
            {
                activeScreen--;
                if ((int)activeScreen < 0)
                {
                    activeScreen = ActiveScreen.Options;
                }
            }
            if (Input.KeyPressed(Keys.Right))
            {
                if (activeScreen == ActiveScreen.Options)
                {
                    mainGame.SetState(GameState.NewGame);
                    activeScreen = ActiveScreen.Intro;
                }
                else
                {
                    activeScreen++;
                }
            }
            if (Input.KeyPressed(Keys.Enter) || Input.KeyPressed(Keys.Space))
            {
                if (activeScreen == ActiveScreen.Options)
                {
                    mainGame.SetState(GameState.NewGame);
                    activeScreen = ActiveScreen.Intro;
                }
                else
                {
                    activeScreen++;
                }
            }
        }

        private void DrawHighScores(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameFinishedSmall.Width, mainGame.GameFinishedSmall.Height), TextLocation.TopMid, mainGame.Window.ClientBounds);
            spriteBatch.Draw(mainGame.GameFinishedSmall, loc, Color.White);


            if (mainGame.HighScoreData.Count != 0)
            {
                stringBuilder.Clear();
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Player Name    Player Score    Level Name    Game Time");
                stringBuilder.AppendLine();
                foreach (HighScoreData data in mainGame.HighScoreData)
                {
                    string line = "- " + data.Name;
                    string wholeLine;
                    line = line.PadRight(35 - line.Length, ' ');
                    wholeLine = line;

                    line = data.Score.ToString();
                    line = line.PadRight(30 - line.Length, ' ');
                    wholeLine += line;

                    line = data.LevelName;
                    line = line.PadRight(20 - line.Length, ' ');
                    wholeLine += line;

                    line = data.TotalTime.Minutes.ToString() + ":" + data.TotalTime.Seconds.ToString();
                    wholeLine += line;
                    stringBuilder.AppendLine(wholeLine);
                }

                loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(stringBuilder), TextLocation.Center, mainGame.Window.ClientBounds);

                spriteBatch.DrawString(mainGame.StateFont, stringBuilder.ToString(), loc, Color.White);
            }
            else
            {
                string line = "No HighScore Data is Available Yet...";

                loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(line), TextLocation.Center, mainGame.Window.ClientBounds);
                spriteBatch.DrawString(mainGame.StateFont, line, loc, Color.White);
            }
        }

        private void DrawIntro(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameFinishedLarge.Width, mainGame.GameFinishedLarge.Height), TextLocation.Center, mainGame.Window.ClientBounds);

            spriteBatch.Draw(mainGame.GameFinishedLarge, loc, Color.White);
        }

        private void DrawOptions(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameFinishedSmall.Width, mainGame.GameFinishedSmall.Height), TextLocation.TopMid, mainGame.Window.ClientBounds);
            spriteBatch.Draw(mainGame.GameFinishedSmall, loc, Color.White);

            stringBuilder.Clear();
            stringBuilder.AppendLine("Congratulations On Finishing The Game");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Press \"Enter\" OR \"Space\" To Start A New Game");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Press \"Escape\" OR \"Q\" To Exit The Game...");

            loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(stringBuilder), TextLocation.Center, mainGame.Window.ClientBounds);
            spriteBatch.DrawString(mainGame.StateFont, stringBuilder, loc, Color.White);
        }

        #endregion
    }
}
