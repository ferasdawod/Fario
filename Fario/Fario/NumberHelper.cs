using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IMPORT_PLATFORM
{
    public  class NumbersHelper
    {
        #region Collision Rectangles Offsets

        //Players Offsets
        public  int[] PlayersXStartOffset = { 10, 10, 11, 10, 10 };
        public  int[] PlayersYStartOffset = { 0, 0, 0, 0, 0 };

        public  int[] PlayersXEndOffset = { 20, 20, 20, 20, 20 };
        public  int[] PlayersYEndOffset = { 0, 0, 0, 0, 0 };

        public  float PlayerDamageDelay = 250;

        //Collectibals Offsets
        public  int DiaCoinsXStartOffset = 17;
        public  int DiaCoinsYStartOffset = 17;
        public  int DiaCoinsXEndOffset = 34;
        public  int DiaCoinsYEndOffset = 34;

        //Traps Offsets
        public  int TrapXStartOffset = 0;
        public  int TrapYStartOffset = 36;
        public  int TrapXEndOffset = 0;
        public  int TrapYEndOffset = 36;

        #endregion        

        #region Hud Sheet Data

        public enum HudItem
        {
            Player0 = 0,
            Player1 = 1,
            Player2 = 2,
            Player3 = 3,
            Player4 = 4,
            EmptyHeart = 5,
            FullHeart = 6,
            Diamond = 7,
            Coin = 8
        };

        private Dictionary<HudItem, Rectangle> SourceRectangles = new Dictionary<HudItem, Rectangle>();

        #endregion

        public NumbersHelper()
        {
            SourceRectangles.Add(HudItem.Player0, new Rectangle(1, 47, 47, 47));
            SourceRectangles.Add(HudItem.Player1, new Rectangle(49, 47, 47, 47));
            SourceRectangles.Add(HudItem.Player2, new Rectangle(97, 47, 47, 47));
            SourceRectangles.Add(HudItem.Player3, new Rectangle(145, 49, 47, 47));
            SourceRectangles.Add(HudItem.Player4, new Rectangle(193, 49, 47, 47));

            SourceRectangles.Add(HudItem.Coin, new Rectangle(163, 1, 47, 47));

            SourceRectangles.Add(HudItem.Diamond, new Rectangle(136, 138, 46, 36));

            SourceRectangles.Add(HudItem.FullHeart, new Rectangle(55, 1, 53, 45));
            SourceRectangles.Add(HudItem.EmptyHeart, new Rectangle(1, 1, 53, 45));
        }

        public Rectangle GetHudSourceRect(HudItem itemType)
        {
            return SourceRectangles[itemType];
        }
    }
}
