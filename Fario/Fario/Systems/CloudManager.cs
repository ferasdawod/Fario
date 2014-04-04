using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public class CloudManager
    {
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;

        private List<Texture2D> textures;

        private int width;
        private int height;

        private int cloudCount;

        private List<Cloud> clouds;

        private FarioMain parentGame;

        public CloudManager(FarioMain mainGame)
        {
            this.parentGame = mainGame;
            clouds = new List<Cloud>();

            textures = new List<Texture2D>();
            textures.Add(mainGame.Content.Load<Texture2D>(@"Textures\Clouds\Cloud0"));
            textures.Add(mainGame.Content.Load<Texture2D>(@"Textures\Clouds\Cloud1"));
            textures.Add(mainGame.Content.Load<Texture2D>(@"Textures\Clouds\Cloud2"));

            width = Tile_Engine.TileMap.MapWidth * Tile_Engine.TileMap.TileWidth;
            height = Tile_Engine.TileMap.MapHeight * Tile_Engine.TileMap.TileHeight;

            cloudCount = TileMap.MapWidth / 3;
            for (int i = 0; i < cloudCount; i++)
            {
                GenerateCloud();
            }
        }

        private void GenerateCloud()
        {
            position = GeneratePosition();

            velocity = new Vector2(
                -(float)parentGame.Randomizer.NextDouble() * 10.0f,
                0);


            texture = textures[parentGame.Randomizer.Next(0, textures.Count)];

            clouds.Add(new Cloud(texture, position, velocity, Color.White));
        }

        private Vector2 GeneratePosition()
        {
            Vector2 newPosition = new Vector2(
                     parentGame.Randomizer.Next(0, width),
                     parentGame.Randomizer.Next(0, height));

            return newPosition;
        }

        private Vector2 Relocate()
        {
            Vector2 newPosition = new Vector2(
                width,
                parentGame.Randomizer.Next(0, height));

            return newPosition;
        }

        public void Update(GameTime gameTime)
        {
            UpdateAndReposition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cloud cloud in clouds)
            {
                if (Tile_Engine.Camera.ObjectIsVisible(cloud.WorldRectangle))
                {
                    cloud.Draw(spriteBatch);
                }
            }
        }

        void UpdateAndReposition(GameTime gameTime)
        {
            for (int i = 0; i <= clouds.Count - 1; i++)
            {
                Cloud cloud = clouds[i];
                cloud.Update(gameTime);
                if (cloud.Position.X < -cloud.Texture.Width)
                {
                    cloud.Position = Relocate();
                }
            }
        }
    }
}
