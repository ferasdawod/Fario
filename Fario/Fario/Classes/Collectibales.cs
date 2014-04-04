using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public enum CollectibaleType { Diamond, Star, BronzCoin, SilverCoin, GoldCoin };

    class Collectibale : StaticGameObject
    {
        private int scoreValue;

        public int ScoreValue
        {
            get { return scoreValue; }
            set { scoreValue = value; }
        }

        private CollectibaleType type;
        public CollectibaleType Type
        {
            get { return type; }
        }

        public Collectibale(
            Texture2D tileSheet,
            int indexInSheet,
            int cellX,
            int cellY,
            int scoreValue,
            NumbersHelper helper,
            CollectibaleType type)
        {
            this.scoreValue = scoreValue;

            this.worldLocation.X = TileMap.TileWidth * cellX;
            this.worldLocation.Y = TileMap.TileHeight * cellY;

            this.xStartOffset = helper.DiaCoinsXStartOffset;
            this.yStartOffset = helper.DiaCoinsYStartOffset;
            this.xEndOffset = helper.DiaCoinsXEndOffset;
            this.yEndOffset = helper.DiaCoinsYEndOffset;

            animations.Add("Idle",
                new AnimationStrip(
                    tileSheet,
                    TileMap.TileSourceRectangle(indexInSheet),
                    "Idle"));
            animations["Idle"].LoopAnimation = true;
            PlayAnimation("Idle");

            this.drawDepth = 0f;
            this.enabled = true;
            this.type = type;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
