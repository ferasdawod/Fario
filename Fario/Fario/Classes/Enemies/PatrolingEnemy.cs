using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IMPORT_PLATFORM
{
    public class PatrolingEnemy : MovingGameObject
    {
        #region Declerations

        protected bool dead;
        public bool Dead
        {
            get { return dead; }
        }

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        protected int scoreValue;
        public int ScoreValue
        {
            get { return scoreValue; }
        }

        /// <summary>
        /// The Snakes Max Speed
        /// </summary>
        protected Vector2 maxSbd;
        /// <summary>
        /// The Snake Accleration
        /// </summary>
        protected float acc;
        /// <summary>
        /// The Snake Deaccleration
        /// </summary>
        protected float dec;
        /// <summary>
        /// Should The Snake Use Gravity
        /// </summary>
        protected bool hasGravity;
        /// <summary>
        /// Should We Check For Collisions With The Map
        /// </summary>
        protected bool checkCollisions;

        protected int moveDirection = -1;
        
        #endregion        

        public override void Update(GameTime gameTime)
        {
            this.beingMoved = true;
            Patrol(gameTime);
            base.Update(gameTime);
            if (worldLocation.X == 0 || worldLocation.X == (Tile_Engine.Camera.WorldRectangle.Width - FrameWidth))
            {
                moveDirection *= -1;
            }
        }

        private void Patrol(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (moveDirection < 0)
            {
                speed.X -= accelerationX * deltaTime;
                flipped = false;
                Vector2 loc = new Vector2(CollisionRectangle.Left - 1, CollisionRectangle.Top);
                Vector2 cell = Tile_Engine.TileMap.GetCellByPixel(loc);
                if (!Tile_Engine.TileMap.CellIsPassable(cell))
                {
                    speed.X = 0;
                    moveDirection *= -1;
                }
                loc = new Vector2(CollisionRectangle.Left - 1, CollisionRectangle.Bottom + 10);
                cell = Tile_Engine.TileMap.GetCellByPixel(loc);
                if (Tile_Engine.TileMap.CellIsPassable(cell))
                {
                    speed.X = 0;
                    moveDirection *= -1;
                }
            }

            if (moveDirection > 0)
            {
                speed.X += accelerationX * deltaTime;
                flipped = true;
                Vector2 loc = new Vector2(CollisionRectangle.Right + 1, CollisionRectangle.Top);
                Vector2 cell = Tile_Engine.TileMap.GetCellByPixel(loc);
                if (!Tile_Engine.TileMap.CellIsPassable(cell))
                {
                    speed.X = 0;
                    moveDirection *= -1;
                }
                loc = new Vector2(CollisionRectangle.Right + 1, CollisionRectangle.Bottom + 10);
                cell = Tile_Engine.TileMap.GetCellByPixel(loc);
                if (Tile_Engine.TileMap.CellIsPassable(cell))
                {
                    speed.X = 0;
                    moveDirection *= -1;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
