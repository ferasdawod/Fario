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
    }
}
