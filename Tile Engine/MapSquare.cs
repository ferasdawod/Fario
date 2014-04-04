using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tile_Engine
{
    [Serializable]
    public class MapSquare
    {
        //declerations
        public int[] LayerTiles = new int[3];
        public String CodeValue = "";
        public bool Passable = true;

        /// <summary>
        /// Creats a new map squar
        /// </summary>
        /// <param name="background">the tile index of the background tile</param>
        /// <param name="interactive">the tile index</param>
        /// <param name="foreground"></param>
        /// <param name="codeValue"></param>
        /// <param name="passable"></param>
        public MapSquare(
            int background,
            int interactive,
            int foreground,
            string codeValue,
            bool passable)
        {
            LayerTiles[0] = background;
            LayerTiles[1] = interactive;
            LayerTiles[2] = foreground;
            CodeValue = codeValue;
            Passable = passable;
        }

        public void TogglePassable()
        {
            Passable = !Passable;
        }
    }
}
