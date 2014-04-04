using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace Partical_System
{
    public class Partical
    {
        #region Declerations

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color cColor { get; set; }
        public float Size { get; set; }
        public float TTL { get; set; }

        #endregion

        #region Constructor

        public Partical(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color cColor, float size, float ttl)
        {
            this.Texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.cColor = cColor;
            this.Size = size;
            this.TTL = ttl;
        }

        #endregion

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            TTL -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movment = new Vector2(Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Position += movment;
            Angle += AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Camera.WorldToScreen(Position), sourceRect, cColor, Angle, origin, Size, SpriteEffects.None, 1f);
        }

        #endregion
    }
}
