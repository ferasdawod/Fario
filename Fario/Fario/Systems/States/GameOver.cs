using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IMPORT_PLATFORM
{
    public class GameOver
    {
        private enum ActiveScreen { Options = 2, HighScores = 1, OverScreen = 0 }

        #region Declerations

        FarioMain mainGame;

        private ActiveScreen activeScreen = ActiveScreen.OverScreen;

        StringBuilder stringBuilder = new StringBuilder();

        #endregion

        public GameOver(FarioMain parentGame)
        {
            this.mainGame = parentGame;
        }

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            CheckInput();
            ScreenChangeInput();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (activeScreen)
            {
                case ActiveScreen.OverScreen:
                    DrawOverScreen(spriteBatch);
                    break;
                case ActiveScreen.Options:
                    DrawOptions(spriteBatch);
                    break;
                case ActiveScreen.HighScores:
                    DrawHighScores(spriteBatch);
                    break;
            }
        }

        #endregion

        #region Helper Methods


        private void DrawOverScreen(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameOverImageLarge.Width, mainGame.GameOverImageLarge.Height), TextLocation.Center, mainGame.Window.ClientBounds);

            spriteBatch.Draw(mainGame.GameOverImageLarge, loc, Color.White);
        }

        private void DrawHighScores(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameOverImageSmall.Width, mainGame.GameOverImageSmall.Height), TextLocation.TopMid, mainGame.Window.ClientBounds);
            spriteBatch.Draw(mainGame.GameOverImageSmall, loc, Color.White);


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

        private void DrawOptions(SpriteBatch spriteBatch)
        {
            Vector2 loc = Extensions.AllignThing(new Vector2(mainGame.GameOverImageSmall.Width, mainGame.GameOverImageSmall.Height), TextLocation.TopMid, mainGame.Window.ClientBounds);
            spriteBatch.Draw(mainGame.GameOverImageSmall, loc, Color.White);

            stringBuilder.Clear();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("- Press \"L\" To Load The Last Save You Made During Play...");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("- Press \"Enter\" OR \"Space\" To Start A New Game...");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("- Press \"Escape\" OR \"Q\" To Exit The Game...");

            loc = Extensions.AllignThing(mainGame.StateFont.MeasureString(stringBuilder), TextLocation.Center, mainGame.Window.ClientBounds);
            spriteBatch.DrawString(mainGame.StateFont, stringBuilder, loc, Color.White);
        }

        private void CheckInput()
        {
            if (Input.KeyPressed(Keys.Escape) || Input.KeyPressed(Keys.Q))
            {
                mainGame.Exit();
            }
            if (Input.KeyPressed(Keys.L))
            {
                mainGame.LoadLastSave();
            }
        }

        private void ScreenChangeInput()
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
                    activeScreen = ActiveScreen.OverScreen;
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
                    activeScreen = ActiveScreen.OverScreen;
                }
                else
                {
                    activeScreen++;
                }
            }
        }

        #endregion
    }
}
