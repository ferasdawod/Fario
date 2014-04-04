using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public  class EnemiesManager
    {
        #region Declerations

        private  FarioMain mainGame;

        private static Texture2D tilesheet;

        private  List<PatrolingEnemy> enemies = new List<PatrolingEnemy>();
        
        #endregion

        #region Initialization

        public  EnemiesManager(FarioMain parentGame)
        {
            mainGame = parentGame;

            tilesheet = mainGame.Content.Load<Texture2D>(@"Textures\Enemies");
        }

        public static Texture2D EnemiesSheet
        {
            get { return tilesheet; }
        }

        #endregion

        #region Update And Draw

        public  void Update(GameTime gameTime)
        {
            UpdateEnemies(gameTime);
        }

        public  void Draw(SpriteBatch spriteBatch)
        {
            DrawEnemies(spriteBatch);
        }

        #endregion

        #region Helper Methods

        public  void AddEnemy(string code, int x, int y)
        {
            Vector2 loc = new Vector2(x * TileMap.TileWidth, y * TileMap.TileHeight);
            switch (code)
            {
                case "ESpider":
                    enemies.Add(new Spider(mainGame.Content,loc));
                    break;
                case "ESnake":
                    enemies.Add(new Snake(mainGame.Content, loc));
                    break;
                case "ESnail" :
                    enemies.Add(new Snail(mainGame.Content, loc));
                    break;
                case "EWorm" :
                    enemies.Add(new Worm(mainGame.Content, loc));
                    break;
                case "EGhost" :
                    enemies.Add(new Ghost(mainGame.Content, loc));
                    break;
                case "EBarnacle" :
                    enemies.Add(new Barnacle(mainGame.Content, loc));
                    break;
                case "EMouse" :
                    enemies.Add(new Mouse(mainGame.Content, loc));
                    break;
                case "ELadyBug" :
                    enemies.Add(new LadyBug(mainGame.Content, loc));
                    break;
                case "ESpinner" :
                    enemies.Add(new Spinner(mainGame.Content, loc));
                    break;
                case "EPSlime" :
                    enemies.Add(new Slime(mainGame.Content, loc, Slime.SlimeColor.Pink));
                    break;
                case "EBSlime" :
                    enemies.Add(new Slime(mainGame.Content, loc, Slime.SlimeColor.Blue));
                    break;
                case "EGSlime" :
                    enemies.Add(new Slime(mainGame.Content, loc, Slime.SlimeColor.Green));
                    break;
                case "EHSpinner" :
                    enemies.Add(new HalfSpinner(mainGame.Content, loc));
                    break;
                default :
                    break;
            }
        }

        #region Updating Enemies

        private  void UpdateEnemies(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                PatrolingEnemy enemy = enemies[i];
                enemy.Update(gameTime);
                if (enemy.CollisionRectangle.Intersects(mainGame.GamePlayer.CollisionRectangle))
                {
                    mainGame.GamePlayer.GetHurt(enemy.ScoreValue);
                    MusicManager.Instance.PlayEffect(SFXType.Hurt);
                }
            }
        }

        #endregion

        private  void DrawEnemies(SpriteBatch sprietBatch)
        {
            foreach (PatrolingEnemy enemy in enemies)
                enemy.Draw(sprietBatch);
        }

        public  void ClearEnemies()
        {
            enemies = new List<PatrolingEnemy>();
        }

        #endregion
    }
}
