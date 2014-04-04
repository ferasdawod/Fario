using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public class Spike : StaticGameObject
    {
        private int scoreValue;

        public int ScoreValue
        {
            get { return scoreValue; }
            set { scoreValue = value; }
        }

        private Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Spike(
            Texture2D tileSheet,
            int indexInSheet,
            int cellX,
            int cellY,
            int scoreValue,
            NumbersHelper helper)
        {
            this.scoreValue = scoreValue;
            this.texture = tileSheet;

            this.worldLocation.X = TileMap.TileWidth * cellX;
            this.worldLocation.Y = TileMap.TileHeight * cellY;

            this.xStartOffset = helper.TrapXStartOffset;
            this.yStartOffset = helper.TrapYStartOffset;
            this.xEndOffset = helper.TrapXEndOffset;
            this.yEndOffset = helper.TrapYEndOffset;

            animations.Add("Idle",
                new AnimationStrip(
                    tileSheet,
                    TileMap.TileSourceRectangle(indexInSheet),
                    "Idle"));
            animations["Idle"].LoopAnimation = true;
            PlayAnimation("Idle");

            this.drawDepth = 0f;
            this.enabled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (!enabled)
            {
                base.Update(gameTime);
            }
        }
    }
}
