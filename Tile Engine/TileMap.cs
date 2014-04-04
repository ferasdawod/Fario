using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using C3.XNA;

namespace Tile_Engine
{
    public enum MapType { Grass = 0, Candy = 1, Chocolat = 2, Castle = 3, StoneBrown = 4, Vanilla = 5, StoneWhite = 6, StonePurpel = 7, Ice = 8 };
    public static class TileMap
    {
        //Declerations
        public static short TileWidth = 70;
        public static int TileHeight = 70;
        public static int MapWidth = 500;
        public static int MapHeight = 500;
        public static int MapLayers = 3;
        private static int skyTile = 0;
        private static int emptyTile = 1;

        static public MapSquare[,] mapCells = new MapSquare[MapWidth, MapHeight];

        public static bool EditorMode = false;

        public static SpriteFont spriteFont;
        static private Texture2D tileSheet;

        private static TilesetData tileData;

        private static List<string> specialTiles = new List<string>();

        public static MapType MapType { get; set; }

        #region Properties

        public static int TilesPerRow
        {
            get { return tileSheet.Width / TileWidth; }
        }

        public static MapSquare[,] GetMapSquares
        {
            get { return mapCells; }
        }

        public static void ResetMapSquares()
        {
            mapCells = new MapSquare[MapWidth, MapHeight];
        }

        public static int TilesPerCol
        {
            get { return tileSheet.Height / TileHeight; }
        }

        public static int SkyTileIndex
        {
            get { return skyTile; }
            set { skyTile = value; }
        }

        public static Texture2D TileSheet
        {
            get { return tileSheet; }
            set { tileSheet = value; }
        }

        public static List<string> SpecialTiles
        {
            get { return specialTiles; }
        }

        #endregion

        static public void Initialize(Texture2D tileTexture)
        {
            tileSheet = tileTexture;
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        mapCells[x, y] = new MapSquare(emptyTile, emptyTile, emptyTile, "", true);
                    }
                }
            }
            tileData = new TilesetData();
        }

        #region Tiles Helper Methods

        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                ((tileIndex % TilesPerRow) * TileWidth),
                ((tileIndex / TilesPerRow) * TileHeight),
                TileWidth,
                TileHeight);
        }

        static public int GetCellByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        static public int GetCellByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        static public Vector2 GetCellByPixel(Vector2 pixelLocation)
        {
            return new Vector2(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y)
                );
        }

        static public Vector2 GetCellCenter(int cellX, int cellY)
        {
            return new Vector2(
                (cellX * TileWidth) + (TileWidth / 2),
                (cellY * TileHeight) + (TileHeight / 2)
                );
        }

        static public Vector2 GetCellCenter(Vector2 cell)
        {
            return GetCellCenter((int)cell.X, (int)cell.Y);
        }

        static public Rectangle CellWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(
                cellX * TileWidth,
                cellY * TileHeight,
                TileWidth,
                TileHeight);
        }

        static public Rectangle CellWorldRectangle(Vector2 cell)
        {
            return CellWorldRectangle(
                (int)cell.X,
                (int)cell.Y);
        }

        static public Rectangle CellScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldRectangle(cellX, cellY));
        }

        static public Rectangle CellScreenRectangle(Vector2 cell)
        {
            return CellScreenRectangle((int)cell.X, (int)cell.Y);
        }

        static public bool CellIsPassable(int cellX, int cellY)
        {
            MapSquare squar = GetMapSquarAtCell(cellX, cellY);

            if (squar == null)
                return true;
            else
                return squar.Passable;
        }

        static public bool CellIsPassable(Vector2 cell)
        {
            return CellIsPassable((int)cell.X, (int)cell.Y);
        }

        static public bool CellIsPassableByPixel(Vector2 pixelLocation)
        {
            return CellIsPassable(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y));
        }

        static public string CellCodeValue(int cellX, int cellY)
        {
            MapSquare squar = GetMapSquarAtCell(cellX, cellY);
            if (squar == null)
                return "";
            else
                return squar.CodeValue;
        }

        static public string CellCodeValue(Vector2 cell)
        {
            return CellCodeValue((int)cell.X, (int)cell.Y);
        }


        static public MapSquare GetMapSquarAtCell(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                return mapCells[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        static public void SetMapSquarAtCell(
            int tileX, int tileY,
            MapSquare tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY] = tile;
            }
        }

        static public void SetTileAtCell(
            int tileX, int tileY,
            int layer,
            int tileIndex)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY].LayerTiles[layer] = (short)tileIndex;
            }
        }

        static public MapSquare GetMapSquarAtPixel(int pixelX, int pixelY)
        {
            return GetMapSquarAtCell(
                GetCellByPixelX(pixelX),
                GetCellByPixelY(pixelY));
        }

        static public MapSquare GetMapSquarAtPixel(Vector2 pixelLocation)
        {
            return GetMapSquarAtPixel(
                (int)pixelLocation.X,
                (int)pixelLocation.Y);
        }

        public static int GetTileIndex(string tileName)
        {
            return tileData.GetTileIndex(tileName);
        }

        public static string GetTileName(int index)
        {
            return tileData.GetTileName(index);
        }

        public static bool IsTileSpecial(int index)
        {
            return tileData.IsTileSpecial(index);
        }

        public static bool IsTileSpecial(string name)
        {
            return tileData.IsTileSpecial(name);
        }

        #endregion

        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetCellByPixelX((int)Camera.Position.X);
            int endX = GetCellByPixelX((int)Camera.Position.X + Camera.ViewPortWidth);

            int startY = GetCellByPixelY((int)Camera.Position.Y);
            int endY = GetCellByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);

            /*int startX = 0;
            int endX = MapWidth * TileWidth;

            int startY = 0;
            int endY = MapHeight * TileHeight;*/

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            spriteBatch.Draw(
                                tileSheet,
                                CellScreenRectangle(x, y),
                                TileSourceRectangle(
                                    mapCells[x, y].LayerTiles[z]),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                1f - ((float)z * 0.1f));
                        }
                    }
                    if (EditorMode)
                    {
                        DrawEditModeItems(spriteBatch, x, y);
                    }
                }
            }
            if (EditorMode)
            {
                DrawGrid(spriteBatch);
            }
        }

        public static void DrawEditModeItems(
            SpriteBatch spriteBatch,
            int x,
            int y)
        {
            if ((x < 0) || (x >= MapWidth) ||
                (y < 0) || (y >= MapHeight))
                return;
            if (!CellIsPassable(x, y))
            {
                spriteBatch.Draw(
                    tileSheet,
                    CellScreenRectangle(x, y),
                    TileSourceRectangle(skyTile),
                    new Color(100, 0, 0, 80),
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0.0f);
            }
            if (mapCells[x, y].CodeValue != "")
            {
                Rectangle screenRect = CellScreenRectangle(x, y);
                spriteBatch.DrawString(
                    spriteFont,
                    mapCells[x, y].CodeValue,
                    new Vector2(screenRect.X, screenRect.Y),
                    Color.Black,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        private static void DrawGrid(SpriteBatch spriteBatch)
        {          
            for (int i = 0; i <= TileMap.MapWidth; i++)
            {
                Vector2 pos1 = new Vector2(i * TileWidth, 0);
                Vector2 pos2 = new Vector2(i * TileWidth, MapHeight * TileHeight);
                spriteBatch.DrawLine(Camera.WorldToScreen(pos1), Camera.WorldToScreen(pos2), Color.Black);
            }
            for (int j = 0; j <= TileMap.MapHeight; j++)
            {
                Vector2 pos1 = new Vector2(0, j * TileHeight);
                Vector2 pos2 = new Vector2(MapWidth * TileWidth, j * TileHeight);
                spriteBatch.DrawLine(Camera.WorldToScreen(pos1), Camera.WorldToScreen(pos2), Color.Black);
            }
        }

        #region LoadingAndSaving
        
        public static void SaveMap(string startpath, string mapName)
        {
            string folderPath = startpath + "/Content/Maps";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            StringBuilder builder = new StringBuilder();
            builder.Clear();
            builder.AppendLine(MapWidth.ToString());
            builder.AppendLine(MapHeight.ToString());

            for (int l = 0; l < MapLayers; l++)
            {
                for (int i = 0; i < MapWidth; i++)
                {
                    for (int j = 0; j < MapHeight; j++)
                    {
                        builder.Append(mapCells[i,j].LayerTiles[l]);
                        builder.Append(",");
                    }
                    builder.AppendLine();
                }
                builder.AppendLine();
                builder.AppendLine();
            }
            int maptype = (int)MapType;
            builder.AppendLine(maptype.ToString());
            //Collision Info
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    MapSquare squar = mapCells[i, j];
                    if (squar.Passable == false)
                    {
                        builder.AppendLine(i + "," + j);
                    }
                }
            }
            builder.AppendLine();
            //Code Values
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    MapSquare squar = mapCells[i, j];
                    if (squar.CodeValue != "")
                    {
                        builder.AppendLine(i + "," + j + "," + squar.CodeValue);
                    }
                }
            }
            string path = startpath + "/Content/Maps/" + mapName + ".FarioMap";
            string map = builder.ToString();
            string zipped = Compress(map);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(zipped);
            }
        }

        private static string Compress(string map)
        {
            var bytes = Encoding.Unicode.GetBytes(map);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());            
            }
        }

        private static string DeCompress(string compressedMap)
        {
            var bytes = Convert.FromBase64String(compressedMap);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        public static void LoadMap(string startpath, string mapName)
        {
            string folderPath = startpath + "/Content/Maps";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string path = folderPath + "/" + mapName + ".FarioMap";

            StreamReader reader = new StreamReader(path);
            string comMap = reader.ReadToEnd();
            reader.Close();
            string decomMap = DeCompress(comMap);
            string newPath = startpath + "/Content/Maps/" + mapName + ".temp";
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                sw.Write(decomMap);
            }
            reader = new StreamReader(newPath);
            int width, height;
            width = int.Parse(reader.ReadLine());
            height = int.Parse(reader.ReadLine());
            MapWidth = width;
            MapHeight = height;
            ResetMapSquares();
            ClearMap();

            string line;
            //loading the map            
            for (int l = 0; l < MapLayers; l++)
            {
                for (int i = 0; i < MapWidth; i++)
                {
                    line = reader.ReadLine();
                    string[] tiles = line.Split(',');
                    for (int j = 0; j < MapHeight; j++)
                    {
                        int index = int.Parse(tiles[j]);
                        mapCells[i, j].LayerTiles[l] = index;
                    }
                }
                reader.ReadLine();
                reader.ReadLine();
            }
            int type = int.Parse(reader.ReadLine());
            MapType = (MapType)type;

            //setting the ground
            while ((line = reader.ReadLine()) != "")
            {
                string[] tiles = line.Split(',');
                int i = int.Parse(tiles[0]);
                int j = int.Parse(tiles[1]);
                mapCells[i, j].TogglePassable();
            }

            specialTiles.Clear();
            //setting the codes
            while ((line = reader.ReadLine()) != null)
            {
                specialTiles.Add(line);
                string[] tiles = line.Split(',');
                int i = int.Parse(tiles[0]);
                int j = int.Parse(tiles[1]);
                mapCells[i, j].CodeValue = tiles[2];
            }
            reader.Close();
            reader.Dispose();
            File.Delete(newPath);
        }

        public static void LoadMap(string path)
        {
            StreamReader reader = new StreamReader(path);
            string comMap = reader.ReadToEnd();
            reader.Close();
            string decomMap = DeCompress(comMap);
            string newPath = path.Replace(".FarioMap", "temp");
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                sw.Write(decomMap);
            }
            reader = new StreamReader(newPath);
            int width, height;
            width = int.Parse(reader.ReadLine());
            height = int.Parse(reader.ReadLine());
            MapWidth = width;
            MapHeight = height;
            ResetMapSquares();
            ClearMap();

            string line;
            //loading the map            
            for (int l = 0; l < MapLayers; l++)
            {
                for (int i = 0; i < MapWidth; i++)
                {
                    line = reader.ReadLine();
                    string[] tiles = line.Split(',');
                    for (int j = 0; j < MapHeight; j++)
                    {
                        int index = int.Parse(tiles[j]);
                        mapCells[i, j].LayerTiles[l] = index;
                    }
                }
                reader.ReadLine();
                reader.ReadLine();
            }
            int type = int.Parse(reader.ReadLine());
            MapType = (MapType)type;

            //setting the ground
            while ((line = reader.ReadLine()) != "")
            {
                string[] tiles = line.Split(',');
                int i = int.Parse(tiles[0]);
                int j = int.Parse(tiles[1]);
                mapCells[i, j].TogglePassable();
            }

            //setting the codes
            while ((line = reader.ReadLine()) != null)
            {
                specialTiles.Add(line);
                string[] tiles = line.Split(',');
                int i = int.Parse(tiles[0]);
                int j = int.Parse(tiles[1]);
                mapCells[i, j].CodeValue = tiles[2];
            }
            reader.Close();
            reader.Dispose();
            File.Delete(newPath);
        }

        public static void ClearMap()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        mapCells[x, y] = new MapSquare(emptyTile, emptyTile, emptyTile, "", true);
                    }
                }
            }
        }

        #endregion

    }
}