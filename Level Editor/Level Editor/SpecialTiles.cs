using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace Level_Editor
{
    public class SpecialTile
    {
        int tileIndex;
        Vector2 position;

        public SpecialTile(int indexInSheet, Vector2 position)
        {
            this.tileIndex = indexInSheet;
            this.position.X = ((int)position.X / TileMap.TileWidth) * TileMap.TileWidth;
            this.position.Y = ((int)position.Y / TileMap.TileHeight) * TileMap.TileHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TileMap.TileSheet, Camera.WorldToScreen(position), TileMap.TileSourceRectangle(tileIndex), Color.White);
        }
    }
}
