using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public class Cloud
    {
        #region Declerations

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Color cColor { get; set; }

        public Rectangle SourceRectangle
        {
            get
            {
                return new Rectangle(
                    0,
                    0,
                    Texture.Width,
                    Texture.Height);
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Texture.Width,
                    Texture.Height);
            }
        }

        #endregion

        #region Constructor

        public Cloud(Texture2D texture, Vector2 position, Vector2 velocity,
            Color cColor)
        {
            this.Texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            this.cColor = cColor;
        }

        #endregion

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            Vector2 movment = new Vector2(Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            Position += movment;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);

            spriteBatch.Draw(Texture, Camera.WorldToScreen(Position), sourceRect, cColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }

        #endregion
    }
}
