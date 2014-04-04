using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public class GameObject
    {
        protected Vector2 worldLocation;

        protected bool enabled;
        protected bool flipped = false;
        protected bool onGround;

        protected float drawDepth = 0.85f;

        protected string currentAnimation;
        protected Dictionary<string, AnimationStrip> animations =
            new Dictionary<string, AnimationStrip>();

        protected int xStartOffset = 0;
        protected int yStartOffset = 0;

        protected int xEndOffset = 0;
        protected int yEndOffset = 0;

        #region Properties

        public Dictionary<String, AnimationStrip> Animations
        {
            get { return animations; }
        }

        public bool Flipped
        {
            get { return flipped; }
            set { flipped = value; }
        }

        public bool OnGround
        {
            get { return onGround; }
        }

        public int XStartOffset
        {
            get { return xStartOffset; }
            set { xStartOffset = value; }
        }

        public int YStartOffset
        {
            get { return yStartOffset; }
            set { yStartOffset = value; }
        }

        public int XEndOffset
        {
            get { return xEndOffset; }
            set { xEndOffset = value; }
        }

        public int YEndOffset
        {
            get { return yEndOffset; }
            set { yEndOffset = value; }
        }

        public int FrameHeight
        {
            get
            {
                return animations[currentAnimation].FrameHeight;
            }
        }

        public int FrameWidth
        {
            get
            {
                return animations[currentAnimation].FrameWidth;
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2(
                    (int)worldLocation.X + (int)(FrameWidth / 2),
                    (int)worldLocation.Y + (int)(FrameHeight / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                    (int)worldLocation.X,
                    (int)worldLocation.Y,
                    FrameWidth,
                    FrameHeight);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)WorldLocation.X + xStartOffset,
                    (int)WorldLocation.Y + yStartOffset,
                    animations[currentAnimation].FrameWidth - xEndOffset,
                    animations[currentAnimation].FrameHeight - yEndOffset);
            }
        }

        #endregion

        private void updateAnimation(GameTime gameTime)
        {
            if (animations.ContainsKey(currentAnimation))
            {
                if (animations[currentAnimation].FinishedPlaying)
                {
                    PlayAnimation(animations[currentAnimation].NextAnimation);

                }
                else
                {
                    animations[currentAnimation].Update(gameTime);
                }
            }
        }

        public void PlayAnimation(string name)
        {
            if (!(name == null) && animations.ContainsKey(name))
            {
                currentAnimation = name;
                animations[name].Play();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            updateAnimation(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!enabled)
                return;
            if (!Camera.ObjectIsVisible(WorldRectangle))
                return;

            if (animations.ContainsKey(currentAnimation))
            {
                SpriteEffects effect = SpriteEffects.None;

                if (flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                spriteBatch.Draw(
                    animations[currentAnimation].Texture,
                    Camera.WorldToScreen(WorldRectangle),
                    animations[currentAnimation].FrameRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    effect,
                    drawDepth);
            }

            if (TileMap.EditorMode)
            {
                spriteBatch.Draw(
                    TileMap.TileSheet,
                    Camera.WorldToScreen(CollisionRectangle),
                    TileMap.TileSourceRectangle(TileMap.SkyTileIndex),
                    new Color(0, 100, 0, 80),
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1f);
            }
        }
    }
}