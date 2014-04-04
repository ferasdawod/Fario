using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tile_Engine
{
    public class TilesetData
    {
        private Dictionary<int, string> indexToName = new Dictionary<int, string>();
        private Dictionary<string, int> nameToIndex = new Dictionary<string, int>();

        /// <summary>
        /// Initialize The TileSet Data
        /// </summary>
        public TilesetData()
        {
            indexToName.Add(2, "DIceTop");
            indexToName.Add(3, "DSeeBot1");
            indexToName.Add(4, "DIceBot");
            indexToName.Add(5, "DIceBot1");
            indexToName.Add(8, "DLavaTop");
            indexToName.Add(6, "DLavaBot");
            indexToName.Add(11, "DSeaTop");
            indexToName.Add(9, "DSeaBot");

            indexToName.Add(18, "Ladder");
            indexToName.Add(19, "LadderT");

            indexToName.Add(52, "CStar");
            indexToName.Add(53, "CBCoin");
            indexToName.Add(55, "CSCoin");
            indexToName.Add(54, "CGCoin");
            indexToName.Add(56, "CBGem");
            indexToName.Add(57, "CGGem");
            indexToName.Add(58, "CRGem");
            indexToName.Add(59, "CYGem");

            indexToName.Add(73, "HDSpikeB");
            indexToName.Add(77, "HDSpikeB1");
            indexToName.Add(78, "HDSpikeT1");
            indexToName.Add(79, "HDSpikeT");
            indexToName.Add(84, "HSSpikeB");
            indexToName.Add(90, "HSSpikeT");
            indexToName.Add(88, "HSSpikeB1");
            indexToName.Add(89, "HSSpikeT1");
            indexToName.Add(94, "HISpikeB");
            indexToName.Add(83, "HISpikeB1");
            indexToName.Add(96, "HISpikeB2");
            indexToName.Add(97, "HISpikeT");
            indexToName.Add(98, "HISpikeT1");
            indexToName.Add(99, "HISpikeT2");
            indexToName.Add(100, "HISpikeT3");
            indexToName.Add(101, "HSpike");

            indexToName.Add(643, "EBarnacle");
            indexToName.Add(644, "ELadyBug");
            indexToName.Add(645, "EMouse");
            indexToName.Add(646, "EPirana");
            indexToName.Add(647, "EPSlime");
            indexToName.Add(649, "EBSlime");
            indexToName.Add(650, "EGSlime");
            indexToName.Add(651, "ESnail");
            indexToName.Add(653, "ELSnake");
            indexToName.Add(652, "ESnake");
            indexToName.Add(648, "ESlimeBlk");
            indexToName.Add(654, "EBat");
            indexToName.Add(655, "EGSnake");
            indexToName.Add(656, "ESpider");
            indexToName.Add(657, "ESpinner");
            indexToName.Add(658, "EHSpinner");
            indexToName.Add(659, "EWorm");
            indexToName.Add(660, "EBee");
            indexToName.Add(661, "EGFish");
            indexToName.Add(662, "EPFish");
            indexToName.Add(663, "EFly");
            indexToName.Add(664, "EFrog");
            indexToName.Add(665, "EGhost");
            indexToName.Add(666, "EBlock");

            foreach (KeyValuePair<int, string> pair in indexToName)
            {
                nameToIndex.Add(pair.Value, pair.Key);
            }
        }

        /// <summary>
        /// Returns True if the given index is listed as special
        /// </summary>
        /// <param name="index">The index of the tile in question</param>
        /// <returns></returns>
        public bool IsTileSpecial(int index)
        {
            return indexToName.ContainsKey(index);
        }

        /// <summary>
        /// Returns True if the given tile name is listed as special
        /// </summary>
        /// <param name="name">The Name of the tile in question</param>
        /// <returns></returns>
        public bool IsTileSpecial(string name)
        {
            return nameToIndex.ContainsKey(name);
        }

        public int GetTileIndex(string tileName)
        {
            if (tileName == "")
                return -1;

            return nameToIndex[tileName];
        }

        public string GetTileName(int index)
        {
            if (index == -1)
                return "";

            return indexToName[index];
        }
    }
}
