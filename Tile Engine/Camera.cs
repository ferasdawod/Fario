using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Tile_Engine
{
    public static class Camera
    {
        //Declerations
        private static Vector2 position = Vector2.Zero;
        private static Vector2 viewPortSize = Vector2.Zero;

        private static Vector2 pos;
        private static float zoom;
        private static Matrix _transform;
        private static Matrix _inverseTransform;
        private static float _rotation;
        private static Viewport _viewport;
        private static MouseState _mState;
        private static KeyboardState _keyState;
        private static Int32 _scroll;


        public static float Zoom
        {
            get { return Camera.zoom; }
            set { Camera.zoom = value; }
        }

        public static Matrix Transform
        {
            get { return Camera._transform; }
            set { Camera._transform = value; }
        }

        public static Matrix InverseTransform
        {
            get { return Camera._inverseTransform; }
            set { Camera._inverseTransform = value; }
        }

        public static float Rotation
        {
            get { return Camera._rotation; }
            set { Camera._rotation = value; }
        }

        public static void Initialize(Viewport viewPort)
        {
            zoom = 1.1f;
            _scroll = 1;
            _rotation = 0.0f;
            _viewport = viewPort;
            pos = Vector2.Zero;
        }

        public static void Update()
        {
            Input();

            zoom = MathHelper.Clamp(zoom, 0.0f, 10.0f);

            _rotation = ClampAngle(_rotation);

            _transform =
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(pos.X, pos.Y, 0);

            _inverseTransform = Matrix.Invert(_transform);
        }

        private static void Input()
        {
            _mState = Mouse.GetState();
            _keyState = Keyboard.GetState();

            if (_mState.ScrollWheelValue > _scroll)
            {
                zoom += 0.1f;
                _scroll = _mState.ScrollWheelValue;
            }
            else if (_mState.ScrollWheelValue < _scroll)
            {
                zoom -= 0.1f;
                _scroll = _mState.ScrollWheelValue;
            }
            if (_keyState.IsKeyDown(Keys.Q))
            {
                _rotation -= 0.01f;
            }
            if (_keyState.IsKeyDown(Keys.E))
            {
                _rotation += 0.01f;
            }

            if (_keyState.IsKeyDown(Keys.D))
            {
                pos.X += 5f;
            }
            if (_keyState.IsKeyDown(Keys.A))
            {
                pos.X -= 5f;
            }
            if (_keyState.IsKeyDown(Keys.W))
            {
                pos.Y += 5f;
            }
            if (_keyState.IsKeyDown(Keys.S))
            {
                pos.Y -= 5f;
            }
        }
        private static float ClampAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        //Properties
        public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(
                    MathHelper.Clamp(value.X,
                    WorldRectangle.X,
                    WorldRectangle.Width - ViewPortWidth),
                    MathHelper.Clamp(value.Y,
                    WorldRectangle.Y,
                    WorldRectangle.Height - ViewPortHeight));
            }
        }

        public static Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle(
                  0,
                  0,
                  TileMap.TileWidth * TileMap.MapWidth,
                  TileMap.TileHeight * TileMap.MapHeight
                    );
            }
        }

        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        public static Rectangle ViewPort
        {
            get
            {
                return new Rectangle(
                    (int)Position.X, (int)Position.Y,
                ViewPortWidth, ViewPortHeight);
            }
        }

        //Functions
        public static void Move(Vector2 offset)
        {
            Position += offset;
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public static Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        public static Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(
            worldRectangle.Left - (int)position.X,
            worldRectangle.Top - (int)position.Y,
            worldRectangle.Width,
            worldRectangle.Height);
        }

        public static Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        public static Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(
            screenRectangle.Left + (int)position.X,
            screenRectangle.Top + (int)position.Y,
            screenRectangle.Width,
            screenRectangle.Height);
        }

    }
}
