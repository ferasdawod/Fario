using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;
using Partical_System;

namespace IMPORT_PLATFORM
{
    public  class LevelManager
    {
        #region Declerations

        FarioMain mainGame;

        private  Player player;
        private  string currentLevel;
        private  Vector2 respawnLocation;
        private int playerStartingScore;

        #region Collectibales and traps declerations

        private List<Collectibale> collectibales = new List<Collectibale>();
        private int collectibalsScore = 500;

        private List<Spike> traps = new List<Spike>();
        private int trapScore = 250;

        private List<Spike> waters = new List<Spike>();
        private int waterScore = 1000;

        #endregion

        private  ParticalEngine particalEngine;
        private  NumbersHelper numHelper;

        #region Enemies Declerations

        private EnemiesManager enemiesManager;

        #endregion

        #endregion

        #region Properties

        public  string CurrentLevel
        {
            get { return currentLevel; }
        }

        public  Vector2 RespawnLocation
        {
            get { return respawnLocation; }
            set { respawnLocation = value; }
        }

        public int StartingScore
        {
            get { return playerStartingScore; }
        }

        #endregion

        #region Constructor and Initialization

        public  LevelManager(FarioMain mainGame)
        {
            this.mainGame = mainGame;
            player = mainGame.GamePlayer;
            numHelper = mainGame.NumHelper;
            enemiesManager = new EnemiesManager(mainGame);

            List<Texture2D> particalTextures = new List<Texture2D>();
            particalTextures.Add(mainGame.Content.Load<Texture2D>(@"Particals\partical0"));
            particalTextures.Add(mainGame.Content.Load<Texture2D>(@"Particals\partical1"));
            particalTextures.Add(mainGame.Content.Load<Texture2D>(@"Particals\partical2"));
            particalTextures.Add(mainGame.Content.Load<Texture2D>(@"Particals\partical3"));

            particalEngine = new ParticalEngine(particalTextures, 20, 1.0f, .8f);
        }

        #endregion

        #region LoadingTheLevel

        public  void LoadLevel(string mapName)
        {
            this.playerStartingScore = player.Score;

            TileMap.LoadMap(Application.StartupPath, mapName);

            FillLists();

            currentLevel = mapName;
            respawnLocation = player.WorldLocation;
        }

        private  void FillLists()
        {
            ClearLists();
            foreach (string line in TileMap.SpecialTiles)
            {
                string[] info = line.Split(',');
                int x = int.Parse(info[0]);
                int y = int.Parse(info[1]);
                string code = info[2];

                if (code != "" && code != null)
                {
                    if (code == "START")
                    {
                        player.WorldLocation = new Vector2(x * TileMap.TileWidth, y * TileMap.TileHeight);
                    }
                    if (code.StartsWith("C"))
                    {
                        AddCollectibale(code, x, y);
                    }
                    if (code.StartsWith("D"))
                    {
                        AddDeath(code, x, y);
                    }
                    if (code.StartsWith("H"))
                    {
                        AddSpikes(code, x, y);
                    }
                    if (code.StartsWith("E"))
                    {
                        enemiesManager.AddEnemy(code, x, y);
                    }
                }
            }
        }

        private  void AddCollectibale(string code, int x, int y)
        {
            if (code == "CStar")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.Star));
            }
            if (code == "CBCoin")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.BronzCoin));
            }
            if (code == "CSCoin")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.SilverCoin));
            }
            if (code == "CGCoin")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.GoldCoin));
            }
            if (code == "CBGem")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.Diamond));
            }
            if (code == "CRGem")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.Diamond));
            }
            if (code == "CYGem")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.Diamond));
            }
            if (code == "CGGem")
            {
                collectibales.Add(new Collectibale(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, collectibalsScore, numHelper, CollectibaleType.Diamond));
            }
        }

        private  void AddDeath(string code, int x, int y)
        {
            waters.Add(new Spike(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, waterScore, numHelper));
        }

        private  void AddSpikes(string code, int x, int y)
        {
            traps.Add(new Spike(TileMap.TileSheet, TileMap.GetTileIndex(code), x, y, trapScore, numHelper));
        }

        private  void ClearLists()
        {
            waters = new List<Spike>();
            collectibales = new List<Collectibale>();
            traps = new List<Spike>();
            enemiesManager.ClearEnemies();
        }

        #endregion

        public  void ReloadLevel()
        {
            Vector2 saveRespawn = respawnLocation;
            LoadLevel(currentLevel);
            respawnLocation = saveRespawn;
            player.WorldLocation = respawnLocation;
        }

        #region Update And Draw

        public  void Update(GameTime gameTime)
        {
            UpdateCollectibales(gameTime);
            UpdateTraps(gameTime);
            UpdateWaters(gameTime);

            particalEngine.Update(gameTime);
            enemiesManager.Update(gameTime);
        }

        public  void Draw(SpriteBatch spriteBatch)
        {
            particalEngine.Draw(spriteBatch);

            foreach (Collectibale co in collectibales)
                co.Draw(spriteBatch);
            foreach (Spike s in traps)
                s.Draw(spriteBatch);
            foreach (Spike ss in waters)
                ss.Draw(spriteBatch);

            enemiesManager.Draw(spriteBatch);
        }

        #endregion

        #region Updating the lists       
 
        private void UpdateCollectibales(GameTime gameTime)
        {
            if (!player.Dead)
            {
                for (int i = 0; i < collectibales.Count; i++)
                {
                    if (Camera.ObjectIsVisible(collectibales[i].WorldRectangle))
                    {
                        collectibales[i].Update(gameTime);
                        if (collectibales[i].CollisionRectangle.Intersects(player.CollisionRectangle))
                        {
                            player.Score += collectibales[i].ScoreValue;
                            particalEngine.GenerateEffect(collectibales[i].WorldCenter, Color.White);
                            collectibales.RemoveAt(i);
                            i--;
                            MusicManager.Instance.PlayEffect(SFXType.PickUp);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateTraps(GameTime gameTime)
        {
            if (!player.Dead)
            {
                for (int i = 0; i < traps.Count; i++)
                {
                    if (Camera.ObjectIsVisible(traps[i].WorldRectangle))
                    {
                        traps[i].Update(gameTime);
                        if (traps[i].CollisionRectangle.Intersects(player.CollisionRectangle))
                        {
                            player.GetHurt(traps[i].ScoreValue);
                            MusicManager.Instance.PlayEffect(SFXType.Hurt);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateWaters(GameTime gameTime)
        {
            if (!player.Dead)
            {
                for (int i = 0; i < waters.Count; i++)
                {
                    if (Camera.ObjectIsVisible(waters[i].WorldRectangle))
                    {
                        waters[i].Update(gameTime);
                        if (waters[i].CollisionRectangle.Intersects(player.CollisionRectangle))
                        {
                            player.LoseLife();
                            MusicManager.Instance.PlayEffect(SFXType.Fall);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
