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
using Tile_Engine;

namespace Level_Editor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        IntPtr drawSurface;
        MapEditor parentForm;
        System.Windows.Forms.PictureBox pictureBox;
        System.Windows.Forms.Control gameForm;

        public int DrawLayer = 0;
        public static int DrawTile = 1;
        public bool EditingCode = false;
        public string CurrentCodeValue = "";
        public string HoverCodeValue = "";
        Vector2 tileLocation;
        public Vector2 TileLocation
        {
            get { return tileLocation; }
        }

        public MouseState lastMouseState;
        System.Windows.Forms.VScrollBar vscroll;
        System.Windows.Forms.HScrollBar hscroll;

        public bool mapChanged = false;
        public bool shouldSet = true;

        public bool shouldUpdate = true;
        public bool shouldDraw = true;

        public List<SpecialTile> specialTiles = new List<SpecialTile>();

        public Game1(IntPtr drawSurface,
                    MapEditor parentForm,
                    System.Windows.Forms.PictureBox surfacePictureBox)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.drawSurface = drawSurface;
            this.parentForm = parentForm;
            this.pictureBox = surfacePictureBox;

            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(
                graphics_PreparingDeviceSettings);

            Mouse.WindowHandle = drawSurface;

            gameForm =
                System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            gameForm.VisibleChanged += new EventHandler(gameForm_VisibleChanged);
            pictureBox.SizeChanged += new EventHandler(pictureBox_SizeChanged);

            vscroll =
                (System.Windows.Forms.VScrollBar)parentForm.Controls["vScrollBar1"];
            hscroll =
                (System.Windows.Forms.HScrollBar)parentForm.Controls["hScrollBar1"];
        }

        void graphics_PreparingDeviceSettings(object sender,
            PreparingDeviceSettingsEventArgs e)
        {

            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;

        }

        private void gameForm_VisibleChanged(object sender, EventArgs e)
        {
            if (gameForm.Visible == true)
                gameForm.Visible = false;
        }

        void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (parentForm.WindowState !=
                System.Windows.Forms.FormWindowState.Minimized)
            {
                graphics.PreferredBackBufferWidth = pictureBox.Width;
                graphics.PreferredBackBufferHeight = pictureBox.Height;
                Camera.ViewPortWidth = pictureBox.Width;
                Camera.ViewPortHeight = pictureBox.Height;
                graphics.ApplyChanges();
            }
        }

        protected override void Initialize()
        {
            mapChanged = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera.ViewPortWidth = pictureBox.Width;
            Camera.ViewPortHeight = pictureBox.Height;
            Camera.Initialize(GraphicsDevice.Viewport);


            TileMap.Initialize(
                Content.Load<Texture2D>(@"Textures\Tileset"));
            TileMap.spriteFont =
                Content.Load<SpriteFont>(@"Fonts\Pericles8");

            lastMouseState = Mouse.GetState();

            pictureBox_SizeChanged(null, null);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!shouldDraw)
                SuppressDraw();

            if (shouldUpdate)
            {
                Camera.Position = new Vector2(hscroll.Value, vscroll.Value);
                Camera.Update();

                MouseState ms = Mouse.GetState();

                if ((ms.X > 0) && (ms.Y > 0) &&
                    (ms.X < Camera.ViewPortWidth) &&
                    (ms.Y < Camera.ViewPortHeight))
                {
                    Vector2 mouseLoc = Camera.ScreenToWorld(new Vector2(ms.X, ms.Y));

                    if (Camera.WorldRectangle.Contains(
                        (int)mouseLoc.X, (int)mouseLoc.Y))
                    {
                        if (shouldSet)
                        {
                            if (ms.LeftButton == ButtonState.Pressed)
                            {
                                if (!TileMap.IsTileSpecial(DrawTile))
                                {
                                    TileMap.SetTileAtCell(
                                      TileMap.GetCellByPixelX((int)mouseLoc.X),
                                      TileMap.GetCellByPixelY((int)mouseLoc.Y),
                                      DrawLayer,
                                      DrawTile);
                                    mapChanged = true;
                                }
                                else
                                {
                                    mapChanged = true;
                                    TileMap.GetMapSquarAtCell(
                                      TileMap.GetCellByPixelX((int)mouseLoc.X),
                                      TileMap.GetCellByPixelY((int)mouseLoc.Y)
                                    ).CodeValue = TileMap.GetTileName(DrawTile);

                                    specialTiles.Add(new SpecialTile(DrawTile, mouseLoc));
                                }
                            }

                            if ((ms.RightButton == ButtonState.Pressed))
                            {
                                if (EditingCode)
                                {
                                    TileMap.GetMapSquarAtCell(
                                      TileMap.GetCellByPixelX((int)mouseLoc.X),
                                      TileMap.GetCellByPixelY((int)mouseLoc.Y)
                                    ).CodeValue = CurrentCodeValue;
                                }
                                else
                                {
                                    TileMap.GetMapSquarAtCell(
                                      TileMap.GetCellByPixelX((int)mouseLoc.X),
                                      TileMap.GetCellByPixelY((int)mouseLoc.Y)
                                    ).Passable = parentForm.MakeGround;
                                }
                                mapChanged = true;
                            }
                        }

                        HoverCodeValue =
                                TileMap.GetMapSquarAtCell(
                                    TileMap.GetCellByPixelX(
                                        (int)mouseLoc.X),
                                    TileMap.GetCellByPixelY(
                                        (int)mouseLoc.Y)).CodeValue;
                        tileLocation = TileMap.GetCellByPixel(new Vector2((int)mouseLoc.X, (int)mouseLoc.Y));

                    }
                }


                lastMouseState = ms;
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (spriteBatch == null || spriteBatch.IsDisposed)
                spriteBatch = new SpriteBatch(GraphicsDevice);
            if (TileMap.TileSheet.IsDisposed)
                TileMap.TileSheet = Content.Load<Texture2D>(@"Textures/Tileset");

            spriteBatch.Begin();

            TileMap.Draw(spriteBatch);
            foreach (SpecialTile tile in specialTiles)
            {
                tile.Draw(spriteBatch);
            }

            if (spriteBatch == null || spriteBatch.IsDisposed)
                spriteBatch = new SpriteBatch(GraphicsDevice);
            if (TileMap.TileSheet.IsDisposed)
                TileMap.TileSheet = Content.Load<Texture2D>(@"Textures/Tileset");

            spriteBatch.End();

            base.Draw(gameTime);

        }

    }
}
